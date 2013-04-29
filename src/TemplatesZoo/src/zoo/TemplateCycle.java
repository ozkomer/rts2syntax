package zoo;

import java.io.File;

import database.ZwickyDB;

import ui.FitsZoo;
import util.ExecCommand;
import util.Parametros;

/**
 * Representación de un archivo con lock en la base de datos.
 * Es necesario modelar los status y las acciones a seguir, por ejemplo:
 * ID del template.
 * Está descargado el archivo en el disco duro?
 * Abrir el archivo con DS9.
 * Evaluar el template.
 * Subir la evaluacion a la base de datos en zwicky.
 * Desbloquear el id en la tabla de templates, incrementar en uno la cantidad de veces que el archivo ha sido evaluado.
 * @author sysop
 *
 */
public class TemplateCycle {

	public final static int STATUS_LOCKED=0;
	public final static int STATUS_WGET_UP=1;
	public final static int STATUS_WGET_DOWN=2;
	public final static int STATUS_DS9_UP=3;
	public final static int STATUS_DS9_DOWN=4;
	public final static int STATUS_EVALUATE_UP=4;
	public final static int STATUS_EVALUATE_DOWN=5;
	public final static int STATUS_INSERTING_DB_UP=6;
	public final static int STATUS_INSERTING_DB_DOWN=7;
	public final static int STATUS_UNLOCKING_DB_UP=8;
	public final static int STATUS_UNLOCKING_DB_DOWN=9;

	private int templateID;
	private File fitsFile;
	private String fitsURL;
	private String jpgURL;
	private int templateQuality;
	private boolean restaCorrecta;
	private int status;
	/**
	 * Tamaño del archivo segun la base de datos.
	 */
	private int fileSizeDB;

	/**
	 * Cantidad de veces que el archivo ha sido evaluado segun la base de datos. 
	 */
	private int evaluations;

	/**
	 * 
	 * @param id
	 * @param fileSizeDB
	 * @param evaluations
	 * @param descriptionurl campo description de la tabla plans (Sirve para mostrar un jpg)
	 */
	public TemplateCycle (int id, int fileSizeDB, int evaluations, String descriptionurl )
	{
		this.templateID = id;
		this.fitsFile = new File(""+this.templateID+".fits");
		this.fitsURL = "http://zwicky_candidatos.das.uchile.cl/templates/"+this.templateID+".fits";
		this.jpgURL = descriptionurl;
		this.templateQuality = -1;
		this.restaCorrecta = false;
		this.fileSizeDB = fileSizeDB;
		this.evaluations = evaluations;
		this.status = STATUS_LOCKED;
		this.checkDownloaded();
	}

	/**
	 * Revisa si el archivo fits ya ha sido descargado.
	 * Si el estado actual del archivo es "LOCKED", pasamos de inmediato al statys WGET_DOWN
	 * @return true si el archivo ya estaba correctamente descargado.
	 */
	private boolean checkDownloaded ()
	{
		this.fitsFile = new File(""+this.templateID+".fits");
		boolean alreadyDownloaded;
		alreadyDownloaded = 	(	(this.fitsFile.exists()) && 
							(this.fitsFile.length()== this.fileSizeDB) 
						);
		if ((this.status == STATUS_LOCKED) && (alreadyDownloaded))
		{
			this.status = STATUS_WGET_DOWN;
		}
		return alreadyDownloaded;
	}

	/**
	 * Descarga con wget el archivo fits
	 * @return true si consigue descargar el archivo sin problemas
	 */
	public boolean Download ()
	{
		Boolean respuesta;
		respuesta = false;
		this.status = STATUS_WGET_UP;
		String comando;
		comando = (FitsZoo.params.get(Parametros.WGET_PATH)+"  --continue --progress=dot:mega --http-user=zwicky --http-password=VW5YlUOR "+this.fitsURL);
		int exitValue=-1;
		ExecCommand execKommand;
		execKommand = new ExecCommand(comando);
		System.out.println(execKommand.getOutput());
		System.err.println(execKommand.getError());
		exitValue = execKommand.getExitValue();
		
//		exitValue = Misc.ejecutaComando(comando);
		if (exitValue==0) 
		{
			respuesta =true;
			this.status = STATUS_WGET_DOWN;
		}
		return respuesta;
	}


	public final static String COMANDO_DS9 = "EXECUTABLE -file PATHFILENAME -invert -scale sinh -align -zoom to fit -zscale";
	//public final static String PARAMS_DS9 = "-file PATHFILENAME -zoom to fit -zscale";

	
	public String comando;
	
	/**
	 * Despues de:
	 * - Haber descargado el archivo .fits con wget.
	 * Se procede a revisar el archivo con DS9
	 * @return true si consigue desplegar el archivo sin problemas
	 */
	public boolean AbrirDS9 ()
	{
		this.status = STATUS_DS9_UP;
		Boolean respuesta;
		respuesta = false;
		int exitValue=-1;		

		String posixFits;
		posixFits = this.fitsFile.getName();
		comando = COMANDO_DS9.replaceAll("EXECUTABLE", FitsZoo.params.get(Parametros.DS9_PATH));
		comando = comando.replaceAll("PATHFILENAME", posixFits);

//		exitValue = Misc.ejecutaComando(comando);
//		parametrosWget = PARAMS_DS9.replaceAll("PATHFILENAME", this.fitsFile.getAbsolutePath());
		ExecCommand execKommand;
		System.out.println("comando="+comando);
		execKommand = new ExecCommand(comando);
		System.out.println(execKommand.getOutput());
		System.err.println(execKommand.getError());
		exitValue = execKommand.getExitValue();
		if (exitValue==0) 
		{
			respuesta =true;
			this.status = STATUS_DS9_DOWN;
		}
		return respuesta;
	}

