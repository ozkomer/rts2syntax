﻿using System;
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

namespace Montura
{
    public partial class Form1 : Form
    {
        public char[] query;
        private ArduinoTcp arduinoTcp;
        private Telescope telescopio;
        static Montura.Properties.Settings settings = Properties.Settings.Default;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Start");
            this.arduinoTcp = new ArduinoTcp(settings.ipAddress, (int)settings.port);
            query = new char[2];
            query[0] = '?';
            query[1] = '\n';
            InitializeComponent();

            this.serialPortMontura.Open();
            this.timerReadSerial.Start();
            this.radioButtonDecHome.Checked = false;
            this.radioButtonRA_East.Checked = false;
            this.radioButtonRA_Home.Checked = false;
            this.radioButtonRA_West.Checked = false;
            telescopio = new Telescope("AstroPhysicsV2.Telescope");
            logger.Info("End");
        }

        private void ApagarMontura()
        {
            logger.Info("Apagando Montura");
            this.BringToFront();
            this.arduinoTcp.Connect();
            if (this.arduinoTcp.Tcpclnt.Connected)
            {
                this.arduinoTcp.readRelays(); // this.readRelays();
                System.Threading.Thread.Sleep(250);

                //for (int i = 0; i < 16; i++)
                //{
                //    Console.Write("\t" + this.arduinoTcp.RelayStatus[i]);
                //}
                //Console.WriteLine("  ...");
                //Solo modificamos el relay correspondiente a la montura
                this.arduinoTcp.RelayStatus[2] = false;//debe ser 2
                this.arduinoTcp.refreshPorts();
                System.Threading.Thread.Sleep(200);

                //this.arduinoTcp.readRelays();
                //System.Threading.Thread.Sleep(200);

                //for (int i = 0; i < 16; i++)
                //{
                //    Console.Write("\t" + this.arduinoTcp.RelayStatus[i]);
                //}
                //Console.WriteLine("  ...");

                if (this.arduinoTcp.Tcpclnt.Connected)
                {
                    this.arduinoTcp.Tcpclnt.Close();
                }
            }
        }

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

        private void timerReadSerial_Tick(object sender, EventArgs e)
        {
            String respuesta;
            respuesta = "Tick";
            serialPortMontura.Write(query, 0, 1);
            respuesta = serialPortMontura.ReadLine();

            //Console.WriteLine(respuesta);
            Status stat;
            stat = new Serduino.Status(respuesta);
            stat.Analiza();

            RefreshColor(this.radioButtonRA_East, stat.RaLimitEast, Color.Pink, Color.LightYellow);
            RefreshColor(this.radioButtonRA_West, stat.RaLimitWest, Color.Pink, Color.LightYellow);
            RefreshColor(this.radioButtonRA_Home, stat.RaHome, Color.LightGreen, Color.LightYellow);
            RefreshColor(this.radioButtonDecHome, stat.DecHome, Color.LightGreen, Color.LightYellow);
            if (stat.RaHome)
            {
                logger.Info("Cruzando RaHome.");
            }
            if (stat.DecHome)
            {
                logger.Info("Cruzando DecHome.");
            }
            if ((!this.buttonContinue.Enabled) && ((stat.RaLimitEast) || (stat.RaLimitWest)))
            {
                this.buttonContinue.Enabled = true;
                logger.Info("RaLimitEast=" + stat.RaLimitEast);
                logger.Info("RaLimitWest=" + stat.RaLimitWest);
                this.ApagarMontura();
            }

            //this.ProcesaTelescopio();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            logger.Info("buttonContinue_Click, Rehabilitando Airbag.");
            buttonContinue.Enabled = false;
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

        private void buttonApagar_Click(object sender, EventArgs e)
        {
            this.ApagarMontura();
        }

        private bool lastSlewing;
        private static String cam = "http://HOST:80/decoder_control.cgi?command=CMD&user=admin&pwd=";

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

        private void ProcesaTelescopio()
        {
            //this.timerTelescopio.Stop();
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


                Boolean slewing;
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
    }
}
