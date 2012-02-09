using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using log4net;

namespace ASCOM.Meteo02
{
    /// <summary>
    /// Registro de variables meteorologicas.
    /// Los registros son leidos desde la base de datos del CTIO
    /// y con convertidas al formato esperado por ASCOM.
    /// </summary>
    public class WeatherRow
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WeatherRow));
        /*  Constantes para el calculo del DewPoint
         * http://www.paroscientific.com/dewpoint.htm
        with      a = 17.27  	
        and       b=237.7 [°C]   
         */
        private const double aa = 17.27;
        private const double bb = 237.7;//[°C] 

        #region Variables de Instancia
        /// <summary>
        /// Fecha y hora del ultimo registro leido.
        /// </summary>
        private DateTime fechaHora;

        /// <summary>
        /// Humedad Relativa
        /// </summary>
        private float relativeHumidity;

        /// <summary>
        /// Temperatura Ambiente
        /// </summary>
        private float ambientTemperature;

        /// <summary>
        /// Temperatura de rocio
        /// </summary>
        private float dewPoint;

        /// <summary>
        /// Presion Barometrica
        /// </summary>
        private float barometricPressure;

        /// <summary>
        /// Direccion del Viento
        /// </summary>
        private float windDirection;

        /// <summary>
        /// Rapidez del viento. 
        /// Velocidad es un vector (rapidez + direccion)
        /// </summary>
        private float windSpeed;

        private Boolean hasErrors;
        #endregion

        /// <summary>
        /// La rapidez del viento lo convierte de (millas por hora) a (knots/hora)
        /// </summary>
        /// <param name="registro"></param>
        public WeatherRow(tololoDataSet.DataTableWeatherRow registro)
        {

            if (
                 (registro.HasErrors) ||

                 (registro.IspresNull()) ||
                 (registro.IstempNull()) ||
                 (registro.IshumNull()) ||
                 (registro.IswdirNull()) ||
                 (registro.IswspeedNull())
                )
            {
                this.hasErrors = true;
                logger.Error("WeatherRow: Se ha recibido registro con errores, DateTime=" + registro.time);
                //Console.WriteLine("WeatherRow: Se ha recibido registro con errores, DateTime=" + registro.time);
                //return;
            }
            this.hasErrors = false;


            this.fechaHora = registro.time;

            try
            {
                this.relativeHumidity = ((registro.hum / 100.0f)); // to percentage
            }
            catch (StrongTypingException ex)
            {
                Console.WriteLine("Nuevo registro tiene humedad=null messaje=" + ex.Message);
            }

            try
            {
                this.ambientTemperature = registro.temp;
            }
            catch (StrongTypingException ex)
            {
                Console.WriteLine("Nuevo registro tiene temp=null messaje=" + ex.Message);
            }

            try
            {
                this.barometricPressure = registro.pres;
            }
            catch (StrongTypingException ex)
            {
                Console.WriteLine("Nuevo registro tiene barometricPressure=null messaje=" + ex.Message);
            }

            try
            {
                this.windSpeed = (registro.wspeed * 0.868976242f); // mph to knots
            }
            catch (StrongTypingException ex)
            {
                Console.WriteLine("Nuevo registro tiene windSpeed=null messaje=" + ex.Message);
            }

            try
            {
                this.windDirection = registro.wdir; //Check if directions match. (between ctio database and what ACP is expecting.
            }
            catch (StrongTypingException ex)
            {
                Console.WriteLine("Nuevo registro tiene windSpeed=null messaje=" + ex.Message);
            }
            this.refreshDewPoint();
        }

        /// <summary>
        /// http://www.paroscientific.com/dewpoint.htm 
        /// This report describes the calculations required for determining the dew point temperature, using the measured air temperature and relative humidity provided by the MET4 or Fan-Aspirated MET4A. The algorithm is based on the Magnus-Tetens formula, over the range  
        /// 0° C < T < 60° C  
        /// 0.01 < RH < 1.00  
        /// 0° C < Td < 50° C  
        /// where T is the measured temperature [°C]
        /// RH is the measured relative humidity
        /// and Td  is the calculated dew point temperature [°C]
        /// </summary>
        private void refreshDewPoint()
        {
            double alphaT_RH;
            alphaT_RH = ((aa * this.ambientTemperature) / (bb + this.ambientTemperature)) + Math.Log(this.relativeHumidity);
            this.dewPoint = (float)((bb * alphaT_RH) / (aa - alphaT_RH));
        }

        public override string ToString()
        {
            StringBuilder respuesta;
            respuesta = new StringBuilder();
            respuesta.Append(this.fechaHora);
            respuesta.Append("\t Temp="); respuesta.Append(this.ambientTemperature);
            respuesta.Append("\t Press="); respuesta.Append(this.barometricPressure);
            respuesta.Append("\t dewPoint="); respuesta.Append(this.dewPoint);
            respuesta.Append("\t Hum="); respuesta.Append(this.relativeHumidity);
            respuesta.Append("\t WindDir="); respuesta.Append(this.windDirection);
            respuesta.Append("\t Wnd[knots/h]="); respuesta.Append(this.windSpeed);
            return respuesta.ToString();
        }


        #region Propiedades

        /// <summary>
        /// Indica si el constructor de esta instancia detecto algun error.
        /// </summary>
        public Boolean HasErrors
        {
            get { return this.hasErrors; }
            set { this.hasErrors = value; }
        }

        public DateTime FechaHora
        {
            get { return this.fechaHora; }
            set { this.fechaHora = value; }
        }



        /// <summary>
        /// Humedad Relativa
        /// </summary>
        public float RelativeHumidity
        {
            get { return this.relativeHumidity; }
            set { this.relativeHumidity = value; }
        }

        /// <summary>
        /// Temperatura Ambiente
        /// </summary>
        public float AmbientTemperature
        {
            get { return this.ambientTemperature; }
            set { this.ambientTemperature = value; }
        }

        /// <summary>
        /// Temperatura de rocio
        /// </summary>
        public float DewPoint
        {
            get { return this.dewPoint; }
            set { this.dewPoint = value; }
        }

        /// <summary>
        /// Presion Barometrica
        /// </summary>
        public float BarometricPressure
        {
            get { return this.barometricPressure; }
            set { this.barometricPressure = value; }
        }

        /// <summary>
        /// Direccion del Viento
        /// </summary>
        public float WindDirection
        {
            get { return this.windDirection; }
            set { this.windDirection = value; }
        }

        /// <summary>
        /// Rapidez del viento. 
        /// Velocidad es un vector (rapidez + direccion)
        /// este valor está en knots/hora
        /// </summary>
        public float WindSpeed
        {
            get { return this.windSpeed; }
            set { this.windSpeed = value; }
        }
        #endregion


    }
}
