using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Serduino;
using ZxRelay16;
using ASCOM.DriverAccess;
using ASCOM;
using log4net;
using log4net.Config;
using ASCOM.DeviceInterface;
using log4net.Appender;

namespace Montura
{
    public partial class Form1 : Form
    {
        public char[] query;
        public char[] pin7Low;
        private Boolean raLimit;
        private Boolean raLimitLast;

        private ArduinoTcp arduinoTcp;
        private Telescope telescopio;
        static Montura.Properties.Settings settings = Properties.Settings.Default;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));

        private int pierFlips;
        private PierSide pierSide;
        private PierSide pierSideLast;

        /// <summary>
        /// true si la montura del telescopio está en movimiento.
        /// </summary>
        private Boolean slewing;
        /// <summary>
        /// penultimo valor de la variable slewing
        /// </summary>
        private Boolean lastSlewing;


        private static readonly String cam = "http://HOST:80/decoder_control.cgi?command=CMD&user=admin&pwd=";

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Consturctor Start.");
            raLimit = false;
            raLimitLast = false;
            this.arduinoTcp = new ArduinoTcp(settings.ipAddress, (int)settings.port);
            query = new char[2];
            query[0] = '?';
            query[1] = '\n'; // revisar si este carcter se usa.
            pin7Low = new char[2];
            pin7Low[0] = '_';
            pin7Low[1] = '\n'; // revisar si este carcter se usa.
            InitializeComponent();


            pierFlips = 0;
            pierSide = PierSide.pierUnknown;
            pierSideLast = PierSide.pierUnknown;

            this.serialPortMontura.Open();
            this.timerReadSerial.Start();
            this.radioButtonDecHome.Checked = false;
            this.radioButtonRA_East.Checked = false;
            this.radioButtonRA_Home.Checked = false;
            this.radioButtonRA_West.Checked = false;
            telescopio = new Telescope("AstroPhysicsV2.Telescope");
            logger.Info("Consturctor End.");
        }

        /// <summary>
        /// Avisa al Usuario que la montura ha sido desconectada
        /// por los microcontroladores Arduino.
        /// </summary>
        private void notificarMontura(Status stat)
        {
            timerReadSerial.Stop();
            logger.Info("notificarMontura");
            this.BringToFront();
            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.Append("La Montura ha alcanzado el limite ");
            if (stat.RaLimitEast)
            {
                mensaje.Append("East");
            }
            if (stat.RaLimitWest)
            {
                mensaje.Append("West");
            }
            mensaje.Append(" en RA.\n\n");

            this.arduinoTcp.Connect();
            if (this.arduinoTcp.Tcpclnt.Connected)
            {
                this.arduinoTcp.readRelays();
                System.Threading.Thread.Sleep(250);

                this.arduinoTcp.readRelays();
                if (this.arduinoTcp.RelayStatus[2] == false)
                {
                    mensaje.Append("La montura esta apagada.\n\n");
                }
                else
                {
                    mensaje.Append("ERROR!!!, la montura sigue encendida!!!.\n\n");
                }
                System.Threading.Thread.Sleep(200);

                if (this.arduinoTcp.Tcpclnt.Connected)
                {
                    this.arduinoTcp.Tcpclnt.Close();
                }
            }

            mensaje.Append("Procedimiento:\n");
            mensaje.Append("1) Con aplicación PDU_chase, encienda la montura.\n");
            mensaje.Append("2) Con alguna guitarra virtual (o real), desplaze la montura hacia el ");
            if (stat.RaLimitEast)
            {
                mensaje.Append("WEST");
            }
            if (stat.RaLimitWest)
            {
                mensaje.Append("EAST");
            }
            mensaje.Append(", hasta Park 3");
            DialogResult dr;
            logger.Info(mensaje.ToString());
            dr = MessageBox.Show(this, mensaje.ToString(), "RA Limit alert.");
            if (dr.Equals(DialogResult.OK))
            {
                timerReadSerial.Start();
            }
        }

        /// <summary>
        /// Cambia el BackColor de un RadioButton para entregar feedback al usuario
        /// </summary>
        /// <param name="ratioButton"></param>
        /// <param name="discriminante">variable para seleccionar el color.</param>
        /// <param name="ColorTrue">Color a usar cuando discriminante=true.</param>
        /// <param name="ColorFalse">Color a usar cuando discriminante=false.</param>
        private void RefreshColor(RadioButton ratioButton, Boolean discriminante, Color ColorTrue, Color ColorFalse)
        {
            if (discriminante)
            {
                ratioButton.BackColor = ColorTrue;
            }
            else
            {
                ratioButton.BackColor = ColorFalse;
            }
            ratioButton.Checked = discriminante;
        }

        /// <summary>
        /// Consulta al arduino de los limit Switches por su estado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerReadSerial_Tick(object sender, EventArgs e)
        {
            String arduinoStatus;
            arduinoStatus = "Tick";
            serialPortMontura.Write(query, 0, 1);
            arduinoStatus = serialPortMontura.ReadLine();

            Console.WriteLine(arduinoStatus);
            Status stat;
            stat = new Serduino.Status(arduinoStatus);
            stat.Analiza();

            RefreshColor(this.radioButtonRA_East, stat.RaLimitEast, Color.Pink, Color.LightYellow);
            RefreshColor(this.radioButtonRA_West, stat.RaLimitWest, Color.Pink, Color.LightYellow);
            RefreshColor(this.radioButtonRA_Home, stat.RaHome, Color.LightGreen, Color.LightYellow);
            RefreshColor(this.radioButtonDecHome, stat.DecHome, Color.LightGreen, Color.LightYellow);
            if ((slewing) && (stat.RaHome))
            {
                logger.Info("Cruzando RaHome.");
            }
            if ((slewing) && (stat.DecHome))
            {
                logger.Info("Cruzando DecHome.");
            }

            raLimit = ((stat.RaLimitEast) || (stat.RaLimitWest));
            if ((raLimit) && (!raLimitLast))
            {
                logger.Info("RaLimitEast=" + stat.RaLimitEast);
                logger.Info("RaLimitWest=" + stat.RaLimitWest);
                //Flanco de subida de la alerta
                this.notificarMontura(stat);
            }
            if ((!raLimit) && (raLimitLast))
            {
                this.pin7_Low();
            }
            raLimitLast = raLimit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ConfirmarClose();
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
            }
        }

        private DialogResult ConfirmarClose()
        {
            DialogResult respuesta;
            respuesta = MessageBox.Show("Closing application. Continue?", "Accelerometers Airbag", MessageBoxButtons.YesNo);
            if (respuesta == DialogResult.Yes)
            {
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

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }


        /// <summary>
        /// Enciende/Apaga los leds infrarojos de las camara IP.
        /// </summary>
        /// <param name="Encender">true->Endender</param>
        private void InfraredControl(bool Encender)
        {
            logger.Info("InfraredControl, encender=" + Encender);
            String url1, url2;
            url1 = cam.Replace("HOST", "139.229.12.82");
            url2 = cam.Replace("HOST", "139.229.12.83");
            String comando;
            if (Encender)
            {
                comando = "95";
            }
            else
            {
                comando = "94";
            }
            url1 = url1.Replace("CMD", comando);
            url2 = url2.Replace("CMD", comando);
            String resp1, resp2;
            resp1 = Http.GetUrl(url1);
            resp2 = Http.GetUrl(url2);
            Console.WriteLine("" + url1 + " ---> " + resp1);
            Console.WriteLine("" + url2 + " ---> " + resp2);
        }

        /// <summary>
        /// Lee el estado del telescopio desde los drivers de ASCOM.
        /// </summary>
        private void ProcesaTelescopio()
        {
            StringBuilder mensaje;
            mensaje = new StringBuilder();
            Boolean conectado;
            conectado = false;
            try
            {
                conectado = telescopio.Connected;
            }
            catch (DriverAccessCOMException e)
            {
                conectado = false;
                logger.Debug(e.Message);
            }
            if (conectado)
            {
                mensaje.Append("Conectado,");
                double ra, dec;
                try
                {
                    ra = telescopio.RightAscension;
                    dec = telescopio.Declination;
                }
                catch (DriverAccessCOMException e)
                {
                    ra = dec = 0;
                    logger.Debug(e.Message);
                }


                slewing = false;
                try
                {
                    slewing = telescopio.Slewing;
                }
                catch (DriverAccessCOMException e)
                {
                    slewing = false;
                    logger.Debug(e.Message);
                }

                if ((this.checkBoxInfrared.Checked) && (slewing != lastSlewing))
                {
                    this.InfraredControl(slewing);
                }

                if (slewing)
                {
                    mensaje.Append("Slewing,");
                }
                else
                {
                    mensaje.Append("Stationary,");
                }

                #region PierSide
                mensaje.Append("PierSide=");
                this.pierSide = telescopio.SideOfPier;
                //if (pierFlips == 0)
                //{
                //    this.pierSideLast = pierSide;
                //}
                String strPierSide;
                strPierSide = "_undef";
                switch (this.pierSide)
                {
                    case (PierSide.pierEast):
                        strPierSide = "East";
                        break;
                    case PierSide.pierUnknown:
                        strPierSide = "Unknown";
                        break;
                    case PierSide.pierWest:
                        strPierSide = "West";
                        break;
                }
                mensaje.Append(strPierSide);
                // SI:
                // 1) Si los ultimos dos estados estan bien determinados.
                // 2) Ambos estados son diferentes
                // ENtonces:
                // Se ha detectado un  Trasito de la montura, o un GEM Flip.

                if ((pierSide != PierSide.pierUnknown) &&  //1)
                     (pierSideLast != PierSide.pierUnknown) && //1)
                     (pierSide != pierSideLast))
                {
                    pierFlips++;
                    logger.Info("#Flips=" + pierFlips + ". Actual pierside=" + strPierSide);
                }
                mensaje.Append("#Flips=");
                mensaje.Append(pierFlips);

                pierSideLast = pierSide;
                #endregion
                //mensaje.Append(" RA=");
                //mensaje.Append(String.Format("{0:0.00}", ra));
                //mensaje.Append(" DEC=");
                //mensaje.Append(String.Format("{0:0.00}", dec));
                lastSlewing = slewing;
            }
            else
            {
                mensaje.Append("Desconectado");
            }
            this.labelTelescope.Text = mensaje.ToString();
            //this.timerTelescopio.Start();
        }

        private void timerTelescope_Tick(object sender, EventArgs e)
        {
        }

        private void checkBoxInfrarojos_CheckedChanged(object sender, EventArgs e)
        {
            this.InfraredControl(this.checkBoxInfrarojos.Checked);
        }

        private void timerTelescopio_Tick(object sender, EventArgs e)
        {
            this.ProcesaTelescopio();
        }

        /// <summary>
        /// Envia una señal al arduino de los limits para que
        /// baje el pin7 a un estado logico Cero==GND
        /// </summary>
        private void pin7_Low()
        {
            this.timerReadSerial.Stop();
            logger.Info("buttonPin7Low_Click.");
            String respuesta;
            respuesta = "Tick";
            serialPortMontura.Write(pin7Low, 0, 1);
            respuesta = serialPortMontura.ReadLine();
            logger.Info("respuesta=" + respuesta);
            //Console.WriteLine(respuesta);
            DialogResult dr;
            dr = MessageBox.Show("Al presionar aceptar, La montura estará nuevamente protegida", "Pin7 Low");
            if (dr.Equals(DialogResult.OK))
            {
                this.timerReadSerial.Start();
            }
        }

        private void buttonPin7Low_Click(object sender, EventArgs e)
        {
            pin7_Low();
        }

        private void buttonShowLog_Click(object sender, EventArgs e)
        {
            RollingFileAppender rootAppender = (RollingFileAppender)((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Appenders[0];
            string filename = rootAppender.File;
            Console.WriteLine("filename=" + filename);
            OpenLink(filename);
        }

        public void OpenLink(string sUrl)
        {
            try
            {
                System.Diagnostics.Process.Start(sUrl);
            }
            catch (Exception exc1)
            {
                // System.ComponentModel.Win32Exception is a known exception that occurs when Firefox is default browser.  
                // It actually opens the browser but STILL throws this exception so we can just ignore it.  If not this exception,
                // then attempt to open the URL in IE instead.
                if (exc1.GetType().ToString() != "System.ComponentModel.Win32Exception")
                {
                    // sometimes throws exception so we have to just ignore
                    // this is a common .NET bug that no one online really has a great reason for so now we just need to try to open
                    // the URL using IE if we can.
                    try
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("IExplore.exe", sUrl);
                        System.Diagnostics.Process.Start(startInfo);
                        startInfo = null;
                    }
                    catch (Exception exc2)
                    {
                        // still nothing we can do so just show the error to the user here.
                    }
                }
            }
        }

        private void buttonFlipCountReset_Click(object sender, EventArgs e)
        {
            this.pierFlips = 0;
            logger.Info("Reseteando a: pierFlips=0");
        }
    }
}
