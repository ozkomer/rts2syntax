/**
 * 
 */
package util;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import org.apache.commons.io.FileUtils;

/**
 * Esto permite manejar parámetros persistentes de la aplicación.
 * Evitando así que el usuario deba ingresar o configurar estos parámetros en cada sesión con la aplicación.
 * @author sysop
 *
 */
public class Parametros extends HashMap<String, String>{

	/**
	 * 
	 */
	private static final long serialVersionUID = 683243655727608786L;

	static File parametersFile = new File ("params.txt");
	public final static String USERNAME = "username";
	public final static String PASSWORD = "password";
	public final static String DS9_PATH = "ds9_path";
	public final static String WGET_PATH = "wget_path";
	public final static String SETUP_OK = "setup_ok";
	
	public Parametros ()
	{	
		super();
		if (parametersFile.exists())
		{
			this.leer();
		}else {
			this.cargaParametrosBasicos();
			this.guardar();
			this.leer();
		}
	}	
	
	@Override
	public String toString() {
		StringBuilder respuesta;
		respuesta = new StringBuilder();
		
		for (String llave : this.keySet()) {
			respuesta.append("valor("+llave+")="+this.get(llave)+"\n");
		}
		return respuesta.toString();
	}

	@Override
	public String get(Object key) {
		// TODO Auto-generated method stub
		return super.get(key);
	}	

	@Override
	public String put(String key, String value) {
		String respuesta;
		respuesta = null;
		boolean guardar;
		guardar=false;
		if (this.containsKey(key))
		{
			if (!(super.get(key).equals(value)))
			{				
				guardar = true;
			}
		}
		respuesta = super.put(key, value);
		if (guardar) this.guardar();
		return respuesta;
	}

	private void cargaParametrosBasicos ()
	{
		System.out.println("-------cargaParametrosBasicos");
		this.clear();
		this.put(USERNAME, "username");
		this.put(PASSWORD, "password");
		this.put(DS9_PATH, "");
		this.put(WGET_PATH, "");
		this.put(SETUP_OK, "00");		
	}
	
	private void leer()
	{
		System.out.println("-------leer");
		List<String> contenido;
		contenido = null;
		try {
			contenido = FileUtils.readLines(parametersFile);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		this.clear();
		if (contenido!=null)
		{
			for (String llave_valor : contenido) {
				String llave;
				String valor;
				int indiceIgual;
				indiceIgual = llave_valor.indexOf('=');
				llave = llave_valor.substring(0, indiceIgual);
				valor = llave_valor.substring(indiceIgual+1);

				if ( 	
						(llave.equals(USERNAME))  	||
						(llave.equals(PASSWORD))  	||
						(llave.equals(DS9_PATH))	||
						(llave.equals(WGET_PATH))	||			
						(llave.equals(SETUP_OK))						
					)
				this.put(llave, valor);
			}
		}	
	}
	
	private void guardar ()
	{
		System.out.println("-------guardar");
		List<String> contenido;
		contenido = new ArrayList<String>();
		contenido.add(USERNAME+"="+this.get(USERNAME));
		contenido.add(PASSWORD+"="+this.get(PASSWORD));
		contenido.add(DS9_PATH+"="+this.get(DS9_PATH));
		contenido.add(WGET_PATH+"="+this.get(WGET_PATH));
		contenido.add(SETUP_OK+"="+this.get(SETUP_OK));
				
		
		try {
			FileUtils.writeLines(parametersFile, contenido);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		Parametros param;
		param = new Parametros();
		System.out.println("params="+param.toString());
	}
}
