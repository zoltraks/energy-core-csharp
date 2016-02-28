using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Energy.Core
{
    public partial class Query
    {
        
    }

    public partial class Query
    {
        public partial class Format
        {
            public string Text(string value)
            {
                if (LiteralQuote.Prefix == null && LiteralQuote.Suffix == null)
                {
                    return value;
                }
                if (LiteralQuote.Prefix != null && value.Contains(LiteralQuote.Prefix))
                {
                    return LiteralQuote.Prefix + value.Replace(LiteralQuote.Prefix, LiteralQuote.Prefix + LiteralQuote.Prefix) + LiteralQuote.Suffix;
                }
                return LiteralQuote.Prefix + value + LiteralQuote.Suffix;
            }

            public struct Quote
            {
                public string Prefix;
                public string Suffix;

                public string Bracket
                {
                    set
                    {
                        if (value.Length % 2 == 0)
                        {
                            Prefix = value.Substring(0, value.Length / 2);
                            Suffix = value.Substring(value.Length);
                        }
                        else
                        {
                            Prefix = Suffix = value;
                        }
                    }
                }

                public string Escape(string content)
                {
                    if (content.Contains(Prefix))
                    {
                        return content.Replace(Prefix, Prefix + Prefix);
                    }
                    else
                    {
                        return content;
                    }
                }

                public string Unescape(string content)
                {
                    return content.Replace(Prefix + Prefix, Prefix);
                }
            }

            public Quote LiteralQuote; // 'column'
            public Quote ObjectQuote; // "value"

            public class ANSI : Format
            {
                public ANSI()
                {
                    ObjectQuote.Prefix = ObjectQuote.Suffix = "`";
                }
            }

            public class MySQL : Format
            {
                public MySQL()
                {
                    LiteralQuote.Bracket = "'";
                    ObjectQuote.Bracket = ObjectQuote.Suffix = "`";
                }
            }

            public class SQLServer : Format
            {
                public SQLServer()
                {
                    ObjectQuote.Prefix = "[";
                    ObjectQuote.Suffix = "]";
                }
            }

            public class SQLite : Format
            {
                public SQLite()
                {
                    ObjectQuote.Prefix = ObjectQuote.Suffix = "'";
                }
            }
        }
    }

    public partial class Query
    {
        public partial class Script
        {
            public Base.Enumeration.SQL Layer { get; set; }

            public Script()
            {
                Debug.Write("Energy.Query.Script created");
            }

            #region Default

            public class Default : Script
            {
                public static Default Instance
                {
                    get
                    {
                        if (_instance == null)
                        {
                            lock (typeof(Default))
                            {
                                if (_instance == null)
                                {
                                    _instance = new Default();
                                }
                            }

                        }

                        return _instance;
                    }
                }

                protected Default() { }

                private static volatile Default _instance = null;
            }

            #endregion

            #region CREATE

            public partial class Create
            {
                public static string Table(Energy.Base.Structure.Table table, Base.Enumeration.SQL layer = Base.Enumeration.SQL.None)
                {
                    if (table == null || table.Column.Count == 0) return "";
                    if (layer == Base.Enumeration.SQL.None) layer = Energy.Core.Query.Script.Default.Instance.Layer;
                    string identity = "id";

                    List<string> script = new List<string>();
                    string tableName = "[" + table.Name + "]";
                    script.Add("IF ( OBJECT_ID('" + tableName + "') ) IS NULL");
                    script.Add("");
                    script.Add("CREATE TABLE " + tableName);
                    script.Add("(");

                    if (!String.IsNullOrEmpty(table.Identity))
                    {
                        Energy.Base.Structure.Column column = table.Column.Get(table.Identity)
                            ?? new Energy.Base.Structure.Column() { Name = table.Identity, Type = "BIGINT" };
                        string type = column.Type;
                        if (type == null || type.Length == 0) type = "BIGINT";
                        script.Add("\t[" + column.Name + "] " + type + " IDENTITY(1,1) NOT NULL ,");
                        identity = table.Identity;
                    }
                    else if (table.Column.Get(identity) == default(Energy.Base.Structure.Column))
                    {
                        script.Add("\t[" + identity + "] BIGINT IDENTITY(1,1) NOT NULL ,");
                    }

                    foreach (Energy.Base.Structure.Column column in table.Column)
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

                    return String.Join(Environment.NewLine, script.ToArray());
                }

                public static string Description(Energy.Base.Structure.Table table)
                {
                    List<string> script = new List<string>();
                    string schema = "dbo";

                    script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Caption',@value=N'Klucz',@level0type='SCHEMA',@level0name='" + schema + "',@level1type='TABLE',@level1name='" + table.Name + "',@level2type='COLUMN',@level2name='id'");
                    foreach (Energy.Base.Structure.Column column in table.Column)
                    {
                        script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Caption',@value=N'" + column.Label + "',@level0type='SCHEMA',@level0name='" + schema + "',@level1type='TABLE',@level1name='" + table.Name + "',@level2type='COLUMN',@level2name='" + column.Name + "'");
                    }

                    script.Add("");
                    script.Add("GO");
                    script.Add("");

                    script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Description',@value=N'Klucz',@level0type='SCHEMA',@level0name='" + schema + "',@level1type='TABLE',@level1name='" + table.Name + "',@level2type='COLUMN',@level2name='id'");
                    foreach (Energy.Base.Structure.Column column in table.Column)
                    {
                        string description = String.IsNullOrEmpty(column.Description) ? column.Label : column.Description;
                        script.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Description',@value=N'" + description + "',@level0type='SCHEMA',@level0name='" + schema + "',@level1type='TABLE',@level1name='" + table.Name + "',@level2type='COLUMN',@level2name='" + column.Name + "'");
                    }

                    script.Add("");
                    script.Add("GO");

                    return String.Join(Environment.NewLine, script.ToArray());
                }

                public static string Index(Energy.Base.Structure.Table table)
                {
                    List<string> script = new List<string>();
                    string tableName = "[" + table.Name + "]";
                    foreach (Energy.Base.Structure.Index index in table.Index)
                    {
                        string indexName = "[" + index.Name + "]";
                        script.Add("--BEGIN TRY DROP INDEX " + indexName + " ON " + tableName + " END TRY BEGIN CATCH END CATCH");
                        script.Add("");
                        script.Add("BEGIN TRY");
                        script.Add("");
                        script.Add("CREATE NONCLUSTERED INDEX " + indexName + " ON " + tableName);
                        script.Add("(");
                        List<string> list;
                        list = new List<string>();
                        foreach (Energy.Base.Structure.Column column in index.Column)
                        {
                            list.Add("[" + column.Name + "]");
                        }
                        script.Add("\t" + String.Join(" , ", list.ToArray()));
                        script.Add(")");
                        if (index.Include.Count > 0)
                        {
                            script.Add("INCLUDE");
                            script.Add("(");
                            list = new List<string>();
                            foreach (Energy.Base.Structure.Column column in index.Include)
                            {
                                list.Add("[" + column.Name + "]");
                            }
                            script.Add("\t" + String.Join(" , ", list.ToArray()));
                            script.Add(")");
                        }
                        script.Add("");
                        script.Add("END TRY");
                        script.Add("BEGIN CATCH");
                        script.Add("END CATCH");
                        script.Add("");
                        script.Add("GO");
                    }

                    return String.Join(Environment.NewLine, script.ToArray());
                }
            }

            #endregion

            public string Merge(Energy.Base.Structure.Table table, bool compact = false)
            {
                string name = "[" + table.Name + "]";
                StringBuilder script = new StringBuilder();
                for (int i = 0; i < table.Row.Count; i++)
                {
                    List<string> where = new List<string>();
                    List<string> column = new List<string>();
                    List<string> insert = new List<string>();
                    List<string> update = new List<string>();
                    for (int j = 0; j < table.Key.Count; j++)
                    {
                        string key = table.Key[j];
                        string value = table.Row[i][table.Key[j]].ToString();
                        key = "[" + key + "]";
                        value = "'" + value + "'";
                        where.Add(key + " = " + value);
                        column.Add(key);
                        insert.Add(value);
                    }
                    foreach (string key in table.Row[i].Keys)
                    {
                        if (table.Key.Contains(key))
                            continue;

                        string value = table.Row[i][key].ToString();
                        update.Add("[" + key + "] = '" + value + "'");
                    }
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
        }
    }
}