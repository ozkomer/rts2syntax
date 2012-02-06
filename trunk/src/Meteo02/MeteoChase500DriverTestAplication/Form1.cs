using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Meteo02;

namespace MeteoChase500DriverTestAplication
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// El driver que se esta testeando
        /// </summary>
        private SafetyMonitor sm;

        public Form1()
        {
            InitializeComponent();
            this.labelMaxHumidity.Text = "Max Hum = " + WeatherAnalisis.MaxHumidity;
            this.labelMaxWindSpeed.Text = "Max Wind Speed = " + WeatherAnalisis.MaxWindSpeed_inKnots;
            this.labelMinDewPoint.Text = "Min Dew Point Delta = " + WeatherAnalisis.MinDewPointDelta;

            sm = new SafetyMonitor();
            this.timerRefresh.Start();
            //sm.TimerLeer.Elapsed += new System.Timers.ElapsedEventHandler(TimerLeer_Elapsed);
            this.refresh();

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
            this.labelHumidity.Text = "Humidity = " + sm.RelativeHumidity +"%";
            this.labelPressure.Text = "Pressure = " + sm.BarometricPressure + " [mBar]";
            this.labelTemperature.Text = "Temperature = " + sm.AmbientTemperature + " [ºC]";

            StringBuilder sbDewpoint;
            sbDewpoint = new StringBuilder();
            sbDewpoint.Append("DewPoint=");
            sbDewpoint.Append(sm.DewPoint);
            sbDewpoint.Append(" [ºC] \t delta =");
            sbDewpoint.Append(sm.AmbientTemperature - sm.DewPoint);
            sbDewpoint.Append(" [ºC]");
            this.labelDewPoint.Text = sbDewpoint.ToString();

            this.labelWindDir.Text = "Wind Direction = " + sm.WindDirection+" [0=east, 90=North]";
            this.labelWindSpeed.Text = "Wind Speed = " + sm.WindVelocity+" [mph]";

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
    }
}
