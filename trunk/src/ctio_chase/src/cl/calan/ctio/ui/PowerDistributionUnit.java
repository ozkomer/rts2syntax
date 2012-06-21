/**
 * 
 */
package cl.calan.ctio.ui;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import java.net.URL;
import java.util.Vector;

import javax.swing.ImageIcon;
import javax.swing.JCheckBox;
import javax.swing.JFrame;
import javax.swing.JOptionPane;

import cl.calan.ctio.ArduinoTcp;


/**
 * @author sysop
 *
 */
public class PowerDistributionUnit extends JFrame implements ComponentListener, ActionListener, WindowListener {

	/**
	 * 
	 */
	private static final long serialVersionUID = -1128218932725094281L;
	private final int frameAncho = 583;
	private final int frameAlto = 310;
	private NetworkUi networkUI;
	private RelaysUI relaysUI;
	
	private ArduinoTcp arduinoTCP;
	
	public PowerDistributionUnit ()
	{
		super("Power Distribution Unit");
		this.setPreferredSize(new Dimension(frameAncho, frameAlto));
		//URL recurso;
		//recurso = this.getClass().getResource();
		//this.setIconImage(new ImageIcon("images/Switch.gif").getImage());
		
		this.setIconImage(new ImageIcon("./images/switch.gif").getImage());
		this.addComponentListener(this);
		this.addWindowListener(this);
		this.setResizable(false);
		this.networkUI = new NetworkUi(this);
		this.relaysUI = new RelaysUI(this);
		this.relaysUI.setVisible(false);
		
		FlowLayout layout;
		layout = new FlowLayout();
		this.getContentPane().setLayout(layout);
		this.getContentPane().add(networkUI);
		this.getContentPane().add(relaysUI);
		
		String host;
		int port;
		host = networkUI.getHost();
		port =  networkUI.getPort();
		arduinoTCP = new ArduinoTcp(host, port);
	}
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		PowerDistributionUnit frame;
		frame = new PowerDistributionUnit();
		//frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);		
		frame.pack();
		frame.setVisible(true);
	}

	@Override
	public void componentHidden(ComponentEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void componentMoved(ComponentEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void componentResized(ComponentEvent arg0) {
		System.out.println("size="+this.getSize().toString());
	}

	@Override
	public void componentShown(ComponentEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void actionPerformed(ActionEvent arg0) {
		String accion;
		accion = arg0.getActionCommand();
		System.out.println("accion="+accion);
		if (accion=="Connect") Conectar();
		if (accion=="Switch Off") SwitchOff();
		if (accion=="Switch On") SwitchOn();
		if (accion=="Read") Read();
		
	}
	
	
	private void Conectar()
	{
		System.out.println("Conectar");
		
		String mensajeError;
		mensajeError = arduinoTCP.Connect();
		if (mensajeError==null)
		{
			System.out.println("Conectado exitosamente.");
			this.Read();
			this.relaysUI.setVisible(true);
		}else			
		{
			JOptionPane.showMessageDialog(this,mensajeError,"Error al conectar.",JOptionPane.WARNING_MESSAGE );
		}
	}
	
	private void SwitchOff()
	{
		System.out.println("SwitchOff");
		switchRelays(false);
	}
	
	private void SwitchOn()
	{
		System.out.println("SwitchOn");
		switchRelays(true);
	}
	
	/**
	 * Recorre la lista de reles, formando una lista con aquellos que estan tickeados. 
	 * @param targetState
	 */
    private void switchRelays(Boolean targetState)
    {
    	Vector<Integer> tickeds;
        JCheckBox cbLocal;
        tickeds = new Vector<Integer>();
        StringBuilder message;
        message = new StringBuilder();
        message.append(" Los reles:\n\n");
        for (int i = 0; i < 16; i++)
        {
            cbLocal = relaysUI.getJcbRelay()[i];
            if (cbLocal.isSelected())
            {
                tickeds.add(i);
                //relayStatus[i] = targetState;
                message.append(cbLocal.getText());
                message.append("\n");
            }
        }
        message.append("serán ");
        if (targetState)
        {
            message.append("encendidos.");
            message.append("\n");
        }
        else
        {
            message.append("apagados");
            message.append("\n");
        }
        
        
        
        int respuesta;
        respuesta = JOptionPane.showConfirmDialog(this, message, "Atención", JOptionPane.YES_NO_OPTION);//MessageBox.Show(message.ToString(), "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
        if (respuesta == JOptionPane.YES_OPTION)
        {
        	for (Integer indice : tickeds) 
            {
                System.out.println("relayStatus[" + indice + "]=" + this.arduinoTCP.getRelayStatus()[indice]+ " ---> " + targetState);
                // Se actualiza la interfaz para que ningun checkBox permanezca tickeado despues mover los switches
                this.relaysUI.getJcbRelay()[indice].setSelected(false);//relayCheckBox[indice].Checked = false;
                // Se actualizan los estados en el arduino, pero no aun en el arreglo de relays
                Boolean[] tmpRelayStatus;
                tmpRelayStatus = this.arduinoTCP.getRelayStatus();
                tmpRelayStatus[indice]=targetState;
                this.arduinoTCP.setRelayStatus(tmpRelayStatus);
            }
            refreshcheckBoxRelayColors();
            // Ahora si se actualiza el arreglo de Relays.
            this.arduinoTCP.refreshPorts();
        }
    }

	
	private void Read()
	{
		System.out.println("Read");
        this.arduinoTCP.readRelays();
        this.refreshcheckBoxRelayColors();
	}

	private void refreshcheckBoxRelayColors() {
        JCheckBox boton;
        JCheckBox[] jcbArray;
        jcbArray = relaysUI.getJcbRelay();
        
        for (int i = 0; i < 16; i++)
        {
            boton = jcbArray[i];
            
            if (this.arduinoTCP.getRelayStatus()[i])
            {
                boton.setBackground(Color.GREEN);
            }
            else
            {
            	boton.setBackground(Color.PINK);
            }
            
            if (!this.arduinoTCP.IsConnected())
            {
                boton.setEnabled( false );
            }
        }
	}

	@Override
	public void windowActivated(WindowEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void windowClosed(WindowEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void windowClosing(WindowEvent arg0) {
		if (arduinoTCP.DisConnect())
		{
			System.out.println("Desconectado exitosamente.");
		}
		System.exit(0);		
	}

	@Override
	public void windowDeactivated(WindowEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void windowDeiconified(WindowEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void windowIconified(WindowEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void windowOpened(WindowEvent arg0) {
		// TODO Auto-generated method stub
		
	}
	

}
