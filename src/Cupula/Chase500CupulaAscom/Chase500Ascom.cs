using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using ModbusTCP;
using ASCOM.DeviceInterface;


namespace Chase500CupulaAscom
{
    public class Chase500Ascom : ASCOM.DeviceInterface.IDomeV2
    {
        #region Variables de Instancia
        /// <summary>
        /// Valores de los primeros 16 bits del registro "zregJ1XT1" del PLC.
        /// En este registro se realizan operaciones de lectura y escritura.
        /// </summary>
        private Boolean[] zregJ1XT1;

        /// <summary>
        /// Valores de los primeros 16 bits del registro "zregO1XT1" del PLC.
        /// En este registro solo se realizan operaciones de lectura.
        /// </summary>
        private Boolean[] zregO1XT1;

        /// <summary>
        /// Timer encargado de mantener la cupula abierta.
        /// </summary>
        private Timer deadManTimer;

        /// <summary>
        /// Por conveniencia esta variable se define como byte, sin embargo
        /// esta variable solo alterna entre los valores 0 y 1.
        /// </summary>
        private byte deadManStatus;

        /// <summary>
        /// Posicion definido por el usuario para la apertura del lado Norte.
        /// </summary>
        private ushort northRoof;

        /// <summary>
        /// Posicion definido por el usuario para la apertura del lado Sur.
        /// </summary>
        private ushort southRoof;

        /// <summary>
        /// Objecto encargado de las comunicaciones con el PLC a traves
        /// del protocolo ModBus Tcp/Ip
        /// </summary>
        private Master zelioConn;

        /// <summary>
        /// Host del PLC
        /// </summary>
        private String host;

        /// <summary>
        /// Puerto de comunicaciones ModBus /TCP del PLC.
        /// </summary>
        private ushort port;

        /// <summary>
        /// Timer encargado de refrescar la variable shutterStatus
        /// </summary>
        private Timer domeMovingTimer;

        /// <summary>
        /// Indica el estado del domo. opening, open, closing, closed, error
        /// </summary>
        private ShutterState shutterStatus;
        #endregion

        #region constantes

        public const long USEC_SEC = 1000000;
        public const long SLEEP_BETWEEN_COMMANDS_MILISECS = 500;
        public const long DEADMAN_MILISECS = 5000;
        public const long DOMESTATUS_MILISECS = 1000;
        /*
        public const UInt16 ZC_NORTH_OPEN = 0x0001;
        public const UInt16 ZC_SOUTH_OPEN = 0x0010;

        public const UInt16 ZC_NORTH_OPEN_50 = 0x0002;
        public const UInt16 ZC_NORTH_CLOSE_50 = 0x0004;

        public const UInt16 ZC_SOUTH_OPEN_50 = 0x0020;
        public const UInt16 ZC_SOUTH_CLOSE_50 = 0x0040;

        public const UInt16 ZC_NORTH_CLOSE = 0x0000;
        public const UInt16 ZC_SOUTH_CLOSE = 0x0000;
        */
        public const ushort ZREG_J1XT1 = 16;
        public const ushort ZREG_J2XT1 = 17;

        public const ushort ZREG_O1XT1 = 20;

        public const UInt16 ZS_SOUTH_OPEN = 4;
        public const UInt16 ZS_SOUTH_50 = 5;
        public const UInt16 ZS_SOUTH_CLOSE = 6;
        public const UInt16 ZS_NORTH_OPEN = 7;
        public const UInt16 ZS_NORTH_50 = 8;
        public const UInt16 ZS_NORTH_CLOSE = 9;

        /// <summary>
        /// Para definir status 100% Open en northRoof o SouthRoof.
        /// </summary>
        public const ushort OPEN = 0;

        /// <summary>
        /// para definir status 50% Open en northRoof o SouthRoof.
        /// </summary>
        public const ushort HALF = 1;

        /// <summary>
        /// Para definir status 100% Closed en northRoof o SouthRoof.
        /// </summary>
        public const ushort CLOSE = 2;
        #endregion

