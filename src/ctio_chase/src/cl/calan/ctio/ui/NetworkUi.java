/**
 * 
 */
package cl.calan.ctio.ui;

import java.awt.Dimension;

import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JSpinner;
import javax.swing.JTextField;
import javax.swing.SpinnerNumberModel;
import javax.swing.border.TitledBorder;

/**
 * @author sysop
 *
 */
public class NetworkUi extends JPanel{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private JButton jbConnect;
	private JLabel  jlHost;
	private JLabel jlPort;
	private JTextField jtHost;
	private JSpinner jspinPort;
	private PowerDistributionUnit parent;
	
	public NetworkUi (PowerDistributionUnit parent)
	{
		super();
		this.parent = parent;
		this.setBorder(new TitledBorder("Network"));
		//this.setPreferredSize(new Dimension(450,50));
		//this.setMaximumSize(new Dimension(450,50));
		
		jlHost = new JLabel("Host");
		jlPort = new JLabel("Tcp Port");
		jtHost = new JTextField();
		SpinnerNumberModel snm;
		snm = new SpinnerNumberModel(18008, 1, 65000, 1);

		jspinPort = new JSpinner(snm);
		
		jbConnect = new JButton("Connect");
		this.jbConnect.setActionCommand(this.jbConnect.getText());
		this.jbConnect.addActionListener(this.parent);
		
		jtHost.setPreferredSize(new Dimension(137, 20));
		jtHost.setText("139.229.12.84");
		this.add(jlHost);
		this.add(jtHost);
		this.add(jlPort);
		this.add(jspinPort);
		this.add(jbConnect);		
		
	}

	public String getHost() {
		return jtHost.getText();
	}

	public int getPort() {
		return ((SpinnerNumberModel)jspinPort.getModel()).getNumber().intValue();
	}
	
	
}
