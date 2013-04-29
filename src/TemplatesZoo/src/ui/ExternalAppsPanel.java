/**
 * 
 */
package ui;

import java.awt.Cursor;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextField;

import util.ExecCommand;
import util.Misc;
import util.Parametros;

/**
 * @author sysop
 *
 */
public class ExternalAppsPanel extends JPanel implements ActionListener, MouseListener{

	/**
	 * 
	 */
	private static final long serialVersionUID = 8654588947730293290L;
	private JCheckBox cbDs9Path;
	private JTextField tfDs9Path;
	private JButton bGuardar;
	private JButton bTestDs9;

	private JCheckBox cbWgetPath;
	private JTextField tfWgetPath;

	private JButton bTestWget;

	public ExternalAppsPanel ()
	{
		this.setBorder(BorderFactory.createTitledBorder("External Applications"));
		this.setEnabled(false);

		cbDs9Path = new JCheckBox("Ds9 Path");
		tfDs9Path = new JTextField();
		bTestDs9 = new JButton("Test Ds9");

		cbWgetPath = new JCheckBox("Wget Path");
		tfWgetPath = new JTextField();
		bTestWget = new JButton ("Test Wget");

		bGuardar = new JButton("Guardar");		

		this.presetFields();
		this.addListeners();
		this.addComponents();
	}

	private void addListeners() {
		this.bGuardar.addActionListener(this);
		this.bTestDs9.addActionListener(this);		
		this.bTestWget.addActionListener(this);
		this.cbDs9Path.addMouseListener(this);
		this.cbWgetPath.addMouseListener(this);
	}

