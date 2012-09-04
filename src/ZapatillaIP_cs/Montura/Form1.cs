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
using System.Net.Sockets;

namespace Montura
{
    public partial class Form1 : Form
    {
        public char[] query;
        public char[] pin7Low;

        /// <summary>
        /// true si se ha alcanzado un limite en Ascencion recta.
        /// </summary>
        private Boolean raLimit;

        /// <summary>
        /// Valor de raLimit en la medición anterior.
        /// </summary>
        private Boolean raLimitLast;

        /// <summary>
        /// true si se ha alcanzado un limite en el angulo Zenital.
        /// </summary>
        private Boolean tiltLimit;

        /// <summary>
        /// Valor de tiltLimit en la medición anterior.
        /// </summary>
        private Boolean tiltLimitLast;

        //private ArduinoTcp arduinoTcp;
        private Telescope telescopio;
        static Montura.Properties.Settings settings = Properties.Settings.Default;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));

        private int pierFlips;
        private PierSide pierSide;
        private PierSide pierSideLast;
        private Status stat;
        private UdpClient udpClient;

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
            logger.Info("Constructor Start.");
            raLimit = false;
            raLimitLast = false;
            tiltLimit = false;
            tiltLimitLast = false;
            this.udpClient = new UdpClient();
            //this.arduinoTcp = new ArduinoTcp(settings.ipAddress, (int)settings.port);
            query = new char[2];
            query[0] = '?';
            query[1] = '\n'; // revisar si este carcter se usa.
            pin7Low = new char[2];
            pin7Low[0] = '_';
            pin7Low[1] = '\n'; // revisar si este carcter se usa.
            InitializeComponent();

            stat = null;
            pierFlips = 0;
            pierSide = PierSide.pierUnknown;
            pierSideLast = PierSide.pierUnknown;

            this.arduinoLimits.Open();
            this.timerReadSerial.Start();
            this.radioButtonDecHome.Checked = false;
            this.radioButtonRA_East.Checked = false;
            this.radioButtonRA_Home.Checked = false;
            this.radioButtonRA_West.Checked = false;

            //try
            //{
            //    this.telescopio = new Telescope(settings.TelescopeProgId);
            //}
            //catch (Exception)
            //{
            //    this.telescopio = null;
            //    logger.Error("Error al escoger telescopio ASCOM.");
            //}
            logger.Info("Constructor End.");
        }

        /// <summary>
        /// Avisa al Usuario que la montura ha sido desconectada
        /// por los microcontroladores Arduino.
        /// Razón, se sobrepasó el limite para el angulo Zenital.
        /// </summary>
        private void notificarMonturaZenith()
        {
            timerReadSerial.Stop();
            logger.Info("notificarMontura");
            this.BringToFront();
            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.Append("La Montura ha alcanzado el limite del ángulo Zenital.\n\n");

            if (!this.stat.MonturaEncendida)
            {
                mensaje.Append("La montura está apagada.\n\n");
            }
            else
            {
                mensaje.Append("ERROR!!!, la montura sigue encendida!!!.\n\n");
            }

            mensaje.AppendLine("Procedimiento:");
            mensaje.AppendLine("1) Con aplicación PDU_chase, encienda la montura.");
            mensaje.AppendLine("2) Con alguna guitarra virtual (o real), desplaze la montura hacia una posición segura.");
            DialogResult dr;
            logger.Info(mensaje.ToString());
            dr = MessageBox.Show(this, mensaje.ToString(), "Zenith Angle Limit alert.");
            if (dr.Equals(DialogResult.OK))
            {
                timerReadSerial.Start();
            }
        }

        /// <summary>
        /// Avisa al Usuario que la montura ha sido desconectada
        /// por los microcontroladores Arduino. Razon, Se alcanzo limit switch en RA.
        /// </summary>
        private void notificarMonturaRA()
        {
            timerReadSerial.Stop();
            logger.Info("notificarMontura");
            this.BringToFront();
            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.Append("La Montura ha alcanzado el limite ");
            if (this.stat.RaLimitEast)
            {
                mensaje.Append("East");
            }
            if (this.stat.RaLimitWest)
            {
                mensaje.Append("West");
            }
            mensaje.Append(" en RA.\n\n");

            if (!this.stat.MonturaEncendida)
            {
                mensaje.Append("La montura está apagada.\n\n");
            }
            else
            {
                mensaje.Append("ERROR!!!, la montura sigue encendida!!!.\n\n");
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
        private void RefreshRatioButtonColor(RadioButton ratioButton, Boolean discriminante, Color ColorTrue, Color ColorFalse)
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

        private void RefreshCheckBoxColor(CheckBox checkBox, Boolean discriminante, Color ColorTrue, Color ColorFalse)
        {
            if (discriminante)
            {
                checkBox.BackColor = ColorTrue;
            }
            else
            {
                checkBox.BackColor = ColorFalse;
            }
            checkBox.Checked = discriminante;
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
            arduinoLimits.Write(query, 0, 1);
            arduinoStatus = arduinoLimits.ReadLine();

            //Console.WriteLine(arduinoStatus);
            stat = new Serduino.Status(arduinoStatus);
            stat.Analiza();
            RefreshCheckBoxColor(this.cbMonturaEncendida, stat.MonturaEncendida, Color.LightGreen, Color.LightPink);
            RefreshCheckBoxColor(this.cbMonturaProtegida, stat.MonturaProtegida, Color.LightGreen, Color.LightYellow);

            RefreshRatioButtonColor(this.radioButtonRA_East, stat.RaLimitEast, Color.Pink, Color.LightYellow);
            RefreshRatioButtonColor(this.radioButtonRA_West, stat.RaLimitWest, Color.Pink, Color.LightYellow);
            RefreshRatioButtonColor(this.radioButtonRA_Home, stat.RaHome, Color.LightGreen, Color.LightYellow);
            RefreshRatioButtonColor(this.radioButtonDecHome, stat.DecHome, Color.LightGreen, Color.LightYellow);
            if (!stat.MonturaProtegida)
            {
                if (this.stat.MonturaEncendida) {this.gbMontura.BackColor=Color.LightYellow;}
                else { this.gbMontura.BackColor = Color.LightPink; }
            }else
            { this.gbMontura.BackColor = Color.LightGreen; }
            if ((slewing) && (stat.RaHome))
            {
                logger.Info("Cruzando RaHome.");
            }
            if ((slewing) && (stat.DecHome))
            {
                logger.Info("Cruzando DecHome.");
            }

            this.raLimit = ((stat.RaLimitEast) || (stat.RaLimitWest));
            if ((raLimit) && (!raLimitLast))
            {
                logger.Info("RaLimitEast=" + stat.RaLimitEast);
                logger.Info("RaLimitWest=" + stat.RaLimitWest);
                //Flanco de subida de la alerta
                this.notificarMonturaRA();
            }
            //if ((!raLimit) && (raLimitLast))
            //{
            //    this.protejeMontura();
            //}

            this.tiltLimit = (this.stat.ZenithCounter != 0);
            if ((tiltLimit) && (!tiltLimitLast))
            {
                logger.Info("Tilt Limit: ZenithAngle=" + stat.ZenithAngleArduino);
                this.notificarMonturaZenith();
            }
            //if ((!tiltLimit) && (tiltLimitLast))
            //{
            //    this.protejeMontura();
            //}
            raLimitLast = raLimit;
            tiltLimitLast = tiltLimit;
            ///////////////////
            StringBuilder mensajeAcelerations;
            StringBuilder mensajeAngles;

            mensajeAcelerations = new StringBuilder();
            mensajeAcelerations.Append("RA="); mensajeAcelerations.Append(stat.AcelerometroRA.AcelerationUnit.ToString());
            mensajeAcelerations.Append("DEC="); mensajeAcelerations.Append(stat.AcelerometroDEC.AcelerationUnit.ToString());
            //mensajeConsola.Append("cwa[°]="); mensajeConsola.Append(stat.CounterWeightAngle);
            this.labelRaDec.Text = mensajeAcelerations.ToString();
            mensajeAngles = new StringBuilder();
            mensajeAngles.Append("[°] cwA="); mensajeAngles.Append(stat.CounterWeightAngle.ToString("0.00"));
            mensajeAngles.Append("\t d="); mensajeAngles.Append(stat.DeclinationAngle.ToString("0.00"));
            mensajeAngles.Append("\t z="); mensajeAngles.Append(stat.ZenithAngle.ToString("0.00"));
            this.lblAlt.Text = (mensajeAngles.ToString());
            //Console.WriteLine(mensaje.ToString());
            Byte[] sendBytes;
            sendBytes = Encoding.ASCII.GetBytes("#" + stat.CounterWeightAngle + " " + stat.DeclinationAngle + " " + stat.ZenithAngle + "#");
            Console.WriteLine(mensajeAcelerations.ToString());
            this.udpClient.Connect(settings.UdpServerHost, settings.UdpServerPort);
            udpClient.Send(sendBytes, sendBytes.Length);
            ///////////////////
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
                //this.Hide();
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
                this.udpClient.Close();
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
                if (this.telescopio != null)
                {
                    try
                    {
                        conectado = telescopio.Connected;
                    }
                    catch (NullReferenceException nref)
                    {
                        logger.Debug(nref.Message);
                        conectado = false;
                    }
                }
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
                mensaje.Append("Telescope ProdID=");
                mensaje.Append(settings.TelescopeProgId);
                mensaje.Append(" ;; Desconectado");
            }
            this.labelTelescope.Text = mensaje.ToString();
            //this.timerTelescopio.Start();
        }

        private void timerTelescope_Tick(object sender, EventArgs e)
        {
        }

        private void checkBoxInfrarojos_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfraredControl(this.checkBoxInfrarojos.Checked);
            this.Cursor = Cursors.Default;
        }

        private void timerTelescopio_Tick(object sender, EventArgs e)
        {
            if (this.telescopio != null)
            {
                this.ProcesaTelescopio();
            }
        }

        /// <summary>
        /// Envia una señal al arduino de los limits para que
        /// baje el pin7 a un estado logico Cero==GND
        /// </summary>
        private void protejeMontura()
        {
            this.timerReadSerial.Stop();
            logger.Info("buttonPin7Low_Click.");
            String respuesta;
            respuesta = "Tick";
            arduinoLimits.Write(pin7Low, 0, 1);
            respuesta = arduinoLimits.ReadLine();
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
            protejeMontura();
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
                        logger.Error(exc2.Message);
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

        private void bSelect_Click(object sender, EventArgs e)
        {
            ASCOM.Utilities.Chooser selector;
            selector = new ASCOM.Utilities.Chooser();
            selector.DeviceType = "Telescope";
            settings.TelescopeProgId = selector.Choose(settings.TelescopeProgId);
            settings.Save();
        }

        private void bSetup_Click(object sender, EventArgs e)
        {
            logger.Info("bSetup_Click");
            if (this.telescopio == null)
            {
                logger.Info("this.telescopio == null");
                nuevoTelescopio();
            }
            //if (!this.telescopio.Connected)
            //{
            //    logger.Info("Conectando Telescopio");
            //    this.telescopio.Connected = true;                
            //}
            this.telescopio.SetupDialog();
        }

        private void nuevoTelescopio()
        {
            logger.Info("nuevoTelescopio:" + settings.TelescopeProgId);
            this.telescopio = new Telescope(settings.TelescopeProgId);
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.telescopio == null)
            {
                nuevoTelescopio();
            }
            if (bConnect.Text == "Connect")
            {
                nuevoTelescopio();
                try
                {
                    this.telescopio.Connected = true;
                }
                catch (DriverAccessCOMException exc)
                {
                    logger.Error(exc.Message);
                }
                bConnect.Text = "Disconnect";
                this.timerTelescopio.Enabled = true;
            }
            else
            {
                try
                {
                    this.telescopio.Connected = false;
                    this.telescopio.Dispose();
                }
                catch (DriverAccessCOMException exc)
                {
                    logger.Error(exc.Message);
                }

                bConnect.Text = "Connect";
                this.timerTelescopio.Enabled = false;
            }
            this.Cursor = Cursors.Default;
        }
    }
}
