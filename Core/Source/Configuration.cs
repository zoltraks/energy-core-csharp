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
        /// Dialect
        /// </summary>
        public Energy.Base.Enumeration.SQL Dialect { get; set;  }


        private static readonly string[] hostArray = new string[] { "Data Source", "Host", "Server" };

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        [DefaultValue(0)]
        public int Port { get; set; }

        public string Catalog { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public bool Encrypt { get; set; }

        public bool Compression { get; set; }

        [DefaultValue(0)]
        public int Timeout { get; set; }

        public string Charset { get; set; }

        public virtual string GetConnectionString()
        {
            List<string> list = new List<string>();

            if (this.Dialect == Base.Enumeration.SQL.MySQL)
            {
                list.Add("Server=" + this.Host);
                if (Port > 0) list.Add("Port=" + Port.ToString());
                if (!String.IsNullOrEmpty(Catalog)) list.Add("Database=" + Catalog);
                if (!String.IsNullOrEmpty(User)) list.Add("Uid=" + User);
                if (!String.IsNullOrEmpty(Password)) list.Add("Pwd=" + Password);
                if (Encrypt) list.Add("Encrypt=true");
            }

            return String.Join(";", list.ToArray());
        }

        public virtual string GetURIDataSourceName()
        {
            List<string> list = new List<string>();

            string uri = "";
            switch (this.Dialect)
            {
                default:
                    uri = Dialect.ToString().ToLower();
                    break;
            }

            if (!String.IsNullOrEmpty(Host)) list.Add("host=" + Host);

            return uri + String.Join(";", list.ToArray());
            
        }

        /// <summary>
        /// Connection string for Entity Framework
        /// </summary>
        public string GetEntityConnectionString(string provider)
        {
            return "metadata=res://*;provider=" + provider + ";provider connection string=\"" + GetConnectionString() + "\"";            
        }
    }
}
