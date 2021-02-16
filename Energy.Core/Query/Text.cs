using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query
{
    /// <summary>
    /// Query script helpers.
    /// Helper functions for working with SQL query text.
    /// </summary>
    public class Text
    {
        #region Quote

        /// <summary>
        /// Quote string value using apostrophe (') as quotation mark.
        /// Function will return "NULL" if value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Quote(string value)
        {
            return Quote(value, '\'', null);
        }

        /// <summary>
        /// Quote string value using specified quotation mark.
        /// Use apostrophe (') for values and quotes (") for database object names.
        /// Function will return "NULL" if value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        public static string Quote(string value, char quote)
        {
            return Quote(value, quote, null);
        }

        /// <summary>
        /// Quote string value using specified quotation mark.
        /// Use apostrophe (') for values and quotes (") for database object names.
        /// Specify text to be returned when value is null or pass null to use default "NULL".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quote"></param>
        /// <param name="nullText"></param>
        /// <returns></returns>
        public static string Quote(string value, char quote, string nullText)
        {
            if (nullText == null)
            {
                nullText = "NULL";
            }
            if (value == null)
            {
                return "NULL";
            }
            if (quote == '\0')
            {
                return value;
            }
            if (0 <= value.IndexOf(quote))
            {
                string replacement = new string(quote, 2);
                value = value.Replace(quote.ToString(), replacement);
            }
            string result = string.Concat(quote, value, quote);
            return result;
        }

        /// <summary>
        /// Quote string value using specified quotation mark.
        /// You might use square parenthesis ([]) to use T-SQL style quotation for database object names.
        /// Function will return "NULL" if value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        public static string Quote(string value, string quote)
        {
            return Quote(value, quote, null);
        }

        /// <summary>
        /// Quote string value using specified quotation mark.
        /// You might use square parenthesis ([]) to use T-SQL style quotation for database object names.
        /// Specify text to be returned when value is null or pass null to use default "NULL".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quote"></param>
        /// <param name="nullText"></param>
        /// <returns></returns>
        public static string Quote(string value, string quote, string nullText)
        {
            if (nullText == null)
            {
                nullText = "NULL";
            }
            if (string.IsNullOrEmpty(quote))
            {
                return Quote(value, '\0', nullText);
            }
            if (quote.Length == 1)
            {
                return Quote(value, quote[0], nullText);
            }
            if (0 != quote.Length % 2)
            {
                throw new NotSupportedException("Quotation mark string must be of even length");
            }
            int half = quote.Length >> 1;
            string prefix = quote.Substring(0, half);
            string suffix = quote.Substring(half);
            if (Energy.Base.Text.Contains(value, suffix))
            {
                string replacement = suffix + suffix;
                value = value.Replace(suffix, replacement);
            }
            string result = string.Concat(prefix, value, suffix);
            return result;
        }

        #endregion

        #region Strip

        /// <summary>
        /// Strip quotation from a value. 
        /// Includes support for apostrophes, quotation marks, square brackets or unquoted values.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Strip(string value)
        {
            if (value == null)
                return null;
            value = value.Trim();
            if (value.Length == 0)
                return value;
            if (value.StartsWith("["))
                return Strip(value, "[]", "NULL");
            if (value.StartsWith("'"))
                return Strip(value, "'", "NULL");
            else
                return Strip(value, @"""", "NULL");
        }

        /// <summary>
        /// Strip quotation from a value.
        /// Return null if nullText parameter is specified and value equals to it.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quote"></param>
        /// <param name="nullText"></param>
        /// <returns></returns>
        public static string Strip(string value, string quote, string nullText)
        {
            if (value == null)
                return null;
            value = value.Trim();
            if (value.Length == 0)
                return value;
            if (!string.IsNullOrEmpty(nullText) && 0 == string.Compare(value, nullText, true))
                return null;
            if (string.IsNullOrEmpty(quote))
                return value;
            if (quote.Length == 1)
            {
                if (value.Length < 2)
                    return value;
                if (value.StartsWith(quote) && value.EndsWith(quote))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                if (Energy.Base.Text.Contains(value, quote))
                {
                    value = value.Replace(quote + quote, quote);
                }
                return value;
            }
            if (0 != quote.Length % 2)
            {
                throw new NotSupportedException("Quotation mark string must be of even length");
            }
            int half = quote.Length >> 1;
            string prefix = quote.Substring(0, half);
            string suffix = quote.Substring(half);
            if (value.StartsWith(prefix) && value.EndsWith(suffix))
            {
                value = value.Substring(prefix.Length, value.Length - prefix.Length - suffix.Length);
                if (Energy.Base.Text.Contains(value, suffix))
                {
                    string replacement = suffix + suffix;
                    value = value.Replace(replacement, suffix);
                }
                return value;
            }
            return value;
        }

        #endregion
    }
}
