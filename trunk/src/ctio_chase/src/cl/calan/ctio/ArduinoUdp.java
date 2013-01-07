/**
 * 
 */
package cl.calan.ctio;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.net.UnknownHostException;

/**
 *  Control de un Arreglo de Reles:
 *  http://www.inexglobal.com/products.php?type=addon&cat=app_control&model=zxrelay16
 *  
 *  Usando un arduino Mega2560 con modulo ethernet.
 *  Por lo tanto el control del arreglo de reles es mediante
 *  comandos de una sesion UDP/IP
 *  
 * @author sysop
 *
 */
public class ArduinoUdp {

    /// <summary>
    /// Cliente que se conecta al modulo Ethernet del arduino
    /// </summary>
    private DatagramSocket socket;
    
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
    private String host;

    /// <summary>
    /// Puerto donde el software servidor del arduino espera clientes.
    /// </summary>
    private int udpPort;

    private InetAddress address = null;;
	

    
	public ArduinoUdp(String host, int udpPort) {
		super();
		this.host = host;
		this.udpPort = udpPort;
		System.out.println("ArduinoUdp .... Host ="+host+"\t udpPort="+udpPort);
		try {
			this.address = InetAddress.getByName(this.host);
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
        relayStatus = new Boolean[16];
        message = new byte[4];
	}	

    public void readRelays()
    {

        	this.socket = null;
        	try {
				this.socket = new DatagramSocket();
			} catch (SocketException e) {
				System.err.println("Error creando DatagramSocket.");
			}
			if (socket==null)
			{
				return;
			}
        	DatagramPacket packetSend;
			byte[] writeBuffer;
			writeBuffer = new byte[1];
			writeBuffer[0]=2;
			packetSend= new DatagramPacket(writeBuffer,1,this.address,udpPort);
        	try {
				this.socket.send(packetSend);
				int puertoOrigen;
				puertoOrigen = this.socket.getLocalPort();
				System.out.println("puertoOrigen="+puertoOrigen);
			} catch (IOException e) {
				System.err.println("Error enviando Datagram.");
			}

			DatagramPacket packetReceive;
			byte[] readBuffer;
			readBuffer = new byte[2];
			packetReceive = new DatagramPacket(readBuffer, 2);
			try {

				//socket.setSoTimeout(1500);
				System.out.println("bloqueado por socket.receive()");
				
				socket.receive(packetReceive);
				System.out.println("\t\t Packete recibido!!!");
			} catch (IOException e) {
				System.err.println("Error recibiendo Datagram.");
			}
			
            byte p0, p1;
            p0 = readBuffer[0];
            p1 = readBuffer[1];
//            try {
//				//p0 = (byte)iStm.read();
//                //p1 = (byte)iStm.read();
//            	p0 = (byte)socket.getInputStream().read();
//            	 try {
//					Thread.sleep(500);
//				} catch (InterruptedException e) {
//					// TODO Auto-generated catch block
//					e.printStackTrace();
//				}
//            	p1 = (byte)socket.getInputStream().read();
//			} catch (IOException e) {
//				// TODO Auto-generated catch block
//				e.printStackTrace();
//			}
            int stat;
            stat = (p0 + (p1 << 8));
            System.out.println("p0=" + p0 + "  p1=" + p1 + "  stat=" + stat);
            for (int i = 0; i < 16; i++)
            {
                relayStatus[i] = ((stat % 2) == 0);
                stat /= 2;
            }        
    }
    
    public void refreshPorts()
    {
        System.out.println("refreshPorts:");

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
        System.out.println("port0=" + port0 + "  port1=" + port1);

		System.out.println("Transmitting.....");
        message[0] = 1;
        message[1] = port0;
        message[2] = port1;
        message[3] = (byte)(1 + port0 + port1);


    	this.socket = null;
    	try {
			this.socket = new DatagramSocket();
		} catch (SocketException e) {
			System.err.println("Error creando DatagramSocket.");
		}
		if (socket==null)
		{
			return;
		}
    	DatagramPacket packet;

		packet= new DatagramPacket(message,4,this.address,udpPort);
    	try {
			this.socket.send(packet);
		} catch (IOException e) {
			System.err.println("Error enviando Datagram.");
		}
    }
    
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

	public Boolean[] getRelayStatus() {
		return relayStatus;
	}

	public void setRelayStatus(Boolean[] relayStatus) {
		this.relayStatus = relayStatus;
	}
	
	

}
