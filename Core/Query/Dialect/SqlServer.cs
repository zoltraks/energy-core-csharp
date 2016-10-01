using System;
using System.Collections.Generic;
using System.Text;
using Energy.Base;

namespace Energy.Query.Dialect
{
    /// <summary>
    /// Microsoft SQL Server
    /// </summary>
    public class SqlServer: Energy.Interface.IQueryDialect
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
            if (table == null || table.Columns.Count == 0) return "";

            List<string> script = new List<string>();

            string identity = "id";
            string identityType = "BIGINT";
            string tableName = "[" + table.Name + "]";

            script.Add("IF ( OBJECT_ID('" + tableName + "') ) IS NULL");
            script.Add("");
            script.Add("CREATE TABLE " + tableName);
            script.Add("(");

            if (!String.IsNullOrEmpty(table.Identity))
            {
                Energy.Base.Structure.Column column = table.Columns.Get(table.Identity) ?? new Energy.Base.Structure.Column(table.Identity);
                string type = column.Type;
                if (type == null || type.Length == 0) type = identityType;
                script.Add("\t[" + column.Name + "] " + type + " IDENTITY(1,1) NOT NULL ,");
                identity = table.Identity;
            }
            else if (table.Columns.Get(identity) == default(Energy.Base.Structure.Column))
            {
                script.Add("\t[" + identity + "] " + identityType + " IDENTITY(1,1) NOT NULL ,");
            }

            foreach (Energy.Base.Structure.Column column in table.Columns)
            {
                if (!column.Ignore && !String.IsNullOrEmpty(column.Name) && column.Name != table.Identity)
                {
                    script.Add("\t[" + column.Name + "] " + column.Type + " NOT NULL ,");
                }
            }

            script.Add("");
            script.Add("\tCONSTRAINT [PK_" + table.Name + "] PRIMARY KEY NONCLUSTERED");
            script.Add("\t(");
            script.Add("\t\t[" + identity + "] ASC");
            script.Add("\t)");
            script.Add("\tWITH ( PAD_INDEX = OFF , STATISTICS_NORECOMPUTE = OFF , IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON )");
            script.Add(")");
            script.Add("ON [PRIMARY]");

            return String.Join(Environment.NewLine, script.ToArray());
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
