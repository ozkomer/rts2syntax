package cl.calan.ctio.vr;



/**
 * @author eduardo
 *
 */
public class DeclinationError implements Comparable<DeclinationError>{

	private double angle;
	private double error;
	
	public DeclinationError(double angle, double error) {
		super();
		this.angle = angle;
		this.error = error;
	}

	
	public double getAngle() {
		return angle;
	}


	public void setAngle(double angle) {
		this.angle = angle;
	}


	public double getError() {
		return error;
	}


	public void setError(double error) {
		this.error = error;
	}


	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}


	@Override
	public int compareTo(DeclinationError o) {
		if (this.error<o.error) return -1;
		if (this.error==o.error) return 0;
		return 1;		
	}

}