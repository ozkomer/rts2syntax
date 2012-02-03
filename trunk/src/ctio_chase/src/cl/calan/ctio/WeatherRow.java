/**
 * 
 */
package cl.calan.ctio;

import java.sql.Date;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.GregorianCalendar;
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
	
	/**
	 * 
	 */
	public WeatherRow() {
		// TODO Auto-generated constructor stub
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

}
