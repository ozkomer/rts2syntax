using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serduino
{
    public class Acelerometro
    {
        private double x;
        private double y;
        private double z;
        public double maxX = Double.MinValue;
        public double maxY = Double.MinValue;
        public double maxZ = Double.MinValue;

        public double minX = Double.MaxValue;
        public double minY = Double.MaxValue;
        public double minZ = Double.MaxValue;

        public void setValues (double x,double y,double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            if (this.x > maxX) maxX = this.x;
            if (this.y > maxY) maxY = this.y;
            if (this.z > maxZ) maxZ = this.z;

            if (this.x < minX) minX = this.x;
            if (this.y < minY) minY = this.y;
            if (this.z < minZ) minZ = this.z;
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
        public double Z
        {
            get { return this.z; }
            set { this.z = value; }
        }

        public int RelativeX
        {
            get { return (int)(100.0 * ((this.x - minX) / (maxX - minX))); }
        }
        public int RelativeY
        {
            get { return (int)(100.0 * ((this.y - minY) / (maxY - minY))); }
        }
        public int RelativeZ
        {
            get { return (int)(100.0 * ((this.z - minZ) / (maxZ - minZ))); }
        }

        /// <summary>
        /// Calculos en base a Acelerometro.ods
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public static double getAlpha(Acelerometro acc)
        {
            double c2, c3, d2, d3, e3, e5, f9;
            c2 = 542; 
            c3 = 81;
            d2 = 554.5;
            d3 = -33.5;
            e3 = 87;
            e5 = 510;
            f9 = 1.35;//4.7123889804;
            double alpha1, alpha2, alpha3;
            double arg1, arg2, arg3;

            #region alpha1
            //Calculamos el argumento a entregar a la funcion arcoseno
            arg1 = ((acc.x - c2) / (c3));
            // aseguramos que el argumento esté en el rango de la funcion seno.
            arg1 = Math.Max(-1, arg1);
            arg1 = Math.Min(1, arg1);
            // Calculamos el arcoseno
            alpha1 = Math.Asin(arg1);
            #endregion

            #region alpha2
            //Calculamos el argumento a entregar a la funcion arcoseno
            arg2 = ((acc.y - d2) / (d3));
            // aseguramos que el argumento esté en el rango de la funcion seno.
            arg2 = Math.Max(-1, arg2);
            arg2 = Math.Min(1, arg2);
            // Calculamos el arcoseno
            alpha2 = Math.Asin(arg2);
            #endregion

            #region alpha3
            //Calculamos el argumento a entregar a la funcion arcoseno
            arg3 = ((acc.z - e5) / (e3));
            // aseguramos que el argumento esté en el rango de la funcion seno.
            arg3 = Math.Max(-1, arg3);
            arg3 = Math.Min(1, arg3);
            // Calculamos el arcoseno
            alpha3 = ((Math.Asin(arg3))+f9);
            #endregion
            Console.WriteLine("alfa123:\t" + alpha1 + "\t" + alpha2 + "\t" + alpha3);
            return (0.9 * alpha1 + 0.1 * alpha2);
        }
    }
}
