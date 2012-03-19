/**
 * 
 */
package cl.calan.ctio.ui;

import java.awt.FlowLayout;
import java.awt.GridLayout;

import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JPanel;
import javax.swing.border.TitledBorder;

/**
 * @author sysop
 *
 */
public class RelaysUI extends JPanel{

	/**
	 * 
	 */
	private static final long serialVersionUID = -2013064047807744059L;
	private JButton   jbSwitchOff;
	private JButton   jbSwitchOn;
	private JButton   jbRead;
    private JPanel    jpReles;
    private JPanel 	  jpBotones;
	private JCheckBox[] jcbRelay;
	private PowerDistributionUnit parent;
	
	public RelaysUI (PowerDistributionUnit parent)
	{
		super();
		this.parent = parent;
		
		this.setBorder(new TitledBorder("16 Relays")); //$NON-NLS-1$
		
		this.jbSwitchOff = new JButton("Switch Off"); //$NON-NLS-1$
		this.jbSwitchOn = new JButton("Switch On"); //$NON-NLS-1$
		this.jbRead = new JButton ("Read"); //$NON-NLS-1$
		
		this.jbSwitchOff.setActionCommand(this.jbSwitchOff.getText());
		this.jbSwitchOff.addActionListener(this.parent);

		this.jbSwitchOn.setActionCommand(this.jbSwitchOn.getText());
		this.jbSwitchOn.addActionListener(this.parent);

		this.jbRead.setActionCommand(this.jbRead.getText());
		this.jbRead.addActionListener(this.parent);

		jcbRelay = new JCheckBox[16];
		GridLayout glayout;
		glayout = new GridLayout(4,4);
		glayout.setHgap(8);
		glayout.setVgap(8);
		jpReles = new JPanel(glayout);
		
		for (int i=0;i<16;i++)
		{
		 jcbRelay[i]= new JCheckBox();
		 switch (i) {
			case 0:
				jcbRelay[i].setText(Messages.getString("RelaysUI.4")); //$NON-NLS-1$
				break;
			case 1:
				jcbRelay[i].setText(Messages.getString("RelaysUI.5")); //$NON-NLS-1$
				break;
			case 2:
				jcbRelay[i].setText(Messages.getString("RelaysUI.6")); //$NON-NLS-1$
				break;
			case 3:
				jcbRelay[i].setText(Messages.getString("RelaysUI.7")); //$NON-NLS-1$
				break;
			case 4:
				jcbRelay[i].setText(Messages.getString("RelaysUI.8")); //$NON-NLS-1$
				break;
			case 5:
				jcbRelay[i].setText(Messages.getString("RelaysUI.9")); //$NON-NLS-1$
				break;
			case 6:
				jcbRelay[i].setText(Messages.getString("RelaysUI.10")); //$NON-NLS-1$
				break;
			case 7:
				jcbRelay[i].setText(Messages.getString("RelaysUI.11")); //$NON-NLS-1$
				break;
			case 8:
				jcbRelay[i].setText(Messages.getString("RelaysUI.12")); //$NON-NLS-1$
				break;
			case 9:
				jcbRelay[i].setText(Messages.getString("RelaysUI.13")); //$NON-NLS-1$
				break;
			case 10:
				jcbRelay[i].setText(Messages.getString("RelaysUI.14")); //$NON-NLS-1$
				break;
			case 11:
				jcbRelay[i].setText(Messages.getString("RelaysUI.15")); //$NON-NLS-1$
				break;
			case 12:
				jcbRelay[i].setText(Messages.getString("RelaysUI.16")); //$NON-NLS-1$
				break;
			case 13:
				jcbRelay[i].setText(Messages.getString("RelaysUI.17")); //$NON-NLS-1$
				break;
			case 14:
				jcbRelay[i].setText(Messages.getString("RelaysUI.18")); //$NON-NLS-1$
				break;
			case 15:
				jcbRelay[i].setText(Messages.getString("RelaysUI.19")); //$NON-NLS-1$
				break;

		default:
			break;
		}
		 //jcbRelay[i].setText("Relay "+(i+1)+".");
		 jpReles.add(jcbRelay[i]);
		}
		
		jpBotones = new JPanel(new FlowLayout());
		jpBotones.add(jbSwitchOff);
		jpBotones.add(jbRead);
		jpBotones.add(jbSwitchOn);

		this.setLayout(new GridLayout(2,1));
		this.add(jpReles);
		this.add(jpBotones);
	}

	public JCheckBox[] getJcbRelay() {
		return jcbRelay;
	}

	public void setJcbRelay(JCheckBox[] jcbRelay) {
		this.jcbRelay = jcbRelay;
	}
	
	
}
