using System;
using System.Collections.Generic;
using System.Text;
using Energy.Source;

namespace Energy.Query.Dialect
{
    /// <summary>
    /// Microsoft SQL Server
    /// </summary>
    public class SQLSERVER: Energy.Interface.IDialect
    {
        public Format Format { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CreateTable(Energy.Source.Structure.Table table, Energy.Query.Style configuration)
        {
            if (table == null || table.Columns.Count == 0) return "";
            if (configuration == null) configuration = Energy.Query.Style.Global;

            List<string> script = new List<string>();

            string identity = configuration.IdentityName;
            string tableName = "[" + table.Name + "]";

            script.Add("IF ( OBJECT_ID('" + tableName + "') ) IS NULL");
            script.Add("");
            script.Add("CREATE TABLE " + tableName);
            script.Add("(");

            if (!String.IsNullOrEmpty(table.Identity))
            {
                Energy.Source.Structure.Column column = table.Columns.Get(table.Identity) ?? new Energy.Source.Structure.Column(table.Identity);
                string type = column.Type;
                if (type == null || type.Length == 0) type = configuration.IdentityType;
                script.Add("\t[" + column.Name + "] " + type + " IDENTITY(1,1) NOT NULL ,");
                identity = table.Identity;
            }
            else if (table.Columns.Get(identity) == default(Energy.Source.Structure.Column))
            {
                script.Add("\t[" + identity + "] " + configuration.IdentityType + " IDENTITY(1,1) NOT NULL ,");
            }

            foreach (Energy.Source.Structure.Column column in table.Columns)
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
            script.Add("");
            script.Add("GO");

            return String.Join(Energy.Base.Text.NL, script.ToArray());
        }

        public string CreateDescription(Energy.Source.Structure.Table table)
        {
            List<string> script = new List<string>();
            string schema = "dbo";

            script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Caption' , @value=N'Klucz' , @level0type='SCHEMA' , @level0name='"
                + schema + "',@level1type='TABLE',@level1name='" + table.Name + "',@level2type='COLUMN',@level2name='id'");
            foreach (Energy.Source.Structure.Column column in table.Columns)
            {
                script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Caption',@value=N'" + column.Label
                    + "',@level0type='SCHEMA',@level0name='" + schema + "',@level1type='TABLE',@level1name='" + table.Name
                    + "',@level2type='COLUMN',@level2name='" + column.Name + "'");
            }

            script.Add("");
            script.Add("GO");
            script.Add("");

            script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Description',@value=N'Klucz',@level0type='SCHEMA',@level0name='"
                + schema + "',@level1type='TABLE',@level1name='" + table.Name + "',@level2type='COLUMN',@level2name='id'");
            foreach (Energy.Source.Structure.Column column in table.Columns)
            {
                string description = String.IsNullOrEmpty(column.Description) ? column.Label : column.Description;
                script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Description',@value=N'" + description
                    + "',@level0type='SCHEMA',@level0name='" + schema + "',@level1type='TABLE',@level1name='" + table.Name
                    + "',@level2type='COLUMN',@level2name='" + column.Name + "'");
            }

            script.Add("");
            script.Add("GO");

            return String.Join(Energy.Base.Text.NL, script.ToArray());
        }

        public string Select(Energy.Query.Statement.Select statement)
        {
            throw new NotImplementedException();
        }

        public string Insert(Energy.Query.Statement.Insert statement)
        {
            throw new NotImplementedException();
        }

        public string CreateTable(Structure.Table table)
        {
            throw new NotImplementedException();
        }

        public string CreateColumn(Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string CreateIndex(Structure.Index index)
        {
            throw new NotImplementedException();
        }

        public string DropTable(Structure.Table table)
        {
            throw new NotImplementedException();
        }

        public string DropColumn(Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string DropIndex(Structure.Index index)
        {
            throw new NotImplementedException();
        }

        public string DropTable(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
