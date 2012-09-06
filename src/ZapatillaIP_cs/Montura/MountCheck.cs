using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montura
{
    public class MountCheck
    {
        private int id;
        private int output;

        private DateTime comienzoTest;
        private TimeSpan elapsed;
        private int counter;
        private String name;
        private int timeOutSeconds;
        private Boolean isRunning;

        /// <summary>
        /// Genera un string con el estatus de este checkeo.
        /// </summary>
        /// <returns></returns>
        public String status()
        {
            StringBuilder mensaje;
            mensaje = new StringBuilder();
            mensaje.Append(this.name);
            mensaje.Append(";");
            this.refreshElapsed();
            mensaje.Append(elapsed.TotalSeconds);
            mensaje.Append("/");
            mensaje.Append(this.timeOutSeconds);
            mensaje.Append(";");
            mensaje.Append("running=" + this.isRunning);
            mensaje.Append(";");
            mensaje.Append("counter=" + this.counter);
            mensaje.Append(";");
            mensaje.Append("output=" + this.output);
            return mensaje.ToString();
        }

        public MountCheck(int id, String name)
        {
            this.id = id;
            this.name = name;
            this.counter = 0;
            this.isRunning = false;
            this.output = -1;
        }

        /// <summary>
        /// Marca el test como en ejecución y marca la hora de inicio.
        /// </summary>
        public void start()
        {
            this.comienzoTest = DateTime.Now;
            this.isRunning = true;
        }

        /// <summary>
        /// Marca el test como finalizado.
        /// </summary>
        /// <param name="output">valor resultante del test.</param>
        public void finish(int output)
        {
            this.refreshElapsed();
            this.isRunning = false;
            this.output = output;
        }


        public Boolean IsRunning
        {
            get { return this.isRunning; }
            //set { this.isRunning = value; }
        }

        public Boolean isTimeOut()
        {
            this.refreshElapsed();
            return (this.elapsed.TotalSeconds > timeOutSeconds);
        }

        public void refreshElapsed()
        {
            this.elapsed = (DateTime.Now - this.comienzoTest);
        }

        public TimeSpan Elapsed
        {
            get { return this.elapsed; }
            set { this.elapsed = value; }
        }

        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Id
        {
            get { return this.id; }
            //set { this.id = value; }
        }

        public int Output
        {
            get { return this.output; }
            //set { this.output = value; }
        }

        public int TimeOutSeconds
        {
            get { return this.timeOutSeconds; }
            set { this.timeOutSeconds = value; }
        }

        public int Counter
        {
            get { return this.counter; }
            set { this.counter = value; }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
