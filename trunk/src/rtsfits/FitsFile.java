/**
 * 
 */
package rtsfits;

/**
 * @author eduardo
 *
 */
public class FitsFile {

	/**
	 * ruta completa en el sistema de archivos.
	 * Esta ruta es la que indica el log de rts2.
	 */
	private String pathFileName;
	
	/**
	 * Usualmente debería ser algo en el conjunto:
	 * darks, skyflats, focusing, archive, trash
	 */
	private String tipo;
	private String filename;
	private String YYYMMDD;

	public FitsFile (String rts2logLine)
	{
		StringBuilder mensaje;
		mensaje = new StringBuilder();
		String[] word;
		String[] pathSection;
		int largoWord;
		int largoPathSection;
		word = rts2logLine.split(" ");
		largoWord = word.length;
		pathFileName = word[largoWord-1];
		pathSection = pathFileName.split("/");
		largoPathSection = pathSection.length;
		tipo = pathSection[3];
		YYYMMDD = pathSection[4];
		filename = pathSection[largoPathSection-1];
		mensaje.append("fecha=");
		mensaje.append(YYYMMDD);
		mensaje.append("\t");
		mensaje.append("tipo=");
		mensaje.append(tipo);
		mensaje.append("\t");
		mensaje.append("filename=");
		mensaje.append(filename);
		mensaje.append("\t");
		mensaje.append("pathFileName=");
		mensaje.append(pathFileName);
		//System.out.println(mensaje.toString());
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

	/**
	 * Ruta completa del archivo.
	 */
	public String getPathFileName() {
		return pathFileName;
	}

	public void setPathFileName(String pathFileName) {
		this.pathFileName = pathFileName;
	}

	public String getTipo() {
		return tipo;
	}

	public void setTipo(String tipo) {
		this.tipo = tipo;
	}

	public String getFilename() {
		return filename;
	}

	public void setFilename(String filename) {
		this.filename = filename;
	}

	public String getYYYMMDD() {
		return YYYMMDD;
	}

	public void setYYYMMDD(String yYYMMDD) {
		YYYMMDD = yYYMMDD;
	}
	
	

}