	/**
	 * Despues de:
	 * - Haber descargado el archivo .fits con wget.
	 * - Haber revisadp el archivo con DS9.
	 * - Haber evaluado el archivo con el formulario de la interfaz gráfica.
	 * Se procede a subir insertar la evaluacion en la base de datos.
	 * 
	 * @return
	 */
	public boolean insertDbOpinion ()
	{
		Boolean respuesta;
		this.status = STATUS_INSERTING_DB_UP;
		respuesta = false;
		String SQL_UPDATE_TEMPLATE, SQL_UPDATE;
		SQL_UPDATE_TEMPLATE = "INSERT INTO  `chase500`.`templateopinions` (\n" + 
		"`id` ,\n" + 
		"`template_id` ,\n" + 
		"`fitszoouser_id` ,\n" + 
		"`quality` ,\n" + 
		"`restacorrecta` ,\n" + 
		"`fecha`\n" + 
		")\n" + 
		"VALUES (\n" + 
		"NULL ,  '_TEMPLATE_ID',  '_USER_ID',  '_QUALITY',  '_RESTA_CORRECTA', \n" + 
		"CURRENT_TIMESTAMP )";
		SQL_UPDATE = SQL_UPDATE_TEMPLATE.replaceAll("_TEMPLATE_ID", ""+this.templateID);
		SQL_UPDATE = SQL_UPDATE.replaceAll("_USER_ID", ""+FitsZoo.getUser_id());
		SQL_UPDATE = SQL_UPDATE.replaceAll("_QUALITY", ""+this.templateQuality);
		String strRestaCorrecta;
		strRestaCorrecta = "0";
		if (this.restaCorrecta) {strRestaCorrecta="1"; }
		SQL_UPDATE = SQL_UPDATE.replaceAll("_RESTA_CORRECTA", strRestaCorrecta);

		int insertedRows;
		insertedRows = ZwickyDB.ejecutaInsert(SQL_UPDATE);
		if (insertedRows == 1)
		{
			respuesta = true;	
			this.status = STATUS_INSERTING_DB_DOWN;
		}

		return respuesta;
	}

	/**
	 * Despues de:
	 * - Haber descargado el archivo .fits con wget.
	 * - Haber revisado el archivo con DS9.
	 * - Haber evaluado el archivo con el formulario de la interfaz gráfica.
	 * - Haber insertadp la evaluación en la base de datos.
	 * Se procede a desbloquear el archivo en la base de datos.
	 * 
	 * @return
	 */
	public boolean updateDbUnlock ()
	{
		Boolean respuesta;
		this.status = STATUS_UNLOCKING_DB_UP;
		respuesta = false;
		String SQL_UPDATE_TEMPLATE, SQL_UPDATE;
		SQL_UPDATE_TEMPLATE = "UPDATE  `chase500`.`templates` SET  `blockinguser_id` =  '0',\n" + 
		"`evaluations` =  '_EVALUATIONS' WHERE  `templates`.`id` =_TEMPLATE_ID;\n";

		SQL_UPDATE = SQL_UPDATE_TEMPLATE.replaceAll("_TEMPLATE_ID", ""+this.templateID);
		SQL_UPDATE = SQL_UPDATE.replaceAll("_EVALUATIONS", ""+this.evaluations+1);


		int updatedRows;
		updatedRows = ZwickyDB.ejecutaInsert(SQL_UPDATE);
		if (updatedRows == 1)
		{
			respuesta = true;	
			this.status = STATUS_UNLOCKING_DB_DOWN;
		}
		return respuesta;
	}

	public int getTemplateID() {
		return templateID;
	}

	public void setTemplateID(int templateID) {
		this.templateID = templateID;
	}

	public File getFitsFile() {
		return fitsFile;
	}

	public void setFitsFile(File fitsFile) {
		this.fitsFile = fitsFile;
	}

	public String getFitsURL() {
		return fitsURL;
	}

	public void setFitsURL(String fitsURL) {
		this.fitsURL = fitsURL;
	}

	public boolean isRestaCorrecta() {
		return restaCorrecta;
	}

	public void setRestaCorrecta(boolean restaCorrecta) {
		this.restaCorrecta = restaCorrecta;
	}

	public int getStatus() {
		return status;
	}

	public void setStatus(int status) {
		this.status = status;
	}

	public int getTemplateQuality() {
		return templateQuality;
	}

	public void setTemplateQuality(int templateQuality) {
		this.templateQuality = templateQuality;
	}

	public int getFileSizeDB() {
		return fileSizeDB;
	}

	public void setFileSizeDB(int fileSizeDB) {
		this.fileSizeDB = fileSizeDB;
	}

	public int getEvaluations() {
		return evaluations;
	}

	public void setEvaluations(int evaluations) {
		this.evaluations = evaluations;
	}

	public String getJpgURL() {
		return jpgURL;
	}

	public void setJpgURL(String jpgURL) {
		this.jpgURL = jpgURL;
	}
}
