/**
 * 
 */
package rtsps;

import java.awt.Container;
import java.awt.Dimension;
import java.awt.event.WindowEvent;
import java.io.IOException;

import javax.swing.JFrame;

/**
 * @author eduardo
 *
 */
public class GuiRtsParse extends JFrame{

	private final int frameAncho =900;
	private final int frameAlto = 700;

	
	public GuiRtsParse() {
		this.setPreferredSize(new Dimension(frameAncho, frameAlto));
		Container contenedor;
		contenedor = this.getContentPane();		
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

}
