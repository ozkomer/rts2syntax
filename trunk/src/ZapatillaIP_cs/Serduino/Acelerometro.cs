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

        /// <summary>
        /// Calcula el octante del vector recibido utilizando la
        /// convención "+=1"
        /// </summary>
        /// <param name="V"></param>
        /// <returns></returns>
        public static Byte Octant(Triplet V)
        {
            Byte respuesta;
            respuesta = 0;
            if (V.X >= 0) respuesta += 1;
            if (V.Y >= 0) respuesta += 2;
            if (V.Z >= 0) respuesta += 4;
            return respuesta;
        }

        /// <summary>
        /// Calcula el producto PUNTO entre los vectores recibidos como parámetros.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double dotProduct(Triplet A, Triplet B)
        {
            double respuesta;
            respuesta = (A.X * B.X);
            respuesta += (A.Y * B.Y);
            respuesta += (A.Z * B.Z);
            return respuesta;
        }

        /// <summary>
        /// Calcula el producto matricial MxA
        /// </summary>
        /// <param name="M">La Matriz (esta matriz premultiplica.</param>
        /// <param name="A">El vector a multiplicar.</param>
        /// <returns></returns>
        public static Triplet matrixProduct(Triplet[] M, Triplet A)
        {
            Triplet respuesta;
            respuesta = new Triplet();
            respuesta.X = Triplet.dotProduct(M[0], A);
            respuesta.Y = Triplet.dotProduct(M[1], A);
            respuesta.Z = Triplet.dotProduct(M[2], A);
            return respuesta;
        }

        /// <summary>
        /// Calcula el producto CRUZ entre los vectores recibidos como parámetros.
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Triplet crossProduct(Triplet A, Triplet B)
        {
            Triplet C = new Triplet();
            C.X = ((A.Y * B.Z) - (A.Z * B.Y));
            C.Y = ((A.Z * B.X) - (A.X * B.Z));
            C.Z = ((A.X * B.Y) - (A.Y * B.X));
            return C;
        }

        /// <summary>
        /// Calcula la magnitud de este vector.
        /// </summary>
        /// <returns></returns>
        public static double magnitude(Triplet V)
        {
            double magnitud;
            magnitud = 0;
            magnitud =  Math.Sqrt(
                (V.X * V.X) + (V.Y * V.Y) + (V.Z * V.Z)
                );
            return magnitud;
        }

        /// <summary>
        /// Crea el vector unitario del vector recibido como parametro
        /// </summary>
        /// <param name="V"></param>
        /// <returns></returns>
        public static Triplet normalized(Triplet V)
        {
            double magnitud;
            magnitud = Triplet.magnitude( V );
            Triplet respuesta;
            if (magnitud == 0)
            {
                return V;
            }
            respuesta.X = (V.X / magnitud);
            respuesta.Y = (V.Y / magnitud);
            respuesta.Z = (V.Z / magnitud);
            return respuesta;
        }

        public override string ToString()
        {
            StringBuilder respuesta;
            respuesta = new StringBuilder();
            respuesta.Append("(");
            Boolean extended;
            extended = false;
            if (extended)
            {
                respuesta.Append(X);
                respuesta.Append(",");
                respuesta.Append(Y);
                respuesta.Append(",");
                respuesta.Append(Z);
            }
            else
            {
                respuesta.Append(X.ToString("0.0000"));
                respuesta.Append(",");
                respuesta.Append(Y.ToString("0.0000"));
                respuesta.Append(",");
                respuesta.Append(Z.ToString("0.0000"));
            }
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
        public static readonly double VREF = 3.33;//[Volts] Medido el Viernes 29 Junio 2012 con 2 testers diferentes
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
            acelerationUnit = Triplet.normalized(aceleration);
        }
    }
}
