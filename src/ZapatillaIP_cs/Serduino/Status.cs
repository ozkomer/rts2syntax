using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Serduino
{

    public class Status
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Status));

        private Acelerometro aRA = new Acelerometro();
        private Acelerometro aDEC = new Acelerometro();

        // Angulo entre el Zenith y la posición del tubo del telescopio. [en grados Sexagesimales].
        private double zenithAngle;

        // Angulo del eje contrapeso. [en grados Sexagesimales].
        private double counterWeightAngle;

        // Angulo del eje Declinación. [en grados Sexagesimales].
        private double declinationAngle;

        // Angulo entre el Zenith y la posición del tubo del telescopio. [en grados Sexagesimales].
        private double zenithAngleArduino;

        // Angulo del eje contrapeso. [en grados Sexagesimales].
        private double counterWeightAngleArduino;

        //Conteo enviado por el arduino, correspondiente a la cantidad de veces que se ha
        // sobrepasado el limite del angulo zenital
        private long zenithCounter;

        /// <summary>
        /// True si el arduino de los Limits, detecta via I2C que la montura está encendida
        /// </summary>
        private Boolean monturaEncendida;

        /// <summary>
        /// True si el arduino de los limits, está apagando la montura en caso
        /// de una posición unsafe.
        /// </summary>
        private Boolean monturaProtegida;

        static Serduino.Properties.Settings settings = Properties.Settings.Default;

        public static readonly Triplet Zenith =
            new Triplet(settings.ZenithX, settings.ZenithY, settings.ZenithZ);

        public static readonly Triplet SouthPole =
            new Triplet(settings.SouthPoleX, settings.SouthPoleY, settings.SouthPoleZ);

        public static Triplet[] RotationMatrix = { 
     new Triplet (0.54531395811267800, -0.03060392726828150, -0.83765195958912100),
     new Triplet (-0.41659904968013800, 0.85727673350022100, -0.30250745347287200),
     new Triplet (0.72735317251892400, 0.51393474997979400, 0.45473742488684700)
                                                 };
        //public static readonly Triplet SouthPoleB =
        //    new Triplet(0.89048047955066900, 0.45291181233736400, 0.04356112591060290 );

        private String linea;
        private int interruptores;
        private bool[] interruptor;
        private bool raLimitEast;
        private bool raLimitWest;
        private bool raHome;
        private bool decHome;

        public Status(String linea)
        {
            this.linea = linea;
            interruptor = new bool[3];
        }

        public bool DecHome
        {
            get { return this.decHome; }
            set { this.decHome = value; }
        }

        public bool RaLimitEast
        {
            get { return this.raLimitEast; }
            set { this.raLimitEast = value; }
        }

        public bool RaLimitWest
        {
            get { return this.raLimitWest; }
            set { this.raLimitWest = value; }
        }

        public bool RaHome
        {
            get { return this.raHome; }
            set { this.raHome = value; }
        }


        public Acelerometro AcelerometroRA
        {
            get { return aRA; }
            set { aRA = value; }
        }

        public Acelerometro AcelerometroDEC
        {
            get { return aDEC; }
            set { aDEC = value; }
        }

        /// <summary>
        /// Angulo entre el Zenith y la posición del tubo del telescopio. [en grados Sexagesimales].
        /// </summary>
        public double ZenithAngle
        {
            get { return this.zenithAngle; }
        }

        /// <summary>
        /// Angulo del eje contrapeso. [en grados Sexagesimales].
        /// </summary>
        public double CounterWeightAngle
        {
            get { return this.counterWeightAngle; }
        }

        /// <summary>
        /// Angulo del eje Declinación. [en grados Sexagesimales].
        /// </summary>
        public double DeclinationAngle
        {
            get { return this.declinationAngle; }
        }

        /// <summary>
        /// Angulo entre el Zenith y la posición del tubo del telescopio. [en grados Sexagesimales].
        /// Este valor ha sido calculado por el Arduino.
        /// </summary>
        public double ZenithAngleArduino
        {
            get { return this.zenithAngleArduino; }
        }

        /// <summary>
        /// Angulo del eje contrapeso. [en grados Sexagesimales].
        /// </summary>
        public double CounterWeightAngleArduino
        {
            get { return this.counterWeightAngleArduino; }
        }

        public long ZenithCounter
        {
            get { return this.zenithCounter; }
        }

        public Boolean MonturaEncendida
        {
            get { return this.monturaEncendida; }
            set { this.monturaEncendida = value; }
        }

        public Boolean MonturaProtegida
        {
            get { return this.monturaProtegida; }
            set { this.monturaProtegida = value; }
        }

        private void refreshZenithAngle()
        {
            double angleRad;
            double dotProduct;
            dotProduct = Triplet.dotProduct(this.aDEC.AcelerationUnit, Status.Zenith);
            angleRad = Math.Acos(dotProduct);
            this.zenithAngle = ((angleRad * 180.0) / Math.PI);
        }

        private void refreshDeclinationAngle()
        {
            double angleRad;
            double dotProduct;
            Triplet DECcrossSouthPole;
            Byte OctantDECcrossSouthPole;
            Triplet SouthPoleB;
            SouthPoleB = Triplet.normalized(Triplet.matrixProduct(Status.RotationMatrix, this.aRA.AcelerationUnit));
            dotProduct = Triplet.dotProduct(this.aDEC.AcelerationUnit, SouthPoleB);
            DECcrossSouthPole = Triplet.crossProduct(this.aDEC.AcelerationUnit, SouthPoleB);
            OctantDECcrossSouthPole = Triplet.Octant(DECcrossSouthPole);
            Console.WriteLine("SouthPoleB=" + SouthPoleB.ToString());
            Console.WriteLine("OctantDECcrossSouthPole=" + OctantDECcrossSouthPole.ToString());
            angleRad = Math.Acos(dotProduct);
            if (OctantDECcrossSouthPole < 6)
            {
                angleRad *= -1.0;
            }
            this.declinationAngle = ((angleRad * 180.0) / Math.PI);
            Console.WriteLine("DEC_RA[º]=" + this.declinationAngle);
        }

        private void refreshCounterWeightAngle()
        {
            double angleRad;
            double dotProduct;
            Triplet ARcrossSouthPole;
            Byte OctantARcrossSouthPole;
            dotProduct = Triplet.dotProduct(this.aRA.AcelerationUnit, Status.SouthPole);
            ARcrossSouthPole = Triplet.crossProduct(this.aRA.AcelerationUnit, Status.SouthPole);
            OctantARcrossSouthPole = Triplet.Octant(ARcrossSouthPole);
            //Console.WriteLine("ARcrossSouthPole=" + ARcrossSouthPole.ToString());
            //Console.WriteLine("OctantARcrossSouthPole=" + OctantARcrossSouthPole.ToString());
            angleRad = Math.Acos(dotProduct);
            if (OctantARcrossSouthPole > 0)
            {
                angleRad *= -1.0;
            }
            angleRad += Math.PI;
            this.counterWeightAngle = ((angleRad * 180.0) / Math.PI);
        }

        /// <summary>
        /// Analiza una linea enviada por el microcontrolador de los Limits.
        /// Esta linea contiene información sobre:
        /// - Limit Switches
        /// - Acelerómetros
        /// - Cwa, Angulo del contrapeso
        /// - Z, ángulo Zenital
        /// - Indicador de protección de la montura
        /// - Indicador de encendido de la montura
        /// </summary>
        public void Analiza()
        {
            String[] part;
            part = this.linea.Split((" ").ToCharArray());
            //for (int i = 0; i < part.Length; i++)
            //{
            //    Console.WriteLine("part[" + i + "]=" + part[i]);
            //}
            try
            {
                this.interruptores = Int16.Parse(part[0]);
            }
            catch (FormatException exc)
            {
                logger.Error("linea=" + this.linea);
                logger.Error(exc.Message);
                return;
            }
            if (part.Length >= 6)
            {
                Triplet analogReadRA;
                Triplet analogReadDEC;
                try
                {
                    analogReadRA = new Triplet(Double.Parse(part[1]), Double.Parse(part[2]), Double.Parse(part[3]));
                    analogReadDEC = new Triplet(Double.Parse(part[4]), Double.Parse(part[5]), Double.Parse(part[6]));
                }
                catch (FormatException exc)
                {
                    logger.Error("linea=" + this.linea);
                    logger.Error(exc.Message);
                    return;
                }
                aRA.AnalogRead = analogReadRA;
                aDEC.AnalogRead = analogReadDEC;
                aRA.refreshAcceleration();
                aDEC.refreshAcceleration();
                //Console.WriteLine("Dec Unit= " + aDEC.AcelerationUnit.ToString());
                this.refreshZenithAngle();
                this.refreshCounterWeightAngle();
                this.refreshDeclinationAngle();
            }
            if (part.Length >= 11)
            {
                try
                {
                    this.counterWeightAngleArduino = Double.Parse(part[7]) * (180.0 / Math.PI);
                    this.zenithAngleArduino = Double.Parse(part[8]) * (180.0 / Math.PI);
                    this.zenithCounter = long.Parse(part[9]);
                    this.monturaEncendida = part[10].Equals("1");
                    this.monturaProtegida = part[11].Trim().Equals("1");
                }
                catch (FormatException exc)
                {
                    logger.Error("linea=" + this.linea);
                    logger.Error(exc.Message);
                    return;
                }
                //Console.WriteLine("DeltaAngles = (" + (this.counterWeightAngleArduino - this.counterWeightAngle) + "," + (this.zenithAngleArduino - this.zenithAngle) + ")");
                Console.WriteLine("Arduino Angles = (" + (this.counterWeightAngleArduino) + "," + (this.zenithAngleArduino) + ")");
            }
            #region analisis interruptores
            int valor;
            valor = this.interruptores;

            for (int i = 0; i < 3; i++)
            {
                this.interruptor[i] = ((valor % 2) == 1);
                valor = (valor / 2);
            }

            this.decHome = interruptor[0];
            this.raLimitEast = ((interruptor[1]) & interruptor[2]);

            if (!raLimitEast)
            {
                this.raHome = interruptor[1];
                this.raLimitWest = interruptor[2];
            }

            #endregion

            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.Append("interruptores="); mensaje.Append(this.interruptores);
            mensaje.Append("\t decHome="); mensaje.Append(this.decHome);
            mensaje.Append("\t raLimitEast="); mensaje.Append(raLimitEast);
            mensaje.Append("\t raLimitWest="); mensaje.Append(this.raLimitWest);
            mensaje.Append("\t raHome="); mensaje.Append(this.raHome);
            Console.WriteLine(mensaje.ToString());

        }
    }
}
