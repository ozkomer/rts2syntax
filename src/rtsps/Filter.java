/**
 * 
 */
package rtsps;


/**
 * @author eduardo
 *
 */
public class Filter {

	/** Abreviacion del filtro */
	private String code;

	/** Cantidad de imagenes a obtener con este filtro*/
	private int repetitions;

	/**
	 * tiempo de exposicion en segundos
	 */
	private int exposure;

	public Filter(String code, int repetitions, int exposure) {
		super();
		this.code = code;
		this.repetitions = repetitions;
		this.exposure = exposure;
	}


	@Override
	public String toString() {
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append(FilterHeader.dictFiltro.get(code));
		sb.append("_rep=");
		sb.append(this.repetitions);
		sb.append("_exp=");
		sb.append(this.exposure);

		return sb.toString();
	}
	
	/**
	 * debe entregar algo del tipo 
	 * filter=CLR for 4 { E 20 }
	 * @return
	 */
	public String getScript ()
	{
		StringBuilder respuesta;
		respuesta = new StringBuilder();
		respuesta.append("filter=");
		respuesta.append(this.code);
		respuesta.append(" for ");
		respuesta.append(this.repetitions);
		respuesta.append(" { E ");
		respuesta.append(this.exposure);
		respuesta.append(" }");
		return respuesta.toString();
	}






	public String getCode() {
		return code;
	}





	public void setCode(String code) {
		this.code = code;
	}





	public int getRepetitions() {
		return repetitions;
	}





	public void setRepetitions(int repetitions) {
		this.repetitions = repetitions;
	}





	public int getExposure() {
		return exposure;
	}





	public void setExposure(int exposure) {
		this.exposure = exposure;
	}





	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

}
