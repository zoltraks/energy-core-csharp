using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query.Standard
{
    public class SqlServer: Energy.Query.Dialect
    {
        public SqlServer()
        {
            this.Format.UseT = true;
            this.Format.ObjectQuote.Bracket = "[]";
            this.Special.AddRange(new string[]
            {
                "CREATE",
                "DEFAULT",
                "DOUBLE",
                "END",
                "FULL",
                "GROUP",
                "IDENTITY",
                "INDEX",
                "KEY",
                "LOAD",
                "LOCK",
                "ORDER",
                "PRIMARY",
                "TABLE",
                "USER",
            });
        }
    }
}
