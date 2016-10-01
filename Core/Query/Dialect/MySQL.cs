using System;
using System.Collections.Generic;
using System.Text;
using Energy.Base;

namespace Energy.Query.Dialect
{
    /// <summary>
    /// MySQL
    /// </summary>
    public class MySQL: Script, Energy.Interface.IQueryDialect
    {
        public MySQL()
        {
            Format.LiteralQuote = "'";
            Format.ObjectQuote = "`";
        }

        public string CreateColumn(Structure.Column column)
        {
            return "";
        }

        public string CreateIndex(Structure.Index index)
        {
            return "";
        }

        public string CreateTable(Structure.Table table)
        {
            return "";
        }

        public string DeleteColumn(Structure.Column column)
        {
            return "";
        }

        public string DeleteIndex(Structure.Index index)
        {
            return "";
        }

        public string DeleteTable(Structure.Table table)
        {
            return "";
        }

        public string Select(Select select)
        {
            throw new NotImplementedException();
        }
    }
}
