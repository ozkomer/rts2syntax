using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace ASCOM.Meteo02
{
    /// <summary>
    /// Estructura FIFO, los registros están ordenados en el tiempo.
    /// los registros recien añadidos estan hacia el presente.
    /// Los registros del final estan hacia el pasado.
    /// El ultimo registro es el más antiguo y el primero en eliminarse.
    /// Las eliminacion ocurre cuando se añade un nuevo registro,
    /// y el Time-Span completo supera la media hora.    /// </summary>
    public class WeatherAnalisis : Queue<WeatherRow>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WeatherRow));

        /// <summary>
        /// Ultimo registro ingresado.
        /// </summary>
        private WeatherRow ultimo;

        /// <summary>
        /// Velociad promedio del viento almacenado en esta estructura
        /// </summary>
        private double averageWindSpeed;

        /// <summary>
        /// Utilizado por el timerUnsafe
        /// </summary>
        public static readonly double _30Minutes_inMilliseconds = 1800000;

        /// <summary>
        /// Mientras este timer esta activo, el estado retornado siempre será UNSAFE.
        /// Este timer se activa cuando efectivamente el último registro ingresado
        /// representa un estado unsafe.
        /// </summary>
        private System.Timers.Timer timerUnsafe;

        /// <summary>
        /// La estructura solo debe albergar datos de hace menos de 30 minutos.
        /// Aqui es donde se especifica esta cantidad de Tiempo
        /// </summary>
        public static readonly TimeSpan mediaHora = new TimeSpan(0, 30, 0);

        /// <summary>
        /// El último dato recibido debe ser de hace menos de 5 minutos.
        /// Aqui es donde se especifica esta cantidad de Tiempo
        /// </summary>
        public static readonly TimeSpan cincoMinutos = new TimeSpan(0,5,0);

        private static Properties.Settings settings = Properties.Settings.Default;

        public static double MaxWindSpeed_inKnots { get { return settings.maxWindSpeed_inKnots; } }
        public static float MaxHumidity { get { return settings.maxHumidity; } }
        public static float MinDewPointDelta { get { return settings.minDewPointDelta; } }

        public WeatherAnalisis()
            : base()
        {
            ultimo = null;
            timerUnsafe = new System.Timers.Timer(_30Minutes_inMilliseconds);
            timerUnsafe.Elapsed += new System.Timers.ElapsedEventHandler(timerUnsafe_Elapsed);
            this.averageWindSpeed = 0;
        }

        private void timerUnsafe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger.Info("desActivando  timerUnsafe.");
            this.timerUnsafe.Stop();
        }

        /// <summary>
        /// Determina a partir de toda la informacion contenida en esta estructura,
        /// si las condiciones meteorologicas son aptas para la observacion.
        /// </summary>
        /// <returns>True si: hay condiciones meteorologicas aptas para la observacion.</returns>
        public bool isSafe()
        {
            bool safe;
            // Decimos que las condiciones son seguras salvo si...
            safe = true;
            // Si hace menos de media hora se declaro un "unsafe"
            if (this.timerUnsafe.Enabled)
            {
                // no se evalua nada, solo se retorna que sigue siendo unsafe observar
                safe = false;
            }
            else
            {
                //Consideraiones para la humedad relativa
                if (this.ultimo.RelativeHumidity > settings.maxHumidity)
                {
                    logger.Info("unsafe, razon:RelativeHumidity=" + this.ultimo.RelativeHumidity + " > " + settings.maxHumidity);
                    safe = false;
                }
                //Consideraiones para el viento
                //double averageWS;
                this.refreshAverageWindSpeed();
                if (this.averageWindSpeed > settings.maxWindSpeed_inKnots)
                {
                    logger.Info("unsafe, razon:AverageWindSpeed=" + this.averageWindSpeed + " > " + settings.maxWindSpeed_inKnots);
                    safe = false;
                }
                //Consideraiones para el dewPoint
                float deltaTemp; //Diferencia entre tenperaturaAmviente y Temperatura de Rocio
                deltaTemp = this.ultimo.AmbientTemperature - this.ultimo.DewPoint;
                if (deltaTemp < settings.minDewPointDelta)
                {

                    logger.Info("unsafe, razon:DewPoint=" + deltaTemp + " < " + settings.minDewPointDelta);
                    safe = false;
                }

                //Consideraciones para asegurar datos de hace menos de 5 minutos
                if (!this.safeCincoMinutos())
                {
                    logger.Info("unsafe, razon:Ultimo dato es de hace mas de 5 minutos; ultimo=" + this.ultimo.ToString());
                    safe = false;
                }

                // Si alguna de las consideraciones ANTERIORES nos lleva a un flanco de bajada.
                // E.D. de safe a unsafe, entonces iniciamos el timer. Y por lo 
                // Tando durante la siguiente media hora seguiremos en estado unsafe
                // pase lo que pase.
                if (!safe)
                {
                    logger.Info("activando  timerUnsafe.");
                    this.timerUnsafe.Start();
                }
            }
            return safe;
        }

        private void refreshAverageWindSpeed()
        {
            double suma;
            suma = 0;
            logger.Info("Calculando AverageWindSpeed.");

            foreach (WeatherRow registro in this)
            {
                suma += registro.WindSpeed;
            }
            averageWindSpeed = (suma / ((double)this.Count));
        }

        /// <summary>
        /// Solamente inserta si el nuevo registro tiene una estampa de tiempo mas reciente que el "ultimo" registro
        /// ingresado. El "ultimo" registro ingresado coincide ademas con el registro ubicado al final del Queue.
        /// </summary>
        /// <param name="nuevo">El registro a ingresar</param>
        public void insertar(WeatherRow nuevo)
        {
            //http://msdn.microsoft.com/en-us/library/x79949f0.aspx
            //if (ultimo is earlier than nuevo)
            if ((ultimo == null) || (ultimo.FechaHora.CompareTo(nuevo.FechaHora) < 0))
            {
                logger.Info("Insertando nuevo registro. registro=" + nuevo);
                this.Enqueue(nuevo);
                this.ultimo = nuevo;
                this.aseguraMediaHora();
            }
        }

        /// <summary>
        /// Asegura que todos los elementos de esta estructura fueron registrados hace menos de media hora.
        /// El correcto funcionamiento de este metodo requiere que:
        /// - La hora de este PC esté UTC.
        /// - La hora de los registros entrgados por la base de datos del CTIO esten en UTC.
        /// </summary>
        private void aseguraMediaHora()
        {
            DateTime haceMediaHora;
            WeatherRow primero;
            haceMediaHora = DateTime.Now.Subtract(WeatherAnalisis.mediaHora);
            while (this.Count > 0)
            {
                primero = this.Peek();
                // if (primero is earlier than haceMediaHora)
                if (primero.FechaHora.CompareTo(haceMediaHora) < 0)
                {
                    primero = this.Dequeue();
                    logger.Debug("registro eliminado:" + primero);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Evalua si el ultimo registro fue recibido hace menos de 5 minutos.
        /// </summary>
        /// <returns></returns>
        private bool safeCincoMinutos()
        {
            DateTime haceCincoMinutos;
            haceCincoMinutos = DateTime.Now.Subtract(WeatherAnalisis.cincoMinutos);
            // if (ultimo is earlier than haceCincoMinutos)
            if (this.ultimo.FechaHora.CompareTo(haceCincoMinutos) < 0)
            {
                logger.Warn("Ultimo registro es de hace mas de 5 minutos. ultimo="+ultimo.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ultimo registro ingresado.
        /// </summary>
        public WeatherRow Ultimo
        {
            get { return this.ultimo; }
        }

        public double AverageWindSpeed
        {
            get { return this.averageWindSpeed; }
        }

    }
}
