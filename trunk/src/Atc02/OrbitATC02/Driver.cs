//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for OrbitATC02
//
// Description:	Esta versión del driver se conecta a la aplicacion "Focus Server".
//              Focus server se encarga finalmente de las comunicaciones seriales con el ATC02.
//              Las comunicaciones entre este driver y FocusServer son breves sesiones TCP/IP.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		Eduardo Maureira <emaureir@gmail.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 09-09-2012	Eduardo Maureira	1.0.0	Initial edit, from ASCOM Focuser Driver template
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
using System.Net.Sockets;
using System.Net;

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

        private static ASCOM.OrbitATC02.Properties.Settings settings = ASCOM.OrbitATC02.Properties.Settings.Default;

        /// <summary>
        /// Manejo interno del estado conectado para engañar a focusMax.
        /// </summary>
        private Boolean conectado;

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
            this.conectado = false;
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
                    respuesta = this.EnviaMensaje("ReadSettings");
                    break;
                case "status":
                    respuesta =  this.EnviaMensaje("status");
                    break;
                //case "SETFAN":
                //    break;
                case "FINDOPTIMA":
                    respuesta = this.EnviaMensaje("FindOptimal");
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
                respuesta = this.EnviaMensaje("FindOptimal");
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
            //throw new System.NotImplementedException();
        }

        public void Move(int value)
        {
            sysLog.LogMessageCrLf("Move()", "value=" + value);
            String comando;
            comando = ("move " + value);
            this.EnviaMensaje(comando);            
        }

        public bool Connected
        {
            get {
                //String respuesta;
                //respuesta = this.EnviaMensaje("IsConnected");
                //conectado = Boolean.Parse( respuesta );
                sysLog.LogMessageCrLf("Connected, get", "this.conectado=" + this.conectado);
                return this.conectado;
            }
            set
            {
                this.conectado = value;
                if (value == true)
                {
                    Focuser.sysLog.LogMessageCrLf("Connected:", "--->conectar.");
                    if (settings.StartUpSecondary)
                    {
                        this.EnviaMensaje("FindOptimal");
                        System.Threading.Thread.Sleep(8000);
                        //this.EnviaMensaje("SetFan 0");
                        System.Threading.Thread.Sleep(1000);
                        String comando;
                        comando = ("move " + settings.StartUpSecondaryPosition);
                        this.EnviaMensaje(comando);
                    }
                }
                else
                {
                    Focuser.sysLog.LogMessageCrLf("Connected:", "--->desConectar");
                    //this.EnviaMensaje("SetFan 100");
                }
            }
        }

        public string Description
        {
            get { return this.EnviaMensaje("ReadSettings"); }
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
                respuesta.Add("conectar");
                respuesta.Add("desconectar");
                respuesta.Add("move");
                respuesta.Add("GetXmlStatus");
                respuesta.Add("GetPosition");
                respuesta.Add("GetAmbientTemperature");
                respuesta.Add("IsMoving");
                respuesta.Add("IsConnected");
                respuesta.Add("ReadSettings");
                respuesta.Add("FindOptimal");
                respuesta.Add("SetFan");
                return respuesta; 
            }
        }

        public bool Absolute
        {
            get { return true; }
        }

        public bool IsMoving
        {
            get {
                String respuesta;
                respuesta = this.EnviaMensaje("IsMoving");
                return Boolean.Parse(respuesta); 
            }
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
                String respuesta;
                int posicion;
                respuesta = this.EnviaMensaje("GetPosition");
                respuesta = respuesta.Replace("\0", "");
                posicion = Int32.Parse(respuesta);
                sysLog.LogMessageCrLf("Position:", "posicion =" + posicion);
                return posicion;
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
                String respuesta;
                respuesta = this.EnviaMensaje("GetAmbientTemperature");
                respuesta = respuesta.Replace("\0", "");
                int temperature;
                temperature = Int32.Parse(respuesta);
                return temperature;
            }
        }

        #endregion

        private String EnviaMensaje(String mensaje)
        {
            String respuesta;
            respuesta = null;
            TcpClient client = new TcpClient();
            IPEndPoint serverEndPoint;
            IPAddress focusServer;
            focusServer = IPAddress.Parse(settings.FocusServer);

            serverEndPoint = new IPEndPoint(focusServer,(int) settings.FocusPort);

            try
            {
                client.Connect(serverEndPoint);
            }
            catch (SocketException)
            {
                this.conectado = false;
                return "Error de Comunicacion";
            }

            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(mensaje);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            byte[] bufferIn;
            bufferIn = new byte[800];
            int bytesRead;
            bytesRead = clientStream.Read(bufferIn, 0, 800);
            respuesta = encoder.GetString(bufferIn, 0, 800);
            Console.WriteLine("-----------");
            Console.WriteLine(respuesta);
            Console.WriteLine("-----------");
            client.Close();
            return respuesta;
        }

    }
}
