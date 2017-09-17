using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Quoting values with "brackets" like {...}, [...], {{...}}, "...".
    /// </summary>
    public class Bracket
    {
        private string _Prefix = "";
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
                _Enclosure = null;
                _MatchExpression = null;
            }
        }

        private string _Suffix = "";
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
                _Enclosure = null;
                _MatchExpression = null;
            }
        }

        private string _Enclosure = null;
        /// <summary>Suffix</summary>
        public string Enclosure
        {
            get
            {
                if (_Enclosure == null)
                {
                    if (0 == string.Compare(_Prefix, _Suffix))
                        _Enclosure = _Prefix;
                    string prefix = (_Prefix ?? "");
                    string suffix = (_Suffix ?? "");
                    int pad = prefix.Length;
                    if (pad < suffix.Length)
                        pad = suffix.Length;
                    if (prefix.Length < pad)
                        prefix = prefix.PadRight(pad, '\0');
                    if (suffix.Length < pad)
                        suffix = suffix.PadRight(pad, '\0');
                    _Enclosure = string.Concat(prefix, suffix);
                }
                return _Enclosure;
            }
            set
            {
                if (_Enclosure != null && 0 == string.Compare(_Enclosure, value))
                    return;
                _MatchExpression = null;
                if (value == null || value == string.Empty)
                {
                    _Enclosure = _Prefix = _Suffix = "";
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
                    _Enclosure = _Prefix = _Suffix = value;
                }
            }
        }

        private string _Include;
        /// <summary>
        /// Optionally include special sequence to include suffix
        /// instead of treating as end of quoted value.
        /// Set to empty string if you don't want to
        /// escape anything inside text in brackets.
        /// </summary>
        public string Include
        {
            get
            {
                if (_Include == null)
                {
                    if (string.IsNullOrEmpty(_Suffix))
                        return null;
                    return string.Concat(_Suffix, _Suffix);
                }
                return _Include;
            }
            set
            {
                if (0 == string.Compare(_Include, value))
                    return;
                _Include = value;
                _MatchExpression = null;
            }
        }

        private string _MatchExpression = null;

        public string MatchExpression
        {
            get
            {
                if (_MatchExpression == null)
                    _MatchExpression = GetMatchExpression();
                return _MatchExpression;
            }
        }

        /// <summary>
        /// Get match exception to search values in brackets.
        /// </summary>
        /// <returns></returns>
        public string GetMatchExpression()
        {            
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(_Prefix))
                list.Add(Energy.Base.Text.EscapeExpression(_Prefix));
            list.Add("(");
            string include = Include;
            if (!string.IsNullOrEmpty(include))
            {
                list.Add("(?:");
                list.Add(Energy.Base.Text.EscapeExpression(include));
                list.Add("|");
            }
            if (!string.IsNullOrEmpty(_Suffix))
            {
                list.Add("[^");
                list.Add(Energy.Base.Text.EscapeExpression(_Suffix[0].ToString()));
                list.Add("]");
            }
            if (!string.IsNullOrEmpty(include))
            {
                list.Add(")*");
            }
            else
            {
                list.Add("*");
            }
            list.Add(")");
            if (!string.IsNullOrEmpty(_Suffix))
                list.Add(Energy.Base.Text.EscapeExpression(_Suffix));
            _MatchExpression = string.Join("", list.ToArray());
            return _MatchExpression;
        }

        private Regex _MatchRegex = null;

        private Regex MatchRegex
        {
            get
            {
                if (_MatchRegex == null)
                    _MatchRegex = new Regex(MatchExpression);
                return _MatchRegex;
            }
        }

        public string[] FindAll(string input)
        {
            List<string> list = new List<string>();
            Regex r = MatchRegex;
            Match m = r.Match(input);
            while (m.Success)
            {
                string v = m.Groups[0].Value;
                list.Add(v);
                m = m.NextMatch();
            }
            return list.ToArray();
        }

        public IEnumerable<string> Find(string input)
        {
            List<string> list = new List<string>();
            Regex r = MatchRegex;
            Match m = r.Match(input);
            while (m.Success)
            {
                string v = m.Groups[0].Value;
                m = m.NextMatch();
                yield return v;
            }
        }
    }
}
