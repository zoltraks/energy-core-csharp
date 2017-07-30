using System;
using System.Collections.Generic;
using System.Text;
using Energy.Source;

namespace Energy.Query.Layer
{
    /// <summary>
    /// MySQL
    /// </summary>
    public class MySQL
    {
        private Energy.Query.Format Format;

        public string DefaultEngine = "InnoDB";

        public MySQL(Energy.Query.Dialect dialect)
        {
            this.Format = dialect.Format;
        }

        public MySQL(Energy.Query.Format format)
        {
            this.Format = format;
        }

        /// <summary>
        /// Create table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public string CreateTable(Energy.Source.Structure.Table table, Energy.Query.Configuration configuration)
        {
            if (table == null || table.Columns.Count == 0)
                return "";
            if (configuration == null)
                configuration = Energy.Query.Configuration.Default;

            StringBuilder script = new StringBuilder();

            if (table == null || table.Columns.Count == 0) return "";
            if (configuration == null) configuration = Energy.Query.Configuration.Default;

            string identity = configuration.IdentityName;
            string tableName = table.Name;

            script.Append("CREATE TABLE IF NOT EXISTS " + Format.Object(tableName));
            script.AppendLine();
            script.Append("(");
            script.AppendLine();

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
            if (type == null || type.Length == 0) type = configuration.IdentityType;
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

            script.Append(string.Concat("\t", string.Join(" ," + Environment.NewLine + "\t", list.ToArray()), Environment.NewLine));
            script.Append(")");
            script.AppendLine();
            string engine = string.IsNullOrEmpty(table.Engine) ? DefaultEngine : table.Engine;
            if (!string.IsNullOrEmpty(engine))
                script.Append("ENGINE = " + engine);
            script.Append(";");
            script.AppendLine();

            return script.ToString();
        }

        public string DropTable(string table)
        {
            return "DROP TABLE IF EXISTS " + Format.Object(table);
        }
    }
}
