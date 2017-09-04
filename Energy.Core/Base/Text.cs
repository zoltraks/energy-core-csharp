using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        /// Split string by new line.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] SplitNewLine(string content)
        {
            return content.Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Split string to array by separators with optional quoted elements.
        /// May be used to explode from strings like "1,2,3", "abc def xyz", "'Smith''s Home'|'Special'".
        /// </summary>
        /// <returns></returns>
        public static string[] SplitList(string text, string commas, string quotes)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(quotes))
            {
                foreach (char c in quotes.ToCharArray())
                {
                    string quote = Regex.Escape(c.ToString()).Replace(@"\ ", @"\s");
                    string escape = Regex.Escape(new string(c, 2)).Replace(@"\ ", @"\s");
                    list.Add(string.Concat(quote, "(?:", escape, "|[^", quote, "])*", quote));
                }
            }
            string separator = "";
            if (string.IsNullOrEmpty(commas) || commas == " ")
            {
                list.Add(@"[^\s]+");
                separator = @"(?:\s*)?";
            }
            else
            {
                string escape = Regex.Escape(commas).Replace(@"\ ", @"\s");
                list.Add(string.Concat("[^", commas, "]+"));
                separator = string.Concat(@"(?:\s*[", escape, @"]\s*)?");
            }
            string join = string.Join("|", list.ToArray());
            string pattern = string.Concat(@"(?<1>", join, ")", separator);

            list.Clear();

            Match match;
            Match next = Regex.Match(text, pattern);
            while ((match = next).Success)
            {
                next = match.NextMatch();
                string value = match.Groups[1].Value;
                if (value == null)
                    continue;
                value = value.Trim();
                if (value.Length == 0)
                    continue;
                if (!string.IsNullOrEmpty(quotes))
                {
                    foreach (char c in quotes.ToCharArray())
                    {
                        string quote = c.ToString();
                        if (!value.StartsWith(quote) || !value.EndsWith(quote))
                            continue;
                        string escape = new string(c, 2);
                        value = value.Substring(1, value.Length - 2).Replace(escape, quote);
                    }
                }
                list.Add(value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Split string by new line
        /// </summary>
        /// <returns></returns>
        public static string[] SplitDictionary(string text, string quotes, string equalities, string brackets)
        {
            //return content.Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
            return null;
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

        /// <summary>
        /// Limit string to have maximum count of characters.
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="limit">int</param>
        /// <returns>string</returns>
        public static string Limit(string text, int limit)
        {
            if (limit < 0)
                return "";

            if (string.IsNullOrEmpty(text) || limit == 0 || text.Length <= limit)
            {
                return text;
            }
            else
            {
                return text.Substring(0, limit);
            }
        }

        /// <summary>
        /// Limit string to have maximum count characters with optional suffix if it was cut off.
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="limit">int</param>
        /// <param name="suffix">string</param>
        /// <returns>string</returns>
        public static string Limit(string text, int limit, string suffix)
        {
            if (limit < 0)
                return "";

            if (string.IsNullOrEmpty(text) || limit == 0 || text.Length <= limit)
            {
                return text;
            }
            else
            {
                if (string.IsNullOrEmpty(suffix))
                {
                    text = text.Substring(0, limit);
                }
                else
                {
                    limit -= suffix.Length;
                    text = string.Concat(text.Substring(0, limit), suffix);
                }
                return text;
            }
        }

        /// <summary>
        /// Limit string to have maximum count characters with optional prefix or suffix if it was cut off.
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="limit">int</param>
        /// <param name="prefix">string</param>
        /// <param name="suffix">string</param>
        /// <returns>string</returns>
        public static string Limit(string text, int limit, string prefix, string suffix)
        {
            if (limit < 0)
                return "";

            if (string.IsNullOrEmpty(text) || limit == 0 || text.Length <= limit)
            {
                return text;
            }
            else
            {
                int length = limit;
                int start = 0;
                if (!string.IsNullOrEmpty(prefix))
                {
                    length -= prefix.Length;
                    start += prefix.Length;
                }
                if (!string.IsNullOrEmpty(suffix))
                {
                    length -= suffix.Length;
                }
                text = string.Concat(prefix, text.Substring(start, length), suffix);
                return text;
            }
        }

        /// <summary>
        /// Convert text containing wild characters (?, *) to SQL like format (_, %).
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>string</returns>
        public static string WildToLike(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            string[] replacement = new string[] 
            {
                "*", "%",
                "?", "_",
            };
            for (int i = 0; i < replacement.Length; i++)
            {
                text = text.Replace(replacement[i], replacement[i++ + 1]);
            }
            return text;
        }
    }
}
