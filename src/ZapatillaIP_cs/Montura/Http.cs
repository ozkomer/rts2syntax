using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using log4net;

namespace Montura
{
    public class Http
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Http));
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
                logger.Debug("WebException "+exc.Message);
            }

            return null;

            //streamReq.Write(baASCIIPostData, 0, baASCIIPostData.Length);

            //// grab the response
            //HttpWebResponse HttpWResp;
            //HttpWResp = null;
            //try
            //{
            //    HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
            //}
            //catch (ProtocolViolationException e)
            //{

            //    logger.Error(e.Message);
            //}
            //catch (NotSupportedException e)
            //{
            //    logger.Error(e.Message);
            //}


            

            //Stream streamResponse = HttpWResp.GetResponseStream();

            //// And read it out
            //StreamReader reader = new StreamReader(streamResponse);
            //String response = reader.ReadToEnd();
            //return response;

        }
    }
}
