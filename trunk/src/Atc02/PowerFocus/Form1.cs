using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using log4net;
using log4net.Config;
using log4net.Appender;
using AtcXml;

namespace PowerFocus
{
    public partial class Form1 : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PowerFocus.Form1));

        private TcpListener tcpListener;
        private Thread listenThread;
        private Boolean continueListen; 
        private Atc02Comm focuser;
        private Atc02Xml atc02Status;


        public Form1()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
            logger.Info("Power Focus. Welcome!!!");
        }


        /// <summary>
        /// http://www.switchonthecode.com/tutorials/csharp-tutorial-simple-threaded-tcp-server
        /// </summary>
        private void ListenForClients()
        {

            this.tcpListener.Start();

            while (this.continueListen)
            {
                
                //blocks until a client has connected to the server
                TcpClient client;
                client = null;
                try
                {
                    client = this.tcpListener.AcceptTcpClient();
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                if (client != null)
                {
                    //create a thread to handle communication 
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }
                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }
                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                String mensajeRecibido;
                String mensajeRespuesta;
                mensajeRecibido = encoder.GetString(message, 0, bytesRead);
                logger.Debug("mensajeRecibido=" + mensajeRecibido);

                #region respuesta al cliente
                mensajeRespuesta = procesaMensaje(mensajeRecibido);
                byte[] buffer = encoder.GetBytes(mensajeRespuesta);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                #endregion
            }
            tcpClient.Close();
        }

        private String procesaMensaje(String mensajeRecibido)
        {
            String mensajeRespuesta;             
            mensajeRespuesta = "no implementado.";
            if (mensajeRecibido.Equals("conectar"))
            {
                this.focuser.conectar();
                mensajeRespuesta = ("" + this.focuser.Conectado);
            }

            if (mensajeRecibido.Equals("desconectar"))
            {
                this.focuser.desConectar();
                mensajeRespuesta = ("" + this.focuser.Conectado);
            }

            if (mensajeRecibido.StartsWith("move"))
            {
                String[] part;
                part = mensajeRecibido.Split( (" ").ToCharArray() );
                int posicion;
                posicion = Int32.Parse(part[1]);
                this.focuser.Move(posicion);
                mensajeRespuesta = (""+this.focuser.Posicion);
            }

            if (mensajeRecibido.Equals("GetXmlStatus"))
            {
                mensajeRespuesta = this.focuser.AtcStat.ToString();
            }

            if (mensajeRecibido.Equals("GetPosition"))
            {
                mensajeRespuesta = ""+this.focuser.Posicion;
            }

            if (mensajeRecibido.Equals("GetAmbientTemperature"))
            {
                mensajeRespuesta = ""+this.focuser.AtcStat.AmbientTemperature;
            }
            
            if (mensajeRecibido.Equals("IsMoving"))
            {
                mensajeRespuesta = "" + this.focuser.EnMovimiento;
            }

            if (mensajeRecibido.Equals("IsConnected"))
            {
                mensajeRespuesta = "" + this.focuser.Conectado;
            }

            if (mensajeRecibido.Equals("ReadSettings"))
            {
                String fwSettings;
                fwSettings = this.focuser.FirmwareSettings;
                if (fwSettings == null)
                {
                    fwSettings = "waiting to read settings";
                }else{
                    fwSettings = fwSettings.Replace("\r\n",";").Replace("\0","");
                }

                mensajeRespuesta = fwSettings;
            }
            if (mensajeRecibido.Equals("FindOptimal"))
            {
                mensajeRespuesta = "" + this.focuser.FindOptimal();
            }
            
            if (mensajeRecibido.StartsWith("SetFan"))
            {
                String[] part;
                part = mensajeRecibido.Split((" ").ToCharArray());
                int fanspeed;
                fanspeed = Int32.Parse(part[1]);

                mensajeRespuesta = this.focuser.SetFan(fanspeed);
            }
            return mensajeRespuesta;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                return;
            }
            DialogResult respuesta;
            respuesta = this.ConfirmarClose();
            if (respuesta == DialogResult.No)
            {

                e.Cancel = true;
                this.WindowState = FormWindowState.Normal;
                return;
            }

            this.timerStatus.Stop();
            this.continueListen = false;
            this.tcpListener.Stop();
            while (this.listenThread.IsAlive)
            {
                Thread.Sleep(500);
                logger.Info("Cerrando listener.");
            }
        }

        PowerFocus.Properties.Settings settings = PowerFocus.Properties.Settings.Default;

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.focuser = Atc02Comm.Instance;
            this.tcpListener = new TcpListener(IPAddress.Any, settings.FocusPort);
            this.continueListen = true;
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
            this.focuser.conectar();
            this.timerStatus.Interval = 1000 * ((int)settings.refreshStatusTimer);
            this.timerStatus.Start();
            this.timerSetFan.Interval = 1000 * settings.setFanTimer;
            this.timerSetFan.Start();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ConfirmarClose();
        }

        private DialogResult ConfirmarClose()
        {
            DialogResult respuesta;
            respuesta = MessageBox.Show("Closing application. Continue?", "Focus Server", MessageBoxButtons.YesNo);
            if (respuesta == DialogResult.Yes)
            {
                this.focuser.desConectar();
                Application.Exit();
            }
            return respuesta;
        }

        /// <summary>
        /// Muestra la ventana de esta aplicacion.
        /// </summary>
        private void MostrarVentana()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        /// <summary>
        /// Envia el comando "GetXmlStatus" al driver ATC02 de Orbit.
        /// y actualiza la estructura de datos atc02Status
        /// </summary>
        private void refreshATC02XmlStatus()
        {
            String xmlStatus;
            xmlStatus = this.focuser.AtcStat.ToString();
            Console.WriteLine("xmlStatus=");
            Console.WriteLine(xmlStatus);
            this.atc02Status = new Atc02Xml(xmlStatus);

            if (this.atc02Status.IsFresh())
            {
                this.BackColor = Color.LightGreen;
                this.Text = settings.AppVersion+", ATC02 ok";
            }
            else
            {
                logger.Warn("ATC02 log outdated");
                this.BackColor = Color.Pink;
                this.Text = settings.AppVersion + ", check ATC02 (power/driver/log)";
            }
            this.analizaATC02();
        }

        private void analizaATC02()
        {
            String[] fila;
            this.dataGridViewATC02.Rows.Clear();

            fila = new String[2];
            fila[0] = "Ultima respuesta";
            StringBuilder textoFecha;
            textoFecha = new StringBuilder();


            textoFecha.Append(this.atc02Status.Timestamp.ToLongDateString());
            textoFecha.Append(" ");
            textoFecha.Append(this.atc02Status.Timestamp.ToShortTimeString());

            Console.WriteLine("timestamp=" + textoFecha.ToString());
            fila[1] = textoFecha.ToString();
            agregaFila(fila);

            fila[0] = "SETFAN";
            fila[1] = this.atc02Status.FanPower.ToString();
            agregaFila(fila);

            fila[0] = "PRITE";
            fila[1] = this.atc02Status.PrimaryTemperature.ToString();
            agregaFila(fila);

            fila[0] = "SECTE";
            fila[1] = this.atc02Status.SecondaryTemperature.ToString();
            agregaFila(fila);

            fila[0] = "BFL";
            fila[1] = this.atc02Status.FocusPosition.ToString();
            agregaFila(fila);

            fila[0] = "FOCSTEP";
            fila[1] = Atc02Xml.BflToFocSetp(this.atc02Status.FocusPosition).ToString();
            agregaFila(fila);

            fila[0] = "AMBTE";
            fila[1] = this.atc02Status.AmbientTemperature.ToString();
            agregaFila(fila);
        }

        private void agregaFila(String[] fila)
        {
            if ((fila[0] != null) && (fila[0].Length > 0))
            {
                try
                {
                    this.dataGridViewATC02.Rows.Add(fila);
                }
                catch (InvalidOperationException ioe)
                {
                    logger.Error(ioe.Message);
                    logger.Error(fila.ToString());
                }
            }
        }

        private void bReadStatus_Click(object sender, EventArgs e)
        {
            this.refreshATC02XmlStatus();
        }

        private void bSetFan_Click(object sender, EventArgs e)
        {
            int fanSpeed;
            fanSpeed = (int) this.trackBarFan.Value;
            this.focuser.SetFan(fanSpeed);
        }

        private void trackBarFan_Scroll(object sender, EventArgs e)
        {
            int fanSpeed;
            fanSpeed = (int)this.trackBarFan.Value;
            this.bSetFan.Text = ("Set Fan "+fanSpeed);
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            this.refreshATC02XmlStatus();            
        }

        private void bFindOptimal_Click(object sender, EventArgs e)
        {
            this.focuser.FindOptimal();
        }

        private void timerSetFan_Tick(object sender, EventArgs e)
        
        {
            Boolean condicion;
            condicion = (   (this.atc02Status.IsFresh()) && 
                            (this.atc02Status.PrimaryTemperature > this.atc02Status.SecondaryTemperature) &&
                            (this.atc02Status.SecondaryTemperature >= this.atc02Status.AmbientTemperature)
                         );

            if (condicion)
            {
                if (this.atc02Status.FanPower!=100)
                {
                    this.focuser.SetFan(100);
                }
            }
            else
            {
                if (this.atc02Status.FanPower != 0)
                {
                    this.focuser.SetFan(0);
                }
            }
        }
        
    }
}
