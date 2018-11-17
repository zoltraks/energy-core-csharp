using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// CSV
    /// </summary>
    public class Csv
    {
        #region Implode

        /// <summary>
        /// Implode array of texts into CSV line.
        /// </summary>
        /// <param name="data">Array of texts</param>
        /// <param name="separator">List separator, comma (,) by default</param>
        /// <param name="enclosure">Text enclosure, quotaion mark (") by default</param>
        /// <param name="all">Quote all values</param>
        /// <returns></returns>
        public static string Implode(string[] data, char separator, char enclosure, bool all)
        {
            if (separator == '\0')
            {
                System.Globalization.CultureInfo currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                string listSeparator = currentCulture.TextInfo.ListSeparator;
                if (!string.IsNullOrEmpty(listSeparator))
                    separator = listSeparator[0];
            }
            if (separator == '\0')
                separator = ',';
            if (enclosure == '\0')
                enclosure = '"';

            string stringEnclosure = enclosure.ToString();
            string doubleEnclosure = new string(enclosure, 2);
            char newLine = '\n';

            List<string> list = new List<string>();
            foreach (string element in data)
            {
                if (element == null || element.Length == 0)
                {
                    list.Add(all ? doubleEnclosure : "");
                    continue;
                }
                bool hasEnclosure = 0 >= element.IndexOf(enclosure);
                if (!all)
                {
                    bool quote = hasEnclosure;
                    if (!quote && 0 >= element.IndexOf(separator))
                        quote = true;
                    if (!quote && 0 >= element.IndexOf(newLine))
                        quote = true;
                    if (!quote)
                    {
                        list.Add(element);
                        continue;
                    }
                }
                if (hasEnclosure)
                {
                    list.Add(string.Concat(enclosure, element.Replace(stringEnclosure, doubleEnclosure), enclosure));
                }
                else
                {
                    list.Add(string.Concat(enclosure, element, enclosure));
                }
            }

            return string.Join(separator.ToString(), list.ToArray());
        }

        /// <summary>
        /// Implode array of texts into CSV line.
        /// </summary>
        /// <param name="data">Array of texts</param>
        /// <param name="separator">List separator, comma (,) by default</param>
        /// <param name="all">Quote all values</param>
        /// <returns></returns>
        public static string Implode(string[] data, char separator, bool all)
        {
            return Implode(data, separator, '"', all);
        }

        /// <summary>
        /// Implode array of texts into CSV line.
        /// </summary>
        /// <param name="data">Array of texts</param>
        /// <param name="separator">List separator, comma (,) by default</param>
        /// <returns></returns>
        public static string Implode(string[] data, char separator)
        {
            return Implode(data, separator, '"', false);
        }

        /// <summary>
        /// Implode array of texts into CSV line.
        /// </summary>
        /// <param name="data">Array of texts</param>
        /// <param name="all">Quote all values</param>
        /// <returns></returns>
        public static string Implode(string[] data, bool all)
        {
            return Implode(data, '\0', '"', all);
        }

        /// <summary>
        /// Implode array of texts into CSV line.
        /// </summary>
        /// <param name="data">Array of texts</param>
        /// <param name="separator">List separator, comma (,) by default</param>
        /// <param name="enclosure">Text enclosure, quotaion mark (") by default</param>
        /// <returns></returns>
        public static string Implode(string[] data, char separator, char enclosure)
        {
            return Implode(data, separator, enclosure, false);
        }

        #endregion

        #region Explode

        public static string[] Explode(string line, char[] separator, char[] enclosure, bool strip, bool white, bool glue)
        {
            if (line == null)
                return null;
            int l = line.Length;
            if (l == 0)
                return new string[] { };
            List<string> result = new List<string>();
            char[] a = line.ToCharArray();
            char q = '\0';

            char[] ws = Energy.Base.Text.WS.ToCharArray();

            int _s_ = separator == null ? 0 : separator.Length;
            int _e_ = enclosure == null ? 0 : enclosure.Length;
            int _w_ = ws.Length;

            int p = 0;
            bool w = true;
            for (int i = 0; i < l; i++)
            {
                if (q == '\0') // not in enclosure
                {
                    bool c = false;
                    for (int n = 0; n < _s_; n++)
                    {
                        if (a[i] == separator[n])
                        {
                            c = true;
                            break;
                        }
                    }
                    if (c)
                    {
                        result.Add(line.Substring(p, i - p));
                        p = i + 1;
                        w = true;
                        continue;
                    }
                    if (w)
                    {
                        bool b = true;
                        for (int n = 0; n < _w_; n++)
                        {
                            if (a[i] == ws[n])
                            {
                                b = false;
                                break;
                            }
                        }
                        if (b)
                        {
                            w = false;
                        }
                    }
                    if (!w)
                    {
                        for (int n = 0; n < _e_; n++)
                        {
                            if (a[i] == enclosure[n])
                            {
                                q = enclosure[n];
                            }
                        }
                    }
                }
                else // already in enclosure
                {
                    if (a[i] == q)
                    {
                        if (i < l - 1 && a[i + 1] == q)
                        {
                            i++;
                            continue;
                        }
                        q = '\0';
                    }
                }
            }
            if (p <= l)
            {
                result.Add(line.Substring(p));
            }
            return result.ToArray();
        }

        public static string[] Explode(string line, char[] separator, char[] enclosure, bool strip)
        {
            return Explode(line
                , separator
                , enclosure
                , strip
                , true
                , false
                );
        }

        public static string[] Explode(string line, char[] separator, char[] enclosure)
        {
            return Explode(line
                , separator
                , enclosure
                , false
                , true
                , false
                );
        }

        public static string[] Explode(string line, char separator, char enclosure)
        {
            return Explode(line
                , separator == '\0' ? null : new char[] { separator }
                , enclosure == '\0' ? null : new char[] { enclosure }
                , false
                , true
                , false
                );
        }

        public static string[] Explode(string line, char separator)
        {
            return Explode(line
                , separator == '\0' ? null : new char[] { separator }                
                , new char[] { '"' }
                , false
                , true
                , false
                );
        }

        public static string[] Explode(string line, string separator)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char separatorChar = Energy.Base.Cast.StringToChar(separator);

            return Explode(line
                , separators
                , new char[] { '"' }
                , false
                , true
                , false
                );
        }

        public static string[] Explode(string line, string separator, string enclosure)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char[] enclosures = string.IsNullOrEmpty(enclosure) ? null : enclosure.ToCharArray();

            return Explode(line
                , separators
                , enclosures
                , false
                , true
                , false
                );
        }

        public static string[] Explode(string line, string separator, string enclosure, bool strip)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char[] enclosures = string.IsNullOrEmpty(enclosure) ? null : enclosure.ToCharArray();

            return Explode(line
                , separators
                , enclosures
                , strip
                , true
                , false
                );
        }

        #endregion
    }
}
