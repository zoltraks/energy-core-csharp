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

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, char[] separator, char[] enclosure, bool strip, bool white, bool equals, bool glue)
        {
            if (line == null)
                return new string[] { };
            int l = line.Length;
            if (l == 0)
                return new string[] { };
            List<string> result = new List<string>();
            char[] a = line.ToCharArray();
            char q = '\0';

            char Q = '\0';
            char E = '\0';

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
                    // check if current character is separator
                    for (int n = 0; n < _s_; n++)
                    {
                        if (a[i] == separator[n])
                        {
                            c = true;
                            break;
                        }
                    }
                    if (c) // is separator?
                    {
                        // add value
                        string v = line.Substring(p, i - p);
                        if (strip && Q != '\0')
                        {
                            if (E == '=')
                            {
                                v = v.Substring(1);
                                E = '\0'; // reset euqals sign usage
                            }
                            v = Energy.Base.Text.Strip(v, Q);
                            Q = '\0'; // reset last character used in text enclosure
                        }
                        else if (!white)
                        {
                            v = v.Trim();
                        }
                        result.Add(v);

                        p = i + 1;
                        w = true;
                        continue;
                    }
                    if (w)
                    {
                        bool b = true;
                        // check if current character is white
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
                        // check if equals sign is used with enclosures
                        if (equals && a[i] == '=' && _e_ > 0 && i < l - 2)
                        {
                            bool b = false;
                            for (int n = 0; n < _e_; n++)
                            {
                                if (a[i + 1] == enclosure[n])
                                {
                                    q = enclosure[n];
                                    p = i; // store current position (no matter if white parameter was used or not)
                                    E = '='; // mark that equals sign was used
                                    i++; // move to next character
                                    b = true;
                                    break;
                                }
                            }
                            if (b)
                            {
                                continue;
                            }
                        }

                        // check if this is last character
                        if (i == l - 1)
                            break;

                        // check if current character is enclosure
                        for (int n = 0; n < _e_; n++)
                        {
                            if (a[i] == enclosure[n])
                            {
                                q = enclosure[n];
                                p = i; // store current position (no matter if white parameter was used or not)
                                break;
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
                        Q = q; // remember quote character used in text enclosure
                        q = '\0';
                    }
                }
            }
            if (p <= l)
            {
                // add value
                string v = line.Substring(p);
                if (strip && Q != '\0')
                {
                    if (E == '=')
                    {
                        v = v.Substring(1);
                    }
                    v = Energy.Base.Text.Strip(v, Q);
                }
                else if (!white)
                {
                    v = v.Trim();
                }
                result.Add(v);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, char[] separator, char[] enclosure, bool strip)
        {
            return Explode(line
                , separator
                , enclosure
                , strip
                , true
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, char[] separator, char[] enclosure)
        {
            return Explode(line
                , separator
                , enclosure
                , false
                , true
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, char separator, char enclosure)
        {
            return Explode(line
                , separator == '\0' ? null : new char[] { separator }
                , enclosure == '\0' ? null : new char[] { enclosure }
                , false
                , true
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, char separator)
        {
            return Explode(line
                , separator == '\0' ? null : new char[] { separator }
                , new char[] { '"' }
                , false
                , true
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, string separator)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char separatorChar = Energy.Base.Cast.StringToChar(separator);

            return Explode(line
                , separators
                , new char[] { '"' }
                , false
                , true
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, string separator, string enclosure)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char[] enclosures = string.IsNullOrEmpty(enclosure) ? null : enclosure.ToCharArray();

            return Explode(line
                , separators
                , enclosures
                , false
                , true
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, string separator, string enclosure, bool strip, bool white)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char[] enclosures = string.IsNullOrEmpty(enclosure) ? null : enclosure.ToCharArray();

            return Explode(line
                , separators
                , enclosures
                , strip
                , white
                , true
                , false
                );
        }

        /// <summary>
        /// Explode CSV line into array of values.
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="separator">Field separator characters (comma, semicolon, etc.)</param>
        /// <param name="enclosure">Text enclosure characters (quotation marks, apostrophes, etc.)</param>
        /// <param name="strip">Strip field value if it was quoted</param>
        /// <param name="white">Allow whitespace before value if not quoted</param>
        /// <param name="equals">Allow usage of equals sign (="01")</param>
        /// <param name="glue">Glue separators together (not supported)</param>
        /// <returns>Empty array if line was empty or null</returns>
        public static string[] Explode(string line, string separator, string enclosure, bool strip)
        {
            char[] separators = string.IsNullOrEmpty(separator) ? null : separator.ToCharArray();
            char[] enclosures = string.IsNullOrEmpty(enclosure) ? null : enclosure.ToCharArray();

            return Explode(line
                , separators
                , enclosures
                , strip
                , true
                , true
                , false
                );
        }

        #endregion

        #region Split

        /// <summary>
        /// Split CSV content into separate lines, including whitespace between quotation marks.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        public static string[] Split(string csv, char[] quote)
        {
            if (string.IsNullOrEmpty(csv))
                return new string[] { };

            List<string> list = new List<string>();
            foreach (string line in Each(csv, quote))
            {
                list.Add(line);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Split CSV content into separate lines, including whitespace between quotation marks.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="quotes"></param>
        /// <returns></returns>
        public static string[] Split(string csv, string quotes)
        {
            if (quotes == null)
                quotes = "";
            return Split(csv, quotes.ToCharArray());
        }

        /// <summary>
        /// Split CSV content into separate lines, including whitespace between quotation marks.
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        public static string[] Split(string csv)
        {
            return Split(csv, new char[] { '"' });
        }

        #endregion

        #region Each

        /// <summary>
        /// Exposes the enumerator, which supports a simple iteration over a collection of
        /// CSV lines, including whitespace between quotation marks.
        /// </summary>
        public static IEnumerable<string> Each(string csv, char[] quote)
        {
            int _e_ = quote == null ? 0 : quote.Length;
            char q = '\0';
            char l = '\0';
            char c = '\0';
            int p = 0;
            for (int i = 0; i < csv.Length; i++)
            {
                if (i > 0)
                    l = c;
                c = csv[i];
                if (q == '\0')
                {
                    for (int n = 0; n < quote.Length; n++)
                    {
                        if (c == quote[n])
                        {
                            q = quote[n];
                            continue;
                        }
                    }

                    if (c == '\n')
                    {
                        if (i == 0)
                        {
                            yield return "";
                            p++;
                            continue;
                        }
                        int x = i - 1;
                        if (l != '\r')
                            x++;
                        yield return csv.Substring(p, x - p);
                        p = i + 1;
                        continue;
                    }
                }
                else
                {
                    if (c == q)
                    {
                        q = '\0';
                        continue;
                    }
                }
            }
            if (p < csv.Length)
                yield return csv.Substring(p);
        }

        #endregion
    }
}
