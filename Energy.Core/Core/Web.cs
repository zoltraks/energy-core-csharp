using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

        #region Certificate validation

        private static bool ServerIgnoreCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return (true);
        }

        private static readonly Energy.Base.Lock _IgnoreCertificateValidationLock = new Energy.Base.Lock();

        private static bool _IgnoreCertificateValidation = false;

        public static bool IgnoreCertificateValidation
        {
            get
            {
                return GetIgnoreCertificateValidation();
            }
            set
            {
                SetIgnoreCertificateValidation(value);
            }
        }

        private static bool GetIgnoreCertificateValidation()
        {
            lock (_IgnoreCertificateValidationLock)
            {
                return _IgnoreCertificateValidation;
            }
        }

        private static void SetIgnoreCertificateValidation(bool value)
        {
            lock (_IgnoreCertificateValidationLock)
            {
                ServicePointManager.ServerCertificateValidationCallback -= ServerIgnoreCertificateValidationCallback;
                _IgnoreCertificateValidation = value;
                if (value)
                {
                    ServicePointManager.ServerCertificateValidationCallback += ServerIgnoreCertificateValidationCallback;
                }
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
            Uri requestUri = new Uri(url);
            request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = method;
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(requestUri);
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

        #region Execute

        /// <summary>
        /// Perform HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpWebRequest">Custom HttpWebRequest object to use</param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Execute(Energy.Base.Http.Request request, HttpWebRequest httpWebRequest)
        {
            HttpWebRequest httpRequest = null;
            //if (request.RequestObject != null)
            //    httpRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            //else
            //    httpRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            if (httpWebRequest != null)
                httpRequest = httpWebRequest;
            else
                httpRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            httpRequest.Method = request.Method;
            if (!string.IsNullOrEmpty(request.ContentType))
                httpRequest.ContentType = request.ContentType;
            if (!string.IsNullOrEmpty(request.AcceptType))
                httpRequest.Accept = request.AcceptType;
            if (request.Headers != null && request.Headers.Count > 0)
            {
                for (int i = 0; i < request.Headers.Count; i++)
                {
                    httpRequest.Headers.Add(request.Headers[i]);
                }
            }

            Energy.Base.Http.Response response = new Base.Http.Response();

            int statusCode = 0;
            //responseHeaders = null;
            byte[] responseData = null;

            byte[] data = request.Data;

            if (data != null && data.Length > 0)
            {
                httpRequest.ContentLength = data.Length;

                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    if (requestStream == null)
                        return response;

                    requestStream.Write(data, 0, data.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }
            }

            using (HttpWebResponse httpResponse = (HttpWebResponse)GetResponseWithoutException(httpRequest))
            {
                //response.ResponseObject = httpResponse;
                statusCode = (int)httpResponse.StatusCode;
                response.StatusCode = statusCode;
                if (httpResponse.Headers.Count > 0)
                {
                    //Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();
                    List<string> headerList = new List<string>();
                    string[] keys = httpResponse.Headers.AllKeys;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        //d[keys[i]] = httpResponse.Headers[i];
                        headerList.Add(keys[i] + ": " + httpResponse.Headers[i]);
                    }
                    response.Headers = headerList;
                    //responseHeaders = d.ToArray();
                }
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (Energy.Base.ByteArrayBuilder builder = new Energy.Base.ByteArrayBuilder(responseStream))
                        {
                            responseData = builder.ToArray();
                        }
                    }
                    response.Data = responseData;
                    return response;
                }
            }
        }

        /// <summary>
        /// Perform HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Execute(Energy.Base.Http.Request request)
        {
            return Execute(request, null);
        }

        #endregion

        #region GET

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "GET",
                Url = url,
            };
            Energy.Base.Http.Response response = Execute(request);
            return response.Body;
        }

        /// <summary>
        /// Perform GET and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="acceptType"></param>
        /// <returns></returns>
        public static string Get(string url, string acceptType)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "GET",
                Url = url,
                AcceptType = acceptType,
            };
            Energy.Base.Http.Response response = Execute(request);
            return response.Body;
        }

        /// <summary>
        /// Perform GET and return status code with data from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static int Get(string url, out byte[] responseData)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "GET",
                Url = url,
            };
            Energy.Base.Http.Response response = Execute(request);
            responseData = response.Data;
            return response.StatusCode;
        }

        /// <summary>
        /// Perform GET and return status code with data from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseBody"></param>
        /// <returns></returns>
        public static int Get(string url, out string responseBody)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "GET",
                Url = url,
            };
            Energy.Base.Http.Response response = Execute(request);
            responseBody = response.Body;
            return response.StatusCode;
        }

        #endregion

        #region POST

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <param name="contentType"></param>
        /// <param name="headerArray"></param>
        /// <returns></returns>
        public static string Post(string url, string requestBody, string contentType, string[] headerArray)
        {
            return Request("POST", url, requestBody, contentType, null, null, headerArray, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Post(string url, string requestBody, string contentType)
        {
            return Request("POST", url, requestBody, contentType, null, null, null, true);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static string Post(string url, string requestBody)
        {
            return Request("POST", url, requestBody, null, null, null, null, true);
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

        #region HEAD

        /// <summary>
        /// Perform HEAD and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseHeaders"></param>
        /// <returns></returns>
        public static int Head(string url, out string[] responseHeaders)
        {
            byte[] responseData;
            int statusCode = Request("HEAD", url, null, null, null, null, out responseHeaders, out responseData);
            return statusCode;
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Perform DELETE and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseHeaders"></param>
        /// <returns></returns>
        public static int Delete(string url, out string[] responseHeaders)
        {
            byte[] responseData;
            int statusCode = Request("DELETE", url, null, null, null, null, out responseHeaders, out responseData);
            return statusCode;
        }

        /// <summary>
        /// Perform DELETE and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static int Delete(string url, out byte[] responseData)
        {
            string[] responseHeaders;
            int statusCode = Request("DELETE", url, null, null, null, null, out responseHeaders, out responseData);
            return statusCode;
        }

        #endregion
    }
}
