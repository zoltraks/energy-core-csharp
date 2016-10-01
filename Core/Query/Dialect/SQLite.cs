using System;
using System.Collections.Generic;
using System.Text;
using Energy.Base;

namespace Energy.Query.Dialect
{
    public class SQLite : Energy.Interface.IQueryDialect
    {
        public string CreateColumn(Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string CreateIndex(Structure.Index index)
        {
            throw new NotImplementedException();
        }

        public string CreateTable(Structure.Table table)
        {
            throw new NotImplementedException();
        }

        public string DeleteColumn(Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string DeleteIndex(Structure.Index index)
        {
            throw new NotImplementedException();
        }

        public string DeleteTable(Structure.Table table)
        {
            throw new NotImplementedException();
        }

        public string Select(Select select)
        {
            throw new NotImplementedException();
        }
    }
}
