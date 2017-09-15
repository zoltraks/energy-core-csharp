using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Quoting values with "brackets" like {...}, [...], {{...}}, "...".
    /// </summary>
    public class Bracket
    {
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
                _Text = null;
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
                _Text = null;
            }
        }

        private string _Text;
        /// <summary>Suffix</summary>
        public string Text
        {
            get
            {
                if (_Text == null)
                {
                    if (0 == string.Compare(_Prefix, _Suffix))
                        _Text = _Prefix;
                    string prefix = (_Prefix ?? "");
                    string suffix = (_Suffix ?? "");
                    int pad = prefix.Length;
                    if (pad < suffix.Length)
                        pad = suffix.Length;
                    if (prefix.Length < pad)
                        prefix = prefix.PadRight(pad, '\0');
                    if (suffix.Length < pad)
                        suffix = suffix.PadRight(pad, '\0');
                    _Text = string.Concat(prefix, suffix);
                }
                return _Text;
            }
            set
            {
                if (_Text != null && 0 == string.Compare(_Text, value))
                    return;
                if (value == null || value == string.Empty)
                {
                    _Text = _Prefix = _Suffix = "";
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
                    _Text = _Prefix = _Suffix;
                }
            }
        }
    }
}
