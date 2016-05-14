using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Cast
{
    public static class As
    {
        /// <summary>
        /// Convert string to integer value without exception
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Integer number</returns>
        public static int Integer(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
            if (value is string)
                return Energy.Base.Cast.StringToInteger((string)value);
            return Energy.Base.Cast.StringToInteger(value.ToString());
        }

        /// <summary>
        /// Convert string to long integer value without exception
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>Long number</returns>
        public static long Long(object value)
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
        public static string String(double value)
        {
            return Energy.Base.Cast.DoubleToString(value);
        }

        /// <summary>
        /// Convert double value to invariant string.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="culture">InvariantCulture by default, that means 1234.56 instead of 1'234,56.</param>
        /// <returns>String</returns>
        public static string String(double value, int precision, System.Globalization.CultureInfo culture = null)
        {
            return Energy.Base.Cast.DoubleToString(value, precision, culture);
        }

        /// <summary>
        /// Convert double value to invariant string.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="precision">Precision</param>
        /// <param name="trim">Trim zeroes from end</param>
        /// <param name="culture">InvariantCulture by default, that means 1234.56 instead of 1'234,56.</param>
        /// <returns>String</returns>
        public static string String(double value, int precision, bool trim, System.Globalization.CultureInfo culture = null)
        {
            return Energy.Base.Cast.DoubleToString(value, precision, trim, culture);
        }

        /// <summary>
        /// Represent object as string by using conversions or ToString() method
        /// </summary>
        /// <param name="value">Object instance</param>
        /// <returns>String representation</returns>
        public static string String(object value)
        {
            return Energy.Base.Cast.ObjectToString(value);
        }

        /// <summary>
        /// Convert string to enum
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>object</returns>
        public static T Enum<T>(string value)
        {
            return (T)Energy.Base.Cast.StringToEnum(value, typeof(T));
        }
    }

}
