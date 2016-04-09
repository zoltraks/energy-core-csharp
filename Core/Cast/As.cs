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
    }
}
