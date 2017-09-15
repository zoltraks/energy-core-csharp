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
        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                if (value == null)
                    value = "";
                if (0 == string.Compare(_Prefix, value))
                    return;
                _Prefix = value;
                _Bracket = null;
            }
        }

        private string _Suffix;
        /// <summary>Suffix</summary>
        public string Suffix
        {
            get
            {
                return _Suffix;
            }
            set
            {
                if (value == null)
                    value = "";
                if (0 == string.Compare(_Suffix, value))
                    return;
                _Suffix = value;
                _Bracket = null;
            }
        }

        private string _Bracket;
        /// <summary>Suffix</summary>
        public string Bracket
        {
            get
            {
                if (_Bracket == null)
                {
                    if (0 == string.Compare(_Prefix, _Suffix))
                        _Bracket = _Prefix;
                    string prefix = (_Prefix ?? "");
                    string suffix = (_Suffix ?? "");
                    int pad = prefix.Length;
                    if (pad < suffix.Length)
                        pad = suffix.Length;
                    if (prefix.Length < pad)
                        prefix = prefix.PadRight(pad, '\0');
                    if (suffix.Length < pad)
                        suffix = suffix.PadRight(pad, '\0');                    
                    _Bracket = string.Concat(prefix, suffix);
                }
                return _Bracket;
            }
            set
            {
                if (_Bracket != null && 0 == string.Compare(_Bracket, value))
                    return;
                if (value == null || value == string.Empty)
                {
                    _Prefix = _Suffix = "";
                    return;
                }
                bool even = 0 == value.Length % 2;
                if (even)
                {
                    int m = value.Length / 2;
                    string prefix = value.Substring(0, m);
                    string suffix = value.Substring(m, m);
                }
                else
                {
                    _Bracket = _Prefix = _Suffix;
                }
            }
        }

        #region Constructor

        public Syntax()
        {
            this.CaseSensitive = false;
            this.Prefix = "{{";
            this.Suffix = "}}";
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
