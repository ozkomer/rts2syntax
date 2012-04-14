using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ASCOM.Chase500;
using ASCOM.DeviceInterface;

namespace Cupula
{
    public partial class Form1 : Form
    {
        Dome cet;
        CheckBox[] checkBoxJ1;
        CheckBox[] checkBoxO1;

        public Form1()
        {
            InitializeComponent();
            cet = new Dome();
            comboBoxControlNorth.SelectedIndex = 0;
            comboBoxControlSouth.SelectedIndex = 0;
            checkBoxJ1 = new CheckBox[8];
            checkBoxO1 = new CheckBox[16];

            int indice;

            indice = 0;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT101; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT102; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT103; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT104; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT105; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT106; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT107; indice++;
            this.checkBoxJ1[indice] = this.checkBoxJ1XT108; indice++;

            indice = 0;
            this.checkBoxO1[indice] = this.checkBoxO1XT101; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT102; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT103; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT104; indice++;
            this.checkBoxO1[Dome.ZS_SOUTH_OPEN] = this.checkBoxO1XT105; indice++;
            this.checkBoxO1[Dome.ZS_SOUTH_50] = this.checkBoxO1XT106; indice++;
            this.checkBoxO1[Dome.ZS_SOUTH_CLOSE] = this.checkBoxO1XT107; indice++;
            this.checkBoxO1[Dome.ZS_NORTH_OPEN] = this.checkBoxO1XT108; indice++;
            this.checkBoxO1[Dome.ZS_NORTH_50] = this.checkBoxO1XT109; indice++;
            this.checkBoxO1[Dome.ZS_NORTH_CLOSE] = this.checkBoxO1XT110; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT111; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT112; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT113; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT114; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT115; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT116; indice++;

            //cet.DeadManTimer.Elapsed += new System.Timers.ElapsedEventHandler(DeadManTimer_Elapsed);
            //cet.DomeMovingTimer.Elapsed += new System.Timers.ElapsedEventHandler(DomeMovingTimer_Elapsed);

        }

        void DomeMovingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(400);
            switch (cet.ShutterStatus)
            {
                case ShutterState.shutterClosed:
                    this.labelStatus.Text = "ShutterState=shutterClosed";
                    this.buttonStartClose.Enabled = true;
                    this.buttonStartOpen.Enabled = true;
                    break;
                case ShutterState.shutterClosing:
                    this.labelStatus.Text = "ShutterState=shutterClosing";
                    break;
                case ShutterState.shutterOpen:
                    this.labelStatus.Text = "ShutterState=shutterOpen";
                    this.buttonStartClose.Enabled = true;
                    this.buttonStartOpen.Enabled = true;
                    break;
                case ShutterState.shutterOpening:
                    this.labelStatus.Text = "ShutterState=shutterOpening";
                    break;

            }
        }

        void DeadManTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.labelKeepOpen.BackColor = Color.LightGreen;
            System.Threading.Thread.Sleep(1000);
            this.labelKeepOpen.BackColor = Color.LightGray;
        }

        private void buttonIsOpened_Click(object sender, EventArgs e)
        {
            this.statusRead();
            int status;
            status = cet.IsOpened();
            buttonIsOpened.Text = ("Is opened=" + status);
        }

        private void buttonIsClosed_Click(object sender, EventArgs e)
        {
            this.statusRead();
            Boolean status;
            status = cet.IsClosed();
            buttonIsClosed.Text = ("C=" + status);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            cet.Disconnect();
        }


        private void checkBoxJ1XT102_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            cet.Host = "10.0.65.10";
            cet.Port = 502;
            cet.Connect();
        }

        private void statusRead()
        {
            cet.Read_ZREG_O1XT1();
            for (int i = 0; i < 16; i++)
            {
                this.checkBoxO1[i].Checked = cet.Zreg_O1XT1[i];
                Color color;
                color = (cet.Zreg_O1XT1[i] ? Color.LightGreen : Color.LightPink);
                this.checkBoxO1[i].BackColor = color;
            }
        }

        private void buttonStatusRead_Click(object sender, EventArgs e)
        {
            this.statusRead();
        }

        private void buttonControlRead_Click(object sender, EventArgs e)
        {
            cet.Read_ZREG_J1XT1();
            for (int i = 0; i < 8; i++)
            {
                this.checkBoxJ1[i].Checked = cet.Zreg_J1XT1[i];
            }
        }

        private void comboBoxControlSouth_SelectedIndexChanged(object sender, EventArgs e)
        {
            cet.SouthRoof = (ushort)comboBoxControlSouth.SelectedIndex;
            Console.WriteLine("cet.SouthRoof=" + cet.SouthRoof);
        }

        private void comboBoxControlNorth_SelectedIndexChanged(object sender, EventArgs e)
        {
            cet.NorthRoof = (ushort)comboBoxControlNorth.SelectedIndex;
            Console.WriteLine("cet.NorthRoof=" + cet.NorthRoof);
        }

        private void buttonStartOpen_Click(object sender, EventArgs e)
        {
            this.buttonStartClose.Enabled = false;
            this.buttonStartOpen.Enabled = false;

            cet.NorthRoof = (ushort)comboBoxControlNorth.SelectedIndex;
            cet.SouthRoof = (ushort)comboBoxControlSouth.SelectedIndex;
            cet.OpenShutter();
            cupulaMovingTimer.Start();
        }

        private void buttonStartClose_Click(object sender, EventArgs e)
        {
            this.buttonStartClose.Enabled = false;
            this.buttonStartOpen.Enabled = false;
            cet.CloseShutter();
            //for (int i = 0; i < 8; i++)
            //{
            //    checkBoxJ1[i].Checked = cet.Zreg_J1XT1[i];
            //}
        }

        private void buttonControlWrite_Click(object sender, EventArgs e)
        {
            cet.Write_ZREG_J1XT1();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            String[] linea;
            linea = this.textBox1.Text.Split(("\n").ToCharArray());
            this.textBox2.Text = linea[(int)numericUpDown1.Value];
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.numericUpDown1.Value++;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Instruccion instr;
            instr = new Instruccion(this.textBox2.Text);
            String resultado;
            resultado = instr.ejecutar(this.cet.TcpSession);
            String textoSalida;
            textoSalida = textBox3.Text;
            textoSalida = textoSalida + Environment.NewLine + resultado;
            textBox3.Text = textoSalida;
            this.numericUpDown1.Value++;
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            cet.SetupDialog();
        }

        private void cupulaMovingTimer_Tick(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(300);
            switch (cet.ShutterStatus)
            {
                case ShutterState.shutterClosed:
                    this.labelStatus.Text = "ShutterState=shutterClosed";
                    this.buttonStartClose.Enabled = true;
                    this.buttonStartOpen.Enabled = true;
                    break;
                case ShutterState.shutterClosing:
                    this.labelStatus.Text = "ShutterState=shutterClosing";
                    break;
                case ShutterState.shutterOpen:
                    this.labelStatus.Text = "ShutterState=shutterOpen";
                    this.buttonStartClose.Enabled = true;
                    this.buttonStartOpen.Enabled = true;
                    break;
                case ShutterState.shutterOpening:
                    this.labelStatus.Text = "ShutterState=shutterOpening";
                    break;

            }
            cupulaMovingTimer.Enabled = cet.DomeMovingTimer.Enabled;
            Console.WriteLine("DomeMovingTimer.Enabled" + cet.DomeMovingTimer.Enabled);
        }





    }
}