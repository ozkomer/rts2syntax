using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace ZxRelay16
{
    public class ArduinoTcp
    {
        private TcpClient tcpclnt;
        private Boolean[] relayStatus;
        private byte port0;
        private byte port1;
        private byte[] message;
        private string host;
        private int tcpPort;

        public Boolean[] RelayStatus
        {
            get { return this.relayStatus; }
        }

        public TcpClient Tcpclnt
        {
            get { return this.tcpclnt; }
        }

        public ArduinoTcp(String host, int tcpPort)
        {
            this.host = host;
            this.tcpPort = tcpPort;
            relayStatus = new Boolean[16];

            message = new byte[4];
            // Setea todo como energizado pone los botones en Verde
            // y al mismo tiempo supone que la PDU esta 
            // energizando todos los equipos.
            //port0 = 0;
            //port1 = 0;

        }

        public void readRelays()
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
        }

        public void refreshPorts()
        {
            Console.WriteLine("refreshPorts:");
            port0 = 0;
            for (int i = 0; i < 8; i++)
            {
                if (!relayStatus[i])
                    port0 = (byte)(port0 | (((byte)1) << ((byte)i)));
            }
            port1 = 0;
            for (int i = 0; i < 8; i++)
            {
                if (!relayStatus[i + 8])
                    port1 = (byte)(port1 | (((byte)1) << ((byte)i)));
            }
            Console.WriteLine("port0=" + port0 + "  port1=" + port1);

            Stream stm = tcpclnt.GetStream();

            Console.WriteLine("Transmitting.....");
            message[0] = 1;
            message[1] = port0;
            message[2] = port1;
            message[3] = (byte)(1 + port0 + port1);
            stm.Write(message, 0, 4);
        }

        public void Connect ()
        {
            tcpclnt = new TcpClient();
                tcpclnt.Connect(this.host, this.tcpPort);

            Console.WriteLine("Connected");
        }


        


    }
}
