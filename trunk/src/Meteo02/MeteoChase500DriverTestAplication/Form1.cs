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
            logger.Info("Start");
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
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
