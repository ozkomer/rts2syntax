/**
 * 
 */
package cl.calan.ctio;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Vector;

import org.apache.log4j.Logger;

/**
 * @author sysop
 *
 */
public class WeatherRow {
	private static final Logger logger = Logger.getLogger(WeatherRow.class);
	private double windSpeed;
	private double windDir;
	private double temperature;
	private int humidity;
	private int pressure;
	private String time;
	
	public final double MAX_WIND_SPEED = 30;
	public final double MAX_HUMIDITY = 75;
	
	
	/**
	 * 
	 */
	public WeatherRow() {
		// TODO Auto-generated constructor stub
	}
	
	/**
	 * Analiza solo los datos de este registro y determina si representa una condicion segura de Observación.
	 * @return
	 */
	public Boolean isSafe()
	{
		if (this.windSpeed>MAX_WIND_SPEED)
		{
			logger.info("Condiciones climaticas inseguras, razon: Windspeed="+this.windSpeed+" > "+MAX_WIND_SPEED+"=MAX_WIND_SPEED");
			return false;
		}
		if (this.humidity>MAX_HUMIDITY)
		{
			logger.info("Condiciones climaticas inseguras, razon: humidity="+this.humidity+" > "+MAX_HUMIDITY+"=MAX_HUMIDITY");
			return false;			
		}		
		return true;
	}
	
	/**
	 * Recibe un resultSet y lo convierte en una lista de elementos de esta clase.
	 * @param rs
	 * @return
	 */
	public static Vector<WeatherRow> getListFrom(ResultSet rs)
	{
		logger.info("getListFrom(ResultSet rs):");
		Vector<WeatherRow> respuesta;
		respuesta = new Vector<WeatherRow>();
		try {
			rs.beforeFirst();
		} catch (SQLException e) {
			logger.error("getListFrom(ResultSet rs):");
			logger.error(e.getMessage());
		}
		try {
			while (rs.next())
			{
				WeatherRow nuevo;
				nuevo = new WeatherRow();
				nuevo.windSpeed = rs.getDouble(1);
				nuevo.windDir		= rs.getDouble(2);
				nuevo.temperature 	= rs.getDouble(3);
				nuevo.humidity 		= rs.getInt(4);
				nuevo.pressure		= rs.getInt(5);
				nuevo.time			= rs.getString(6);
				respuesta.add(nuevo);
			}
		} catch (SQLException e) {
			logger.error("Error explorando resultSet:");
			logger.error(e.getMessage());
		}
		
		return respuesta;
	}
	
	

	@Override
	public String toString() {
		StringBuilder respuesta;
		respuesta = new StringBuilder();
		respuesta.append("time="+this.time);		
		respuesta.append("\t windSpeed="+this.windSpeed);
		respuesta.append("\t windDir="+this.windDir);
		respuesta.append("\t temperature="+this.temperature);
		respuesta.append("\t humidity="+this.humidity);
		respuesta.append("\t pressure="+this.pressure);
		return respuesta.toString();
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

	public double getWindSpeed() {
		return windSpeed;
	}

	public void setWindSpeed(double windSpeed) {
		this.windSpeed = windSpeed;
	}

	public double getWindDir() {
		return windDir;
	}

	public void setWindDir(double windDir) {
		this.windDir = windDir;
	}

	public double getTemperature() {
		return temperature;
	}

	public void setTemperature(double temperature) {
		this.temperature = temperature;
	}

	public int getHumidity() {
		return humidity;
	}

	public void setHumidity(int humidity) {
		this.humidity = humidity;
	}

	public int getPressure() {
		return pressure;
	}

	public void setPressure(int pressure) {
		this.pressure = pressure;
	}

	public String getTime() {
		return time;
	}

	public void setTime(String time) {
		this.time = time;
	}

	
}
