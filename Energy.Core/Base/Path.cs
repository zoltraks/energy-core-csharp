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
    }
}
