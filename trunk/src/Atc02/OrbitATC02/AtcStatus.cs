using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.OrbitATC02
{
    public class AtcStatus
    {
        private double stabilizationTemperature;
        private int fanPower;
        private double primaryTemperature;
        private double secondaryTemperature;

        private double focusPosition;
        private int shutterStatus;
        private double ambientTemperature;
        private double relativeHumidity;
        private double pressure;
        private double dewPoint;

        /// <summary>
        /// Crea un status a partir de la informacion cruda que envia el ATC02
        /// </summary>
        /// <param name="rawStatus"></param>
        public AtcStatus(String rawStatus)
        {
            String[] lineArray;
            lineArray = rawStatus.Split(("\n").ToCharArray());
            for (int i = 0; i < lineArray.Length; i++)
            {
                String line;
                line = lineArray[i];
                Console.WriteLine("Linea " + i + "=" + line);
                if (line.StartsWith("STABT"))   { this.stabilizationTemperature = extraeDouble(line); }
                if (line.StartsWith("SETFAN"))  { this.fanPower                 = extraeInt(line); }
                if (line.StartsWith("PRITE"))   { this.primaryTemperature       = extraeDouble(line); }
                if (line.StartsWith("SECTE"))   { this.secondaryTemperature     = extraeDouble(line); }
                if (line.StartsWith("BFL"))     { this.focusPosition            = extraeDouble(line); }
                if (line.StartsWith("SHUTTER")) { this.shutterStatus            = extraeInt(line); }
                if (line.StartsWith("AMBTE"))   { this.ambientTemperature       = extraeDouble(line); }
                if (line.StartsWith("HUMID"))   { this.relativeHumidity         = extraeDouble(line); }
                if (line.StartsWith("PRES"))    { this.pressure                 = extraeDouble(line); }
                if (line.StartsWith("DEWPO"))   { this.dewPoint                 = extraeDouble(line); }
            }
        }

        public override string ToString()
        {
            StringBuilder respuesta;
            respuesta = new StringBuilder();
            respuesta.Append("stabilizationTemperature="); respuesta.Append(this.stabilizationTemperature);
            respuesta.AppendLine();
            respuesta.Append("fanPower="); respuesta.Append(this.fanPower);
            respuesta.AppendLine();
            respuesta.Append("primaryTemperature="); respuesta.Append(this.primaryTemperature);
            respuesta.AppendLine();
            respuesta.Append("secondaryTemperature="); respuesta.Append(this.secondaryTemperature);
            respuesta.AppendLine();
            respuesta.Append("focusPosition="); respuesta.Append(this.focusPosition);
            respuesta.AppendLine();
            respuesta.Append("shutterStatus="); respuesta.Append(this.shutterStatus);
            respuesta.AppendLine();
            respuesta.Append("ambientTemperature="); respuesta.Append(this.ambientTemperature);
            respuesta.AppendLine();
            respuesta.Append("relativeHumidity="); respuesta.Append(this.relativeHumidity);
            respuesta.AppendLine();
            respuesta.Append("pressure="); respuesta.Append(this.pressure);
            respuesta.AppendLine();
            respuesta.Append("dewPoint="); respuesta.Append(this.dewPoint);
            respuesta.AppendLine();
            return respuesta.ToString();
        }

        /// <summary>
        /// Extrae un numero entero ubicado despues del último espacio del texto recibido
        /// </summary>
        /// <param name="linea">En este texto se busca el último elemento y se parsea como Int32</param>
        /// <returns></returns>
        private static int extraeInt(String linea)
        {
            String[] part;
            int respuesta;
            part = linea.Split((" ").ToCharArray());
            respuesta = Int32.Parse(part[part.Length - 1]);
            return respuesta;
        }

        /// <summary>
        /// Extrae un numero entero ubicado despues del último espacio del texto recibido
        /// </summary>
        /// <param name="linea">En este texto se busca el último elemento y se parsea como Int32</param>
        /// <returns></returns>
        private static double extraeDouble(String linea)
        {
            String[] part;
            double respuesta;
            part = linea.Split((" ").ToCharArray());
            respuesta = Double.Parse(part[part.Length - 1]);
            return respuesta;
        }

        public double StabilizationTemperature
        {
            get { return this.stabilizationTemperature; }
            set { this.stabilizationTemperature = value; }
        }

        public int FanPower
        {
            get { return this.fanPower; }
            set { this.fanPower = value; }
        }

        public double PrimaryTemperature
        {
            get { return this.primaryTemperature; }
            set { this.primaryTemperature = value; }
        }

        public double SecondaryTemperature
        {
            get { return this.secondaryTemperature; }
            set { this.secondaryTemperature = value; }
        }


        public double FocusPosition
        {
            get { return this.focusPosition; }
            set { this.focusPosition = value; }
        }

        public int ShutterStatus
        {
            get { return this.shutterStatus; }
            set { this.shutterStatus = value; }
        }

        public double AmbientTemperature
        {
            get { return this.ambientTemperature; }
            set { this.ambientTemperature = value; }
        }

        public double RelativeHumidity
        {
            get { return this.relativeHumidity; }
            set { this.relativeHumidity = value; }
        }

        public double Pressure
        {
            get { return this.pressure; }
            set { this.pressure = value; }
        }

        public double DewPoint
        {
            get { return this.dewPoint; }
            set { this.dewPoint = value; }
        }



    }
}
