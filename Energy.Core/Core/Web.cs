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
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="acceptType"></param>
        /// <param name="encoding"></param>
        /// <param name="headerArray"></param>
        /// <param name="detectEncodingFromByteOrderMarks"></param>
        /// <returns></returns>
        public static string Rest(string method, string url
            , string body
            , string contentType
            , string acceptType
            , System.Text.Encoding encoding
            , string[] headerArray
            , bool detectEncodingFromByteOrderMarks)
        {
            if (encoding == null)
                encoding = System.Text.Encoding.UTF8;

            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;
            //if (acceptType != null && acceptType.Length > 0)
            //    request.Accept = acceptType[0];
            if (!string.IsNullOrEmpty(acceptType))
                request.Accept = acceptType;

            if (headerArray != null && headerArray.Length > 0)
            {
                for (int i = 0; i < headerArray.Length / 2; i++)
                {
                    request.Headers.Add(headerArray[i], headerArray[i + 1]);
                }
            }

            if (!string.IsNullOrEmpty(body))
            {
                byte[] data = encoding.GetBytes(body);
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    if (requestStream == null)
                        return null;

                    requestStream.Write(data, 0, data.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                        return null;

                    using (StreamReader responseReader = new StreamReader(responseStream, encoding, detectEncodingFromByteOrderMarks))
                    {
                        string responseText = responseReader.ReadToEnd();
                        return responseText;
                    }
                }
            }
        }

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="acceptType"></param>
        /// <returns></returns>
        public static string Get(string url, string body, string acceptType)
        {
            return Rest("GET", url, body, null, acceptType, null, null, true);
        }

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string Get(string url, string body)
        {
            return Rest("GET", url, body, null, null, null, null, true);
        }

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Rest("GET", url, null, null, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Post(string url, string body, string contentType)
        {
            return Rest("POST", url, body, contentType, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string Post(string url, string body)
        {
            return Rest("POST", url, body, null, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Post(string url)
        {
            return Rest("POST", url, null, null, null, null, null, true);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Put(string url, string body, string contentType)
        {
            return Rest("PUT", url, body, contentType, null, null, null, true);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Put(string url)
        {
            return Rest("PUT", url, null, null, null, null, null, true);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string Put(string url, string body)
        {
            return Rest("PUT", url, body, null, null, null, null, true);
        }

        public static string Post(string url, string body, string contentType, string[] headerArray)
        {
            return Rest("POST", url, body, contentType, null, null, headerArray, true);
        }
    }
}
