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
using ASCOM.Meteo02.tololoDataSetTableAdapters;

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

        #region variables instancia

        /// <summary>
        /// Aqui estan los últimos 40 registros.
        /// Y se evalua si la condiciones de tiempo
        /// son seguras para la ovserbación.
        /// </summary>
        private WeatherAnalisis weather_Analisis;

        /// <summary>
        /// objeto que contiene las variables meteorologicas.
        /// </summary>
        //private WeatherRow weatherRow;

        /// <summary>
        /// Timer que se gatilla cada 30 segundos. La base de datos de tololo
        /// se gatilla cada 60 segundos. Luego el intervalo escogido para este timer
        /// siempre deberia garantizar que todos los ultimos registros
        /// sean leidos con un retraso inferior a 30 segundos.
        /// </summary>
        private System.Timers.Timer timerLeer;

        /// <summary>
        /// Determina si el driver se ha comunicado con la base de datos
        /// </summary>
        private bool conectado;

        /// <summary>
        /// Objeto con la tabla de datos meteorologicos
        /// </summary>
        private DataTableWeatherTableAdapter dtWeatheTa;
        
        #endregion
        public SafetyMonitor()
        {            
            dtWeatheTa = new DataTableWeatherTableAdapter();
            weather_Analisis = new WeatherAnalisis();

            this.conectado = (dtWeatheTa != null); 
            this.LeerUltimoRegistro();
            timerLeer = new System.Timers.Timer();
            timerLeer.Interval = 30000;
            timerLeer.Elapsed+=new System.Timers.ElapsedEventHandler(timerLeer_Elapsed);
            timerLeer.Start();
        }

        void timerLeer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.LeerUltimoRegistro();
        }

        
        void LeerUltimoRegistro()
        {

            tololoDataSet.DataTableWeatherDataTable dtWeatherDT;
            tololoDataSet.DataTableWeatherRow registro;
            if (weather_Analisis.Count == 0)
            {
                dtWeatherDT = dtWeatheTa.GetDataBy40MostRecent();
                int largoDT;
                largoDT = dtWeatherDT.Count;
                //Recorremos el DataTable desde el pasado hacia el futuro
                for (int i = (largoDT - 1); i >= 0; i--)
                {
                    WeatherRow nuevo;
                    tololoDataSet.DataTableWeatherRow nuevoRow;
                    nuevoRow = (tololoDataSet.DataTableWeatherRow)dtWeatherDT.Rows[i];
                    nuevo = new WeatherRow(nuevoRow);
                    weather_Analisis.insertar(nuevo);
                }
            }
            else
            {
                WeatherRow nuevo;
                dtWeatherDT = dtWeatheTa.GetDataByMostRecent();
                registro = (tololoDataSet.DataTableWeatherRow)dtWeatherDT.Rows[0];
                nuevo = new WeatherRow(registro);
                weather_Analisis.insertar(nuevo);
            }
        }

        public WeatherAnalisis Weather_Analisis
        {
            get { return this.weather_Analisis; }

        }

        /// <summary>
        /// Expone el timer para que una aplicacion pueda
        /// desplegar los nuevos valores apenas se obtengan
        /// </summary>
        public System.Timers.Timer TimerLeer
        {
            get { return this.timerLeer; }
            set { this.timerLeer = value; }
        }

        public DateTime FechaHora
        { get { return this.weather_Analisis.Ultimo.FechaHora; } }

        #region Weather Object
        public float AmbientTemperature
        {
            get { return this.weather_Analisis.Ultimo.AmbientTemperature; }
        }

        //public bool Available
        //{
        //    get { return true; }
        //}

        
        public float BarometricPressure
        {
            get { return this.weather_Analisis.Ultimo.BarometricPressure; }
        }

        public float Clouds
        {
            get { throw new System.NotImplementedException(); }
            //get { return (float)0; }
        }


        public float DewPoint
        {
            get { return this.weather_Analisis.Ultimo.DewPoint; }
        }

        public float InsideTemperature
        {
            get { throw new System.NotImplementedException(); }
            //get { return (float)0; }
        }


        public string Name
        {
            get { return "Chase500 Weather."; }
        }

        public bool Precipitation { get { return false; } }

        public float RelativeHumidity
        {
            get { return this.weather_Analisis.Ultimo.RelativeHumidity; }
        }

        public bool Safe
        {
            get { return this.weather_Analisis.isSafe(); }
        }

        public bool Connected
        {
            get { return conectado; }
            set { this.conectado = value; }
        }

        public float WindDirection
        {
            get { return this.weather_Analisis.Ultimo.WindDirection; }
        }

        /// <summary>
        /// Aclaracion:
        /// ACP quiere windVelocity. (definido por la interfaz que ellos exigen)
        /// Velocity -> Vectores. Lo que aqui se provee y solicita es claramente un escalar.
        /// </summary>
        public float WindVelocity
        {
            get { return this.weather_Analisis.Ultimo.WindSpeed; }
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