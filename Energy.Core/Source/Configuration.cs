using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Energy.Source
{
    /// <summary>
    /// Data source configuration class
    /// </summary>
    public class Configuration
    {     
        /// <summary>
        /// Connection string database dialect type.
        /// This value is used for making connection string for any SQL server in exact form recognized by server of that dialect.
        /// </summary>
        public Energy.Enumeration.SqlDialect Dialect { get; set;  }

        /// <summary>
        /// Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        [DefaultValue(0)]
        public int Port { get; set; }

        /// <summary>
        /// Catalog
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Encryption
        /// </summary>
        public bool Encryption { get; set; }

        /// <summary>
        /// Compression
        /// </summary>
        public bool Compression { get; set; }

        /// <summary>
        /// Timeout
        /// </summary>
        [DefaultValue(0)]
        public int Timeout { get; set; }

        /// <summary>
        /// Charset
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Connection string property
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return GetConnectionString();
            }
        }

        /// <summary>
        /// Represent configuration as connection string.
        /// </summary>
        /// <returns></returns>
        public virtual string GetConnectionString()
        {
            string[] special = new string[] { ";" };
            List<string> list = new List<string>();

            if (Dialect == Energy.Enumeration.SqlDialect.SqlServer)
            {
                //Data Source=.\SQLEXPRESS;Initial Catalog=BisSQL;Integrated Security=Yes;Connect Timeout=10;
                //Data Source=.\SQLEXPRESS;Initial Catalog=BisSQL;Integrated Security=False;User ID=abc;Password=xyz;Connect Timeout=10
                string server = String.IsNullOrEmpty(Server) ? "." : Server;
                string port = Port > 0 ? ',' + Port.ToString() : "";
                list.Add(String.Concat("Data Source=", server, port));
                if (!String.IsNullOrEmpty(Catalog))
                {
                    list.Add("Initial Catalog=" + Energy.Base.Text.Surround(Catalog, "\"", special));
                }
                if (!String.IsNullOrEmpty(User))
                {
                    list.Add("User ID=" + Energy.Base.Text.Surround(User, "\"", special));
                    if (!String.IsNullOrEmpty(Password))
                    {
                        list.Add("Password=" + Energy.Base.Text.Surround(Password, "\"", special));
                    }
                }
                else
                {
                    list.Add("Integrated Security=Yes");
                }
                if (Timeout > 0)
                {
                    list.Add("Connect Timeout=" + Timeout.ToString());
                }
            }

            if (this.Dialect == Energy.Enumeration.SqlDialect.MySQL)
            {
                string server = String.IsNullOrEmpty(Server) ? "localhost" : Server;
                list.Add("Server=" + server);
                if (Port > 0) list.Add("Port=" + Port.ToString());
                if (!String.IsNullOrEmpty(Catalog)) list.Add("Database=" + Catalog);
                if (!String.IsNullOrEmpty(User)) list.Add("Uid=" + User);
                if (!String.IsNullOrEmpty(Password)) list.Add("Pwd=" + Password);
                if (Encryption) list.Add("Encrypt=true");
            }

            return String.Join(";", list.ToArray());
        }

        /// <summary>
        /// Connection string for Entity Framework
        /// </summary>
        public string GetEntityConnectionString(string provider)
        {
            return "metadata=res://*;provider=" + provider + ";provider connection string=\"" + GetConnectionString() + "\"";            
        }

        /// <summary>
        /// Get prefix for DSN in PHP/Perl style
        /// </summary>
        /// <returns></returns>
        public virtual string GetDSNPrefix()
        {
            switch (Dialect)
            {
                case Energy.Enumeration.SqlDialect.MySQL:
                    return "mysql";
                case Energy.Enumeration.SqlDialect.SqlServer:
                    return "sqlsrv";
                case Energy.Enumeration.SqlDialect.PostgreSQL:
                    return "pg";
                case Energy.Enumeration.SqlDialect.Oracle:
                    return "oracle";
                case Energy.Enumeration.SqlDialect.SQLite:
                    return "sqlite";
                case Energy.Enumeration.SqlDialect.Firebird:
                    return "firebird";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string prefix = GetDSNPrefix();

            List<string> list = new List<string>();
            string[] special = new string[] { ";", "\"" };

            if (Dialect == Energy.Enumeration.SqlDialect.SqlServer)
            {
                string server = String.IsNullOrEmpty(Server) ? "." : Energy.Base.Text.Surround(Server, "'", special);
                string port = Port > 0 ? ',' + Port.ToString() : "";
                list.Add(String.Concat("Server=", server, port));

                if (!String.IsNullOrEmpty(Catalog))
                {
                    list.Add("Database=" + Energy.Base.Text.Surround(Catalog, "'", special));
                }
            }

            if (Dialect == Enumeration.SqlDialect.MySQL)
            {
                list.Add("host=" + (String.IsNullOrEmpty(Server) ? "localhost" : Energy.Base.Text.Surround(Server, "'", special)));
                if (Port > 0)
                    list.Add("port=" + Port.ToString());
                if (!String.IsNullOrEmpty(Catalog))
                    list.Add("dbname=" + Energy.Base.Text.Surround(Catalog, "'", special));
                if (!String.IsNullOrEmpty(Charset))
                    list.Add("charset=" + Charset);
            }

            if (list.Count > 0)
            {
                string text = string.Join(";", list.ToArray());
                if (!string.IsNullOrEmpty(prefix))
                    text = string.Concat(prefix, ":", text);
                return text;
            }

            return "";
        }

        private static readonly string[] hostArray = new string[] { "Data Source", "Host", "Server" };
    }
}
