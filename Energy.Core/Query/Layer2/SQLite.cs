using System;
using System.Collections.Generic;
using System.Text;
using Energy.Source;

namespace Energy.Query
{
    public partial class Script
    {
        public class SQLite: Energy.Query.Script
        {
            #region Constructor

            public SQLite()
				: this(Energy.Enumeration.SqlDialect.SQLITE)
			{
            }

            public SQLite(Energy.Enumeration.SqlDialect dialect)
            {
                this.Dialect = dialect;
            }

            public SQLite(Energy.Query.Format format)
            {
                this.Format = format;
            }

            #endregion

            public override string CreateTable(Energy.Source.Structure.Table table, Energy.Query.Configuration configuration)
            {
                StringBuilder s = new StringBuilder();
                s.Append("CREATE TABLE IF NOT EXISTS ");
                s.Append(Format.Object(table.Schema, table.Name));
                s.AppendLine();
                s.AppendLine("(");

                string identity = Energy.Base.Text.Select(table.Identity, table.Columns.GetPrimaryName());

                List<string> list = new List<string>();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    Energy.Source.Structure.Column column = table.Columns[i];

                    if (column.Ignore || string.IsNullOrEmpty(column.Name))
                        continue;

                    string line = string.Concat(Format.Object(column.Name), " ", column.Type, " ", (column.NotNull ? "NOT " : ""), "NULL");
                    if (column.Name == identity)
                    {
                        line += " PRIMARY KEY";
                        if (column.Increment > 0)
                            line += " AUTOINCREMENT";
                    }
                    list.Add(line);
                }
                string join = string.Concat(" ,", Environment.NewLine, "\t");
                s.Append(string.Concat("\t", string.Join(join, list.ToArray())));
                s.AppendLine();
                s.AppendLine(")");
                s.AppendLine();
                return s.ToString();
            }
        }
    }
}