	private void addComponents() {
		this.setLayout(new GridBagLayout());

		int fila; fila= 0;
		GridBagConstraints c;
		c = new GridBagConstraints();

//		c.fill = GridBagConstraints.HORIZONTAL;
//		c.gridx = 0; c.gridy = fila;
//		this.add(lblDescargarDs9,c);
//
//		c.fill = GridBagConstraints.HORIZONTAL;
//		c.gridx = 2; c.gridy = fila;
//		this.add(lblDescargarWget,c);

		fila++;
		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 0; c.gridy = fila;
		this.add(cbDs9Path,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.weightx = 1;
		c.gridx = 1; c.gridy = fila;		
		this.add(tfDs9Path,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.weightx = 0.5;
		c.gridx = 2; c.gridy = fila;		
		this.add(bTestDs9,c);

		fila++;
		c.fill = GridBagConstraints.HORIZONTAL;
		c.weightx = 0.0;
		c.gridx = 0; c.gridy = fila;		
		this.add(cbWgetPath,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.weightx = 1.0;
		c.gridx = 1; c.gridy = fila;	
		this.add(tfWgetPath,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.weightx = 0.5;
		c.gridx = 2; c.gridy = fila;	
		this.add(bTestWget,c);

		fila++;
		c.fill = GridBagConstraints.HORIZONTAL;
		c.weightx = 0.5;
		c.gridx = 0; c.gridy = fila;
		c.gridwidth = 3;
		this.add(bGuardar,c);
	}

	private void presetFields() {
		this.cbDs9Path.setEnabled(false);
		this.cbWgetPath.setEnabled(false);
		this.cbDs9Path.setSelected(FitsZoo.params.get(Parametros.SETUP_OK).charAt(0)=='1');
		this.cbWgetPath.setSelected(FitsZoo.params.get(Parametros.SETUP_OK).charAt(1)=='1');

		this.tfDs9Path.setText(FitsZoo.params.get(Parametros.DS9_PATH));	
		this.tfWgetPath.setText(FitsZoo.params.get(Parametros.WGET_PATH));
	}

	public void actionPerformed(ActionEvent arg0) {
		Object source;
		source = arg0.getSource();
		if (source.equals(this.bTestDs9))
		{
			System.out.println("Test DS9");	
			testDS9();
		}
		if (source.equals(this.bTestWget))
		{
			System.out.println("Test Wget.");	
			testWget();
		}

		if (source.equals(this.bGuardar))
		{		
			StringBuilder status;
			status =new StringBuilder();
			if (this.cbDs9Path.isSelected())
			{
				status.append('1');
			}else
			{
				status.append('0');
			}
			if (this.cbWgetPath.isSelected())
			{
				status.append('1');
			}else
			{
				status.append('0');
			}			
			FitsZoo.params.put(Parametros.SETUP_OK, status.toString());
			FitsZoo.params.put(Parametros.DS9_PATH, this.tfDs9Path.getText().replace('\\', '/'));
			FitsZoo.params.put(Parametros.WGET_PATH, this.tfWgetPath.getText().replace('\\', '/'));			
		}

	}


	/**
	 * Ejecuta la aplicacion DS9
	 * Recordar que esto se puede mejorar usando los parametros ds9:
	 * 
	 * para abrir desde archivo:
	 * -file archivo.fits
	 * 
	 * para abrir archivo remoto
	 * 
	 * -url http://zwicky_candidatos.das.uchile.cl/templates/F00003.20120906_063419_1.fits
	 * -zoom to fit -zscale
	 * @return
	 */
	public int testDS9()
	{
		int exitValue=-1;
		ExecCommand execKommand;
		execKommand = new ExecCommand(this.tfDs9Path.getText());
		System.out.println(execKommand.getOutput());
		System.err.println(execKommand.getError());
		exitValue = execKommand.getExitValue();
//		exitValue = Misc.ejecutaComando(this.tfDs9Path.getText());
		feedBackTestDS9(exitValue);
		return exitValue;
	}

	public int testWget()
	{
		int exitValue=-1;
		ExecCommand execKommand;
		execKommand = new ExecCommand(this.tfWgetPath.getText()+" --continue http://zwicky.das.uchile.cl/pictures/chase500night.JPG");
		System.out.println(execKommand.getOutput());
		System.err.println(execKommand.getError());
		exitValue = execKommand.getExitValue();
		//exitValue = Misc.ejecutaComando(this.tfWgetPath.getText()+" http://zwicky.das.uchile.cl/pictures/chase500night.JPG");
		feedBackTestWget(exitValue);

		return exitValue;
	}

	private void DialogoAdvertencia (String body,String title,int exitvalue)
	{
		int messageType;
		messageType = JOptionPane.WARNING_MESSAGE;
		if (exitvalue==0) messageType=JOptionPane.INFORMATION_MESSAGE;
		JOptionPane.showConfirmDialog(this, body ,title, JOptionPane.DEFAULT_OPTION ,messageType);		
	}

	private void feedBackTestDS9 (int exitValue)
	{	
		String titulo;
		titulo = "DS9";
		if (exitValue==0) 
		{
			this.cbDs9Path.setSelected(true);
			DialogoAdvertencia("DS9 Ok.",titulo , exitValue);
		}
		else
		{
			DialogoAdvertencia("Al Parecer la aplicación DS9 ha fallado. Revise si el ejecutable indicado es el correcto.",titulo , exitValue);
		}
	}

	private void feedBackTestWget (int exitValue)
	{
		String titulo;
		titulo = "wget";
		if (exitValue==0) 
		{
			this.cbWgetPath.setSelected(true);
			DialogoAdvertencia("wget Ok.",titulo , exitValue);
		}
		else
		{
			DialogoAdvertencia("Al Parecer la aplicación wget ha fallado. Revise si el ejecutable indicado es el correcto.",titulo, exitValue);
		}
	}

	@Override
	public void mouseClicked(MouseEvent evt) {
		Object source;
		source = evt.getSource();
		if (source.equals(this.cbDs9Path))
		{
			Misc.AbrirNavegadorDefault("http://hea-www.harvard.edu/RD/ds9/site/Download.html");
		}
		if (source.equals(this.cbWgetPath))
		{
			Misc.AbrirNavegadorDefault("http://gnuwin32.sourceforge.net/packages/wget.htm");
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
