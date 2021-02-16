using System;
using System.Collections.Generic;
using System.Text;
using Energy.Interface;
using Energy.Source;

namespace Energy.Query.Dialect
{
    public class MYSQL: Energy.Interface.IDialect
    {
        public Energy.Query.Format Format { get; set; }

        public object[] DataTypes;

        public string DefaultEngine = "InnoDB";

        #region Constructor

        public MYSQL()
            : this(Energy.Enumeration.SqlDialect.MYSQL)
        {
        }

        public MYSQL(Energy.Query.Format format)
        {
            this.Format = format;
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public string CreateTable(Energy.Source.Structure.Table table, Energy.Query.Style configuration)
        {
            if (table == null || table.Columns.Count == 0)
                return "";
            if (configuration == null)
                configuration = Energy.Query.Style.Global;

            StringBuilder script = new StringBuilder();

            if (table == null || table.Columns.Count == 0)
                return "";
            if (configuration == null)
                configuration = Energy.Query.Style.Global;

            string identity = configuration.IdentityName;
            string tableName = table.Name;

            script.Append("CREATE TABLE IF NOT EXISTS " + Format.Object(tableName));
            script.Append(Energy.Base.Text.NL);
            script.Append("(");
            script.Append(Energy.Base.Text.NL);

            Energy.Source.Structure.Column primary = null;

            if (!string.IsNullOrEmpty(table.Identity))
                identity = table.Identity;
            else
            {
                primary = table.Columns.GetPrimary();
                if (primary != null)
                    identity = primary.Name;
            }

            if (primary == null)
            {
                primary = table.Columns.Get(identity) ?? new Energy.Source.Structure.Column(table.Identity);
            }

            List<string> list = new List<string>();
            string line;

            string type = primary.Type;
            if (type == null || type.Length == 0)
                type = configuration.IdentityType;
            line = string.Concat(Format.Object(primary.Name), " ", type, " NOT NULL");
            if (Energy.Query.Type.IsNumeric(type))
                line += " AUTO_INCREMENT";
            list.Add(line);

            for (int i = 0; i < table.Columns.Count; i++)
            {
                Energy.Source.Structure.Column column = table.Columns[i];

                if (column.Ignore || string.IsNullOrEmpty(column.Name) || column.IsName(identity))
                    continue;

                line = string.Concat(Format.Object(column.Name), " ", column.Type, " ", (column.NotNull ? "NOT " : ""), "NULL");
                list.Add(line);
            }

            script.Append(string.Concat("\t", string.Join(" ," + Energy.Base.Text.NL + "\t", list.ToArray()), Energy.Base.Text.NL));
            script.Append(")");
            script.Append(Energy.Base.Text.NL);
            string engine = string.IsNullOrEmpty(table.Engine) ? DefaultEngine : table.Engine;
            if (!string.IsNullOrEmpty(engine))
                script.Append("ENGINE = " + engine);
            script.Append(";");
            script.Append(Energy.Base.Text.NL);

            return script.ToString();
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

        #endregion

        #region DROP

        public string DropTable(string table)
        {
            return "DROP TABLE IF EXISTS " + Format.Object(table);
        }

        public string Select(Energy.Query.Statement.Select statement)
        {
            throw new NotImplementedException();
        }

        public string Insert(Energy.Query.Statement.Insert statement)
        {
            throw new NotImplementedException();
        }

        public string CreateTable(Energy.Source.Structure.Table table)
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

        public string DropTable(Energy.Source.Structure.Table table)
        {
            throw new NotImplementedException();
        }

        public string DropColumn(Energy.Source.Structure.Column column)
        {
            throw new NotImplementedException();
        }

        public string DropIndex(Energy.Source.Structure.Index index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
