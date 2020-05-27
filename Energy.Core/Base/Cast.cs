using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Energy.Enumeration;

namespace Energy.Base
{
    /// <summary>
    /// Conversion and type casting
    /// </summary>
    public static class Cast
    {
        #region As

        #region Generic

        /// <summary>
        /// Generic conversion from one type to another.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T As<T>(object value)
        {
            if (value == null)
            {
                return default;
            }
            Type type = typeof(T);
            return (T)As(type, value);
        }

        /// <summary>
        /// Generic conversion from one type to another.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object As(System.Type type, object value)
        {
            if (value == null)
            {
                return null;
            }

            Type r = type;
            Type t = value.GetType();

            if (t == r)
                return value;

            if (r == typeof(object))
                return (object)value;

            if (r == typeof(string))
                return ObjectToString(value);
            if (r == typeof(byte))
                return ObjectToByte(value);
            if (r == typeof(sbyte))
                return ObjectToSignedByte(value);
            if (r == typeof(char))
                return ObjectToChar(value);
            if (r == typeof(float))
                return ObjectToFloat(value);
            if (r == typeof(double))
                return ObjectToDouble(value);
            if (r == typeof(decimal))
                return ObjectToDecimal(value);
            if (r == typeof(Int16))
                return ObjectToWord(value);
            if (r == typeof(UInt16))
                return ObjectToUnsignedWord(value);
            if (r == typeof(Int32))
                return ObjectToInteger(value);
            if (r == typeof(UInt32))
                return ObjectToUnsignedInteger(value);
            if (r == typeof(Int64))
                return ObjectToLong(value);
            if (r == typeof(UInt64))
                return ObjectToUnsignedLong(value);
            if (r == typeof(bool))
                return ObjectToBool(value);
            if (r == typeof(Stream))
                return ObjectToStream(value);

            if (r.IsEnum)
                return StringToEnum(ObjectToString(value), r);

            return Energy.Base.Class.GetDefault(type);
        }

        #endregion

        #region AsInteger

        /// <summary>
        /// Convert string to integer value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Integer number</returns>
        public static int AsInteger(string value)
        {
            return Energy.Base.Cast.StringToInteger(value);
        }

        public static string TimeSpanToStringMicroseconds(object p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert string to integer value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Integer number</returns>
        public static int AsInteger(object value)
        {
            return Energy.Base.Cast.ObjectToInteger(value);
        }

        /// <summary>
        /// Convert string to unsigned integer value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Integer number</returns>
        public static uint AsUnsignedInteger(string value)
        {
            return Energy.Base.Cast.StringToUnsignedInteger(value);
        }

        /// <summary>
        /// Convert string to unsigned integer value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Integer number</returns>
        public static uint AsUnsignedInteger(object value)
        {
            return Energy.Base.Cast.ObjectToUnsignedInteger(value);
        }

        #endregion

        #region AsLong

        /// <summary>
        /// Convert string to long integer value without exception.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Long number</returns>
        public static long AsLong(string value)
        {
            return Energy.Base.Cast.StringToLong(value, Behaviour.INTEGER_COMMA);
        }

        /// <summary>
        /// Convert string to long integer value without exception
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static long AsLong(object value)
        {
            return Energy.Base.Cast.ObjectToLong(value);
        }

        /// <summary>
        /// Convert string to unsigned long integer value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static ulong AsUnsignedLong(object value)
        {
            return Energy.Base.Cast.ObjectToUnsignedLong(value);
        }

        /// <summary>
        /// Convert string to unsigned long integer value without exception.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Long number</returns>
        public static ulong AsUnsignedLong(string value)
        {
            return Energy.Base.Cast.StringToUnsignedLong(value, Energy.Base.Cast.Behaviour.INTEGER_COMMA);
        }

        #endregion

        #region AsDecimal

        /// <summary>
        /// Convert string to decimal value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static decimal AsDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
            string stringValue = "";
            if (value is string)
                stringValue = (string)value;
            else
                stringValue = value.ToString();
            return StringToDecimal((string)value);
        }

        #endregion

        #region AsDouble

        /// <summary>
        /// Convert string to double value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static double AsDouble(object value)
        {
            return ObjectToDouble(value);
        }

        /// <summary>
        /// Convert string to double value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static double AsDouble(string value)
        {
            return StringToDouble(value);
        }

        #endregion

        #region AsFloat

        /// <summary>
        /// Convert string to float value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static double AsFloat(object value)
        {
            return ObjectToFloat(value);
        }

        /// <summary>
        /// Convert string to float value without exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static double AsFloat(string value)
        {
            return StringToFloat(value);
        }

        #endregion

        #region AsBool

        /// <summary>
        /// Convert string to boolean value without exception
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AsBool(string value)
        {
            return Energy.Base.Cast.StringToBool(value);
        }

        /// <summary>
        /// Convert object to boolean value without exception
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AsBool(object value)
        {
            return Energy.Base.Cast.ObjectToBool(value);
        }

        #endregion

        #region AsString

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
        /// Return as string using ObjectToString method.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string AsString(object o)
        {
            return ObjectToString(o);
        }

        #endregion

        #region AsDateTime

        /// <summary>
        /// Return as DateTime using StringToDateTime method.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(string text)
        {
            return StringToDateTime(text);
        }

        /// <summary>
        /// Return as DateTime using ObjectToDateTime method.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(object o)
        {
            return ObjectToDateTime(o);
        }

        /// <summary>
        /// Return as DateTime value using StringToDateTime method.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(string text, DateTime minimum, DateTime maximum)
        {
            return StringToDateTime(text, minimum, maximum);
        }

        /// <summary>
        /// Return as DateTime value using ObjectToDateTime method.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(object o, DateTime minimum, DateTime maximum)
        {
            return ObjectToDateTime(o, minimum, maximum);
        }

        #endregion

        #region AsEnum

        /// <summary>
        /// Convert string to enum
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>object</returns>
        public static T AsEnum<T>(string value)
        {
            return (T)Energy.Base.Cast.StringToEnum(value, typeof(T));
        }

        #endregion

        #endregion

        #region Behaviour

        /// <summary>
        /// Behaviour settings
        /// </summary>
        public static class Behaviour
        {
            #region Constant

            /// <summary>
            /// Allow to use decimal point in integer numbers when converting
            /// </summary>
            public const bool INTEGER_COMMA = true;

            public const bool INTEGER_EXCEED = false;

            /// <summary>
            /// Allow to use decimal point in integer numbers when converting
            /// </summary>
            public const bool BYTE_COMMA = true;

            public const NumberStyles DECIMAL_NUMBER_STYLES = NumberStyles.Float;

            public readonly static string DOUBLE_MIN_STRING = double.MinValue.ToString(CultureInfo.InvariantCulture);

            public readonly static string DOUBLE_MAX_STRING = double.MaxValue.ToString(CultureInfo.InvariantCulture);

            public readonly static string DOUBLE_MAX_STRING_PLUS = "+" + DOUBLE_MAX_STRING;

            public readonly static string DECIMAL_MIN_STRING = decimal.MinValue.ToString(CultureInfo.InvariantCulture);

            public readonly static string DECIMAL_MAX_STRING = decimal.MaxValue.ToString(CultureInfo.InvariantCulture);

            public readonly static string DECIMAL_MAX_STRING_PLUS = "+" + DECIMAL_MAX_STRING;

            public const string DOUBLE_STRING_FORMAT_G17 = "G17";

            public static string DOUBLE_STRING_FORMAT = "G17";  // this one is G17 with special treatment. Contains workaround for 61.3 being represented as 61.299999999999997.
            //public readonly static string DOUBLE_STRING_FORMAT = "G18"; // wrong for 61.3. Actual:<61.299999999999997>.
            //public readonly static string DOUBLE_STRING_FORMAT = "G16"; // wrong for -1234567890.0987654. Actual:<-1234567890.098765>.
            //public readonly static string DOUBLE_STRING_FORMAT = "";     // fall back to bad code which corrupt values by rounding. Tests will not pass. Expected:<-1234567890.0987654>. Actual:<-1234567890.09877>.
            //public readonly static string DOUBLE_STRING_FORMAT = "G";  // wrong for -1234567890.0987654. Actual:<-1234567890.09877>.
            //public readonly static string DOUBLE_STRING_FORMAT = "$G17";  // wrong for -1234567890.0987654. Actual:<-1234567890.09877>.

            public static string SINGLE_STRING_FORMAT = "G9";

            public static readonly string DATETIME_FORMAT_DEFAULT_MILLISECOND = "yyyy-MM-dd HH:mm:ss.fff";

            public static readonly string DATETIME_FORMAT_DEFAULT_MICROSECOND = "yyyy-MM-dd HH:mm:ss.ffffff";

            public static readonly string DATETIME_FORMAT_DEFAULT_TICK = "yyyy-MM-dd HH:mm:ss.fffffff";

            public static readonly string DATETIME_FORMAT_DEFAULT_SECOND = "yyyy-MM-dd HH:mm:ss";

            public static readonly string DATETIME_FORMAT_DEFAULT_DATE = "yyyy-MM-dd";

            #endregion
        }

        #endregion

        #region RemoveNumericalDifferences

        /// <summary>
        /// Remove numerical differences from text representation of number.
        /// Treat comma "," the same as dot "." as decimal point.
        /// Ignore space, underscore and apostrophes between digits.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string RemoveNumericalDifferences(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.Trim();
            if (value.Length == 0)
                return value;

            if (value.IndexOf(' ') >= 0)
                value = value.Replace(" ", null);
            if (value.IndexOf('_') >= 0)
                value = value.Replace("_", null);
            if (value.IndexOf('\'') >= 0)
                value = value.Replace("'", null);
            if (value.IndexOf('’') >= 0)
                value = value.Replace("’", null);

