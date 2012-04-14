using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace ZxRelay16
{
    /// <summary>
    /// Control de un Arreglo de Reles:
    /// http://www.inexglobal.com/products.php?type=addon&cat=app_control&model=zxrelay16
    /// Usando un arduino Mega2560 con modulo ethernet.
    /// Por lo tanto el control del arreglo de reles es mediante
    /// comandos de una sesion TCP/IP
    /// </summary>
    public class ArduinoTcp
    {
        #region Variables de Instancia
        /// <summary>
        /// Cliente que se conecta al modulo Ethernet del arduino
        /// </summary>
        private TcpClient tcpCliente;

        /// <summary>
        /// Status de cada uno de los 16 relays.
        /// true -> Dispositivo energizado.
        /// </summary>
        private Boolean[] relayStatus;

        /// <summary>
        /// Status de los 8 primeros reles.
        /// </summary>
        private byte port0;

        /// <summary>
        /// Status de los 8 ultimos reles.
        /// </summary>
        private byte port1;

        /// <summary>
        /// Los comandos que entiende el arduino, corresponden
        /// a secuencias de 4 bytes.
        /// </summary>
        private byte[] message;

        /// <summary>
        /// Host del Ethernet Shiel del arduino
        /// </summary>
        private string host;

        /// <summary>
        /// Puerto donde el software servidor del arduino espera clientes.
        /// </summary>
        private int tcpPort;
        #endregion

        /// <summary>
        /// Status de cada uno de los 16 relays.
        /// true -> Dispositivo energizado.
        /// </summary>        
        public Boolean[] RelayStatus
        {
            get { return this.relayStatus; }
        }

        public TcpClient Tcpclnt
        {
            get { return this.tcpCliente; }
        }

        public ArduinoTcp(String host, int tcpPort)
        {
            this.host = host;
            this.tcpPort = tcpPort;
            relayStatus = new Boolean[16];
            message = new byte[4];
        }

        public void readRelays()
        {
            Stream stm;
            stm = null;
            if (tcpCliente.Connected)
            {
                stm = tcpCliente.GetStream();
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

            Stream stm = tcpCliente.GetStream();

            Console.WriteLine("Transmitting.....");
            message[0] = 1;
            message[1] = port0;
            message[2] = port1;
            message[3] = (byte)(1 + port0 + port1);
            stm.Write(message, 0, 4);
        }

        public void Connect ()
        {
            tcpCliente = new TcpClient();
            tcpCliente.Connect(this.host, this.tcpPort);            
        }
    }
}
