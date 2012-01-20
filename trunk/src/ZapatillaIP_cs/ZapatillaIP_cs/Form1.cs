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

namespace ZapatillaIP_cs
{
    public partial class Form1 : Form
    {
        TcpClient tcpclnt;
        List<CheckBox> relayCheckBox;
        Boolean[] relayStatus;
        byte port0;
        byte port1;
        byte[] message;
        static ZapatillaIP_cs.Properties.Settings settings = Properties.Settings.Default;

        public Form1()
        {
            InitializeComponent();
            tcpclnt = new TcpClient();

            try
            {
                tcpclnt.Connect(settings.ipAddress, (int) settings.port);
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
                //Application.Exit();
                //this.Close();
                //return;
            }
            
            Console.WriteLine("Connected");

            relayStatus = new Boolean[16];


            message = new byte[4];
            // Setea todo como energizado pone los botones en Verde
            // y al mismo tiempo supone que la PDU esta 
            // energizando todos los equipos.
            port0 = 0;
            port1 = 0;
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
            Stream stm;
            stm = null;
            if (tcpclnt.Connected)
            {
                stm = tcpclnt.GetStream();
                stm.WriteByte(2);
                byte p0, p1;
                p0 = (byte)stm.ReadByte();
                p1 = (byte)stm.ReadByte();
                int stat;
                stat = (p0 + (p1 << 8));
                Console.WriteLine("p0=" + p0 + "  p1=" + p1 + "  stat=" + stat);
                for (int i = 0; i < 16; i++)
                {
                    relayStatus[i] = ((stat % 2) == 0);
                    stat /= 2;
                }
            }
            this.refreshcheckBoxRelayColors();
        }

        private void refreshcheckBoxRelayColors()
        {
            CheckBox boton;
            for (int i = 0; i < 16; i++)
            {
                boton = relayCheckBox[i];
                if (relayStatus[i])
                {
                    boton.BackColor = Color.LightGreen;
                }
                else
                {
                    boton.BackColor = Color.Pink;
                }
                if (!tcpclnt.Connected)
                {
                    boton.Enabled = false;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tcpclnt.Close();
        }

        private void refreshPorts()
        {
            Console.WriteLine("refreshPorts:");
            port0 = 0;
            for (int i = 0; i < 8; i++)
            {
                if (!relayStatus[i])
                    port0 = (byte) (port0 | (((byte)1) << ((byte)i)));
            }
            port1 = 0;
            for (int i = 0; i < 8; i++)
            {
                if (!relayStatus[i+8])
                    port1 = (byte)(port1 | (((byte)1) << ((byte)i)));
            }
            Console.WriteLine("port0="+port0+"  port1="+port1);

            Stream stm = tcpclnt.GetStream();

            Console.WriteLine("Transmitting.....");
            message[0] = 1;
            message[1] = port0;
            message[2] = port1;
            message[3] = (byte)(1 + port0 + port1);
            stm.Write(message,0,4);
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
                    Console.WriteLine("relayStatus[" + indice + "]=" + relayStatus[indice] + " ---> " + targetState);
                    relayCheckBox[indice].Checked = false;
                    relayStatus[indice] = targetState;

                }
                refreshcheckBoxRelayColors();
                refreshPorts();
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

 
    }
}