            if (value.IndexOf(',') >= 0)
                value = value.Replace(',', '.');

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
            {
                return false;
            }
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
        /// Convert bool to string. Returns "1" for true and "0" for false.
        /// </summary>
        /// <param name="value">bool</param>
        /// <returns>string</returns>
        public static string BoolToString(bool value)
        {
            return value ? "1" : "0";
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
        /// Convert boolean value to its string representation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string BoolToString(bool value, Energy.Enumeration.BooleanStyle style)
        {
            switch (style)
            {
                case Energy.Enumeration.BooleanStyle.X: return value ? "X" : " ";
                case Energy.Enumeration.BooleanStyle.V: return value ? "V" : " ";
                case Energy.Enumeration.BooleanStyle.B: return value ? "1" : "0";
                case Energy.Enumeration.BooleanStyle.Y: return BoolToString(value, "Y/N");
                case Energy.Enumeration.BooleanStyle.T: return BoolToString(value, "T/F");
                case Energy.Enumeration.BooleanStyle.YesNo: return BoolToString(value, "Yes/No");
                case Energy.Enumeration.BooleanStyle.TrueFalse: return BoolToString(value, "True/False");
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Convert bool to string.
        /// </summary>
        /// <param name="value">bool</param>
        /// <returns>string</returns>
        public static string BoolToString(bool? value)
        {
            return value == null ? BoolToString(false) : BoolToString((bool)value);
        }

        /// <summary>
        /// Convert bool to string using style.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string BoolToString(bool? value, string style)
        {
            return value == null ? BoolToString(false, style) : BoolToString((bool)value, style);
        }

        /// <summary>
        /// Convert bool to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string BoolToString(bool? value, Energy.Enumeration.BooleanStyle style)
        {
            return value == null ? BoolToString(false, style) : BoolToString((bool)value, style);

        }

        #endregion

        #region Char

        /// <summary>
        /// Get first character from a string without exception.
        /// If value is null or empty string, function will result '\0'. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static char StringToChar(string value)
        {
            return string.IsNullOrEmpty(value) ? '\0' : value[0];
        }

        /// <summary>
        /// Get character at specified position from a string without exception.
        /// If value is null or empty string, function will result '\0'. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static char StringToChar(string value, int position)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= position)
                return '\0';
            else
                return value[position];
        }

        #endregion

        #region Number

        /// <summary>
        /// Represent number string as prefixed with positive sign and empty on zero.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NumberToStringSign(string value)
        {
            return NumberToStringSign(value, null);
        }

        /// <summary>
        /// Represent number string as prefixed with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string NumberToStringSign(string value, string sign)
        {
            if (string.IsNullOrEmpty(sign))
                sign = "+";

            if (string.IsNullOrEmpty(value))
                value = "0";

            bool isZero = value == "0";

            if (!isZero)
            {
                if (value[0] == '0' && value.Length > 1 
                    && (value[1] == '.' || value[1] == ',')
                    )
                {
                    bool allZeroes = true;
                    for (int i = 2; i < value.Length; i++)
                    {
                        if (value[i] == 'e' || value[i] == 'E')
                            break;
                        if (value[i] != '0')
                        {
                            allZeroes = false;
                            break;
                        }
                    }
                    if (allZeroes)
                        isZero = true;
                }
            }

            if (isZero)
            {
                char signZero = sign.Length <= 2 ? '\0' : sign[2];
                if (signZero == '\0')
                    signZero = ' ';
                return string.Concat(signZero, value);
            }
            else if (value.StartsWith("-"))
            {
                char signNegative = sign.Length <= 1 ? '\0' : sign[1];
                if (signNegative == '\0' || signNegative == '-')
                    return value;
                else
                    return string.Concat(signNegative, value.Substring(1));
            }
            else
            {
                char signPositive = sign[0];
                return string.Concat(signPositive, value);
            }
        }

        #endregion

        #region Integer

        /// <summary>
        /// Convert text to signed 32-bit integer number without exception
        /// ignoring leading and trailing whitespace characters.
        /// <br/><br/>
        /// If conversion cannot be performed, default value 0 is returned.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int StringToInt(string text)
        {
            text = Energy.Base.Text.Trim(text);
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            if (!int.TryParse(text, out int result))
            {
                return 0;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Convert string to integer value without exception.
        /// Allows to convert floating point values resulting in decimal part.
        /// Treat comma "," the same as dot "." as decimal point.
        /// Returns zero on overflow.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Integer number</returns>
        public static int StringToInteger(string value)
        {
            return StringToInteger(value, true);
        }

        /// <summary>
        /// Convert string to integer value without exception.
        /// Allows to convert floating point values resulting in decimal part.
        /// Treat comma "," the same as dot "." as decimal point.
        /// Returns zero on overflow.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="allowDecimal">Allow decimal numbers</param>
        /// <returns>Integer number</returns>
        public static int StringToInteger(string value, bool allowDecimal)
        {
            if (null == value || 0 == value.Length)
                return 0;
            int result;
            if (int.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.Trim(value);
            if (trim.Length != value.Length)
                if (int.TryParse(value, out result))
                    return result;
            if (!allowDecimal)
                return 0;
            if (value.IndexOf(',') >= 0)
                value = value.Replace(',', '.');
            decimal number = 0;
            if (decimal.TryParse(value, out number))
                if (number < int.MinValue || number > int.MaxValue)
                    return 0;
                else
                    return (int)number;
            return 0;
        }

        /// <summary>
        /// Convert string to integer value removing numerical differences without exception.
        /// Allows to convert floating point values resulting in decimal part.
        /// Treat comma "," the same as dot "." as decimal point.
        /// Returns zero on overflow.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Integer number</returns>
        public static int StringToIntegerSmart(string value)
        {
            return StringToInteger(RemoveNumericalDifferences(value), true);
        }

        /// <summary>
        /// Convert string to long integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Long number</returns>
        public static uint StringToUnsignedInteger(string value)
        {
            long x = StringToLong(value, true);
            if (x < uint.MinValue || x > uint.MaxValue)
                return 0;
            else
                return (uint)x;
        }

        /// <summary>
        /// Represent integer number as text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntegerToString(int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent integer number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntegerToStringSign(int value)
        {
            return NumberToStringSign(value.ToString(), null);
        }

        /// <summary>
        /// Represent integer number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string IntegerToStringSign(int value, string sign)
        {
            return NumberToStringSign(value.ToString(), sign);
        }

        /// <summary>
        /// Convert int to short without exception.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short IntegerToShort(int value)
        {
            if (value < short.MinValue || value > short.MaxValue)
                return 0;
            return (short)value;
        }

        public static byte IntegerToByte(int value)
        {
            if (value < byte.MinValue || value > byte.MaxValue)
                return 0;
            return (byte)value;
        }

        /// <summary>
        /// Convert integer to hexadecimal value.
        /// 
        /// Resulting string will have always 8 digits or letters (A-F).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntegerToHex(int value)
        {
            return Energy.Base.Hex.IntegerToHex(value);
        }

        /// <summary>
        /// Convert integer to hexadecimal value.
        /// 
        /// Resulting string will have count specified by size of digits or letters (A-F).
        /// If number representation will be larger than size, it will be truncated to the last characters.
        /// Example: IntegerToHex(100000, 4) will result with "86a0" instead of "186a0" or "186a".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string IntegerToHex(int value, int size)
        {
            return Energy.Base.Hex.IntegerToHex(value, size);
        }

        public static string IntegerToHex(int value, int size, bool upperCase)
        {
            return Energy.Base.Hex.IntegerToHex(value, size, upperCase);
        }

        public static string IntegerToHex(int value, bool upperCase)
        {
            return Energy.Base.Hex.IntegerToHex(value, upperCase);
        }

        /// <summary>
        /// Convert hexadecimal string to integer value (System.Int32).
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToInteger(string hex)
        {
            return Energy.Base.Hex.HexToInteger(hex);
        }

        /// <summary>
        /// Convert octal string to integer value (System.Int32).
        /// </summary>
        /// <param name="oct"></param>
        /// <returns></returns>
        public static int OctToInteger(string oct)
        {
            if (!Energy.Base.Text.HasDigitsOnly(oct))
                return 0;
            try
            {
                return Convert.ToInt32(oct, 8);
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Catch(exception);
            }
            return 0;
        }

        #endregion

        #region Long

        /// <summary>
        /// Convert string to long integer value without exception.
        /// Allows decimal numbers by default.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Long number</returns>
        public static long StringToLong(string value)
        {
            return StringToLong(value, Energy.Base.Cast.Behaviour.INTEGER_COMMA);
        }

        /// <summary>
        /// Convert string to long integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="allowDecimal">Allow decimal numbers</param>
        /// <returns>Long number</returns>
        public static long StringToLong(string value, bool allowDecimal)
        {
            if (value == null || value.Length == 0)
                return 0;
            long result = 0;
            if (long.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.Trim(value);
            if (trim.Length != value.Length)
                if (long.TryParse(value, out result))
                    return result;
            if (!allowDecimal)
                return 0;
            if (value.IndexOf(',') >= 0)
                value = value.Replace(',', '.');
            decimal number = 0;
            if (decimal.TryParse(value, out number))
                if (number < long.MinValue || number > long.MaxValue)
                    return 0;
                else
                    return (long)number;
            return 0;
        }

        /// <summary>
        /// Convert string to long integer value without exception.
        /// Allows decimal numbers by default.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Long number</returns>
        public static ulong StringToUnsignedLong(string value)
        {
            return StringToUnsignedLong(value, Energy.Base.Cast.Behaviour.INTEGER_COMMA);
        }

        /// <summary>
        /// Convert string to long integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="allowDecimal">Allow decimal number</param>
        /// <returns>Long number</returns>
        public static ulong StringToUnsignedLong(string value, bool allowDecimal)
        {
            if (value == null || value.Length == 0)
                return 0;
            ulong result = 0;
            if (ulong.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.Trim(value);
            if (trim.Length != value.Length)
                if (ulong.TryParse(value, out result))
                    return result;
            if (!allowDecimal)
                return 0;
            if (value.IndexOf(',') >= 0)
                value = value.Replace(',', '.');
            decimal number = 0;
            if (decimal.TryParse(value, out number))
                if (number < 0 || number > ulong.MaxValue)
                    return 0;
                else
                    return (ulong)number;
            return 0;
        }

        /// <summary>
        /// Convert string to long integer value without exception 
        /// removing numerical differences.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Long number</returns>
        public static long StringToLongSmart(string value)
        {
            return StringToLong(RemoveNumericalDifferences(value), true);
        }

        /// <summary>
        /// Convert string to unsigned long integer value without exception 
        /// removing numerical differences.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Long number</returns>
        public static ulong StringToUsignedLongSmart(string value)
        {
            return StringToUnsignedLong(RemoveNumericalDifferences(value), true);
        }

        /// <summary>
        /// Check if value is long number with or without negative minus sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="negative"></param>
        /// <returns></returns>
        public static bool IsLong(string value, bool negative)
        {
            if (negative)
            {
                long _;
                return long.TryParse(value, out _);
            }
            else
            {
                ulong _;
                return ulong.TryParse(value, out _);
            }
        }

        /// <summary>
        /// Represent long number as text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToString(long value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent long number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToStringSign(long value)
        {
            return NumberToStringSign(value.ToString(), null);
        }

        /// <summary>
        /// Represent long number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string LongToStringSign(long value, string sign)
        {
            return NumberToStringSign(value.ToString(), sign);
        }

        /// <summary>
        /// Represent unsigned number as text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnsignedLongToString(ulong value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent unsigned number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnsignedLongToStringSign(ulong value)
        {
            return NumberToStringSign(value.ToString(), null);
        }

        /// <summary>
        /// Represent unsigned long number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string UnsignedLongToStringSign(ulong value, string sign)
        {
            return NumberToStringSign(value.ToString(), sign);
        }

        #endregion

        #region Short

        /// <summary>
        /// Convert string to short integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="point">Allow decimal point in numbers</param>
        /// <param name="exceed">Allow value to exceed maximum (reminder will be returned)</param>
        /// <returns>Short number</returns>
        public static short StringToShort(string value, bool point, bool exceed)
        {
            if (value == null || value.Length == 0)
                return 0;
            short result = 0;
            if (short.TryParse(value, out result))
            {
                return result;
            }
            string trim = Energy.Base.Text.Trim(value);
            if (trim.Length != value.Length)
            {
                if (short.TryParse(value, out result))
                {
                    return result;
                }
            }
            if (exceed)
            {
                long _;
                if (long.TryParse(value, out _))
                {
                    return (short)(_ % (1 + short.MaxValue));
                }
            }
            if (!point)
            {
                return 0;
            }
            if (value.IndexOf(',') >= 0)
            {
                value = value.Replace(',', '.');
            }
            decimal number;
            if (decimal.TryParse(value, out number))
            {
                if (number < short.MinValue || number > short.MaxValue)
                {
                    if (exceed)
                    {
                        return (short)(number % (1 + short.MaxValue));
                    }
                    return 0;
                }
                else
                {
                    return (short)number;
                }
            }
            return 0;
        }

        /// <summary>
        /// Convert string to short integer value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="point">Allow decimal point in numbers</param>
        /// <param name="exceed">Allow value to exceed maximum (reminder will be returned)</param>
        /// <returns>Short number</returns>
        public static ushort StringToUnsignedShort(string value, bool point, bool exceed)
        {
            if (value == null || value.Length == 0)
            {
                return 0;
            }
            ushort result = 0;
            if (ushort.TryParse(value, out result))
            {
                return result;
            }
            string trim = Energy.Base.Text.Trim(value);
            if (0 == trim.Length)
            {
                return 0;
            }
            if (trim.Length != value.Length)
            {
                if (ushort.TryParse(value, out result))
                {
                    return result;
                }
            }
            if (exceed)
            {
                if (value.StartsWith("-"))
                {
                    long _;
                    if (long.TryParse(value, out _))
                    {
                        return (ushort)(-_ % (1 + ushort.MaxValue));
                    }
                }
                else
                {
                    ulong _;
                    if (ulong.TryParse(value, out _))
                    {
                        return (ushort)(_ % (1 + ushort.MaxValue));
                    }
                }
            }
            if (!point)
            {
                return 0;
            }
            if (value.IndexOf(',') >= 0)
            {
                value = value.Replace(',', '.');
            }
            decimal number;
            if (decimal.TryParse(value, out number))
            {
                if (false)
                { }
                else if (number < 0)
                {
                    if (exceed)
                    {
                        return (ushort)(-number % (1 + ushort.MaxValue));
                    }
                    return 0;
                }
                else if (number > ushort.MaxValue)
                {
                    if (exceed)
                    {
                        return (ushort)(number % (1 + ushort.MaxValue));
                    }
                    return 0;
                }
                else
                {
                    return (ushort)number;
                }
            }
            return 0;
        }

        /// <summary>
        /// Convert string to short integer value without exception.
        /// Allows decimal numbers by default.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Short number</returns>
        public static short StringToShort(string value)
        {
            return StringToShort(value
                , Energy.Base.Cast.Behaviour.INTEGER_COMMA
                , Energy.Base.Cast.Behaviour.INTEGER_EXCEED
                );
        }

        /// <summary>
        /// Convert string to short integer value without exception.
        /// Allows decimal numbers by default.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Short number</returns>
        public static ushort StringToUnsignedShort(string value)
        {
            return StringToUnsignedShort(value
                , Energy.Base.Cast.Behaviour.INTEGER_COMMA
                , Energy.Base.Cast.Behaviour.INTEGER_EXCEED
                );
        }

        /// <summary>
        /// Convert string to short integer value without exception 
        /// removing numerical differences.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Short number</returns>
        public static short StringToShortSmart(string value)
        {
            return StringToShort(RemoveNumericalDifferences(value)
                , Energy.Base.Cast.Behaviour.INTEGER_COMMA
                , Energy.Base.Cast.Behaviour.INTEGER_EXCEED
                );
        }

        /// <summary>
        /// Convert string to unsigned short integer value without exception 
        /// removing numerical differences.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Short number</returns>
        public static ushort StringToUsignedShortSmart(string value)
        {
            return StringToUnsignedShort(RemoveNumericalDifferences(value)
                , Energy.Base.Cast.Behaviour.INTEGER_COMMA
                , Energy.Base.Cast.Behaviour.INTEGER_EXCEED
                );
        }

        /// <summary>
        /// Represent short number as text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShortToString(short value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent unsigned number as text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnsignedShortToString(ushort value)
        {
            return value.ToString();
        }

        #endregion

        #region Stream

        /// <summary>
        /// Convert string to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Stream StringToStream(string value)
        {
            if (value == null)
                value = string.Empty;
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
            return stream;
        }

        /// <summary>
        /// Convert string to a stream using specified encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Stream StringToStream(string value, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (value == null)
                value = string.Empty;
            MemoryStream stream = new MemoryStream(encoding.GetBytes(value));
            return stream;
        }

        /// <summary>
        /// Read data from a stream until limit is reached.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static byte[] StreamRead(Stream stream, int limit)
        {
            byte[] buffer = new byte[8 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int count;
                while (0 < (count = stream.Read(buffer, 0, limit <= 0 || limit > buffer.Length ? buffer.Length : limit)))
                {
                    ms.Write(buffer, 0, count);
                    if (limit > 0)
                    {
                        limit -= count;
                    }
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Read all data from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamRead(Stream stream)
        {
            byte[] buffer = new byte[8 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int count;
                while (0 < (count = stream.Read(buffer, 0, buffer.Length)))
                {
                    ms.Write(buffer, 0, count);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Return string from a stream using specified encoding.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string StreamToString(Stream stream, Encoding encoding)
        {
            if (null == encoding)
            {
                encoding = Encoding.UTF8;
            }
            byte[] data = StreamRead(stream);
            if (null == data)
            {
                return null;
            }
            if (0 == data.Length)
            {
                return "";
            }
            else
            {
                return encoding.GetString(data);
            }
        }

        /// <summary>
        /// Return string from a stream using UTF-8 encoding.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string StreamToString(Stream stream)
        {
            Encoding encoding = System.Text.Encoding.UTF8;
            byte[] data = StreamRead(stream);
            if (null == data)
            {
                return null;
            }
            if (0 == data.Length)
            {
                return "";
            }
            else
            {
                return encoding.GetString(data);
            }
        }

        #endregion

        #region Decimal

        /// <summary>
        /// Convert string to decimal value without exception.
        /// Treat comma "," the same as dot "." as decimal point.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>decimal</returns>
        public static decimal StringToDecimal(string value)
        {
            return StringToDecimal(value, Energy.Base.Cast.Behaviour.DECIMAL_NUMBER_STYLES);
        }

        /// <summary>
        /// Convert string to decimal value without exception.
        /// Treat comma "," the same as dot "." as decimal point.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="numberStyles"></param>
        /// <returns>decimal</returns>
        public static decimal StringToDecimal(string value, NumberStyles numberStyles)
        {
            if (null == value || 0 == value.Length)
                return 0;
            decimal result;
            if (decimal.TryParse(value, numberStyles, System.Globalization.CultureInfo.InvariantCulture, out result))
                return result;
            string trim = Energy.Base.Text.Trim(value);
            bool hasComma = value.IndexOf(',') >= 0;
            if (trim.Length == value.Length && !hasComma)
                return 0;
            if (hasComma)
                value = value.Replace(',', '.');
            if (decimal.TryParse(value, numberStyles, System.Globalization.CultureInfo.InvariantCulture, out result))
                return result;
            double _double = 0;
            if (double.TryParse(value, numberStyles, System.Globalization.CultureInfo.InvariantCulture, out _double))
            {
                if (_double < (double)decimal.MinValue)
                    return decimal.MinValue;
                if (_double > (double)decimal.MaxValue)
                    return decimal.MaxValue;
                else
                    return 0;
            }
            return 0;
        }

        /// <summary>
        /// Convert string to decimal value without exception.
        /// Remove numerical differences from text representation of number.
        /// Treat comma "," the same as dot "." as decimal point.
        /// Ignore space, underscore and apostrophes between digits.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal StringToDecimalSmart(string value)
        {
            return StringToDecimal(RemoveNumericalDifferences(value));
        }

        /// <summary>
        /// Represent decimal number as text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimalToString(decimal value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent decimal number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimalToStringSign(decimal value)
        {
            return NumberToStringSign(DecimalToString(value), null);
        }

        /// <summary>
        /// Represent decimal number as text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string DecimalToStringSign(decimal value, string sign)
        {
            return NumberToStringSign(DecimalToString(value), sign);
        }

        #endregion

        #region Double

        /// <summary>
        /// Convert string to double value without exception.
        /// Treat comma "," the same as dot "." as decimal point.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>double</returns>
        public static double StringToDouble(string value)
        {
            return StringToDouble(value, Energy.Base.Cast.Behaviour.DECIMAL_NUMBER_STYLES);
        }

        /// <summary>
        /// Convert string to double value without exception.
        /// Treat comma "," the same as dot "." as decimal point.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="numberStyles"></param>
        /// <returns>double</returns>
        public static double StringToDouble(string value, NumberStyles numberStyles)
        {
            value = Energy.Base.Text.Trim(value);
            if (null == value || 0 == value.Length)
            {
                return 0;
            }
            if (value.IndexOf(',') >= 0)
            {
                value = value.Replace(',', '.');
            }
            double result;
            if (double.TryParse(value, numberStyles, System.Globalization.CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            if (0 == string.Compare(Energy.Base.Cast.Behaviour.DOUBLE_MIN_STRING, value, true))
                return double.MinValue;
            if (0 == string.Compare(Energy.Base.Cast.Behaviour.DOUBLE_MAX_STRING, value, true))
                return double.MaxValue;
            if (0 == string.Compare(Energy.Base.Cast.Behaviour.DOUBLE_MAX_STRING_PLUS, value, true))
                return double.MaxValue;
            return 0;
        }

        /// <summary>
        /// Convert string to double value without exception.
        /// Remove numerical differences from text representation of number.
        /// Treat comma "," the same as dot "." as decimal point.
        /// Ignore space, underscore and apostrophes between digits.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double StringToDoubleSmart(string value)
        {
            return StringToDouble(RemoveNumericalDifferences(value));
        }

        /// <summary>
        /// Convert double value to invariant string
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="trim">Trim trailing zeros from fractional part</param>
        /// <param name="culture">InvariantCulture if null, that means 1234.56 instead of 1'234,56.</param>
        /// <returns>String</returns>
        public static string DoubleToString(double value, int precision, bool trim
            , System.Globalization.CultureInfo culture)
        {
            // HACK: Missing "??=" operator :-)
            //culture ??= System.Globalization.CultureInfo.InvariantCulture;
            culture = culture ?? System.Globalization.CultureInfo.InvariantCulture;
            if (precision < 0)
            {
                return value.ToString(culture);
            }
            else
            {
                string result = value.ToString(Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT, culture);
                string decimalPoint = culture.NumberFormat.NumberDecimalSeparator;
                int exponent = result.IndexOfAny(new char[] { 'E', 'e' });
                string exponentString = null;
                if (exponent > 0)
                {
                    exponentString = result.Substring(exponent);
                    result = result.Substring(0, exponent);
                    if (result.Length == 0 || result == "-")
                        result = result + "0.0";
                }
                int point = result.IndexOf(decimalPoint);
                if (point < 0)
                {
                    result += ".0";
                    point = result.IndexOf(decimalPoint);
                }
                int length = 1 + point + precision;
                if (trim)
                {
                    if (result.EndsWith("0"))
                    {
                        int original = result.Length;
                        result = result.TrimEnd('0');
                    }
                }
                else
                {
                    if (result.Length < length)
                        result = result.PadRight(length, '0');
                }
                if (result.Length > length)
                    result = result.Substring(0, length);
                if (result.EndsWith(decimalPoint))
                    result = result.Substring(0, result.Length - decimalPoint.Length);
                if (exponentString != null)
                    result = result + exponentString;
                return result;
            }
        }

        /// <summary>
        /// Convert floating value to invariant string
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>string</returns>
        public static string DoubleToString(double value)
        {
            if (Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT 
                == Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT_G17)
            {
                string s17 = value.ToString("G17", CultureInfo.InvariantCulture);
                int dot = s17.IndexOf(".");
                if (0 > dot)
                {
                    return s17;
                }
                if (s17.Length - dot > 10)
                {
                    string s16 = value.ToString("G16", CultureInfo.InvariantCulture);
                    if (s17.Length - s16.Length > 10)
                    {
                        Energy.Core.Bug.Write("Energy.Base.Cast.DoubleToString"
                            , delegate () {
                                return string.Format("Choosing {0} over {1}", s16, s17);
                            });
                        return s16;
                    }
                }
                return s17;
            }
            if (string.IsNullOrEmpty(Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT))
            {
                return value.ToString(CultureInfo.InvariantCulture);
            }
            else if ("$" == Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT)
            {
                decimal cast = (decimal)value;
                return cast.ToString(CultureInfo.InvariantCulture);
            }
            else if (Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT.StartsWith("$"))
            {
                decimal cast = (decimal)value;
                return cast.ToString(Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT.Substring(1)
                    , CultureInfo.InvariantCulture);
            }
            else
            {
                return value.ToString(Energy.Base.Cast.Behaviour.DOUBLE_STRING_FORMAT, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Convert numeric text to invariant string
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>string</returns>
        public static string DoubleToString(string value)
        {
            return DoubleToString(StringToDouble(value));
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
            return DoubleToString(value, precision, false, culture);
        }

        /// <summary>
        /// Convert double value to invariant string
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <returns>String</returns>
        public static string DoubleToString(double value, int precision)
        {
            return DoubleToString(value, precision, false, null);
        }

        /// <summary>
        /// Convert double value to invariant string.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="trim">Trim zeroes from end</param>
        /// <returns>String</returns>
        public static string DoubleToString(double value, int precision, bool trim)
        {
            return DoubleToString(value, precision, trim, CultureInfo.InvariantCulture);
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
            double number = StringToDouble(value);
            if (number < float.MinValue)
                return float.MinValue;
            else if (number > float.MaxValue)
                return float.MaxValue;
            else
                return (float)number;
        }

        /// <summary>
        /// Convert string to float value without exception.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="numberStyles"></param>
        /// <returns>double</returns>
        public static float StringToFloat(string value, NumberStyles numberStyles)
        {
            double number = StringToDouble(value, numberStyles);
            if (number < float.MinValue || number > float.MaxValue)
                return 0;
            else
                return (float)number;
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

        #region Byte

        /// <summary>
        /// Convert string to byte value without exception.
        /// Allows decimal numbers by default.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Integer number</returns>
        public static byte StringToByte(string value)
        {
            return StringToByte(value, Energy.Base.Cast.Behaviour.BYTE_COMMA);
        }

        /// <summary>
        /// Convert string to byte value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="allowDecimal">Allow decimal numbers</param>
        /// <returns>Integer number</returns>
        public static byte StringToByte(string value, bool allowDecimal)
        {
            if (value == null || value.Length == 0)
                return 0;
            byte result = 0;
            if (byte.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.Trim(value);
            if (trim.Length != value.Length)
            {
                if (byte.TryParse(value, out result))
                    return result;
            }
            if (!allowDecimal)
                return 0;
            if (value.IndexOf(',') >= 0)
                value = value.Replace(',', '.');
            decimal number = 0;
            if (decimal.TryParse(value, out number))
            {
                if (number < byte.MinValue || number > byte.MaxValue)
                    return 0;
                else
                    return (byte)number;
            }
            return 0;
        }


        /// <summary>
        /// Convert string to byte value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Integer number</returns>
        public static sbyte StringToSignedByte(string value)
        {
            return StringToSignedByte(value, Energy.Base.Cast.Behaviour.BYTE_COMMA);
        }

        /// <summary>
        /// Convert string to byte value without exception.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="allowDecimal">Allow decimal numbers</param>
        /// <returns>Integer number</returns>
        public static sbyte StringToSignedByte(string value, bool allowDecimal)
        {
            if (value == null || value.Length == 0)
                return 0;
            sbyte result = 0;
            if (sbyte.TryParse(value, out result))
                return result;
            string trim = Energy.Base.Text.Trim(value);
            if (trim.Length != value.Length)
                if (sbyte.TryParse(value, out result))
                    return result;
            if (!allowDecimal)
                return 0;
            if (value.IndexOf(',') >= 0)
                value = value.Replace(',', '.');
            decimal number = 0;
            if (decimal.TryParse(value, out number))
                if (number < sbyte.MinValue || number > sbyte.MaxValue)
                    return 0;
                else
                    return (sbyte)number;
            return 0;
        }

        /// <summary>
        /// Represent byte as decimal text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ByteToString(byte value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent byte as decimal text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ByteToStringSign(byte value)
        {
            return NumberToStringSign(value.ToString(), null);
        }

        /// <summary>
        /// Represent byte as decimal text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string ByteToStringSign(byte value, string sign)
        {
            return NumberToStringSign(value.ToString(), sign);
        }

        /// <summary>
        /// Represent signed byte as decimal text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SignedByteToString(sbyte value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Represent signed byte as decimal text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SignedByteToStringSign(sbyte value)
        {
            return NumberToStringSign(value.ToString(), null);
        }

        /// <summary>
        /// Represent signed byte as decimal text with positive sign.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string SignedByteToStringSign(sbyte value, string sign)
        {
            return NumberToStringSign(value.ToString(), sign);
        }

        #endregion

        #region Bcd

        /// <summary>
        /// Convert byte to BCD value
        /// </summary>
        /// <param name="value">Byte value</param>
        /// <returns>BCD value</returns>
        public static byte ByteToBcd(byte value)
        {
            return Energy.Base.Bcd.FromByte(value);
        }

        /// <summary>
        /// Convert word to BCD value
        /// </summary>
        /// <param name="value">Byte value</param>
        /// <returns>BCD value</returns>
        public static ushort WordToBcd(ushort value)
        {
            return Energy.Base.Bcd.FromWord(value);
        }

        /// <summary>
        /// Convert BCD value to byte
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Word value</returns>
        public static ushort BcdToByte(byte value)
        {
            return Energy.Base.Bcd.ToByte(value);
        }

        /// <summary>
        /// Convert BCD value to word
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Word value</returns>
        public static ushort BcdToWord(ushort value)
        {
            return Energy.Base.Bcd.ToWord(value);
        }

        #endregion

        #region Text

        /// <summary>
        /// Join multiline text into single string.
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
        /// Convert date and time text to DateTime value.
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>DateTime</returns>
        public static DateTime StringToDateTime(string text)
        {
            return Clock.Parse(text);
        }

        /// <summary>
        /// Convert date and time text to DateTime value within range.
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns>DateTime</returns>
        public static DateTime StringToDateTime(string text, DateTime minimum, DateTime maximum)
        {
            DateTime value = Clock.Parse(text);
            if (minimum != DateTime.MinValue && value < minimum)
            {
                value = minimum;
            }
            else if (maximum != DateTime.MaxValue && value > maximum)
            {
                value = maximum;
            }
            return value;
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
        /// Return DateTime value as text with custom format and empty value. 
        /// Optional list of "empty" DateTime values may be specified.
        /// </summary>
        /// <param name="stamp">Nullable DateTime</param>
        /// <param name="customTimeFormat">Custom format text, like "yyyy-MM-dd HH:mm:ss.fff"</param>
        /// <param name="customDateFormat">Custom format text for date, if time part is midnight, like "yyyy-MM-dd"</param>
        /// <param name="emptyValue">Text representation for empty value, like " N/A "</param>
        /// <param name="emptyList">Array of DateTime values considered to be empty, like new DateTime[] { DateTime.MinValue, new DateTime(1753, 1, 1) }</param>
        /// <returns>Date and time string representation</returns>
        //[Energy.Attribute.Code.Refactoring("Extract method for automatic selection of custom format string for DateTime", Progress = 10.5)]
        public static string DateTimeToString(DateTime? stamp, string customTimeFormat, string customDateFormat, string emptyValue, DateTime[] emptyList)
        {
            if (stamp == null)
            {
                return emptyValue;
            }

            if (emptyList != null && emptyList.Length > 0)
            {
                for (int i = 0; i < emptyList.Length; i++)
                {
                    if (emptyList[i] == stamp)
                    {
                        return emptyValue;
                    }
                }
            }
            else if (stamp == DateTime.MinValue || stamp == DateTime.MaxValue)
            {
                return emptyValue;
            }

            string customFormat = customTimeFormat;

            if (customDateFormat != null)
            {
                if (((DateTime)stamp).TimeOfDay.Ticks == 0)
                {
                    customFormat = customDateFormat;
                }
            }
            else if (customFormat == null)
            {
                // TODO Refactoring
                long microseconds = ((DateTime)stamp).TimeOfDay.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
                if (microseconds % 1000 > 0)
                {
                    customFormat = Energy.Base.Cast.Behaviour.DATETIME_FORMAT_DEFAULT_MICROSECOND;
                }
                else if (((DateTime)stamp).Millisecond > 0)
                {
                    customFormat = Energy.Base.Cast.Behaviour.DATETIME_FORMAT_DEFAULT_MILLISECOND;
                }
                else
                {
                    if (((DateTime)stamp).TimeOfDay.Ticks > 0)
                    {
                        customFormat = Energy.Base.Cast.Behaviour.DATETIME_FORMAT_DEFAULT_SECOND;
                    }
                    else
                    {
                        customFormat = Energy.Base.Cast.Behaviour.DATETIME_FORMAT_DEFAULT_DATE;
                    }
                }
            }

            string text = "" == customFormat ? "" : ((DateTime)stamp).ToString(customFormat);

            return text;
        }

        /// <summary>
        /// Return DateTime as ISO 8601 date string yyyy-MM-dd (empty if default)
        /// </summary>
        /// <param name="stamp">Nullable DateTime</param>
        /// <returns>Date string representation</returns>
        public static string DateTimeToStringDate(DateTime stamp)
        {
            if (stamp == DateTime.MinValue)
                return "";
            return stamp.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Return DateTime as ISO 8601 date string yyyy-MM-dd (empty if default or null)
        /// </summary>
        /// <param name="stamp">Nullable DateTime</param>
        /// <returns>Date string representation</returns>
        public static string DateTimeToStringDate(DateTime? stamp)
        {
            if (stamp == null)
                return "";
            if ((DateTime)stamp == DateTime.MinValue)
                return "";
            return ((DateTime)stamp).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Return DateTime as ISO 8601 time string with milliseconds if not zero (empty if default)
        /// </summary>
        /// <param name="stamp">DateTime</param>
        /// <returns>Time string representation</returns>
        public static string DateTimeToStringTime(DateTime stamp)
        {
            if (stamp == DateTime.MinValue)
                return "";
            if (stamp.Millisecond != 0)
                return stamp.ToString("HH:mm:ss.fff");
            else
                return stamp.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Return DateTime as ISO time string with milliseconds if not zero (empty if default or null)
        /// </summary>
        /// <param name="stamp">DateTime</param>
        /// <returns>Time string representation</returns>
        public static string DateTimeToStringTime(DateTime? stamp)
        {
            return stamp == null ? "" : DateTimeToStringTime((DateTime)stamp);
        }

        /// <summary>
        /// Represent date and time strictly according to ISO 8601 standard
        /// with "T" for time, "Z" for UTC and "+/-" for time zone.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateTimeToISO8601(DateTime? value)
        {
            return Energy.Base.Clock.GetISO8601(value);
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

        #region TimeSpan

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000".
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="omitZeroMilliseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <param name="roundUp">Round up to 1 ms if not exactly 0 ms</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(double seconds, bool omitZeroMilliseconds, bool omitZeroHours, bool roundUp)
        {
            int truncate = (int)seconds;
            double fractional = seconds - truncate;
            double milliseconds = fractional * 1000.0;
            int s = truncate % 60;
            truncate /= 60;
            int m = truncate % 60;
            truncate /= 60;
            int h = truncate;
            int ms = (int)milliseconds;

            if (roundUp && ms == 0 && milliseconds - ms > 0)
            {
                ms = 1;
            }

            StringBuilder sb = new StringBuilder();

            if (!omitZeroHours || h > 0)
            {
                sb.Append(h.ToString("00"));
                sb.Append(":");
            }

            sb.Append(m.ToString("00"));
            sb.Append(":");
            sb.Append(s.ToString("00"));

            if (!omitZeroMilliseconds || ms > 0)
            {
                sb.Append(".");
                sb.Append(ms.ToString("000"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="omitZeroMilliseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(double seconds, bool omitZeroMilliseconds, bool omitZeroHours)
        {
            return TimeSpanToStringMicroseconds(seconds, omitZeroMilliseconds, omitZeroHours, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="omitZeroMilliseconds">Omit milliseconds part if zero</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(double seconds, bool omitZeroMilliseconds)
        {
            return TimeSpanToStringMicroseconds(seconds, omitZeroMilliseconds, true, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(double seconds)
        {
            return TimeSpanToStringMicroseconds(seconds, true, true, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <param name="omitZeroMilliseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <param name="roundUp">Round up to 1 ms if not exactly 0 ms</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(TimeSpan timeSpan, bool omitZeroMilliseconds, bool omitZeroHours, bool roundUp)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMilliseconds(seconds, omitZeroMilliseconds, omitZeroHours, roundUp);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <param name="omitZeroMilliseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(TimeSpan timeSpan, bool omitZeroMilliseconds, bool omitZeroHours)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMilliseconds(seconds, omitZeroMilliseconds, omitZeroHours, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <param name="omitZeroMilliseconds">Omit milliseconds part if zero</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(TimeSpan timeSpan, bool omitZeroMilliseconds)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMilliseconds(seconds, omitZeroMilliseconds, true, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324" or "00:03:10.123" or "00:00.000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMilliseconds(TimeSpan timeSpan)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMilliseconds(seconds, false, false, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="omitZeroMicroseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <param name="roundUp">Round up to 1 μs if not exactly 0 μs</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMicroseconds(double seconds, bool omitZeroMicroseconds, bool omitZeroHours, bool roundUp)
        {
            int truncate = (int)seconds;
            double fractional = seconds - truncate;
            double microseconds = fractional * 1000000.0;
            int s = truncate % 60;
            truncate /= 60;
            int m = truncate % 60;
            truncate /= 60;
            int h = truncate;
            int μ = (int)microseconds;

            if (roundUp && μ == 0 && microseconds - μ > 0)
            {
                μ = 1;
            }

            StringBuilder sb = new StringBuilder();

            if (!omitZeroHours || h > 0)
            {
                sb.Append(h.ToString("00"));
                sb.Append(":");
            }

            sb.Append(m.ToString("00"));
            sb.Append(":");
            sb.Append(s.ToString("00"));

            if (!omitZeroMicroseconds || μ > 0)
            {
                sb.Append(".");
                sb.Append(μ.ToString("000000"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="omitZeroMicroseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMicroseconds(double seconds, bool omitZeroMicroseconds, bool omitZeroHours)
        {
            return TimeSpanToStringMicroseconds(seconds, omitZeroMicroseconds, omitZeroMicroseconds, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <param name="omitZeroMicroseconds">Omit milliseconds part if zero</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMicroseconds(double seconds, bool omitZeroMicroseconds)
        {
            return TimeSpanToStringMicroseconds(seconds, omitZeroMicroseconds, true, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <returns>string</returns>
        public static string TimeSpanToStringMicroseconds(double seconds)
        {
            return TimeSpanToStringMicroseconds(seconds, true, true, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <param name="omitZeroMicroseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <param name="roundUp">Round up to 1 μs if not exactly 0 μs</param>
        /// <returns></returns>
        public static string TimeSpanToStringMicroseconds(TimeSpan timeSpan, bool omitZeroMicroseconds, bool omitZeroHours, bool roundUp)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMicroseconds(seconds, omitZeroMicroseconds, omitZeroMicroseconds, roundUp);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <param name="omitZeroMicroseconds">Omit milliseconds part if zero</param>
        /// <param name="omitZeroHours">Omit hours part if zero</param>
        /// <returns></returns>
        public static string TimeSpanToStringMicroseconds(TimeSpan timeSpan, bool omitZeroMicroseconds, bool omitZeroHours)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMicroseconds(seconds, omitZeroMicroseconds, omitZeroMicroseconds, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <param name="omitZeroMicroseconds">Omit milliseconds part if zero</param>
        /// <returns></returns>
        public static string TimeSpanToStringMicroseconds(TimeSpan timeSpan, bool omitZeroMicroseconds)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMicroseconds(seconds, omitZeroMicroseconds, true, true);
        }

        /// <summary>
        /// Convert TimeSpan to short string with milliseconds, ie. "99:20:03.324567" or "00:03:10.123456" or "00:00.000000"
        /// </summary>
        /// <param name="timeSpan">TimeSpan object</param>
        /// <returns></returns>
        public static string TimeSpanToStringMicroseconds(TimeSpan timeSpan)
        {
            double seconds = timeSpan.TotalSeconds;
            return TimeSpanToStringMicroseconds(seconds, false, true, true);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">Dictionary&gt;string, T&lt;</param>
        /// <returns></returns>
        public static string[] StringDictionaryToStringArray<T>(Dictionary<string, T> dictionary)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, T> _ in dictionary)
            {
                list.Add(_.Key);
                list.Add(_.Value.ToString());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Convert string array to dictionary containing key and value pairs
        /// </summary>
        /// <param name="array">string[]</param>
        /// <returns></returns>
        public static Dictionary<string, string> StringArrayToStringDictionary(params string[] array)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i + 1 < array.Length; i = i + 2)
            {
                string key = array[i];
                if (key == null)
                    continue;
                string value = array[i + 1];
                dictionary[key] = value;
            }
            if (array.Length % 2 != 0)
            {
                string key = array[array.Length - 1];
                if (key != null)
                    dictionary[key] = null;
            }
            return dictionary;
        }

        /// <summary>
        /// Convert string array to dictionary containing key and value pairs
        /// </summary>
        /// <param name="array">string[]</param>
        /// <returns></returns>
        public static Dictionary<string, object> StringArrayToObjectDictionary(params object[] array)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int i = 0; i + 1 < array.Length; i = i + 2)
            {
                string key = Energy.Base.Cast.ObjectToString(array[i]);
                if (key == null)
                    continue;
                object value = array[i + 1];
                dictionary[key] = value;
            }
            if (array.Length % 2 != 0)
            {
                string key = Energy.Base.Cast.ObjectToString(array[array.Length - 1]);
                if (key != null)
                    dictionary[key] = null;
            }
            return dictionary;
        }

        /// <summary>
        /// Convert string array to dictionary containing key and value pairs
        /// </summary>
        /// <param name="array">string[]</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> StringArrayToDictionary<TKey, TValue>(params object[] array)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i + 1 < array.Length; i = i + 2)
            {
                TKey key = Energy.Base.Cast.As<TKey>(array[i]);
                if (key == null)
                    continue;
                TValue value = Energy.Base.Cast.As<TValue>(array[1 + i]);
                dictionary[key] = value;
            }
            if (array.Length % 2 != 0)
            {
                TKey key = Energy.Base.Cast.As<TKey>(array[array.Length - 1]);
                if (key != null)
                {
                    dictionary[key] = default;
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Convert list of strings to character array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="onlyFirstCharacter">
        /// Take at most the first letter of the text. 
        /// Otherwise, add each possible character from the text.
        /// </param>
        /// <returns></returns>
        public static char[] StringArrayToFirstCharArray(string[] array, bool onlyFirstCharacter)
        {
            List<char> charList = new List<char>();
            foreach (string s in array)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                if (onlyFirstCharacter)
                    charList.Add(s[0]);
                else
                {
                    foreach (char c in s.ToCharArray())
                    {
                        if (!charList.Contains(c))
                            charList.Add(c);
                    }
                }
            }
            return charList.ToArray();
        }

        /// <summary>
        /// Convert list of strings to character array.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="onlyFirstCharacter">
        /// Take at most the first letter of the text. 
        /// Otherwise, add each possible character from the text.
        /// </param>
        /// <returns></returns>
        public static char[] StringListToFirstCharArray(List<string> list, bool onlyFirstCharacter)
        {
            return StringArrayToFirstCharArray(list.ToArray(), onlyFirstCharacter);
        }

        /// <summary>
        /// Convert dictionary to array of objects of key value pairs.
        /// Convert generic dictionary to object array containing key and value pairs one by another in one dimensional array.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static object[] DictionaryToObjectArray<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            List<object> list = new List<object>();
            foreach (TKey key in dictionary.Keys)
            {
                list.Add((object)key);
                list.Add((object)dictionary[key]);
            };
            return list.ToArray();
        }

        /// <summary>
        /// Convert generic dictionary to object array containing key and value pairs one by another in one dimensional array.
        /// </summary>
        /// <param name="dictionary">Dictionary</param>
        /// <returns></returns>
        public static object[] DictionaryToObjectArrayKeyValuePair<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            List<object> list = new List<object>();
            foreach (KeyValuePair<TKey, TValue> _ in dictionary)
            {
                list.Add(_.Key);
                list.Add(_.Value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Convert string dictionary to array of objects of key value pairs
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static object[] StringDictionaryToObjectArray<TValue>(Dictionary<string, TValue> dictionary)
        {
            List<object> list = new List<object>();
            foreach (string key in dictionary.Keys)
            {
                list.Add((object)key);
                list.Add((object)dictionary[key]);
            };
            return list.ToArray();
        }

        /// <summary>
        /// Convert string dictionary to array of objects of key value pairs
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static object[] StringDictionaryToObjectArray(Dictionary<string, string> dictionary)
        {
            List<object> list = new List<object>();
            foreach (string key in dictionary.Keys)
            {
                list.Add((object)key);
                list.Add((object)dictionary[key]);
            };
            return list.ToArray();
        }

        #endregion

        #region Object

        /// <summary>
        /// Represent object as string by using conversions or ToString() method.
        /// </summary>
        /// <param name="value">Object instance</param>
        /// <returns>String representation</returns>
        public static string ObjectToString(object value)
        {
            // treat DBNull as empty string //
            if (value == null || value == System.DBNull.Value)
                return null;
            // maybe it is already string? //
            if (value is string)
                return (string)value;
            // what about bool numbers //
            if (value is bool)
                return (bool)value ? "1" : "0";
            // convert to culture invariant form //
            if (value is double)
                return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is decimal)
                return ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is float)
                return ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            // works with nullable version DateTime? //
            if (value is DateTime)
            {
                if ((DateTime)value == DateTime.MinValue)
                {
                    return null;
                }
                if (((DateTime)value).Date == DateTime.MinValue.Date)
                {
                    return ((DateTime)value).ToString("HH:mm:ss");
                }
                if (((DateTime)value).Millisecond > 0)
                {
                    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                {
                    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            // works with nullable version TimeSpan? //
            if (value is TimeSpan)
            {
                return TimeSpanToStringMilliseconds((TimeSpan)value, true, true, true);
            }
            // return default string representation //
            return value.ToString();
        }

        public static int ObjectToInteger(object value)
        {
            if (value == null)
                return 0;

            if (value is Int32)
                return (int)(Int32)value;

            if (value is Int16)
                return (int)(Int16)value;
            if (value is Int64)
                return (int)(Int64)value;
            if (value is UInt16)
                return (int)(UInt16)value;

            try
            {
                if (value is UInt32)
                    return (int)(UInt32)value;
                if (value is UInt64)
                    return (int)(UInt64)value;
                if (value is double)
                    return (int)(double)value;
                if (value is float)
                    return (int)(float)value;
                if (value is decimal)
                    return (int)(decimal)value;
            }
            catch (OverflowException)
            {
                return 0;
            }

            if (value is byte)
                return (int)(byte)value;
            if (value is sbyte)
                return (int)(sbyte)value;
            if (value is char)
                return (int)(char)value;

            string s = value is string ? (string)value : value.ToString();

            return StringToInteger(s);
        }

        /// <summary>
        /// Convert string to unsigned integer value without exception.
        /// Allows to convert floating point values resulting in decimal part.
        /// Treat comma "," the same as dot "." as decimal point.     
        /// Returns zero on overflow.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint ObjectToUnsignedInteger(object value)
        {
            if (value == null)
                return 0;

            if (value is UInt32)
                return (uint)(UInt32)value;

            if (value is Int16)
                return (uint)(Int16)value;
            if (value is Int32)
                return (uint)(Int32)value;
            if (value is UInt16)
                return (uint)(UInt16)value;

            try
            {
                if (value is Int64)
                    return (uint)(Int64)value;
                if (value is UInt64)
                    return (uint)(UInt64)value;
                if (value is double)
                    return (uint)(double)value;
                if (value is float)
                    return (uint)(float)value;
                if (value is decimal)
                    return (uint)(decimal)value;
            }
            catch (OverflowException)
            {
                return 0;
            }

            if (value is byte)
                return (uint)(byte)value;
            if (value is sbyte)
                return (uint)(sbyte)value;
            if (value is char)
                return (uint)(char)value;

            string s = value is string ? (string)value : value.ToString();

            return StringToUnsignedInteger(s);
        }

        /// <summary>
        /// Convert string to byte value without exception.
        /// Allows to convert floating point values resulting in decimal part.
        /// Treat comma "," the same as dot "." as decimal point.     
        /// Returns zero on overflow.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ObjectToByte(object value)
        {
            if (value == null)
                return 0;

            if (value is byte)
                return (byte)(byte)value;
            if (value is char)
                return (byte)(char)value;

            try
            {
                if (value is Int16)
                    return (byte)(Int16)value;
                if (value is Int32)
                    return (byte)(Int32)value;
                if (value is Int64)
                    return (byte)(Int64)value;
                if (value is UInt16)
                    return (byte)(UInt16)value;
                if (value is UInt32)
                    return (byte)(UInt32)value;
                if (value is UInt64)
                    return (byte)(UInt64)value;
                if (value is double)
                    return (byte)(double)value;
                if (value is float)
                    return (byte)(float)value;
                if (value is decimal)
                    return (byte)(decimal)value;
                if (value is sbyte)
                    return (byte)(sbyte)value;
            }
            catch (OverflowException)
            {
                return 0;
            }

            string s = value is string ? (string)value : value.ToString();

            return StringToByte(s);
        }

        /// <summary>
        /// Convert string to signed byte value without exception.
        /// Allows to convert floating point values resulting in decimal part.
        /// Treat comma "," the same as dot "." as decimal point.     
        /// Returns zero on overflow.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static sbyte ObjectToSignedByte(object value)
        {
            if (value == null)
                return 0;

            if (value is sbyte)
                return (sbyte)(sbyte)value;
            if (value is char)
                return (sbyte)(char)value;

            try
            {
                if (value is Int16)
                    return (sbyte)(Int16)value;
                if (value is Int32)
                    return (sbyte)(Int32)value;
                if (value is Int64)
                    return (sbyte)(Int64)value;
                if (value is UInt16)
                    return (sbyte)(UInt16)value;
                if (value is UInt32)
                    return (sbyte)(UInt32)value;
                if (value is UInt64)
                    return (sbyte)(UInt64)value;
                if (value is double)
                    return (sbyte)(double)value;
                if (value is float)
                    return (sbyte)(float)value;
                if (value is decimal)
                    return (sbyte)(decimal)value;
                if (value is byte)
                    return (sbyte)(byte)value;
            }
            catch (OverflowException)
            {
                return 0;
            }

            string s = value is string ? (string)value : value.ToString();

            return StringToSignedByte(s, Energy.Base.Cast.Behaviour.BYTE_COMMA);
        }

        /// <summary>
        /// Convert string to char value without exception.
        /// Allows to convert floating point values resulting in decimal part for character code.
        /// For a string return first character.
        /// Returns '\0' on overflow.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static char ObjectToChar(object value)
        {
            if (value == null)
                return '\0';

            if (value is char)
                return (char)(char)value;
            if (value is byte)
                return (char)(byte)value;
            if (value is sbyte)
                return (char)(sbyte)value;

            try
            {
                if (value is Int16)
                    return (char)(Int16)value;
                if (value is Int32)
                    return (char)(Int32)value;
                if (value is Int64)
                    return (char)(Int64)value;
                if (value is UInt16)
                    return (char)(UInt16)value;
                if (value is UInt32)
                    return (char)(UInt32)value;
                if (value is UInt64)
                    return (char)(UInt64)value;

                if (value is double)
                    return (char)(double)value;
                if (value is decimal)
                    return (char)(decimal)value;
                if (value is float)
                    return (char)(float)value;
            }
            catch (OverflowException)
            {
                return '\0';
            }

            string s = value is string ? (string)value : value.ToString();

            return StringToChar(s);
        }

        /// <summary>
        /// Convert object to long integer number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ObjectToLong(object value)
        {
            if (value == null)
                return 0;

            if (value is Int64)
                return (long)(long)value;

            try
            {
                if (value is UInt64)
                    return (long)(ulong)value;

                if (value is double)
                    return (long)(double)value;
                if (value is decimal)
                    return (long)(decimal)value;
                if (value is float)
                    return (long)(float)value;
            }
            catch (OverflowException)
            {
                return 0;
            }

            if (value is Int32)
                return (long)(int)value;
            if (value is UInt32)
                return (long)(uint)value;
            if (value is Int16)
                return (long)(Int16)value;
            if (value is UInt16)
                return (long)(UInt16)value;
            if (value is byte)
                return (long)(byte)value;
            if (value is sbyte)
                return (long)(sbyte)value;
            if (value is char)
                return (long)(char)value;

            string s = value is string ? (string)value : value.ToString();

            return StringToLong(s);
        }

        /// <summary>
        /// Convert object to long integer number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong ObjectToUnsignedLong(object value)
        {
            if (value == null)
                return 0;

            if (value is UInt64)
                return (ulong)(ulong)value;

            try
            {
                if (value is Int64)
                    return (ulong)(long)value;
                if (value is Int32)
                    return (ulong)(int)value;
                if (value is Int16)
                    return (ulong)(Int16)value;
                if (value is sbyte)
                    return (ulong)(sbyte)value;

                if (value is double)
                    return (double)value < 0 ? 0 : (ulong)(double)value;
                if (value is decimal)
                    return (decimal)value < 0 ? 0 : (ulong)(decimal)value;
                if (value is float)
                    return (float)value < 0 ? 0 : (ulong)(float)value;
            }
            catch (OverflowException)
            {
                return 0;
            }

            if (value is UInt32)
                return (ulong)(uint)value;
            if (value is UInt16)
                return (ulong)(UInt16)value;
            if (value is byte)
                return (ulong)(byte)value;
            if (value is char)
                return (ulong)(char)value;

            string s = value is string ? (string)value : value.ToString();

            return StringToUnsignedLong(s);
        }

        /// <summary>
        /// Convert object to boolean value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ObjectToBool(object value)
        {
            if (value == null)
                return false;
            if (value == System.DBNull.Value)
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

        /// <summary>
        /// Convert object to double value without exception.
        /// Treat comma "," the same as dot "." as decimal point
        /// when converting from string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ObjectToDouble(object value)
        {
            if (value == null || value == System.DBNull.Value)
                return 0;

            if (value is Int64)
                return (double)(Int64)value;
            if (value is Int32)
                return (double)(Int32)value;
            if (value is double)
                return (double)(double)value;
            if (value is UInt32)
                return (double)(UInt32)value;
            if (value is UInt64)
                return (double)(UInt64)value;
            if (value is decimal)
                return (double)(decimal)value;

            if (value is Int16)
                return (double)(Int16)value;
            if (value is UInt16)
                return (double)(UInt16)value;

            if (value is byte)
                return (double)(byte)value;
            if (value is sbyte)
                return (double)(sbyte)value;
            if (value is char)
                return (long)(char)value;

            string s = value is string ? (string)value : value.ToString();

            return StringToDouble(s);
        }

        /// <summary>
        /// Convert object to float value without exception.
        /// Treat comma "," the same as dot "." as decimal point
        /// when converting from string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ObjectToFloat(object value)
        {
            double x = ObjectToDouble(value);
            if (x < float.MinValue)
                return float.MinValue;
            else if (x > float.MaxValue)
                return float.MaxValue;
            else
                return (float)x;
        }

        /// <summary>
        /// Convert object to decimal number.
        /// Treat comma "," the same as dot "." as decimal point
        /// when converting from string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object value)
        {
            if (value == null)
                return 0;

            if (value is long)
                return (decimal)(long)value;
            if (value is int)
                return (decimal)(int)value;
            if (value is double)
                return (decimal)(double)value;
            if (value is uint)
                return (decimal)(uint)value;
            if (value is ulong)
                return (decimal)(ulong)value;
            if (value is decimal)
                return (decimal)(decimal)value;

            if (value is Int16)
                return (decimal)(Int16)value;
            if (value is UInt16)
                return (decimal)(UInt16)value;

            if (value is short)
                return (decimal)(short)value;
            if (value is ushort)
                return (decimal)(ushort)value;
            if (value is byte)
                return (decimal)(byte)value;
            if (value is char)
                return (decimal)(char)value;

            string s = value is string ? (string)value : value.ToString();

            return StringToDecimal(s);
        }

        public static Int16 ObjectToWord(object value)
        {
            int number = ObjectToInteger(value);
            if (number < Int16.MinValue)
                return Int16.MinValue;
            else if (number > Int16.MaxValue)
                return Int16.MaxValue;
            else
                return (Int16)number;
        }

        public static UInt16 ObjectToUnsignedWord(object value)
        {
            int number = ObjectToInteger(value);
            if (number <= 0)
                return 0;
            else if (number > UInt16.MaxValue)
                return UInt16.MaxValue;
            else
                return (UInt16)number;
        }

        /// <summary>
        /// Convert object to DateTime value.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object o)
        {
            if (null == o)
            {
                return DateTime.MinValue;
            }
            if (o is DateTime)
            {
                return (DateTime)o;
            }
            string text = o is string ? (string)o : o.ToString();
            return Energy.Base.Clock.Parse(text);
        }

        /// <summary>
        /// Convert object to DateTime value within specified range.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns>DateTime</returns>
        public static DateTime ObjectToDateTime(object o, DateTime minimum, DateTime maximum)
        {
            if (null == o)
            {
                return minimum;
            }
            DateTime value;
            if (o is DateTime)
            {
                value = (DateTime)o;
            }
            else
            {
                string text = o is string ? (string)o : o.ToString();
                value = Clock.Parse(text);
            }
            if (minimum != DateTime.MinValue && value < minimum)
            {
                value = minimum;
            }
            else if (maximum != DateTime.MaxValue && value > maximum)
            {
                value = maximum;
            }
            return value;
        }

        public static TStringToObject StringToObject<TStringToObject>(string input)
        {
            System.Type type = typeof(TStringToObject);
            if (false)
            { }
            else if (type == typeof(string))
                return (TStringToObject)(object)input;
            else if (type == typeof(long))
                return (TStringToObject)(object)Energy.Base.Cast.StringToLong(input);
            else if (type == typeof(int))
                return (TStringToObject)(object)Energy.Base.Cast.StringToInteger(input);
            else if (type == typeof(bool))
                return (TStringToObject)(object)Energy.Base.Cast.StringToBool(input);
            else if (type == typeof(double))
                return (TStringToObject)(object)Energy.Base.Cast.StringToDouble(input);
            else if (type == typeof(decimal))
                return (TStringToObject)(object)Energy.Base.Cast.StringToDecimal(input);
            else if (type == typeof(float))
                return (TStringToObject)(object)Energy.Base.Cast.StringToFloat(input);
            else if (type == typeof(byte))
                return (TStringToObject)(object)Energy.Base.Cast.StringToByte(input);
            else if (type == typeof(DateTime))
                return (TStringToObject)(object)Energy.Base.Cast.StringToDateTime(input);
            else
                return (TStringToObject)(object)input;
        }

        /// <summary>
        /// Convert object array to Dictionary&lt;TKey, TValue&gt;
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ObjectArrayToDictionary<TKey, TValue>(object[] array)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i - 1 < array.Length; i += 2)
            {
                dictionary[(TKey)array[i]] = (TValue)array[i + 1];
            }
            return dictionary;
        }

        /// <summary>
        /// Object array to
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ObjectArrayToStringDictionary<T>(object[] array)
        {
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            for (int i = 0; i - 1 < array.Length; i += 2)
            {
                dictionary[(string)array[i]] = (T)array[i + 1];
            }
            return dictionary;
        }

        public static Stream ObjectToStream(object o)
        {
            string s = ObjectToString(o);
            return StringToStream(s);
        }

        public static TStream ObjectToStream<TStream>(object o) where TStream : Stream
        {
            string s = ObjectToString(o);
            if (null == s)
            {
                return default;
            }
            return StringToStream(s) as TStream;
        }

        #endregion

        #region Enum

        /// <summary>
        /// Convert string to enumeration value.
        /// Ignore case and surrounding whitespace.
        /// If string value can't be found, equivalent of 0 will be returned.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="type">Type</param>
        /// <returns>object</returns>
        public static object StringToEnum(string value, Type type)
        {
            return StringToEnum(value, type, true);
        }

        /// <summary>
        /// Convert string to enumeration value.
        /// Ignore leading and trailing whitespace.
        /// If string value can't be found, equivalent of 0 will be returned.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="type">Type</param>
        /// <param name="ignoreCase"></param>
        /// <returns>object</returns>
        public static object StringToEnum(string value, Type type, bool ignoreCase)
        {
            if (type == null)
            {
                return 0;
            }
            if (value == null)
            {
                return 0;
            }
            value = Energy.Base.Text.Trim(value);
            if (value.Length == 0)
            {
                return 0;
            }
            string[] names = Enum.GetNames(type);
            for (int i = 0; i < names.Length; i++)
            {
                if (string.Compare(value, names[i], ignoreCase) == 0)
                {
                    return Enum.Parse(type, value, ignoreCase);
                }
            }
            return 0;
        }

        /// <summary>
        /// Convert string to enumeration value.
        /// Ignore case and surrounding whitespace.
        /// If string value can't be found, equivalent of 0 will be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string value)
        {
            return (T)StringToEnum(value, typeof(T), true);
        }

        /// <summary>
        /// Convert string to enumeration value.
        /// Ignore leading and trailing whitespace.
        /// If string value can't be found, equivalent of 0 will be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string value, bool ignoreCase)
        {
            return (T)StringToEnum(value, typeof(T), ignoreCase);
        }

        /// <summary>
        /// Convert integer number to enumeration value.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="type">Type</param>
        /// <returns>object</returns>
        public static object IntToEnum(int value, Type type)
        {
            return Enum.ToObject(type, value);
        }

        /// <summary>
        /// Convert integer number to enumeration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T IntToEnum<T>(int value)
        {
            return (T)IntToEnum(value, typeof(T));
        }

        #endregion

        #region Char

        public static bool CharToBool(char value)
        {
            char _ = (char)value;
            return _ != '\0' && _ != '0';
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
        /// Convert UTF-8 string to Base64 text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringToBase64(string input)
        {
            return StringToBase64(input, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Convert string to Base64 text
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string StringToBase64(string input, System.Text.Encoding encoding)
        {
            byte[] data = encoding.GetBytes(input);
            return System.Convert.ToBase64String(data);
        }

        /// <summary>
        /// Convert Base64 input to UTF-8 string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64ToString(string input)
        {
            return Base64ToString(input, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Convert Base64 input to string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64ToString(string input, System.Text.Encoding encoding)
        {
            if (input == null)
                return null;
            if (input.Length == 0)
                return "";
            byte[] data;
            try
            {
                data = System.Convert.FromBase64String(input);
            }
            catch (FormatException)
            {
                return null;
            }
            return System.Text.Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Convert Base64 input to byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Base64ToByteArray(string input)
        {
            if (input == null)
            {
                return null;
            }
            if (input.Length == 0)
            {
                return new byte[] { };
            }
            byte[] data;
            try
            {
                data = System.Convert.FromBase64String(input);
            }
            catch (FormatException)
            {
                return null;
            }
            return data;
        }

        /// <summary>
        /// Convert byte array to Base64 string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToBase64(byte[] data)
        {
            if (data == null)
                return null;
            if (data.Length == 0)
                return "";
            string base64 = System.Convert.ToBase64String(data);
            return base64;
        }

        #endregion

        #region Json

        /// <summary>
        /// Escape JSON characters and include string in double quotes.
        /// Null strings will be represented as "null".
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringToJsonString(string value)
        {
            return Energy.Base.Json.Escape(value);
        }

        /// <summary>
        /// Convert object to formal JSON value string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ObjectToJsonValue(object value)
        {
            // treat DBNull as empty string //
            if (value == null || value is System.DBNull)
                return "null";
            // maybe it is already string? //
            if (value is string)
                return (string)StringToJsonString((string)value);
            // what about bool numbers //
            if (value is bool)
                return (bool)value ? "true" : "false";
            // convert to culture invariant form //
            if (value is double)
                return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is decimal)
                return ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is float)
                return ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (value is DateTime)
                return string.Concat("\"", DateTimeToISO8601((DateTime)value), "\"");
            // return default string representation //
            return StringToJsonString(value.ToString());
        }

        /// <summary>
        /// Convert JSON value string to an object.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static object JsonValueToObject(string text)
        {
            if (text == "null")
                return null;
            if (text == "true")
                return true;
            if (text == "false")
                return false;
            int intValue = 0;
            if (int.TryParse(text, out intValue))
                return intValue;
            long longValue = 0;
            if (long.TryParse(text, out longValue))
                return longValue;
            ulong ulongValue = 0;
            if (ulong.TryParse(text, out ulongValue))
            {
                return ulongValue;
            }
            double doubleValue = 0;
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleValue))
                return doubleValue;
            decimal decimalValue = 0;
            if (decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out decimalValue))
                return decimalValue;
            return Energy.Base.Json.Strip(text);
        }

        #endregion

        #region MemorySize

        private static readonly string[] _MemorySizeSuffix = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        /// <summary>
        /// Represents memory size as a string containing a numeric value with an attached size unit.
        /// Units used are: "B", "KB", "MB", "GB", "TB", "PB", "EB".
        /// </summary>
        /// <param name="sizeInBytes"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="numberCeiling"></param>
        /// <returns></returns>
        public static string MemorySizeToString(long sizeInBytes, int decimalPlaces, bool numberCeiling)
        {
            if (sizeInBytes <= 0)
                return "0 " + _MemorySizeSuffix[0];
            int exponent = Convert.ToInt32(Math.Floor(Math.Log(sizeInBytes, 1024)));
            double number = sizeInBytes / Math.Pow(1024, exponent);
            int p = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                p *= 10;
            }
            if (numberCeiling)
                number = Math.Ceiling(number * p);
            else
                number = Math.Floor(number * p);

            number /= p;

            return string.Concat(number.ToString(CultureInfo.InvariantCulture), " "
                , _MemorySizeSuffix[exponent]);
        }

        #endregion

        #region MemoryStream

        public static string MemoryStreamToString(System.IO.MemoryStream memoryStream)
        {
            byte[] data = memoryStream.ToArray();
            int index = 0;
            int count = data.Length;
            string result = Energy.Base.Text.Encoding(null).GetString(data, index, count);
            return result;
        }

        #endregion

        #region Enumeration

        /// <summary>
        /// Enumeration conversions
        /// </summary>
        public static class Enumeration
        {
            public static Energy.Enumeration.TextPad TextAlignToTextPad(Energy.Enumeration.TextAlign align)
            {
                switch (align)
                {
                    default:
                        return Energy.Enumeration.TextPad.None;

                    case Energy.Enumeration.TextAlign.Center:
                        return Energy.Enumeration.TextPad.Center;

                    case Energy.Enumeration.TextAlign.Left:
                        return Energy.Enumeration.TextPad.Right;

                    case Energy.Enumeration.TextAlign.Right:
                        return Energy.Enumeration.TextPad.Left;
                }
            }

            public static Energy.Enumeration.TextAlign TextPadToTextAlign(Energy.Enumeration.TextPad pad)
            {
                bool beLeft = 0 < (pad & Energy.Enumeration.TextPad.Left);
                bool beRight = 0 < (pad & Energy.Enumeration.TextPad.Right);
                if (beLeft && beRight)
                    return Energy.Enumeration.TextAlign.Center;
                else if (beLeft)
                    return Energy.Enumeration.TextAlign.Right;
                else if (beRight)
                    return Energy.Enumeration.TextAlign.Left;
                else
                    return Energy.Enumeration.TextAlign.None;
            }

            public static Energy.Enumeration.TextAlign CharToTextAlign(char align)
            {
                switch (align)
                {
                    default:
                        return Energy.Enumeration.TextAlign.None;
                    case '<':
                        return Energy.Enumeration.TextAlign.Left;
                    case '>':
                        return Energy.Enumeration.TextAlign.Right;
                    case '-':
                        return Energy.Enumeration.TextAlign.Center;
                    case '=':
                        return Energy.Enumeration.TextAlign.Justify;
                }
            }
        }

        #endregion
    }
}
