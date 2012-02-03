/**
 * 
 */
package cl.calan.ctio;

import java.sql.Connection;
import java.sql.Date;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Vector;

import org.apache.log4j.Logger;
import org.apache.log4j.xml.DOMConfigurator;

/**
 * La base de datos "tololo" tiene una tabla "weather" donde se almacena un 
 * nuevo registro cada 5 minutos aproximadamente.
 * Luego, 288 registros equivalen a un dia completo.
 * @author sysop
 *
 */
public class TololoWeather {
	private static final Logger logger = Logger.getLogger(TololoWeather.class);

	private int countWeather;
	private Connection connect = null;
	private Statement statement = null;
	private ResultSet resultSet = null;

	/**
	 * Lista con la información de los últimos 288 registros de la tabla weather de la
	 * base de datos del clima de tololo.
	 */
	private Vector<WeatherRow> weatherList;

	/**
	 * 
	 */
	public TololoWeather() {
		// TODO Auto-generated constructor stub
	}

	/**
	 * Inicia una sesion con la base de datos meteorologica de Cerro Tololo.
	 */
	public void conectar() {
		logger.info("Conectando con Base de datos");
		int cantErrores;
		cantErrores = 0;
		// This will load the MySQL driver, each DB has its own driver
		try {
			Class.forName("com.mysql.jdbc.Driver");
		} catch (ClassNotFoundException e) {
			cantErrores++;
			logger.error("Error cargando my SQL driver:");
			logger.error(e.getMessage());
		}
		// Setup the connection with the DB
		try {
			connect = DriverManager
			.getConnection("jdbc:mysql://139.229.13.202/tololo?"
					+ "user=webuser&password=webuser_sql");
		} catch (SQLException e) {
			cantErrores++;
			logger.error("iniciando sesion con base datos Tololo.");
			logger.error(e.getMessage());
		}
		if (cantErrores>0)
		{
			logger.info("Cantidad Errores ="+cantErrores);
			connect = null;
			return;
		}				
		logger.info("Conexion exitosa.");
	}

	/**
	 * Obtiene los últimos 288 registros de la tabla weather.
	 */
	private void refreshWeather ()
	{
		logger.info("refreshWeather ();;Start.");
		int first, last;
		last = this.countWeather;
		first = this.countWeather - 288;

		// Statements allow to issue SQL queries to the database
		try {
			statement = connect.createStatement();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		StringBuilder query;
		query = new StringBuilder();
		query.append("SELECT * FROM weather  LIMIT ");
		query.append(first);
		query.append(", ");
		query.append(last);
		logger.info("EjecutandoQuery"+query.toString());
		// Result set get the result of the SQL query
		try {
			resultSet = statement
			.executeQuery(query.toString());
		} catch (SQLException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		this.weatherList = WeatherRow.getListFrom(resultSet);
		if (weatherList.size()!=288)
		{
			logger.error("weatherList.size()!=288;;weatherList.size()="+weatherList.size());
		}
		for (int i=0;i<weatherList.size();i++)
		{
			System.out.println(weatherList.elementAt(i).toString());
		}
		logger.info("refreshWeather ();;End.");
	}

	/**
	 * Obtiene la cantidad de registros de la tabla weather.
	 */
	private void refreshCountWeather() {
		// Statements allow to issue SQL queries to the database
		try {
			statement = connect.createStatement();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		// Result set get the result of the SQL query
		try {
			resultSet = statement
			.executeQuery("SELECT count(*) FROM weather");
		} catch (SQLException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}

		countWeather = -1;
		try {
			resultSet.next();
			countWeather =  resultSet.getInt(1);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		//writeResultSet(resultSet);
		logger.info("cantidadRegistrosWeather="+countWeather);
	}

	// You need to close the resultSet
	private void close() {
		logger.info("Cerrando base de datos.");
		try {
			if (resultSet != null) {
				resultSet.close();
			}

			if (statement != null) {
				statement.close();
			}

			if (connect != null) {
				connect.close();
			}
		} catch (Exception e) {
			logger.error("Error Cerrando base de datos.");
		}
		logger.info("Base de datos Cerrada.");

	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		DOMConfigurator.configure("logging.xml");
		logger.info("Tololo Weather Start.");
		TololoWeather wea;
		wea = new TololoWeather();
		wea.conectar();
		wea.refreshCountWeather();
		wea.refreshWeather();
		wea.close();
		logger.info("Tololo Weather End.");

	}

}
