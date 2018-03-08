using System;
using System.Collections.Generic;
using System.Text;
using Energy.Base;

namespace Energy.Query
{
    public class Insert: Energy.Base.Query.Statement
    {
        private string _Table;

        public Insert Table(string table)
        {
            _Table = table;
            return this;
        }

        public Insert Value(string column, object value)
        {
            return this;
        }

        public Insert Values(params object[] columnValuePairs)
        {
            return this;
        }

        public override string ToString(Base.Query.Dialect dialect)
        {
            return dialect.Insert(this);
        }
    }
}
