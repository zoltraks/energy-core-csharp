using System;
using System.Collections.Generic;
using System.Text;
using Energy.Query;

namespace Energy.Base
{
    /// <summary>
    /// SQL database query specific classes
    /// </summary>
    public class Query
    {
        public class Dialect
        {
            internal string Insert(Insert insert)
            {
                throw new NotImplementedException();
            }
        }

        public abstract class Statement
        {
            private Energy.Interface.IDialect Dialect;

            public Statement()
            {
            }

            public Statement(Energy.Interface.IDialect dialect)
            {
                Dialect = dialect;
            }

            public abstract string ToString(Energy.Base.Query.Dialect dialect);
        }
    }
}
