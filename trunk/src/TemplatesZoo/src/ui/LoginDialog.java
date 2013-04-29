/**
 * 
 */
package ui;

import java.awt.Color;
import java.awt.Frame;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.sql.ResultSet;
import java.sql.SQLException;

import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPasswordField;
import javax.swing.JTextField;

import org.apache.commons.codec.digest.DigestUtils;

import util.Parametros;

/**
 * @author sysop
 *
 */
public class LoginDialog extends JDialog implements ItemListener, ActionListener {
	/**
	 * 
	 */
	private static final long serialVersionUID = 3009267096532671121L;
	private JLabel jlUsername;
	private JLabel jlUPassword;
	private JTextField jtUsername;
	private JPasswordField jpPassword;
	private JCheckBox jchkRememberMe;
	private JButton bLogin;
	private java.sql.PreparedStatement statement = null;

	private Frame parent;
	
	public LoginDialog(Frame parent, String title) {
		super(parent, title);
		this.setModal(true);
		this.parent = parent;
		//this.setBorder(BorderFactory.createTitledBorder("Login"));
		this.jlUsername = new JLabel("Username");
		this.jlUPassword = new JLabel("Password");
		this.jtUsername = new JTextField(20);
		this.jpPassword = new JPasswordField(20);
		this.jchkRememberMe = new JCheckBox("RememberMe");
		this.bLogin = new JButton ("Login");
		this.presetFields();
		this.addListeners();
		this.addComponents();		
	}
	
	private void presetFields ()
	{
		this.jtUsername.setText(FitsZoo.params.get(Parametros.USERNAME));
		this.jpPassword.setText(FitsZoo.params.get(Parametros.PASSWORD));
	}
	
	private void addListeners ()
	{
	 this.jchkRememberMe.addItemListener(this);
	 this.bLogin.addActionListener(this);
	 
	 this.setDefaultCloseOperation(
			    JDialog.DO_NOTHING_ON_CLOSE);
	 this.addWindowListener(new WindowAdapter() {
			    public void windowClosing(WindowEvent we) {
			    	int n = JOptionPane.showOptionDialog(parent, "Desea Cerrar esta aplicación?", "Login",JOptionPane.YES_NO_OPTION, JOptionPane.WARNING_MESSAGE, null, null,null);
			    	System.out.println("n="+n);
			    	if (n==0) {
			    		System.exit(1);
			    	}
			    }
			});
	}
	
	private void addComponents ()
	{
		this.setLayout(new GridLayout(3, 2));
		this.add(this.jlUsername);
		this.add(this.jtUsername);
		this.add(this.jlUPassword);
		this.add(this.jpPassword);
		this.add(this.jchkRememberMe);
		this.add(this.bLogin);
	}

	public void itemStateChanged(ItemEvent e) {
		if (e.getSource()== this.jchkRememberMe)
		{
			System.out.println("checked="+this.jchkRememberMe.isSelected());
		}

		
	}

	public void actionPerformed(ActionEvent e) {
		if (e.getSource()==this.bLogin) 
		{
			System.out.println("Boton Login");

			String username;
			char[] pw;
			username = this.jtUsername.getText();
			pw = jpPassword.getPassword();
			
			if (this.login(username,pw))  
			{
				this.getContentPane().setBackground(Color.GREEN);
				

				util.Misc.sleep(1000);
				if (this.jchkRememberMe.isSelected())
				{
					FitsZoo.params.put(Parametros.USERNAME, username);
					FitsZoo.params.put(Parametros.PASSWORD, new String( pw,0,pw.length));
					//FitsZoo.params.guardar();
				}
				this.setVisible(false);				
			}else
			{
				util.Misc.sleep(4000);
				this.getContentPane().setBackground(Color.PINK);
			}
		}
	}

	public static String saltedString (String username, char[] pw)
	{
		StringBuilder salt;
		salt = new StringBuilder();
		salt.append(username.trim());
		salt.append("fitszoousers");
		salt.append(pw);
		return salt.toString().trim();
	}
	
	public static String hashedString (String saltedString)
	{
		String respuesta;		
		respuesta = DigestUtils.sha1Hex(saltedString.getBytes());
		System.out.println("salted="+saltedString.toString());
		System.out.println("sha1(salt)="+respuesta);
		return respuesta;		
	}
	
	/**
	 * 
	 * @param username 
	 * @param password 
	 * @return true en caso de login exitoso.
	 */
	private boolean login (String username, char[] password)
	{
		Boolean respuesta;
		respuesta = false;
		StringBuilder query;
		query = new StringBuilder();
		query.append("SELECT id, hash FROM fitszoousers WHERE name= '");
		query.append(username);
		query.append("'");
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

		String bdHash, localSha1;

		bdHash = null;
		ResultSet resultSet;
		try {
			resultSet = statement.executeQuery();
			resultSet.next();
			FitsZoo.setUser_id(resultSet.getInt(1));
			bdHash = resultSet.getString(2);
			resultSet.close();
		} catch (SQLException e) {
			System.out.println("Error al leer user ID, resultSetVacio."+e.getMessage());
		}
		
		FitsZoo.zwickyDB.close();
		System.out.println("bdSha1="+bdHash);
		localSha1 = hashedString(saltedString(username, password));
		
		respuesta = ((bdHash!=null) && (bdHash.equals(localSha1)));

		return respuesta;
	}


	
	
}
