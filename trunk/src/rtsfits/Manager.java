/**
 * 1) Aplicación originalmente desarrollada para:
 * Vigila el log de rts2 ubicado en:
 * /var/log/rts2-debug
 * 
 * 
 * Cada vez que aparece una nueva linea, revisa si esta línea se refiere a la creación
 * de un nuevo archivo fits.
 * Si ese es el caso, crea un comando para copiar es archivo al pipeline de zwicky
 * 
 * 2) Se ha diseñado la aplicación de manera modular-generica, etc con el fín de
 * no solo detectar nuevos archivos y copiarlos sino ademas cualquier otra acción arbitraria.
 * 
 * Para esto, se ha definido la clase abstracta EventAction. En este Manager
 * se revisa una lista de EventAction los cuales modelan las distintas detecciones&acciones
 * que la aplicación puede realizar.
 */
package rtsfits;

import java.io.IOException;
import java.util.Scanner;
import java.util.Vector;

/**
 * @author eduardo
 *
 */
public class Manager {

	private Runtime runtime;
	private Process process;
	private Scanner scanner;

	/**
	 * Llamada a sistema. generalmente será un comando del tipo
	 * "tailf  /var/log/rts2-debug"
	 */
	private String systemCall;
	private Vector<EventAction> vctEventAcion;

	/**
	 * 
	 * @param systemCall El comando con la llamada a sistema cuya salida estandard se debe vigilar.
	 */
	public Manager(String systemCall)
	{
		this.runtime = null;
		this.process = null;
		this.scanner = null;
		this.systemCall = systemCall;
		vctEventAcion = new Vector<EventAction>();
	}

	/**
	 * Ejecuta una llamada a system y examina cada línea de la salida estandard del comando ejecutado
	 */
	public void vigilar()
	{		
		// LA interfaz con el sistema operativo
		runtime= Runtime.getRuntime();

		try {
			//La llamada a sistema con su proceso respectivo.
			process = runtime.exec(this.systemCall);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		// aqui esta el vigilante que revisa cada línea de la salida standard del comando ejecutado
		scanner = new Scanner(process.getInputStream());
		while (scanner.hasNextLine()) {
			String line = scanner.nextLine();
			for (EventAction evtAct : vctEventAcion) {
				evtAct.processLine(line);
			}
		}
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		System.out.println("Rts2Log Manager: Start");
		//test1();
		test2(args[0], Boolean.parseBoolean(args[1]));
		System.out.println("Rts2Log Manager: End");
	}

	/**
	 * Prueba que puede realizarse en una maquina diferente de baade, donde no están los archivos señalados 
	 * en el log de rts2.
	 * En este caso el log de rts2 es "rts2-debug.1.gz"
	 */
	private static void test1() {
		Manager man;
		MoveFits move;
		man = new Manager("gunzip -c /home/eduardo/workspace2/rts2Syntax/rts2-debug.1.gz");
		move = new MoveFits(false);
		man.getVctEventAcion().add(move);
		man.vigilar();
	}

	/**
	 * Esto permite que la aplicación funcione de manera genérica
	 * @param comando
	 */
	private static void test2(String comando,boolean enable) {
		//System.out.println("Comando="+comando);
		Manager man;
		MoveFits move;
		man = new Manager(comando);
		move = new MoveFits(enable);
		man.getVctEventAcion().add(move);
		man.vigilar();
	}



	public Runtime getRuntime() {
		return runtime;
	}

	public void setRuntime(Runtime runtime) {
		this.runtime = runtime;
	}

	public Process getProcess() {
		return process;
	}

	public void setProcess(Process process) {
		this.process = process;
	}

	public Scanner getScanner() {
		return scanner;
	}

	public void setScanner(Scanner scanner) {
		this.scanner = scanner;
	}

	public String getSystemCall() {
		return systemCall;
	}

	public void setSystemCall(String systemCall) {
		this.systemCall = systemCall;
	}

	public Vector<EventAction> getVctEventAcion() {
		return vctEventAcion;
	}

	public void setVctEventAcion(Vector<EventAction> vctEventAcion) {
		this.vctEventAcion = vctEventAcion;
	}



}
