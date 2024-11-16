namespace Energy.Core
{
    /// <summary>
    /// Text editor class.
    /// </summary>
    public class Editor : Energy.Base.Pattern.GlobalDestroy<Editor>
    {
        /// <summary>
        /// Insert text before first line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public string InsertBeforeFirstLine(string text, string line)
        {
            return string.Concat(line, text);
        }

        /// <summary>
        /// Append text after first line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public string AppendAfterFirstLine(string text, string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return text;
            }
            if (string.IsNullOrEmpty(text))
            {
                if (text == null && line == null)
                {
                    return null;
                }
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

        /// <summary>
        /// Insert text before second line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public string InsertBeforeSecondLine(string text, string line)
        {
            return string.Concat(line, text);
        }

        /// <summary>
        /// Insert text before last line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public string InsertBeforeLastLine(string text, string line)
        {
            return string.Concat(line, text);
        }

        /// <summary>
        /// Append text after last line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public string AppendAfterLastLine(string text, string line)
        {
            return string.Concat(line, text);
        }

        public string GetFirstLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
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

        /// <summary>
        /// Get last line of text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetLastLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
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

        /// <summary>
        /// Ensure not empty text ends with new line marker.
        /// <br/><br/>
        /// Add new line to the end of text if not included already.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string EnsureNewLineAtEnd(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            string[] nll = Energy.Base.Text.NEWLINE_ARRAY;
            if (nll == null || nll.Length == 0)
            {
                nll = new string[] { Energy.Base.Text.NL };
            }
            foreach (string nl in nll)
            {
                if (text.EndsWith(nl))
                {
                    return text;
                }
            }
            text = string.Concat(text, nll[0]);
            return text;
        }

        /// <summary>
        /// Convert new line delimiter to specified one.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public string ConvertNewLine(string text, string newLine)
        {
            return Energy.Base.Text.ConvertNewLine(text, newLine);
        }

        /// <summary>
        /// Convert newline delimiter to environment default.
        /// Value of constant **Energy.Base.Text.NL** is used.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ConvertNewLine(string text)
        {
            return Energy.Base.Text.ConvertNewLine(text);
        }
    }
}
