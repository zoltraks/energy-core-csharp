using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Inspired by classics, a class representing a connection string to a data source.
    /// Simply, ODBC style connection string.
    /// </summary>
    [Serializable]
    public class ConnectionString : Dictionary<string, string>
    {
        #region Accessor

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// Case insensitive.
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

        #endregion

        #region Constructor

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

        #endregion

        #region Implicit

        /// <summary>
        /// Implicit string operator
        /// </summary>
        /// <param name="value">Connection string</param>
        public static implicit operator ConnectionString(string value)
        {
            return new ConnectionString(value);
        }

        #endregion

        #region Property

        /// <summary>
        /// Catalog name taken from one of alternatives:
        /// "Database", "Database Name", "Initial Catalog".
        /// </summary>
        public string Catalog { get { return GetCatalog(); } set { SetCatalog(value); } }

        /// <summary>
        /// Server name taken from one of alernatives:
        /// "Data Source", "Server", "DataSource", "Server Name", "Dbq".
        /// </summary>
        public string Server { get { return GetServer(); } set { SetServer(value); } }

        /// <summary>
        /// User name taken from one of alernatives:
        /// "User", "User ID".
        /// </summary>
        public string User { get { return GetUser(); } set { SetUser(value); } }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get { return GetPassword(); } set { SetPassword(value); } }

        private string _Protocol;
        /// <summary>Protocol</summary>
        public string Protocol { get { return _Protocol; } set { _Protocol = value; } }

        #endregion

        #region Private

        private readonly string[] _ServerAlternatives = new string[]
        {
                "Data Source", "Server", "DataSource", "Server Name", "Dbq",
        };

        private readonly string[] _CatalogAlternatives = new string[]
        {
                "Database", "Database Name", "Initial Catalog",
        };

        private readonly string[] _UserAlternatives = new string[]
        {
                "User", "User ID",
        };

        private readonly string[] _PasswordAlternatives = new string[]
        {
                "Password",
        };

        private string GetUser()
        {
            return FindValue(_UserAlternatives);
        }

        private void SetUser(string value)
        {
            string key = FindKey(_UserAlternatives);
            if (key == null)
                key = _UserAlternatives[0];
            this[key] = value;
        }

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

        private string GetPassword()
        {
            return FindValue(_PasswordAlternatives);
        }

        private void SetPassword(string value)
        {
            string key = FindKey(_PasswordAlternatives);
            if (key == null)
                key = _PasswordAlternatives[0];
            this[key] = value;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Represent as string.
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

        #endregion

        #region ToDsnString

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

        #endregion

        #region FindKey

        /// <summary>
        /// Find key by name. 
        /// Return null if not found.
        /// Search is case insensitive.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find key by any name. 
        /// Return null if not found.
        /// Search is case insensitive.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
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

        #region Escape

        /// <summary>
        /// Escape connection string value if needed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Escape(string key)
        {
            if (Energy.Base.Text.Contains(key, "\""))
            {
                return "'" + key.Replace("'", "''") + "'";
            }
            if (Energy.Base.Text.Contains(key, "'"))
            {
                return "\"" + key.Replace("\"", "\"\"") + "\"";
            }
            if (Energy.Base.Text.Contains(key, ";"))
            {
                return "{" + key + "}";
            }
            return key;
        }

        #endregion

        #region Quote

        /// <summary>
        /// Quote connection string value if needed.
        /// <br /><br />
        /// This method will affect on values containing space, semicolon, apostrophe or quotation mark.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Quote(string value)
        {
            if (Energy.Base.Text.Contains(value, " ") || Energy.Base.Text.Contains(value, ";")
                || Energy.Base.Text.Contains(value, "'") || Energy.Base.Text.Contains(value, "\""))
            {
                if (Energy.Base.Text.Contains(value, "'"))
                {
                    return "\"" + value.Replace("\"", "\"\"") + "\"";
                }
                if (Energy.Base.Text.Contains(value, "\""))
                {
                    return "'" + value.Replace("'", "''") + "'";
                }
            }
            return value;
        }

        #endregion

        #region Unquote

        /// <summary>
        /// Strip quotes from connection string value.
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

        #endregion

        #region Safe

        /// <summary>
        /// Remove password from connection string to make it more safe to display.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string Safe(string connectionString, string replaceWith)
        {
            Energy.Base.ConnectionString _ = connectionString;
            if (null != _.Password)
            {
                _.Password = replaceWith;
            }
            return _.ToString();
        }

        /// <summary>
        /// Remove password from connection string to make it more safe to display.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string Safe(string connectionString)
        {
            return Safe(connectionString, "(***)");
        }

        #endregion
    }
}
