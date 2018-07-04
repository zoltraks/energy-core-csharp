using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    public class Url
    {
        /// <summary>
        /// Scheme
        /// </summary>
        public string Scheme;

        /// <summary>
        /// Host
        /// </summary>
        public string Host;

        /// <summary>
        /// Port
        /// </summary>
        public string Port;

        /// <summary>
        /// Path
        /// </summary>
        public string Path;

        /// <summary>
        /// Query
        /// </summary>
        public string Query;

        /// <summary>
        /// Fragment
        /// </summary>
        public string Fragment;

        /// <summary>
        /// User
        /// </summary>
        public string User;

        /// <summary>
        /// Password
        /// </summary>
        public string Password;

        /// <summary>
        /// Represent URL structure as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string scheme = this.Scheme;
            string user = this.User;
            string password = this.Password;
            string host = this.Host;
            string port = this.Port;
            string path = this.Path;
            string query = this.Query;
            string fragment = this.Fragment;

            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(scheme))
            {
                list.Add(scheme);
                list.Add("://");
            }
            if (!string.IsNullOrEmpty(user))
            {
                list.Add(user);
                if (!string.IsNullOrEmpty(password))
                {
                    list.Add(":");
                    list.Add(password);
                }
                list.Add("@");
            }
            if (!string.IsNullOrEmpty(host))
            {
                list.Add(host);
                if (!string.IsNullOrEmpty(port))
                {
                    list.Add(":");
                    list.Add(port);
                }
            }
            if (!string.IsNullOrEmpty(path))
            {
                list.Add(path);
            }
            if (!string.IsNullOrEmpty(query))
            {
                list.Add("?");
                list.Add(query);
            }
            if (!string.IsNullOrEmpty(fragment))
            {
                list.Add("#");
                list.Add(fragment);
            }

            string url = string.Join("", list.ToArray());
            return url;
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
            result.Scheme = m.Groups["scheme"].Value;
            result.Host = m.Groups["host"].Value;
            result.Port = m.Groups["port"].Value;
            result.Path = m.Groups["path"].Value;
            result.Query = m.Groups["query"].Value;
            result.Fragment = m.Groups["fragment"].Value;
            result.User = m.Groups["user"].Value;
            result.Password = m.Groups["user"].Value;
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
        public static string SetPort(string url, string port)
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
        public static string SetHostAndPort(string url, string host, string port)
        {
            Energy.Base.Url x = url;
            x.Host = host;
            x.Port = port;
            return x.ToString();
        }

        /// <summary>
        /// Make URL address overriding parts of it. Use null as parameter 
        /// to ignore or empty value to remove it.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="value">Optionally replace placeholder {0} as value</param>
        /// <returns></returns>
        public static string Make(string url, string scheme, string host, string port
            , string path, string query, string fragment, string user, string password
            , string value)
        {
            Energy.Base.Url x = url;
            if (scheme != null)
                x.Scheme = scheme;
            if (host != null)
                x.Host = host;
            if (port != null)
                x.Port = port;
            if (path != null)
                x.Path = path;
            if (query != null)
                x.Query = query;
            if (fragment != null)
                x.Fragment = fragment;
            if (user != null)
                x.User = user;
            if (password != null)
                x.Password = password;
            url = x.ToString();
            if (value != null && url.Contains("{0}"))
            {
                url = url.Replace("{0}", value);
            }
            return url;
        }
    }
}
