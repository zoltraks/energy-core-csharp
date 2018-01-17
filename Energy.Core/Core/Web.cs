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
        #region Private

        private static WebResponse GetResponseWithoutException(WebRequest request)
        {
            if (request == null)
            {
                return null;
            }
            try
            {
                return request.GetResponse();
            }
            catch (WebException x)
            {
                if (x.Response == null)
                {
                    throw;
                }
                return x.Response;
            }
        }

        #endregion

        #region Rest

        /// <summary>
        /// Rest service class
        /// </summary>
        public class Rest { }

        #endregion

        #region Request

        /// <summary>
        /// Perform HTTP request and return string from URL
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
        public static string Request(string method, string url
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

            using (HttpWebResponse response = (HttpWebResponse)GetResponseWithoutException(request))
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
        /// Perform HTTP request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="acceptType"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="responseData"></param>
        /// <param name="responseHeaders"></param>
        /// <returns>HTTP status code</returns>
        public static int Request(string method, string url, byte[] data
            , string contentType
            , string acceptType
            , string[] requestHeaders
            , out string[] responseHeaders
            , out byte[] responseData
            )
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;
            if (!string.IsNullOrEmpty(acceptType))
                request.Accept = acceptType;
            if (requestHeaders != null && requestHeaders.Length > 0)
            {
                for (int i = 0; i < requestHeaders.Length / 2; i++)
                {
                    request.Headers.Add(requestHeaders[i], requestHeaders[i + 1]);
                }
            }

            int statusCode = 0;
            responseHeaders = null;
            responseData = null;

            if (data != null && data.Length > 0)
            {
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    if (requestStream == null)
                        return 0;

                    requestStream.Write(data, 0, data.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)GetResponseWithoutException(request))
            {
                statusCode = (int)response.StatusCode;
                if (response.Headers.Count > 0)
                {
                    Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();
                    string[] keys = response.Headers.AllKeys;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        d[keys[i]] = response.Headers[i];
                    }
                    responseHeaders = d.ToArray();
                }
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (Energy.Base.ByteArrayBuilder builder = new Energy.Base.ByteArrayBuilder(responseStream))
                        {
                            responseData = builder.ToArray();
                        }
                    }
                    return statusCode;
                }
            }
        }

        #endregion

        #region GET

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="acceptType"></param>
        /// <returns></returns>
        public static string Get(string url, string acceptType)
        {
            return Request("GET", url, null, null, acceptType, null, null, true);
        }

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Request("GET", url, null, null, null, null, null, true);
        }

        /// <summary>
        /// Perform GET and return status code with data from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static int Get(string url, out byte[] responseData)
        {
            string[] responseHeaders;
            int statusCode = Request("GET", url, null, null, null, null, out responseHeaders, out responseData);
            return statusCode;
        }

        #endregion

        #region POST

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="headerArray"></param>
        /// <returns></returns>
        public static string Post(string url, string body, string contentType, string[] headerArray)
        {
            return Request("POST", url, body, contentType, null, null, headerArray, true);
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
            return Request("POST", url, body, contentType, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string Post(string url, string body)
        {
            return Request("POST", url, body, null, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Post(string url)
        {
            return Request("POST", url, null, null, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] Post(string url, byte[] body, string contentType)
        {
            string[] responseHeaders;
            byte[] responseData;
            int statusCode = Request("POST", url, body, contentType, null, null, out responseHeaders, out responseData);
            return responseData;
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static byte[] Post(string url, byte[] body)
        {
            string[] responseHeaders;
            byte[] responseData;
            int statusCode = Request("POST", url, body, null, null, null, out responseHeaders, out responseData);
            return responseData;
        }

        /// <summary>
        /// Perform POST and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static int Post(string url, byte[] body, out byte[] responseData)
        {
            string[] responseHeaders;
            int statusCode = Request("POST", url, body, null, null, null, out responseHeaders, out responseData);
            return statusCode;
        }

        /// <summary>
        /// Perform POST and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="responseString"></param>
        /// <returns></returns>
        public static int Post(string url, byte[] body, out string responseString)
        {
            string[] responseHeaders;
            byte[] responseData;
            int statusCode = Request("POST", url, body, null, null, null, out responseHeaders, out responseData);
            responseString = System.Text.Encoding.UTF8.GetString(responseData);
            return statusCode;
        }

        #endregion

        #region PUT

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Put(string url, string body, string contentType)
        {
            return Request("PUT", url, body, contentType, null, null, null, true);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Put(string url)
        {
            return Request("PUT", url, null, null, null, null, null, true);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string Put(string url, string body)
        {
            return Request("PUT", url, body, null, null, null, null, true);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static byte[] Put(string url, byte[] body)
        {
            string[] responseHeaders;
            byte[] responseData;
            int statusCode = Request("PUT", url, body, null, null, null, out responseHeaders, out responseData);
            return responseData;
        }

        /// <summary>
        /// Perform PUT and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static int Put(string url, byte[] body, out byte[] responseData)
        {
            string[] responseHeaders;
            int statusCode = Request("PUT", url, body, null, null, null, out responseHeaders, out responseData);
            return statusCode;
        }

        /// <summary>
        /// Perform PUT and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="responseString"></param>
        /// <returns></returns>
        public static int Put(string url, byte[] body, out string responseString)
        {
            string[] responseHeaders;
            byte[] responseData;
            int statusCode = Request("PUT", url, body, null, null, null, out responseHeaders, out responseData);
            responseString = System.Text.Encoding.UTF8.GetString(responseData);
            return statusCode;
        }

        #endregion
    }
}
