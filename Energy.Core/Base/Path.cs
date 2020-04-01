using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    public static class Path
    {
        #region Split

        /// <summary>
        /// Split path into segments.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] Split(string path)
        {
            return Split(path, null);
        }

        //public static string[] Split(string path)
        //{
        //    if (path == null)
        //        return null;
        //    if (path.Length == 0)
        //        return new string[] { };
        //    Regex r = new Regex(Energy.Base.Expression.PathSplitCapture, RegexOptions.IgnorePatternWhitespace);
        //    Match m = r.Match(path);
        //    if (!m.Success)
        //        return new string[] { };
        //    List<string> l = new List<string>();
        //    for (int n = 1; n < m.Groups.Count; n++)
        //    {
        //        for (int i = 0; i < m.Groups[n].Captures.Count; i++)
        //        {
        //            l.Add(m.Groups[n].Captures[i].Value);
        //        }
        //    }
        //    return l.ToArray();
        //}

        public static string[] Split(string path, SplitFormat format)
        {
            if (null == path)
            {
                return null;
            }
            if (0 == path.Length)
            {
#pragma warning disable CA1825 // Avoid zero-length array allocations.
                return new string[] { };
#pragma warning restore CA1825 // Avoid zero-length array allocations.
            }
            if (format == null)
            {
                format = SplitFormat.Default;
            }
            //if (options == null)
            //{
            //    options = new SplitOptions();
            //}

            string[] slashes = format.Slashes ?? new string[] { };
            string[] quotes = format.Quotes ?? new string[] { };
            bool optionDoublets = (bool)(format.Doublets ?? false);
            bool optionCStyle = (bool)(format.CStyle ?? false);
            //bool optionIncludeSeparator = (bool)(options.IncludeSeparator ?? false);
            //bool optionIcludeWhitespace = (bool)(options.IncludeWhitespace ?? false);

            List<string> list = new List<string>();

            string pattern = BuildSplitPattern(slashes, quotes, optionDoublets, optionCStyle);

            Match match = Regex.Match(path, pattern);

            while (match.Success)
            {
                if (false)
                { }
                else if (0 < match.Groups["white"].Length)
                {
                    //if (optionIcludeWhitespace)
                    //{
                    //    list.Add(match.Groups["white"].Value);
                    //}
                }
                else if (0 < match.Groups["slash"].Length)
                {
                    //if (optionIncludeSeparator)
                    //{
                    //    list.Add(match.Groups["slash"].Value);
                    //}
                    if (0 == list.Count)
                    {
                        list.Add(match.Groups["slash"].Value);
                    }
                    else
                    {
                        list[-1 + list.Count] += match.Groups["slash"].Value;
                    }
                }
                else if (0 < match.Groups["item"].Length)
                {
                    list.Add(match.Groups["item"].Value);
                }
                match = match.NextMatch();
            }

            return list.ToArray();
        }

        public static string[] Split(string path, string[] slashes, string[] quotes
            , bool? doublets, bool? cstyle)
        {
            return Split(path, new SplitFormat()
            {
                Slashes = slashes,
                Quotes = quotes,
                Doublets = doublets,
                CStyle = cstyle
            });
        }

        public static string[] Split(string path, string[] slashes, string[] quotes)
        {
            return Split(path, new SplitFormat()
            {
                Slashes = slashes,
                Quotes = quotes
            });
        }

        #endregion

        #region SplitFormat

        public class SplitFormat
        {
            /// <summary>
            /// Array of supported slash separators withing path.
            /// </summary>
            public string[] Slashes;

            /// <summary>
            /// Array of supported quotation mark characters.
            /// </summary>
            public string[] Quotes;

            /// <summary>
            /// Allow to use double quotes to escape quotation mark characters.
            /// </summary>
            public bool? Doublets;

            /// <summary>
            /// Allow to use C style slashes in names for escaping quotation mark characters.
            /// </summary>
            public bool? CStyle;

            private static SplitFormat _Default;

            public static SplitFormat Default
            {
                get
                {
                    if (null == _Default)
                    {
                        _Default = new SplitFormat()
                        {
                            Slashes = new string[] { "\\", "/" },
                            Quotes = new string[] { "\"", "'", "`" },
                            Doublets = true,
                            CStyle = true,
                        };
                    }
                    return _Default;
                }
            }

            public static SplitFormat Create(string[] slashes, string[] quotes, bool? doublets, bool? cstyle)
            {
                SplitFormat o = new SplitFormat()
                {
                    Slashes = slashes,
                    Quotes = quotes,
                    Doublets = doublets,
                    CStyle = cstyle
                };
                return o;
            }

            public static SplitFormat Create(string[] slashes, string[] quotes)
            {
                return Create(slashes, quotes, null, null);
            }

            public static SplitFormat Create(string slashes, string quotes, bool? doublets, bool? cstyle)
            {
                char[] charsSlashes = slashes.ToCharArray();
                string[] arraySlashes = new List<char>(charsSlashes)
                    .ConvertAll<string>(delegate (char c) { return new string(c, 1); })
                    .ToArray();
                char[] charsQuotes = quotes.ToCharArray();
                string[] arrayQuotes = new List<char>(charsQuotes)
                    .ConvertAll<string>(delegate (char c) { return new string(c, 1); })
                    .ToArray();
                SplitFormat o = new SplitFormat()
                {
                    Slashes = arraySlashes,
                    Quotes = arrayQuotes,
                    Doublets = doublets,
                    CStyle = cstyle
                };
                return o;
            }

            public static SplitFormat Create(string slashes, string quotes)
            {
                return Create(slashes, quotes, null, null);
            }
        }

        #endregion

        #region SplitOptions

        public class SplitOptions
        {
            /// <summary>
            /// Include path separator strings in results.
            /// </summary>
            public bool? IncludeSeparator;

            /// <summary>
            /// Include whitespace between paths in results.
            /// </summary>
            public bool? IncludeWhitespace;
        }

        #endregion

        #region BuildSplitPattern

        /// <summary>
        /// Build regular expression pattern for splitting path.
        /// </summary>
        /// <param name="slashes"></param>
        /// <param name="quotes"></param>
        /// <param name="doublets"></param>
        /// <param name="cstyle"></param>
        /// <returns></returns>
        public static string BuildSplitPattern(string[] slashes, string[] quotes, bool doublets, bool cstyle)
        {
            List<string> t = new List<string>();
            t.Add("(?<white>[\\r\\n]+)");
            // tabs and special characters need to be changed to regular expression pattern equivalents too.
            // that's why whe should cover this with external function.
            string escape = ".+*?()[{|\\^$#";

            string _slash = "";
            if (0 < slashes.Length)
            {
                var b = new StringBuilder();
                int n = slashes.Length;
                for (int i = 0; i < n; i++)
                {
                    foreach (char c in slashes[i].ToCharArray())
                    {
                        if (0 <= escape.IndexOf(c))
                        {
                            b.Append('\\');
                        }
                        b.Append(c);
                    }
                }
                _slash = b.ToString();
            }
            if (0 < _slash.Length)
            {
                t.Add("(?<slash>[" + _slash + "]+)");
            }

            string _quote = "";
            if (0 < quotes.Length)
            {
                var b = new StringBuilder();
                int n = quotes.Length;
                for (int i = 0; i < n; i++)
                {
                    foreach (char c in quotes[i].ToCharArray())
                    {
                        if (0 <= escape.IndexOf(c))
                        {
                            b.Append('\\');
                        }
                        b.Append(c);
                    }
                }
                _quote = b.ToString();
            }

            var v = new List<string>();
            v.Add("[^" + _slash + _quote + "\\r\\n" + "]+");
            if (0 < quotes.Length)
            {
                int n = quotes.Length;
                for (int i = 0; i < n; i++)
                {
                    var b = new StringBuilder();
                    foreach (char c in quotes[i].ToCharArray())
                    {
                        if (0 <= escape.IndexOf(c))
                        {
                            b.Append('\\');
                        }
                        b.Append(c);
                    }
                    var q = b.ToString();
                    var x = new List<string>();
                    if (doublets)
                    {
                        x.Add(q + q);
                    }
                    if (cstyle)
                    {
                        x.Add("\\\\" + q);
                    }
                    x.Add("[^" + q + "]");
                    var s = q + "(?:(?:" + string.Join("|", x.ToArray()) + ")*)" + q;
                    v.Add(s);
                }
            }
            t.Add("(?<item>(" + string.Join("|", v.ToArray()) + ")+)");

            var r = string.Join("|", t.ToArray());
            return r;
        }

        #endregion

        #region Short

        /// <summary>
        /// Shorten path from left side to match limit optionally including leading prefix.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="limit"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string ShortLeft(string path, int limit, string prefix)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= limit)
                return path;
            List<string> list = new List<string>(Energy.Base.Path.Split(path));
            if (!string.IsNullOrEmpty(prefix))
                limit -= prefix.Length;
            if (limit <= 0)
                return "";
            while (limit < Energy.Base.Collection.StringList.GetTotalLength(list))
            {
                if (list.Count == 1)
                    break;
                list.RemoveAt(0);
            }
            if (!string.IsNullOrEmpty(prefix))
                list.Insert(0, prefix);
            return string.Join("", list.ToArray());
        }

        /// <summary>
        /// Shorten path from right side to match limit optionally including trailing suffix.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="limit"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string ShortRight(string path, int limit, string suffix)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= limit)
                return path;
            List<string> list = new List<string>(Energy.Base.Path.Split(path));
            if (!string.IsNullOrEmpty(suffix))
                limit -= suffix.Length;
            if (limit <= 0)
                return "";
            while (limit < Energy.Base.Collection.StringList.GetTotalLength(list))
            {
                if (list.Count == 1)
                    break;
                list.RemoveAt(list.Count - 1);
            }
            if (!string.IsNullOrEmpty(suffix))
                list.Add(suffix);
            string line = string.Join("", list.ToArray());
            return line;
        }

        /// <summary>
        /// Shorten path from middle to match limit optionally including trailing suffix.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="limit"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string ShortMiddle(string path, int limit, string suffix)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= limit)
                return path;
            List<string> list = new List<string>(Energy.Base.Path.Split(path));
            if (!string.IsNullOrEmpty(suffix))
                limit -= suffix.Length;
            if (limit <= 0)
                return "";
            if (list.Count > 2)
            {
                List<string> left = new List<string>();
                List<string> right = new List<string>();
                left.Add(list[0]);
                right.Add(list[list.Count - 1]);
                list.RemoveAt(0);
                list.RemoveAt(list.Count - 1);
                int total = left[0].Length + right[0].Length;
                int i = 0;
                while (list.Count > 0)
                {
                    int n = (++i % 2) * (list.Count - 1);
                    string v = list[n];
                    if (total + v.Length > limit)
                        break;
                    if (i % 2 == 0)
                        left.Add(v);
                    else
                        right.Insert(0, v);
                    total += v.Length;
                    list.RemoveAt(n);
                }
                string line = string.Concat(string.Join("", left.ToArray()), suffix, string.Join("", right.ToArray()));
                return line;
            }
            return string.Join("", list.ToArray());
        }

        #endregion

        #region Separator

        /// <summary>
        /// Change any directory separator to native one for compatibility.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ChangeSeparator(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return filePath;
            if (System.IO.Path.DirectorySeparatorChar == '\\')
            {
                if (filePath.IndexOf('/') < 0)
                    return filePath;
                else
                    return filePath.Replace('/', '\\');
            }
            if (System.IO.Path.DirectorySeparatorChar == '/')
            {
                if (filePath.IndexOf('\\') < 0)
                    return filePath;
                else
                    return filePath.Replace('\\', '/');
            }
            return filePath;
        }

        /// <summary>
        /// Convert UNIX slashes into DOS backslashes in path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToDos(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            if (!path.Contains("/"))
                return path;
            return path.Replace("/", "\\");
        }

        /// <summary>
        /// Converts DOS backslashes into UNIX slashes in path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToUnix(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            if (!path.Contains("\\"))
                return path;
            return path.Replace("\\", "/");
        }

        /// <summary>
        /// Include trailing path separator if not exists. Works with DOS and UNIX style.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string IncludeTrailingSeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            if (path.EndsWith("\\") || path.EndsWith("/"))
                return path;
            path = string.Concat(path, System.IO.Path.DirectorySeparatorChar);
            return path;
        }

        #endregion

        #region Strip

        /// <summary>
        /// Strip quotation marks from file path.
        /// Converts C:\"Program Files"\"Dir" into C:\Program Files\Dir.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string StripQuotation(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            if (!path.Contains("\""))
                return path;
            return path.Replace("\"", null);
        }

        #endregion

        #region Environment

        /// <summary>
        /// Functions related to environmental path variable. 
        /// You may pass %PATH% as System.Environment.GetEnvironmentVariable("PATH").
        /// </summary>
        public class Environment
        {
            /// <summary>
            /// Split path string as used in environment variables by a platform-specific separator character.
            /// </summary>
            /// <param name="pathVariable"></param>
            /// <param name="splitChars"></param>
            /// <returns></returns>
            public static string[] Split(string pathVariable, char[] splitChars)
            {
                if (false)
                { }
                else if (null == pathVariable)
                {
                    return null;
                }
                else if ("" == pathVariable)
                {
                    return new string[] { "" };
                }
                else if (null == splitChars)
                {
                    splitChars = new char[] { System.IO.Path.PathSeparator };
                }
                else if (0 == splitChars.Length)
                {
                    return new string[] { pathVariable };
                }

                string token = "";
                if (splitChars.Length == 1)
                {
                    token = Energy.Base.Expression.Escape(splitChars[0]);
                }
                else
                {
                    token += "[";
                    foreach (char splitChar in splitChars)
                    {
                        token += Energy.Base.Expression.Escape(splitChar);
                    }
                    token += "]";
                }
                string pattern = token + @"(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
                return Regex.Split(pathVariable, pattern);
            }

            /// <summary>
            /// Split path string as used in environment variables by a platform-specific separator character.
            /// </summary>
            /// <param name="pathVariable"></param>
            /// <returns></returns>
            public static string[] Split(string pathVariable)
            {
                return Split(pathVariable, null);
            }

            /// <summary>
            /// Get array of directories to search for executable files from PATH enviromental variable.
            /// </summary>
            public static string[] AsArray
            {
                get
                {
                    return GetAsArray();
                }
            }

            private static string[] GetAsArray()
            {
                return Split(System.Environment.GetEnvironmentVariable("PATH"), null);
            }
        }

        #endregion

        #region Walk

        /// <summary>
        /// Walk through relative path and return without any dot folders.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="currentDirectory">Optional current directory for dot folder</param>
        /// <returns></returns>
        public static string Walk(string path, string currentDirectory)
        {
            return path;
        }

        #endregion

        #region Each

        public static IEnumerable<string> Each(string path, SplitFormat format)
        {
            if (null == path)
            {
                yield break;
            }
            string[] array = Energy.Base.Path.Split(path, format);
            for (int i = 0, length = array.Length; i < length; i++)
            {
                yield return array[i];
            }
        }

        public static IEnumerable<string> Each(string path)
        {
            return Each(path, null);
        }

        #endregion

        #region BuildSeparatorPattern

        public static string BuildSeparatorPattern(string[] slashes)
        {
            if (null == slashes)
            {
                return null;
            }
            if (0 == slashes.Length)
            {
                return "";
            }
            List<string> b = new List<string>(), c = new List<string>();
            for (int i = 0, length = slashes.Length; i < length; i++)
            {
                string s = slashes[i];
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }
                if (1 == s.Length)
                {
                    c.Add(Energy.Base.Text.EscapeExpression(s[0]));
                }
                else
                {
                    b.Add(Energy.Base.Text.EscapeExpression(s));
                }
            }
            string p = "";
            if (0 < c.Count)
            {
                if (1 == c.Count)
                {
                    p = c[0];
                }
                else
                {
                    p = "[" + string.Concat(c.ToArray()) + "]";
                }
                if (0 == b.Count)
                {
                    p += "+";
                }
                else
                {
                    b.Add(p);
                }
            }
            string pattern = 0 == b.Count ? p : "(?:" + string.Join("|", b.ToArray()) + ")+";
            return pattern;
        }

        #endregion

        #region IsSeparator

        /// <summary>
        /// Check if file path part is directory separator or not.
        /// Multiple separator characters are allowed.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="slashes">
        /// Array of strings  
        /// </param>
        /// <returns></returns>
        public static bool IsSeparator(string part, string[] slashes)
        {
            string pattern = BuildSeparatorPattern(slashes);
            Match match = Regex.Match(part, pattern);
            bool success = match.Success;
            if (success)
            {
                success = part.Length == match.Length;
            }
            return success;
        }
        public static bool IsSeparator(string part, string slashes)
        {
            char[] charsSlashes = slashes.ToCharArray();
            string[] arraySlashes = new List<char>(charsSlashes)
                .ConvertAll<string>(delegate (char c) { return new string(c, 1); })
                .ToArray();
            return IsSeparator(part, arraySlashes);
        }

        public static bool IsSeparator(string part)
        {
            return IsSeparator(part, "\\/");
        }

        #endregion

        #region TrimLeft

        public static string TrimLeft(string path, string[] slashes)
        {
            if (null == slashes)
            {
                return null;
            }
            string s = Energy.Base.Path.BuildSeparatorPattern(slashes);
            if (string.IsNullOrEmpty(s))
            {
                return path;
            }
            string p = "^" + s;
            path = Regex.Replace(path, p, "");
            return path;
        }

        public static string TrimLeft(string path)
        {
            return TrimLeft(path, new string[] { "\\", "/" });
        }

        #endregion

        #region TrimRight

        public static string TrimRight(string path, string[] slashes)
        {
            if (null == slashes)
            {
                return null;
            }
            string s = Energy.Base.Path.BuildSeparatorPattern(slashes);
            if (string.IsNullOrEmpty(s))
            {
                return path;
            }
            string p = s + "$";
            path = Regex.Replace(path, p, "");
            return path;
        }

        public static string TrimRight(string path)
        {
            return TrimRight(path, new string[] { "\\", "/" });
        }

        #endregion

        #region Trim

        public static string Trim(string path, string[] slashes)
        {
            if (null == slashes)
            {
                return null;
            }
            string s = Energy.Base.Path.BuildSeparatorPattern(slashes);
            if (string.IsNullOrEmpty(s))
            {
                return path;
            }
            string p = "^" + s + "|" + s + "$";
            path = Regex.Replace(path, p, "");
            return path;
        }

        public static string Trim(string path)
        {
            return Trim(path, new string[] { "\\", "/" });
        }

        #endregion
    }
}
