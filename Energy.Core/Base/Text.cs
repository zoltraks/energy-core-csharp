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
        public static string Trim(string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim(' ', '\t', '\r', '\n', '\v', '\0');
        }

        #region Join

        /// <summary>
        /// Join non empty strings into one list with separator
        /// </summary>
        /// <param name="with">Separator string</param>
        /// <param name="array">Parts to join</param>
        /// <returns>Example: JoinWith(" : ", "A", "B", "", "C") = "A : B : C".</returns>
        public static string JoinWith(string with, params string[] array)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                if (String.IsNullOrEmpty(array[i]))
                    continue;
                string trim = array[i].Trim();
                if (trim.Length == 0)
                    continue;
                list.Add(trim);
            }
            return String.Join(with, list.ToArray());
        }

        #endregion

        #region Each

        public static IEnumerable<string> Each(string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (System.IO.StringReader reader = new System.IO.StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        #endregion

        #region Split

        private static readonly string[] _NewLine = new string[] { "\r\n", "\n", "\r" };

        /// <summary>
        /// Split string to array by new line characters. Elements will not include new line itself.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] SplitNewLine(string content)
        {
            return content.Split(_NewLine, StringSplitOptions.None);
        }

        public static string[] Split(string input)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            foreach (string line in Each(input))
                list.Add(line);
            return list.ToArray();
        }

        #endregion

        /// <summary>
        /// Split string to array by separators with optional quoted elements.
        /// May be used to explode from strings like "1,2,3", "abc def xyz", "'Smith''s Home'|'Special | New'|Other value".
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

        /// <summary>
        /// Remove empty lines from string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveEmptyLines(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            bool eol = text.EndsWith("\r\n") || text.EndsWith("\n") || text.EndsWith("\r");
            string pattern = @"^[\s\t\v\ ]*(?:\r\n|\n|\r)+";
            string result = Regex.Replace(text, pattern, "", RegexOptions.Multiline);
            if (eol)
                result += Environment.NewLine;
            return result;
        }

        private static string[] _EscapeExpressionStringArray;

        private static string[] EscapeExpressionStringArray
        {
            get
            {
                if (_EscapeExpressionStringArray == null)
                {
                    _EscapeExpressionStringArray = new string[]
                    {
                        ".", "$", "^", "{", "[", "(", "|", ")", "*", "+", "?", "|", "\\",
                    };
                }
                return _EscapeExpressionStringArray;
            }
        }

        private static Dictionary<string, string> _EscapeExpressionStringDictionary;

        private static Dictionary<string, string> EscapeExpressionStringDictionary
        {
            get
            {
                if (_EscapeExpressionStringDictionary == null)
                {
                    _EscapeExpressionStringDictionary = new Dictionary<string, string>();
                    _EscapeExpressionStringDictionary.Add("\\", @"\\");
                    _EscapeExpressionStringDictionary.Add(".", @"\.");
                    _EscapeExpressionStringDictionary.Add(" ", @"\ ");
                    _EscapeExpressionStringDictionary.Add("\t", @"\t");
                    _EscapeExpressionStringDictionary.Add("\r", @"\r");
                    _EscapeExpressionStringDictionary.Add("\n", @"\n");
                    _EscapeExpressionStringDictionary.Add("$", @"\$");
                    _EscapeExpressionStringDictionary.Add("^", @"\^");
                    _EscapeExpressionStringDictionary.Add("*", @"\*");
                    _EscapeExpressionStringDictionary.Add("?", @"\?");
                    _EscapeExpressionStringDictionary.Add("+", @"\+");
                    _EscapeExpressionStringDictionary.Add("|", @"\+");
                    _EscapeExpressionStringDictionary.Add("{", @"\{");
                    _EscapeExpressionStringDictionary.Add("[", @"\[");
                    _EscapeExpressionStringDictionary.Add("(", @"\(");
                    _EscapeExpressionStringDictionary.Add("}", @"\}");
                    _EscapeExpressionStringDictionary.Add("]", @"\]");
                    _EscapeExpressionStringDictionary.Add(")", @"\)");
                }
                return _EscapeExpressionStringDictionary;
            }
        }

        /// <summary>
        /// Escape text for regular expression.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EscapeExpression(string text)
        {
            System.Text.StringBuilder s = null;
            foreach (KeyValuePair<string, string> _ in EscapeExpressionStringDictionary)
            {
                if (text.Contains(_.Key))
                {
                    if (s == null)
                        s = new System.Text.StringBuilder(text);
                    s.Replace(_.Key, _.Value);

                }
            }
            return s == null ? text : s.ToString();
        }

        #region Random

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <returns></returns>
        public static string Random()
        {
            return Energy.Base.Random.GetRandomText();
        }

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <param name="available">Available characters for generating random text</param>
        /// <param name="minimum">Minimum number of characters</param>
        /// <param name="maximum">Maximum number of characters</param>
        /// <returns></returns>
        public static string Random(string available, int minimum, int maximum)
        {
            return Energy.Base.Random.GetRandomText(available, minimum, maximum);
        }

        #endregion

        #region Join

        public static string Join(string glue, string format, params object[][] array)
        {
            if (array == null)
                return null;

            int count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null || array[i].Length == 0)
                    continue;
                if (array[i].Length > count)
                    count = array[i].Length;
            }

            List<string> list = new List<string>();
            List<string> args = new List<string>();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (array[j] != null && array[j].Length > i)
                        args.Add(Energy.Base.Cast.ObjectToString(array[j][i]));
                    else
                        args.Add("");
                }
                list.Add(string.Format(format, args.ToArray()));
                args.Clear();
            }
            return string.Join(glue, list.ToArray());
        }

        #endregion

        #region Naming convention

        /// <summary>
        /// Capitalize string by uppercasing the first letter,
        /// remaining the rest unchanged
        /// </summary>
        /// <remarks>
        /// You will want to call String.ToLower() before
        /// calling this method
        /// </remarks>
        /// <param name="word">Word</param>
        /// <returns>Word</returns>
        public static string UppercaseFirst(string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;
            if (word.Length == 0)
                return char.ToUpperInvariant(word[0]).ToString();
            return string.Concat(char.ToUpperInvariant(word[0])
                , word.Substring(1).ToLowerInvariant());
        }

        /// <summary>
        /// Return words as a string separated with hyphen character
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string HyphenCase(string[] words)
        {
            return string.Join("-", words);
        }

        /// <summary>
        /// Return words as a string separated with underscore character
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string UnderscoreCase(string[] words)
        {
            return string.Join("_", words);
        }

        /// <summary>
        /// Return words using medial capitalization
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string PascalCase(string[] words)
        {
            if (words == null || words.Length == 0)
                return "";
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = UppercaseFirst(words[i]);
            }
            return string.Join("", words);
        }

        /// <summary>
        /// Return words using medial capitalization in optional java
        /// style of camel case
        /// </summary>
        /// <remarks>
        /// Example of getting "camelGoesFirst" word
        /// <pre>
        ///     string[] words = { "Camel", "gOES", "fIrSt" };
        ///     MessageBox.Show(Core.Transform.CamelCase(words, true));
        /// </pre>
        /// </remarks>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string CamelCase(string[] words)
        {
            if (words == null || words.Length == 0)
                return "";
            words[0] = words[0].ToLowerInvariant();
            for (int i = 1; i < words.Length; i++)
            {
                    words[i] = UppercaseFirst(words[i]);
            }
            return string.Join("", words);
        }

        #endregion

        #region Unique

        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] Unique(string[] array)
        {
            if (array == null || array.Length <= 1)
                return array;
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>
            {
                array[0]
            };
            for (int i = 1; i < array.Length; i++)
            {
                int index = list.IndexOf(array[i]);
                if (index >= 0)
                    list[index] = array[i];
                else
                    list.Add(array[i]);
            }
            return list.ToArray();
        }

        #endregion
    }
}
