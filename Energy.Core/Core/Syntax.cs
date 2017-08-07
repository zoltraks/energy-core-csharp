using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    [Serializable]
    public class Syntax: Energy.Base.Collection.StringDictionary
    {
        private string _Text;
        /// <summary>Text</summary>
        public string Text { get { return _Text; } set { _Text = value; } }

        private string _Prefix;
        /// <summary>Prefix</summary>
        public string Prefix { get { return _Prefix; } set { _Prefix = value; } }

        private string _Suffix;
        /// <summary>Suffix</summary>
        public string Suffix { get { return _Suffix; } set { _Suffix = value; } }

        #region Constructor

        public Syntax()
        {
            this.CaseSensitive = false;
        }

        public Syntax(string text)
        {
            _Text = text;
        }

        #endregion

        public override string ToString()
        {
            return Parse(_Text);
        }

        public string Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder builder = new StringBuilder();

            int last = 0;
            bool quote = false;
            int position = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (position < 0)
                {
                    if (text.Length - i >= Prefix.Length && text.Substring(i, Prefix.Length).Equals(Prefix))
                    {
                        if (i - last > 0)
                        {
                            builder.Append(text.Substring(last, i - last));
                        }
                        position = i + Prefix.Length;
                        i += Prefix.Length - 1;
                    }
                }
                else
                {
                    if (quote)
                    {
                        if (text[i] == '"')
                        {
                            if (text.Length - i >= 2 && text.Substring(i, 2).Equals("\"\""))
                            {
                                i++;
                            }
                            else
                            {
                                quote = false;
                            }
                        }
                    }
                    else if (text[i] == '"')
                    {
                        quote = true;
                    }
                    else if (text.Length - i >= Suffix.Length && text.Substring(i, Suffix.Length).Equals(Suffix))
                    {
                        builder.Append(Interpolate(text.Substring(position, i - position)));
                        position = -1;
                        i += Suffix.Length - 1;
                        last = i + 1;
                    }
                }
            }

            if (position < 0 && last < text.Length)
            {
                builder.Append(text.Substring(last, text.Length - last));
            }

            return builder.ToString();
        }

        private string Interpolate(string expression)
        {
            return expression;
        }
    }
}
