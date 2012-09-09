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

namespace PowerFocus
{
    public partial class Form1 : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PowerFocus.Form1));

        private TcpListener tcpListener;
        private Thread listenThread;
        private Boolean continueListen; 
        private Atc02Comm focuser;

        public Form1()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
            logger.Info("Power Focus. Welcome!!!");
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
                logger.Info("mensajeRecibido=" + mensajeRecibido);

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
                mensajeRespuesta = "" + this.focuser.ReadSettings();
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
            
        
    }
}
