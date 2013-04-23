using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net;
using System.IO;

namespace ASCOM.Meteo02
{
    public class PromptAnalisis
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PromptAnalisis));
        private System.Timers.Timer timerCheckPrompt;

        /// <summary>
        /// Si a partir del status de los prompt se determina un safe, esto se refleja aqui.
        /// </summary>
        private Boolean isSafe;

        private int cantDomosAbiertos;
        private Boolean[] is_openPrompt;

        public bool Safe
        {
            get { return this.isSafe; }
        }
        
        public PromptAnalisis()
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

        private void timerCheckPrompt_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            refreshIsSafe();
        }

        /// <summary>
        /// Este método es invocado desde un timer cada 30 segundos.
        /// Determina cuantos telescopios prompt estan abiertos.
        /// Si 3 o más prompt estan abiertos, entonces determina un Safe.
        /// </summary>
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
            
    }
}
