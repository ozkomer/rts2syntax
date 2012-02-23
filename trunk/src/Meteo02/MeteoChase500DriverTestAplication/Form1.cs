using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Meteo02;
using log4net.Config;
using log4net;
using log4net.Appender;

namespace MeteoChase500DriverTestAplication
{
    public partial class Form1 : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));
        /// <summary>
        /// El driver que se esta testeando
        /// </summary>
        private SafetyMonitor sm;

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Start");
            InitializeComponent();
            this.labelMaxHumidity.Text = "Max Hum = " + (WeatherAnalisis.MaxHumidity * 100)+" %";
            this.labelMaxWindSpeed.Text = "Max Wind Speed = " + WeatherAnalisis.MaxWindSpeed_inKnots;
            this.labelMinDewPoint.Text = "Min Dew Point Delta = " + WeatherAnalisis.MinDewPointDelta + " [ºC]";

            sm = new SafetyMonitor();
            this.timerRefresh.Start();
            this.refresh();
            logger.Info("End");
        }

        void refresh()
        {
            if (this.sm.Safe)
            {
                this.BackColor = Color.LightGreen;
            }
            else
            {
                this.BackColor = Color.LightPink;
            }

            this.labelDateTime.Text = "FechaHora = " + sm.FechaHora;
            this.labelHumidity.Text = "Humidity = " + (sm.RelativeHumidity * 100) +"%";
            this.labelPressure.Text = "Pressure = " + sm.BarometricPressure + " [mBar]";
            this.labelTemperature.Text = "Temperature = " + sm.AmbientTemperature + " [ºC]";
            this.labelAverageWindSpeed.Text = "Average Wind Speed = " + sm.Weather_Analisis.AverageWindSpeed;
            StringBuilder sbDewpoint;
            sbDewpoint = new StringBuilder();
            sbDewpoint.Append("DewPoint=");
            sbDewpoint.Append(sm.DewPoint);
            sbDewpoint.Append(" [ºC] \t delta =");
            sbDewpoint.Append(sm.AmbientTemperature - sm.DewPoint);
            sbDewpoint.Append(" [ºC]");
            this.labelDewPoint.Text = sbDewpoint.ToString();

            this.labelWindDir.Text = "Wind Direction = " + sm.WindDirection+" [0=east, 90=North]";
            this.labelWindSpeed.Text = "Wind Speed = " + sm.WindVelocity;

            this.listBoxRegistros.Items.Clear();
            foreach (WeatherRow wr in this.sm.Weather_Analisis.Reverse())
            {
                this.listBoxRegistros.Items.Add(wr);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("timerRefresh_Tick");
            this.refresh();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            this.sm.SetupDialog();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.MostrarVentana();
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
            respuesta = MessageBox.Show("Closing application. Continue?", "Chase500 Meteorology Analisis.", MessageBoxButtons.YesNo);
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


    }
}
