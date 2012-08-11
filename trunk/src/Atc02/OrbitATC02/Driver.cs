//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for OrbitATC02
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;
using System.IO.Ports;
using System.Text;

namespace ASCOM.OrbitATC02
{
    //
    // Your driver's ID is ASCOM.OrbitATC02.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.OrbitATC02.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("939f5113-ed2b-400a-b14e-81604acc5ede")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Focuser : IFocuserV2
    {
        /// <summary>
        /// Puerto serial del ATC-02
        /// </summary>
        private SerialPort ttyATC;

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
        public readonly String OPENREM = "OPENREM   ";
        public readonly String CLOSEREM = "CLOSEREM  ";
        public readonly String READSETT = "READSETT  ";
        public readonly String UPDATEPC = "UPDATEPC  ";
        public readonly String SETFAN = "SETFAN ";
        public readonly String SETBFL = "BFL ";
        public readonly String FINDOPTIMA = "FINDOPTIMA";

        public static TraceLogger sysLog = new TraceLogger(null, "OrbitAtc02");

        private Form1 ventana;

        #region Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private const string driverId = "ASCOM.OrbitATC02.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "OrbitATC02 Focuser";
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
                p.DeviceType = "Focuser";
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

        public Focuser()
        {
            sysLog.Enabled = true;
            sysLog.LogMessageCrLf("Focuser()", "Instanciando Focuser.");
            this.ventana = new Form1();
            this.ventana.Show();
            this.conectado = false;
            this.ttyATC = new SerialPort(
                Properties.Settings.Default.CommPort,
                (int)Properties.Settings.Default.BaudRate
                );
            this.atcStat = null;
            this.statusTimer = new  System.Timers.Timer();
            this.statusTimer.Interval = (double)(1000 * ( Properties.Settings.Default.refreshStatusTimer));
            this.statusTimer.Elapsed += new System.Timers.ElapsedEventHandler(statusTimer_Elapsed);
            this.enMovimiento = false;
        }

        void statusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            sysLog.LogMessageCrLf("statusTimer_Elapsed()", "statusTimer_Elapsed");
            if (!enMovimiento)
            {
                this.RefreshAtcStatus();
                this.posicion = (int) (100.0*(this.atcStat.FocusPosition - 130));
                sysLog.LogMessageCrLf("statusTimer_Elapsed()", "posicion="+this.posicion);
            }
        }

        #region Implementation of IFocuserV2

        public void SetupDialog()
        {
            sysLog.LogMessageCrLf("SetupDialog()", "SetupDialog.");

            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            sysLog.LogMessageCrLf("Action()", "actionName=" + actionName + "\t actionParameters=" + actionParameters);

            String respuesta;
            respuesta = null;
            switch (actionName)
            {
                case "READSETT":
                    respuesta = this.ReadSettings();
                    break;
                case "UPDATEPC":                    
                    this.RefreshAtcStatus();
                    if (this.atcStat == null)
                    {
                        respuesta = "NULL STATUS";
                    }
                    else
                    {
                        respuesta = this.atcStat.ToString();
                    }
                    break;
                case "SETFAN":
                    respuesta = this.SetFan(Int32.Parse(actionParameters));
                    break;
                case "FINDOPTIMA":
                    respuesta = this.FindOptimal();
                    break;
                default:
                    break;
            }
            return respuesta;
        }

        public void CommandBlind(string command, bool raw)
        {
            sysLog.LogMessageCrLf("CommandBlind: command=" + command, "raw = " + raw);
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            sysLog.LogMessageCrLf("CommandBool: command=" + command, "raw = " + raw);
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            sysLog.LogMessageCrLf("CommandString: command=" + command, "raw = " + raw);
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            sysLog.LogMessageCrLf("Dispose()","not implemented.");
            throw new System.NotImplementedException();
        }

        public void Halt()
        {
            sysLog.LogMessageCrLf("Halt", "not implemented.");
            throw new System.NotImplementedException();
        }

        public void Move(int value)
        {
            sysLog.LogMessageCrLf("Move()", "value=" + value); 
            this.enMovimiento = true;
            this.posicion = value;
            Double bfl;
            int newPosition;
            StringBuilder comandoBFL;
            String respuesta;
            bfl = 130 + ( ((double) posicion) / 100.0 );
            comandoBFL = new StringBuilder();
            comandoBFL.Append(SETBFL);

            comandoBFL.Append(bfl.ToString("000.00"));
            //Console.WriteLine("->" + comandoBFL.ToString());
            sysLog.LogMessageCrLf("Move()", "enviando comando serial:"+comandoBFL.ToString());
            ttyATC.Write(comandoBFL.ToString());

            respuesta = LeerSerial(10).Trim();
            sysLog.LogMessageCrLf("Move()", "respuesta=" + respuesta); 

            String[] parte;
            int largoParte;
            
            parte = respuesta.Split((" ").ToCharArray());
            largoParte = parte.Length;
            try
            {
                newPosition = (int)(100.0 * (Double.Parse(parte[largoParte-1]) - 130));
            }
            catch (Exception )
            {
                newPosition = -1;
            }
            
            this.posicion = newPosition;
            this.enMovimiento = false;
        }

