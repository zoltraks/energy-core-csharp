using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// ODBC style connection string.
    /// </summary>
    [Serializable]
    public class ConnectionString : Dictionary<string, string>
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public new string this[string key]
        {
            get
            {
                if (this.ContainsKey(key))
                {
                    return base[key];
                }
                foreach (string _ in this.Keys)
                {
                    if (String.Compare(_, key, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    {
                        return base[_];
                    }
                }
                return null;
            }
            set
            {
                if (this.ContainsKey(key))
                {
                    base[key] = value;
                    return;
                }
                foreach (string _ in this.Keys)
                {
                    if (String.Compare(_, key, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    {
                        base.Remove(_);
                        break;
                    }
                }
                base[key] = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ConnectionString(string connectionString)
        {
            Regex regex = new Regex(Energy.Base.Expression.ConnectionString);
            Match match = regex.Match(connectionString);
            while (true)
            {
                if (!match.Success) break;
                string key = Unquote(match.Groups["key"].Value);
                string value = Unquote(match.Groups["value"].Value);
                this[key] = value;
                match = match.NextMatch();
            }
        }

        /// <summary>
        /// Represent as string
        /// </summary>
        /// <returns>Connection string</returns>
        public override string ToString()
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            foreach (KeyValuePair<string, string> item in this)
            {
                list.Add(Escape(item.Key) + "=" + Quote(item.Value));
            }
            return String.Join(";", list.ToArray());
        }

        /// <summary>
        /// Implicit string operator
        /// </summary>
        /// <param name="value">Connection string</param>
        public static implicit operator ConnectionString(string value)
        {
            return new ConnectionString(value);
        }

        /// <summary>
        /// Escape connection string option name if needed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Escape(string key)
        {
            if (key.Contains("\""))
            {
                return "'" + key.Replace("'", "''") + "'";
            }
            if (key.Contains("'"))
            {
                return "\"" + key.Replace("\"", "\"\"") + "\"";
            }
            if (key.Contains(";"))
            {
                return "{" + key + "}";
            }
            return key;
        }

        /// <summary>
        /// Quote connection string option value if needed
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Quote(string value)
        {
            if (value.Contains(" ") || value.Contains(";") || value.Contains("'") || value.Contains("\""))
            {
                if (value.Contains("'"))
                {
                    return "\"" + value.Replace("\"", "\"\"") + "\"";
                }
                if (value.Contains("\""))
                {
                    return "'" + value.Replace("'", "''") + "'";
                }
            }
            return value;
        }

        /// <summary>
        /// Strip quotes from option name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Unquote(string value)
        {
            if (value.StartsWith("{") && value.EndsWith("}"))
            {
                return value.Substring(1, value.Length - 2);
            }
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
            }
            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                return value.Substring(1, value.Length - 2).Replace("''", "'");
            }
            return value;
        }

        /// <summary>Catalog</summary>
        public string Catalog { get { return GetCatalog(); } set { SetCatalog(value); } }

        /// <summary>Server</summary>
        public string Server { get { return GetServer(); } set { SetServer(value); } }

        private readonly string[] _ServerAlternatives = new string[]
        {
                "Data Source", "Server", "DataSource", "Server Name", "Dbq",
        };

        private readonly string[] _CatalogAlternatives = new string[]
        {
                "Database", "Database Name", "Initial Catalog",
        };

        private string GetServer()
        {
            return FindValue(_ServerAlternatives);
        }

        private void SetServer(string value)
        {
            string key = FindKey(_ServerAlternatives);
            if (key == null)
                key = _ServerAlternatives[0];
            this[key] = value;
        }

        private string GetCatalog()
        {
            return FindValue(_CatalogAlternatives);
        }

        private void SetCatalog(string value)
        {
            string key = FindKey(_CatalogAlternatives);
            if (key == null)
                key = _CatalogAlternatives[0];
            this[key] = value;
        }

        private string _Protocol;
        /// <summary>Protocol</summary>
        public string Protocol { get { return _Protocol; } set { _Protocol = value; } }

        #region FindKey

        public string FindKey(string name)
        {
            List<string> search = new List<string>(this.Keys);
            foreach (string needle in search)
            {
                if (0 == string.Compare(needle, name, true))
                {
                    return needle;
                }
            }
            return null;
        }

        public string FindKey(params string[] names)
        {
            List<string> search = new List<string>(this.Keys);
            foreach (string name in names)
            {
                foreach (string needle in search)
                {
                    if (0 == string.Compare(needle, name, true))
                    {
                        return needle;
                    }
                }
            }
            return null;
        }

        #endregion

        #region FindValue

        public string FindValue(string name)
        {
            List<string> search = new List<string>(this.Keys);
            foreach (string needle in search)
            {
                if (0 == string.Compare(needle, name, true))
                {
                    return this[needle];
                }
            }
            return null;
        }

        public string FindValue(params string[] names)
        {
            List<string> search = new List<string>(this.Keys);
            foreach (string name in names)
            {
                foreach (string needle in search)
                {
                    if (0 == string.Compare(needle, name, true))
                    {
                        return this[needle];
                    }
                }
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Represent ODBC Connection String as DSN.
        /// </summary>
        /// <returns></returns>
        public string ToDsnString()
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(_Protocol))
            {
                list.Add(_Protocol);
                list.Add(":/");
            }
            string server = this.Server;
            if (!string.IsNullOrEmpty(server))
            {
                list.Add(server);
            }
            string catalog = this.Catalog;
            if (!string.IsNullOrEmpty(catalog))
            {
                list.Add(catalog);
            }
            string dsn = string.Join("/", list.ToArray());
            return dsn;
        }
    }
}
