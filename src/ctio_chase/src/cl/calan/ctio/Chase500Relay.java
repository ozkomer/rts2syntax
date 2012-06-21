/**
 * 
 */
package cl.calan.ctio;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

/**
 * @author sysop
 *
 */
public class Chase500Relay extends Thread {
 
    protected DatagramSocket socket = null;
    private double cwAngle;
    private double zenithAngle;
    private byte[] buf;
    private DatagramPacket packet;
    private StringBuilder mensaje;

    
    public Chase500Relay() throws IOException {
        this("QuoteServerThread");
    }
 
    public Chase500Relay(String name) throws IOException {
        super(name);
        buf = new byte[100];
        socket = new DatagramSocket(19000);
        cwAngle = 0;
        zenithAngle = 0;
    }
    
    private void refreshMessage()
    {
        // figure out response
    	String updateInfo;
    	updateInfo = (new String( packet.getData())).trim();
    	System.out.println("updateInfo=\t"+updateInfo);
    	updateInfo = updateInfo.replace("#", "");
    	String[] part;
    	part = updateInfo.split(" ");
    	this.cwAngle = Double.parseDouble(part[0]);
    	this.zenithAngle = Double.parseDouble(part[1]);
        mensaje = new StringBuilder();
        mensaje.append("_");
        mensaje.append(this.cwAngle);
        mensaje.append(" ");
        mensaje.append(this.zenithAngle);
        mensaje.append("_");
        System.out.println("nuevoMensaje=\t"+mensaje.toString());
        buf = new byte[100];
        this.buf = mensaje.toString().getBytes();
    }
 
    public void run() {
 
    	boolean continuar;
    	continuar = true;
    	System.out.println("Iniciando Servidor");
        while (continuar) {
            try { 
                // receive request
            	buf = new byte[100];
                this.packet = new DatagramPacket(buf, buf.length);
                socket.receive(packet);
                
                if (packet.getAddress().getHostAddress().equals("139.229.12.76"))//(packet.getLength()>1) // Si el mensaje viene de Chase500
                {
                 //System.out.println("Chase 500: IP="+packet.getAddress().getCanonicalHostName());
                 this.refreshMessage();
                }
                else //Si no, se envia el mensaje al cliente de la aplicacion 3D
                {
                    // send the response to the client at "address" and "port"
                    InetAddress address = packet.getAddress();
                    int port = packet.getPort();
                    buf = new byte[100];
                    this.buf = mensaje.toString().getBytes();
                    packet = new DatagramPacket(buf, buf.length, address, port);
                    socket.send(packet);
                    System.out.print(".");
                }
            } catch (IOException e) {
                e.printStackTrace();
            
            }
        }
        socket.close();
    }
    
    public static void main(String[] args) throws IOException {
        new Chase500Relay().start();
    }
}