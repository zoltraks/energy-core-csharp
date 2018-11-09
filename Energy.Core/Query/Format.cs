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
        #region Class

        public class Class
        {
            public class Bracket
            {
                /// <summary>
                /// Literal bracket for values.
                /// </summary>
                public Energy.Base.Bracket Literal; // 'text'

                /// <summary>
                /// Object bracket for database objects.
                /// </summary>
                public Energy.Base.Bracket Object; // "column"
            }
        }

        #endregion

        #region Private

        private Dictionary<string, Format> _DialectFormatDictionary;

        #endregion

        #region Accessor

        public Format this[string dialect]
        {
            get
            {
                if (string.IsNullOrEmpty(dialect))
                    return null;
                Energy.Query.Format findFormat = null;
                if (_DialectFormatDictionary == null)
                {
                    _DialectFormatDictionary = new Dictionary<string, Format>();
                }
                else
                {
                    findFormat = Energy.Base.Collection.GetStringDictionaryValue<Energy.Query.Format>(_DialectFormatDictionary, dialect, true);
                }
                if (findFormat == null)
                {
                    System.Type classFormat = null;
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Energy.Query.Format));
                    System.Type[] types;
                    types = assembly.GetTypes();
                    System.Type[] class1Array = Energy.Base.Class.GetTypesWithAttribute(types, typeof(Energy.Attribute.Code.TemporaryAttribute));
                    System.Type[] class2Array = Energy.Base.Class.GetTypesWithInterface(types, typeof(Energy.Interface.IDialect));

                    classFormat = Energy.Base.Collection.GetFirstOrDefault<System.Type>(class1Array, class2Array);

                    if (classFormat != null)
                    {
                        findFormat = (Energy.Query.Format)Activator.CreateInstance(classFormat);
                        _DialectFormatDictionary[dialect] = findFormat;
                    }
                }
                return findFormat;

            }
        }

        #endregion

        #region Property

        /// <summary>
        /// Quotation settings
        /// </summary>
        public Class.Bracket Bracket = new Class.Bracket();

        /// <summary>Use TZ format for DATETIME.</summary>
        public bool UseT;

        public string CurrentStamp;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Format()
        {
            Bracket.Literal = "'";
            Bracket.Object = "\"";

            CurrentStamp = "CURRENT_TIMESTAMP";
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="dialect">Database SQL dialect to use for defaults</param>
        public Format(string dialect)
        {
            Format format = new Format();
            format.SetDialect(dialect);
        }

        private void SetDialect(string dialect)
        {
            if (dialect == null)
                dialect = "";
            string literal = "'";
            string special = "\"";
            string current = "";
            bool useT = false;
            switch (dialect.Trim().ToUpper())
            {
                case "ANSI":
                default:
                    literal = "'";
                    special = "\"";
                    current = "CURRENT_TIMESTAMP";
                    break;

                case "MYSQL":
                    literal = "'";
                    special = "`";
                    current = "CURRENT_TIMESTAMP()";
                    break;

                case "SQLSERVER":
                    literal = "'";
                    special = "[]";
                    current = "GETDATE()";
                    useT = true;
                    break;

                case "SQLITE":
                    literal = "'";
                    special = "\"";
                    current = "(STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW'))";
                    break;
            }

            this.Bracket.Literal = literal;
            this.Bracket.Object = special;
            this.CurrentStamp = current;
            this.UseT = useT;
        }

        /// <summary>
        /// Create object from SqlDialect enumeration
        /// </summary>
        /// <param name="dialect"></param>
        public static implicit operator Format(Energy.Enumeration.SqlDialect dialect)
        {
            return dialect.ToString();
        }

        /// <summary>
        /// Create object from string
        /// </summary>
        /// <param name="dialect"></param>
        public static implicit operator Format(string dialect)
        {
            return new Format(dialect);
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
                            _Default = new Energy.Query.Format();
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
                return Bracket.Object.Quote(value);
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
            return Bracket.Literal.Quote(value);
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
            return Bracket.Literal.Quote(value);
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
        /// <returns></returns>
        public string Number(object value)
        {
            return Number(value, false);
        }

        /// <summary>
        /// Format as NUMBER.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns></returns>
        public string Number(object value, bool nullify)
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

            string s = value is string ? (string)value : value.ToString();
            string v = s.Trim(' ', '\t', '\r', '\n', '\v', '\0').Replace(" ", null);
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

        #endregion

        #region Integer

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number">decimal</param>
        /// <returns>string</returns>
        public string Integer(decimal number)
        {
            return Number(Math.Floor(number));
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(double number)
        {
            return Number(Math.Floor(number));
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(int number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Integer(object number, bool nullify)
        {
            if (number == null)
                return Number(number, nullify);
            if (number is decimal)
                return Number((decimal)number, nullify);
            return Number(Energy.Base.Cast.ObjectToLong(number), nullify);
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
            return Bracket.Literal.Quote(value.ToString("yyyy-MM-dd"));
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
            return Bracket.Literal.Quote(text);
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

            return Bracket.Literal.Quote(text);
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

        #region Now

        /// <summary>
        /// Format as current time equivalent.
        /// </summary>
        /// <returns>string</returns>
        public string Now()
        {
            return this.CurrentStamp;
        }

        #endregion

    }
}
