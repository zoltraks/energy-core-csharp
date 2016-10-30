using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Plain text functions
    /// </summary>
    public class Plain
    {
        /// <summary>
        /// Separated values functions
        /// </summary>
        public class Separated
        {
            public static string[] Explode(string line, char[] separator, char[] enclosure, char[] white)
            {
                if (line == null) return null;
                int l = line.Length;
                if (l == 0) return new string[] { };
                List<string> result = new List<string>();
                char[] a = line.ToCharArray();
                char q = '\0';

                int _s_ = separator == null ? 0 : separator.Length;
                int _e_ = enclosure == null ? 0 : enclosure.Length;
                int _w_ = white == null ? 0 : white.Length;

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
                                if (a[i] == white[n])
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
                            if (i < l && a[i + 1] == q)
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

            public static string[] Explode(string line, char separator, char enclosure)
            {
                return Explode(line
                    , separator == '\0' ? null : new char[] { separator }
                    , enclosure == '\0' ? null : new char[] { enclosure }
                    , new char[] { ' ', '\t' }
                    );
            }

            public static string[] Explode(string line, char separator)
            {
                return Explode(line, separator, '"');
            }

            public static string[] Explode(string line, string separator)
            {
                return Explode(line, string.IsNullOrEmpty(separator) ? '\0' : separator[0]);
            }
        }
    }
}
