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
            if (path == null)
                return null;
            if (path.Length == 0)
                return new string[] { };
            Regex r = new Regex(Energy.Base.Expression.PathSplitCapture, RegexOptions.IgnorePatternWhitespace);
            Match m = r.Match(path);
            if (!m.Success)
                return new string[] { };
            List<string> l = new List<string>();
            for (int n = 1; n < m.Groups.Count; n++)
            {
                for (int i = 0; i < m.Groups[n].Captures.Count; i++)
                {
                    l.Add(m.Groups[n].Captures[i].Value);
                }
            }
            return l.ToArray();
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
        /// Strip quotation from file path.
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
    }
}
