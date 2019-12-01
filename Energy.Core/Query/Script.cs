using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Energy.Query
{
    public partial class Script
    {
        #region Constructor

        public Script()
        {
            Energy.Core.Bug.Write("");
            Debug.WriteLine("Energy.Query.Script created");
        }

        public Script(Energy.Enumeration.SqlDialect dialect)
        {
            Dialect = dialect;
            Debug.WriteLine(String.Format("Energy.Query.Script created for {0} dialect", dialect.ToString()));
        }

        public static implicit operator Script(Energy.Enumeration.SqlDialect dialect)
        {
            try
            {
                return new Script() { Dialect = dialect };
            }
            finally
            {
                Debug.Write(String.Format("Energy.Query.Script created for {0} dialect", dialect.ToString()));
            }
        }

        #endregion

        private Energy.Enumeration.SqlDialect _Dialect;

        public Energy.Enumeration.SqlDialect Dialect
        {
            get
            {
				return _Dialect;
            }
            set
            {
				_Dialect = value;
            }
        }

		private Energy.Query.Format _Format;

		public Energy.Query.Format Format {
			get {
				if (_Format == null)
					_Format = (Energy.Query.Format)Dialect;
				return _Format;
			}
			set {
				_Format = value;
			}
		}

        #region CREATE

        public virtual string CreateTable(Energy.Source.Structure.Table table, Energy.Query.Style configuration)
        {
            throw new NotImplementedException();
        }

        public virtual string CreateTable(Energy.Source.Structure.Table table)
        {
            throw new NotImplementedException();
        }

        public virtual string CreateDescription(Energy.Source.Structure.Table table)
        {
			throw new NotImplementedException ();
        }

        public virtual string CreateIndex(Energy.Source.Structure.Table table)
        {
			throw new NotImplementedException ();
        }

        #endregion

        #region DROP

		public virtual string DropTable(string table)
		{
			throw new NotImplementedException ();
		}

        #endregion

        public string Merge(Energy.Source.Structure.Table table, bool compact)
        {
            string name = Format.Object(table.Name);
            StringBuilder script = new StringBuilder();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                List<string> where = new List<string>();
                List<string> column = new List<string>();
                List<string> insert = new List<string>();
                List<string> update = new List<string>();
                //for (int j = 0; j < table.Keys.Count; j++)
                //{
                //    string key = table.Keys[j];
                //    string value = table.Rows[i][table.Keys[j]].ToString();
                //    key = "[" + key + "]";
                //    value = "'" + value + "'";
                //    where.Add(key + " = " + value);
                //    column.Add(key);
                //    insert.Add(value);
                //}
                //foreach (string key in table.Rows[i].Keys)
                //{
                //    if (table.Keys.Contains(key))
                //        continue;

                //    string value = table.Rows[i][key].ToString();
                //    update.Add("[" + key + "] = '" + value + "'");
                //}
                script.Append("IF NOT EXISTS ( SELECT 1 FROM " + name + " WHERE " + String.Join(" AND ", where.ToArray()) + " )");
                if (!compact) script.AppendLine();
                script.Append(compact ? " " : "\t");
                script.Append("INSERT INTO " + name + " ( " + String.Join(" , ", column.ToArray())
                    + " ) VALUES ( " + String.Join(" , ", insert.ToArray()) + " )");
                if (!compact) script.AppendLine();
                if (compact) script.Append(" ");
                script.Append("UPDATE " + name + " SET " + String.Join(" , ", update.ToArray()));
                if (!compact) script.AppendLine();
                script.Append(compact ? " " : "\t");
                script.Append("WHERE " + String.Join(" AND ", where.ToArray()));
                script.AppendLine();
            }

            return script.ToString().Trim();
        }

        public virtual string CurrentTimestamp()
        {
            return "CURRENT_TIMESTAMP";
        }

        public string Merge(Energy.Source.Structure.Table table)
        {
            return Merge(table, false);
        }
    }
}