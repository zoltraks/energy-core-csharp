using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Class used to represent URL (Uniform Resource Locator).
    /// </summary>
    public class Url: Energy.Interface.ICopy<Url>
    {
        #region Field

        /// <summary>
        /// Scheme (protocol).
        /// Examples of popular schemes include http, https, ftp, mailto, file, data, and irc.
        /// </summary>
        public string Scheme;

        /// <summary>
        /// Optional slashes which may be not present, i.e. mailto:user@host.
        /// </summary>
        public string Slash;

        /// <summary>
        /// User name.
        /// </summary>
        public string User;

        /// <summary>
        /// Optional password.
        /// </summary>
        public string Password;

        /// <summary>
        /// Host address.
        /// </summary>
        public string Host;

        /// <summary>
        /// Port number or empty string if not specified.
        /// </summary>
        public string Port;

        /// <summary>
        /// Path component.
        /// </summary>
        public string Path;

        /// <summary>
        /// Query string.
        /// </summary>
        public string Query;

        /// <summary>
        /// Fragment component.
        /// </summary>
        public string Fragment;

        #endregion

        #region Property

        #region IsEmpty

        /// <summary>
        /// Returns true if URL is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return true
                    && string.IsNullOrEmpty(this.Scheme)
                    && string.IsNullOrEmpty(this.Slash)
                    && string.IsNullOrEmpty(this.Host)
                    && string.IsNullOrEmpty(this.Port)
                    && string.IsNullOrEmpty(this.Path)
                    && string.IsNullOrEmpty(this.Query)
                    && string.IsNullOrEmpty(this.Fragment)
                    && string.IsNullOrEmpty(this.User)
                    && string.IsNullOrEmpty(this.Password)
                    ;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Url()
        {
            this.Scheme = "";
            this.Slash = "";
            this.User = "";
            this.Password = "";
            this.Host = "";
            this.Port = "";
            this.Path = "";
            this.Query = "";
            this.Fragment = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url"></param>
        public Url(string url)
        {
            Url o = Explode(url);
            this.Scheme = o.Scheme;
            this.Slash = o.Slash;
            this.User = o.User;
            this.Password = o.Password;
            this.Host = o.Host;
            this.Port = o.Port;
            this.Path = o.Path;
            this.Query = o.Query;
            this.Fragment = o.Fragment;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Represent URL object as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string scheme = this.Scheme;
            string slash = this.Slash;
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
                list.Add(":");
            }
            if (!string.IsNullOrEmpty(slash))
            {
                list.Add(slash);
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
                if (!path.StartsWith("/"))
                {
                    list.Add("/");
                }
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

        #endregion

        #region Overwrite

        /// <summary>
        /// Combine with another URL, overwriting all or only empty values.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="overwrite">When overwrite is true, not null parameters from second object will always be overwritten. If not, only null and empty values will be overwritten.</param>
        /// <returns></returns>
        public Url Overwrite(Url url, bool overwrite)
        {
            return Overwrite(this, url, overwrite);
        }

        /// <summary>
        /// Combine with another URL, overwriting empty values.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Url Overwrite(Url url)
        {
            return Overwrite(this, url, false);
        }

        #endregion

        #region Copy

        /// <summary>
        /// Make a copy of object.
        /// </summary>
        /// <returns></returns>
        public Url Copy()
        {
            Url o = new Url()
            {
                Scheme = this.Scheme,
                Slash = this.Slash,
                Host = this.Host,
                Port = this.Port,
                Path = this.Path,
                Query = this.Query,
                Fragment = this.Fragment,
                User = this.User,
                Password = this.Password,
            };
            return o;
        }

        #endregion

        #region Implicit

        /// <summary>
        /// Create URL object from string representation.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static implicit operator Url(string url)
        {
            return Explode(url);
        }

        #endregion

        #region Static

        #region Explode

        /// <summary>
        /// Create URL object from string.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Url Explode(string url)
        {
            if (null == url)
            {
                return null;
            }
            else if (0 == url.Length)
            {
                return new Energy.Base.Url();
            }
            string pattern = Energy.Base.Expression.Url;
            Regex r = new Regex(pattern);
            Match m = r.Match(url);
            if (!m.Success)
            {
                return null;
            }
            Energy.Base.Url result = new Energy.Base.Url();
            result.Scheme = m.Groups["scheme"].Value;
            result.Slash = m.Groups["slash"].Value;
            result.Host = m.Groups["host"].Value;
            result.Port = m.Groups["port"].Value;
            result.Path = m.Groups["path"].Value;
            result.Query = m.Groups["query"].Value;
            result.Fragment = m.Groups["fragment"].Value;
            result.User = m.Groups["user"].Value;
            result.Password = m.Groups["password"].Value;
            return result;
        }

        #endregion

        #region Make

        /// <summary>
        /// Make URL address overriding parts of it. 
        /// Pass null as parameter to skip it or empty value to remove specified part from URL.
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
            {
                string pattern = @":([^\s]*)";
                Match match = Regex.Match(scheme, pattern);
                if (match.Success)
                {
                    x.Slash = match.Groups[1].Value;
                    x.Scheme = scheme.Substring(0, match.Index);
                }
                else
                {
                    x.Slash = "//";
                    x.Scheme = scheme;
                }
            }
            if (host != null)
            {
                x.Host = host;
            }
            if (port != null)
            {
                x.Port = port;
            }
            if (path != null)
            {
                x.Path = path;
            }
            if (query != null)
            {
                x.Query = query;
            }
            if (fragment != null)
            {
                x.Fragment = fragment;
            }
            if (user != null)
            {
                x.User = user;
            }
            if (password != null)
            {
                x.Password = password;
            }
            url = x.ToString();
            if (value != null && url.Contains("{0}"))
            {
                url = url.Replace("{0}", value);
            }
            return url;
        }

        /// <summary>
        /// Make URL address overriding parts of it. 
        /// Pass null as parameter to skip it or empty value to remove specified part from URL.
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
        public static string Make(string url, string scheme, string host, int port
            , string path, string query, string fragment, string user, string password
            , string value)
        {
            return Make(url, scheme, host
                , port > 0 && port <= 65535 ? port.ToString() : null
                , path, query, fragment
                , user, password
                , value);
        }

        /// <summary>
        /// Make URL address overriding parts of it. 
        /// Pass null as parameter to skip it or empty value to remove specified part from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <param name="value">Optionally replace placeholder {0} as value</param>
        /// <returns></returns>
        public static string Make(string url, string scheme, string host, string port
            , string path, string query, string fragment
            , string value)
        {
            return Make(url, scheme, host, port
                , path, query, fragment
                , null, null
                , value);
        }

        /// <summary>
        /// Make URL address overriding parts of it. 
        /// Pass null as parameter to skip it or empty value to remove specified part from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <param name="value">Optionally replace placeholder {0} as value</param>
        /// <returns></returns>
        public static string Make(string url, string scheme, string host, int port
            , string path, string query, string fragment
            , string value)
        {
            return Make(url, scheme, host
                , port > 0 && port <= 65535 ? port.ToString() : null
                , path, query, fragment
                , null, null
                , value);
        }

        /// <summary>
        /// Make URL address overriding parts of it. 
        /// Pass null as parameter to skip it or empty value to remove specified part from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static string Make(string url, string scheme, string host, string port
            , string path, string query, string fragment)
        {
            return Make(url, scheme, host, port
                , path, query, fragment
                , null, null, null);
        }

        /// <summary>
        /// Make URL address overriding parts of it. 
        /// Pass null as parameter to skip it or empty value to remove specified part from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static string Make(string url, string scheme, string host, int port
            , string path, string query, string fragment)
        {
            return Make(url, scheme, host
                , port > 0 && port <= 65535 ? port.ToString() : null
                , path, query, fragment
                , null, null, null);
        }

        #endregion

        #region SetHost

        /// <summary>
        /// Set host name or address in URL.
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

        #endregion

        #region SetPort

        /// <summary>
        /// Set port number in URL.
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
        /// Set port number in URL.
        /// When port number is not in range 1 .. 65535 it will be considered undefined and will be removed.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string SetPort(string url, int port)
        {
            Energy.Base.Url x = url;
            x.Port = port > 0 && port <= 65535 ? port.ToString() : "";
            return x.ToString();
        }

        #endregion

        #region SetHostAndPort

        /// <summary>
        /// Set host name or address and port number in URL.
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
        /// Set host name or address and port number in URL.
        /// When port number is not in range 1 .. 65535 it will be considered undefined and will be removed.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string SetHostAndPort(string url, string host, int port)
        {
            Energy.Base.Url x = url;
            x.Host = host;
            x.Port = port > 0 && port <= 65535 ? port.ToString() : "";
            return x.ToString();
        }

        #endregion

        #region Overwrite

        /// <summary>
        /// Combine two URL objects, overwriting all or only empty parts from second one.
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <param name="all">
        /// When all is true, values will always be overwritten with not empty parameters from second address.
        /// Otherwise, only empty values will be overwritten.
        /// </param>
        /// <returns></returns>
        public static Url Overwrite(Url url1, Url url2, bool all)
        {
            Url url0 = url1.Copy();
            if (all)
            {
                if (!string.IsNullOrEmpty(url2.Scheme))
                {
                    url0.Scheme = url2.Scheme;
                }
                if (!string.IsNullOrEmpty(url2.Slash))
                {
                    url0.Slash = url2.Slash;
                }
                if (!string.IsNullOrEmpty(url2.Host))
                {
                    url0.Host = url2.Host;
                }
                if (!string.IsNullOrEmpty(url2.Port))
                {
                    url0.Port = url2.Port;
                }
                if (!string.IsNullOrEmpty(url2.Path))
                {
                    url0.Path = url2.Path;
                }
                if (!string.IsNullOrEmpty(url2.Query))
                {
                    url0.Query = url2.Query;
                }
                if (!string.IsNullOrEmpty(url2.Fragment))
                {
                    url0.Fragment = url2.Fragment;
                }
                if (!string.IsNullOrEmpty(url2.User))
                {
                    url0.User = url2.User;
                }
                if (!string.IsNullOrEmpty(url2.Password))
                {
                    url0.Password = url2.Password;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(url0.Scheme) && url2.Scheme != null)
                {
                    url0.Scheme = url2.Scheme;
                }
                if (string.IsNullOrEmpty(url0.Host) && url2.Host != null)
                {
                    url0.Host = url2.Host;
                }
                if (string.IsNullOrEmpty(url0.Port) && url2.Port != null)
                {
                    url0.Port = url2.Port;
                }
                if (string.IsNullOrEmpty(url0.Path) && url2.Path != null)
                {
                    url0.Path = url2.Path;
                }
                if (string.IsNullOrEmpty(url0.Query) && url2.Query != null)
                {
                    url0.Query = url2.Query;
                }
                if (string.IsNullOrEmpty(url0.Fragment) && url2.Fragment != null)
                {
                    url0.Fragment = url2.Fragment;
                }
                if (string.IsNullOrEmpty(url0.User) && url2.User != null)
                {
                    url0.User = url2.User;
                }
                if (string.IsNullOrEmpty(url0.Password) && url2.Password != null)
                {
                    url0.Password = url2.Password;
                }
            }
            return url0;
        }

        /// <summary>
        /// Combine two URL objects, overwriting empty values in first one with values from second one.
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public static Url Overwrite(Url url1, Url url2)
        {
            return Overwrite(url1, url2, false);
        }

        #endregion

        #region Combine

        /// <summary>
        /// Combine parts of URL together.
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string Combine(params string[] parts)
        {
            if (null == parts)
            {
                return null;
            }
            if (0 == parts.Length)
            {
                return "";
            }
            List<string> list = new List<string>();
            string q = "/";
            int l = parts.Length;
            string p = null;
            for (int i = 0; i < l; i++)
            {
                if (string.IsNullOrEmpty(parts[i]))
                {
                    continue;
                }
                if (null != p)
                {
                    if ("" != q)
                    {
                        if (!p.EndsWith(q) && !parts[i].StartsWith(q))
                        {
                            list.Add(q);
                        }
                    }
                }
                p = parts[i];
                list.Add(p);
                if ("/" == q)
                {
                    if (0 <= p.IndexOf('#'))
                    {
                        q = "";
                    }
                    else if (0 <= p.IndexOf('?'))
                    {
                        q = "&";
                    }
                }
                else if ("&" == q)
                {
                    if (0 <= p.IndexOf('#'))
                    {
                        q = "";
                    }
                }
            }
            return string.Join("", list.ToArray());
        }

        #endregion

        #endregion
    }
}
