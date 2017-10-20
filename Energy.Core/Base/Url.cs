using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    public class Url
    {
        /// <summary>
        /// Protocol
        /// </summary>
        public string Protocol;
        /// <summary>
        /// Server name
        /// </summary>
        public string Host;
        /// <summary>
        /// Port number
        /// </summary>
        [DefaultValue(0)]
        public int Port;
        /// <summary>
        /// Path
        /// </summary>
        public string Path;
        /// <summary>
        /// Query
        /// </summary>
        public string Query;

        /// <summary>
        /// Represent URL structure as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = Host;
            if (Port > 0)
            {
                if (String.IsNullOrEmpty(result))
                    result = "localhost";
                result += ":" + Port.ToString();
            }
            if (!String.IsNullOrEmpty(Path))
            {
                if (!String.IsNullOrEmpty(result) && !Path.StartsWith("/"))
                    result += "/";
                result += Path;
            }
            if (!String.IsNullOrEmpty(Query))
            {
                result += "?" + Query;
            }
            if (!String.IsNullOrEmpty(Protocol))
                result = Protocol + "://" + result;
            return result;
        }

        /// <summary>
        /// Create Url structure object from text
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Url Explode(string url)
        {
            string pattern = Energy.Base.Expression.Url;
            Regex r = new Regex(pattern);
            Match m = r.Match(url);
            if (!m.Success)
                return null;
            Energy.Base.Url result = new Energy.Base.Url();
            result.Protocol = m.Groups["protocol"].Value;
            result.Host = m.Groups["host"].Value;
            result.Port = Energy.Base.Cast.AsInteger(m.Groups["port"].Value);
            result.Path = m.Groups["path"].Value;
            result.Query = m.Groups["query"].Value;
            return result;
        }

        /// <summary>
        /// Create object from string representation
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static implicit operator Url(string url)
        {
            return Explode(url);
        }

        /// <summary>
        /// Set host in URL text
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string SetHost(string url, string host)
        {
            Energy.Base.Url x = url;
            x.Host = host;
            return x.ToString();
        }

        /// <summary>
        /// Set port in URL text
        /// </summary>
        /// <param name="url"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string SetPort(string url, int port)
        {
            Energy.Base.Url x = url;
            x.Port = port;
            return x.ToString();
        }

        /// <summary>
        /// Set host and port in URL text
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string SetHostAndPort(string url, string host, int port)
        {
            Energy.Base.Url x = url;
            x.Host = host;
            x.Port = port;
            return x.ToString();
        }
    }
}
