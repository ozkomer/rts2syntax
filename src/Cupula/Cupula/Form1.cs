using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Chase500;

namespace Cupula
{
    public partial class Form1 : Form
    {
        CupulaEthernet cet;
        CheckBox[] checkBoxJ1;
        CheckBox[] checkBoxO1;

        public Form1()
        {
            InitializeComponent();
            cet = new CupulaEthernet();
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
            this.checkBoxO1[indice] = this.checkBoxO1XT105; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT106; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT107; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT108; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT109; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT110; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT111; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT112; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT113; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT114; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT115; indice++;
            this.checkBoxO1[indice] = this.checkBoxO1XT116; indice++;

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
            int status;
            status = cet.IsClosed();
            buttonIsClosed.Text = ("Is closed=" + status);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cet.TcpSession.connected)
            {
                cet.TcpSession.disconnect();
            }
        }


        private void checkBoxJ1XT102_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!cet.TcpSession.connected)
            {
                this.cet.TcpSession.connect("10.0.65.10", 502);
            }
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

        }

        private void buttonStartClose_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if ((i == 3) || (i == 7))
                {
                    checkBoxJ1[i].Checked = true;
                }
                else
                {
                    checkBoxJ1[i].Checked = false;
                }
                cet.Zreg_J1XT1[i] = checkBoxJ1[i].Checked;
            }
        }





    }
}