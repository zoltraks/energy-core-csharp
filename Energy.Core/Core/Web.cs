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
        [Obsolete]
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
                //for (int i = 0; i < headerArray.Length / 2; i++)
                //{
                //    request.Headers.Add(headerArray[i], headerArray[i + 1]);
                //}
                for (int i = 0; i < headerArray.Length; i++)
                {
                    request.Headers.Add(headerArray[i]);
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
        [Obsolete]
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
                //for (int i = 0; i < requestHeaders.Length / 2; i++)
                //{
                //    request.Headers.Add(requestHeaders[i], requestHeaders[i + 1]);
                //}
                for (int i = 0; i < requestHeaders.Length; i++)
                {
                    request.Headers.Add(requestHeaders[i]);
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
                    int responseHeadersCount = httpResponse.Headers.Count;
                    string[] responseHeaders = new string[httpResponse.Headers.Count];
                    string[] keyArray = httpResponse.Headers.AllKeys;
                    for (int i = 0; i < responseHeadersCount; i++)
                    {
                        responseHeaders[i] = string.Concat(keyArray[i], ": ", httpResponse.Headers[i]);
                    }
                    response.Headers.AddRange(responseHeaders);
                    //Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();
                    //List<string> headerList = new List<string>();
                    //string[] keys = httpResponse.Headers.AllKeys;
                    //for (int i = 0; i < keys.Length; i++)
                    //{
                    //    //d[keys[i]] = httpResponse.Headers[i];
                    //    headerList.Add(keys[i] + ": " + httpResponse.Headers[i]);
                    //}
                    //response.Headers = headerList;
                    ////responseHeaders = d.ToArray();
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
        /// Perform GET and return response from URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Get(Energy.Base.Http.Request request)
        {
            request.Method = "GET";
            return Execute(request);
        }

        /// <summary>
        /// Perform GET and return response from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="acceptType"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Get(string url, string acceptType)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "GET",
                Url = url,
                AcceptType = acceptType,
            };
            Energy.Base.Http.Response response = Execute(request);
            return response;
        }

        /// <summary>
        /// Perform GET and return response from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Get(string url)
        {
            return Get(url, null);
        }

        #endregion

        #region POST

        public static Energy.Base.Http.Response Post(Energy.Base.Http.Request request)
        {
            request.Method = "POST";
            return Execute(request);
        }

        /// <summary>
        /// Perform POST and return response from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url, object requestBody, string contentType, string[] headers)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "POST",
                Url = url,
                ContentType = contentType,
            };
            request.SetValue(requestBody);
            if (headers != null && headers.Length > 0)
            {
                request.Headers.AddRange(headers);
            }
            return Execute(request);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url, string requestBody, string contentType)
        {
            return Post(url, requestBody, contentType, null);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url, string requestBody)
        {
            return Post(url, requestBody, null, null);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url)
        {
            return Post(url, null, null, null);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url, byte[] data, string contentType)
        {
            return Post(url, data, contentType, null);
        }

        /// <summary>
        /// Perform POST and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url, byte[] data)
        {
            return Post(url, data, null, null);
        }

        /// <summary>
        /// Perform POST and return status from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Post(string url, byte[] data, out byte[] responseData)
        {
            Energy.Base.Http.Response response = Post(url, data, null, null);
            if (response == null)
            {
                responseData = null;
                return null;
            }
            responseData = response.Data;
            return response;
        }

        #endregion

        #region PUT

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Put(string url, object body, string contentType, string[] headers)
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request()
            {
                Method = "PUT",
                Url = url,
                ContentType = contentType,
            };
            request.SetValue(body);
            if (headers != null && headers.Length > 0)
            {
                request.Headers.AddRange(headers);
            }
            Energy.Base.Http.Response response = Execute(request);
            return response;
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Put(string url)
        {
            return Put(url, null, null, null);
        }

        /// <summary>
        /// Perform PUT and return string from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Put(string url, object body)
        {
            return Put(url, body, null, null);
        }

        #endregion

        #region HEAD

        /// <summary>
        /// Perform HEAD and return response from URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Head(Energy.Base.Http.Request request)
        {
            request.Method = "HEAD";
            return Execute(request);
        }

        /// <summary>
        /// Perform HEAD and return response from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Head(string url)
        {
            Energy.Base.Http.Request request = new Base.Http.Request()
            {
                Method = "HEAD",
                Url = url,
            };
            return Execute(request);
        }

        /// <summary>
        /// Perform HEAD and return response from URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseHeaders"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Head(string url, out string[] responseHeaders)
        {
            Energy.Base.Http.Request request = new Base.Http.Request()
            {
                Method = "HEAD",
                Url = url,
            };
            Energy.Base.Http.Response response = Execute(request);
            if (response == null)
            {
                responseHeaders = null;
                return null;
            }
            else
            {
                responseHeaders = response.Headers.ToArray();
                return response;
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Perform DELETE and return response from URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Delete(Energy.Base.Http.Request request)
        {
            request.Method = "DELETE";
            return Execute(request);
        }

        /// <summary>
        /// Perform DELETE and return response from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Energy.Base.Http.Response Delete(string url)
        {
            Energy.Base.Http.Request request = new Base.Http.Request()
            {
                Method = "DELETE",
                Url = url,
            };
            return Execute(request);
        }

        #endregion
    }
}
