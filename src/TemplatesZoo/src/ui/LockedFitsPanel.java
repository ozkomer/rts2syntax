/**
 * 
 */
package ui;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Vector;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JSplitPane;
import javax.swing.JTextArea;

import zoo.TemplateCycle;
import database.ZwickyDB;

/**
 * @author sysop
 *
 */
public class LockedFitsPanel extends JPanel implements ActionListener, PropertyChangeListener, ComponentListener {

	/**
	 * 
	 */
	private static final long serialVersionUID = 7181754066241018378L;

	private JTextArea taResumen;
	private JButton bDescargar;
	private JButton bDesbloquear;
	private JComboBox cbDesbloquear;

	private JPanel panelDesbloquear;
	private PanelTemplate panelTemplate;

	private JScrollPane				scrollTopPanel;
	private JScrollPane				scrollBottomPanel;
	private JSplitPane				splitPane;
	
	private JPanel panelUP;
	private JPanel panelDown;

	private static final String msgRESUMEN = "Ud tiene ? archivos fits por revisar.\n" +
	"_XX deben ser descargados.";
	private java.sql.PreparedStatement statement = null;

	/**
	 * Lista de archivos bloqueados en la base de datos. Siempre refiriendonos al usuario actual.
	 */
	private Vector<TemplateCycle> lockedTemplates;

	/**
	 * Lista de archivos bloqueados en la base de datos. Siempre refiriendonos al usuario actual.
	 */
	private Vector<TemplateCycle> toDownloadTemplates;

	public LockedFitsPanel ()
	{		

		this.panelUP = new JPanel();
		this.panelUP.setMinimumSize(new Dimension(698,170));
		this.panelUP.setBorder(BorderFactory.createTitledBorder("Locked Fits"));
		this.panelDown = new JPanel();

		//this.panelUP.setMinimumSize(new Dimension(400,100));
		this.panelTemplate = new PanelTemplate(null);
		this.panelDesbloquear = new JPanel();
		this.llenaPanelDesbloquear();
		//this.setEnabled(false);
		this.taResumen = new JTextArea(msgRESUMEN);
		this.bDescargar = new JButton("Download All");
		this.bDescargar.setEnabled(false);
		
		this.scrollBottomPanel = new JScrollPane(panelDown);
		//this.scrollBottomPanel.setMinimumSize(new Dimension(200,400));
		//this.scrollBottomPanel.add();
		
		this.scrollTopPanel = new JScrollPane(panelUP);
		this.scrollTopPanel.addComponentListener(this);
//		this.scrollTopPanel.setMinimumSize(new Dimension(698,170));
		this.addListeners();
		this.addPanelUpComponents();
		//this.scrollTopPanel.add(panelUP);
		this.splitPane = new JSplitPane(JSplitPane.VERTICAL_SPLIT, scrollTopPanel, scrollBottomPanel);
		//this.splitPane.setMinimumSize(new Dimension(300, 500));
		this.splitPane.setOneTouchExpandable(true);
		this.splitPane.setDividerLocation(111);
		this.splitPane.setResizeWeight(0);
		
		this.splitPane.setPreferredSize(new Dimension(700,866));
		
		toDownloadTemplates = null;
		this.add(splitPane);
	}

	private void addListeners() {
		this.bDescargar.addActionListener(this);
		this.bDesbloquear.addActionListener(this);
		
	}

	private void addPanelUpComponents ()
	{
		this.panelUP.setLayout(new GridBagLayout());
		GridBagConstraints c = new GridBagConstraints();

		c.fill = GridBagConstraints.BOTH;
		c.gridx = 0;	c.gridy = 0;
		c.weightx = 0.5;	c.weighty = 1.0;
		c.gridwidth=1;		c.gridheight = 3;
		this.panelUP.add(taResumen,c);

		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 1;	c.gridy = 0;
		c.weightx = 0.5;	c.weighty = 0.5;
		c.gridwidth=1;		c.gridheight = 1;		
		this.panelUP.add(panelDesbloquear,c);
		
		c.fill = GridBagConstraints.HORIZONTAL;
		c.gridx = 1;	c.gridy = 2;
		c.weightx = 0.5;	c.weighty = 0.5;
		c.gridwidth=1;		c.gridheight = 1;
		this.panelUP.add(bDescargar,c);		
		
		
	}
	
	private void llenaPanelDesbloquear()
	{
		this.panelDesbloquear.setBorder(BorderFactory.createTitledBorder("#Archivos a Desbloquear:"));
	
			this.bDesbloquear = new JButton("Desbloquear");
			
			this.cbDesbloquear = new JComboBox();
			for (int i=0;i<8;i++)
			{
				this.cbDesbloquear.addItem(""+(int)Math.pow(2, i));
			}
			this.cbDesbloquear.setSelectedIndex(2);
			this.bDesbloquear.setEnabled(false);

			this.panelDesbloquear.add(cbDesbloquear);
			this.panelDesbloquear.add(bDesbloquear);
			
	}

