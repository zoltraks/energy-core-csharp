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
            if (value.Contains(suffix))
            {
                string replacement = suffix + suffix;
                value = value.Replace(suffix, replacement);
            }
            string result = string.Concat(prefix, value, suffix);
            return result;
        }
    }
}