        public Chase500Ascom()
        {
            zelioConn = new Master();
            this.host = null;
            this.port = 502;
            zregJ1XT1 = new Boolean[16];
            zregO1XT1 = new Boolean[16];

            northRoof = Chase500Ascom.OPEN;
            southRoof = Chase500Ascom.OPEN;
            shutterStatus = ShutterState.shutterClosed;

            Console.WriteLine("zelioConn.connected=" + zelioConn.connected);
            deadManTimer = new Timer(DEADMAN_MILISECS);
            deadManStatus = 0;
            deadManTimer.Elapsed += new ElapsedEventHandler(deadMan_Elapsed);

            domeMovingTimer = new Timer(DOMESTATUS_MILISECS);
            domeMovingTimer.Elapsed += new ElapsedEventHandler(domeMovingTimer_Elapsed);
        }


        #region IDomeV2 Members

        public void AbortSlew()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string Action(string ActionName, string ActionParameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public double Altitude
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool AtHome
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool AtPark
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public double Azimuth
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool CanFindHome
        {
            get { return false; }
        }

        public bool CanPark
        {
            get { return false; }
        }

        public bool CanSetAltitude
        {
            get { return false; }
        }

        public bool CanSetAzimuth
        {
            get { return false; }
        }

        public bool CanSetPark
        {
            get { return false; }
        }

        public bool CanSetShutter
        {
            get { return true; }
        }

        public bool CanSlave
        {
            get { return false; }
        }

        public bool CanSyncAzimuth
        {
            get { return false; }
        }

        public void CloseShutter()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CommandBlind(string Command, bool Raw)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CommandBool(string Command, bool Raw)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Recibe parametros para la cupula en el formato "KEY VALUE".
        /// Ejemplo:
        /// HOST 10.0.65.10
        /// PORT 502
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Raw"></param>
        /// <returns></returns>
        public string CommandString(string Command, bool Raw)
        {

            String[] part;
            part = Command.Split((" ").ToCharArray());
            if (part.Length < 2) { return "Error, se esperan mas parametros."; }
            switch (part[0])
            {
                case ("HOST"):

                    this.host = part[1];

                    break;
                case ("PORT"):
                    this.port = ushort.Parse(part[1]);
                    break;
            }
            return "ok";
        }

        public bool Connected
        {
            get
            {
                return this.zelioConn.connected;
            }
            set
            {
                if (value)
                {
                    this.Connect();
                }
                else
                {
                    this.Disconnect();
                }
            }
        }

        public string Description
        {
            get { return "Chase500 Dome Driver."; }
        }

        public void Dispose()
        {
            this.Disconnect();
        }

        public string DriverInfo
        {
            get
            {
                StringBuilder respuesta;
                respuesta = new StringBuilder();
                respuesta.Append("Chase 500, control básico del domo.\n\r");
                respuesta.Append("Domo basado en sistema hidráulico gobernado por un PLC\n\r");
                respuesta.Append("Desarrollado por Eduardo Maureira. emaureir@das.uchile.cl\n\r");

                return respuesta.ToString();
            }
        }

        public string DriverVersion
        {
            get { return "Version 0.1, 2010_01_30"; }
        }

        public void FindHome()
        {

        }

        public short InterfaceVersion
        {
            get { return 1; }
        }

        public string Name
        {
            get { return "Chase 500"; }
        }

        public void OpenShutter()
        {
            this.northRoof = Chase500Ascom.OPEN;
            this.southRoof = Chase500Ascom.OPEN;
            this.OpenDome();
        }

        public void Park()
        {

        }

        public void SetPark()
        {

        }

        public void SetupDialog()
        {
            DialogBox db;
            db = new DialogBox(this);
            db.ShowDialog();
        }

        public ASCOM.DeviceInterface.ShutterState ShutterStatus
        {
            get { return this.shutterStatus; }
        }

        public bool Slaved
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SlewToAzimuth(double Azimuth)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Slewing
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public System.Collections.ArrayList SupportedActions
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void SyncToAzimuth(double Azimuth)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Timer para reflejar en la interfaz gráfica el estado del Domo.
        /// </summary>
        public Timer DomeMovingTimer
        {
            get { return this.domeMovingTimer; }
            set { this.domeMovingTimer = value; }
        }

        /// <summary>
        /// Dirección IPv4 del PLC
        /// </summary>
        public String Host
        {
            get { return this.host; }
            set { this.host = value; }
        }

        /// <summary>
        /// Puerto para las comunicaciones ModBus del PLC.
        /// </summary>
        public ushort Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        /// <summary>
        /// Posicion definida por el usuario para la apertura del lado Norte.
        /// </summary>
        public ushort NorthRoof
        {
            get { return this.northRoof; }
            set { this.northRoof = value; }
        }

        /// <summary>
        /// Posicion definida por el usuario para la apertura del lado Sur.
        /// </summary>
        public ushort SouthRoof
        {
            get { return this.southRoof; }
            set { this.southRoof = value; }
        }


        /// <summary>
        /// Componente encargado de las comunicaciones TCP/IP
        /// bajo el protocolo ModBus.
        /// </summary>
        public Master TcpSession
        {
            get { return this.zelioConn; }
            set { this.zelioConn = value; }
        }

        /// <summary>
        /// Timer encargado de mantener la cupula abierta.
        /// </summary>
        public Timer DeadManTimer
        {
            get { return this.deadManTimer; }
            set { this.deadManTimer = value; }
        }

        /// <summary>
        /// Valores de los primeros 16 bits del registro "zregJ1XT1" del PLC.
        /// En este registro se realizan operaciones de lectura y escritura.
        /// </summary>
        public Boolean[] Zreg_J1XT1
        {
            get { return this.zregJ1XT1; }
            set { this.zregJ1XT1 = value; }
        }

        /// <summary>
        /// Valores de los primeros 16 bits del registro "zregO1XT1" del PLC.
        /// En este registro solo se realizan operaciones de lectura.
        /// </summary>
        public Boolean[] Zreg_O1XT1
        {
            get { return this.zregO1XT1; }
            set { this.zregO1XT1 = value; }
        }

        #endregion


        #region Funciones
        /// <summary>
        /// Inicia una sesión ModBus con el PLC.
        /// </summary>
        public void Connect()
        {
            if (!zelioConn.connected)
            {
                zelioConn.connect(this.host, this.port);
            }
        }

        /// <summary>
        /// Termina la sesion Modbus TcpIP con el PLC.
        /// </summary>
        public void Disconnect()
        {
            if (zelioConn.connected)
            {
                zelioConn.disconnect();
            }
        }


        /// <summary>
        /// Método que se ejecuta cada vez que el timer se gatilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deadMan_Elapsed(object sender, ElapsedEventArgs e)
        {
            byte[] deadMan;
            deadMan = new byte[2];
            deadMan[0] = 0;
            deadMan[1] = deadManStatus;
            zelioConn.WriteSingleRegister(0, ZREG_J2XT1, deadMan);
            deadManStatus++;
            deadManStatus = (byte)(deadManStatus % 2);
            Console.WriteLine("deadMan_Elapsed");
        }

        void domeMovingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Read_ZREG_O1XT1();
            if (this.IsClosed() == 3)
            {
                this.shutterStatus = ShutterState.shutterClosed;
                if (this.domeMovingTimer.Enabled)
                {
                    this.domeMovingTimer.Stop();
                }
            }
            if (this.IsOpened() == 3)
            {
                this.shutterStatus = ShutterState.shutterOpen;
                if (this.domeMovingTimer.Enabled)
                {
                    this.domeMovingTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Abre la cupula en las posiciones definidas para northRoof y southRoof.
        /// </summary>
        public void OpenDome()
        {
            this.shutterStatus = ShutterState.shutterOpening;
            this.domeMovingTimer.Start();
            Console.WriteLine("Abrir");

            //Desbloquea el PLC para que acepte la instruccion de apertura en la configuracion requerida
            #region Unlock
            byte[] unlock;
            unlock = new byte[2];
            unlock[0] = 0;
            unlock[1] = 0;

            zelioConn.WriteSingleRegister(0, ZREG_J2XT1, unlock);
            System.Threading.Thread.Sleep(500);
            unlock[1] = 1;
            zelioConn.WriteSingleRegister(0, ZREG_J2XT1, unlock);
            #endregion

            // ZC_NORTH_STOP        0x0008,ZC_SOUTH_STOP        0x0080
            // Envia un 0x0088->Equivalente a: NORTH_STOP y un SOUTH_STOP 
            #region
            for (int i = 0; i < 16; i++)
            {
                if ((i == 3) || (i == 7))
                {
                    Zreg_J1XT1[i] = true;
                }
                else
                {
                    Zreg_J1XT1[i] = false;
                }
            }
            Write_ZREG_J1XT1();
            #endregion
            System.Threading.Thread.Sleep(500);
            Read_ZREG_O1XT1();
            /// En base a los valores de northRoof y southRoof
            /// Genera un valor para escribir en el PLC a la hora de
            /// abrir la cupula.
            #region Genera Comando Open
            int valor;
            valor = 0;
            // Lado Norte
            switch (northRoof)
            {
                case (Chase500Ascom.OPEN):
                    valor += 1;
                    break;
                case (Chase500Ascom.HALF):
                    if (Zreg_O1XT1[Chase500Ascom.ZS_NORTH_CLOSE])
                    {
                        valor += (1 << 1);
                    }
                    if (Zreg_O1XT1[Chase500Ascom.ZS_NORTH_OPEN])
                    {
                        valor += (1 << 2);
                    }
                    break;
                case (Chase500Ascom.CLOSE):
                    //valor += (1 << 3);
                    break;
            }
            switch (southRoof)
            {
                case (Chase500Ascom.OPEN):
                    valor += (1 << 4);
                    break;
                case (Chase500Ascom.HALF):
                    if (Zreg_O1XT1[Chase500Ascom.ZS_SOUTH_CLOSE])
                    {
                        valor += (1 << 5);
                    }
                    if (Zreg_O1XT1[Chase500Ascom.ZS_SOUTH_OPEN])
                    {
                        valor += (1 << 6);
                    }
                    break;
                case (Chase500Ascom.CLOSE):
                    //valor += (1 << 7);
                    break;
            }
            for (int i = 0; i < 16; i++)
            {
                Zreg_J1XT1[i] = ((valor % 2) == 1);
                valor /= 2;
            }
            Write_ZREG_J1XT1();
            #endregion
            // Finalmente arrancamos el timer que permite mantener la cupula abierta.
            if (!(this.deadManTimer.Enabled))
            {
                this.deadManTimer.Start();
            }
        }

        /// <summary>
        /// Invocar cuando en la interfaz se escoja la opcion "Set Close".
        /// </summary>
        public void CloseDome()
        {
            this.shutterStatus = ShutterState.shutterClosing;
            this.domeMovingTimer.Start();
            this.deadManTimer.Stop();
            //for (int i = 0; i < 8; i++)
            //{
            //    if ((i == 3) || (i == 7))
            //    {
            //        Zreg_J1XT1[i] = true;
            //    }
            //    else
            //    {
            //        Zreg_J1XT1[i] = false;
            //    }
            //}
            byte[] cerrar;
            cerrar = new byte[2];
            cerrar[0] = 0;
            cerrar[1] = 0;

            zelioConn.WriteSingleRegister(0, ZREG_J1XT1, cerrar);
        }


        /// <summary>
        /// Metodo generico para leer registros del PLC
        /// </summary>
        /// <param name="startRegister">registro donde comienza la lectura</param>
        /// <param name="cantBytes">cantidad de bytes a leer.</param>
        /// <returns></returns>
        private Boolean[] Read_PLC(ushort startRegister, ushort cantBytes)
        {
            byte[] regs;
            regs = new byte[cantBytes];
            try
            {
                if (zelioConn.connected)
                {
                    zelioConn.ReadHoldingRegister(0, startRegister, cantBytes, ref regs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            ulong total;
            total = 0;
            for (int i = 0; i < cantBytes; i++)
            {
                total = (total << (8 * i));
                total += regs[i];
            }
            Console.WriteLine("Read_PLC: total=" + total);
            Boolean[] respuesta;
            respuesta = new Boolean[8 * cantBytes];
            for (int i = 0; i < respuesta.Length; i++)
            {
                respuesta[i] = ((total % 2) == 1);
                total /= 2;
            }
            return respuesta;
        }

        /// <summary>
        /// Lee los 16 bits del primer "Control register".
        /// Ojo, solo son relevantes los primeros 8 bits
        /// </summary>
        public void Read_ZREG_J1XT1()
        {
            this.zregJ1XT1 = Read_PLC(ZREG_J1XT1, 2);
        }

        /// <summary>
        /// Envia al PLC los 16 bits del primer "Control Register".
        /// </summary>
        public void Write_ZREG_J1XT1()
        {
            byte[] valor;
            valor = new byte[2];
            int indice;
            for (int i = 0; i < 16; i++)
            {
                indice = (i / 8);
                if (zregJ1XT1[i])
                {
                    valor[(indice + 1) % 2] += (byte)(1 << (i - (indice * 8)));
                }
            }
            Console.WriteLine("valor[0]=" + valor[0] + "    valor[1]=" + valor[1]);
            if (zelioConn.connected)
            {
                zelioConn.WriteSingleRegister(0, ZREG_J1XT1, valor);
            }
        }

        /// <summary>
        /// Lee los 16 bits del primer "Ouput Register"
        /// </summary>
        public void Read_ZREG_O1XT1()
        {
            this.zregO1XT1 = Read_PLC(ZREG_O1XT1, 2);
        }

        /// <summary>
        /// Al invocar a esta funcion, los valores de zregO1XT1 deben estar frescos.
        /// </summary>
        /// <returns> 0 si el estado de apertura difiere del estado deseado para los techos norte y sur.
        /// 1 si el estado de apertura coincide solo para el techo norte.
        /// 2 si el estado de apertura coincide solo para el techo sur.
        /// 3 si el estado de apertura coincide para ambos techos.
        /// </returns>
        public int IsOpened()
        {
            int hits = 0;
            switch (this.northRoof)
            {
                case OPEN:
                    if (zregO1XT1[ZS_NORTH_OPEN])
                        hits |= 0x01;
                    break;
                case HALF:
                    if (zregO1XT1[ZS_NORTH_50])
                        hits |= 0x01;
                    break;
                case CLOSE:
                    if (zregO1XT1[ZS_NORTH_CLOSE])
                        hits |= 0x01;
                    break;
            }
            switch (this.southRoof)
            {
                case 0:
                    if (zregO1XT1[ZS_SOUTH_OPEN])
                        hits |= 0x02;
                    break;
                case 1:
                    if (zregO1XT1[ZS_SOUTH_50])
                        hits |= 0x02;
                    break;
                case 2:
                    if (zregO1XT1[ZS_SOUTH_CLOSE])
                        hits |= 0x02;
                    break;
            }

            //if (hits == 0x03)
            //    return -2;
            //return 0;
            return hits;
        }

        /// <summary>
        /// Al invocar a esta funcion, los valores de zregO1XT1 deben estar frescos.
        /// </summary>
        /// <returns></returns>
        public int IsClosed()
        {
            // check states of end switches..
            int hits = 0;
            if (zregO1XT1[ZS_SOUTH_CLOSE])
            {
                hits |= 0x01;
            }
            if (zregO1XT1[ZS_NORTH_CLOSE])
            {
                hits |= 0x02;
            }
            return hits;
        }

        #endregion

    }
}
