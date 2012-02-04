//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM SafetyMonitor driver for Meteo02
//
// Description:	Se ha reutilizado la interfaz "SafetyMonitor" y el mecanismo de 
//				Creacion de drivers ascom para crear un Driver ACP de Meteorologia.
//				Las razones para usar el mecanismo de ascom son:
//				1) Permite determinar el ProgID de la "dll" generada.
//				2) Permite usar Visual Studio .net y el lenguange C# para programar el Driver.
//              Con C# y Visual Studio a mano, el resto del proyecto se simplifica extremadamente
//              debido a la flexibilida y potencialidades del lenguage y del ambiente de desarrollo.
//
// Implements:	ASCOM SafetyMonitor interface version: 1.0
// Author:		(XXX) Eduardo Maureira <emaureir@gmail.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 04-002-2012	XXX	0.5.0	Initial edit, from ASCOM SafetyMonitor Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.Meteo02
{
    //
    // Your driver's ID is ASCOM.Meteo02.SafetyMonitor
    //
    // The Guid attribute sets the CLSID for ASCOM.Meteo02.SafetyMonitor
    // The ClassInterface/None addribute prevents an empty interface called
    // _SafetyMonitor from being created and used as the [default] interface
    //
    [Guid("fa187cc7-b469-4b4f-8ac5-11fef8f5e579")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class SafetyMonitor //: ISafetyMonitor, IDisposable
    {
        #region Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        // Set the driver Id to match the namespace.class
        //
        private const string driverId = "ASCOM.Meteo02.SafetyMonitor";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "Meteo02 SafetyMonitor";
        #endregion

        #region ASCOM Registration

        // Constructor - Must be public for COM registration!
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        public static void RegUnregASCOM(bool bRegister)
        {
            using (var p = new Profile())
            {
                p.DeviceType = "SafetyMonitor";
                if (bRegister)
                {
                    p.Register(driverId, driverDescription);
                    p.CreateSubKey(driverId, "SafetyMonitores");
                }
                else
                {
                    p.Unregister(driverId);
                }
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

        public void SetupDialog()
        {
            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }
        #endregion

        #region Weather Object

        public float AmbientTemperature
        {
            get { return (float)33; }

        }

        //public bool Available
        //{
        //    get { return true; }
        //}


        public float BarometricPressure
        {
            get { return (float)33; }

        }

        public float Clouds
        {
            get { return (float)33; }

        }


        public float DewPoint
        {
            get { return (float)33; }

        }

        public float InsideTemperature
        {
            get { return (float)33; }

        }


        public string Name
        {
            get { return "Chase500 Weather."; }
        }

        public bool Precipitation { get { return false; } }

        public float RelativeHumidity
        {
            get { return (float)33; }

        }

        public bool Safe
        {
            get { return true; }
        }

        private bool conectado;
        public bool Connected
        {
            get { return conectado; }
            set { this.conectado = value; }
        }


        public float WindDirection
        {
            get { return (float)33; }

        }

        public float WindVelocity
        {
            get { return (float)33; }

        }


        #endregion
        /*
        #region IDisposable Members



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

        //public bool Connected
        //{
        //    get { throw new System.NotImplementedException(); }
        //    set { throw new System.NotImplementedException(); }
        //}

        public string Description
        {
            get { throw new System.NotImplementedException(); }
        }

        public string DriverInfo
        {
            get { throw new System.NotImplementedException(); }
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

        //public string Name
        //{
        //    get { throw new System.NotImplementedException(); }
        //}

        public ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public bool IsSafe
        {
            get { throw new System.NotImplementedException(); }
        }

        void IDisposable.Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion
        */

    }
}