﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Regular expression pattern constants
    /// </summary>
    public static partial class Expression
    {
        #region Constant

        private readonly static string SPECIAL_CHARACTERS_STRING = "\\.$^{[(|)*+?";

        /// <summary>
        /// Connection string pattern
        /// </summary>
        public static readonly string ConnectionString = "(?<key>{[^}]*}|[^;=\\r\\n]+)(?:=(?<value>\"(?:\"\"|[^\"])*\"|{[^}]*}|[^;]*))?(?:;)?";

        /// <summary>
        /// Geolocation string containg longitude and latitude values.
        ///
        /// Allows matching several standards:
        /// 66° 33′ 39″ N 0° 0′ 0″
        /// 51.5074° N, 0.1278° W
        /// 51,5074° N 0,1278° W
        /// 51,5074 N 0,1278 W
        /// -51,5074 -0,1278
        /// </summary>
        public static readonly string LatitudeAndLongitude = @"
(?:(?<latitude_name>[A-Za-z][^:]*):\s*)?
(?:
(?:
(?<latitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*°\s*
(?:(?<latitude_minute>[-+]?\d+(?:[.,]\d+)?)\s*[′']\s*)?
(?:(?<latitude_second>[-+]?\d+(?:[.,]\d+)?)\s*[″""]\s*)?
)
|
(?:
(?<latitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*
)
)
(?:(?<latitude_direction>[NSns]\s*)?)
(?:[,.|]\s*)?
(?:(?<longitude_name>\w[^:]*):\s*)?
(?:
(?:
(?<longitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*°\s*
(?:(?<longitude_minute>[-+]?\d+(?:[.,]\d+)?)\s*[′']\s*)?
(?:(?<longitude_second>[-+]?\d+(?:[.,]\d+)?)\s*[″""]\s*)?
)
|
(?:
(?<longitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*
)
)
";

        /// <summary>
        /// Date pattern for YYYY-MM-DD, DD-MM-YYYY, DD.MM.YYYY, MM/DD/YYYY or YYYY/MM/DD formats.
        /// </summary>
        public static readonly string Date = @"(?<year>\d{4})\-(?<month>\d{1,2})\-(?<day>\d{1,2})|(?<day>\d{1,2})\-(?<month>\d{1,2})\-(?<year>\d{4})|(?<day>\d{1,2})\.(?<month>\d{1,2})\.(?<year>\d{4})|(?<month>\d{1,2})\/(?<day>\d{1,2})\/(?<year>\d{4})|(?<year>\d{4})\/(?<month>\d{1,2})\/(?<day>\d{1,2})";

        /// <summary>
        /// Time pattern (hour + minute + second + fraction)
        /// </summary>
        public static readonly string Time = @"(((?<hour>\d{1,2}):)?(?<minute>\d{1,2}):)?(?<second>\d{1,2})[,\.](?<fraction>\d{1,7})|((?<hour>\d{1,2}):(?<minute>\d{1,2}))(:(?<second>\d{1,2}))?";

        /// <summary>
        /// Date pattern for YYYYMMDD format.
        /// </summary>
        public static readonly string NumericDate = @"(?:\b|[^0-9])(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?:\b|[^0-9])";

        /// <summary>
        /// URI pattern (scheme + : + // + user + : + password + @ + host + : + port + / + path + ? + query + # + fragment).
        /// </summary>
        public static readonly string Url = @"(?:(?<scheme>\w+):(?<slash>//)?)?(?:(?<user>[^:@\r\n]*)(?::(?<password>[^@\r\n]*))?@)?(?!\d+(?:[\r\n]|$))(?<host>[\w\d_\-\.=]+)(?:\:(?<port>\d+))?(?<path>/[^?#\r\n]*)?(?:\?(?<query>[^#\r\n]*))?(?:\#(?<fragment>[^\r\n]*))?";

        /// <summary>
        /// Matching for "VARCHAR(50) NOT NULL" or "DECIMAL(20, 3) NULL DEFAULT = '';"
        /// </summary>
        public static readonly string SqlTypeDeclaration = @"(?<type>[\w_][\w\d_]*)\s*(?<parameter>\(\s*(?<length>[0-9][^\)]*)\))?(?:\s(?<null>(?:NOT\s+)?NULL))?";

        /// <summary>
        /// Matchin for token in declarations like "CHARACTER VARYING NOT NULL".
        /// </summary>
        public static readonly string SqlDeclarationToken = @"[a-zA-Z][a-zA-Z0-9]*|\([^)]*\)|\[(?:\[]]|[^]])*]|[0-9]+(?:.[0-9]+)?|""(?:""""|[^""])*""|'(?:''|[^'])*'";

        /// <summary>
        /// Matching for elements of list "10 20 30" or "1,2, 3|4|5" or with description like "flower (Rose), fruit (Banana)"
        /// </summary>
        public static readonly string StringListOfValuesWithDescription = "(?<1>[a-zA-Z_0-9]+)\\s*(?:\\((?<2>(?:\\)\\)|[^\\)])*)\\))?\\s*[,|]?\\s*";

        /// <summary>
        /// Matching for database column type, optional argument, nullability option and default values.
        /// </summary>
        public static readonly string SqlTypeGenericDefinition = @"
(?<type>
  (?:[a-zA-Z][a-zA-Z0-9_]*)
  (?:\s*(?!NOT)[a-zA-Z][a-zA-Z0-9_]*){0,1}
)

(?<parameter>
  (?:\s*)
  \(
    (?:\s*(?:(?<size>[0-9]+)\s*))
    (?:(?:(?:,\s*)(?<extra>[0-9])*\s*)?)*?
  \)
)?

(?:\s*(?:

(?<null>(?:NOT\s*)?(?:NULL))

|

(?:DEFAULT\s*)
(?<default>""(?:""""|[^""])*""|'(?:''|[^'])*'|[a-zA-Z_][a-zA-Z_\-0-9]*)?

|

(?<option>AUTO_INCREMENT|COMMENT|COLLATE|FORMAT|COLUMN_FORMAT)
(?:\s*(?<value>""(?:""""|[^""])*""|'(?:''|[^'])*'|[a-zA-Z_][a-zA-Z_\-0-9]*))?

))*
";

        /// <summary>
        /// Expression for splitting path into segments
        /// </summary>
        public static readonly string PathSplitCapture = @"
^
(
(?:[A-Za-z][A-Za-z0-9]*:(?:[\\/]+)?)
|
(?:\\\\[A-Za-z][A-Za-z0-9]*(?:[\\/]+)?)
|
(?:[\\/]+)
)?
(
(?:[^\\/\r\n\v]+(?:[\\/]+)?)
)
+
";

        public static readonly string PathSplitCaptureUniversal = @"
(?<white>[\r\n]+)
|
(?<slash>[\\/]+)
|
(?<item>
[^\\/""'`\r\n]+
|
""(?:\\""|""""|[^""])*""
|
'(?:\\'|''|[^'])*'
|
`(?:\\`|``|[^`])*`
)+
";

        /// <summary>
        /// Expression for finding root element in XML
        /// </summary>
        public static readonly string XmlRootName = @"(?:<\?[xX][mM][lL][^>]*>\s*)?(?:<\s*([a-zA-Z_][^\s>]*))";
        
        /// <summary>
        /// Expression for XML tag line (informal)
        /// </summary>
        public static string XmlTagLineInformal = @"<\s*(?<name>[\?!a-zA-Z:_][a-zA-Z0-9:.\-\u00B7\u0300-\u036F\u203F-\u2040_]*)(?<attribute>(?:(?:\s+(?:""(?:""""|\\""|[^""])*""|[^=>/?\s]+)(?:\s*=\s*(?:""(?:""""|\\""|[^""])*)""|[^>/?\s]+)))*)(?<end>\s*(?:[/?]?\s*)>)?";

        /// <summary>
        /// Expression for matching tilde color text
        /// </summary>
        public static readonly string TildeText = @"~`(?:``|[^`])*`~|~\d+~|~[\w\d]+~|~+?|[^~]+";

        #endregion

        #region Utility

        #region Escape

        /// <summary>
        /// Escape character with backslash if is one of special for regular expressions.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="special">Special characters string, like "\\.$^{[(|)*+?"</param>
        /// <returns></returns>
        public static string Escape(char? character, string special)
        {
            if (character == null)
                return null;
            else if (special == null || 0 >= special.IndexOf((char)character))
                return ((char)character).ToString();
            else
                return string.Concat('\\', (char)character);
        }

        /// <summary>
        /// Escape character with backslash if is one of special for regular expressions.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Escape(char? character)
        {
            return Escape(character, SPECIAL_CHARACTERS_STRING);
        }

        public static string Escape(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            StringBuilder b = new StringBuilder();
            foreach (char c in text.ToCharArray())
            {
                b.Append(Escape(c));
            }
            string s = b.ToString();
            return s;
        }

        public static string EscapeJoin(string glue, char[] array)
        {
            if (array == null)
                return null;
            if (array.Length == 0)
                return "";
            List<string> list = new List<string>();
            foreach (char character in array)
            {
                string e = Energy.Base.Text.EscapeExpression((char)character);
                list.Add(e);
            }
            return string.Join(glue, list.ToArray());
        }

        public static string EscapeSurround(string prefix, string suffix, char[] array)
        {
            if (array == null)
                return null;
            if (array.Length == 0)
                return "";
            StringBuilder s = new StringBuilder();
            foreach (char character in array)
            {
                s.Append(prefix);
                s.Append(Energy.Base.Text.EscapeExpression((char)character));
                s.Append(suffix);
            }
            return s.ToString();
        }

        #endregion

        #region Group

        /// <summary>
        /// Get group description list from regular expression match.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="value"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Class.GroupDescription GetGroupDescription(string pattern, string value, RegexOptions option)
        {
            Regex regex = new Regex(pattern, option);
            return GetGroupDescription(regex, regex.Match(value));
        }

        /// <summary>
        /// Get group description list from regular expression match.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static Class.GroupDescription GetGroupDescription(string pattern, Match match)
        {
            Regex regex = new Regex(pattern);
            return GetGroupDescription(regex, match);
        }

        /// <summary>
        /// Get group description list from regular expression match.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static Class.GroupDescription GetGroupDescription(Match match)
        {
            return GetGroupDescription(default(Regex), match);
        }

        /// <summary>
        /// Get group description list from regular expression match.
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static Class.GroupDescription GetGroupDescription(Regex regex, Match match)
        {
            Class.GroupDescription result;
            result = new Class.GroupDescription();
            string[] groupNames = null == regex ? null : regex.GetGroupNames();
            for (int i = 0; i < match.Groups.Count; i++)
            {
                Group group = match.Groups[i];
                Class.GroupDescription.Item item = new Class.GroupDescription.Item()
                {
                    Key = groupNames != null && groupNames.Length > i 
                        ? groupNames[i] : null,
                    Value = group.Value,
                    Index = group.Index,
                    Length = group.Length,
                    Order = i,
                };
                result.Add(item);
            }
            return result;
        }
        #endregion

        #region MatchNamedGroups

        /// <summary>
        /// Match named capture groups in text and return them as Dictionary of strings.
        /// Returning Dictionary object will always contain keys.
        /// Values that were not captured will remain null values.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="regexPattern"></param>
        /// <param name="regexOptions"></param>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> MatchNamedGroups(string inputText, string regexPattern, RegexOptions regexOptions, string[] groupNames)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Regex regex = null;
            List<string> groups = null;
            try
            {
                if (null == groupNames)
                {
                    if (string.IsNullOrEmpty(regexPattern))
                    {
                        return result;
                    }
                    regex = regex ?? new Regex(regexPattern, regexOptions);
                    List<string> filtered = new List<string>();
                    foreach (string candidate in regex.GetGroupNames())
                    {
                        if (Regex.Match(candidate, "^[0-9]+$").Success)
                        {
                            continue;
                        }
                        filtered.Add(candidate);
                    }
                    groupNames = filtered.ToArray();
                    groups = groups ?? new List<string>(groupNames);
                }

                if (0 == groupNames.Length)
                {
                    return result;
                }

                foreach (string name in groupNames)
                {
                    result[name] = null;
                }

                if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(regexPattern))
                {
                    return result;
                }

                regex = regex ?? new Regex(regexPattern, regexOptions);
                groups = groups ?? new List<string>(regex.GetGroupNames());

                Match match = Regex.Match(inputText, regexPattern, regexOptions);
                if (!match.Success)
                {
                    return result;
                }
                foreach (string key in groupNames)
                {
                    if (!groups.Contains(key))
                    {
                        continue;
                    }
                    result[key] = match.Groups[key].Value;
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(Energy.Base.Clock.CurrentTime + " " + x.Message);
            }
            return result;
        }

        #endregion

        #endregion

        #region Class

        public class Class
        {
            public class GroupDescription : List<GroupDescription.Item>
            {
                public class Item
                {
                    public string Key;
                    public string Value;
                    public int Index;
                    public int Length;
                    public int? Order;

                    public override string ToString()
                    {
                        List<string> l = new List<string>();
                        if (Order != null)
                        {
                            l.Add("" + Order + ".");
                        }
                        if (Key != null)
                        {
                            if (Key != "")
                                l.Add("(" + Key + ")");
                        }
                        if (!string.IsNullOrEmpty(Value))
                        {
                            if (l.Count > 0)
                                l.Add("=");
                            l.Add(Energy.Base.Text.Quote(Value));
                        }
                        l.Add(Index + ":" + Length);
                        return string.Join(" ", l.ToArray());
                    }
                }

                public override string ToString()
                {
                    List<string> l = new List<string>();
                    foreach (Item item in this)
                    {
                        l.Add(item.ToString());
                    }
                    return string.Join(Energy.Base.Text.NL, l.ToArray());
                }
            }
        }

        #endregion
    }
}
