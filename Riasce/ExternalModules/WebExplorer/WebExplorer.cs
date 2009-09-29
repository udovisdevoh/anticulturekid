using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace AntiCulture.Kid
{
    /// <summary>
    /// Used to get websites HTML contents
    /// </summary>
    static class WebExplorer
    {
        #region Fields
        /// <summary>
        /// Default user agent
        /// </summary>
        private static readonly string userAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; Media Center PC 6.0; InfoPath.2; MS-RTC LM 8)";
        #endregion

        #region Public Methods
        /// <summary>
        /// Get string page content from URL
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string page content from URL</returns>
        public static string GetStringPageContent(string url)
        {
            // used to build entire input
            StringBuilder content = new StringBuilder();
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            // used on each read operation
            byte[] buf = new byte[8192];


            request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = userAgent;
            if (request != null)
            {
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch
                {
                }
            }

            if (response != null)
            {
                Stream resStream = response.GetResponseStream();


                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        content.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?
            }

            return content.ToString();
        }

        /// <summary>
        /// Get byte array content from URL
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>byte array content from URL</returns>
        public static byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);

                myReq.UserAgent = userAgent;

                WebResponse myResp = myReq.GetResponse();

                Stream stream = myResp.GetResponseStream();
                //int i;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    //i = (int)(stream.Length);
                    b = br.ReadBytes(500000);
                    br.Close();
                }
                myResp.Close();
            }
            catch
            {
                b = new byte[1];
            }
            return b;
        }
        #endregion
    }
}
