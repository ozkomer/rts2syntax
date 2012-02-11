using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serduino
{
    public class Status
    {
        private String linea;
        private int interruptores;
        private static Acelerometro aRA=  new Acelerometro ();
        private static Acelerometro aDEC = new Acelerometro();
        private bool[] interruptor;
        private bool raLimit1;
        private bool raLimit2;
        private bool raHome;
        private bool decHome;


        public Status(String linea)
        {
            this.linea = linea;
            interruptor = new bool[3];
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

        public void Analiza()
        {
            String[] part;
            part = this.linea.Split((" ").ToCharArray());
            this.interruptores = Int16.Parse(part[0]);
            if (part.Length >= 6)
            {
                aRA.setValues(Double.Parse(part[1]), Double.Parse(part[2]), Double.Parse(part[3]));
                aDEC.setValues(Double.Parse(part[4]), Double.Parse(part[5]), Double.Parse(part[6]));
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
            this.raHome = (interruptor[1] & interruptor[2]);
            if (!raHome)
            {
                this.raLimit1 = interruptor[1];
                this.raLimit2 = interruptor[2];
            }
            #endregion

            StringBuilder mensaje;
            mensaje = new StringBuilder ();
            mensaje.Append("interruptores="); mensaje.Append(this.interruptores);
            mensaje.Append("\t decHome="); mensaje.Append(this.decHome);
            mensaje.Append("\t raLimit1="); mensaje.Append(this.raLimit1);
            mensaje.Append("\t raLimit2="); mensaje.Append(this.raLimit2);
            mensaje.Append("\t raHome="); mensaje.Append(this.raHome);
            Console.WriteLine(mensaje.ToString());

        }
    }
}
