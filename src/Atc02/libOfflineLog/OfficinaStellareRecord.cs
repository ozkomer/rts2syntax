using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libOfflineLog
{
    public class OfficinaStellareRecord :IComparable<OfficinaStellareRecord>

    {
        private DateTime fecha;
        private double primaryTemperature;
        private double secondaryTemperature;
        private double ambientTemperature;
        private double relativehumidity;
        private double pressure;
        private double bfl;

        public OfficinaStellareRecord()
        {
            this.primaryTemperature = -1;
            this.secondaryTemperature = -1;
            this.ambientTemperature = -1;
            this.relativehumidity = -1;
            this.pressure = -1;
            this.bfl = -1;
        }

        public DateTime Fecha
        {
            get { return this.fecha; }
            set { this.fecha = value; }
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

        public double AmbientTemperature
        {
            get { return this.ambientTemperature; }
            set { this.ambientTemperature = value; }
        }

        public double Relativehumidity
        {
            get { return this.relativehumidity; }
            set { this.relativehumidity = value; }
        }

        public double Pressure
        {
            get { return this.pressure; }
            set { this.pressure = value; }
        }

        public double Bfl
        {
            get { return this.bfl; }
            set { this.bfl = value; }
        }

        public override string ToString()
        {
            StringBuilder linea;
            linea = new StringBuilder();
            linea.Append(this.fecha.ToString());
            linea.Append('\t');
            linea.Append(this.bfl);
            linea.Append('\t');
            linea.Append(this.ambientTemperature);
            linea.Append('\t');
            linea.Append(this.primaryTemperature);
            linea.Append('\t');
            linea.Append(this.secondaryTemperature);
            linea.Append('\t');
            linea.Append(this.pressure);
            linea.Append('\t');
            linea.Append(this.relativehumidity);
            return linea.ToString();
        }

        #region IComparable<OfficinaStellareRecord> Members
        public int CompareTo(OfficinaStellareRecord other)
        {
            return this.fecha.CompareTo(other.fecha);
        }
        #endregion
    }
}
