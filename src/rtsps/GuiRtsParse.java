/**
 * 
 */
package rtsps;

import java.awt.Color;
import java.awt.Container;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowEvent;
import java.util.Vector;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;

/**
 * @author eduardo
 *
 */
public class GuiRtsParse extends JFrame implements ActionListener{

	/**
	 * 
	 */
	private static final long serialVersionUID = 6918521189718810309L;
	private final int frameAncho =800;
	private final int frameAlto = 600;
	//private JTabbedPane jtabbed;
	private JButton jbProcesar;

	private JPanel jpInput;
	private JTextArea jtInput;

	private JPanel jpNewTarget;
	private JTextArea jtNewTarget;


	private JPanel jpTarget;
	private JTextArea jtTarget;

	private JPanel jpVisibilidad;
	private JTextArea jtVisibilidad;


	public GuiRtsParse() {
		this.setPreferredSize(new Dimension(frameAncho, frameAlto));
		Container contenedor;
		contenedor = this.getContentPane();		
		//this.jtabbed =  new JTabbedPane();
		jpInput = new JPanel();
		jpNewTarget = new JPanel();
		jpTarget = new JPanel();
		jpVisibilidad = new JPanel();
		
		jpInput.setBackground(Color.MAGENTA);
		jpNewTarget.setBackground(Color.LIGHT_GRAY);
		jpTarget.setBackground(Color.WHITE);
		jpVisibilidad.setBackground(Color.ORANGE);
		
		//jtabbed.addTab("input", jpInput);
		//jtabbed.addTab("newTarget", jpNewTarget);
		JScrollPane jspInput;
		JScrollPane jspNewTarget;
		JScrollPane jspTarget;
		JScrollPane jspVisibilidad;
		

		jtInput = new JTextArea();
		jtInput.setToolTipText("Ingrese los requests aquí.");
		jspInput = new JScrollPane(jtInput);
		
		jtNewTarget = new JTextArea();
		jtNewTarget.setToolTipText("Aqui estarán las sentencias rts-newtarget.");
		jspNewTarget = new JScrollPane(jtNewTarget);
		jspNewTarget.setPreferredSize(new Dimension(600, 130));
		jpNewTarget.add(jspNewTarget);
		
		jtTarget = new JTextArea();
		jtTarget.setToolTipText("Aqui estarán las sentencias rts-target.");
		jspTarget = new JScrollPane (jtTarget);
		jspTarget.setPreferredSize(new Dimension(600, 130));
		jpTarget.add(jspTarget);
		
		jtVisibilidad = new JTextArea();
		jtVisibilidad.setToolTipText("Aqui estarán las sentencias para: http://catserver.ing.iac.es/staralt/index.php");
		jspVisibilidad = new JScrollPane(jtVisibilidad);
		jspVisibilidad.setPreferredSize(new Dimension(600, 130));
		jpVisibilidad.add(jspVisibilidad);
		
		jtInput.setText("");
		jtInput.append("sn2011hs         22 57 11.77 -43 23 04.8   BVu'g'R'i'z' B=15x60 V=11x60 u'=25x60 g'=r'=i'=11x60 z'=15x60\n");
		jtInput.append("sn2011iv  03 38 51.35 -35 35 32.0  BVu'g'r'i'Z  B=3x60 u=5x60 V=g'=i'=r'=3x60 Z=5x60\n");
		jtInput.append("tphe0000       00 30 15.8 -46 30 02 BV B=3x30 V=3x30\n");
		jtInput.append("Rubin149       07 24 14.0 -00 31 38.0   BVu'g'r'i'z' B=3x30 V=3x30 u'=3x60 g'=r'=i'=3x30 z'=3x60\n");
		jtInput.append("sn2011ir 11 48 00.32 04 29 47.1   B=15x60 V=g'=r'=i'=11x60\n");


		//jspInput.setPreferredSize(new Dimension(700, 500));
		jbProcesar = new JButton("Procesar");
		jbProcesar.setActionCommand("jbProcesar");
		jbProcesar.addActionListener(this);
		jpInput.add(jspInput);
		jpInput.add(jbProcesar);
		contenedor.add(jpInput);		
		jpInput.add(jpNewTarget);
		jpInput.add(jpTarget);
		jpInput.add(jpVisibilidad);
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		GuiRtsParse frame;
		frame = new GuiRtsParse();
		//frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.addWindowListener(new java.awt.event.WindowAdapter(){
			public void windowClosing(WindowEvent e){
				System.exit(0);
			}
		}

		);

		frame.pack();
		frame.setVisible(true);
	}

	@Override
	public void actionPerformed(ActionEvent arg0) {
		System.out.println(arg0.getActionCommand());
		if (arg0.getActionCommand().equals("jbProcesar"))
		{
			this.jtNewTarget.setText("");		
			this.jtTarget.setText("");
			this.jtVisibilidad.setText("");

			test2(this.jtInput.getText());
		}
	}

	private boolean test2(String texto) {
		Vector<String> request;
		Vector<Propuesta> vprop;
		request = new Vector<String>();
		vprop = new Vector<Propuesta>();
		String[] textoProcesar;
		textoProcesar = texto.split("\n");
		request = new Vector<String>();
		for (int i=0;i< textoProcesar.length;i++)
		{
			request.add(textoProcesar[i]);
		}


		for (int i=0;i<request.size();i++)
		{
			Propuesta prop;
			String linea;
			linea = request.elementAt(i);
			StringBuilder mensaje;
			mensaje = new StringBuilder();
			mensaje.append("Procesando Línea:\n");
			mensaje.append(linea);
			prop = null;
			try{
				prop = new Propuesta(linea);
			} catch (NumberFormatException e) {
				mensaje.append("\nError al intentar convertir en un entero. "+e.getMessage());
				JOptionPane.showConfirmDialog(this, mensaje.toString(), "Error", JOptionPane.WARNING_MESSAGE);
				return false;
			}
			vprop.add(prop);
		}
		for (int i=0;i<vprop.size();i++)
		{
			Propuesta prop;
			prop = vprop.elementAt(i);

			System.out.println("---------------------------------");
			System.out.println("prop.name="+prop.getName());
			System.out.println("RA="+prop.getRadec().getRa());
			System.out.println("DEC="+prop.getRadec().getDec());
			System.out.println("Filters Header="+prop.getFilterH());
		}

		System.out.println("---------------comandos rts2-newtarget------------------");
		for (int i=0;i<vprop.size();i++)
		{
			String nuevaLinea;
			Propuesta prop;
			prop = vprop.elementAt(i);
			nuevaLinea = prop.comandoNewTarget();
			this.jtNewTarget.append(nuevaLinea);
			this.jtNewTarget.append("\n");
			System.out.println(nuevaLinea);
		}
		System.out.println("---------------comandos rts2-target------------------");
		for (int i=0;i<vprop.size();i++)
		{
			String nuevaLinea;			
			Propuesta prop;
			prop = vprop.elementAt(i);
			nuevaLinea = prop.comandoTarget();
			this.jtTarget.append(nuevaLinea);
			this.jtTarget.append("\n");
			

			System.out.println(nuevaLinea);
		}
		System.out.println("---------------comandos CalculoVisibilidad------------------");
		for (int i=0;i<vprop.size();i++)
		{
			String nuevaLinea;
			Propuesta prop;
			prop = vprop.elementAt(i);
			nuevaLinea = prop.getCalculoVisibilidad();
			this.jtVisibilidad.append(nuevaLinea);
			this.jtVisibilidad.append("\n");
			System.out.println(nuevaLinea);
		}
		return true;
	}
}
