using System;
using System.Collections.Generic;
using System.Text;
using Energy.Enumeration;

namespace Energy.Query
{
    /// <summary>
    /// Query language dialect settings
    /// </summary>
    public class Dialect
    {
        public Query.Format Format { get; set; }

        public Dialect(Energy.Enumeration.SqlDialect dialect)
        {
            this.Format = (Format)dialect;
        }

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
