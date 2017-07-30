using System;
using System.Collections.Generic;
using System.Text;
using Energy.Source;

namespace Energy.Query.Layer
{
    public class SQLite
    {
        public class Create
        {
            public static string Table(Energy.Source.Structure.Table table, Energy.Query.Configuration configuration)
            {
                return "";
            }
        }

        internal string CreateTable(Structure.Table table)
        {
            throw new NotImplementedException();
        }
    }
}
