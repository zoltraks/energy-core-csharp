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
            public static string Table(Energy.Source.Structure.Table table, Energy.Query.Configuration configuration)
            {
                if (table == null || table.Columns.Count == 0) return "";
                if (configuration == null) configuration = Energy.Query.Configuration.Default;

                return "";
            }
        }
    }
}
