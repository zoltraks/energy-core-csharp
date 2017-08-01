using System;
using System.Collections.Generic;

namespace Energy.Base
{
    /// <summary>
    /// Text related functions
    /// </summary>
    // TODO This class probably should be renamed to avoid conflicts and allow to add using Energy.Base
    public class Text
    {
        /// <summary>
        /// Exchange texts between each other
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static void Exchange(ref string first, ref string second)
        {
            string remember = first;
            first = second;
            second = remember;
        }

        /// <summary>
        /// Select first non empty string element
        /// </summary>
        /// <param name="list">string[]</param>
        /// <returns>string</returns>
        public static string Select(params string[] list)
        {
            foreach (string element in list)
            {
                if (!String.IsNullOrEmpty(element)) return element;
            }
            return null;
        }

        /// <summary>
        /// Surround text with delimiters if contains delimiter itself or any of special characters
        /// </summary>
        /// <param name="value">Text value</param>
        /// <param name="delimiter">Delimiter like ' or "</param>
        /// <param name="special">List of special char</param>
        /// <returns></returns>
        public static string Surround(string value, string delimiter, string[] special)
        {
            if (value == null || value == "")
                return value;

            bool quote = false;

            if (String.IsNullOrEmpty(delimiter))
            {
                if (special == null)
                    return value;
            }
            else if (value.Contains(delimiter))
            {
                quote = true;
            }

            if (!quote && special != null)
            {
                for (int i = 0; i < special.Length; i++)
                {
                    if (value.Contains(special[i]))
                    {
                        quote = true;
                        break;
                    }
                }
            }

            if (!quote)
                return value;

            return String.Concat(delimiter, value, delimiter);
        }

        /// <summary>
        /// Surround text with delimiters if contains delimiter itself or any of special characters
        /// </summary>
        /// <param name="value">Text value</param>
        /// <param name="delimiter">Delimiter like ' or "</param>
        /// <returns></returns>
        public static string Surround(string value, string delimiter)
        {
            return Surround(value, delimiter, null);
        }

        public static string Texture(string pattern, int size)
        {
            if (string.IsNullOrEmpty(pattern))
                return pattern;
            if (size == pattern.Length)
                return pattern;
            if (size < pattern.Length)
                return pattern.Substring(0, size);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            while (s.Length < size)
                s.Append(s.Length == 0 ? pattern : s.ToString());
            if (s.Length == size)
                return s.ToString();
            return s.ToString().Substring(0, size);
        }

        /// <summary>
        /// Remove leading and trailing whitespace.
        /// Includes space, tabulation (horizontal and vertical), new line and null characters.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Trimmed string</returns>
        public static string TrimWhite(string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim(' ', '\t', '\r', '\n', '\v', '\0');
        }

        /// <summary>
        /// Join non empty strings into one list with separator
        /// </summary>
        /// <param name="with">Separator string</param>
        /// <param name="parts">Parts to join</param>
        /// <returns>Example: JoinWith(" : ", "A", "B", "", "C") = "A : B : C".</returns>
        public static string JoinWith(string with, params string[] parts)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (String.IsNullOrEmpty(parts[i]))
                    continue;
                string trim = parts[i].Trim();
                if (trim.Length == 0)
                    continue;
                list.Add(trim);
            }
            return String.Join(with, list.ToArray());
        }

        /// <summary>
        /// Split string by new line
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] SplitByNewLine(string content)
        {
            return content.Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Test function delegate
        /// </summary>
        public delegate string ConvertAction(string value);

        public static string Convert(string value, ConvertAction action)
        {
            if (action == null)
                return value;

            return action(value);
        }

        public static string[] Convert(string[] array, ConvertAction action)
        {
            if (action == null)
                return array;

            List<string> a = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                a.Add(action(array[i]));
            }
            return a.ToArray();
        }

        private static char GetMiddleStringPatternChar(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return '\0';
            if (pattern.Length % 2 == 0)
            {
                return '\0';
            }
            else if (pattern.Length == 1)
            {
                return pattern[0];
            }
            else
            {
                int length = pattern.Length / 2;
                return pattern.Substring(length, 1)[0];
            }
        }

        private static string GetMiddleStringPrefix(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return pattern;
            if (pattern.Length % 2 == 0)
            {
                return pattern.Substring(0, pattern.Length / 2);
            }
            else if (pattern.Length == 1)
            {
                return pattern;
            }
            else
            {
                int length = pattern.Length / 2;
                return pattern.Substring(0, length);
            }
        }

        /// <summary>
        /// Return character or two characters string from the middle of text.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static string GetMiddleStringSuffix(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return pattern;
            int half = pattern.Length / 2;
            if (pattern.Length % 2 == 0)
            {
                return pattern.Substring(half, half);
            }
            else if (pattern.Length == 1)
            {
                return pattern;
            }
            else
            {
                int length = pattern.Length / 2;
                return pattern.Substring(half + 1, length);
            }
        }
    }
}
