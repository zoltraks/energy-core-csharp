using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Energy.Base;

namespace Energy.Query
{
    /// <summary>
    /// Text formatter class for SQL queries.
    /// </summary>
    public class Format
    {
        public Energy.Base.Format.Quote LiteralQuote; // 'column'

        public Energy.Base.Format.Quote ObjectQuote; // "value"

        public static implicit operator Format(Energy.Enumeration.SqlDialect dialect)
        {
            Format format = new Format();
            switch (dialect)
            {
                default:
                    format.CurrentTimestamp = "CURRENT_TIMESTAMP";
                    format.LiteralQuote = "'";
                    format.ObjectQuote = "\"";
                    break;

                case Enumeration.SqlDialect.MySQL:
                    format.LiteralQuote = "'";
                    format.ObjectQuote = "`";
                    format.CurrentTimestamp = "CURRENT_TIMESTAMP()";
                    break;

                case Enumeration.SqlDialect.SqlServer:
                    format.LiteralQuote = "'";
                    format.ObjectQuote = "[]";
                    format.CurrentTimestamp = "GETDATE()";
                    break;

                case Enumeration.SqlDialect.SQLite:
                    format.LiteralQuote = "'";
                    format.ObjectQuote = "\"";
                    format.CurrentTimestamp = "(STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW'))";
                    break;
            }

            return format;
        }

        public string CurrentTimestamp;

        /// <summary>
        /// Format as database object (table, column, ...).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Object(string value)
        {
            return ObjectQuote.Surround(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Object(string database, string schema, string name)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(database))
            {
                list.Add(Object(database));
                if (!string.IsNullOrEmpty(schema))
                    list.Add(Object(schema));
                else
                    list.Add("");
            }
            else
            {
                if (!string.IsNullOrEmpty(schema))
                    list.Add(Object(schema));
            }
            list.Add(Object(name));
            return string.Join(".", list.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Object(string schema, string name)
        {
            if (string.IsNullOrEmpty(schema))
                return Object(name);
            else
                return string.Concat(Object(schema), ".", Object(name));
        }

        /// <summary>
        /// Format as TEXT.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Text(string value)
        {
            return LiteralQuote.Escape(value);
        }
    }
}
