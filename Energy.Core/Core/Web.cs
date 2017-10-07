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
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="acceptType"></param>
        /// <param name="encoding"></param>
        /// <param name="detectEncodingFromByteOrderMarks"></param>
        /// <returns></returns>
        public static string Rest(string method, string url, string body, string contentType, string acceptType
            , System.Text.Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            if (encoding == null)
                encoding = System.Text.Encoding.UTF8;

            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;
            if (!string.IsNullOrEmpty(acceptType))
                request.Accept = acceptType;

            byte[] data = null;

            if (string.IsNullOrEmpty(body))
            {
                request.ContentLength = 0;
            }
            else
            {
                data = encoding.GetBytes(body);
                request.ContentLength = data.Length;
            }

            WebResponse webResponse;
            webResponse = request.GetResponse();

            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream == null)
                    return null;

                if (data != null)
                    webStream.Write(data, 0, data.Length);

                using (StreamReader responseReader = new StreamReader(webStream, encoding, detectEncodingFromByteOrderMarks))
                {
                    string response = responseReader.ReadToEnd();
                    return response;
                }
            }
        }

        /// <summary>
        /// Perform GET and return string from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="acceptType"></param>
        /// <returns></returns>
        public static string Get(string url, string acceptType)
        {
            return Rest("GET", url, null, null, acceptType, null, true);
        }

        /// <summary>
        /// Perform GET and return string from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Rest("GET", url, null, null, null, null, true);
        }
    }
}
