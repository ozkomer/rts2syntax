using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serduino
{
    public struct Triplet
    {
        public double X;
        public double Y;
        public double Z;

        public Triplet(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double DotProduct(Triplet B)
        {
            double respuesta;
            respuesta = (this.X * B.X);
            respuesta += (this.Y * B.Y);
            respuesta += (this.Z * B.Z);
            return respuesta;
        }

        public override string ToString()
        {
            StringBuilder respuesta;
            respuesta = new StringBuilder();
            respuesta.Append("(");
            respuesta.Append(X.ToString("0.0000"));
            respuesta.Append(",");
            respuesta.Append(Y.ToString("0.0000"));
            respuesta.Append(",");
            respuesta.Append(Z.ToString("0.0000"));
            respuesta.Append("#");
            respuesta.Append( ( (X*X) +  (Y*Y) + (Z*Z) ) );
            respuesta.Append(")");

            return respuesta.ToString();
        }
    }

    /// <summary>
    /// Los datos recibidos provienen de un acelerómetro ADXL335.
    /// Energizado a 3.3V (Vref), y digitalizado a 10 bits.
    /// El rango de aceleraciones de este sensor es [-3g,3g].
    /// Suponiendo una sensitividad de 0.55 volt/g, 
    /// 
    /// </summary>
    public class Acelerometro
    {
        public static readonly double VREF = 3.0;//[Volts]
        public static readonly double VZEROG = (VREF/2);//[Volts]
        public static readonly double BitsResolution = 10;//[bits]
        public static readonly double MaxAnalogRead = (Math.Pow(2, BitsResolution) - 1);// Valor Escalar.
        public static readonly double VoltsPerCount = (VREF / MaxAnalogRead);//[volts]
        public static readonly double Sensitivity = 0.33;//0.55;//[Volt/g]

        private Triplet analogRead;
        private Triplet aceleration;
        private Triplet acelerationUnit;

        public Triplet AnalogRead
        {
            get { return this.analogRead; }
            set { this.analogRead = value; }
        }

        public Triplet Acceleration
        {
            get { return this.aceleration; }
            set { this.aceleration = value; }
        }

        public Triplet AcelerationUnit
        {
            get { return this.acelerationUnit; }
            set { this.acelerationUnit = value; }
        }

        public void refreshAcceleration()
        {
            aceleration.X = ((analogRead.X * VoltsPerCount) - (VZEROG)) / Sensitivity;
            aceleration.Y = ((analogRead.Y * VoltsPerCount) - (VZEROG)) / Sensitivity;
            aceleration.Z = ((analogRead.Z * VoltsPerCount) - (VZEROG)) / Sensitivity;
            double squareSum;
            double CSquare;
            double cRoot;
            squareSum = ((aceleration.X * aceleration.X) +
                            (aceleration.Y * aceleration.Y) +
                            (aceleration.Z * aceleration.Z));
            CSquare = (1.0f / squareSum);
            cRoot = Math.Sqrt(CSquare);
            acelerationUnit.X = (aceleration.X * cRoot);
            acelerationUnit.Y = (aceleration.Y * cRoot);
            acelerationUnit.Z = (aceleration.Z * cRoot);
        }

    }
}
