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
        #region Constants

        private static string _BR = "<br>";
        /// <summary>HTML break</summary>
        public static string BR { get { return _BR; } private set { _BR = value; } }

        private static string _NL;
        /// <summary>New line "character"</summary>
        public static string NL
        {
            get
            {
                if (_NL == null)
                {
                    if (Environment.OSVersion.Platform.ToString().StartsWith("Win"))
                        _NL = "\r\n";
                    else
                        _NL = "\n";
                }
                return _NL;
            }
            private set
            {
                _NL = value;
            }
        }

        private static string _WS;
        /// <summary>Whitespace characters string</summary>
        public static string WS
        {
            get
            {
                if (_WS == null)
                {
                    _WS = " \t\r\n\v";
                }
                return _WS;
            }
            private set
            {
                _WS = value;
            }
        }

        #endregion

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

        #region Surround

        /// <summary>
        /// Surround text with delimiters if contains delimiter itself or any of special characters.
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

        /// <summary>
        /// Surround text with prefix and suffix, optionally adding prefix only when needed.
        /// </summary>
        /// <param name="text">Text to surround</param>
        /// <param name="prefix">Prefix to add at begining</param>
        /// <param name="suffix">Suffix to add at ending</param>
        /// <param name="optional">Add prefix and suffix only when needed</param>
        /// <returns></returns>
        public static string Surround(string text, string prefix, string suffix, bool optional)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }
            if (!optional)
            {
                return String.Concat(prefix, text, suffix);
            }
            else
            {
                if (!text.StartsWith(prefix))
                    text = prefix + text;
                if (!text.EndsWith(suffix))
                    text = text + suffix;
                return text;
            }
        }

        #endregion

        /// <summary>
        /// Repeat string pattern to specified amount of characters length.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Texture(string pattern, int size)
        {
            if (string.IsNullOrEmpty(pattern))
                return pattern;
            if (size == pattern.Length)
                return pattern;
            if (size < pattern.Length)
                return pattern.Substring(0, size);
            if (pattern.Length == 1)
                return new string(pattern[0], size);
            System.Text.StringBuilder s = new System.Text.StringBuilder(size);
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
            if (value == null || value.Length == 0)
            {
                return value;
            }
            else
            {
                value = value.Trim(' ', '\t', '\r', '\n', '\v', '\0');
                return value;
            }
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


        public static bool CheckWild(string input, string wild, bool ignoreCase)
        {
            RegexOptions options = RegexOptions.CultureInvariant;
            if (ignoreCase)
                options |= RegexOptions.IgnoreCase;
            string pattern = Energy.Base.Text.WildToRegex(wild);
            return CheckRegex(input, pattern, options);
        }

        public static bool CheckSame(string input, string filter, bool ignoreCase)
        {
            return 0 == string.Compare(input, filter, ignoreCase, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if input string is equal or starts or ends with filter string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
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
        public static string Join(string with, params string[] array)
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
            return string.Join(with, list.ToArray());
        }

        /// <summary>
        /// Join non empty strings into one list with separator
        /// </summary>
        /// <param name="with">Separator string</param>
        /// <param name="array">Parts to join</param>
        /// <returns>Example: JoinWith(" : ", "A", "B", "", "C") = "A : B : C".</returns>
        [Energy.Attribute.Code.Obsolete("Use shorter version Join()")]
        public static string JoinWith(string with, params string[] array)
        {
            return Join(with, array);
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
        /// Split string to array by new line characters.
        /// Elements will not include new line itself.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] SplitNewLine(string content)
        {
            return content.Split(_NewLine, StringSplitOptions.None);
        }

        /// <summary>
        /// Split string to array by new line characters.
        /// Elements will not include new line itself.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="removeEmptyEntries"></param>
        /// <returns></returns>
        public static string[] SplitNewLine(string content, bool removeEmptyEntries)
        {
            string[] split = content.Split(_NewLine
                , removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None
                );
            return split;
        }

        public static string[] Split(string input)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            foreach (string line in Each(input))
                list.Add(line);
            return list.ToArray();
        }

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

        #endregion

        #region Convert

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

        #endregion

        #region Contains

        /// <summary>
        /// Check if object as string contains searched string.
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static bool Contains(object o, string search)
        {
            if (o == null)
                return false;
            string str = o as string;
            if (str == null)
                str = o.ToString();
            return str.Contains(search);
        }

        #endregion

        #region MiddleString

        /// <summary>
        /// Get middle character from a pattern string.
        /// If the length of pattern text is even or empty, function will return an empty character.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static char GetMiddleStringPatternChar(string pattern)
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

        /// <summary>
        /// Get left part from a pattern string.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GetMiddleStringPrefix(string pattern)
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
        /// Get right part from a pattern string.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GetMiddleStringSuffix(string pattern)
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

        #endregion

        #region RemoveWhiteSpace

        /// <summary>
        /// Remove whitespace characters from entire string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveWhitespace(string value)
        {
            if (value == null || value.Length == 0)
                return value;
            string white = Energy.Base.Text.WS;
            char[] charArray = value.ToCharArray();
            if (!Energy.Base.Text.ContainsWhitespace(charArray))
            {
                return value;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (white.IndexOf(charArray[i]) >= 0)
                    continue;
                else
                    sb.Append(charArray[i]);
            }
            return sb.ToString();
        }

        #endregion

        #region ContainsWhitespace

        /// <summary>
        /// Check if array contains any of whitespace character.
        /// </summary>
        /// <param name="charArray"></param>
        /// <returns></returns>
        public static bool ContainsWhitespace(char[] charArray)
        {
            if (null == charArray)
                return false;
            if (0 == charArray.Length)
                return false;
            string white = Energy.Base.Text.WS;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (0 <= white.IndexOf(charArray[i]))
                    return true;
            }
            return false;
        }

        #endregion

        #region Limit

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
        /// Limit string to have maximum count characters with optional suffix if it was cut off.
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="limit">int</param>
        /// <param name="end"></param>
        /// <param name="with">string</param>
        /// <returns>string</returns>
        public static string Limit(string text, int limit, int end, string with)
        {
            if (limit < 0)
                return "";

            if (string.IsNullOrEmpty(text) || limit == 0 || text.Length <= limit)
            {
                return text;
            }
            else
            {
                string last = text.Substring(text.Length - end);
                string first = "";
                if (string.IsNullOrEmpty(with))
                {
                    first = text.Substring(0, limit - last.Length);
                }
                else
                {
                    limit -= with.Length;
                    first = string.Concat(text.Substring(0, limit - last.Length), with);
                }
                text = string.Concat(first, last);
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

        #endregion

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
        /// Convert string containing wild characters (*, ?) into Regex pattern.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns></returns>
        [Energy.Attribute.Code.Verify]
        [Energy.Attribute.Code.Extend]
        public static string WildToRegex(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                List<string> tab = new List<string>();
                tab.AddRange(new string[] {
                    "\\", "\\\\",
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

        #region RemoveEmptyLines

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
                result += Energy.Base.Text.NL;
            return result;
        }

        #endregion

        #region EscapeExpression

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

        #endregion

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

        #region Case

        /// <summary>
        /// Capitalize string by uppercasing the first letter, remaining the rest unchanged.
        /// </summary>
        /// <param name="word">Word</param>
        /// <returns>Word</returns>
        public static string UpperFirst(string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;
            if (word.Length == 1)
                return char.ToUpperInvariant(word[0]).ToString();
            return string.Concat(char.ToUpperInvariant(word[0])
                , word.Substring(1).ToLowerInvariant());
        }

        /// <summary>
        /// Upper case conversion for string array.
        /// </summary>
        /// <param name="words">Words list</param>
        /// <returns>Words list</returns>
        public static string[] Upper(string[] words)
        {
            if (words == null || words.Length == 0)
                return words;
            for (int i = 0; i < words.Length; i++)
            {
                if (null == words[i])
                    continue;
                words[i] = words[i].ToUpperInvariant();
            }
            return words;
        }

        /// <summary>
        /// Upper case conversion for string.
        /// </summary>
        /// <param name="word">Word</param>
        /// <returns>Words list</returns>
        public static string Upper(string word)
        {
            return word == null ? null : word.ToUpperInvariant();
        }

        /// <summary>
        /// Lower case conversion for string array.
        /// </summary>
        /// <param name="words">Words list</param>
        /// <returns>Words list</returns>
        public static string[] Lower(string[] words)
        {
            if (words == null || words.Length == 0)
                return words;
            for (int i = 0; i < words.Length; i++)
            {
                if (null == words[i])
                    continue;
                words[i] = words[i].ToLowerInvariant();
            }
            return words;
        }

        /// <summary>
        /// Lower case conversion for string.
        /// </summary>
        /// <param name="word">Word</param>
        /// <returns>Words list</returns>
        public static string Lower(string word)
        {
            return word == null ? null : word.ToLowerInvariant();
        }

        #endregion

        #region Naming convention

        /// <summary>
        /// Return words lower case, separated with hyphen character.
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
            if (words == null || words.Length == 0)
                return "";
            return string.Join("-", Lower(words));
        }

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string SnakeCase(string[] words)
        {
            if (words == null || words.Length == 0)
                return "";
            return string.Join("-", Lower(words));
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
                words[i] = UpperFirst(words[i]);
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
                words[i] = UpperFirst(words[i]);
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

        #region Newline endings

        /// <summary>
        /// Convert newline delimiter to specified one
        /// </summary>
        /// <param name="text"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public static string ConvertNewLine(string text, string newLine)
        {
            Regex re = new Regex(@"\r\n|\r|\n");
            text = re.Replace(text, newLine);
            return text;
        }

        /// <summary>
        /// Convert newline delimiter to environment default
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>string[]</returns>
        public static string ConvertNewLine(string text)
        {
            return ConvertNewLine(text, Energy.Base.Text.NL);
        }

        #endregion

        #region Uniquify

        /// <summary>
        /// Uniquify array of strings by adding sequential numbers
        /// and optional text between element and number.
        /// </summary>
        /// <param name="array">Array of strings to make unique</param>
        /// <param name="ignoreCase">Case insensitive</param>
        /// <param name="initialNumber">Initial number to add on duplicate</param>
        /// <param name="subText">Optional text between element and number</param>
        /// <returns></returns>
        public static string[] Uniquify(string[] array, bool ignoreCase, int initialNumber, string subText)
        {
            if (array == null || array.Length < 2)
                return array;
            Dictionary<string, int> next = new Dictionary<string, int>();
            List<string> index = new List<string>();
            List<string> output = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                string needle = ignoreCase ? Upper(array[i]) : array[i];
                if (index.Count == 0)
                {
                    index.Add(needle);
                    output.Add(array[i]);
                    continue;
                }
                if (index.Contains(needle))
                {
                    if (needle == null)
                        continue;
                    int number = initialNumber;
                    if (next.ContainsKey(needle))
                        number = next[needle];
                    while (true)
                    {
                        string candidate = string.Concat(array[i], subText, number++);
                        bool found = false;
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (0 == string.Compare(array[j], candidate, ignoreCase))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            output.Add(candidate);
                            index.Add(ignoreCase ? Upper(candidate) : candidate);
                            next[needle] = number;
                            break;
                        }
                    }
                }
                else
                {
                    output.Add(array[i]);
                    index.Add(needle);
                }
            }
            return output.ToArray();
        }

        /// <summary>
        /// Uniquify array of strings by adding sequential numbers
        /// after text.
        /// </summary>
        /// <param name="array">Array of strings to make unique</param>
        /// <param name="ignoreCase">Case insensitive</param>
        /// <param name="initialNumber">Initial number to add on duplicate</param>
        /// <returns></returns>
        public static string[] Uniquify(string[] array, bool ignoreCase, int initialNumber)
        {
            return Uniquify(array, ignoreCase, initialNumber, "");
        }

        /// <summary>
        /// Uniquify array of strings by adding sequential numbers
        /// after text.
        /// </summary>
        /// <param name="array">Array of strings to make unique</param>
        /// <param name="initialNumber">Initial number to add on duplicate</param>
        /// <returns></returns>
        public static string[] Uniquify(string[] array, int initialNumber)
        {
            return Uniquify(array, false, initialNumber, "");
        }

        /// <summary>
        /// Uniquify array of strings by adding sequential numbers
        /// and optional text between element and number.
        /// </summary>
        /// <param name="array">Array of strings to make unique</param>
        /// <param name="initialNumber">Initial number to add on duplicate</param>
        /// <param name="subText">Optional text between element and number</param>
        /// <returns></returns>
        public static string[] Uniquify(string[] array, int initialNumber, string subText)
        {
            return Uniquify(array, false, initialNumber, subText);
        }

        #endregion

        #region Compare

        /// <summary>
        /// Compare arrays of strings.
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static int Compare(string[] array1, string[] array2, bool ignoreCase)
        {
            if (array1 == null && array2 == null)
                return 0;
            if (array1 == null && array2 != null)
                return -1;
            if (array1 != null && array2 == null)
                return 1;
            if (array1.Length < array2.Length)
                return -1;
            if (array1.Length > array2.Length)
                return 1;
            for (int i = 0; i < array1.Length; i++)
            {
                int c = string.Compare(array1[i], array2[i], ignoreCase);
                if (c != 0)
                    return c;
            }
            return 0;
        }

        /// <summary>
        /// Compare arrays of strings.
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static int Compare(string[] array1, string[] array2)
        {
            return Compare(array1, array2, false);
        }

        #endregion

        #region Chop

        /// <summary>
        /// Cut first element of object array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T Chop<T>(ref T[] array)
        {
            if (array == null || array.Length == 0)
                return default(T);
            T first = array[0];
            List<T> list = new List<T>(array.Length);
            list.AddRange(array);
            list.RemoveAt(0);
            array = list.ToArray();
            return first;

        }

        /// <summary>
        /// Cut first element of string array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string Chop(ref string[] array)
        {
            return Chop<string>(ref array);
        }

        /// <summary>
        /// Cut first element of object array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static object Chop(ref object[] array)
        {
            return Chop<object>(ref array);
        }

        /// <summary>
        /// Get element from array if exists or empty if not.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetElementOrEmpty(string[] array, int index)
        {
            return GetElementOrEmpty<string>(array, index, "");
        }

        /// <summary>
        /// Get element from array if exists or empty if not.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="emptyValue"></param>
        /// <returns></returns>
        public static string GetElementOrEmpty(string[] array, int index, string emptyValue)
        {
            return GetElementOrEmpty<string>(array, index, emptyValue);
        }

        /// <summary>
        /// Get element from array if exists or empty if not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="emptyValue"></param>
        /// <returns></returns>
        public static T GetElementOrEmpty<T>(T[] array, int index, T emptyValue)
        {
            if (array == null || array.Length == 0)
                return emptyValue;
            else if (array.Length <= index)
                return emptyValue;
            else
                return array[index];
        }

        #endregion

        #region TryParse

        public static bool TryParse(string text, out bool boolean)
        {
            boolean = Energy.Base.Cast.StringToBool(text);
            return true;
        }

        private static bool? _TypeInt32HasTryParse = null;
        private static bool? _TypeUInt32HasTryParse = null;
        private static bool? _TypeInt64HasTryParse = null;
        private static bool? _TypeUInt64HasTryParse = null;

        public static bool TryParse(string text, out int signedInteger)
        {
            if (text == null || text.Length == 0)
            {
                signedInteger = 0;
                return false;
            }
            if (_TypeInt32HasTryParse == null)
            {
                Type type = typeof(int);
                System.Reflection.MemberInfo method = type.GetMethod("TryParse"
                    , System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public
                    , null
                    , new Type[] { typeof(string), type.MakeByRefType() } // Method TryParse() with 2 parameters
                    , null
                    );
                _TypeInt32HasTryParse = method != null;
            }
            if ((bool)_TypeInt32HasTryParse)
            {
                return int.TryParse(text, out signedInteger);
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (i == 0 && text[0] == '-')
                    {
                        if (text.Length == 1)
                        {
                            signedInteger = 0;
                            return false;
                        }
                        continue;
                    }
                    if (text[i] < '0' || text[i] > '9')
                    {
                        signedInteger = 0;
                        return false;
                    }
                }
                try
                {
                    signedInteger = System.Convert.ToInt32(text);
                    return true;
                }
                catch (FormatException)
                {
                    signedInteger = 0;
                    return false;
                }
                catch (OverflowException)
                {
                    signedInteger = 0;
                    return false;
                }
            }
        }

        public static bool TryParse(string text, out uint unsignedInteger)
        {
            if (text == null || text.Length == 0)
            {
                unsignedInteger = 0;
                return false;
            }
            if (_TypeUInt32HasTryParse == null)
            {
                Type type = typeof(uint);
                System.Reflection.MemberInfo method = type.GetMethod("TryParse"
                    , System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public
                    , null
                    , new Type[] { typeof(string), type.MakeByRefType() } // Method TryParse() with 2 parameters
                    , null
                    );
                _TypeInt32HasTryParse = method != null;
            }
            if ((bool)_TypeInt32HasTryParse)
            {
                return uint.TryParse(text, out unsignedInteger);
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] < '0' || text[i] > '9')
                    {
                        unsignedInteger = 0;
                        return false;
                    }
                }
                try
                {
                    unsignedInteger = System.Convert.ToUInt32(text);
                    return true;
                }
                catch (FormatException)
                {
                    unsignedInteger = 0;
                    return false;
                }
                catch (OverflowException)
                {
                    unsignedInteger = 0;
                    return false;
                }
            }
        }

        public static bool TryParse(string text, out long signedLong)
        {
            if (text == null || text.Length == 0)
            {
                signedLong = 0;
                return false;
            }
            if (_TypeInt64HasTryParse == null)
            {
                Type type = typeof(long);
                System.Reflection.MemberInfo method = type.GetMethod("TryParse"
                    , System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public
                    , null
                    , new Type[] { typeof(string), type.MakeByRefType() } // Method TryParse() with 2 parameters
                    , null
                    );
                _TypeInt64HasTryParse = method != null;
            }
            if ((bool)_TypeInt64HasTryParse)
            {
                return long.TryParse(text, out signedLong);
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (i == 0 && text[0] == '-')
                    {
                        if (text.Length == 1)
                        {
                            signedLong = 0;
                            return false;
                        }
                        continue;
                    }
                    if (text[i] < '0' || text[i] > '9')
                    {
                        signedLong = 0;
                        return false;
                    }
                }
                try
                {
                    signedLong = System.Convert.ToInt64(text);
                    return true;
                }
                catch (FormatException)
                {
                    signedLong = 0;
                    return false;
                }
                catch (OverflowException)
                {
                    signedLong = 0;
                    return false;
                }
            }
        }

        public static bool TryParse(string text, out ulong unsignedLong)
        {
            if (text == null || text.Length == 0)
            {
                unsignedLong = 0;
                return false;
            }
            if (_TypeUInt64HasTryParse == null)
            {
                Type type = typeof(ulong);
                System.Reflection.MemberInfo method = type.GetMethod("TryParse"
                    , System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public
                    , null
                    , new Type[] { typeof(string), type.MakeByRefType() } // Method TryParse() with 2 parameters
                    , null
                    );
                _TypeUInt64HasTryParse = method != null;
            }
            if ((bool)_TypeUInt64HasTryParse)
            {
                return ulong.TryParse(text, out unsignedLong);
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] < '0' || text[i] > '9')
                    {
                        unsignedLong = 0;
                        return false;
                    }
                }
                try
                {
                    unsignedLong = System.Convert.ToUInt32(text);
                    return true;
                }
                catch (FormatException)
                {
                    unsignedLong = 0;
                    return false;
                }
                catch (OverflowException)
                {
                    unsignedLong = 0;
                    return false;
                }
            }
        }

        #endregion

        #region Quote

        /// <summary>
        /// Surround text with quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Quote(string text)
        {
            return Quote(text, "\"", "\"");
        }

        /// <summary>
        /// Surround text with quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        public static string Quote(string text, string with)
        {
            return Quote(text, with, with);
        }

        /// <summary>
        /// Surround text with quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Quote(string text, string with, string escape)
        {
            if (text == null || text.Length == 0)
                return string.Concat(with, with);

            if (text.Contains(with))
            {
                return string.Concat(with, text.Replace(with, escape + with), with);
            }
            else
            {
                return string.Concat(with, text, with);
            }
        }

        /// <summary>
        /// Surround text with quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <param name="escape"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static string Quote(string text, string with, string escape, bool optional)
        {
            if (optional && !text.Contains(with))
                return text;
            return Quote(text, with, escape);
        }


        /// <summary>
        /// Surround text with quotation characters.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Quote(object value)
        {
            string text = Energy.Base.Cast.AsString(value);
            return Quote(text, "\"", "\"");
        }

        /// <summary>
        /// Surround text with quotation characters.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        public static string Quote(object value, string with)
        {
            string text = Energy.Base.Cast.AsString(value);
            return Quote(text, with, with);
        }

        #endregion

        #region Strip

        /// <summary>
        /// Strip text from quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strip(string text)
        {
            return Strip(text, "\"", "\"");
        }

        /// <summary>
        /// Strip text from quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        public static string Strip(string text, string with)
        {
            return Strip(text, with, with);
        }

        /// <summary>
        /// Strip text from quotation characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Strip(string text, string with, string escape)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            int a = 0;
            int b = text.Length;
            if (text.StartsWith(with))
            {
                a = with.Length;
                b -= with.Length;
            }
            if (text.EndsWith(with))
            {
                b -= with.Length;
            }
            string cut = text.Substring(a, b);
            return cut.Replace(escape + with, with);
        }

        #endregion

        #region InArray

        /// <summary>
        /// Check if string element is a part of string array.
        /// If array is null or empty, function will result false.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="element"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool InArray(string[] array, string element, bool ignoreCase)
        {
            if (array == null || array.Length == 0)
                return false;
            if (element == null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (0 == string.Compare(array[i], element, ignoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Check if string element is a part of string array.
        /// If array is null or empty, function will result false.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool InArray(string[] array, string element)
        {
            return InArray(array, element, false);
        }

        #endregion

        #region Editor

        public class Editor
        {
            public string InsertBeforeFirstLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string AppendAfterFirstLine(string text, string line)
            {
                if (string.IsNullOrEmpty(line))
                    return text;
                if (string.IsNullOrEmpty(text))
                {
                    if (text == null && line == null)
                        return null;
                }
                int p = Energy.Base.Text.IndexOfAny(text, _NewLine);
                if (p < 0)
                {
                    return string.Concat(text ?? "", line ?? "");
                }
                else
                {
                    string result = ""
                        + text.Substring(0, p) + line + text.Substring(p)
                        ;
                    return result;
                }
            }

            public string InsertBeforeSecondLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string InsertBeforeLastLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string AppendAfterLastLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string GetFirstLine(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                int p = Energy.Base.Text.IndexOfAny(text, _NewLine);
                if (p < 0)
                {
                    return text;
                }
                else
                {
                    return text.Substring(0, p);
                }
            }

            public string GetLastLine(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                int p = Energy.Base.Text.AfterOfAny(text, _NewLine);
                if (p < 0)
                {
                    return text;
                }
                else
                {
                    return text.Substring(p);
                }
            }

            public string EnsureNewLineAtEnd(string text)
            {
                string[] nll = _NewLine;
                if (nll == null || nll.Length == 0)
                {
                    nll = new string[] { Environment.NewLine };
                }
                if (string.IsNullOrEmpty(text))
                {
                    text = nll[0];
                }
                else
                {
                    foreach (string nl in nll)
                    {
                        if (text.EndsWith(nl))
                            return text;
                    }
                    text = string.Concat(text, nll[0]);
                }
                return text;
            }
        }

        #endregion

        #region IndexOfAny

        public static int IndexOfAny(string text, string[] any)
        {
            int m = -1;
            for (int i = 0; i < any.Length; i++)
            {
                int p = text.IndexOf(any[i]);
                if (p < 0)
                    continue;
                if (m >= 0 && p > m)
                    continue;
                else
                    m = p;
            }
            return m;
        }

        #endregion

        #region LastOfAny

        public static int LastOfAny(string text, string[] any)
        {
            int m = -1;
            for (int i = 0; i < any.Length; i++)
            {
                int p = text.LastIndexOf(any[i]);
                if (p < 0)
                    continue;
                if (p < m)
                    continue;
                else
                    m = p;
            }
            return m;
        }

        #endregion

        #region AfterOfAny

        public static int AfterOfAny(string text, string[] any)
        {
            int m = -1;
            for (int i = 0; i < any.Length; i++)
            {
                int p = text.LastIndexOf(any[i]);
                if (p < 0)
                    continue;
                p += any[i].Length;
                if (p < m)
                    continue;
                else
                    m = p;
            }
            return m;
        }

        #endregion
    }
}
