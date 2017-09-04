using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Web connectivity
    /// </summary>
    public class Web
    {
        /// <summary>
        /// Perform GET and return string from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Get(string url, string contentType)
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = contentType;
            request.ContentLength = 0;

            WebResponse webResponse;
            webResponse = request.GetResponse();

            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream == null)
                    return null;

                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    return response;
                }
            }
        }
    }
}
