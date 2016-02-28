using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Energy.Query
{
    public class Format
    {
        public string Text(string value)
        {
            if (LiteralQuote.Prefix == null && LiteralQuote.Suffix == null)
            {
                return value;
            }
            if (LiteralQuote.Prefix != null && value.Contains(LiteralQuote.Prefix))
            {
                return LiteralQuote.Prefix + value.Replace(LiteralQuote.Prefix, LiteralQuote.Prefix + LiteralQuote.Prefix) + LiteralQuote.Suffix;
            }
            return LiteralQuote.Prefix + value + LiteralQuote.Suffix;
        }

        public struct Quote
        {
            public string Prefix;
            public string Suffix;

            public string Bracket
            {
                set
                {
                    if (value.Length % 2 == 0)
                    {
                        Prefix = value.Substring(0, value.Length / 2);
                        Suffix = value.Substring(value.Length);
                    }
                    else
                    {
                        Prefix = Suffix = value;
                    }
                }
            }

            public string Escape(string content)
            {
                if (content.Contains(Prefix))
                {
                    return content.Replace(Prefix, Prefix + Prefix);
                }
                else
                {
                    return content;
                }
            }

            public string Unescape(string content)
            {
                return content.Replace(Prefix + Prefix, Prefix);
            }
        }

        public Quote LiteralQuote; // 'column'
        public Quote ObjectQuote; // "value"

        public class ANSI : Format
        {
            public ANSI()
            {
                ObjectQuote.Prefix = ObjectQuote.Suffix = "`";
            }
        }

        public class MySQL : Format
        {
            public MySQL()
            {
                LiteralQuote.Bracket = "'";
                ObjectQuote.Bracket = ObjectQuote.Suffix = "`";
            }
        }

        public class SQLServer : Format
        {
            public SQLServer()
            {
                ObjectQuote.Prefix = "[";
                ObjectQuote.Suffix = "]";
            }
        }

        public class SQLite : Format
        {
            public SQLite()
            {
                ObjectQuote.Prefix = ObjectQuote.Suffix = "'";
            }
        }
    }
}
