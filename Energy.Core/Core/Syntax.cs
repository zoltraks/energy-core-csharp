using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Core
{
    [Serializable]
    public class Syntax: Energy.Base.Pattern.DefaultProperty<Syntax>
    {
        private string _Text;
        /// <summary>Text</summary>
        public string Text { get { return _Text; } set { _Text = value; } }

        public event Energy.Base.Anonymous.Event<Syntax> OnReset;

        public void Reset()
        {
            Dictionary.Clear();
            if (OnReset != null)
                OnReset(this);
        }

        //private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

        public Energy.Base.Collection.StringDictionary<object> Dictionary;

        public Energy.Base.Bracket.Array Brackets; 
        
        #region Constructor

        public Syntax()
        {
            this.Dictionary = new Energy.Base.Collection.StringDictionary<object>()
            {
                CaseSensitive = false,
            };
            this.Brackets = new Base.Bracket.Array();
            this.Brackets.Add(new Energy.Base.Bracket()
            {
                Prefix = "{{",
                Suffix = "}}",
            });
        }

        public Syntax(string text)
            : this()
        {
            _Text = text;
        }

        #endregion

        public object this[string key]
        {
            get
            {
                return Dictionary[key];
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        public override string ToString()
        {
            return Parse(_Text);
        }

        public string Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder builder = new StringBuilder();

            string pattern = Brackets.GetMatchExpression();
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(text);
            if (matches.Count == 0)
                return text;
            int p = 0;
            foreach (Match match in matches)
            {
                if (match.Index > p)
                {
                    builder.Append(text.Substring(p, match.Index - p));
                }
                p = match.Index + match.Length;
            }
            return builder.ToString();
        }

        private string Interpolate(string expression)
        {
            return expression;
        }
    }
}
