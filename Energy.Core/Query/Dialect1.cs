using System;
using System.Collections.Generic;
using System.Text;
using Energy.Enumeration;

namespace Energy.Query
{
    /// <summary>
    /// Query language dialect settings
    /// </summary>
    public partial class Dialect1
    {
        public Energy.Query.Format Format { get; set; }

        public System.Collections.Generic.List<string> Special = new List<string>();

        #region Constructor

        public Dialect1(Energy.Enumeration.SqlDialect dialect)
        {
            this.Format = (Energy.Query.Format)dialect;
        }

        public Dialect1()
        {
            this.Format = (Energy.Query.Format)Energy.Enumeration.SqlDialect.ANSI;
        }

        #endregion

        #region Singleton

        private static Energy.Query.Dialect1 _Default;
        private static readonly object _DefaultLock = new object();
        /// <summary>Singleton</summary>
        public static Energy.Query.Dialect1 Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_DefaultLock)
                    {
                        if (_Default == null)
                        {
                            _Default = new Energy.Query.Dialect1();
                        }
                    }
                }
                return _Default;
            }
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="full"></param>
        /// <returns></returns>
        public static Energy.Enumeration.SqlDialect Guess(string name, string full)
        {
            if (!string.IsNullOrEmpty(full))
            {
                if (full == "System.Data.SqlClient.SqlConnection")
                    return Energy.Enumeration.SqlDialect.SQLSERVER;
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (name == "SqlConnection")
                    return Energy.Enumeration.SqlDialect.SQLSERVER;
            }
            return Energy.Enumeration.SqlDialect.ANSI;
        }

        public static Energy.Enumeration.SqlDialect Guess(string name)
        {
            return Guess(name, null);
        }

        public static explicit operator Dialect1(SqlDialect dialect)
        {
            return new Energy.Query.Dialect1(dialect);
        }
    }
}
