using System;
using System.Collections.Generic;
using System.Text;
using Energy.Enumeration;

namespace Energy.Query
{
    /// <summary>
    /// Query language dialect settings
    /// </summary>
    public partial class Dialect
    {
        public Energy.Query.Format Format { get; set; }

        public System.Collections.Generic.List<string> Special = new List<string>();

        #region Constructor

        public Dialect(Energy.Enumeration.SqlDialect dialect)
        {
            this.Format = (Energy.Query.Format)dialect;
        }

        public Dialect()
        {
            this.Format = (Energy.Query.Format)Energy.Enumeration.SqlDialect.Generic;
        }

        #endregion

        #region Singleton

        private static Energy.Query.Dialect _Default;
        private static readonly object _DefaultLock = new object();
        /// <summary>Singleton</summary>
        public static Energy.Query.Dialect Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_DefaultLock)
                    {
                        if (_Default == null)
                        {
                            _Default = new Energy.Query.Dialect();
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
                    return Energy.Enumeration.SqlDialect.SqlServer;
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (name == "SqlConnection")
                    return Energy.Enumeration.SqlDialect.SqlServer;
            }
            return Energy.Enumeration.SqlDialect.None;
        }

        public static Energy.Enumeration.SqlDialect Guess(string name)
        {
            return Guess(name, null);
        }

        public static explicit operator Dialect(SqlDialect dialect)
        {
            return new Energy.Query.Dialect(dialect);
        }
    }
}
