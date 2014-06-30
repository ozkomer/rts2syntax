//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Dome driver for Cata500
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Dome interface version: 1.0
// Author:		Eduardo Maureira <emaureir@gmail.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Dome Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;
using ModbusTCP;
using System.Timers;
using System.Text;

namespace ASCOM.Chase500
{
    //
    // Your driver's ID is ASCOM.Chase500.Dome
    //
    // The Guid attribute sets the CLSID for ASCOM.Chase500.Dome
    // The ClassInterface/None addribute prevents an empty interface called
    // _Dome from being created and used as the [default] interface
    //
    [Guid("d3d41484-e9d1-4de8-8557-ca24dc6e8c39")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Dome : IDomeV2
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
        private System.Timers.Timer deadManTimer;

        /// <summary>
        /// Por conveniencia esta variable se define como byte, sin embargo
        /// esta variable solo alterna entre los valores 0 y 1.
        /// </summary>
        private byte deadManStatus;

        /// <summary>
        /// Posicion definida por el usuario para la apertura del lado Norte.
        /// </summary>
        private ushort northRoof;

        /// <summary>
        /// Posicion definida por el usuario para la apertura del lado Sur.
        /// </summary>
        private ushort southRoof;

        /// <summary>
        /// Objecto encargado de las comunicaciones con el PLC a traves
        /// del protocolo ModBus Tcp/Ip
        /// </summary>
        private Master zelioConn;

        /// <summary>
        /// Timer encargado de refrescar la variable shutterStatus
        /// </summary>
        private System.Timers.Timer domeMovingTimer;

        /// <summary>
        /// Indica el estado del domo. opening, open, closing, closed, error
        /// </summary>
        private ShutterState shutterStatus;
        #endregion

        #region constantes

        //public const long USEC_SEC = 1000000;
        public const int SLEEP_BETWEEN_COMMANDS_MILISECS = 500;
        public const long DEADMAN_MILISECS    = 20000;
        public const long DOMESTATUS_MILISECS =  1000;
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

        public const UInt16 ZS_MAGNETIC_LOCK1 = 0;
        public const UInt16 ZS_MAGNETIC_LOCK2 = 3;
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

        private static TraceLogger sysLog = new TraceLogger(null,"chase500Dome");


        #region Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private const string driverId = "ASCOM.Chase500.Dome";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "Chase500 Dome";
        #endregion

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var p = new Profile())
            {
                p.DeviceType = "Dome";
                if (bRegister)
                    p.Register(driverId, driverDescription);
                else
                    p.Unregister(driverId);
            }
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        public Dome()
        {
            sysLog.Enabled = true;

            sysLog.LogMessageCrLf("Dome()", "Instanciando Dome().");
            zelioConn = new Master();            
            zregJ1XT1 = new Boolean[16];
            zregO1XT1 = new Boolean[16];

            northRoof = Properties.Settings.Default.NorthOpen;
            southRoof = Properties.Settings.Default.SouthOpen;
            shutterStatus = ShutterState.shutterClosed;
            //sysLog.LogMessageCrLf(driverDescription, "zelioConn.connected=" + zelioConn.connected);

            //Console.WriteLine("zelioConn.connected=" + zelioConn.connected);
            deadManTimer = new System.Timers.Timer(DEADMAN_MILISECS);
            deadManStatus = 0;
            deadManTimer.Elapsed += new ElapsedEventHandler(deadMan_Elapsed);

            domeMovingTimer = new System.Timers.Timer(DOMESTATUS_MILISECS);
            domeMovingTimer.Elapsed += new ElapsedEventHandler(domeMovingTimer_Elapsed);
        }

        #region Implementation of IDomeV2

