/**
 * 
 */
package util;

import java.awt.Desktop;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

/**
 * @author sysop
 *
 */
public class Misc {


	public static void AbrirNavegadorDefault (String url)
	{
		if(Desktop.isDesktopSupported())
		{
			try {
				Desktop.getDesktop().browse(new URI(url));
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (URISyntaxException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	/**
	 * Duerme la hebra en curso.
	 * @param miliseconds Tiempo a dormir.
	 */
	public static void sleep (int miliseconds)
	{
		try {
			Thread.sleep(miliseconds);
		} catch (InterruptedException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
	}

//	/**
//	 * 	Ejecuta un comando llamando al sistema operativo.
//	 * @param comando Llamada a sistema a ejecutar.
//	 * @return exit value de la llamada
//	 */
//	public static int ejecutaComando(String comando)
//	{
//		int exitValue=-1;
//		System.out.println("comando=("+comando+")");
//		if (comando.length()==0)
//		{
//			return exitValue;
//		}
//		try
//		{
//			Process p=Runtime.getRuntime().exec(comando);
//			p.waitFor();
//			BufferedReader reader=new BufferedReader(new InputStreamReader(p.getInputStream()));
//			String line=reader.readLine();
//			while(line!=null)
//			{
//				System.out.println(line);
//				line=reader.readLine();
//			}
//			exitValue = p.exitValue();
//		}
//		catch(IOException e1) {}
//		catch(InterruptedException e2) {}
//		System.out.println("exitValue="+exitValue);
//		System.out.println("finished.");
//		return exitValue;
//	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

}
