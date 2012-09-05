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

        public static readonly int RA_SWITCH_WEST = 0;
        public static readonly int RA_SWITCH_HOME = 1;
        public static readonly int DEC_SWITCH = 2;
        public static readonly int FLAG_MONTURA_ENCENDIDA = 3;
        public static readonly int FLAG_MONTURA_PROTEGIDA = 4;


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

        /// <summary>
        /// Cada bit de esta variable aporta info sobre estado de:
        /// - Limit Switches (RA, DEC)
        /// - Encendido Montura
        /// - Proteccion Montura
        /// </summary>
        private int flags;

        /// <summary>
        /// Los bits de la variable flag son desmenuzados en esta variable.
        /// Finalmente, las componentes del arreglo flag, son asignadas
        /// a las variables Boolean.
        /// </summary>
        private Boolean[] flag;

        /// <summary>
        /// True si el telescopio alcanza el limite East.
        /// </summary>
        private Boolean raLimitEast;

        /// <summary>
        /// True si el telescopio alcanza el limite West.
        /// </summary>
        private Boolean raLimitWest;

        /// <summary>
        /// True si el telescopio esta en el Home en RA. (contrapeso hacia abajo).
        /// </summary>
        private Boolean raHome;

        /// <summary>
        /// True si el telescopio está apuntando hacia el polo sur.
        /// </summary>
        private Boolean decHome;

        /// <summary>
        /// True si el arduino de los Limits, detecta via I2C que la montura está encendida
        /// </summary>
        private Boolean monturaEncendida;

        /// <summary>
        /// True si el arduino de los limits, está apagando la montura en caso
        /// de una posición unsafe.
        /// </summary>
        private Boolean monturaProtegida;


        public Status(String linea)
        {
            this.linea = linea;
        }

        /// <summary>
        /// True si el telescopio está apuntando hacia el polo sur.
        /// </summary>
        public bool DecHome
        {
            get { return this.decHome; }
            set { this.decHome = value; }
        }

        /// <summary>
        /// True si el telescopio alcanza el limite East.
        /// </summary>
        public bool RaLimitEast
        {
            get { return this.raLimitEast; }
            set { this.raLimitEast = value; }
        }

        /// <summary>
        /// True si el telescopio alcanza el limite West.
        /// </summary>
        public bool RaLimitWest
        {
            get { return this.raLimitWest; }
            set { this.raLimitWest = value; }
        }

        /// <summary>
        /// True si el telescopio esta en el Home en RA. (contrapeso hacia abajo).
        /// </summary>
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
                this.flags = Int16.Parse(part[0]);
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
            if (part.Length >= 9)
            {
                try
                {
                    this.counterWeightAngleArduino = Double.Parse(part[7]) * (180.0 / Math.PI);
                    this.zenithAngleArduino = Double.Parse(part[8]) * (180.0 / Math.PI);
                    this.zenithCounter = long.Parse(part[9]);
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
            #region analisis Flags (Limits, Status Arduino)
            int valor;
            valor = this.flags;
            StringBuilder strBinary;
            strBinary = new StringBuilder();
            
            flag = new Boolean[10];
            for (int i = 1; i < 10; i++)
            {
                flag[i-1] = ((valor & (1 << i - 1)) != 0);
                if (flag[i-1]) 
                    strBinary.Append(1);
                else
                    strBinary.Append(0);
            }

            this.decHome = flag[DEC_SWITCH];
            this.raLimitEast = ((flag[1]) & flag[RA_SWITCH_WEST]);

            if (!raLimitEast)
            {
                this.raHome = flag[RA_SWITCH_HOME];
                this.raLimitWest = flag[RA_SWITCH_WEST];
            }
            this.monturaEncendida = flag[FLAG_MONTURA_ENCENDIDA];
            this.monturaProtegida = flag[FLAG_MONTURA_PROTEGIDA];

            #endregion

            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.Append("flags="); mensaje.Append(this.flags);
            mensaje.Append("\t strBinary="); mensaje.Append(strBinary);
            mensaje.Append("\t decHome="); mensaje.Append(this.decHome);
            mensaje.Append("\t raLimitEast="); mensaje.Append(raLimitEast);
            mensaje.Append("\t raLimitWest="); mensaje.Append(this.raLimitWest);
            mensaje.Append("\t raHome="); mensaje.Append(this.raHome);
            Console.WriteLine(mensaje.ToString());

        }
    }
}
