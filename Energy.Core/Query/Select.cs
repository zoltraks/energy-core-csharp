using System;
using System.Collections.Generic;
using System.Text;
using Energy.Base;

namespace Energy.Query
{
    public class Select: Energy.Base.Query.Statement
    {
        private List<string> _Columns = new List<string>();

        private List<string> _Tables = new List<string>();

        private List<string> _Join = new List<string>();

        public override string ToString(Base.Query.Dialect dialect)
        {
            return "";
        }
    }
}