        public bool Connected
        {
            get {
                sysLog.LogMessageCrLf("Connected, get", "this.conectado=" + this.conectado);
                return this.conectado; 
            }
            set
            {
                if (value == true)
                {
                    sysLog.LogMessageCrLf("Connected:", "value == true"); 

                    this.ttyATC = new SerialPort(
                        Properties.Settings.Default.CommPort,
                        (int)Properties.Settings.Default.BaudRate
                        );
                    this.ttyATC.Open();
                    //Console.WriteLine("Esperando Conexion");
                    sysLog.LogMessageCrLf("Connected:", "Esperando Conexion.");
                    while (this.ttyATC.IsOpen != true)
                    {
                        Console.Write(".");
                        System.Threading.Thread.Sleep(300);
                    }
                    //Console.WriteLine("conectado!!");
                    sysLog.LogMessageCrLf("Connected:", "conectado!");

                    ttyATC.Write(OPENREM);
                    String respuesta;
                    respuesta = LeerSerial(10).Trim();
                    this.conectado = OPENREM.Contains(respuesta);
                    sysLog.LogMessageCrLf("Connected:", "respuesta=" + respuesta);
                    sysLog.LogMessageCrLf("Connected:", "this.conectado=" + this.conectado);
                    //if (Properties.Settings.Default.refreshStatus)
                    //{
                    this.statusTimer.Enabled = true;
                        this.statusTimer.Start();
                    //}
                }
                else
                {
                    sysLog.LogMessageCrLf("Connected:", "value == false"); 

                    this.statusTimer.Stop();
                    ttyATC.Write(CLOSEREM);
                    String respuesta;
                    respuesta = LeerSerial(10).Trim();
                    this.conectado = (!(CLOSEREM.Contains(respuesta)));
                    if (!conectado)
                    {
                        try
                        {
                            sysLog.LogMessageCrLf("Connected:", "cerrando tty.");
                            this.ttyATC.Close();
                        }
                        catch (InvalidOperationException)
                        {
                            conectado = true;
                        }
                    }
                }
            }
        }

        public string Description
        {
            get { return Properties.Settings.Default.DeviceDescription; }
        }

        public string DriverInfo
        {
            get
            {
                String respuesta;
                respuesta = "Chase500 ATC-02 Focuser. Desarrollado por Eduardo Maureira emaureir@gmail.com";
                return respuesta;
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
            get { return "Chase500 ATC-02 Focuser."; }
        }

        public ArrayList SupportedActions
        {
            get {
                ArrayList respuesta;
                respuesta = new ArrayList();
                respuesta.Add(READSETT);
                respuesta.Add(UPDATEPC);
                respuesta.Add(SETFAN);
                respuesta.Add(FINDOPTIMA);
                return respuesta; 
            }
        }

        public bool Absolute
        {
            get { return true; }
        }

        public bool IsMoving
        {
            get { return this.enMovimiento; }
        }

        // use the V2 connected property
        public bool Link
        {
            get { return this.Connected; }
            set { this.Connected = value; }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; i.e. the maximum number of steps allowed in one move operation.
        /// </summary>
        public int MaxIncrement
        {
            get { return 8000; }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        public int MaxStep
        {
            get { return 8000; }
        }

        public int Position
        {
            get {
                sysLog.LogMessageCrLf("Position:", "posicion "+this.posicion);
                return this.posicion;
            }
        }

        public double StepSize
        {
            get { return Properties.Settings.Default.StepSize; }
        }

        public bool TempComp
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool TempCompAvailable
        {
            get { return false; }
        }

        public double Temperature
        {
            get {
                if ((this.atcStat == null) || (!this.statusTimer.Enabled))
                {
                    this.RefreshAtcStatus();
                }
                return this.atcStat.AmbientTemperature;
            }
        }

        #endregion
        #region Metodos privados

        private String ReadSettings()
        {
            sysLog.LogMessageCrLf("ReadSettings()", "Enviando:" + READSETT);
            //Console.WriteLine("->" + READSETT);
            this.ttyATC.Write(READSETT);
            String respuesta;
            respuesta = LeerSerial(130).Trim();
            sysLog.LogMessageCrLf("ReadSettings()", "respuesta=" + respuesta);
            return respuesta;
        }

        private void RefreshAtcStatus()
        {
            sysLog.LogMessageCrLf("RefreshAtcStatus()", "Enviando:" + UPDATEPC);
            //Console.WriteLine("->" + UPDATEPC);
            this.ttyATC.Write(UPDATEPC);
            String rawStatus;
            rawStatus = LeerSerial(100).Trim();
            sysLog.LogMessageCrLf("RefreshAtcStatus()", "rawStatus=" + rawStatus);
            this.atcStat = new AtcStatus(rawStatus);
            this.posicion = (int)(100.0 * (this.atcStat.FocusPosition - 130));
        }

        private String FindOptimal()
        {
            Console.WriteLine("->" + FINDOPTIMA);
            this.ttyATC.Write(FINDOPTIMA);
            String respuesta;
            respuesta = LeerSerial(10).Trim();
            return respuesta;
        }

        private String SetFan(Int32 valor)
        {
            StringBuilder comandoFan;
            comandoFan = new StringBuilder();
            comandoFan.Append(SETFAN);
            comandoFan.Append(valor.ToString("000"));
            Console.WriteLine("->" + comandoFan);
            ttyATC.Write(comandoFan.ToString());
            String respuesta;
            respuesta = LeerSerial(10).Trim();
            return respuesta;
        }

        private String LeerSerial(int cantCaracteres)
        {
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
            return respuesta;
        }
        #endregion
    }
}
