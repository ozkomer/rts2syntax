/**
 * 
 */
package rtsps;

/**
 * @author eduardo
 *
 */
public class RaDec {

	private HoraAngulo ra;
	private HoraAngulo dec;

	public RaDec (String[] ra,String[] dec)
	{
		this.ra = new HoraAngulo(ra);
		this.dec = new HoraAngulo(dec);
	}



	@Override
	public String toString() {
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append("'");
		sb.append(ra.formatoHora());
		sb.append(" ");
		sb.append(dec.formatoHora());
		
		sb.append("'");

		return sb.toString();
	}
	
	/**
	 * Para ser utilizado en el sitio:
	 * http://catserver.ing.iac.es/staralt/index.php
	 * @return
	 */
	public String getFormatoVisibilidad() {
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append(ra);
		sb.append(" ");
		sb.append(dec);
		return sb.toString();
	}




	public HoraAngulo getRa() {
		return ra;
	}



	public void setRa(HoraAngulo ra) {
		this.ra = ra;
	}



	public HoraAngulo getDec() {
		return dec;
	}



	public void setDec(HoraAngulo dec) {
		this.dec = dec;
	}





	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

}
