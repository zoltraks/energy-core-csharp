using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Core
{
    [Serializable]
    public class Syntax
    {
        private static Syntax _Default;

        private readonly static object _DefaultLock = new object();

        /// <summary>
        /// Single static instance of class (default)
        /// </summary>
        public static Syntax Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_DefaultLock)
                    {
                        if (_Default == null)
                        {
                            try
                            {
                                _Default = new Syntax();
                                _Default.OnReset += PopulateDefault;
                            }
                            catch (Exception exception)
                            {
                                Energy.Core.Bug.Catch(exception);
                            }
                        }
                    }
                }
                return _Default;
            }
        }

        /// <summary>
        /// Populate set of default variables according to standard
        /// </summary>
        /// <param name="syntax"></param>
        public static void PopulateDefault(Syntax syntax)
        {
            DateTime now = DateTime.Now;
            syntax["DATE"] = now.ToString("yyyy-MM-dd");
            syntax["TIME"] = now.ToString("HH:mm:ss");
            syntax["TIME_MS"] = now.ToString("HH:mm:ss.fff");
            syntax["TIME_US"] = now.ToString("HH:mm:ss.ffffff");
            syntax["STAMP"] = now.ToString("yyyy-MM-dd HH:mm:ss");
            syntax["STAMP_MS"] = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            syntax["STAMP_US"] = now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            syntax["YEAR"] = now.ToString("yyyy");
            syntax["YYYY"] = now.ToString("yyyy");
            syntax["YY"] = now.ToString("yy");
            syntax["MONTH"] = now.ToString("MM");
            syntax["MM"] = now.ToString("MM");
            syntax["DAY"] = now.ToString("DD");
            syntax["DD"] = now.ToString("DD");
            syntax["HOUR"] = now.ToString("HH");
            syntax["HOUR_12"] = now.ToString("hh");
            syntax["HOUR_24"] = now.ToString("HH");
            syntax["HH"] = now.ToString("HH");
            syntax["MINUTE"] = now.ToString("mm");
            syntax["II"] = now.ToString("mm");
            syntax["SECOND"] = now.ToString("ss");
            syntax["SS"] = now.ToString("ss");
            syntax["MILLISECOND"] = now.ToString("fff");
            syntax["MS"] = now.ToString("fff");
            syntax["MICROSECOND"] = now.ToString("fff");
            syntax["US"] = now.ToString("ffffff");
            syntax["AM"] = now.TimeOfDay < Energy.Base.Clock.Midday ? "AM" : "PM";
        }

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
