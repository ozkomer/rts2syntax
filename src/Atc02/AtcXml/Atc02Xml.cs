using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;

namespace AtcXml
{
    public class Atc02Xml
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AtcXml.Atc02Xml));

        private DateTime timeStamp;
        private double stabilizationTemperature;
        private int fanPower;
        private double primaryTemperature;
        private double secondaryTemperature;
        private double focusPosition;
        private double shutterStatus;
        private double ambientTemperature;
        private double relativeHumidity;
        private double pressure;
        private double dewPoint;

        private XmlDocument xDoc;

        public Atc02Xml(String strXML)
        {
            if (strXML.Length > 0)
            {
                xDoc = new XmlDocument();
                try
                {
                    xDoc.LoadXml(strXML);
                }
                catch (XmlException)
                {
                    this.timeStamp = DateTime.MinValue;
                    return;
                }

                long ticks;
                ticks = long.Parse(contenidoNodo("timeStamp"));
                this.timeStamp = new DateTime(ticks);
                this.stabilizationTemperature = Double.Parse(contenidoNodo("stabilizationTemperature"));
                this.fanPower = Int32.Parse(contenidoNodo("fanPower"));
                this.primaryTemperature = Double.Parse(contenidoNodo("primaryTemperature"));
                this.secondaryTemperature = Double.Parse(contenidoNodo("secondaryTemperature"));
                this.focusPosition = Double.Parse(contenidoNodo("focusPosition"));
                this.shutterStatus = Double.Parse(contenidoNodo("shutterStatus"));
                this.ambientTemperature = Double.Parse(contenidoNodo("ambientTemperature"));
                this.relativeHumidity = Double.Parse(contenidoNodo("relativeHumidity"));
                this.pressure = Double.Parse(contenidoNodo("pressure"));
                this.dewPoint = Double.Parse(contenidoNodo("dewPoint"));
            }
            else
            {
                this.timeStamp = DateTime.MinValue;
            }
        }

        /// <summary>
        /// Ecuación propia del sistema de enfoque del Chase500
        /// que convierte el BFL en los Steps (Para Stepper Motors)
        /// que cuenta el firmware para desplazar el espejo secundario
        /// </summary>
        /// <param name="FocStep"></param>
        /// <returns></returns>
        public static int BflToFocSetp(Double focusPosition)
        {
            int respuesta;
            respuesta = (int)(100.0 * (focusPosition - 130.0));
            return respuesta;
        }

        private String contenidoNodo(String tag)
        {
            XmlNodeList nodo;
            nodo = this.xDoc.GetElementsByTagName(tag);
            return nodo[0].InnerText;
        }

        /// <summary>
        /// Revisa si la última fecha leida por el ATC02 es cercana
        /// a la fecha actual de el sistema.
        /// 
        /// Si la fecha es cercana, este metodo permitira ingresar la informacion
        /// leida del Officina Stellare ATC02 a los archivos .fits
        /// </summary>
        /// <returns></returns>
        public Boolean IsFresh()
        {
            Boolean respuesta;
            respuesta = false;
            TimeSpan vejezLectura; // Diferencia de tiempo entre el presente y la ultima lectura del ATC02
            vejezLectura = DateTime.Now.Subtract(this.timeStamp);
            if (vejezLectura.TotalSeconds < 120)
            {
                respuesta = true;
            }
            else
            {
                logger.Warn("Ultima lectura, desfasada c/r al presente por " + vejezLectura.TotalSeconds + " segundos.");
            }
            return respuesta;
        }


        public DateTime Timestamp { get { return this.timeStamp; } }
        public double StabilizationTemperature { get { return this.stabilizationTemperature; } }
        public int FanPower { get { return this.fanPower; } }
        public double PrimaryTemperature { get { return this.primaryTemperature; } }
        public double SecondaryTemperature { get { return this.secondaryTemperature; } }
        public double FocusPosition { get { return this.focusPosition; } }
        public double ShutterStatus { get { return this.shutterStatus; } }
        public double AmbientTemperature { get { return this.ambientTemperature; } }
        public double RelativeHumidity { get { return this.relativeHumidity; } }
        public double Pressure { get { return this.pressure; } }
        public double DewPoint { get { return this.dewPoint; } }
    }

}
