using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using log4net;

namespace PowerFocus
{
    public class Atc02Comm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PowerFocus.Atc02Comm));
        private static Atc02Comm _instance = new Atc02Comm();

        /// <summary>
        /// Datos invariantes en el firmware del ATC02
        /// </summary>
        private String firmwareSettings;

        /// <summary>
        /// Si está en true, permite que los mensajes UPDATEPC sean enviados al ATC02.
        /// Cuando el ATC02 esta moviendo el secundario, es preferible evitar este tipo de 
        /// mensajes, eso explica la existencia de esta variable
        /// </summary>
        private Boolean enableTtyWrite;

        public static Atc02Comm Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Puerto serial del ATC-02
        /// </summary>
        public SerialPort ttyATC;

        /// <summary>
        /// Status obtenido a traves del metodo UPDATEPC del firmware del ATC-02
        /// </summary>
        private AtcStatus atcStat;

        /// <summary>
        /// Se esta conectado cuando al enviar el mensaje OPENREM se recibe el mismo texto como respuesta.
        /// Se esta DESconectado cuando al enviar el mensaje CLOSEREM se recibe el mismo texto como respuesta.
        /// </summary>
        private bool conectado;

        /// <summary>
        /// true si el motor del secundario esta en movimiento. O más especifico,
        /// si se está ejecutando el método: Move().
        /// </summary>
        private bool enMovimiento;

        /// <summary>
        /// Step en que está actualmente el secundario. Se semisetea con el parámetro del
        /// metodo Move(), pero se actualiza de manera definitiva con la respuesta del ATC al comando Move.
        /// </summary>
        private int posicion;

        private System.Timers.Timer statusTimer;

        //                                0123456789
        public static readonly String OPENREM = "OPENREM   ";
        public static readonly String CLOSEREM = "CLOSEREM  ";
        public static readonly String READSETT = "READSETT  ";
        public static readonly String UPDATEPC = "UPDATEPC  ";
        public static readonly String SETFAN = "SETFAN ";
        public static readonly String SETBFL = "BFL ";
        public static readonly String FINDOPTIMA = "FINDOPTIMA";

        /// <summary>
        /// Arranca la puerta serial.
        /// </summary>
        private Atc02Comm()
        {
            this.conectado = false;
            this.enableTtyWrite = true;
            this.ttyATC = new SerialPort(
                Properties.Settings.Default.CommPort,
                (int)Properties.Settings.Default.BaudRate
                );
            this.firmwareSettings = null;
            this.atcStat = new AtcStatus();
            this.statusTimer = new System.Timers.Timer();
            this.statusTimer.Interval = (double)(1000 * ( Properties.Settings.Default.refreshStatusTimer));
            this.statusTimer.Elapsed += new System.Timers.ElapsedEventHandler(statusTimer_Elapsed);
            this.enMovimiento = false;
        }

        public String FirmwareSettings
        {
            get { return this.firmwareSettings; }
        }

        public AtcStatus AtcStat
        {
            get { return this.atcStat; }
            set { this.atcStat = value; }
        }

        /// <summary>
        /// Se esta conectado cuando al enviar el mensaje OPENREM se recibe el mismo texto como respuesta.
        /// Se esta DESconectado cuando al enviar el mensaje CLOSEREM se recibe el mismo texto como respuesta.
        /// </summary>
        public Boolean Conectado
        {
            get { return this.conectado; }
        }

        /// <summary>
        /// true si el motor del secundario esta en movimiento. O más especifico,
        /// si se está ejecutando el método: Move().
        /// </summary>
        public Boolean EnMovimiento
        {
            get { return this.enMovimiento; }
        }

        /// <summary>
        /// Step en que está actualmente el secundario. Se semisetea con el parámetro del
        /// metodo Move(), pero se actualiza de manera definitiva con la respuesta del ATC al comando Move.
        /// </summary>
        public int Posicion
        {
            get { return this.posicion; }
        }

        void statusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger.Info( "statusTimer_Elapsed");
            if (enableTtyWrite) 
            {
                this.RefreshAtcStatus();
            }
        }

        /// <summary>
        /// Si la puerta serial esta cerrada, la abre.
        /// Una vez abierta la puerta serial envia:
        /// - un comando OPENREM.
        /// - un comando FINDOPTIMAL
        /// 
        /// </summary>
        /// <returns>true si el comando OPENREM es exitoso.</returns>
        public Boolean conectar()
        {
            if (!this.ttyATC.IsOpen)
            {
                logger.Info("Abriendo Conexion.");
                this.ttyATC = new SerialPort(
                    Properties.Settings.Default.CommPort,
                    (int)Properties.Settings.Default.BaudRate
                    );
                this.ttyATC.Open();
            }
            else
            {
                logger.Info( "Conexion ya estaba abierta.");
            }
            logger.Info( "Esperando Conexion.");
            
            while (this.ttyATC.IsOpen != true)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(300);
            }
            //Console.WriteLine("conectado!!");
            logger.Info( "conectado!");
            this.ttyWrite(OPENREM);//            ttyATC.Write(OPENREM);
            String respuesta;
            respuesta = LeerSerial(10).Trim();
            this.conectado = OPENREM.Contains(respuesta);
            logger.Info( "respuesta=" + respuesta);
            logger.Info( "this.conectado=" + this.conectado);
            if (this.conectado)
            {
                String findOptimal;
                findOptimal = this.FindOptimal();
                logger.Info( "findOptimal=" + findOptimal);
                if (Properties.Settings.Default.lastSecondaryStartUp)
                {
                    this.Move((int) Properties.Settings.Default.lastSecondaryPosition);
                }
                this.ReadSettings();
                this.RefreshAtcStatus();

            }
            //if (Properties.Settings.Default.refreshStatus)
            //{
            this.statusTimer.Enabled = true;
            this.statusTimer.Start();
            //}
            
            return this.conectado;
        }

        /// <summary>
        /// Detiene el status Timer.
        /// envia un COMANDO CLOSEREM.
        /// si el comando es exitoso, ademas cierra la puerta serial.
        /// </summary>
        /// <returns>true si el comando CLOSEREM es exitoso</returns>
        public Boolean desConectar()
        {
            this.statusTimer.Stop();
            this.ttyWrite(CLOSEREM);// ttyATC.Write(CLOSEREM);
            String respuesta;
            respuesta = LeerSerial(10).Trim();
            this.conectado = (!(CLOSEREM.Contains(respuesta)));
            if (!conectado)
            {
                try
                {
                    logger.Info( "cerrando tty.");
                    this.ttyATC.Close();
                }
                catch (InvalidOperationException)
                {
                    conectado = true;
                }
            }
            return !this.conectado;
        }

        /// <summary>
        /// Desplaza el espejo secundario al step indicado.
        /// </summary>
        /// <param name="value"></param>
        public void Move(int value)
        {
            this.enMovimiento = true;
            this.posicion = value;
            Double bfl;
            //int newPosition;
            StringBuilder comandoBFL;
            String respuesta;
            bfl = 130 + (((double)posicion) / 100.0);
            comandoBFL = new StringBuilder();
            comandoBFL.Append(SETBFL);

            comandoBFL.Append(bfl.ToString("000.00"));
            //Console.WriteLine("->" + comandoBFL.ToString());


            logger.Info( "enviando comando serial:" + comandoBFL.ToString());
            this.ttyWrite(comandoBFL.ToString());// ttyATC.Write(comandoBFL.ToString());


            respuesta = LeerSerial(10).Trim();
            logger.Info( "respuesta=" + respuesta);

            this.atcStat.refresh(respuesta, DateTime.Now);
            //String[] parte;
            //int largoParte;

            //parte = respuesta.Split((" ").ToCharArray());
            //largoParte = parte.Length;
            //try
            //{
            //    newPosition = (int)(100.0 * (Double.Parse(parte[largoParte - 1]) - 130));
            //}
            //catch (Exception)
            //{
            //    newPosition = -1;
            //}

            //this.posicion = newPosition;
            this.enMovimiento = false;
            //bfl = 130 + (((double)posicion) / 100.0);
            //this.atcStat.FocusPosition = bfl;
            this.posicion = (int) ((this.atcStat.FocusPosition - 130) * 100.0);
            Properties.Settings.Default.lastSecondaryPosition = this.posicion;
            Properties.Settings.Default.Save();
        }

        private String ReadSettings()
        {
            logger.Info("Enviando:" + READSETT);
            //Console.WriteLine("->" + READSETT);
            this.ttyWrite(READSETT);// this.ttyATC.Write(READSETT);
            this.firmwareSettings = LeerSerial(130).Trim();
            logger.Info("firmwareSettings=" + this.firmwareSettings);
            return this.firmwareSettings;
        }

        /// <summary>
        /// Envia un comando "UPDATEPC"
        /// </summary>
        public void RefreshAtcStatus()
        {
            logger.Info("Enviando:" + UPDATEPC);
            //Console.WriteLine("->" + UPDATEPC);
            this.ttyWrite(UPDATEPC);// this.ttyATC.Write(UPDATEPC);
            String rawStatus;
            rawStatus = LeerSerial(100).Trim();
            logger.Info( "rawStatus=" + rawStatus);
            this.atcStat.refresh(rawStatus, DateTime.Now);
            this.posicion = (int)(100.0 * (this.atcStat.FocusPosition - 130));
        }

        /// <summary>
        /// Envia un comando "FINDOPTIMA" al ATC02
        /// </summary>
        /// <returns>Primeros 10 caracteres enviados por ATC02 luego de enviar el comando.</returns>
        public String FindOptimal()
        {
            logger.Info( "Enviando:" + FINDOPTIMA);
            this.ttyWrite(FINDOPTIMA);// this.ttyATC.Write(FINDOPTIMA);
            String respuesta;
            respuesta = LeerSerial(10).Trim();
            return respuesta;
        }

        /// <summary>
        /// Envia un comando "SETFAN fanSpeed" al ATC02
        /// </summary>
        /// <param name="fanSpeed">de 0 a 100, capacidad de giro de los ventiladores </param>
        /// <returns>Primeros 10 caracteres enviados por ATC02 luego de enviar el comando.</returns>
        public String SetFan(Int32 fanSpeed)
        {
            StringBuilder comandoFan;
            comandoFan = new StringBuilder();
            comandoFan.Append(SETFAN);
            comandoFan.Append(fanSpeed.ToString("000"));
            //Console.WriteLine("->" + comandoFan);
            this.ttyWrite(comandoFan.ToString());// ttyATC.Write(comandoFan.ToString());
            String respuesta;
            respuesta = LeerSerial(10).Trim();
            return respuesta;
        }

        #region Metodos privados

        private void ttyWrite (String comando)
        {
            while (!enableTtyWrite)
            {
                Console.Write("#");
                System.Threading.Thread.Sleep(200);
            }
            this.ttyATC.Write(comando);
        }

        private String LeerSerial(int cantCaracteres)
        {
            this.enableTtyWrite = false;

            String respuesta;
            Console.WriteLine("<---LeerSerial--->");
            Console.Write("Esperando respuesta ");

            while (ttyATC.BytesToRead < cantCaracteres)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(200);
            }
            Console.WriteLine(".");
            System.Threading.Thread.Sleep(200);
            respuesta = this.ttyATC.ReadExisting();
            Console.WriteLine(respuesta);
            Console.WriteLine("<---LeerSerial--->");
            this.enableTtyWrite = true;
            return respuesta;
        }
        #endregion


    }
}
