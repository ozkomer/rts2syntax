/**
 * 
 */
package ui;

import java.awt.Color;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;

import javax.imageio.ImageIO;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JSlider;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

import util.Misc;
import zoo.TemplateCycle;

/**
 * Modela las actividades que es necesario realizar sobre un template.
 * A partir del status del objeto TemplateCycle, se van activando o desactivando las componentes de esta interfaz
 * a fin de:
 * Examinar con DS9
 * Evaluar el Archivo fits.
 * Ingresar la evaluacion a la base de datos
 * desbloquear el archivo de la base de datos.
 * @author sysop
 *
 */
public class PanelTemplate extends JPanel implements ActionListener, ChangeListener, MouseListener {

	/**
	 * 
	 */
	private static final long serialVersionUID = -6109939096702733577L;
	private JButton bExaminar;
	private JLabel lblDigitalSky; // Aqui se despliega el jpg del sitio: http://www.das.uchile.cl/~cata500/images/jpg/
	private JLabel lblPuntaje;
	private JSlider sldPuntaje;
	private JCheckBox chkRestaCorrecta;
	private TemplateCycle template;
	private JButton bSubmit;

	public PanelTemplate (TemplateCycle template)
	{

		this.template = template;
		if (this.template!=null)
		{
			this.setBorder(BorderFactory.createTitledBorder("Template id="+this.template.getTemplateID()));
		}

		this.bExaminar = new JButton ("Examinar");

		this.lblPuntaje = new JLabel ("Calidad Imagen",JLabel.CENTER);

		this.sldPuntaje = new JSlider(1, 10,1);
		this.sldPuntaje.setMinorTickSpacing(1);
		this.sldPuntaje.setPaintTicks(true);
		this.sldPuntaje.setPaintLabels(true);
		java.util.Hashtable<Integer, JLabel> labelTable;
		labelTable = new java.util.Hashtable<Integer, JLabel> ();

		labelTable.put(new Integer(1), new JLabel("No Sirve"));
		labelTable.put(new Integer(10), new JLabel("Sirve"));
		this.sldPuntaje.setLabelTable(labelTable);
		this.sldPuntaje.setPaintLabels(true);
		

		lblDigitalSky = new JLabel (this.getDigitalSkyImage());
		Dimension dimensionImagenJpg;
		dimensionImagenJpg = new Dimension(640, 640);
		lblDigitalSky.setMinimumSize(dimensionImagenJpg);
		lblDigitalSky.setPreferredSize(dimensionImagenJpg);

		this.chkRestaCorrecta = new JCheckBox("Sustracción Correcta", true);
		this.bSubmit = new JButton("Submit");
		
		this.setBackground(Color.WHITE);
		if (template!=null)
		{
			this.addListeners();
			this.addComponents();
			this.refreshEnableds();
		}
		
		
	}

