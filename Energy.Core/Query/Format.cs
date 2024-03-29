﻿using Energy.Enumeration;
using System;
using System.Collections.Generic;

namespace Energy.Query
{
    /// <summary>
    /// Value formatter for SQL
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

#if !NETCF

        #region Private

        private Dictionary<string, Format> _DialectFormatDictionary;

        #endregion

#endif

        #region Accessor

        // TODO Check if this property is needed anywhere. Maybe it should be removed?

#if !NETCF

        public Format this[string dialect]
        {
            get
            {
                if (string.IsNullOrEmpty(dialect))
                {
                    return null;
                }
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

#endif

        #endregion

        #region Property

        /// <summary>
        /// Quotation settings
        /// </summary>
        public Class.Bracket Bracket = new Class.Bracket();

        /// <summary>Use TZ format for DATETIME.</summary>
        public bool UseT;

        public string CurrentStamp;

        private System.Text.Encoding _Encoding = System.Text.Encoding.UTF8;
        /// <summary>
        /// Encoding for conversions
        /// </summary>
        public System.Text.Encoding Encoding { get { return _Encoding; } set { _Encoding = value; } }

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
        /// Format string value as TEXT.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Text(string value)
        {
            if (value == null)
            {
                return "NULL";
            }
            return Bracket.Literal.Quote(value);
        }

        /// <summary>
        /// Format string value as TEXT.
        /// When nullify parameter is set to true, null values
        /// will be represented as "NULL" instead of "''".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Text(string value, bool nullify)
        {
            if (nullify && value == null)
            {
                return "NULL";
            }
            return Bracket.Literal.Quote(value);
        }

        /// <summary>
        /// Format string value as TEXT with limited length.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="limit"></param>
        /// <returns>string</returns>
        public string Text(string text, int limit)
        {
            return Text(Energy.Base.Text.Limit(text, limit));
        }

        /// <summary>
        /// Format string value as TEXT with limited length.
        /// When nullify parameter is set to true, null values
        /// will be represented as "NULL" instead of "''".
        /// </summary>
        /// <param name="text"></param>
        /// <param name="limit"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Text(string text, int limit, bool nullify)
        {
            return Text(Energy.Base.Text.Limit(text, limit), nullify);
        }

        /// <summary>
        /// Format object value as TEXT.
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>static</returns>
        public string Text(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            return Text(Energy.Base.Cast.ObjectToString(value));
        }

        /// <summary>
        /// Format string values as TEXT.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public string[] Text(string[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return array;
            }
            List<string> list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(Text(array[i]));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Format string values as TEXT with limited length.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="array"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string[] Text(string[] array, int limit)
        {
            if (null == array || 0 == array.Length)
            {
                return array;
            }
            List<string> list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(Text(array[i], limit));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Format string values as TEXT with limited length.
        /// When nullify parameter is set to true, null values
        /// will be represented as "NULL" instead of "''".
        /// </summary>
        /// <param name="array"></param>
        /// <param name="limit"></param>
        /// <param name="nullify"></param>
        /// <returns></returns>
        public string[] Text(string[] array, int limit, bool nullify)
        {
            if (null == array || 0 == array.Length)
            {
                return array;
            }
            List<string> list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(Text(array[i], limit, nullify));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Format string values as TEXT.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public string[] Text(object[] array)
        {
            if (null == array)
            {
                return null;
            }
            if (0 == array.Length)
            {
                return new string[] { };
            }
            List<string> list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(Text(array[i]));
            }
            return list.ToArray();
        }

        #endregion

        #region Unicode

        /// <summary>
        /// Format as Unicode TEXT.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public string Unicode(object value)
        {
            if (value == null)
                return "NULL";
            DateTime dateTime = Energy.Base.Cast.ObjectToDateTime(value);
            if (dateTime != DateTime.MinValue)
            {
                bool optionRecogniseDateTime;
                optionRecogniseDateTime = false;
                if (optionRecogniseDateTime)
                {
                    return Stamp(dateTime);
                }
            }
            return "N" + Text(Energy.Base.Cast.ObjectToString(value));
        }

        /// <summary>
        /// Format as Unicode TEXT.
        /// Null values will be represented as "NULL".
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public string Unicode(string value)
        {
            if (value == null)
                return "NULL";
            return "N" + Text(value);
        }

        /// <summary>
        /// Format as Unicode TEXT.
        /// When nullify parameter is set to true, null values
        /// will be represented as "NULL" instead of "''".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Unicode(string value, bool nullify)
        {
            if (nullify && value == null)
                return "NULL";
            return "N" + Text(value);
        }

        /// <summary>
        /// Format as Unicode TEXT.
        /// When nullify parameter is set to true, null values
        /// will be represented as "NULL" instead of "''".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns>string</returns>
        public string Unicode(object value, bool nullify)
        {
            return Unicode(Energy.Base.Cast.ObjectToString(value), nullify);
        }

        #endregion

        #region Number

        /// <summary>
        /// Format as NUMBER.
        /// Real numbers are represented with dot "." as decimal point separator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Number(object value)
        {
            return Number(value, false);
        }

        /// <summary>
        /// Format as NUMBER.
        /// Real numbers are represented with dot "." as decimal point separator.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullify"></param>
        /// <returns></returns>
        public string Number(object value, bool nullify)
        {
            if (value == null)
            {
                return nullify ? "NULL" : "0";
            }
            if (value is bool)
            {
                return (bool)value ? "1" : "0";
            }
            if (value is long)
            {
                return ((long)value).ToString();
            }
            if (value is int)
            {
                return ((int)value).ToString();
            }
            if (value is short)
            {
                return ((short)value).ToString();
            }
            if (value is sbyte)
            {
                return ((sbyte)value).ToString();
            }
            if (value is ulong)
            {
                return ((ulong)value).ToString();
            }
            if (value is uint)
            {
                return ((uint)value).ToString();
            }
            if (value is ushort)
            {
                return ((ushort)value).ToString();
            }
            if (value is byte)
            {
                return ((byte)value).ToString();
            }
            if (value is double)
            {
                return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            if (value is float)
            {
                return ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            if (value is decimal)
            {
                return ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            string s = value is string ? (string)value : value.ToString();

            if (!string.IsNullOrEmpty(s))
            {
                long _long;
                if (Energy.Base.Text.TryParse<long>(s, out _long))
                {
                    return Number(_long);
                }

                if (Energy.Base.Text.Contains(s, ","))
                {
                    s = s.Replace(',', '.');
                }

                decimal _decimal;
                if (Energy.Base.Text.TryParse<decimal>(s, out _decimal))
                {
                    return Number(_decimal);
                }
            }

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
#if !NETCF
            return Number(Math.Floor(number));
#endif
#if NETCF
            return Number(Math.Floor((double)number));
#endif
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number">double</param>
        /// <returns>string</returns>
        public string Integer(double number)
        {
            return Number(Math.Floor(number));
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number">float</param>
        /// <returns>string</returns>
        public string Integer(float number)
        {
            return Number(Math.Floor(number));
        }

        /// <summary>
        /// Format as INTEGER.
        /// Returns "1" for true and "0" for false.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(bool number)
        {
            return number ? "1" : "0";
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(char number)
        {
            return ((int)number).ToString();
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
        /// <returns>string</returns>
        public string Integer(uint number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(long number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(ulong number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Format as INTEGER.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public string Integer(object number)
        {
            return Integer(number, false);
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
            {
                return nullify ? "NULL" : "0";
            }
            if (number is decimal)
            {
#if !NETCF
                return Number(Math.Floor((decimal)number));
#endif
#if NETCF
                return Number(Math.Floor((double)(decimal)number));
#endif
            }
            if (number is float)
            {
                return Number(Math.Floor((float)number));
            }
            if (number is double)
            {
                return Number(Math.Floor((double)number));
            }
            return Number(Energy.Base.Cast.ObjectToLong(number));
        }

        #endregion

        #region Date

        /// <summary>
        /// Format as DATE.
        /// Represents date as quoted date string using "YYYY-MM-DD" format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Date(DateTime value)
        {
            return Bracket.Literal.Quote(value.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Format as DATE.
        /// Represents date as quoted date string using "YYYY-MM-DD" format.
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
        /// Uses 24h "hh:mm:ss" format.
        /// Milliseconds will be used if present.
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
        /// Uses 24h "hh:mm:ss" format.
        /// Milliseconds will be used if present.
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
        /// Uses by default "YYYY-MM-DD hh:mm:ss" format or "YYYY-MM-DD**T**hh:mm:ss"
        /// depending on settings.
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
        /// Uses by default "YYYY-MM-DD hh:mm:ss" format or "YYYY-MM-DD**T**hh:mm:ss"
        /// depending on settings.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Stamp(object value)
        {
            return Stamp(Energy.Base.Cast.ObjectToDateTime(value));
        }

        #endregion

        #region Binary

        /// <summary>
        /// Format as BINARY.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Binary(byte[] data)
        {
            if (null == data || 0 == data.Length)
            {
                return "NULL";
            }
            string result = string.Concat("0x", BitConverter.ToString(data).Replace("-", null));
            return result;
        }

        /// <summary>
        /// Format as BINARY.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Binary(string text)
        {
            return Binary(this.Encoding.GetBytes(text));
        }

        /// <summary>
        /// Format as BINARY.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Binary(object value)
        {
            if (null == value)
            {
                return "NULL";
            }
            if (value is byte[])
            {
                return Binary((byte[])value);
            }
            if (value is string)
            {
                return Binary((string)value);
            }
            return Binary(Text(value));
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

        #region Null

        /// <summary>
        /// Format as NULL.
        /// </summary>
        /// <returns></returns>
        public string Null()
        {
            return "NULL";
        }

        #endregion

        #region Value

        /// <summary>
        /// Format value as desired type. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="unicode"></param>
        /// <returns></returns>
        public string Value(object value, FormatType type, bool unicode)
        {
            switch (type)
            {
                default:
                    if (unicode)
                    {
                        return this.Unicode(value);
                    }
                    else
                    {
                        return this.Text(value);
                    }

                case Energy.Enumeration.FormatType.Number:
                    return this.Number(value);

                case Energy.Enumeration.FormatType.Integer:
                    return this.Integer(value);

                case Energy.Enumeration.FormatType.Date:
                    return this.Date(value);

                case Energy.Enumeration.FormatType.Time:
                    return this.Time(value);

                case Energy.Enumeration.FormatType.Stamp:
                    return this.Stamp(value);

                case Enumeration.FormatType.Binary:
                    return this.Binary(value);

                case Enumeration.FormatType.Plain:
                    return Energy.Base.Cast.ObjectToString(value);
            }
        }

        #endregion
    }
}
