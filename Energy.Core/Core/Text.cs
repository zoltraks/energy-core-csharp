using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    public class Text
    {
        #region Editor

        public class Editor : Energy.Base.Pattern.DefaultProperty<Editor>
        {
            public string InsertBeforeFirstLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string AppendAfterFirstLine(string text, string line)
            {
                if (string.IsNullOrEmpty(line))
                    return text;
                if (string.IsNullOrEmpty(text))
                {
                    if (text == null && line == null)
                        return null;
                }
                int p = Energy.Base.Text.IndexOfAny(text, Energy.Base.Text.NEWLINE_ARRAY);
                if (p < 0)
                {
                    return string.Concat(text ?? "", line ?? "");
                }
                else
                {
                    string result = ""
                        + text.Substring(0, p) + line + text.Substring(p)
                        ;
                    return result;
                }
            }

            public string InsertBeforeSecondLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string InsertBeforeLastLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string AppendAfterLastLine(string message, string line)
            {
                return string.Concat(line, message);
            }

            public string GetFirstLine(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                int p = Energy.Base.Text.IndexOfAny(text, Energy.Base.Text.NEWLINE_ARRAY);
                if (p < 0)
                {
                    return text;
                }
                else
                {
                    return text.Substring(0, p);
                }
            }

            public string GetLastLine(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                int p = Energy.Base.Text.AfterOfAny(text, Energy.Base.Text.NEWLINE_ARRAY);
                if (p < 0)
                {
                    return text;
                }
                else
                {
                    return text.Substring(p);
                }
            }

            public string EnsureNewLineAtEnd(string text)
            {
                string[] nll = Energy.Base.Text.NEWLINE_ARRAY;
                if (nll == null || nll.Length == 0)
                {
                    nll = new string[] { Environment.NewLine };
                }
                if (string.IsNullOrEmpty(text))
                {
                    text = nll[0];
                }
                else
                {
                    foreach (string nl in nll)
                    {
                        if (text.EndsWith(nl))
                            return text;
                    }
                    text = string.Concat(text, nll[0]);
                }
                return text;
            }

            public string ConvertNewLine(string text, string newLine)
            {
                string pattern = @"\r\n|\r|\n";
                return System.Text.RegularExpressions.Regex.Replace(text, pattern, newLine);
            }

            public string ConvertNewLine(string text)
            {
                return ConvertNewLine(text, "\r\n");
            }
        }

        #endregion
    }
}
