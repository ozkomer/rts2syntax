using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serduino
{
     
    public class Status
    {
        private Acelerometro aRA = new Acelerometro();
        private Acelerometro aDEC = new Acelerometro();

        // Angulo entre el Zenith y la posición del tubo del telescopio. [en grados Sexagesimales].
        private double zenithAngle;

        // Angulo del eje contrapeso. [en grados Sexagesimales].
        private double counterWeightAngle;

        // Angulo entre el Zenith y la posición del tubo del telescopio. [en grados Sexagesimales].
        private double zenithAngleArduino;

        // Angulo del eje contrapeso. [en grados Sexagesimales].
        private double counterWeightAngleArduino;


        public static readonly Triplet Zenith =
            new Triplet(-0.2116771931, 0.975214028, 0.0644233307);

        public static readonly Triplet SouthPole =
            new Triplet(0.3780225716, 0.3874084811 , -0.84084101);

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

        private void refreshZenithAngle()
        {
            double angleRad;
            double crossProduct;
            crossProduct = this.aDEC.AcelerationUnit.DotProduct(Status.Zenith);
            angleRad = Math.Acos ( crossProduct);
            this.zenithAngle = ((angleRad * 180.0) / Math.PI);
        }

        private void refreshCounterWeightAngle()
        {
            double angleRad;
            double crossProduct;
            crossProduct = this.aRA.AcelerationUnit.DotProduct(Status.SouthPole);
            angleRad = Math.Acos(crossProduct);
            this.counterWeightAngle = ((angleRad * 180.0) / Math.PI);
        }

        public void Analiza()
        {
            String[] part;
            part = this.linea.Split((" ").ToCharArray());
            for (int i = 0; i < part.Length; i++)
            {
                Console.WriteLine("part["+i+"]=" + part[i]);
            }
            this.interruptores = Int16.Parse(part[0]);
            if (part.Length >= 6)
            {
                Triplet analogReadRA;
                Triplet analogReadDEC;
                analogReadRA  = new Triplet(Double.Parse(part[1]), Double.Parse(part[2]), Double.Parse(part[3]));
                analogReadDEC = new Triplet(Double.Parse(part[4]), Double.Parse(part[5]), Double.Parse(part[6]));
                aRA.AnalogRead = analogReadRA;
                aDEC.AnalogRead = analogReadDEC;
                aRA.refreshAcceleration();
                aDEC.refreshAcceleration();
                this.refreshZenithAngle();
                this.refreshCounterWeightAngle();
            }
            if (part.Length >= 8)
            {
                this.counterWeightAngleArduino = Double.Parse(part[7]) *(180.0 / Math.PI);
                this.zenithAngleArduino = Double.Parse(part[8]) *(180.0 / Math.PI);
                //Console.WriteLine("DeltaAngles = (" + (this.counterWeightAngleArduino - this.counterWeightAngle) + "," + (this.zenithAngleArduino - this.zenithAngle) + ")");
                Console.WriteLine("Arduino Angles = (" + (this.counterWeightAngleArduino ) + "," + (this.zenithAngleArduino ) + ")");
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
            this.raLimitEast = (interruptor[1] & interruptor[2]);
            if (!raLimitEast)
            {
                this.raHome = interruptor[1];
                this.raLimitWest = interruptor[2];
            }

            #endregion

            //StringBuilder mensaje;
            //mensaje = new StringBuilder ();
            //mensaje.Append("interruptores="); mensaje.Append(this.interruptores);
            //mensaje.Append("\t decHome="); mensaje.Append(this.decHome);
            //mensaje.Append("\t raLimit1="); mensaje.Append(this.raLimitEast);
            //mensaje.Append("\t raLimit2="); mensaje.Append(this.raLimitWest);
            //mensaje.Append("\t raHome="); mensaje.Append(this.raHome);
            //mensaje.Append("RA="); mensaje.Append(aRA.Acceleration.ToString());
            //mensaje.Append("\t DEC="); mensaje.Append(aDEC.Acceleration.ToString());
            //Console.WriteLine(mensaje.ToString());

        }
    }
}
