/**
 * 
 */
package cl.calan.ctio;

import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 *  Control de un Arreglo de Reles:
 *  http://www.inexglobal.com/products.php?type=addon&cat=app_control&model=zxrelay16
 *  
 *  Usando un arduino Mega2560 con modulo ethernet.
 *  Por lo tanto el control del arreglo de reles es mediante
 *  comandos de una sesion TCP/IP
 *  
 * @author sysop
 *
 */
public class ArduinoTcp {

    /// <summary>
    /// Cliente que se conecta al modulo Ethernet del arduino
    /// </summary>
    private java.net.Socket tcpCliente;
    
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
    private int tcpPort;

    
	public ArduinoTcp(String host, int tcpPort) {
		super();
		this.host = host;
		this.tcpPort = tcpPort;
        relayStatus = new Boolean[16];
        message = new byte[4];
	}
	
    public String Connect ()
    {
    	String respuesta;
    	respuesta = null;
        try {
			tcpCliente = new Socket(this.host, this.tcpPort);
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			System.out.println("error al conectar:"+e.getMessage());
			respuesta = e.getMessage();			
		}
		return respuesta;
    }
    
    public Boolean IsConnected ()
    {
    	//Boolean respuesta;
    	//respuesta = false;
    	if (tcpCliente==null) return false;
    	return tcpCliente.isConnected(); 
    }
    
    public Boolean DisConnect()
    {
    	Boolean respuesta;
    	respuesta = true;
    	
    	try {
    		if (tcpCliente!=null) 
    		{	tcpCliente.close();
    		}		
		} catch (IOException e) {
			System.out.println("error al desconectar:"+e.getMessage());
			respuesta = false;
		}
		return respuesta;
    }


    public void readRelays()
    {

        if (tcpCliente.isConnected())
        {
        	try {
				tcpCliente.getOutputStream().write(2);
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
        	
            byte p0, p1;
            p0 = 0;
            p1 = 0;
            try {
				//p0 = (byte)iStm.read();
                //p1 = (byte)iStm.read();
            	p0 = (byte)tcpCliente.getInputStream().read();
            	 try {
					Thread.sleep(500);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
            	p1 = (byte)tcpCliente.getInputStream().read();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
            int stat;
            stat = (p0 + (p1 << 8));
            System.out.println("p0=" + p0 + "  p1=" + p1 + "  stat=" + stat);
            for (int i = 0; i < 16; i++)
            {
                relayStatus[i] = ((stat % 2) == 0);
                stat /= 2;
            }
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
        try {
			this.tcpCliente.getOutputStream().write(message);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
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
