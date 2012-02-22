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
        private ArduinoTcp arduinoTcp;
        static Montura.Properties.Settings settings = Properties.Settings.Default;

        public Form1()
        {
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
        }

        private void ApagarMontura()
        {
            this.BringToFront();
            this.arduinoTcp.Connect();
            if (this.arduinoTcp.Tcpclnt.Connected)
            {
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

            Console.WriteLine(respuesta);
            Status stat;
            stat = new Serduino.Status(respuesta);
            stat.Analiza();

            RefreshColor(this.radioButtonRA_East, stat.RaLimitEast, Color.Pink, Color.LightYellow);
            RefreshColor(this.radioButtonRA_West, stat.RaLimitWest, Color.Pink, Color.LightYellow);
            RefreshColor(this.radioButtonRA_Home, stat.RaHome, Color.LightGreen, Color.LightYellow);
            RefreshColor(this.radioButtonDecHome, stat.DecHome, Color.LightGreen, Color.LightYellow);
            
            if ((!this.buttonContinue.Enabled) && ((stat.RaLimitEast) || (stat.RaLimitWest)))
            {
                this.buttonContinue.Enabled = true;
                this.ApagarMontura();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            //this.timerReadSerial.Start();
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
    }
}
