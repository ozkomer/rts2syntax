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

namespace Montura
{
    public partial class Form1 : Form
    {
        public char[] query;
        private int riskLevel;
        private ArduinoTcp arduinoTcp;
        static Montura.Properties.Settings settings = Properties.Settings.Default;

        public Form1()
        {
            this.arduinoTcp = new ArduinoTcp(settings.ipAddress, (int)settings.port);
            query = new char[2];
            query[0] = '?';
            query[1] = '\n';
            InitializeComponent();
            riskLevel = 0;
            
            //this.perfChartRA_x.ScaleMode = SpPerfChart.ScaleMode.Absolute;

            this.perfChartRA_x.PerfChartStyle.ChartLinePen.Color = Color.Yellow;
            this.perfChartRA_x.PerfChartStyle.AvgLinePen.Color = Color.SteelBlue;
            this.perfChartRA_Y.PerfChartStyle.ChartLinePen.Color = Color.Pink;
            this.perfChartRA_Y.PerfChartStyle.AvgLinePen.Color = Color.SteelBlue;
            this.perfChartRA_Z.PerfChartStyle.ChartLinePen.Color = Color.Violet;
            this.perfChartRA_Z.PerfChartStyle.AvgLinePen.Color = Color.SteelBlue;

            this.serialPortMontura.Open();
            this.timerReadSerial.Start();
        }

        private void ApagarMontura()
        {
            this.arduinoTcp.Connect();
            if (this.arduinoTcp.Tcpclnt.Connected)
            {
                this.tabControl1.Visible = true;
                this.arduinoTcp.readRelays(); // this.readRelays();
                System.Threading.Thread.Sleep(250);

                for (int i = 0; i < 16; i++)
                {
                    Console.Write("\t" + this.arduinoTcp.RelayStatus[i]);
                }
                Console.WriteLine("  ...");
                //Solo modificamos el relay correspondiente a la montura
                this.arduinoTcp.RelayStatus[10] = false;//debe ser 10
                this.arduinoTcp.refreshPorts();
                System.Threading.Thread.Sleep(200);
                
                //this.arduinoTcp.readRelays();
                //System.Threading.Thread.Sleep(200);

                for (int i = 0; i < 16; i++)
                {
                    Console.Write("\t" + this.arduinoTcp.RelayStatus[i]);
                }
                Console.WriteLine("  ...");

                if (this.arduinoTcp.Tcpclnt.Connected)
                {
                    this.arduinoTcp.Tcpclnt.Close();
                }
            }
        }

        private void timerReadSerial_Tick(object sender, EventArgs e)
        {
            String respuesta;
            respuesta = "Tick";
            serialPortMontura.Write (query, 0, 1);
            respuesta = serialPortMontura.ReadLine();
         
            Console.WriteLine(respuesta);
            Status stat;
            stat = new Serduino.Status(respuesta);
            stat.Analiza();
            this.perfChartRA_x.AddValue(stat.AcelerometroRA.RelativeX);
            this.perfChartRA_Y.AddValue(stat.AcelerometroRA.RelativeY);
            this.perfChartRA_Z.AddValue(stat.AcelerometroRA.RelativeZ);
            StringBuilder raX,raY,raZ;
            raX = new StringBuilder();
            raX.Append("RA X: min=");
            raX.Append(stat.AcelerometroRA.minX);
            raX.Append("\t max=");
            raX.Append(stat.AcelerometroRA.maxX);
            this.labelRA_X.Text = raX.ToString();

            raY = new StringBuilder();
            raY.Append("RA y: min=");
            raY.Append(stat.AcelerometroRA.minY);
            raY.Append("\t max=");
            raY.Append(stat.AcelerometroRA.maxY);
            this.labelRA_Y.Text = raY.ToString();

            raZ = new StringBuilder();
            raZ.Append("RA Z: min=");
            raZ.Append(stat.AcelerometroRA.minZ);
            raZ.Append("\t max=");
            raZ.Append(stat.AcelerometroRA.maxZ);
            this.labelRA_Z.Text = raZ.ToString();
            double alpha;
            alpha = Acelerometro.getAlpha(stat.AcelerometroRA);
            alpha = ((180 * alpha) / (Math.PI));
            this.labelAlpha.Text = "Angle ~=" + alpha;
            if ((alpha > 88.4) || (alpha < -74.5))
            {
                this.riskLevel++;
            }
            else
            {
                this.riskLevel--;
            }
            if (riskLevel < 0) { riskLevel = 0; }
            if (riskLevel > 5) { this.riskLevel = 5; }
            this.progressBarRisk.Value = (20 * riskLevel);
            Color fore;
            fore = Color.Green;
            if (riskLevel > 1)
            {
                fore = Color.Yellow;
            }
            if (riskLevel > 3)
            {
                fore = Color.Red;
            }
            if (riskLevel == 5)
            {
                this.timerReadSerial.Stop();
                this.buttonContinue.Enabled = true;
                this.ApagarMontura();
            }
            this.progressBarRisk.ForeColor = fore;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            this.riskLevel = 0;
            this.timerReadSerial.Start();
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
    }
}
