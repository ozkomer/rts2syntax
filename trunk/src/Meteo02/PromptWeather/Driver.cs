//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM SafetyMonitor driver for PromptWeather
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM SafetyMonitor interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM SafetyMonitor Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;
using System.Windows.Forms;
using log4net;
using System.Net;
using System.IO;
using System.Text;

namespace ASCOM.PromptWeather
{
    //
    // Your driver's ID is ASCOM.PromptWeather.SafetyMonitor
    //
    // The Guid attribute sets the CLSID for ASCOM.PromptWeather.SafetyMonitor
    // The ClassInterface/None addribute prevents an empty interface called
    // _SafetyMonitor from being created and used as the [default] interface
    //
    [Guid("fbf484ac-c8a1-49cb-8630-f4a8fd579dd7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class SafetyMonitor //: ISafetyMonitor, IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SafetyMonitor));
        #region Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        // Set the driver Id to match the namespace.class
        //
        private const string driverId = "ASCOM.PromptWeather.SafetyMonitor";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "PromptWeather SafetyMonitor";
        #endregion

        #region ASCOM Registration

        // Constructor - Must be public for COM registration!
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
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

        #endregion

        #region IDisposable Members

        public void SetupDialog()
        {
            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }

        public bool Connected
        {
            get { return true; }
            set { }
        }
        /*
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

        public ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public bool IsSafe
        {
            get { return this.isSafe; }
        }

        void IDisposable.Dispose()
        {
            throw new System.NotImplementedException();
        }
        */
        #endregion

        private System.Timers.Timer timerCheckPrompt;
        private Boolean isSafe;
        private int cantDomosAbiertos;
        private Boolean[] is_openPrompt;


        public SafetyMonitor()
        {
            logger.Debug("Prompt Meteorologic monitor:Constructor Start");
            is_openPrompt = new Boolean[5];

            timerCheckPrompt = new System.Timers.Timer();
            timerCheckPrompt.Interval = 30000;

            timerCheckPrompt.Elapsed += new System.Timers.ElapsedEventHandler(timerCheckPrompt_Elapsed);
            timerCheckPrompt.Enabled = true;
            this.refreshIsSafe();
            logger.Debug("Prompt Meteorologic monitor:Constructor End");
        }

        void timerCheckPrompt_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            refreshIsSafe();
        }

        /// <summary>
        /// http://forums.devshed.com/c-programming-42/calling-a-url-from-c-371960.html
        /// </summary>
        /// <param name="url"></param>
        public static String GetUrl(String url)
        {
            String postData = "parameter=text&param2=text2";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] baASCIIPostData = encoding.GetBytes(postData);

            HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create(url);//"http://www.server.com/page.php");
            HttpWReq.Method = "POST";
            HttpWReq.Accept = "text/plain";
            HttpWReq.ContentType = "application/x-www-form-urlencoded";
            HttpWReq.ContentLength = baASCIIPostData.Length;

            Stream streamReq;
            streamReq = null;
            // Prepare web request and send the data.
            try
            {
                streamReq = HttpWReq.GetRequestStream();
            }
            catch (WebException exc)
            {
                logger.Debug("WebException " + exc.Message);
                return null;
            }

            //return null;

            streamReq.Write(baASCIIPostData, 0, baASCIIPostData.Length);

            // grab the response
            HttpWebResponse HttpWResp;
            HttpWResp = null;
            try
            {
                HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
            }
            catch (ProtocolViolationException e)
            {
                logger.Error(e.Message);
                return null;
            }
            catch (NotSupportedException e)
            {
                logger.Error(e.Message);
            }
            Stream streamResponse = HttpWResp.GetResponseStream();
            // And read it out
            StreamReader reader = new StreamReader(streamResponse);
            String response = reader.ReadToEnd();
            return response;
        }

        private void refreshIsSafe()
        {
            String strHtml;
            int[] indexDomePrompt;
            String[] strDomePrompt;
            indexDomePrompt = new int[5];
            strDomePrompt = new String[5];

            strHtml = GetUrl("http://skynet.unc.edu/live/update.php");

            cantDomosAbiertos = 0;
            if (strHtml != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    indexDomePrompt[i] = strHtml.IndexOf("domePrompt" + (i + 1));
                    Console.WriteLine("indexDomePrompt[" + i + "]=" + indexDomePrompt[i]);
                    strDomePrompt[i] = strHtml.Substring(indexDomePrompt[i], 30);
                    Console.WriteLine("strDomePrompt[" + i + "]=" + strDomePrompt[i]);
                    is_openPrompt[i] = strDomePrompt[i].Contains("OPEN");
                    Console.WriteLine("is_openPrompt[" + i + "]=" + is_openPrompt[i]);
                    if (is_openPrompt[i])
                    {
                        cantDomosAbiertos++;
                    }
                }
            }
            if (cantDomosAbiertos >= 3)
            {
                this.isSafe = true;
            }
            else
            {
                this.isSafe = false;
            }
        }

        public bool Safe
        {
            get { return this.isSafe; }
        }

        public int CantDomosAbiertos
        {
            get { return this.cantDomosAbiertos; }
        }
        ///////////////////////////////////////////////////

        public float AmbientTemperature
        {
            get { throw new System.NotImplementedException(); }
        }


        //public bool Available
        //{
        //    get { return true; }
        //}


        public float BarometricPressure
        {
            get { throw new System.NotImplementedException(); }
        }

        public float Clouds
        {
            get { throw new System.NotImplementedException(); }
            //get { return (float)0; }
        }


        public float DewPoint
        {
            get { throw new System.NotImplementedException(); }
        }

        public float InsideTemperature
        {
            get { throw new System.NotImplementedException(); }
            //get { return (float)0; }
        }


        public string Name
        {
            get
            {
                StringBuilder respuesta;
                respuesta = new StringBuilder();
                respuesta.Append("Weather<->Prompt Domes=");
                for (int i = 0; i < 5; i++)
                {
                    if (is_openPrompt[i])
                    {
                        respuesta.Append("*");
                    }
                    else
                    {
                        respuesta.Append("_");
                    }
                }
                return respuesta.ToString();
            }
        }

        public bool Precipitation { get { return false; } }


        public float RelativeHumidity
        {
            get { throw new System.NotImplementedException(); }
        }

        public float WindDirection
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Aclaracion:
        /// ACP quiere windVelocity. (definido por la interfaz que ellos exigen)
        /// Velocity -> Vectores. Lo que aqui se provee y solicita es claramente un escalar.
        /// </summary>
        public float WindVelocity
        {
            get { throw new System.NotImplementedException(); }
        }

        ///////////////////////////////////////////////////
    }
}