﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Regular expression pattern constants
    /// </summary>
    public static partial class Expression
    {
        #region Constant

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
        public static readonly string Time = @"(((?<hour>\d{1,2}):)?(?<minute>\d{1,2}):)?(?<second>\d{1,2})[,\.](?<fraction>\d{1,6})|((?<hour>\d{1,2}):(?<minute>\d{1,2}))(:(?<second>\d{1,2}))?";

        /// <summary>
        /// URI pattern (scheme + user + password + host + port + path + query + fragment)
        /// </summary>
        public static readonly string Url = @"(?:(?<scheme>\w+):)?(?://)?(?:(?<user>[^:@\r\n]*)(?::(?<password>[^:@\r\n]*))?@)?(?<host>[\w\d_\-\.=]+)(?:\:(?<port>\d+))?(?<path>/[^?\r\n]*)?(?:\?(?<query>[^#\r\n]*))?(?:\#(?<fragment>[^\r\n]*))?";

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

        /// <summary>
        /// Expression for finding root element in XML
        /// </summary>
        public static readonly string XmlRootName = @"(?:<\?[xX][mM][lL][^>]*>\s*)?(?:<\s*([a-zA-Z_][^\s>]*))";

        /// <summary>
        /// Expression for matching tilde color text
        /// </summary>
        public static readonly string TildeText = @"~`(?:``|[^`])*`~|~\d+~|~[\w\d]+~|~+?|[^~]+";

        #endregion

        #region Utility

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
    }
}
