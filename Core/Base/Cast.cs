using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Conversion and type casting
    /// </summary>
    public static class Cast
    {
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
            if (String.IsNullOrEmpty(text) || text == "0") return false;
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
        /// Convert string to integer value without exception
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

        #endregion

        #region Long

        /// <summary>
        /// Convert string to long integer value without exception
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

        #endregion

        #region Decimal

        /// <summary>
        /// Convert string to decimal value without exception
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>decimal</returns>
        public static decimal StringToDecimal(string value)
        {
            decimal result = 0;
            if (!String.IsNullOrEmpty(value))
            {
                decimal.TryParse(value.Trim(' ', '\t', '\r', '\n', '\v', '\0'), out result);
            }
            return result;
        }
 
        #endregion

        #region Double

        /// <summary>
        /// Convert string to double value without exception.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>double</returns>
        public static double StringToDouble(string value)
        {
            double result = 0;
            if (value != null && value.Length > 0)
            {
                value = value.Trim(' ', '\t', '\r', '\n', '\v', '\0');
                if (value.Contains(" ")) value = value.Replace(" ", null);
                if (!double.TryParse(value,
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out result))
                {
                    double.TryParse(value,
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.CurrentCulture,
                        out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert floating value to invariant string.
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>string</returns>
        public static string DoubleToString(double value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert numeric text to invariant string.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>string</returns>
        public static string DoubleToString(string value)
        {
            return StringToDouble(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert floating value to invariant string.
        /// </summary>
        /// <param name="value">double</param>
        /// <param name="precision">precision</param>
        /// <param name="culture"></param>
        /// <returns>string</returns>
        public static string DoubleToString(double value, int precision, System.Globalization.CultureInfo culture = null)
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

        #endregion

        #region Text

        /// <summary>
        /// Join multiline text into single string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SingleLine(string text)
        {
            return String.Join(" ", (new List<string>(text.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)))
                .ConvertAll<string>(delegate (string s) { return s.Trim(); }).ToArray());
        }

        #endregion

        #region Object

        /// <summary>
        /// Represent object value as string
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>string</returns>
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

        #endregion
    }
}
