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
            if (false)
            { }
            else if (dialect == Energy.Enumeration.SqlDialect.SqlServer)
            {
                format.LiteralQuote = "'";
                format.ObjectQuote = "[]";
            }
            else if (dialect == Energy.Enumeration.SqlDialect.MySQL)
            {
                format.LiteralQuote = "'";
                format.ObjectQuote = "`";
            }
            else
            {
                format.LiteralQuote = "'";
                format.ObjectQuote = "\"";
            }
            return format;
        }

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
