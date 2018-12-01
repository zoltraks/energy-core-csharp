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
        #region Array

        public class Array : Energy.Base.Collection.Array<Bracket>
        {
            public string GetMatchExpression()
            {
                Energy.Base.Bracket[] array = this.ToArray();
                if (array.Length == 0)
                    return "";
                if (array.Length == 1)
                    return array[0].MatchExpression;
                List<string> list = new List<string>();
                foreach (Energy.Base.Bracket bracket in array)
                {
                    list.Add(bracket.MatchExpression);
                }
                string pattern = string.Join("|", list.ToArray());
                return pattern;
            }
        }

        #endregion

        #region Constructor

        public Bracket() { }

        public Bracket(string enclosure)
        {
            Enclosure = enclosure;
        }

        public Bracket(string prefix, string suffix)
        {
            _Prefix = prefix;
            _Suffix = suffix;
        }

        public Bracket(string prefix, string suffix, string characterClass)
        {
            _Prefix = prefix;
            _Suffix = suffix;
            _CharacterClass = characterClass;
        }

        public static implicit operator Bracket(string enclosure)
        {
            return new Bracket(enclosure);
        }

        #endregion

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
                MatchExpression = null;
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
                MatchExpression = null;
            }
        }

        private string _Enclosure = null;
        /// <summary>Enclosure</summary>
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
                MatchExpression = null;
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
                    _Prefix = prefix;
                    _Suffix = suffix;
                    _Enclosure = value;
                }
                else
                {
                    _Enclosure = _Prefix = _Suffix = value;
                }
            }
        }

        private string _CharacterClass = @"^\s";
        /// <summary>
        /// Character class for single prefixed or suffixed values. 
        /// Default is everything but not whitespace.
        /// </summary>
        public string CharacterClass
        {
            get
            {
                return _CharacterClass;
            }
            set
            {
                if (0 == string.Compare(value, _CharacterClass))
                    return;
                _CharacterClass = value;
                MatchExpression = null;
            }
        }

        private string _Include;
        /// <summary>
        /// Optionally specify special sequence to include suffix
        /// instead of treating as end of quoted value.
        /// Set to empty string if you don't want to
        /// escape anything inside text in brackets.
        /// Set to null if you want to use default double suffix.
        /// </summary>
        public string Include
        {
            get
            {
                return _Include;
            }
            set
            {
                if (0 == string.Compare(_Include, value))
                    return;
                _Include = value;
                MatchExpression = null;
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
            set
            {
                if (0 == string.Compare(value, _MatchExpression))
                    return;
                _MatchExpression = value;
                _MatchRegex = null;
            }
        }

        /// <summary>
        /// Get match exception to search values in brackets.
        /// </summary>
        /// <returns></returns>
        public string GetMatchExpression()
        {
            List<string> list = new List<string>();
            bool allowEmpty = true;
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
            else
            {
                list.Add("[");
                list.Add(_CharacterClass);
                list.Add("]");
                allowEmpty = false;
            }
            if (!string.IsNullOrEmpty(include))
            {
                list.Add(")");
            }
            list.Add(allowEmpty ? "*" : "+");
            //list.Add("?");
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

        /// <summary>
        /// Find all values in brackets.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string[] FindArray(string input)
        {
            return new List<string>(Find(input)).ToArray();
        }

        /// <summary>
        /// Find all values in brackets.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find all values in brackets.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<SearchResult> Search(string input)
        {
            List<string> list = new List<string>();
            Regex r = MatchRegex;
            Match m = r.Match(input);
            while (m.Success)
            {
                string v = m.Groups[0].Value;
                int i = m.Index;
                m = m.NextMatch();
                yield return new SearchResult()
                {
                    Position = i,
                    Length = v.Length,
                    Value = v,
                };
            }
        }

        public override string ToString()
        {
            return string.Concat(Enclosure);
        }

        public class SearchResult
        {
            /// <summary>
            /// Position in searched text
            /// </summary>
            public int Position;

            /// <summary>
            /// Length of found text
            /// </summary>
            public int Length;

            /// <summary>
            /// Search value
            /// </summary>
            public string Value;

            public override string ToString()
            {
                return Value;
            }
        }

        #region GetPrefixText

        /// <summary>
        /// Get prefix text selecting from single enclosure or empty string if null.
        /// </summary>
        /// <returns></returns>
        public string GetPrefixText()
        {
            if (_Prefix != null)
                return _Prefix;
            else
                return "";
        }

        #endregion

        #region GetSuffixText

        /// <summary>
        /// Get suffix text selecting from single enclosure or empty string if null.
        /// </summary>
        /// <returns></returns>
        private string GetSuffixText()
        {
            if (_Suffix != null)
                return _Suffix;
            else
                return "";
        }

        #endregion

        #region GetIncludeText

        /// <summary>
        /// Get include text selecting from single enclosure or empty string if null.
        /// </summary>
        /// <returns></returns>
        private string GetIncludeText()
        {
            if (_Include == null)
            {
                if (string.IsNullOrEmpty(_Suffix))
                    return "";
                return string.Concat(_Suffix, _Suffix);
            }
            return _Include;
        }

        #endregion

        #region Quote

        /// <summary>
        /// Quote text using bracket settings.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Quote(string text)
        {
            string prefix = GetPrefixText();
            string suffix = GetSuffixText();
            string include = GetIncludeText();

            if (include.Length > 0)
                text = text.Replace(suffix, include);

            return string.Concat(prefix, text, suffix);
        }

        #endregion
    }
}
