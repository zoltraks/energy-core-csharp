﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query.Dialect
{
    public class SQLITE : Energy.Interface.IDialect
    {
        #region Constructor

        public SQLITE()
        {
            this.Format = Energy.Enumeration.SqlDialect.SQLITE;
        }

        #endregion

        public Format Format { get; set; }

        public string Select(Select statement)
        {
            throw new NotImplementedException();
        }

        public string Insert(Insert statement)
        {
            throw new NotImplementedException();
        }

        public string CreateColumn(Energy.Source.Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string CreateIndex(Energy.Source.Structure.Index index)
        {
            throw new NotImplementedException();
        }

        public string CreateTable(Energy.Source.Structure.Table table)
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

        public string DropColumn(Energy.Source.Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string DropIndex(Energy.Source.Structure.Index index)
        {
            throw new NotImplementedException();
        }

        public string DropTable(Energy.Source.Structure.Table table)
        {
            throw new NotImplementedException();
        }
    }
}