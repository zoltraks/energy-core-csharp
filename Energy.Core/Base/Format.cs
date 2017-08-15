using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Various formatting functions
    /// </summary>
    public class Format
    {
        #region Text quotation format

        public class Quote
        {
            private string prefix;
            public string Prefix
            {
                get
                {
                    return prefix;
                }
                set
                {
                    prefix = value;
                    with = value == null ? null : value + value;
                }
            }

            private string suffix;
            public string Suffix
            {
                get
                {
                    return suffix;
                }
                set
                {
                    suffix = value;
                }
            }

            public string Single
            {
                get
                {
                    return prefix == suffix ? prefix : null;
                }
                set
                {
                    prefix = value;
                    suffix = value;
                    with = value == null ? null : value + value;
                }
            }

            private string with;

            public string Bracket
            {
                get
                {
                    return string.Concat(prefix, suffix);
                }
                set
                {
                    if (value.Length % 2 == 0)
                    {
                        Prefix = value.Substring(0, value.Length / 2);
                        Suffix = value.Substring(value.Length);
                    }
                    else
                    {
                        Single = value;
                    }
                }
            }

            public static implicit operator Quote(string value)
            {
                Quote quote = new Quote();
                quote.Bracket = value;
                return quote;
            }

            public string Escape(string content)
            {
                if (content.Contains(prefix))
                {
                    return content.Replace(prefix, with);
                }
                else
                {
                    return content;
                }
            }

            public string Unescape(string content)
            {
                return content.Replace(with, prefix);
            }

            public string Surround(string content)
            {
                return String.Concat(prefix, Escape(content), suffix);
            }

            public string Lose(string content)
            {
                if (string.IsNullOrEmpty(content))
                    return content;
                if (!string.IsNullOrEmpty(prefix) && !content.StartsWith(prefix))
                    return content;
                return Unescape(content.Substring((prefix ?? "").Length, content.Length - (prefix ?? "").Length - (suffix ?? "").Length));
            }
        }

        #endregion
    }
}