	/**
	 * A partir del userID obtenido durante el login, obtiene la lista de archivos bloqueados.
	 * Luego se obtiene la lista de archivos que es necesario descargar.
	 */
	public void refresh ()
	{
		String query;
		query = "SELECT templates.id, size, evaluations, plans.description " +
				"FROM templates, observations, plans " +
				"WHERE blockinguser_id=NN AND " +
				"templates.field = observations.description AND " +
				"observations.plan_id = plans.id";
		query = query.replace("NN", ""+FitsZoo.getUser_id());
		System.out.println("query="+query);
		FitsZoo.zwickyDB.conectar();
		try {

			statement = FitsZoo.zwickyDB.getConnect().prepareStatement	(
					query.toString(),
					ResultSet.TYPE_FORWARD_ONLY, 
					ResultSet.CONCUR_READ_ONLY		);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		try {
			statement.setFetchSize(Integer.MIN_VALUE);
		} catch (SQLException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
		}
		this.lockedTemplates = new Vector<TemplateCycle>();

		ResultSet resultSet;
		try {
			resultSet = statement.executeQuery();
			int template_id;
			int fileSize;
			int evaluations;
			String jpgURL;
			while (resultSet.next())			
			{				
				template_id = resultSet.getInt(1);
				fileSize = resultSet.getInt(2);
				evaluations = resultSet.getInt(3);	
				jpgURL = resultSet.getString(4);
				this.lockedTemplates.add(new TemplateCycle(template_id, fileSize, evaluations, jpgURL));	
				System.out.println("bloqueo en template_id="+template_id);
			}
			resultSet.close();
			FitsZoo.zwickyDB.close();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		refreshResumen ();
		this.MuestraSiguienteTemplate();
	}

	/**
	 * Actualiza el textarea que indica:
	 * Cuantos archivos están bloqueados y cuantos de estos se deben descargar.
	 * 
	 */
	private void refreshResumen ()
	{
		int archivosPorDescargar;
		this.refreshToDownloadTemplates();
		archivosPorDescargar = this.toDownloadTemplates.size();
		
		String resumen;
		resumen = msgRESUMEN.replace("?", ""+this.lockedTemplates.size());
		resumen = resumen.replaceFirst("_XX", ""+archivosPorDescargar);
		taResumen.setText(resumen);
		if (archivosPorDescargar>0) {
			this.bDescargar.setEnabled(true);
		}else 
		{
			this.bDescargar.setEnabled(false);
		}
		if (this.lockedTemplates.size() ==0 )
		{
			String body;
			String title;
			body = "Para continuar revisando templates, ud debe solicitar algunos templates tanto a la base de datos" +
					" como al repositorio.\n\n" +
					"Al solicitar un template en la base de datos, este queda bloqueado hasta que ud. termine de evaluarlo." +
					"\n\n" +
					"Este mecanismo asegura también que ningún otro usuario revise el mismo template que ud está revisando." +
					"\n\n" +
					"A Continuación, por favor escoja cuantos templates desea solicitar (bloquear).";
			title = "Solicitar Templates";			
			JOptionPane.showConfirmDialog(this, body ,title, JOptionPane.DEFAULT_OPTION ,JOptionPane.INFORMATION_MESSAGE);
			this.bDesbloquear.setEnabled(true);
			this.bDescargar.setEnabled(false);
			this.splitPane.setDividerLocation(180);
		}else
		{
			this.bDesbloquear.setEnabled(false);
		}
	}
	
	/**
	 * Actualiza la lista de templates por descargar.
	 */
	private void refreshToDownloadTemplates ()
	{
		this.toDownloadTemplates = new Vector<TemplateCycle>();
		for (TemplateCycle fits : lockedTemplates) {
			if (fits.getStatus()==TemplateCycle.STATUS_LOCKED)
			{
				this.toDownloadTemplates.add(fits);
			}
		}
	}

	public void actionPerformed(ActionEvent evt) {
		Object source;
		source = evt.getSource();
		if (source.equals(this.bDescargar))
		{
			this.bDescargar.setEnabled(false);
			this.descargarTodo ();
			//this.splitPane.setDividerLocation(0);
			this.refresh();
			this.bDescargar.setEnabled(true);
		}
		if (source.equals(this.bDesbloquear))
		{
			int cantidadSolicitada;
			cantidadSolicitada = Integer.parseInt((String)this.cbDesbloquear.getSelectedItem());
			System.out.println("cantidadSolicitada="+cantidadSolicitada);
			this.desbloqueaDB(cantidadSolicitada);
			this.refresh();
		}
	}
	
	/**
	 * Envia un comando update a la base de datos.
	 * Este comando afecta a la tabla templates en las columnas blockinguser_id.
	 * Afectara solo a la "cantidad" especificada de parámetros.
	 * 
	 * La modificación consiste en escoger de manera aleatoria "cantidad" registros de la tabla templates
	 * y setear el campo blockinguser_id por el "user ID" obtenido durante el login.
	 * @param cantidad Cantidad de registros a solicitar
	 * @return true si la operación UPDATE en SQL resulta exitosa.
	 */
	private boolean desbloqueaDB (int cantidad)
	{
		Boolean respuesta;

		respuesta = false;
		String SQL_UPDATE_TEMPLATE, SQL_UPDATE;
		SQL_UPDATE_TEMPLATE = "update templates set blockinguser_id = _USER_ID " + 
				"WHERE  `pathfilename` LIKE  '%workspace%' " + 
				"	AND  `size` >0 " + 
				"	AND blockinguser_id = 0 " + 
				"	AND evaluations = 0 " + 
				"ORDER BY RAND() " + 
				"LIMIT _CANTIDAD";

		SQL_UPDATE = SQL_UPDATE_TEMPLATE.replaceAll("_USER_ID", ""+FitsZoo.getUser_id());
		SQL_UPDATE = SQL_UPDATE.replaceAll("_CANTIDAD", ""+cantidad);


		int updatedRows;
		updatedRows = ZwickyDB.ejecutaInsert(SQL_UPDATE);
		if (updatedRows == cantidad)
		{
			respuesta = true;	
		}
		return respuesta;
	}

	/**
	 * Decarga todos los archivos que el usuario tiene pendiente y que aun no han sido descargados
	 * @return True si consigue descargar todos los archivos sin ningun inconveniente.
	 */
	private Boolean descargarTodo() {
		
		Boolean respuesta;
		respuesta = true; // Suponemos que no va a ocurrir ningun inconveniente durante la descarga
		String body;
		String title;
		body = "Dependiendo de su conexión a Internet, esto podria tomar mucho tiempo.\n" +
		"Por eso optamos por hacer todas las descargas anted de comenzar a examinar las imágenes.\n\n" +
		"Una vez finalizadas las descargas, pasaremos a examinar.";
		title = "Download All";			
		JOptionPane.showConfirmDialog(this, body ,title, JOptionPane.DEFAULT_OPTION ,JOptionPane.INFORMATION_MESSAGE);
		
		for (TemplateCycle fits : this.toDownloadTemplates) {
			this.taResumen.append("\n Descargando archivo id="+fits.getTemplateID()+" size="+fits.getFileSizeDB()+":");
			if (fits.Download())
			{
				this.taResumen.append("OK");
			}
			else
			{
				// Si pasamos por aca es porque ocurrio un problema en la descarga y ya no podemos retornar true
				respuesta = false;
				this.taResumen.append("Problemas en la descarga.");
				this.taResumen.getParent().repaint();			
				body = "Ocurrio un problema al descargar este archivo. \n" +
				"Por favor intente agregando el siguiente DNS a su entorno de red:\n\n" +
				"DNS --> 146.83.9.3";
				JOptionPane.showConfirmDialog(this, body ,title, JOptionPane.DEFAULT_OPTION ,JOptionPane.INFORMATION_MESSAGE);
			}
		}
		return respuesta;
	}

	@Override
	public void propertyChange(PropertyChangeEvent evt) {
		Object source;
		source = evt.getSource();
		if (source.equals(this.panelTemplate) && this.panelTemplate.getBackground().equals(Color.GREEN))
		{
			this.panelTemplate.removePropertyChangeListener(this);
			System.out.println("LockedFitsPanel:Submit Finalizado.");
			//MuestraSiguienteTemplate();
			this.refresh();
		}
	}

	/**
	 * 
	 * @return True si consigue desplegar la interfaz para un nuevo template.
	 */
	private Boolean MuestraSiguienteTemplate ()
	{
		Boolean respuesta;
		respuesta = true;
		int archivosPorDescargar;
		archivosPorDescargar = this.toDownloadTemplates.size();
		if ((archivosPorDescargar==0) && (this.lockedTemplates.size()>0))
		{
			this.panelDown.remove(panelTemplate);
			TemplateCycle templateToReview;
			templateToReview = this.lockedTemplates.remove(0);
			this.panelTemplate = new PanelTemplate(templateToReview);

			this.taResumen.append("\nExaminando:\n"+templateToReview.getJpgURL()+"\n"+templateToReview.getFitsURL());

			GridBagConstraints c;
			c = new GridBagConstraints ();
			c.fill = GridBagConstraints.HORIZONTAL;
			c.gridx = 0;	c.gridy = 4;
			c.weightx = 1;	c.weighty = 1.0;
			c.gridwidth = 2; c.gridheight = 3;
			
			this.panelDown.add(this.panelTemplate,c);
			this.panelTemplate.addPropertyChangeListener(this);
			this.getRootPane().updateUI();
			respuesta = true;
		}
		return respuesta;
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
		System.out.println("ancho="+this.scrollTopPanel.getWidth()+ "\t alto="+this.scrollTopPanel.getHeight());
		
	}

	@Override
	public void componentShown(ComponentEvent arg0) {
		// TODO Auto-generated method stub
		
	}
}
