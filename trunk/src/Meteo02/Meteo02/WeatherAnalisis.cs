﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <summary>
        /// Ultimo registro ingresado.
        /// </summary>
        private WeatherRow ultimo;        

        /// <summary>
        /// Utilizado por el timerUnsafe
        /// </summary>
        public const double _30Minutes_inMilliseconds = 1800000;

        /// <summary>
        /// Mientras este timer esta activo, el estado retornado siempre será UNSAFE.
        /// Este timer se activa cuando efectivamente el último registro ingresado
        /// representa un estado unsafe.
        /// </summary>
        private System.Timers.Timer timerUnsafe;

        public static TimeSpan mediaHora = new TimeSpan(0, 30, 0);

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
        }

        private void timerUnsafe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
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
                    safe = false;
                }
                //Consideraiones para el viento
                if (this.getAverageWindSpeed() > settings.maxWindSpeed_inKnots)
                {
                    safe = false;
                }
                //Consideraiones para el dewPoint
                float deltaTemp; //Diferencia entre tenperaturaAmviente y Temperatura de Rocio
                deltaTemp = this.ultimo.AmbientTemperature - this.ultimo.DewPoint;
                if ( deltaTemp < settings.minDewPointDelta)
                {
                    safe = false;
                }

                // Si alguna de las consideraciones nos lleva a un flanco de bajada.
                // E.D. de safe a unsafe, entonces iniciamos el timer. Y por lo 
                // Tando durante la siguiente media hora seguiremos en estado unsafe
                // pase lo que pase.
                if (!safe)
                {
                    this.timerUnsafe.Start();
                }
            }
            return safe;
        }

        private double getAverageWindSpeed()
        {
            double suma;
            suma = 0;
            foreach (WeatherRow registro in this)
            {
                suma += registro.WindSpeed;
            }
            return (suma / ((double)this.Count));
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
            haceMediaHora = DateTime.Now.Subtract(mediaHora);
            while (this.Count > 0)
            {
                primero = this.Peek();
                // if (primero is earlier than haceMediaHora)
                if (primero.FechaHora.CompareTo(haceMediaHora)<0)
                {
                    primero = this.Dequeue();
                    Console.WriteLine("registro eliminado:" + primero);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Ultimo registro ingresado.
        /// </summary>
        public WeatherRow Ultimo
        {
            get { return this.ultimo; }
        }



    }
}
