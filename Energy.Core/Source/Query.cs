using System;
using System.Collections.Generic;
using System.Text;
using Energy.Enumeration;

namespace Energy.Source
{
    /// <summary>
    /// Query language generic class
    /// </summary>
    public class Query
    {
        #region Format

        /// <summary>
        /// Text formatter class for SQL queries.
        /// </summary>
        public class Format
        {
            public Energy.Base.Format.Quote LiteralQuote; // 'column'

            public Energy.Base.Format.Quote ObjectQuote; // "value"

            /// <summary>
            /// Format as TEXT.
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string Text(string value)
            {
                return LiteralQuote.Escape(value);
            }

            /// <summary>
            /// Format as database object (table, column, ...).
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public string Object(string value)
            {
                return ObjectQuote.Escape(value);
            }

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
        }

        #endregion

        /// <summary>
        /// Query language dialect settings
        /// </summary>
        public class Dialect
        {
            public Query.Format Format;
            private SqlDialect _Dialect;

            public Dialect(SqlDialect _Dialect)
            {
                this._Dialect = _Dialect;
            }

            public static SqlDialect Guess(string name, string full)
            {
                if (!string.IsNullOrEmpty(full))
                {
                    if (full == "System.Data.SqlClient.SqlConnection")
                        return SqlDialect.SqlServer;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    if (name == "SqlConnection")
                        return SqlDialect.SqlServer;
                }
                return SqlDialect.None;
            }

            public static SqlDialect Guess(string name)
            {
                return Guess(name, null);
            }
        }
    }
}
