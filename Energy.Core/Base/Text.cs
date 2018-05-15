﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Energy.Enumeration;

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

        #region Is

        /// <summary>
        /// Check if string contains one of wild characters ("*" or "?")
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>bool</returns>
        public static bool IsWild(string text)
        {
            return text.Contains("*") || text.Contains("?");
        }

        /// <summary>
        /// Check if string contains one of characters used in LIKE ("%" or "_")
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>bool</returns>
        public static bool IsLike(string text)
        {
            return text.Contains("%") || text.Contains("_");
        }

        #endregion

        #region Check

        public static bool Check(string input, MatchStyle matchStyle, MatchMode matchMode, bool ignoreCase, string[] filters)
        {
            switch (matchStyle)
            {
                default:
                    return false;
                case MatchStyle.All:
                    return CheckAll(input, matchMode, ignoreCase, filters);
                case MatchStyle.Not:
                    return CheckNot(input, matchMode, ignoreCase, filters);
                case MatchStyle.Any:
                    return CheckAny(input, matchMode, ignoreCase, filters);
                case MatchStyle.One:
                    return CheckOne(input, matchMode, ignoreCase, filters);
            }
        }

        public static bool CheckAny(string input, MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
                return false;
            foreach (string filter in filters)
            {
                if (Check(input, matchMode, ignoreCase, filter))
                    return true;
            }
            return false;
        }

        public static bool CheckAll(string input, MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
                return false;
            foreach (string filter in filters)
            {
                if (!Check(input, matchMode, ignoreCase, filter))
                    return false;
            }
            return true;
        }

        public static bool CheckNot(string input, MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
                return false;
            foreach (string filter in filters)
            {
                if (Check(input, matchMode, ignoreCase, filter))
                    return false;
            }
            return true;
        }

        public static bool CheckOne(string input, MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
                return false;
            bool found = false;
            foreach (string filter in filters)
            {
                if (Check(input, matchMode, ignoreCase, filter))
                {
                    if (found)
                        return false;
                    else
                        found = true;
                }
            }
            return found;
        }

        public static bool Check(string input, MatchMode matchMode, bool ignoreCase, string filter)
        {
            if (input == null || filter == null)
                return false;

            switch (matchMode)
            {
                default:
                case MatchMode.None:
                    return false;
                case MatchMode.Same:
                    return CheckSame(input, filter, ignoreCase);
                case MatchMode.Simple:
                    return CheckSimple(input, filter, ignoreCase);
                case MatchMode.Regex:
                    return CheckRegex(input, filter, ignoreCase);
                case MatchMode.Wild:
                    return CheckWild(input, filter, ignoreCase);
            }
        }

        [Energy.Attribute.Code.Wrapper]
        public static bool CheckRegex(string input, string pattern, RegexOptions options)
        {
            try
            {
                return Regex.Match(input, pattern, options).Success;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Catch(exception);
                return false;
            }
        }

        public static bool CheckRegex(string input, string pattern, bool ignoreCase)
        {
            RegexOptions options = RegexOptions.CultureInvariant;
            if (ignoreCase)
                options |= RegexOptions.IgnoreCase;
            return CheckRegex(input, pattern, options);
        }


        public static bool CheckWild(string input, string pattern, bool ignoreCase)
        {
            RegexOptions options = RegexOptions.CultureInvariant;
            if (ignoreCase)
                options |= RegexOptions.IgnoreCase;
            return CheckRegex(input, pattern, options);
        }

        public static bool CheckSame(string input, string filter, bool ignoreCase)
        {
            return 0 == string.Compare(input, filter, ignoreCase, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static bool CheckSimple(string input, string filter, bool ignoreCase)
        {
            if (0 == string.Compare(input, filter, ignoreCase, System.Globalization.CultureInfo.InvariantCulture))
                return true;
            if (!ignoreCase)
            {
                return input.StartsWith(filter) || input.EndsWith(filter);
            }
            else
            {
                string insensitiveInput = input.ToUpperInvariant();
                string insensitiveFilter = filter.ToUpperInvariant();
                if (insensitiveInput.StartsWith(insensitiveFilter))
                    return true;
                else if (insensitiveInput.EndsWith(insensitiveFilter))
                    return true;
                else
                    return false;
            }
        }

        #endregion

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

        #region Wild

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
        /// Convert string containing DOS wild characters into Regex pattern.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="like">bool</param>
        /// <returns></returns>
        [Energy.Attribute.Code.Verify]
        [Energy.Attribute.Code.Extend]
        public static string WildToRegex(string value, bool like)
        {
            if (!string.IsNullOrEmpty(value))
            {
                List<string> tab = new List<string>();
                tab.AddRange(new string[] {
                    ".", "\\.",
                    "(", "\\(",
                    "?", ".",
                    "*", ".*",
                });

                for (int i = 0; i < tab.Count; i++)
                {
                    value = value.Replace(tab[i], tab[i++ + 1]);
                }
            }
            return value;
        }

        #endregion

        #region Like

        /// <summary>
        /// Convert LIKE string into Regex pattern.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns></returns>
        [Energy.Attribute.Code.Verify]
        [Energy.Attribute.Code.Extend]
        public static string LikeToRegex(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                List<string> tab = new List<string>();
                tab.AddRange(new string[] {
                    ".", "\\.",
                    "*", "\\*",
                    "(", "\\(",
                    "_", ".",
                    "%", ".*",
                    "?", ".",
                    "*", ".*",
                });
                for (int i = 0; i < tab.Count; i++)
                {
                    value = value.Replace(tab[i], tab[i++ + 1]);
                }
            }
            return value;
        }

        #endregion

        /// <summary>
        /// Remove empty lines from string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [Energy.Attribute.Code.Verify]
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
        /// Return words as a string separated with dash (hyphen minus) character
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string DashCase(string[] words)
        {
            return string.Join("-", words);
        }

        /// <summary>
        /// Return words as a string separated with hyphen minus (dash) character
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

        /// <summary>
        /// Convert PascalCase to dash-case
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string PascalCaseToDashCase(string name)
        {
            string pattern = @"[A-Z]{2,}|[A-Z][^A-Z\s]*";
            MatchCollection matches = Regex.Matches(name, pattern);
            List<string> pieces = new List<string>();
            foreach (Match match in matches)
            {
                string value = match.Value;
                if (string.IsNullOrEmpty(value))
                    continue;
                value = value.Trim();
                if (value == "")
                    continue;
                pieces.Add(value);
            }
            string text = string.Join("-", pieces.ToArray());
            text = text.ToLowerInvariant();
            return text;
        }

        #endregion

        #region Unique

        /// <summary>
        /// Get unique array of strings
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

        /// <summary>
        /// Get unique array of strings case sensitive or not
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string[] Unique(string[] array, bool ignoreCase)
        {
            if (!ignoreCase)
                return Unique(array);
            if (array == null || array.Length <= 1)
                return array;
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>
            {
                array[0]
            };
            System.Collections.Generic.List<string> index = new System.Collections.Generic.List<string>
            {
                array[0] == null ? null
                    : array[0].ToLower(System.Globalization.CultureInfo.InvariantCulture)
            };
            for (int i = 1; i < array.Length; i++)
            {
                string needle = array[i] == null ? null
                    : array[i].ToLower(System.Globalization.CultureInfo.InvariantCulture);
                int n = list.IndexOf(needle);
                if (n < 0)
                {
                    index.Add(needle);
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        #endregion

        #region Encoding

        /// <summary>
        /// Find System.Text.Encoding for specified name.
        /// Get System.Text.Encoding.UTF8 by default or if encoding not exists.
        /// Treats UCS-2 the same as UTF-16 besides differences.
        /// could not be found.
        /// </summary>
        /// <param name="encoding">UTF-8, UTF, UTF8, UNICODE, UCS-2 LE, UCS-2 BE, 1250, 1252, ...</param>
        /// <returns>System.Text.Encoding</returns>
        public static System.Text.Encoding Encoding(string encoding)
        {
            if (string.IsNullOrEmpty(encoding))
            {
                return System.Text.Encoding.UTF8;
            }
            if (0 == string.Compare(encoding, "utf8", true))
            {
                return System.Text.Encoding.UTF8;
            }
            if (Regex.Match(encoding, @"^(?:unicode|utf-?16(?:\ ?le?)?|ucs(?:\ |-)?2(?:\ ?le?)?)$"
                , RegexOptions.IgnoreCase).Success)
            {
                return System.Text.Encoding.Unicode;
            }
            if (Regex.Match(encoding, @"^(?:utf-?16\ ?be?|ucs(?:\ |-)?2\ ?be?)$"
                , RegexOptions.IgnoreCase).Success)
            {
                return System.Text.Encoding.BigEndianUnicode;
            }            
            int number = Energy.Base.Cast.StringToInteger(encoding);
            try
            {
                if (number > 0)
                    return System.Text.Encoding.GetEncoding(number);
                else
                    return System.Text.Encoding.GetEncoding(encoding);
            }
            catch (ArgumentException)
            {
                return System.Text.Encoding.UTF8;
            }
            catch (NotSupportedException)
            {
                return System.Text.Encoding.UTF8;
            }
        }

        #endregion
    }
}
