using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ZxRelay16;

namespace ZapatillaIP_cs
{
    public partial class Form1 : Form
    {

        List<CheckBox> relayCheckBox;
        static ZapatillaIP_cs.Properties.Settings settings = Properties.Settings.Default;
        private ArduinoTcp arduinoTcp;

        public Form1()
        {
            InitializeComponent();
            this.arduinoTcp = new ArduinoTcp(settings.ipAddress, (int)settings.port);
            this.arduinoTcp.Connect();
            relayCheckBox = new List<CheckBox>();
            relayCheckBox.Add(checkBoxRelay1);
            relayCheckBox.Add(checkBoxRelay2);
            relayCheckBox.Add(checkBoxRelay3);
            relayCheckBox.Add(checkBoxRelay4);
            relayCheckBox.Add(checkBoxRelay5);
            relayCheckBox.Add(checkBoxRelay6);
            relayCheckBox.Add(checkBoxRelay7);
            relayCheckBox.Add(checkBoxRelay8);
            relayCheckBox.Add(checkBoxRelay9);
            relayCheckBox.Add(checkBoxRelay10);
            relayCheckBox.Add(checkBoxRelay11);
            relayCheckBox.Add(checkBoxRelay12);
            relayCheckBox.Add(checkBoxRelay13);
            relayCheckBox.Add(checkBoxRelay14);
            relayCheckBox.Add(checkBoxRelay15);
            relayCheckBox.Add(checkBoxRelay16);


            readRelays();
        }

        private void readRelays()
        {
            this.arduinoTcp.readRelays();
            this.refreshcheckBoxRelayColors();
        }

        private void refreshcheckBoxRelayColors()
        {
            CheckBox boton;
            for (int i = 0; i < 16; i++)
            {
                boton = relayCheckBox[i];
                if (this.arduinoTcp.RelayStatus[i])
                {
                    boton.BackColor = Color.LightGreen;
                }
                else
                {
                    boton.BackColor = Color.Pink;
                }
                if (!this.arduinoTcp.Tcpclnt.Connected)
                {
                    boton.Enabled = false;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.arduinoTcp.Tcpclnt.Close();
        }


        private void buttonReadRelay_Click(object sender, EventArgs e)
        {
            readRelays();
        }

        //private void buttonRelay_Click(object sender, EventArgs e)
        //{
        //    int botonPresionado;
        //    //botonPresionado = -1;
        //    botonPresionado = relayButton.IndexOf((Button)sender);
        //    Console.WriteLine("botonPresionado=" + botonPresionado);
        //    relayStatus[botonPresionado] = !relayStatus[botonPresionado];
        //    refreshButtonRelayColors();
        //    refreshPorts();
        //}

        /**
         * Recorre la lista de reles, formando una lista con aquellos que estan tickeados. 
         * 
         */
        private void switchRelays(Boolean targetState)
        {
            List<int> tickeds;
            CheckBox cbLocal;
            tickeds = new List<int>();
            StringBuilder message;
            message = new StringBuilder();
            message.AppendLine(" Los reles:\n");
            for (int i = 0; i < 16; i++)
            {
                cbLocal = relayCheckBox[i];
                if (cbLocal.Checked)
                {
                    tickeds.Add(i);
                    //relayStatus[i] = targetState;
                    message.AppendLine(relayCheckBox[i].Text);
                }
            }
            message.Append("serán ");
            if (targetState)
            {
                message.AppendLine("encendidos.");
            }
            else
            {
                message.AppendLine("apagados");
            }
            DialogResult respuesta;
            respuesta = MessageBox.Show(message.ToString(), "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (respuesta == DialogResult.Yes)
            {
                foreach (int indice in tickeds)
                {
                    Console.WriteLine("relayStatus[" + indice + "]=" + this.arduinoTcp.RelayStatus[indice] + " ---> " + targetState);
                    relayCheckBox[indice].Checked = false;
                    this.arduinoTcp.RelayStatus[indice] = targetState;
                }
                refreshcheckBoxRelayColors();
                this.arduinoTcp.refreshPorts();
            }
        }

        private void buttonSwitchOff_Click(object sender, EventArgs e)
        {
            switchRelays(false);
        }

        private void buttonSwitchOn_Click(object sender, EventArgs e)
        {
            switchRelays(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

 
    }
}