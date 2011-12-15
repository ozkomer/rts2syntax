/**
 * Manager recibira instancias de hijos de esta clase.
 * el Manager al vigilar, entregara la nueva linea encontrada.
 * y la pasará al objeto processLine.
 */
package rtsfits;

/**
 * @author eduardo
 *
 */
public abstract class EventAction {

	public abstract boolean processLine(String line);
	public abstract void action(String line);
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

}
