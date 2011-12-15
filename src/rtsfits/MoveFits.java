/**
 * Busca en las lineas del archivo "/var/log/rts2-debug"
 * por líneas con el texto "move /images".
 * Sin encuentra esta línea significa que se ha creado un nuevo archivo .fits ya sea en:
 * /images//archive/
 * /images//darks/
 * /images//skyflats/
 * /images//trash/
 */
package rtsfits;

import java.io.IOException;
import java.util.Scanner;


/**
 * @author eduardo
 *
 */
public class MoveFits extends EventAction {

	private static Process process;
	private static Runtime runtime;
	private static Scanner scanner;
	private boolean enableCommand;
	/**
	 * Cualquiera de las líneas de log de interés tiene en común el siguiente String.
	 */
	public static final String commonPattern ="move /images//que/";

	/** 
	 * Ahora, si la línea contiene el patron comun, debe poseer además alguno de los siguientes:
	 */
	public static final String searchPattern[] = {
		"/images//archive/" 
		, "/images//darks/"
		, "/images//skyflats/" 
		, "/images//trash/"};




	public MoveFits(boolean enableCommand) {
		super();
		this.enableCommand = enableCommand;
	}

	static {
		runtime = null;
		process = null;
		// LA interfaz con el sistema operativo
		runtime= Runtime.getRuntime();
	}

	/* (non-Javadoc)
	 * @see rtsfits.EventAction#processLine(java.lang.String)
	 */
	@Override
	public boolean processLine(String line) {

		if (line.indexOf(commonPattern)>=0)
		{
			this.action(line);
		}
		return false;
	}


	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}


	/**
	 * Aqui solo puede llegar una línea que poseea el patron en comun.
	 * el archivo que interesa está definido después del último espacio.
	 * En el archivo detectado, se identifica la fecha del archivo observando los separadores "/"
	 * Ejemplo:
	 * 2011-12-14T22:17:41.362 CLT IMGP 8 move /images//que/20111214/20111215011624-167-RA.fits to /images//archive/20111214/16904/20111215011624-167-RA.fits
	 * el texto despues del último espacio es:
	 * /images//archive/20111214/16904/20111215011624-167-RA.fits
	 * Separando el texto anterior por "/", la fecha es:
	 * 20111214
	 * que corresponde al tercer espacio definido por el separador "/".
	 */
	@Override
	public void action(String line) {
		FitsFile fitsFile;
		fitsFile = new FitsFile(line);
		//scp /home/emaureir/20111212.tar.gz sne@calan.das.uchile.cl:CHASE500
		StringBuilder comando;
		comando = new StringBuilder();
		comando.append("scp ");
		comando.append(fitsFile.getPathFileName());
		comando.append(" chase@zwicky.ctio.noao.edu:data/v8.0/CC/workspace/");
		comando.append(fitsFile.getYYYMMDD());
		comando.append("/");
		comando.append(fitsFile.getFilename());
		System.out.println(comando);
		if (enableCommand){
			ejecutaComando("wget http://i1-win.softpedia-static.com/screenshots/Windows-7-High-Resolution-Regional-Wallpapers_1.jpg");
		}
		return;
	}

	public static int ejecutaComando (String comando)
	{
		try {
			//La llamada a sistema con su proceso respectivo.
			process = runtime.exec(comando);

		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		scanner = new Scanner(process.getErrorStream());
		while(scanner.hasNextLine())
		{
			String line = scanner.nextLine();
			System.out.println(line);
		}
		try {
			process.waitFor();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return process.exitValue();		
	}

}
