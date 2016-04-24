using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query.Dialect
{
    /// <summary>
    /// MySQL
    /// </summary>
    public class MySQL
    {
        /// <summary>
        /// Create
        /// </summary>
        public class Create
        {
            /// <summary>
            /// Create table
            /// </summary>
            /// <param name="table"></param>
            /// <param name="configuration"></param>
            /// <returns></returns>
            public static string Table(Energy.Base.Structure.Table table, Energy.Query.Configuration configuration = null)
            {
                if (table == null || table.Columns.Count == 0) return "";
                if (configuration == null) configuration = Energy.Query.Configuration.Default;

                return "";
            }
        }
    }
}
