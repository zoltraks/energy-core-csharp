using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace Energy.Base
{
    public class Http
    {
        /// <summary>
        /// Represents HTTP message body used in request and response
        /// </summary>
        public class Message
        {
            #region Public

            public string Body { get { return GetBody(); } set { SetBody(value); } }

            public byte[] Data { get { return GetData(); } set { SetData(value); } }

            [XmlIgnore]
            public System.Text.Encoding Encoding { get { return GetEncoding(); } set { SetEncoding(value); } }

            public string Charset { get { return GetCharset(); } set { SetCharset(value); } }

            [XmlElement("Header")]
            public List<string> Headers = new List<string>();

            #endregion

            #region Private

            private string _Body;

            private byte[] _Data;

            private System.Text.Encoding _Encoding;

            private string _Charset;

            #endregion

            #region Method

            private string GetBody()
            {
                if (_Body != null)
                {
                    return _Body;
                }
                if (_Data == null)
                {
                    return null;
                }
                System.Text.Encoding encoding = _Encoding;
                if (encoding == null)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                string result = encoding.GetString(_Data, 0, _Data.Length);
                return result;
            }

            private void SetBody(string value)
            {
                _Body = value;
                _Data = null;
            }

            private byte[] GetData()
            {
                if (_Data != null)
                    return _Data;
                if (_Body == null)
                    return null;
                System.Text.Encoding encoding = GetEncodingOrDefault();
                return encoding.GetBytes(_Body);
            }

            private void SetData(byte[] value)
            {
                _Data = value;
                _Body = null;
            }

            private Encoding GetEncoding()
            {
                if (_Encoding != null)
                    return _Encoding;
                if (_Charset == null)
                    return null;
                return Energy.Base.Text.Encoding(_Charset);
            }

            private Encoding GetEncodingOrDefault()
            {
                System.Text.Encoding encoding = GetEncoding();
                if (encoding == null)
                    encoding = System.Text.Encoding.UTF8;
                return encoding;
            }

            private void SetEncoding(Encoding value)
            {
                _Encoding = value;
                _Charset = null;
            }

            private string GetCharset()
            {
                if (_Charset != null)
                {
                    return _Charset;
                }
                if (_Encoding == null)
                {
                    return null;
                }
                //return _Encoding.EncodingName;
                return _Encoding.WebName; // will return "utf-8" instead of "Unicode (UTF-8)"
            }

            private void SetCharset(string value)
            {
                _Charset = value;
                _Encoding = null;
            }

            public override string ToString()
            {
                return GetBody();
            }

            public void SetValue(object o)
            {
                if (o == null)
                {
                    _Body = null;
                    _Data = null;
                    return;
                }
                if (o is string)
                    Body = (string)o;
                else if (o is byte[])
                    Data = (byte[])o;
                else
                    Body = o.ToString();
            }

            public static string GetBody(Message message)
            {
                return message == null ? null : message.Body;
            }

            public static string[] GetHeaders(Message message, string[] emptyResult)
            {
                return message == null || message.Headers == null ? emptyResult : message.Headers.ToArray();
            }

            public static string[] GetHeaders(Message message)
            {
                return GetHeaders(message, null);
            }

            #endregion
        }

        /// <summary>
        /// HTTP request
        /// </summary>
        public class Request : Message
        {
            #region Public

            //public WebRequest RequestObject { get; set; }

            [DefaultValue(null)]
            public string Method { get; set; }

            [DefaultValue(null)]
            public string Url { get; set; }

            [DefaultValue(null)]
            public string ContentType { get; set; }

            [DefaultValue(null)]
            public string AcceptType { get; set; }

            //[XmlElement("Header")]
            //public List<string> Headers = new List<string>();
            
            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            public Request()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="method"></param>
            public Request(string method)
            {
                Method = method;
            }

            ///// <summary>
            ///// Constructor
            ///// </summary>
            ///// <param name="requestObject"></param>
            //public Request(WebRequest requestObject)
            //{
            //    RequestObject = requestObject;
            //}

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="method"></param>
            /// <param name="url"></param>
            public Request(string method, string url)
            {
                Method = method;
                Url = url;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="method"></param>
            /// <param name="url"></param>
            /// <param name="contentType"></param>
            public Request(string method, string url, string contentType)
            {
                Method = method;
                Url = url;
                ContentType = contentType;
            }

            #endregion

            #region Private

            #endregion

            #region Method
            
            #endregion
        }

        /// <summary>
        /// HTTP response
        /// </summary>
        public class Response : Message
        {
            #region Public
     
            //public WebResponse ResponseObject { get; set; }

            [DefaultValue((int)0)]
            public int StatusCode { get; set; }

            //[XmlElement("Header")]
            //public List<string> Headers = new List<string>();

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            public Response()
            {
            }

            public static int GetStatusCode(Response response)
            {
                return response == null ? -1 : response.StatusCode;
            }

            #endregion

            #region Method

            #endregion
        }
    }
}
