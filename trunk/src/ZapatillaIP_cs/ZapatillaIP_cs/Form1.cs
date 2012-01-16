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
        List<Button> relayButton;
        Boolean[] relayStatus;
        byte port0;
        byte port1;
        byte[] message;
        static ZapatillaIP_cs.Properties.Settings settings = Properties.Settings.Default;

        public Form1()
        {
            InitializeComponent();
            tcpclnt = new TcpClient();


            tcpclnt.Connect(settings.ipAddress, (int) settings.port);
            Console.WriteLine("Connected");

            relayStatus = new Boolean[16];
            for (int i = 0; i < 16; i++)
            {
                relayStatus[i] = true;
            }

            message = new byte[4];
            // Setea todo como energizado pone los botones en Verde
            // y al mismo tiempo supone que la PDU esta 
            // energizando todos los equipos.
            port0 = 0;
            port1 = 0;
            relayButton = new List<Button>();
            relayButton.Add(buttonRelay1);
            relayButton.Add(buttonRelay2);
            relayButton.Add(buttonRelay3);
            relayButton.Add(buttonRelay4);
            relayButton.Add(buttonRelay5);
            relayButton.Add(buttonRelay6);
            relayButton.Add(buttonRelay7);
            relayButton.Add(buttonRelay8);
            relayButton.Add(buttonRelay9);
            relayButton.Add(buttonRelay10);
            relayButton.Add(buttonRelay11);
            relayButton.Add(buttonRelay12);
            relayButton.Add(buttonRelay13);
            relayButton.Add(buttonRelay14);
            relayButton.Add(buttonRelay15);
            relayButton.Add(buttonRelay16);

            refreshButtonRelayColors();
        }

        private void refreshButtonRelayColors()
        {
            Button boton;
            for (int i = 0; i < 16; i++)
            {
                boton = relayButton[i];
                if (relayStatus[i])
                {
                    boton.BackColor = Color.LightGreen;
                }
                else
                {
                    boton.BackColor = Color.Pink;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tcpclnt.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }

        private void buttonRelay_Click(object sender, EventArgs e)
        {
            int botonPresionado;
            //botonPresionado = -1;
            botonPresionado = relayButton.IndexOf((Button)sender);
            Console.WriteLine("botonPresionado=" + botonPresionado);
            relayStatus[botonPresionado] = !relayStatus[botonPresionado];
            refreshButtonRelayColors();
            refreshPorts();
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
            Stream stm = tcpclnt.GetStream();
            stm.WriteByte(2);
            byte p0, p1;
            p0 = (byte)stm.ReadByte();
            p1 = (byte)stm.ReadByte();
            int stat;
            stat = (p0 + (p1 << 8));
            Console.WriteLine("p0=" + p0 + "  p1=" + p1+ "  stat="+stat);
            for (int i = 0; i < 16; i++)
            {
                relayStatus[i] = ((stat%2)==0);
                stat /= 2;
            }
            this.refreshButtonRelayColors();
        }
    }
}