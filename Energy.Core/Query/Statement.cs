using System;
using System.Collections.Generic;
using System.Text;
using Energy.Base;

namespace Energy.Query
{
    public class Statement
    {
        #region Base

        public abstract class Base
        {
            private Energy.Interface.IDialect Dialect;

            public Base()
            {
            }

            public Base(Energy.Interface.IDialect dialect)
            {
                Dialect = dialect;
            }

            //public abstract string ToString(Energy.Base.Query.Dialect dialect);
        }

        #endregion

        #region Select

        public class Select : Energy.Query.Statement.Base
        {
            private List<string> _Columns = new List<string>();

            private List<string> _Tables = new List<string>();

            private List<string> _Join = new List<string>();

            //public override string ToString(Energy.Base.Query.Dialect dialect)
            //{
            //    return "";
            //}
        }

        #endregion

        #region Insert

        public class Insert : Energy.Query.Statement.Base
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

            //public override string ToString(Energy.Base.Query.Dialect dialect)
            //{
            //    return dialect.Insert(this);
            //}
        }

        #endregion
    }
}
