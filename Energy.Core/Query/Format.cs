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
        public Energy.Base.Format.Quote LiteralQuote; // 'text'

        public Energy.Base.Format.Quote ObjectQuote; // "column"

        /// <summary>Use TZ format for DATETIME.</summary>
        public bool UseT;

        public string CurrentTimestamp;

        #region Implicit

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

        #endregion

        #region Singleton

        private static Energy.Query.Format _Default;
        private static readonly object _DefaultLock = new object();
        /// <summary>Singleton</summary>
        public static Energy.Query.Format Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_DefaultLock)
                    {
                        if (_Default == null)
                        {
                            _Default = new Energy.Query.Format()
                                {
                                    LiteralQuote = "'",
                                    ObjectQuote = @"""", 
                                }
                                ;
                        }
                    }
                }
                return _Default;
            }
        }

        #endregion

        #region Object

        /// <summary>
        /// Format as database object (table, column, ...).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Object(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            else
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

        #endregion

        #region Text

        /// <summary>
        /// Format as TEXT.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Text(string value)
        {
            if (value == null)
                return "NULL";
            return LiteralQuote.Surround(value);
        }

        /// <summary>
        /// Format as TEXT. 
        /// When nullify parameter is set to true, null values
        /// will be represented as "NULL" instead of "''".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Text(string value, bool nullify)
        {
            if (nullify && value == null)
                return "NULL";
            return LiteralQuote.Surround(value);
        }

        /// <summary>
        /// Format as TEXT with limited length.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="limit"></param>
        /// <returns>string</returns>
        public string Text(string text, int limit)
        {
            return Text(Energy.Base.Text.Limit(text, limit));
        }

        /// <summary>
        /// Format as TEXT.
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>static</returns>
        public string Text(object value)
        {
            if (value == null)
                return "NULL";
            return Text(Energy.Base.Cast.ObjectToString(value));
        }

        #endregion

        #region Unicode

        /// <summary>
        /// Format as Unicode TEXT.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>string</returns>
        public string Unicode(string name)
        {
            return "N" + Text(name);
        }

        /// <summary>
        /// Format as Unicode TEXT.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Unicode(string name, bool nullify)
        {
            if (nullify && name == null)
                return "NULL";
            return "N" + Text(name);
        }

        #endregion

        #region Number

        /// <summary>
        /// Format as NUMBER.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns></returns>
        public string Number(object value, bool nullify = false)
        {
            if (value == null)
                return nullify ? "NULL" : "0";
            if (value is bool)
                return (bool)value ? "1" : "0";
            if (value is long || value is Int64)
                return ((long)value).ToString();
            if (value is int || value is Int32)
                return ((int)value).ToString();
            if (value is short || value is Int16)
                return ((short)value).ToString();
            if (value is SByte)
                return ((SByte)value).ToString();
            if (value is ulong || value is UInt64)
                return ((ulong)value).ToString();
            if (value is uint || value is UInt32)
                return ((uint)value).ToString();
            if (value is ushort || value is UInt16)
                return ((ushort)value).ToString();
            if (value is byte)
                return ((byte)value).ToString();
            if (value is double)
                return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is decimal)
                return ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is string)
            {
                string v = ((string)value).Trim(' ', '\t', '\r', '\n', '\v', '\0').Replace(" ", null);
                long l;
                if (long.TryParse(v, out l))
                    return Number(l);
                double d;
                if (double.TryParse(v, System.Globalization.NumberStyles.Float
                    , System.Globalization.CultureInfo.InvariantCulture, out d))
                    return Number(d);
                if (double.TryParse(v, System.Globalization.NumberStyles.Float
                    , System.Globalization.CultureInfo.CurrentCulture, out d))
                    return Number(d);
                return nullify ? "NULL" : "0";
            }
            throw new NotImplementedException();
        }

        #endregion

        #region Integer

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number">decimal</param>
        /// <returns>string</returns>
        public static string Integer(decimal number)
        {
            return ((long)Math.Floor(number)).ToString();
        }

        #endregion

        #region Date

        /// <summary>
        /// Format as DATE.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Date(DateTime value)
        {
            return LiteralQuote.Surround(value.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Format as DATE.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Date(object value)
        {
            return Date(Energy.Base.Cast.ObjectToDateTime(value));
        }

        #endregion

        #region Time

        /// <summary>
        /// Format as TIME.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Time(DateTime value)
        {
            string text = value.Millisecond == 0
                ? value.ToString("HH:mm:ss")
                : value.ToString("HH:mm:ss.fff")
                ;
            return LiteralQuote.Surround(text);
        }

        /// <summary>
        /// Format as TIME.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Time(object value)
        {
            return Time(Energy.Base.Cast.ObjectToDateTime(value));
        }

        #endregion

        #region Stamp

        /// <summary>
        /// Format as DATETIME.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Stamp(DateTime value)
        {
            if (value == System.DateTime.MinValue)
                return "NULL";
            string text = null;
            if (value.Millisecond > 0)
                text = value.ToString("yyyy-MM-dd HH:mm:ss");
            else
                text = value.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (UseT)
                text = text.Replace(' ', 'T');

            return LiteralQuote.Surround(text);
        }

        /// <summary>
        /// Format as DATETIME.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Stamp(object value)
        {
            return Stamp(Energy.Base.Cast.ObjectToDateTime(value));
        }

        #endregion
    }
}
