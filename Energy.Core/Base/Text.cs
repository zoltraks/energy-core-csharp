using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Energy.Base
{
    /// <summary>
    /// Text related functions.
    /// </summary>
    /// <remarks>
    /// There was a small question whether the class name should be renamed from **Energy.Base.Text** to something else to avoid possible conflicts with **System.Text** 
    /// when anyone wants to add **Energy.Base** namespace to *using* list.
    /// It was decided to keep it as it is while recommending using full class names 
    /// in *using* list and synonyms as well.
    /// </remarks>
    public static class Text
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

        /// <summary>
        /// An array of empty texts containing end-of-line characters.
        /// </summary>
        public static readonly string[] NEWLINE_ARRAY = new string[] { "\r\n", "\n", "\r" };

        /// <summary>
        /// Regular expressions pattern for new line.
        /// </summary>
        public const string NEWLINE_PATTERN = "\r\n|\n|\r";

        /// <summary>
        /// Regular expressions pattern for new line.
        /// Greedy version.
        /// </summary>
        public const string NEWLINE_GREEDY_PATTERN = "(?:\r\n|\n|\r)+";

        #endregion

        #region Private

        private static Dictionary<Class.ControlStringOptions, string> _ControlStringExpressionCache;

        #endregion

        #region Class

        public class Class
        {
            public struct ControlStringOptions
            {
                public char Quote;

                public char Escape;

                public string[] DecimalPrefix;

                public string[] HexadecimalPrefix;

                public string[] OctalPrefix;

                public string[] BinaryPrefix;

                public bool Wide;

                /// <summary>
                /// Allow whitespace between text and character codes
                /// </summary>
                public bool White;

                /// <summary>
                /// Include whitespace into resulting text (not likely useful).
                /// </summary>
                public bool IncludeWhite;

                /// <summary>
                /// Include not recognised sequences in resulting text (not likely useful).
                /// </summary>
                public bool IncludeUnknown;
            }
        }

        #endregion

        #region Quotation

        /// <summary>
        /// Quotation definition
        /// </summary>
        public class Quotation
        {
            /// <summary>
            /// Quotation prefix
            /// </summary>
            public string Prefix;

            /// <summary>
            /// Quotation suffix
            /// </summary>
            public string Suffix;

            /// <summary>
            /// Possible special character sequences that 
            /// allows to use suffix or other characters inside quoted text
            /// </summary>
            public string[] Escape;

            /// <summary>
            /// Create quotation definition object from a string. If string is null or empty, null will be returned.
            /// <br/><br/>
            /// If string contains only 1 character, it would be treated as prefix and suffix character,
            /// double suffix character will be used as escape sequence.
            /// If definition contains spaces but not starts with, it will be splited by it. 
            /// First element of such array will be used as prefix, last one as suffix,
            /// and all elements between them will be treated as escape sequences.
            /// If the number of characters is even, first half will be treated as prefix, second as suffix,
            /// double suffix will be treated as escape sequence.
            /// If the number of characters is odd, middle character will be treated as escape character for
            /// suffix sequence of characters. It's common to use backslash there.
            /// <br/><br/>
            /// Examples: 
            /// <br/>
            /// "'" (apostrophe will be used as prefix and suffix, double apostrophes are allowed),
            /// <br/>
            /// "$%" (dollar will be used as prefix and percentage as suffix, double percentages are allowed),
            /// <br/>
            /// "[[]]" ([[ will be used as prefix and ]] as suffix, ]]]] will be treated as ]]),
            /// <br/>
            /// "%/%" (percentage will be used as prefix and suffix, / will be treated as escape character, so /% sequence is allowed).
            /// </summary>
            /// <param name="definition"></param>
            /// <returns></returns>
            public static Quotation From(string definition)
            {
                if (string.IsNullOrEmpty(definition))
                {
                    return null;
                }
                Quotation o = new Quotation();
                if (1 == definition.Length)
                {
                    o.Prefix = definition;
                    o.Suffix = definition;
                    o.Escape = new string[] { definition + definition };
                }
                else if (0 < definition.IndexOf(' '))
                {
                    string[] a = definition.Split(' ');
                    switch (a.Length)
                    {
                        case 1:
                            o.Prefix = a[0];
                            o.Suffix = a[0];
                            o.Escape = new string[] { a[0] + a[0] };
                            break;
                        case 2:
                            o.Prefix = a[0];
                            o.Suffix = a[1];
                            o.Escape = new string[] { a[1] + a[1] };
                            break;
                        default:
                            o.Prefix = a[0];
                            o.Suffix = a[a.Length - 1];
                            List<string> l = new List<string>();
                            for (int i = 1; i < a.Length - 1; i++)
                            {
                                l.Add(a[i]);
                            }
                            o.Escape = l.ToArray();
                            break;
                    }
                }
                else if (0 == definition.Length % 2)
                {
                    int h = definition.Length / 2;
                    string suffix = definition.Substring(h, h);
                    o.Prefix = definition.Substring(0, h);
                    o.Suffix = suffix;
                    o.Escape = new string[] { suffix + suffix };
                }
                else
                {
                    int h = definition.Length / 2;
                    string suffix = definition.Substring(h + 1, h);
                    o.Prefix = definition.Substring(0, h);
                    o.Suffix = suffix;
                    o.Escape = new string[] { definition.Substring(h, 1) + suffix };
                }
                return o;
            }

            public override string ToString()
            {
                List<string> l = new List<string>();
                if (null != Prefix)
                {
                    l.Add(Prefix);
                }
                if (null != Escape)
                {
                    for (int i = 0; i < Escape.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Escape[i]))
                        {
                            l.Add(Escape[i]);
                        }
                    }
                }
                if (null != Suffix)
                {
                    l.Add(Suffix);
                }
                return string.Join(" ", l.ToArray());
            }
        }

        #endregion

        #region Exchange

        /// <summary>
        /// Exchange texts between each other.
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
        /// Exchange characters between each other.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static void Exchange(ref char first, ref char second)
        {
            char remember = first;
            first = second;
            second = remember;
        }

        #endregion

        #region Select

        /// <summary>
        /// Select first non empty string element.
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

        #endregion

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
            {
                return value;
            }

            bool quote = false;

            if (String.IsNullOrEmpty(delimiter))
            {
                if (special == null)
                {
                    return value;
                }
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
            {
                return value;
            }

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
// Warning code IDE0054 is invalid number in Visual Studio 2008
#pragma warning disable IDE0054 // Use compound assignment
                    text = text + suffix;
#pragma warning restore IDE0054 // Use compound assignment
                return text;
            }
        }

        #endregion

        #region Texture

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

        #endregion

        #region Trim

        private static readonly char[] whiteCharacters = new char[] { ' ', '\t', '\n', '\r', '\v', '\f', '\0' };

        /// <summary>
        /// Remove all leading and trailing whitespace characters from text.
        /// <br/><br/>
        /// Following characters are treated as whitespace: space character " " (code 32),
        /// horizontal tab "\t" (code 09), line feed "\n" (code 10),
        /// carriage return "\r" (code 13), vertical tab "\v" (code 11),
        /// form feed "\f" (code 12), null character "\0" (code 0).
        /// </summary>
        /// <remarks>EBS-0</remarks>
        /// <param name="text">String value</param>
        /// <returns>Trimmed string</returns>
        public static string Trim(string text)
        {
            if (null == text || 0 == text.Length)
            {
                return text;
            }
            if (0 <= text.IndexOfAny(whiteCharacters))
            {
                //value = value.Trim(' ', '\t', '\n', '\r', '\v', '\f', '\0');
                text = text.Trim(whiteCharacters);
            }
            return text;
        }

        #endregion

        #region Is

        /// <summary>
        /// Check if string contains one of wild characters ("*" or "?").
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>bool</returns>
        public static bool IsWild(string text)
        {
            return text.Contains("*") || text.Contains("?");
        }

        /// <summary>
        /// Check if string contains one of characters used in LIKE ("%" or "_").
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>bool</returns>
        public static bool IsLike(string text)
        {
            return text.Contains("%") || text.Contains("_");
        }

        /// <summary>
        /// Check if string is null, empty or contains only whitespace.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsWhite(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;
            else if (0 == text.Trim(new char[] { ' ', '\r', '\n', '\t', '\v' }).Length)
                return true;
            else
                return false;
        }

        #endregion

        #region Check

        public static bool Check(string input, Energy.Enumeration.MatchStyle matchStyle, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, string[] filters)
        {
            switch (matchStyle)
            {
                default:
                    return false;
                case Energy.Enumeration.MatchStyle.All:
                    return CheckAll(input, matchMode, ignoreCase, filters);
                case Energy.Enumeration.MatchStyle.Not:
                    return CheckNot(input, matchMode, ignoreCase, filters);
                case Energy.Enumeration.MatchStyle.Any:
                    return CheckAny(input, matchMode, ignoreCase, filters);
                case Energy.Enumeration.MatchStyle.One:
                    return CheckOne(input, matchMode, ignoreCase, filters);
            }
        }

        public static bool CheckAny(string input, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
            {
                return false;
            }
            foreach (string filter in filters)
            {
                if (Check(input, matchMode, ignoreCase, filter))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckAll(string input, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
            {
                return false;
            }
            foreach (string filter in filters)
            {
                if (!Check(input, matchMode, ignoreCase, filter))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckNot(string input, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
            {
                return false;
            }
            foreach (string filter in filters)
            {
                if (Check(input, matchMode, ignoreCase, filter))
                    return false;
            }
            return true;
        }

        public static bool CheckOne(string input, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, params string[] filters)
        {
            if (input == null || filters == null)
            {
                return false;
            }
            bool found = false;
            foreach (string filter in filters)
            {
                if (Check(input, matchMode, ignoreCase, filter))
                {
                    if (found)
                    {
                        return false;
                    }
                    else
                    {
                        found = true;
                    }
                }
            }
            return found;
        }

        public static bool Check(string input, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, string filter)
        {
            if (input == null || filter == null)
            {
                return false;
            }
            switch (matchMode)
            {
                default:
                case Energy.Enumeration.MatchMode.None:
                    return false;
                case Energy.Enumeration.MatchMode.Same:
                    return CheckSame(input, filter, ignoreCase);
                case Energy.Enumeration.MatchMode.Simple:
                    return CheckSimple(input, filter, ignoreCase);
                case Energy.Enumeration.MatchMode.Regex:
                    return CheckRegex(input, filter, ignoreCase);
                case Energy.Enumeration.MatchMode.Wild:
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
                string insensitiveInput = input.ToUpper(CultureInfo.InvariantCulture);
                string insensitiveFilter = filter.ToUpper(CultureInfo.InvariantCulture);
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
        /// Join non empty and optionally empty strings into one list with separator.
        /// For example Energy.Base.Text.Join(" : ", false, "A", "B", "", "C") will return "A : B : C".
        /// </summary>
        /// <param name="glue">Separator string</param>
        /// <param name="empty">Include empty values</param>
        /// <param name="array">Parts to join</param>
        /// <returns></returns>
        public static string Join(string glue, bool empty, params string[] array)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                if (string.IsNullOrEmpty(array[i]))
                {
                    if (empty)
                    {
                        list.Add("");
                    }
                    continue;
                }
                string trim = array[i].Trim();
                if (trim.Length == 0 && !empty)
                    continue;
                list.Add(trim);
            }
            return string.Join(glue, list.ToArray());
        }

        /// <summary>
        /// Join strings into one list with separator.
        /// For example Energy.Base.Text.Join(" : ", "A", "B", "", "C") will return "A : B : : C".
        /// </summary>
        /// <param name="glue">Separator string</param>
        /// <param name="array">Parts to join</param>
        /// <returns></returns>
        public static string Join(string glue, params string[] array)
        {
            return Energy.Base.Text.Join(glue, true, array);
        }

        /// <summary>
        /// Join multiple arrays using format string for each set of elements from every array.
        /// </summary>
        /// <param name="glue">Separator string</param>
        /// <param name="format">String format for each dictionary set, i.e. "{0}: {1}, {2}"</param>
        /// <param name="array">Arrays (one for each dimension)</param>
        /// <returns></returns>
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

        /// <summary>
        /// Join elements of string dictionary.
        /// </summary>
        /// <param name="glue"></param>
        /// <param name="format">String format for each dictionary key-value pair, i.e. "{0}: {1}"</param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string Join(string glue, string format, Dictionary<string, string> dictionary)
        {
            if (null == glue)
            {
                return null;
            }
            if (string.IsNullOrEmpty(format))
            {
                format = "{0}" + glue + "{1}";
            }
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> e in dictionary)
            {
                string s = string.Format(format, new string[] { e.Key, e.Value });
                list.Add(s);
            }
            return string.Join(glue, list.ToArray());
        }

        /// <summary>
        /// Join elements of dictionary.
        /// </summary>
        /// <param name="glue"></param>
        /// <param name="format">String format for each dictionary key-value pair, i.e. "{0}: {1}"</param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string Join(string glue, string format, Dictionary<string, object> dictionary)
        {
            if (null == glue)
            {
                return null;
            }
            if (string.IsNullOrEmpty(format))
            {
                format = "{0}" + glue + "{1}";
            }
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, object> e in dictionary)
            {
                string s = string.Format(format, new string[] { e.Key, Energy.Base.Cast.AsString(e.Value) });
                list.Add(s);
            }
            return string.Join(glue, list.ToArray());
        }

        #endregion

        #region Implode

        /// <summary>
        /// Concatenate the maximum number of elements at once and return the array of such joins.
        /// If maximum elements is less than 1, null value will be returned.
        /// </summary>
        /// <param name="glue"></param>
        /// <param name="maximum"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] Implode(string glue, int maximum, string[] array)
        {
            if (null == array || 1 > maximum)
            {
                return null;
            }
            else if (maximum == 1 || 0 == array.Length)
            {
                return array;
            }
            if (null == glue)
            {
                glue = "";
            }
            List<string> result = new List<string>();
            string[] a = null;
            int n = 0;
            while (n < array.Length)
            {
                if (n + maximum >= array.Length)
                {
                    int l = array.Length - n;
                    a = new string[l];
                    Array.Copy(array, n, a, 0, l);
                }
                else
                {
                    if (null == a)
                    {
                        a = new string[maximum];
                    }
                    Array.Copy(array, n, a, 0, maximum);
                }
                result.Add(Join(glue, false, a));
                n += maximum;
            }
            return result.ToArray();
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

        public static string[] Split(string input)
        {
            System.Collections.Generic.List<string> list = new List<string>();
            foreach (string line in Each(input))
            {
                list.Add(line);
            }
            return list.ToArray();
        }

        #endregion

        #region SplitLine

        /// <summary>
        /// Split string to array by new line characters.
        /// Elements will not include new line itself.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] SplitLine(string content)
        {
            if (null == content)
            {
                return null;
            }

            if (0 == content.Length)
            {
                return new string[] { };
            }
            
            //return content.Split(NEWLINE_ARRAY, StringSplitOptions.None);
            //return content.Split(NEWLINE_ARRAY);
            return Regex.Split(content, NEWLINE_PATTERN);
        }

        /// <summary>
        /// Split string to array by new line characters.
        /// <br /><br />
        /// Elements will not include new line character itself.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="removeEmpty"></param>
        /// <returns></returns>
        public static string[] SplitLine(string content, bool removeEmpty)
        {
            if (null == content)
            {
                return null;
            }

            if (0 == content.Length)
            {
                return new string[] { };
            }

            //string[] split = content.Split(NEWLINE_ARRAY
            //    , removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None
            //    );
            //return split;

            //string[] array = content.Split(NEWLINE_ARRAY);
            //if (!removeEmpty || 0 == array.Length)
            //{
            //    return array;
            //}
            //else
            //{
            //    var list = new List<string>(array);
            //    for (int i = array.Length - 1; i >= 0; i--)
            //    {
            //        if (string.IsNullOrEmpty(list[i]))
            //        {
            //            list.RemoveAt(i);
            //        }
            //    }
            //    array = list.ToArray();
            //    return array;
            //}

            if (removeEmpty)
            {
                return Regex.Split(content, NEWLINE_GREEDY_PATTERN);
            }
            else
            {
                return Regex.Split(content, NEWLINE_PATTERN);
            }
        }

        #endregion

        #region SplitArray

        /// <summary>
        /// Split string to an array by separators with optional quoted elements.
        /// May be used to explode from strings like "1,2,3", "abc def xyz", 
        /// or "'Smith''s Home'|'Special | New'|Other value".
        /// Quotation marks in values will be stripped.
        /// </summary>
        /// <param name="text">Text to split</param>
        /// <param name="commas">
        /// Separator characters string.
        /// Example value of ",:=" will allow three different characters to be used.
        /// Space character will indicate any of white characters, including new line or tabulation characters.
        /// </param>
        /// <param name="quotes">
        /// Available characters for quoting values.
        /// Example value of "'\"" will allow use of apostrophes together with ASCII quotation marks. 
        /// </param>
        /// <returns></returns>
        public static string[] SplitArray(string text, string commas, string quotes)
        {
            if (null == text)
            {
                return null;
            }
            if (0 == text.Length)
            {
                return new string[] { };
            }
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(quotes))
            {
                foreach (char c in quotes.ToCharArray())
                {
                    string quote = Regex.Escape(c.ToString());
                    string escape = quote + quote;
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
                {
                    continue;
                }
                value = value.Trim();
                if (value.Length == 0)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(quotes))
                {
                    foreach (char c in quotes.ToCharArray())
                    {
                        string quote = c.ToString();
                        if (!value.StartsWith(quote) || !value.EndsWith(quote))
                        {
                            continue;
                        }
                        string escape = new string(c, 2);
                        value = value.Substring(1, value.Length - 2).Replace(escape, quote);
                    }
                }
                list.Add(value);
            }
            return list.ToArray();
        }

        #endregion

        #region SplitDictionary

        ///// <summary>
        ///// Split string by new line
        ///// </summary>
        ///// <returns></returns>
        //// TODO Implement
        //[Energy.Attribute.Code.Draft]
        //[Energy.Attribute.Code.Implement]
        //private static string[] SplitDictionary(string text, string quotes, string equalities, string brackets)
        //{
        //    //return content.Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
        //    return null;
        //}

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

        #region RemoveWhite

        /// <summary>
        /// Remove whitespace characters from entire string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveWhite(string value)
        {
            if (value == null || value.Length == 0)
            {
                return value;
            }
            string white = Energy.Base.Text.WS;
            char[] charArray = value.ToCharArray();
            if (!Energy.Base.Text.ContainsWhite(charArray))
            {
                return value;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (white.IndexOf(charArray[i]) >= 0)
                {
                    continue;
                }
                else
                {
                    sb.Append(charArray[i]);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region ContainsWhite

        /// <summary>
        /// Check if array contains any of whitespace characters.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool ContainsWhite(char[] array)
        {
            if (null == array)
            {
                return false;
            }
            if (0 == array.Length)
            {
                return false;
            }
            string white = Energy.Base.Text.WS;
            for (int i = 0; i < array.Length; i++)
            {
                if (0 <= white.IndexOf(array[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if text contains whitespace character.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ContainsWhite(string text)
        {
            return ContainsWhite(text.ToCharArray());
        }

        #endregion

        #region ReplaceWhite

        /// <summary>
        /// Replace whitespace characters with replacement string in entire string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceWhite(string text, string replacement)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            text = Regex.Replace(text, @"\s+", replacement);
            return text;
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

        #region Escape

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
                    _EscapeExpressionStringDictionary.Add("#", @"\#");
                    _EscapeExpressionStringDictionary.Add(".", @"\.");
                    _EscapeExpressionStringDictionary.Add(" ", @"\ ");
                    _EscapeExpressionStringDictionary.Add("\t", @"\t");
                    _EscapeExpressionStringDictionary.Add("\r", @"\r");
                    _EscapeExpressionStringDictionary.Add("\n", @"\n");
                    _EscapeExpressionStringDictionary.Add("\v", @"\v");
                    _EscapeExpressionStringDictionary.Add("$", @"\$");
                    _EscapeExpressionStringDictionary.Add("^", @"\^");
                    _EscapeExpressionStringDictionary.Add("*", @"\*");
                    _EscapeExpressionStringDictionary.Add("?", @"\?");
                    _EscapeExpressionStringDictionary.Add("+", @"\+");
                    _EscapeExpressionStringDictionary.Add("|", @"\|");
                    _EscapeExpressionStringDictionary.Add("[", @"\[");
                    _EscapeExpressionStringDictionary.Add("]", @"\]");
                    _EscapeExpressionStringDictionary.Add("(", @"\(");
                    _EscapeExpressionStringDictionary.Add(")", @"\)");
                    _EscapeExpressionStringDictionary.Add("{", @"\{");
                    _EscapeExpressionStringDictionary.Add("}", @"\}");
                }
                return _EscapeExpressionStringDictionary;
            }
        }

        /// <summary>
        /// Escape text array for regular expression pattern.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] EscapeExpression(string[] array)
        {
            List<string> list = new List<string>();
            for (int i = 0, length = array.Length; i < length; i++)
            {
                list.Add(EscapeExpression(array[i]));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Escape text for regular expression pattern.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Energy.Attribute.Code.Benchmark("Check versus building string from characters one by one replacing specials with equivalents.")]
        public static string EscapeExpression(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            System.Text.StringBuilder s = null;
            foreach (KeyValuePair<string, string> _ in EscapeExpressionStringDictionary)
            {
                if (input.Contains(_.Key))
                {
                    if (s == null)
                    {
                        s = new System.Text.StringBuilder(input);
                    }
                    s.Replace(_.Key, _.Value);
                }
            }
            return s == null ? input : s.ToString();
        }

        /// <summary>
        /// Escape character for regular expression.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string EscapeExpression(char character)
        {
            return EscapeExpression(character.ToString());
        }

        #endregion

        #endregion

        #region Random

        /// <summary>
        /// Generate random text.
        /// Resulting string will contain upper and lower latin letters and numbers only.
        /// You may expect length from 3 to 10 characters.
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

        #region Capitalize

        /// <summary>
        /// Return a word with its first letter upper case and remaining letters in lower case.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Capitalize(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }
            else
            {
                return word.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + word.Substring(1).ToLower(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Return array of words with their first letters upper case and remaining letters in lower case.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string[] Capitalize(string[] words)
        {
            if (null == words || 0 == words.Length)
            {
                return words;
            }
            string[] result = new string[words.Length];
            for (int i = 0, l = words.Length; i < l; i++)
            {
                result[i] = Capitalize(words[i]);
            }
            return result;
        }

        #endregion

        #region Upper

        /// <summary>
        /// Change letters in a word to upper case.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Upper(string word)
        {
            return word == null ? null : word.ToUpper(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Change letters in word list to upper case.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string[] Upper(string[] words)
        {
            if (null == words || 0 == words.Length)
            {
                return words;
            }
            int n = words.Length;
            string[] result = new string[n];
            for (int i = 0; i < n; i++)
            {
                if (null == words[i])
                {
                    continue;
                }
                words[i] = words[i].ToUpper(CultureInfo.InvariantCulture);
            }
            return words;
        }

        #endregion

        #region Lower

        /// <summary>
        /// Change letters in a word to lower case.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Lower(string word)
        {
            return word == null ? null : word.ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Change letters in word list to lower case.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string[] Lower(string[] words)
        {
            if (null == words || 0 == words.Length)
            {
                return words;
            }
            int n = words.Length;
            string[] result = new string[n];
            for (int i = 0; i < n; i++)
            {
                if (null == words[i])
                {
                    continue;
                }
                words[i] = words[i].ToLower(CultureInfo.InvariantCulture);
            }
            return words;
        }

        #endregion

        #region Naming convention

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        [Obsolete("Use Energy.Base.Naming.DashCase instead")]
        public static string DashCase(string[] words)
        {
            return Energy.Base.Naming.DashCase(words);
        }

        /// <summary>
        /// Return words as a string separated with hyphen minus (dash) character
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        [Obsolete("Use Energy.Base.Naming.HyphenCase instead")]
        public static string HyphenCase(string[] words)
        {
            return Energy.Base.Naming.HyphenCase(words);
        }

        /// <summary>
        /// Return words lower case, separated with underscore character.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        [Obsolete("Use Energy.Base.Naming.SnakeCase instead")]
        public static string SnakeCase(string[] words)
        {
            return Energy.Base.Naming.SnakeCase(words);
        }

        /// <summary>
        /// Return words as a string separated with underscore character
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        [Obsolete("Use Energy.Base.Naming.UnderscoreCase instead")]
        public static string UnderscoreCase(string[] words)
        {
            return Energy.Base.Naming.UnderscoreCase(words);
        }

        /// <summary>
        /// Return words using medial capitalization
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        [Obsolete("Use Energy.Base.Naming.PascalCase instead")]
        public static string PascalCase(string[] words)
        {
            return Energy.Base.Naming.PascalCase(words);
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
        [Obsolete("Use Energy.Base.Naming.CamelCase instead")]
        public static string CamelCase(string[] words)
        {
            return Energy.Base.Naming.CamelCase(words);
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
            text = text.ToLower(CultureInfo.InvariantCulture);
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
                {
                    return System.Text.Encoding.GetEncoding(number);
                }
                else
                {
                    return System.Text.Encoding.GetEncoding(encoding);
                }
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
        /// Convert new line delimiter to specified one.
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
        /// Convert newline delimiter to environment default.
        /// Value of constant **Energy.Base.Text.NL** is used.
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

        public static int Compare(string one, string two)
        {
            return string.Compare(one, two);
        }

        public static int Compare(string one, string two, bool ignoreCase)
        {
            return string.Compare(one, two, ignoreCase);
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
            {
                return default(T);
            }
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

        #endregion

        #region GetElementOrEmpty

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

        #region GetControlStringPattern

        public static string GetControlStringPattern(Class.ControlStringOptions options)
        {
            if (_ControlStringExpressionCache != null)
            {
                if (_ControlStringExpressionCache.ContainsKey(options))
                {
                    return _ControlStringExpressionCache[options];
                }
            }

            List<string> alternatives = new List<string>();

            char quote = options.Quote;
            bool wide = options.Wide;
            bool white = options.White;
            char escape = options.Escape;

            if (quote != '\0')
            {
                string doubleQuote = escape == '\0'
                    ? string.Concat(quote, quote)
                    : string.Concat(escape, quote)
                    ;
                string _quote = EscapeExpression(quote);
                string _doubleQuote = EscapeExpression(doubleQuote);
                string pattern = _quote + "(?:" + _doubleQuote + "|[^" + _quote + "])*" + _quote;
                pattern = "(?<q>" + pattern + ")";
                alternatives.Add(pattern);
            }

            List<KeyValuePair<int, string>> codes = new List<KeyValuePair<int, string>>();

            if (options.DecimalPrefix != null)
            {
                foreach (string decPrefix in options.DecimalPrefix)
                {
                    if (string.IsNullOrEmpty(decPrefix))
                        continue;
                    int max = wide ? 5 : 3;
                    string pattern = "";
                    if (!white)
                    {
                        pattern += EscapeExpression(decPrefix);
                    }
                    else
                    {
                        pattern += Energy.Base.Expression.EscapeSurround(null, @"\s*", decPrefix.ToCharArray());
                    }
                    pattern += "(?<d>[0-9]{1," + max + "})";
                    codes.Add(new KeyValuePair<int, string>(decPrefix.Length, pattern));
                }
            }

            if (options.HexadecimalPrefix != null)
            {
                foreach (string hexPrefix in options.HexadecimalPrefix)
                {
                    if (string.IsNullOrEmpty(hexPrefix))
                        continue;
                    int max = wide ? 5 : 3;
                    string pattern = "";
                    if (!white)
                    {
                        pattern += EscapeExpression(hexPrefix);
                    }
                    else
                    {
                        pattern += Energy.Base.Expression.EscapeSurround(null, @"\s*", hexPrefix.ToCharArray());
                    }
                    pattern += "(?<h>[0-9A-Fa-f]{1," + max + "})";
                    codes.Add(new KeyValuePair<int, string>(hexPrefix.Length, pattern));
                }
            }

            if (options.OctalPrefix != null)
            {
                foreach (string octPrefix in options.OctalPrefix)
                {
                    if (string.IsNullOrEmpty(octPrefix))
                        continue;
                    int max = wide ? 6 : 3;
                    string pattern = "";
                    if (!white)
                    {
                        pattern += EscapeExpression(octPrefix);
                    }
                    else
                    {
                        pattern += Energy.Base.Expression.EscapeSurround(null, @"\s*", octPrefix.ToCharArray());
                    }
                    pattern += "(?<o>[0-9]{1," + max + "})";
                    codes.Add(new KeyValuePair<int, string>(octPrefix.Length, pattern));
                }
            }

            if (options.BinaryPrefix != null)
            {
                foreach (string binPrefix in options.BinaryPrefix)
                {
                    if (string.IsNullOrEmpty(binPrefix))
                        continue;
                    int max = wide ? 16 : 8;
                    string pattern = "";
                    if (!white)
                    {
                        pattern += EscapeExpression(binPrefix);
                    }
                    else
                    {
                        pattern += Energy.Base.Expression.EscapeSurround(null, @"\s*", binPrefix.ToCharArray());
                    }
                    pattern += "(?<b>[0-1]{1," + max + "})";
                    codes.Add(new KeyValuePair<int, string>(binPrefix.Length, pattern));
                }
            }

            codes.Sort(delegate (KeyValuePair<int, string> x1, KeyValuePair<int, string> x2)
            {
                return x1.Key.CompareTo(x2.Key);
            });

            for (int i = codes.Count; --i >= 0;)
            {
                alternatives.Add(codes[i].Value);
            }

            alternatives.Add(@"(?<w>\s+?)");

            alternatives.Add(@"(?<a>.+?)");

            string expression = string.Join("|", alternatives.ToArray());

            if (_ControlStringExpressionCache == null)
            {
                _ControlStringExpressionCache = new Dictionary<Class.ControlStringOptions, string>();
                _ControlStringExpressionCache[options] = expression;
            }

            return expression;
        }

        #endregion

        #region DecodeControlString

        /// <summary>
        /// Decode control string, like "'Hello'#13#10".
        /// </summary>
        /// <param name="text"></param>
        /// <param name="quote"></param>
        /// <param name="escape"></param>
        /// <param name="decPrefix"></param>
        /// <param name="hexPrefix"></param>
        /// <param name="octPrefix"></param>
        /// <param name="binPrefix"></param>
        /// <param name="encoding"></param>
        /// <param name="white"></param>
        /// <param name="unquote"></param>
        /// <param name="wide"></param>
        /// <returns></returns>
        public static string DecodeControlString(string text, char quote, char escape
           , string decPrefix, string hexPrefix, string octPrefix, string binPrefix
           , System.Text.Encoding encoding, bool white, bool unquote, bool wide
           )
        {
            return DecodeControlString(text, encoding, new Class.ControlStringOptions()
            {
                Quote = quote,
                Escape = escape,
                DecimalPrefix = new string[] { decPrefix },
                HexadecimalPrefix = new string[] { hexPrefix },
                OctalPrefix = new string[] { octPrefix },
                BinaryPrefix = new string[] { binPrefix },
                White = white,
                Wide = wide,
            });
        }

        /// <summary>
        /// Decode control string, like "'Hello'#13#10".
        /// Use default System.Text.Encoding.UTF8 as encoding.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string DecodeControlString(string text, Class.ControlStringOptions options)
        {
            return DecodeControlString(text, System.Text.Encoding.UTF8, options);
        }

        /// <summary>
        ///  Decode control string, like "'Hello'#13#10".
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string DecodeControlString(string text, System.Text.Encoding encoding
            , Class.ControlStringOptions options
            )
        {
            string pattern = GetControlStringPattern(options);
            Regex regex = new Regex(pattern, RegexOptions.None);
            Match match = regex.Match(text);
            StringBuilder sb = new StringBuilder();
            while (match.Success)
            {
                if (false)
                { }
                else if (match.Groups["q"].Success)
                {
                    string value = Energy.Base.Text.Strip(match.Groups["q"].Value, options.Quote, options.Escape);
                    sb.Append(value);
                }
                else if (match.Groups["d"].Success)
                {
                    int number = Energy.Base.Cast.AsInteger(match.Groups["d"].Value);
                    sb.Append((char)number);
                }
                else if (match.Groups["h"].Success)
                {
                    int number = Energy.Base.Cast.HexToInteger(match.Groups["h"].Value);
                    sb.Append((char)number);
                }
                else if (match.Groups["w"].Success)
                {
                    if (options.IncludeWhite)
                    {
                        sb.Append(match.Groups["w"].Value);
                    }
                }
                else if (match.Groups["a"].Success)
                {
                    if (options.IncludeUnknown)
                    {
                        sb.Append(match.Groups["a"].Value);
                    }
                }
                else if (match.Groups["o"].Success)
                {
                    int number = Energy.Base.Cast.OctToInteger(match.Groups["o"].Value);
                    sb.Append((char)number);
                }

                match = match.NextMatch();
            }

            return sb.ToString();
        }

        #endregion

        #region Parse

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
        /// Surround text with quotation characters (").
        /// Escape existing quotation characters with additional quotation character ("").
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Quote(string text)
        {
            return Quote(text, "\"", "\"");
        }

        /// <summary>
        /// Surround text with quotation characters (") optionally.
        /// If optional parameter is true, text will be quoted only when text contains specified quotation character.
        /// Escape existing quotation characters with additional quotation character ("").
        /// </summary>
        /// <param name="text"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static string Quote(string text, bool optional)
        {
            if (optional && !text.Contains("\""))
            {
                return text;
            }
            return Quote(text, "\"", "\"");
        }

        /// <summary>
        /// Surround text with specified quotation characters.
        /// Escape existing quotation characters with additional quotation character.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        public static string Quote(string text, string with)
        {
            return Quote(text, with, with);
        }

        /// <summary>
        /// Surround text with specified quotation characters optionally.
        /// If optional parameter is true, text will be quoted only when text contains specified quotation character.
        /// Escape existing quotation characters with additional quotation character.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static string Quote(string text, string with, bool optional)
        {
            if (optional && !text.Contains(with))
            {
                return text;
            }
            return Quote(text, with, with);
        }

        /// <summary>
        /// Surround text with specified quotation characters.
        /// Escape existing quotation character with specified escape character.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Quote(string text, string with, string escape)
        {
            if (text == null || text.Length == 0)
            {
                return string.Concat(with, with);
            }

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
        /// Surround text with specified quotation characters optionally.
        /// If optional parameter is true, text will be quoted only when text contains specified quotation character.
        /// Escape existing quotation character with specified escape character.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="with"></param>
        /// <param name="escape"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static string Quote(string text, string with, string escape, bool optional)
        {
            if (optional && !text.Contains(with))
            {
                return text;
            }
            return Quote(text, with, escape);
        }

        /*
         * Removed to avoid using any value and possible problem in future when
         * new methods with different parameter types will appear.
         * 

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

         */

        #endregion

        #region Strip

        /// <summary>
        /// Strip text from double quotation marks.
        /// <br/><br/>
        /// Two sequential quotation characters inside the text will be replaced by single characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strip(string text)
        {
            return Strip(text, "\"", "\"");
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Two sequential quotation characters inside the text will be replaced by single characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        public static string Strip(string text, char quote)
        {
            string s = quote.ToString();
            return Strip(text, s, s);
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Two sequential quotation characters inside the text will be replaced by single characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        public static string Strip(string text, string quote)
        {
            return Strip(text, quote, quote);
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Escape character for including quotation characters inside the text may be provided.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="quote"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Strip(string text, char quote, char escape)
        {
            string q = quote.ToString();
            string e = escape.ToString();
            return Strip(text, q, e);
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Escape character for including quotation characters inside the text may be provided.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="quote"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Strip(string text, string quote, string escape)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            int a = 0;
            int b = text.Length;
            if (text.StartsWith(quote))
            {
                a = quote.Length;
                b -= quote.Length;
            }
            if (text.EndsWith(quote))
            {
                b -= quote.Length;
            }
            string cut = text.Substring(a, b);
            if (!string.IsNullOrEmpty(escape))
            {
                cut = cut.Replace(escape + quote, quote);
            }
            return cut;
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Escape character for including quotation characters inside the text may be provided.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="escape"></param>
        /// <param name="change">Will be set to true if text was stripped</param>
        /// <returns></returns>
        public static string Strip(string text, string start, string end, string escape, out bool change)
        {
            change = false;
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            int a = 0;
            int b = text.Length;
            if (!string.IsNullOrEmpty(start) && text.StartsWith(start))
            {
                change = true;
                a += start.Length;
                b -= a;
            }
            if (!string.IsNullOrEmpty(end) && text.EndsWith(end))
            {
                change = true;
                b -= end.Length;
            }
            text = text.Substring(a, b);
            if (!string.IsNullOrEmpty(escape))
            {
                string e = escape + end;
                if (text.Contains(e))
                {
                    change = true;
                    text = text.Replace(e, end);
                }
            }
            return text;
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Escape character for including quotation characters inside the text may be provided.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Strip(string text, string start, string end, string escape)
        {
            bool _;
            return Strip(text, start, end, escape, out _);
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Escape character for including quotation characters inside the text may be provided.
        /// <br/><br/>
        /// Multiple variations may be set to allow different quotation styles to work.
        /// <br/><br/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="escape"></param>
        /// <param name="change">Will be set to true if text was stripped</param>
        /// <returns></returns>
        public static string Strip(string text, string[] start, string[] end, string[] escape, out bool change)
        {
            change = false;
            if (null == start || null == end || null == escape)
            {
                return text;
            }
            int n = start.Length;
            if (n != end.Length || n != escape.Length)
            {
                return text;
            }
            for (int i = 0; i < n; i++)
            {
                text = Strip(text, start[i], end[i], escape[i], out change);
                if (change)
                {
                    break;
                }
            }
            return text;
        }

        /// <summary>
        /// Strip text from quotation.
        /// <br/><br/>
        /// Escape character for including quotation characters inside the text may be provided.
        /// <br/><br/>
        /// Multiple variations may be set to allow different quotation styles to work.
        /// <br/><br/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string Strip(string text, string[] start, string[] end, string[] escape)
        {
            bool _;
            return Strip(text, start, end, escape, out _);
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
        /// Search an array for any of elements to look for.
        /// If array or look is null or empty, function will result false.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="look"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool InArray(string[] array, string[] look, bool ignoreCase)
        {
            if (null == array || array.Length == 0)
                return false;
            if (null == look || look.Length == 0)
                return false;
            for (int i = 0; i < look.Length; i++)
            {
                if (InArray(array, look[i], ignoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Search an array for any of elements to look for.
        /// If array or look is null or empty, function will result false.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="look"></param>
        /// <returns></returns>
        public static bool InArray(string[] array, string[] look)
        {
            return InArray(array, look, false);
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

        [Energy.Attribute.Code.Obsolete("Energy.Base.Text.Editor moved to Energy.Core.Text namespace")]
        [Obsolete("Energy.Base.Text.Editor moved to Energy.Core.Text namespace", false)]
        public class Editor : Energy.Core.Text.Editor { }

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

        #region Cell

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <param name="prefix">
        /// Optional prefix text that can be added if there is a space in resulting text to match size.
        /// </param>
        /// <param name="suffix">
        /// Optional suffix text that can be added if there is a space in resulting text to match size.
        /// </param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextPad pad, char fill, string prefix, string suffix, out string remains)
        {
            remains = "";

            if (size == 0)
                return "";

            if (text == null)
                text = "";
            if (prefix == null)
                prefix = "";
            if (suffix == null)
                suffix = "";

            if (start < 0)
            {
                start = text.Length + start;
                if (start < 0)
                {
                    text = "";
                    start = 0;
                }
            }

            if (size < 0)
            {
                size = -size;
                if (text.Length > size)
                {
                    text = text.Substring(text.Length - size);
                    return text;
                }
            }

            if (start > 0)
            {
                if (start >= text.Length)
                    text = "";
                else if (text.Length - start <= size)
                    text = text.Substring(start);
                else
                {
                    remains = text.Substring(start + size);
                    text = text.Substring(start, size);
                    return text;
                }
            }

            if (text.Length == size)
                return text;

            bool leftJustify = 0 < (pad & Energy.Enumeration.TextPad.Left);

            if (size > 0)
            {
                if (text.Length > size)
                {
                    remains = text.Substring(size);
                    text = text.Substring(0, size);
                    return text;
                }
            }

            int width = size;

            if (leftJustify)
            {
                if (prefix.Length > 0 && width - text.Length >= prefix.Length)
                    width -= prefix.Length;
                if (suffix.Length > 0 && width - text.Length >= suffix.Length)
                    width -= suffix.Length;
            }
            else
            {
                if (suffix.Length > 0 && width - text.Length >= suffix.Length)
                    width -= suffix.Length;
                if (prefix.Length > 0 && width - text.Length >= prefix.Length)
                    width -= prefix.Length;
            }

            string result = Energy.Base.Text.Pad(text, width, fill, pad, true);

            if (leftJustify)
            {
                if (prefix.Length > 0 && result.Length + prefix.Length <= size)
                    result = prefix + result;
                if (suffix.Length > 0 && result.Length + suffix.Length <= size)
                    result = result + suffix;
            }
            else
            {
                if (suffix.Length > 0 && result.Length + suffix.Length <= size)
                    result = result + suffix;
                if (prefix.Length > 0 && result.Length + prefix.Length <= size)
                    result = prefix + result;
            }

            return result;
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextPad pad, out string remains)
        {
            remains = "";
            return Cell(text, start, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextPad pad, char fill, out string remains)
        {
            remains = "";
            return Cell(text, start, size, pad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextPad pad)
        {
            string remains = "";
            return Cell(text, start, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextPad pad, char fill)
        {
            string remains = "";
            return Cell(text, start, size, pad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextPad pad, out string remains)
        {
            remains = "";
            return Cell(text, 0, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextPad pad, char fill, out string remains)
        {
            remains = "";
            return Cell(text, 0, size, pad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextPad pad, char fill)
        {
            string remains = "";
            return Cell(text, 0, size, pad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="align">Text alignment</param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextAlign align, out string remains)
        {
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, start, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="align">Text alignment (&lt; for left, &gt; for right, - for center and = for justification)</param>
        /// <param name="remains">Output remaining string</param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, char align, char fill, out string remains)
        {
            Energy.Enumeration.TextAlign textAlign = Energy.Base.Cast.Enumeration.CharToTextAlign(align);
            Energy.Enumeration.TextPad textPad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(textAlign);
            return Cell(text, start, size, textPad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="align">Text alignment (&lt; for left, &gt; for right, - for center and = for justification)</param>
        /// <param name="remains">Output remaining string</param>
        /// <returns></returns>
        public static string Cell(string text, int size, char align, char fill, out string remains)
        {
            Energy.Enumeration.TextAlign textAlign = Energy.Base.Cast.Enumeration.CharToTextAlign(align);
            Energy.Enumeration.TextPad textPad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(textAlign);
            return Cell(text, 0, size, textPad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align text to the specified size. 
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="align">Text alignment (&lt; for left, &gt; for right, - for center and = for justification)</param>
        /// <returns></returns>
        public static string Cell(string text, int size, char align, char fill)
        {
            Energy.Enumeration.TextAlign textAlign = Energy.Base.Cast.Enumeration.CharToTextAlign(align);
            Energy.Enumeration.TextPad textPad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(textAlign);
            string useless;
            return Cell(text, 0, size, textPad, fill, null, null, out useless);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="align">Text alignment</param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextAlign align)
        {
            string remains = "";
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, start, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="start">
        /// The initial index of the text to be cut out. 
        /// If less than zero, it indicates the last characters of the text.
        /// </param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="align">Text alignment</param>
        /// <returns></returns>
        public static string Cell(string text, int start, int size, Energy.Enumeration.TextAlign align, char fill)
        {
            string remains = "";
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, start, size, pad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="align">Text alignment</param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextAlign align, out string remains)
        {
            remains = "";
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, 0, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="align">Text alignment</param>
        /// <param name="remains"></param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextAlign align, char fill, out string remains)
        {
            remains = "";
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, 0, size, pad, fill, null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="align">Text alignment</param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextAlign align)
        {
            string remains = "";
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, 0, size, pad, ' ', null, null, out remains);
        }

        /// <summary>
        /// Align and limit the text to the specified size. 
        /// Cut the initial characters from the text value. 
        /// If there are enough space, add a prefix and a suffix in order from the alignment direction of the text.
        /// </summary>
        /// <param name="text">Text value to be aligned in a cell</param>
        /// <param name="size"></param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="align">Text alignment</param>
        /// <returns></returns>
        public static string Cell(string text, int size, Energy.Enumeration.TextAlign align, char fill)
        {
            string remains = "";
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Cell(text, 0, size, pad, fill, null, null, out remains);
        }

        #endregion

        #region Pad

        /// <summary>
        /// Expand the text on the left or right by filling in the specified character.
        /// Optionally, cut the text to the desired size.
        /// </summary>
        /// <remarks>
        ///
        /// GOOD
        ///
        ///   if (0 &lt; (pad &amp; (Energy.Enumeration.TextPad.Left)) &amp;&amp; (0 &lt; (pad &amp; Energy.Enumeration.TextPad.Right)))
        ///
        /// WORKS
        ///
        ///   if (0 &lt; (pad &amp; (Energy.Enumeration.TextPad.Left)) &amp; (0 &lt; (pad &amp; Energy.Enumeration.TextPad.Right)))
        ///
        /// WRONG
        ///
        ///   if (0 &lt; (pad &amp; (Energy.Enumeration.TextPad.Left | Energy.Enumeration.TextPad.Right)))
        ///
        /// </remarks>
        /// <param name="text"></param>
        /// <param name="size">
        /// The number of characters in the resulting string, equal to the number of original
        /// characters plus any additional padding characters.
        /// </param>
        /// <param name="fill">
        /// Character that will be used if text is shorter than specified size.
        /// </param>
        /// <param name="pad">
        /// Padding direction, may be left or right.
        /// Because padding is defined as flags, center or middle is also avaiable.
        /// </param>
        /// <param name="cut">If true, text will be limited to specified size</param>
        /// <returns></returns>
        public static string Pad(string text, int size, char fill, Energy.Enumeration.TextPad pad, bool cut)
        {
            if (text == null)
                text = "";

            if (text == "")
            {
                if (size == 0)
                    return "";
            }

            bool beLeft = 0 < (pad & Energy.Enumeration.TextPad.Left);
            bool beRight = 0 < (pad & Energy.Enumeration.TextPad.Right);

            if (text.Length < size)
            {
                if (beLeft && beRight)
                {
                    int d = size - text.Length;
                    int d2 = d / 2;
                    int d21 = d - d2; // may be higher or equal d2
                    text = text.PadLeft(text.Length + d21, fill);
                    if (text.Length < size)
                    {
                        text = text.PadRight(text.Length + d2, fill);
                    }
                }
                else if (beLeft)
                {
                    text = text.PadLeft(size, fill);
                }
                else if (beRight)
                {
                    text = text.PadRight(size, fill);
                }
            }

            if (cut && text.Length > size)
            {
                if (beLeft && beRight)
                {
                    int d = text.Length - size;
                    int d2 = d / 2;
                    text = text.Substring(d2, size);
                }
                else if (beRight)
                {
                    text = text.Substring(text.Length - size, size);
                }
                else
                {
                    text = text.Substring(0, size);
                }
            }

            return text;
        }

        /// <summary>
        /// Expand the text on the left or right by filling in the specified character.
        /// If text is longer than size, it will remain untouched.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size">
        /// The number of characters in the resulting string, equal to the number of original
        /// characters plus any additional padding characters
        /// </param>
        /// <param name="fill"></param>
        /// <param name="pad">Padding direction</param>
        /// <returns></returns>
        public static string Pad(string text, int size, char fill, Energy.Enumeration.TextPad pad)
        {
            return Pad(text, size, fill, pad, false);
        }

        /// <summary>
        /// Expand the text on the left or right by filling in the specified character.
        /// If text is longer than size, it will remain untouched.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size">
        /// The number of characters in the resulting string, equal to the number of original
        /// characters plus any additional padding characters
        /// </param>
        /// <param name="fill"></param>
        /// <param name="align">Text alignment</param>
        /// <returns></returns>
        public static string Pad(string text, int size, char fill, Energy.Enumeration.TextAlign align)
        {
            Energy.Enumeration.TextPad pad = Energy.Base.Cast.Enumeration.TextAlignToTextPad(align);
            return Pad(text, size, fill, pad, false);
        }

        #endregion

        #region Interpolate

        /// <summary>
        /// Interpolate text content by changing specified texts known also as placeholders
        /// with specified values.
        /// This version uses regular expression match, so might be slower.
        /// It is not recursive so with ":a:" = ":b:" and ":b:" = ":a:" interpolating
        /// text containing ":a:" will result in ":b:". 
        /// Be careful about using it in recursion.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="array">Key value pairs dictionary for interpolation</param>
        /// <returns></returns>
        public static string Interpolate(string text, bool ignoreCase, params string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return text;
            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < array.Length / 2; i++)
            {
                string key = array[i * 2] ?? "";
                if (ignoreCase)
                    key = key.ToUpper(CultureInfo.InvariantCulture);
                dictionary[key] = array[1 + i * 2] ?? "";
            }
            if (dictionary.Count == 0)
            {
                return text;
            }
            List<string> suspect = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                suspect.Add(Energy.Base.Text.EscapeExpression(key));
            }
            suspect.Sort((string s1, string s2) => { return (s2 ?? "").Length - (s1 ?? "").Length; });
            string pattern = string.Join("|", suspect.ToArray());
            int Δ = 0;
            string result = text;
            RegexOptions option = RegexOptions.None;
            if (ignoreCase)
                option |= RegexOptions.IgnoreCase;
            Match match = Regex.Match(text, pattern, option);
            while (match.Success)
            {
                int position = match.Index;
                int length = match.Length;
                position += Δ;
                string value = match.Value;
                if (ignoreCase)
                    value = value.ToUpper(CultureInfo.InvariantCulture);
                string replacement = dictionary[value];
                result = string.Concat(result.Substring(0, position), replacement, result.Substring(position + length));
                Δ += replacement.Length - length;
                match = match.NextMatch();
            }
            return result;
        }

        #endregion

        #region HasDigitsOnly

        /// <summary>
        /// Checks if string contains only digits.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasDigitsOnly(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        #endregion

        #region IsInteger

        /// <summary>
        /// Checks if string is an integer number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="negative"></param>
        /// <returns></returns>
        public static bool IsInteger(string value, bool negative)
        {
            if (negative)
            {
                int useless;
                return int.TryParse(value, out useless);
            }
            else
            {
                uint useless;
                return uint.TryParse(value, out useless);
            }
        }

        ///// <summary>
        ///// Checks if string is an integer number.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static bool IsInteger(string value)
        //{
        //    return IsInteger(value, true);
        //}

        /// <summary>
        /// Check if value represents integer number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInteger(string input) 
        {
            return Regex.Match(input ?? "", @"^[+\-]?[0-9]+$").Success;
        }

        #endregion

        #region IsNumber

        /// <summary>
        /// Check if value represents number.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            return Regex.Match(input ?? "", @"^[+\-]?[0-9]+([.,][0-9]*)?([eE][+\-]?[0-9]+)?$").Success;
        }

        #endregion

        #region IsLong

        /// <summary>
        /// Checks if string is a long integer number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="negative"></param>
        /// <returns></returns>
        public static bool IsLong(string value, bool negative)
        {
            if (negative)
            {
                long useless;
                return long.TryParse(value, out useless);
            }
            else
            {
                long useless;
                return long.TryParse(value, out useless);
            }
        }

        /// <summary>
        /// Checks if string is a long integer number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLong(string value)
        {
            return IsLong(value, true);
        }

        #endregion

        #region First

        /// <summary>
        /// Return first character of a string or empty string if doesn't contain any characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string First(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return text[0].ToString();
        }

        /// <summary>
        /// Return first maximum characters of a string or empty string if doesn't contain any characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maximum">maximum</param>
        /// <returns></returns>
        public static string First(string text, int maximum)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else if (text.Length > maximum)
                return text.Substring(0, maximum);
            else
                return text;
        }

        /// <summary>
        /// Return first character of a string or null if doesn't contain any characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FirstOrNull(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            else
                return text[0].ToString();
        }

        /// <summary>
        /// Return first maximum characters of a string or null if doesn't contain any characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maximum">maximum</param>
        /// <returns></returns>
        public static string FirstOrNull(string text, int maximum)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            else if (text.Length > maximum)
                return text.Substring(0, maximum);
            else
                return text;
        }

        #endregion

        #region EmptyIfNull

        /// <summary>
        /// Return empty string when string parameter is null or string parameter itself otherwise.
        /// This function ensures string objects are always defined.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EmptyIfNull(string value)
        {
            return value != null ? value : "";
        }

        #endregion

        #region Cut

        /// <summary>
        /// Return part of text which ends with one of ending sequences, supporting optional quotations.
        /// <br/><br/>
        /// If text contains part in quotes, it will be included as is, together with quotation characters until ending sequence is found.
        /// <br/><br/>
        /// When cutting text "a$b$Hello '$'$d" by dollar sign ($) as ending and apostrophes (') as quotations
        /// text will be cutted in following pieces: "a", "b", "Hello '$'", "d".
        /// </summary>
        /// <param name="text"></param>
        /// <param name="terminator">
        /// Array of possible ending sequences.
        /// </param>
        /// <param name="quotation">
        /// Array of possible quotations, written in the form used by Energy.Base.Text.Quotation.From (see documentation for more information).
        /// <br/><br/>
        /// Examples: "'", "''", @"'\'", @"' '' \' '", "[]", "%". 
        /// </param>
        /// <returns></returns>
        public static string Cut(string text, string[] terminator, string[] quotation)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (null == terminator || 0 == terminator.Length)
            {
                return text;
            }
            List<Quotation> list = new List<Quotation>();
            if (null != quotation)
            {
                for (int i = 0; i < quotation.Length; i++)
                {
                    Quotation e = Quotation.From(quotation[i]);
                    if (null != e)
                    {
                        list.Add(e);
                    }
                }
            }
            int q = -1;
            int l = text.Length;
            for (int i = 0; i < l; i++)
            {
                // check for quotation
                if (0 > q)
                {
                    if (0 < list.Count)
                    {
                        for (int n = 0; n < list.Count; n++)
                        {
                            string e = list[n].Prefix;
                            if (e.Length >= l - i)
                            {
                                continue;
                            }
                            if (0 == string.Compare(e, text.Substring(i, e.Length)))
                            {
                                q = n; // in quotes => set q to quotation
                                i += -1 + e.Length;
                                break;
                            }
                        }
                    }
                    if (0 <= q)
                    {
                        continue;
                    }
                }

                // check for escape in quotation
                if (0 <= q)
                {
                    bool b = false;
                    string[] x = list[q].Escape;
                    if (null != x || 0 < x.Length)
                    {
                        for (int n = 0; n < x.Length; n++)
                        {
                            string e = list[q].Escape[n];
                            if (string.IsNullOrEmpty(e))
                            {
                                continue;
                            }
                            if (e.Length < l - i && 0 == string.Compare(e, text.Substring(i, e.Length)))
                            {
                                i += -1 + e.Length;
                                b = true;
                                break;
                            }
                        }
                    }
                    if (b)
                    {
                        continue;
                    }
                }

                // check for end of quotation
                if (0 <= q)
                {
                    string e = list[q].Suffix;
                    if (!string.IsNullOrEmpty(e))
                    {
                        if (e.Length < l - i && 0 == string.Compare(e, text.Substring(i, e.Length)))
                        {
                            q = -1;
                            i += -1 + e.Length;
                            //b = true;
                            //break;
                        }
                    }
                    continue;
                }

                // check for ending mark
                for (int m = 0; m < terminator.Length; m++)
                {
                    string e = terminator[m];
                    if (e.Length < l - i && 0 == string.Compare(e, text.Substring(i, e.Length)))
                    {
                        string r = text.Substring(0, i + e.Length);
                        return r;
                    }
                }
            }
            return text;
        }

        /// <summary>
        /// Cut part of text which ends with one of ending sequences, supporting optional quotations.
        /// <br/><br/>
        /// If text contains part in quotes, it will be included as is, together with quotation characters until ending sequence is found.
        /// <br/><br/>
        /// When cutting text "a$b$Hello '$'$d" by dollar sign ($) as ending and apostrophes (') as quotations
        /// text will be cutted in following pieces: "a", "b", "Hello '$'", "d".
        /// <br/><br/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="terminator">
        /// Array of possible ending sequences.
        /// </param>
        /// <param name="quotation">
        /// Array of possible quotations, written in the form used by Energy.Base.Text.Quotation.From (see documentation for more information).
        /// <br/><br/>
        /// Examples: "'", "''", @"'\'", @"' '' \' '", "[]", "%". 
        /// </param>
        /// <returns></returns>
        public static string Cut(ref string text, string[] terminator, string[] quotation)
        {
            string cut = Cut(text, terminator, quotation);
            text = text.Substring(cut.Length);
            return cut;
        }

        #endregion

        #region AddSlashes

        /// <summary>
        /// Quote string with slashes in a C style.
        /// Returns a string with backslashes before special characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string AddSlashes(string text)
        {
            return AddSlashes(text, GetSlashesDictionary());
        }

        /// <summary>
        /// Quote string with slashes in a C style.
        /// Returns a string with backslashes before special characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string AddSlashes(string text, Dictionary<char, string> dictionary)
        {
            if (string.IsNullOrEmpty(text) || dictionary == null || dictionary.Count == 0)
            {
                return text;
            }
            int length = text.Length;
            StringBuilder s = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                if (dictionary.ContainsKey(text[i]))
                {
                    s.Append(dictionary[text[i]]);
                    continue;
                }
                s.Append(text[i]);
            }
            return s.ToString();
        }

        private static Dictionary<char, string> _SlashesDictionary;

        private static Dictionary<char, string> GetSlashesDictionary()
        {
            if (null == _SlashesDictionary)
            {
                Dictionary<char, string> d = new Dictionary<char, string>();
                d['\0'] = @"\0";
                d['\x07'] = @"\a";
                d['\x08'] = @"\b";
                d['\x1b'] = @"\e";
                d['\x0c'] = @"\f";
                d['\x0a'] = @"\n";
                d['\x0d'] = @"\r";
                d['\x09'] = @"\t";
                d['\x0b'] = @"\v";
                d['\\'] = @"\";
                d['\''] = @"\'";
                d['"'] = @"\""";
                d['?'] = @"\?";
                d['\x01'] = @"\x01";
                d['\x02'] = @"\x02";
                d['\x03'] = @"\x03";
                d['\x04'] = @"\x04";
                d['\x05'] = @"\x05";
                d['\x0e'] = @"\x0e";
                d['\x0f'] = @"\x0f";
                d['\x10'] = @"\x10";
                d['\x11'] = @"\x11";
                d['\x12'] = @"\x12";
                d['\x13'] = @"\x13";
                d['\x14'] = @"\x14";
                d['\x15'] = @"\x15";
                d['\x16'] = @"\x16";
                d['\x17'] = @"\x17";
                d['\x18'] = @"\x18";
                d['\x19'] = @"\x19";
                d['\x1a'] = @"\x1a";
                d['\x1c'] = @"\x1c";
                d['\x1d'] = @"\x1d";
                d['\x1e'] = @"\x1e";
                d['\x1f'] = @"\x1f";
                if (null == _SlashesDictionary)
                {
                    _SlashesDictionary = d;
                }
            }
            return _SlashesDictionary;
        }

        #endregion

        #region IfEmpty

        /// <summary>
        /// Returns first non empty string from a parameter list.
        /// Strings are considered to be empty if they are null.
        /// Function will return empty string ("") if parameter list is empty.
        /// </summary>
        /// <remarks>EBS-0</remarks>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string IfEmpty(params string[] input)
        {
            if (null != input && 0 < input.Length)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (!string.IsNullOrEmpty(input[i]))
                    {
                        return input[i];
                    }
                }
            }
            return "";
        }

        #endregion

        #region IfWhite

        /// <summary>
        /// Returns first non white string from a parameter list. 
        /// White string is considered to be null, zero length or string containing only whitespace characters.
        /// Function will return empty string ("") if parameter list is empty.
        /// </summary>
        /// <remarks>EBS-0</remarks>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string IfWhite(params string[] input)
        {
            if (null != input && 0 < input.Length)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (string.IsNullOrEmpty(input[i]))
                    {
                        continue;
                    }
                    string t = Energy.Base.Text.Trim(input[i]);
                    if (0 == t.Length)
                    {
                        continue;
                    }
                    return input[i];
                }
            }
            return "";
        }

        #endregion

        #region FindAnyWord

        /// <summary>
        /// Find any word from word list using regular expression match.
        /// <br/><br/>
        /// Return null if no word could be found.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="words"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string FindAnyWord(string input, string[] words, bool ignoreCase)
        {
            if (null == input || null == words || 0 == input.Length || 0 == words.Length)
            {
                return null;
            }
            List<string> list = new List<string>();
            foreach (string word in words)
            {
                list.Add("(?:" + EscapeExpression(word) + ")");
            }
            string pattern = ""
                + @"("
                + string.Join("|", list.ToArray())
                + @")"
                ;
            RegexOptions options = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            Match m = Regex.Match(input, pattern, options);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find any word from word list using regular expression match.
        /// <br/><br/>
        /// Return null if no word could be found.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string FindAnyWord(string input, string[] words)
        {
            return FindAnyWord(input, words, false);
        }

        #endregion

        #region Obsolete

        /// <summary>
        /// Capitalize string by uppercasing the first letter, remaining the rest unchanged.
        /// </summary>
        /// <param name="word">Word</param>
        /// <returns>Word</returns>
        [Obsolete("Use Energy.Base.Text.Capitalize() instead.")]
        public static string UpperFirst(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }
            else if (1 == word.Length)
            {
                return char.ToUpper(word[0], CultureInfo.InvariantCulture).ToString();
            }
            else
            {
                return string.Concat(char.ToUpper(word[0], CultureInfo.InvariantCulture), word.Substring(1).ToLower(CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region Match

        public static string Match(string text, Regex regex)
        {
            return Match(text, regex, -1);
        }

        public static string Match(string text, Regex regex, int index)
        {
            Match match = regex.Match(text);
            if (!match.Success)
            {
                return null;
            }
            if (index < 0)
            {
                return match.Value;
            }
            else if (index >= match.Groups.Count)
            {
                return null;
            }
            else
            {
                return match.Groups[index].Value;
            }
        }

        #endregion
    }
}
