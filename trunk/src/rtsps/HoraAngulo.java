/**
 * 
 */
package rtsps;

import java.text.DecimalFormat;

/**
 * @author eduardo
 *
 */
public class HoraAngulo {

	public static DecimalFormat DD = new DecimalFormat("00");
	
	int gg;
	int mm;
	double ss;


	public HoraAngulo(int gg, int mm, double ss) {
		super();
		this.gg = gg;
		this.mm = mm;
		this.ss = ss;
	}



	public HoraAngulo (String[] blocks)
	{
		if (blocks.length!=3){
			System.err.println("Error creando HoraAngulo.");
		}
		this.gg = Integer.parseInt(blocks[0]);
		this.mm = Integer.parseInt(blocks[1]);
		this.ss = Double.parseDouble(blocks[2]);
	}



	@Override
	public String toString() {
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append(DD.format(gg));
		sb.append(" ");
		sb.append(mm);
		sb.append(" ");
		sb.append(ss);
		return sb.toString();
	}
	
	public String formatoHora()
	{
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append(DD.format(gg));
		sb.append(":");
		sb.append(DD.format(mm));
		sb.append(":");
		sb.append(ss);
		return sb.toString();
	}



	public int getGg() {
		return gg;
	}



	public void setGg(int gg) {
		this.gg = gg;
	}



	public int getMm() {
		return mm;
	}



	public void setMm(int mm) {
		this.mm = mm;
	}



	public double getSs() {
		return ss;
	}



	public void setSs(double ss) {
		this.ss = ss;
	}

	public static void main (String[] args)
	{
		String RA;
		RA = "22 57 11.77";
		HoraAngulo ha;
		ha = new HoraAngulo(RA.split(" "));
		System.out.println("ha="+ha);
	}

}