        public void SetupDialog()
        {
            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.MethodNotImplementedException("Action");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void AbortSlew()
        {
            throw new System.NotImplementedException();
        }

        public void CloseShutter()
        {
            this.CloseDome();
        }

        public void FindHome()
        {
            throw new System.NotImplementedException();
        }

        public void OpenShutter()
        {
            //northRoof = Dome.OPEN;
            //southRoof = Dome.OPEN;
            this.OpenDome();
            System.Threading.Thread.Sleep(20000);
            this.OpenDome();
        }

        public void Park()
        {
            throw new System.NotImplementedException();
        }

        public void SetPark()
        {
            throw new System.NotImplementedException();
        }

        public void SlewToAltitude(double altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToAzimuth(double azimuth)
        {
            throw new System.NotImplementedException();
        }

        public void SyncToAzimuth(double azimuth)
        {
            throw new System.NotImplementedException();
        }

        public bool Connected
        {
            get { return this.zelioConn.connected; }
            set
            {
                if (value)
                {
                    if (!this.zelioConn.connected)
                    {
                        this.AseguraPlcConnected();
                    }                     
                }
                else
                {
                    if (this.zelioConn.connected)
                    {
                        this.Disconnect();
                    }
                }
            }
        }

        public string Description
        {
            get { return Properties.Settings.Default.DriverName; }
        }

        public string DriverInfo
        {
            get
            {
                StringBuilder respuesta;
                respuesta = new StringBuilder();
                respuesta.Append("Chase 500, control básico del domo.\n\r");
                respuesta.Append("Domo basado en sistema hidráulico gobernado por un PLC.\n\r");
                respuesta.Append("Desarrollado por Eduardo Maureira. emaureir@das.uchile.cl\n\r");

                return respuesta.ToString();
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            }
        }

        public short InterfaceVersion
        {
            get { return 2; }
        }

        public string Name
        {
            get { return "Chase 500"; }
        }

        public ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public double Altitude
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool AtHome
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool AtPark
        {
            get { throw new System.NotImplementedException(); }
        }

        public double Azimuth
        {
            get { throw new System.NotImplementedException(); }
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

        public ShutterState ShutterStatus
        {
            get { return this.shutterStatus; }
        }

        public bool Slaved
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Slewing
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Dirección IPv4 del PLC
        /// </summary>
        public String Host
        {
            get { return Properties.Settings.Default.Host; }
            set { 
                Properties.Settings.Default.Host = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Puerto para las comunicaciones ModBus del PLC.
        /// </summary>
        public Decimal Port
        {
            get { return Properties.Settings.Default.Port; }
            set {
                Properties.Settings.Default.Port = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Posicion definida por el usuario para la apertura del lado Norte.
        /// </summary>
        public ushort NorthRoof
        {
            get { return Properties.Settings.Default.NorthOpen; }
            set { 
                Properties.Settings.Default.NorthOpen = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Posicion definida por el usuario para la apertura del lado Sur.
        /// </summary>
        public ushort SouthRoof
        {
            get { return Properties.Settings.Default.SouthOpen; }
            set {
                Properties.Settings.Default.SouthOpen = value;
                Properties.Settings.Default.Save();
            }
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
        public System.Timers.Timer DeadManTimer
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
        /// Asegura una sesión TCP/IP ModBus con el PLC. Que permita
        /// comandos de escritura o Lectura.
        /// </summary>
        public void AseguraPlcConnected()
        {
            sysLog.LogMessageCrLf("AseguraPlcConnected()", "AseguraPlcConnected.");
            if (this.zelioConn == null)
            {
                sysLog.LogMessageCrLf("AseguraPlcConnected()", "zelioConn==NULL, reinstanciando");
                this.zelioConn = new Master();
            }
            if (this.zelioConn.connected)
            {
                sysLog.LogMessageCrLf("AseguraPlcConnected()", "Plc already Connected.");            
            }
            else
            {
                sysLog.LogMessageCrLf("AseguraPlcConnected()", "Iniciando sesion ModBus con el PLC.");
                this.zelioConn.connect(Properties.Settings.Default.Host,
                                    (ushort)Properties.Settings.Default.Port);
            }
        }

        /// <summary>
        /// Termina la sesion Modbus TcpIP con el PLC.
        /// </summary>
        public void Disconnect()
        {
            sysLog.LogMessageCrLf("Disconnect()", "Manteniendo sesion ModBus con el PLC.");
            //if (zelioConn.connected)
            //{
            //    sysLog.LogMessageCrLf("Disconnect()", "Finalizando sesion ModBus con el PLC.");
            //    zelioConn.disconnect();
            //}
        }


        /// <summary>
        /// Método que se ejecuta cada vez que el timer se gatilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deadMan_Elapsed(object sender, ElapsedEventArgs e)
        {
            byte[] deadMan;
            this.AseguraPlcConnected();
            deadMan = new byte[2];
            deadMan[0] = 0;
            deadMan[1] = deadManStatus;
            zelioConn.WriteSingleRegister(0, ZREG_J2XT1, deadMan);
            deadManStatus++;
            deadManStatus = (byte)(deadManStatus % 2);
            sysLog.LogMessageCrLf("deadMan_Elapsed()", "deadMan_Elapsed.");
            System.Threading.Thread.Sleep(SLEEP_BETWEEN_COMMANDS_MILISECS);
            this.Read_ZREG_O1XT1();
        }

        void domeMovingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sysLog.LogMessageCrLf("domeMovingTimer_Elapsed()", "domeMovingTimer_Elapsed.");

            this.Read_ZREG_O1XT1();
            sysLog.LogMessageCrLf("domeMovingTimer_Elapsed()", "shutterStatus=" + shutterStatus);

            if ((shutterStatus == ShutterState.shutterClosing) && (this.IsClosed()))
            {
                sysLog.LogMessageCrLf("domeMovingTimer_Elapsed()", "shutterClosing->shutterClosed.");
                this.shutterStatus = ShutterState.shutterClosed;
                //if (this.domeMovingTimer.Enabled)
                {
                    this.domeMovingTimer.Stop();
                }
            }
            if ((shutterStatus == ShutterState.shutterOpening) && (this.IsOpened() >= 2))
            {
                sysLog.LogMessageCrLf("domeMovingTimer_Elapsed()", "shutterOpening->IsOpened.");
                this.shutterStatus = ShutterState.shutterOpen;
                if (this.domeMovingTimer.Enabled)
                {
                    this.domeMovingTimer.Stop();
                }
            }
            //Console.WriteLine("domeMovingTimer_Elapsed");
        }

        /// <summary>
        /// Abre la cupula en las posiciones definidas para northRoof y southRoof.
        /// </summary>
        private void OpenDome()
        {
            this.shutterStatus = ShutterState.shutterOpening;
            this.domeMovingTimer.Start();
            sysLog.LogMessageCrLf("OpenDome()", "Abriendo el domo.");
            this.AseguraPlcConnected();
            //Console.WriteLine("Abrir");

            //Desbloquea el PLC para que acepte la instruccion de apertura en la configuracion requerida
            #region Unlock
            byte[] unlock;
            unlock = new byte[2];
            unlock[0] = 0;
            unlock[1] = 0;

            zelioConn.WriteSingleRegister(0, ZREG_J2XT1, unlock);
            System.Threading.Thread.Sleep(SLEEP_BETWEEN_COMMANDS_MILISECS);
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
            System.Threading.Thread.Sleep(SLEEP_BETWEEN_COMMANDS_MILISECS);
            this.Read_ZREG_O1XT1();
            /// En base a los valores de northRoof y southRoof
            /// Genera un valor para escribir en el PLC a la hora de
            /// abrir la cupula.
            #region Genera Comando Open
            int valor;
            valor = 0;
            this.northRoof = Properties.Settings.Default.NorthOpen;
            this.southRoof = Properties.Settings.Default.SouthOpen;
            // Lado Norte
            switch (this.northRoof)
            {
                case (Dome.OPEN):
                    valor += 1;
                    break;
                case (Dome.HALF):
                    if (Zreg_O1XT1[Dome.ZS_NORTH_CLOSE])
                    {
                        valor += (1 << 1);
                    }
                    if (Zreg_O1XT1[Dome.ZS_NORTH_OPEN])
                    {
                        valor += (1 << 2);
                    }
                    break;
                case (Dome.CLOSE):
                    //valor += (1 << 3);
                    break;
            }
            switch (this.southRoof)
            {
                case (Dome.OPEN):
                    valor += (1 << 4);
                    break;
                case (Dome.HALF):
                    if (Zreg_O1XT1[Dome.ZS_SOUTH_CLOSE])
                    {
                        valor += (1 << 5);
                    }
                    if (Zreg_O1XT1[Dome.ZS_SOUTH_OPEN])
                    {
                        valor += (1 << 6);
                    }
                    break;
                case (Dome.CLOSE):
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
        private void CloseDome()
        {
            sysLog.LogMessageCrLf("CloseDome()", "Cerrando el Domo.");
            this.shutterStatus = ShutterState.shutterClosing;
            this.domeMovingTimer.Start();
            this.deadManTimer.Stop();

            byte[] cerrar;
            cerrar = new byte[2];
            cerrar[0] = 0;
            cerrar[1] = 0;
            this.AseguraPlcConnected();
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
                this.AseguraPlcConnected();
                zelioConn.ReadHoldingRegister(0, startRegister, cantBytes, ref regs);
            }
            catch (Exception e)
            {
                sysLog.LogMessageCrLf("Read_PLC()", "Message=" + e.Message);
                //sysLog.LogMessageCrLf(driverDescription, "Read_PLC:Source=" + e.Source);
                sysLog.LogMessageCrLf("Read_PLC()", "StackTrace=" + e.StackTrace);
                sysLog.LogMessageCrLf("Read_PLC()", "exception=" + e.ToString());
                sysLog.LogMessageCrLf("Read_PLC()", "zelioConn=NULL, forzando reinstanciamiento de zelioConn.");
                this.zelioConn = null;
                return null;
            }
            ulong total;
            total = 0;
            for (int i = 0; i < cantBytes; i++)
            {
                total = (total << (8 * i));
                total += regs[i];
            }
            sysLog.LogMessageCrLf("Read_PLC()", " total=" + total);
            //Console.WriteLine("Read_PLC: total=" + total);
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
            //Console.WriteLine("valor[0]=" + valor[0] + "    valor[1]=" + valor[1]);
            this.AseguraPlcConnected();
            zelioConn.WriteSingleRegister(0, ZREG_J1XT1, valor);
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
                case OPEN:
                    if (zregO1XT1[ZS_SOUTH_OPEN])
                        hits |= 0x02;
                    break;
                case HALF:
                    if (zregO1XT1[ZS_SOUTH_50])
                        hits |= 0x02;
                    break;
                case CLOSE:
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
        /// Se leerá la info de los magnetic locks para determinar si 
        /// la cupula esta cerrada.
        /// </summary>
        /// <returns></returns>
        public Boolean IsClosed()
        {

            Boolean respuesta;
            respuesta = ( ( ! (Zreg_O1XT1[ZS_MAGNETIC_LOCK1]))
                            &&
                          ( ! (Zreg_O1XT1[ZS_MAGNETIC_LOCK2]))
                        );
            return respuesta;
        }

        #endregion

    }
}
