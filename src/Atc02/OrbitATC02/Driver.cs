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

        public static TraceLogger sysLog = new TraceLogger(null, "OrbitAtc02");

        public static readonly String cmdGetXmlStatus = "GetXmlStatus";

        Atc02Comm singleton;

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
            this.singleton = Atc02Comm.Instance;
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
                    respuesta = this.singleton.ReadSettings();
                    break;
                case "UPDATEPC":                    
                    this.singleton.RefreshAtcStatus();
                    if (this.singleton.AtcStat == null)
                    {
                        respuesta = "NULL STATUS";
                    }
                    else
                    {
                        respuesta = this.singleton.AtcStat.ToString();
                    }
                    break;
                case "SETFAN":
                    respuesta = this.singleton.SetFan(Int32.Parse(actionParameters));
                    break;
                case "FINDOPTIMA":
                    respuesta = this.singleton.FindOptimal();
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
            String respuesta;
            respuesta = "Comando Desconocido";
            sysLog.LogMessageCrLf("CommandString: command=" + command, "raw = " + raw);
            if (command == cmdGetXmlStatus)
            {
                respuesta = this.singleton.AtcStat.ToString();
            }
            return respuesta;
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
            this.singleton.Move(value);
        }

        public bool Connected
        {
            get {
                sysLog.LogMessageCrLf("Connected, get", "this.conectado=" + this.singleton.Conectado);
                return this.singleton.Conectado; 
            }
            set
            {
                if (value == true)
                {
                    Focuser.sysLog.LogMessageCrLf("Connected:", "--->conectar.");
                    this.singleton.conectar();
                }
                else
                {
                    Focuser.sysLog.LogMessageCrLf("Connected:", "--->desConectar");
                    this.singleton.desConectar();
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
                respuesta.Add(Atc02Comm.READSETT);
                respuesta.Add(Atc02Comm.UPDATEPC);
                respuesta.Add(Atc02Comm.SETFAN);
                respuesta.Add(Atc02Comm.FINDOPTIMA);
                return respuesta; 
            }
        }

        public bool Absolute
        {
            get { return true; }
        }

        public bool IsMoving
        {
            get { return this.singleton.EnMovimiento; }
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
                sysLog.LogMessageCrLf("Position:", "posicion ="+this.singleton.Posicion);
                return this.singleton.Posicion;
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
                //if ((this.singleton.AtcStat == null) || (!Properties.Settings.Default.refreshStatus))
                //{
                //    this.singleton.RefreshAtcStatus();
                //}
                return this.singleton.AtcStat.AmbientTemperature;
            }
        }

        #endregion
    }
}
