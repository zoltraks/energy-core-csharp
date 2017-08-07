using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Conversion and type casting
    /// </summary>
    public static class Cast
    {
        #region As

        public static T As<T>(object value)
        {
            Type r = typeof(T);
            Type t = value.GetType();

            if (t == r)
                return (T)value;

            if (r == typeof(string))
                return (T)(object)(ObjectToString(value));

            return default(T);
        }

        /// <summary>
        /// Convert string to integer value without exception
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Integer number</returns>
        public static int AsInteger(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
            if (value is string)
                return Energy.Base.Cast.StringToInteger((string)value);
            return Energy.Base.Cast.StringToInteger(value.ToString());
        }

        public static byte ObjectToByte(object value)
        {
            if (value == null)
                return (byte)0;
            if (value is byte)
                return (byte)value;
            if (value is string)
                return StringToByte((string)value);
            return (byte)(ObjectToInteger(value) % 256);
        }

        /// <summary>
        /// Convert string to long integer value without exception
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static long AsLong(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
            if (value is string)
                return Energy.Base.Cast.StringToLong((string)value);
            return Energy.Base.Cast.StringToLong(value.ToString());
        }

        /// <summary>
        /// Convert double value to string
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Text representaiton</returns>
        public static string AsString(double value)
        {
            return Energy.Base.Cast.DoubleToString(value);
        }

        /// <summary>
        /// Convert double value to invariant string
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="culture">InvariantCulture by default, that means 1234.56 instead of 1'234,56</param>
        /// <returns>String</returns>
        public static string AsString(double value, int precision, System.Globalization.CultureInfo culture)
        {
            return Energy.Base.Cast.DoubleToString(value, precision, culture);
        }

        /// <summary>
        /// Convert double value to invariant string
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <returns>String</returns>
        public static string AsString(double value, int precision)
        {
            return Energy.Base.Cast.DoubleToString(value, precision, null);
        }

        /// <summary>
        /// Return DateTime as ISO 8601 string (empty if default)
        /// </summary>
        /// <param name="stamp">DateTime</param>
        /// <returns>Date and time string representation</returns>
        public static string AsString(DateTime stamp)
        {
            return Energy.Base.Cast.DateTimeToString(stamp);
        }

        /// <summary>
        /// Return DateTime as ISO 8601 string (empty if default or null)
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date and time string representation</returns>
        public static string AsString(DateTime? stamp)
        {
            return Energy.Base.Cast.DateTimeToString(stamp);
        }

        /// <summary>
        /// Return as string using ObjectToString method
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string AsString(object o)
        {
            return ObjectToString(o);
        }

        /// <summary>
        /// Convert string to enum
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>object</returns>
        public static T AsEnum<T>(string value)
        {
            return (T)Energy.Base.Cast.StringToEnum(value, typeof(T));
        }

        /// <summary>
        /// Convert double value to invariant string.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="trim">Trim zeroes from end</param>
        /// <param name="culture">InvariantCulture by default, that means 1234.56 instead of 1'234,56.</param>
        /// <returns>String</returns>
        public static string DoubleToString(double value, int precision, bool trim, CultureInfo culture)
        {
            string text = DoubleToString(value, precision, culture);
            return trim ? text.TrimEnd('0').TrimEnd('.') : text;
        }

        #endregion

        #region Class

        private static string RemoveNumericalDifferences(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            if (value.Contains(" "))
                value = value.Replace(" ", null);
            if (value.Contains("_"))
                value = value.Replace("_", null);
            if (value.Contains("'"))
                value = value.Replace("'", null);
            if (value.Contains("’"))
                value = value.Replace("'", null);
            return value;
        }

        #endregion

        #region Boolean

        /// <summary>
        /// Convert string to boolean assuming empty strings or containing only whitespace
        /// as false, including several common string constants meaning false like "0",
        /// "NULL" or "FALSE" (case insensitive).
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>bool</returns>
        // TODO: How to treat "0.0" which converted to integer and then to boolean be different?
        // Possible solution: check if string is a numeric value and treat 0 as false.     
        public static bool StringToBool(string text)
        {
            if (string.IsNullOrEmpty(text) || text == "0")
                return false;
            switch (text.Trim().ToUpper())
            {
                case "":
                case "NO":
                case "FALSE":
                case "N":
                case "F":
                case "NONE":
                case "NULL":
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Convert bool to string.
        /// </summary>
        /// <param name="value">bool</param>
        /// <returns>string</returns>
        public static string BoolToString(bool value)
        {
            return value ? "X" : "";
        }

        /// <summary>
        /// Convert bool to string using style.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string BoolToString(bool value, string style)
        {
            if (style == null)
                return null;
            string[] array = style.Split('/');
            if (array.Length == 2)
            {
                string yes = array[0];
                string no = array[1];
                return value ? yes : no;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Convert bool to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string BoolToString(bool value, Energy.Enumeration.BooleanStyle style)
        {
            switch (style)
            {
                case Energy.Enumeration.BooleanStyle.X: return value ? "X" : "";
                case Energy.Enumeration.BooleanStyle.V: return value ? "V" : "";
                case Energy.Enumeration.BooleanStyle.B: return value ? "1" : "0";
                case Energy.Enumeration.BooleanStyle.Y: return BoolToString(value, "Y/N");
                case Energy.Enumeration.BooleanStyle.T: return BoolToString(value, "T/F");
                case Energy.Enumeration.BooleanStyle.YesNo: return BoolToString(value, "Yes/No");
                case Energy.Enumeration.BooleanStyle.TrueFalse: return BoolToString(value, "True/False");
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region Integer

        /// <summary>
        /// Convert string to integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Integer number</returns>
        public static int StringToInteger(string value)
        {
            if (value == null || value.Length == 0)
                return 0;
            int result = 0;
            if (int.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.TrimWhite(value);
            if (trim.Length == value.Length)
                return 0;
            if (int.TryParse(value, out result))
                return result;
            return 0;
        }

        public static int StringToIntegerSmart(string value)
        {
            return StringToInteger(RemoveNumericalDifferences(value));
        }

        public static string IntegerToString(int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Convert int to short without exception.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short IntegerToShort(int value)
        {
            return (short)value;
        }

        public static byte IntegerToByte(int value)
        {
            return (byte)value;
        }

        #endregion

        #region Byte

        /// <summary>
        /// Convert string to byte value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Integer number</returns>
        public static byte StringToByte(string value)
        {
            if (value == null || value.Length == 0)
                return 0;
            byte result = 0;
            if (byte.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.TrimWhite(value);
            if (trim.Length == value.Length)
                return 0;
            if (byte.TryParse(value, out result))
                return result;
            return 0;
        }


        #endregion

        #region Long

        /// <summary>
        /// Convert string to long integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Long number</returns>
        public static long StringToLong(string value)
        {
            if (value == null || value.Length == 0)
                return 0;
            long result = 0;
            if (long.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.TrimWhite(value);
            if (trim.Length == value.Length)
                return 0;
            if (long.TryParse(value, out result))
                return result;
            return 0;
        }

        public static long StringToLongSmart(string value)
        {
            return StringToLong(RemoveNumericalDifferences(value));
        }

        /// <summary>
        /// Convert long to string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToString(long value)
        {
            return value.ToString();
        }

        #endregion

        #region Decimal

        /// <summary>
        /// Convert string to decimal value without exception
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>decimal</returns>
        public static decimal StringToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            value = value.Trim(' ', '\t', '\r', '\n', '\v', '\0');
            decimal result = 0;
            if (decimal.TryParse(value, out result))
                return result;
            return 0;
        }

        public static decimal StringToDecimalSmart(string value)
        {
            return StringToDecimal(RemoveNumericalDifferences(value));
        }

        #endregion

        #region Double

        /// <summary>
        /// Convert string to double value without exception
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>double</returns>
        public static double StringToDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            double result = 0;
            value = Energy.Base.Text.TrimWhite(value);
            if (!double.TryParse(value, System.Globalization.NumberStyles.Float
                , System.Globalization.CultureInfo.InvariantCulture
                , out result))
            {
                double.TryParse(value, System.Globalization.NumberStyles.Float
                    , System.Globalization.CultureInfo.CurrentCulture
                    , out result);
            }

            return result;
        }

        /// <summary>
        /// Smart convert string to double value without exception.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>double</returns>
        public static double StringToDoubleSmart(string value)
        {
            return StringToDouble(RemoveNumericalDifferences(value));
        }

        /// <summary>
        /// Convert floating value to invariant string
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>string</returns>
        public static string DoubleToString(double value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert numeric text to invariant string
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>string</returns>
        public static string DoubleToString(string value)
        {
            return StringToDouble(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert double value to invariant string
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="culture">InvariantCulture if null, that means 1234.56 instead of 1'234,56.</param>
        /// <returns>String</returns>
        public static string DoubleToString(double value, int precision, System.Globalization.CultureInfo culture)
        {
            // HACK: Missing "??=" operator :-)
            //culture ??= System.Globalization.CultureInfo.InvariantCulture;
            culture = culture ?? System.Globalization.CultureInfo.InvariantCulture;
            if (precision < 1)
            {
                return value.ToString(culture);
            }
            else
            {
                return value.ToString("0." + new String('0', precision), culture);
            }
        }

        /// <summary>
        /// Convert double value to invariant string
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <returns>String</returns>
        public static string DoubleToString(double value, int precision)
        {
            return DoubleToString(value, precision, null);
        }

        #endregion

        #region Float

        /// <summary>
        /// Convert string to float value without exception.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>double</returns>
        public static float StringToFloat(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            float result = 0;
            value = Energy.Base.Text.TrimWhite(value);
            if (!float.TryParse(value, System.Globalization.NumberStyles.Float
                , System.Globalization.CultureInfo.InvariantCulture
                , out result))
            {
                float.TryParse(value, System.Globalization.NumberStyles.Float
                    , System.Globalization.CultureInfo.CurrentCulture
                    , out result);
            }

            return result;
        }

        /// <summary>
        /// Smart convert string to float value without exception.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>double</returns>
        public static float StringToFloatSmart(string value)
        {
            return StringToFloat(RemoveNumericalDifferences(value));
        }

        /// <summary>
        /// Convert floating value to invariant string.
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>string</returns>
        public static string FloatToString(float value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert numeric text to invariant string.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>string</returns>
        public static string FloatToString(string value)
        {
            return StringToFloat(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert double value to invariant string.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="culture">InvariantCulture if null, that means 1234.56 instead of 1'234,56.</param>
        /// <returns>String</returns>
        public static string FloatToString(float value, int precision, System.Globalization.CultureInfo culture)
        {
            // HACK: Missing "??=" operator :-)
            //culture ??= System.Globalization.CultureInfo.InvariantCulture;
            culture = culture ?? System.Globalization.CultureInfo.InvariantCulture;
            if (precision < 1)
            {
                return value.ToString(culture);
            }
            else
            {
                return value.ToString("0." + new String('0', precision), culture);
            }
        }

        /// <summary>
        /// Convert float value to invariant string.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <returns>String</returns>
        public static string FloatToString(float value, int precision)
        {
            return FloatToString(value, precision, null);
        }

        #endregion

        #region Text

        /// <summary>
        /// Join multiline text into single string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SingleLine(string text)
        {
            return string.Join(" ", (new System.Collections.Generic.List<string>(text.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)))
                .ConvertAll<string>(delegate (string s) { return s.Trim(); }).ToArray());
        }

        #endregion

        #region DateTime

        /// <summary>
        /// Convert stamp text to DateTime.
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>DateTime</returns>
        public static DateTime StringToDateTime(string text)
        {
            return Clock.Parse(text);
        }

        /// <summary>
        /// Convert stamp text to DateTime.
        /// If stamp was not recognized null will be returned.
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>DateTime or null</returns>
        public static DateTime? StringToDateTimeSmart(string text)
        {
            DateTime d = Clock.Parse(text);
            return d != DateTime.MinValue ? d : (DateTime?)null;
        }

        /// <summary>
        /// Return DateTime as ISO 8601 string (empty if default)
        /// </summary>
        /// <param name="stamp">DateTime</param>
        /// <returns>Date and time string representation</returns>
        public static string DateTimeToString(DateTime stamp)
        {
            if (stamp == DateTime.MinValue)
                return "";
            if (stamp.Millisecond != 0)
                return stamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                return stamp.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Return DateTime as ISO 8601 string (empty if default or null)
        /// </summary>
        /// <param name="stamp">Nullable DateTime</param>
        /// <returns>Date and time string representation</returns>
        public static string DateTimeToString(DateTime? stamp)
        {
            return stamp == null ? "" : DateTimeToString((DateTime)stamp);
        }

        /// <summary>
        /// Convert DateTime to unix time
        /// </summary>
        /// <param name="stamp">DateTime</param>
        /// <returns>Unix time</returns>
        public static double DateTimeToUnixTime(DateTime stamp)
        {
            return Energy.Base.Clock.GetUnixTime(stamp);
        }

        /// <summary>
        /// Convert DateTime to unix time
        /// </summary>
        /// <param name="stamp">Nullable DateTime</param>
        /// <returns>Unix time</returns>
        public static double DateTimeToUnixTime(DateTime? stamp)
        {
            if (stamp == null)
                return 0;
            return Energy.Base.Clock.GetUnixTime((DateTime)stamp);
        }

        /// <summary>
        /// Return unix time as DateTime
        /// </summary>
        /// <param name="time">Unix time</param>
        /// <returns>DateTime</returns>
        public static DateTime UnixTimeToDateTime(double time)
        {
            return Energy.Base.Clock.GetDateTime(time);
        }

        /// <summary>
        /// Represent unix time as ISO 8601 string
        /// </summary>
        /// <param name="time">Unix time</param>
        /// <returns>String</returns>
        public static string UnixTimeToString(double time)
        {
            return DateTimeToString(Energy.Base.Clock.GetDateTime(time));
        }

        /// <summary>
        /// Convert any input to datetime. Includes ISO date, time format and unix time (UTC).
        /// </summary>
        /// <param name="input">Any object</param>
        /// <returns>DateTime.MinValue if date or time was not recognized.</returns>
        public static DateTime InputToDateTime(object input)
        {
            string value = input is string ? (string)input : input.ToString();

            if (String.IsNullOrEmpty(value) || (value = value.Trim()).Length == 0)
            {
                return DateTime.MinValue;
            }

            double unix = Energy.Base.Cast.StringToDouble(value);
            if (unix == 0)
            {
                return StringToDateTime(value);
            }
            else
            {
                return UnixTimeToDateTime(unix);
            }
        }

        #endregion

        #region Dictionary

        /// <summary>
        /// Convert string dictionary to array containing key and value pairs one by another in one dimensional array.
        /// </summary>
        /// <param name="dictionary">Dictionary&gt;string, string&lt;</param>
        /// <returns></returns>
        public static string[] StringDictionaryToStringArray(Dictionary<string, string> dictionary)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> _ in dictionary)
            {
                list.Add(_.Key);
                list.Add(_.Value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Convert string dictionary to array containing key and value pairs one by another in one dimensional array.
        /// </summary>
        /// <param name="dictionary">Dictionary&gt;string, string&lt;</param>
        /// <returns></returns>
        public static string[] LongDictionaryToStringArray(Dictionary<string, long> dictionary)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, long> _ in dictionary)
            {
                list.Add(_.Key);
                list.Add(_.Value.ToString());
            }
            return list.ToArray();
        }

        #endregion

        #region Object

        /// <summary>
        /// Represent object as string by using conversions or ToString() method
        /// </summary>
        /// <param name="value">Object instance</param>
        /// <returns>String representation</returns>
        public static string ObjectToString(object value)
        {
            // treat DBNull as empty string
            if (value == null || value is System.DBNull)
            {
                return "";
            }
            // what about bool numbers //
            if (value is bool)
            {
                return (bool)value ? "X" : "";
            }
            // convert to culture invariant form //
            if (value is double)
            {
                return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            if (value is decimal)
            {
                return ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            if (value is float)
            {
                return ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            // works with nullable version DateTime? //
            if (value is DateTime)
            {
                if (((DateTime)value).Date == DateTime.MinValue.Date)
                {
                    return ((DateTime)value).ToString("HH:mm:ss");
                }
                if (((DateTime)value).Millisecond > 0)
                {
                    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            // return default string representation //
            return value.ToString();
        }

        /// <summary>
        /// Convert object to long integer number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ObjectToLong(object value)
        {
            if (value is long)
                return (long)(long)value;
            if (value is int)
                return (long)(int)value;
            if (value is double)
                return (long)(double)value;
            if (value is uint)
                return (long)(uint)value;
            if (value is ulong)
                return (long)(ulong)value;
            if (value is decimal)
                return (long)(decimal)value;

            if (value is Int16)
                return (long)(Int16)value;
            if (value is UInt16)
                return (long)(UInt16)value;

            if (value is short)
                return (long)(short)value;
            if (value is ushort)
                return (long)(ushort)value;
            if (value is byte)
                return (long)(byte)value;
            if (value is char)
                return (long)(char)value;

            string s = null;
            if (value is string)
                s = (string)value;
            else
                s = value.ToString();

            if (s == null)
                return 0;

            long r = 0;
            double d = 0;

            if (long.TryParse(s, out r))
                return r;
            else if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return (long)d;
            else if (double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out d))
                return (long)d;

            s = s.Trim();

            if (long.TryParse(s, out r))
                return r;
            else if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return (long)d;
            else if (double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out d))
                return (long)d;

            return 0;
        }

        public static bool ObjectToBool(object value)
        {
            if (value == null)
                return false;
            if (value is int)
                return (int)value != 0;
            if (value is long)
                return (long)value != 0;
            if (value is string)
                return StringToBool((string)value);
            if (value is double)
                return (double)value != 0;
            if (value is byte)
                return (byte)value != 0;
            if (value is float)
                return (float)value != 0;
            if (value is char)
                return CharToBool((char)value);
            if (value is sbyte)
                return (sbyte)value != 0;
            if (value is double)
                return (double) value != 0;
            if (value is uint)
                return (uint)value != 0;
            if (value is ulong)
                return (ulong)value != 0;
            if (value is Int16)
                return (Int16)value != 0;
            if (value is UInt64)
                return (UInt64)value != 0;
            if (value is decimal)
                return (decimal)value != 0;
            if (value is DateTime)
                return (DateTime)value != DateTime.MinValue;
            if (value is TimeSpan)
                return (TimeSpan)value != TimeSpan.MinValue;
            if (value == DBNull.Value)
                return false;
            if (value.GetType().IsArray)
                return ((object[])value).Length > 0;

            return StringToBool(ObjectToString(value));
        }

        public static bool CharToBool(char value)
        {
            char _ = (char)value;
            return _ != '\0' && _ != '0';
        }

        public static int ObjectToInteger(object value)
        {
            return (int)ObjectToLong(value);
        }

        #endregion

        #region Enum

        /// <summary>
        /// Convert string to enum
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="type">Type</param>
        /// <returns>object</returns>
        public static object StringToEnum(string value, Type type)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            string[] names = Enum.GetNames(type);
            for (int i = 0; i < names.Length; i++)
            {
                if (String.Compare(value, names[i], true) == 0)
                {
                    return Enum.Parse(type, value, true);
                }
            }
            int length = value.Length;
            value = Energy.Base.Text.TrimWhite(value);
            if (value.Length == length)
                return 0;
            for (int i = 0; i < names.Length; i++)
            {
                if (String.Compare(value, names[i], true) == 0)
                {
                    return Enum.Parse(type, value, true);
                }
            }
            return 0;
        }

        #endregion

        #region DMS (geolocation)

        /// <summary>
        /// Return number as degrees ° minutes ' seconds "
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static string DoubleToDMS(double coordinate)
        {
            int degrees = (int)coordinate;
            double minute = (coordinate - degrees) * 60;
            double second = (minute - (int)minute) * 60;
            second = Math.Round(second, 6);
            if (second >= 60)
            {
                minute += 1;
                second -= 60;
            }
            string potato = "";
            if (Math.Abs(second) >= 0.01)
                potato = String.Concat(" "
                    , Energy.Base.Cast.DoubleToString(second, 2).TrimEnd('0').TrimEnd('.')
                    , "\"");
            return String.Concat(degrees.ToString(), "° ", ((int)minute).ToString(), "'", potato);
        }

        #endregion

        #region Base64

        /// <summary>
        /// Convert UTF-8 string to Base64 text.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringToBase64(string input)
        {
            byte[] data = ASCIIEncoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(data);
        }

        /// <summary>
        /// Convert Base64 input to UTF-8 string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64ToString(string input)
        {
            byte[] data = System.Convert.FromBase64String(input);
            return ASCIIEncoding.UTF8.GetString(data);
        }

        #endregion
    }
}
