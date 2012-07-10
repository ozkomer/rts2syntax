using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.PromptWeather;

namespace PromptWeatherTest
{
    public partial class Form1 : Form
    {
        private SafetyMonitor monitor;

        public Form1()
        {
            monitor = new SafetyMonitor();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.AppendLine(DateTime.Now.ToShortDateString());
            mensaje.Append("Prompt: Cant Domos Abiertos=");
            mensaje.AppendLine(""+monitor.CantDomosAbiertos);
            mensaje.Append("IsSafe=");
            mensaje.Append(monitor.Safe);
            Console.WriteLine(mensaje.ToString());
        }
    }
}