	private ImageIcon getDigitalSkyImage ()
	{
		URL url;
		url = null;
		String strUrl;
		if (this.template!=null)
		{
			strUrl = this.template.getJpgURL();	
		}else
		{
			strUrl = "http://zwicky.das.uchile.cl/pictures/chase500night.JPG";	
		}
		
		try {
			url = new URL(strUrl);
		} catch (MalformedURLException e) {
			System.err.println("Error Creando URL='"+strUrl+"'");
			e.printStackTrace();
		}
		BufferedImage image;
		image = null;
		
		try {
			System.out.println("Digital Sky url:"+url);
			image = ImageIO.read(url);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		ImageIcon respuesta;
		respuesta = null;
		try {
		respuesta = new ImageIcon(image);
		} catch (NullPointerException e)
		{
			try {
				respuesta = new ImageIcon (ImageIO.read(new URL("http://www.yourway.lk/Images/R15/R151.jpg")));
			} catch (MalformedURLException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (IOException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		}
		return respuesta;
	}
	
	/**
	 * Actualiza las componentes de la interfaz que deben estar habilitadas segun el status del objeto TemplateCycle
	 */
	private void refreshEnableds ()
	{	
		int status;
		status = this.template.getStatus();
		switch (status) {
		case TemplateCycle.STATUS_WGET_DOWN:
			this.bExaminar.setEnabled(true);
			this.lblPuntaje.setEnabled(false);
			this.sldPuntaje.setEnabled(false);
			this.chkRestaCorrecta.setEnabled(false);
			this.bSubmit.setEnabled(false);
			break;
		case TemplateCycle.STATUS_DS9_DOWN:
			this.bExaminar.setEnabled(true);
			this.lblPuntaje.setEnabled(true);
			this.sldPuntaje.setEnabled(true);
			this.chkRestaCorrecta.setEnabled(true);
			this.bSubmit.setEnabled(false);
			break;
		case TemplateCycle.STATUS_EVALUATE_DOWN:
			this.bExaminar.setEnabled(false);
			this.lblPuntaje.setEnabled(false);
			this.sldPuntaje.setEnabled(false);
			this.chkRestaCorrecta.setEnabled(false);
			this.bSubmit.setEnabled(false);
			break;
		default:
			this.bExaminar.setEnabled(false);
			this.lblPuntaje.setEnabled(false);
			this.sldPuntaje.setEnabled(false);
			this.chkRestaCorrecta.setEnabled(false);
			this.bSubmit.setEnabled(false);
			break;
		}
	}

	private void addListeners() {
		this.bExaminar.addActionListener(this);		
		this.sldPuntaje.addChangeListener(this);
		this.bSubmit.addActionListener(this);
		this.chkRestaCorrecta.addActionListener(this);
		this.lblDigitalSky.addMouseListener(this);
	}

	private void addComponents() {
		this.setLayout(new GridBagLayout());

		GridBagConstraints c = new GridBagConstraints();

		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 0;	c.gridy = 0;
		c.weightx = 0.5;	c.weighty =  0.5;
		c.gridwidth=1;		c.gridheight = 1;
		this.add(bExaminar,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 1;	c.gridy = 0;
		c.weightx = 0.5;	c.weighty =  0.5;
		c.gridwidth=1;		c.gridheight = 1;
		this.add(lblPuntaje,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 2;	c.gridy = 0;
		c.weightx = 1.0;	c.weighty =  0.5;
		c.gridwidth=1;		c.gridheight = 1;
		this.add(sldPuntaje,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 1;	c.gridy = 1;
		c.weightx = 0.5;	c.weighty =  0.5;
		c.gridwidth=1;		c.gridheight = 1;
		this.add(chkRestaCorrecta,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 2;	c.gridy = 1;
		c.weightx = 0.1;	c.weighty =  0.1;
		c.gridwidth=1;		c.gridheight = 1;
		this.add(bSubmit,c);
		
		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 0;	c.gridy = 2;
		c.weightx = 1;	c.weighty = 1;
		c.gridwidth = 3;	c.gridheight = 3;
		this.add(this.lblDigitalSky,c);
	}

	public void actionPerformed(ActionEvent evt) {
		Object source;
		source = evt.getSource();
		if (source.equals(this.bExaminar))
		{			
			this.template.AbrirDS9();
			this.refreshEnableds();
		}
		if (source.equals(this.bSubmit))
		{
			this.template.setRestaCorrecta(this.chkRestaCorrecta.isSelected());
			procesaSubmit();
			this.refreshEnableds();
		}		
		if (source.equals(this.chkRestaCorrecta))
		{
			this.template.setRestaCorrecta(this.chkRestaCorrecta.isSelected());	
			this.bSubmit.setEnabled(true);
		}
	}

	/**
	 * Procesa las acciones a realizar cuando se presiona el boton "submit".
	 */
	public void procesaSubmit()
	{
		Boolean submitOK;
		submitOK = false;
		String body;
		String title;
		int tipoMensaje;
		title = "Submit";
		body = "Ha ocurrido un error al intentar ingresar sus datos la base de datos";			
		if ( this.template.insertDbOpinion() )
		{
			if ( this.template.updateDbUnlock() )
			{
				body = "Muchas gracias, su evaluación ha sido correctamente ingresada a la base de datos.";			
				submitOK = true;
			}
		}
		if (submitOK) 
		{ 
			this.refreshEnableds();
			tipoMensaje = JOptionPane.INFORMATION_MESSAGE; 
			JOptionPane.showConfirmDialog(this, body ,title, JOptionPane.DEFAULT_OPTION , tipoMensaje);
			this.setBackground(Color.GREEN); // Esto gatillara un evento en el LockedFitsPanel
		} else { tipoMensaje = JOptionPane.WARNING_MESSAGE;
			JOptionPane.showConfirmDialog(this, body ,title, JOptionPane.DEFAULT_OPTION , tipoMensaje);
		}
		
	}

	@Override
	public void stateChanged(ChangeEvent evt) {
		Object source;
		source = evt.getSource();
		if (source.equals(this.sldPuntaje))
		{
			int puntaje;
			//			this.template.setStatus(TemplateCycle.STATUS_EVALUATE_DOWN);
			puntaje = this.sldPuntaje.getValue();
			this.template.setTemplateQuality(puntaje);
			this.bSubmit.setEnabled(true);
			String labelText;
			labelText = ("Calidad Imagen="+this.template.getTemplateQuality());
			this.lblPuntaje.setText(labelText);			
		}
	}

	@Override
	public void mouseClicked(MouseEvent evt) {
		Object source;
		source = evt.getSource();
		if (source.equals(this.lblDigitalSky))
		{
			Misc.AbrirNavegadorDefault(this.template.getJpgURL());
		}
	}

	@Override
	public void mouseEntered(MouseEvent arg0) {
		this.getRootPane().setCursor(Cursor.getPredefinedCursor(Cursor.HAND_CURSOR));
		
	}

	@Override
	public void mouseExited(MouseEvent arg0) {
		this.getRootPane().setCursor(Cursor.getPredefinedCursor(Cursor.DEFAULT_CURSOR));
	}

	@Override
	public void mousePressed(MouseEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void mouseReleased(MouseEvent arg0) {
		// TODO Auto-generated method stub
		
	}


}
