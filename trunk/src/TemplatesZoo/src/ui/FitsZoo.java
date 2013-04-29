/**
 * 
 */
package ui;

import java.awt.Container;
import java.awt.Dimension;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;

import javax.swing.JFrame;
import javax.swing.JTabbedPane;

import util.Parametros;
import database.ZwickyDB;

/**
 * @author sysop
 *
 */
public class FitsZoo extends JFrame implements ComponentListener {

	/**
	 * 
	 */
	private static final long serialVersionUID = -2015795467088056514L;

	private LoginDialog 			loginDialog;
	private LockedFitsPanel 		lockedFitsPanel; 
	private ExternalAppsPanel 		externalAppsPanel;
	private JTabbedPane				tabbdedPane;

	
	public static ZwickyDB zwickyDB;

	public static Parametros params = new Parametros();
	private static int user_id;

	public static void setUser_id(int userId) {
		user_id = userId;
	}


	public static int getUser_id() {
		return user_id;
	}


	public FitsZoo ()
	{
		super("Template Zoo v0.62");		
		zwickyDB = new ZwickyDB();
		this.loginDialog = new LoginDialog(this,"login");
		this.lockedFitsPanel = new LockedFitsPanel();
		this.externalAppsPanel = new ExternalAppsPanel();
		this.tabbdedPane = new JTabbedPane();

		this.addListeners();
		this.addComponents();
	}

	private void addListeners() {
		this.addComponentListener(this);
	}

	private void addComponents ()
	{
		Container mainContainer;
		mainContainer = this.getContentPane();
		this.tabbdedPane.add("Locked Fits",this.lockedFitsPanel);
		this.tabbdedPane.add("Setup", this.externalAppsPanel);
		if (params.get(Parametros.SETUP_OK).equals("11"))
		{
			this.tabbdedPane.setSelectedIndex(0);
		}else
		{
			this.tabbdedPane.setSelectedIndex(1);
		}
		mainContainer.add(this.tabbdedPane);
	}

	public void showLoginDialog ()
	{
		loginDialog.pack();
		loginDialog.setVisible(true);
		System.out.println("Dialogo Cerrado.");
		this.lockedFitsPanel.refresh();
	}

	/**
	 * Create the GUI and show it.  For thread safety,
	 * this method should be invoked from the
	 * event-dispatching thread.
	 */
	private static void createAndShowGUI() {
		//Create and set up the window.
		JFrame frame = new FitsZoo();
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		//Display the window.
		frame.setMinimumSize(new Dimension(682, 802));
		frame.pack();
		frame.setVisible(true);
		((FitsZoo)frame).showLoginDialog();
	}

	public static void main(String[] args) {
		//Schedule a job for the event-dispatching thread:
		//creating and showing this application's GUI.
		javax.swing.SwingUtilities.invokeLater(new Runnable() {
			public void run() {
				createAndShowGUI();
			}
		});
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
		System.out.println("ancho="+this.getWidth()+ "\t alto="+this.getHeight());
	}


	@Override
	public void componentShown(ComponentEvent arg0) {
		// TODO Auto-generated method stub
		
	}



}
