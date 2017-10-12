using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query
{
    /// <summary>
    /// Support for parametrized queries.
    /// </summary>
    public class Parameter
    {
        #region Bag

        public class Bag : List { }

        #endregion

        #region List

        /// <summary>
        /// Parameter bag.
        /// Use it to define parameters for parametrized query and to parse it.
        /// </summary>
        public class List : Energy.Base.Collection.StringDictionary<object>
        {
            private Energy.Query.Format _Format;
            /// <summary>Format</summary>
            public Energy.Query.Format Format { get { return _Format; } set { _Format = value; } }

            public List()
            {
                this.CaseSensitive = false;
            }

            /// <summary>
            /// Parameter format dictionary.
            /// </summary>
            public class TypeDictionary: Energy.Base.Collection.StringDictionary<Energy.Enumeration.FormatType>
            {
                public TypeDictionary()
                {
                    this.CaseSensitive = false;
                }

                /// <summary>
                /// Value representation format indexer.
                /// </summary>
                /// <param name="index"></param>
                /// <returns></returns>
                public new Energy.Enumeration.FormatType this[string index]
                {
                    get
                    {
                        if (!base.ContainsKey(index))
                            return Enumeration.FormatType.Text;
                        return base[index];
                    }
                    set
                    {
                        base[index] = value;
                    }
                }
            }

            private Energy.Base.Bracket _Bracket;

            /// <summary>Bracket</summary>
            public Energy.Base.Bracket Bracket
            {
                get
                {
                    if (_Bracket == null)
                    {
                        _Bracket = new Energy.Base.Bracket("@", "", @"\d\w_@");
                    }
                    return _Bracket;
                }
            }

            public TypeDictionary Type = new TypeDictionary();

            private Option _Option;

            /// <summary>
            /// Parser options
            /// </summary>
            public Option Option { get { return _Option; } set { _Option = value; } }

            /// <summary>
            /// Parameter names must be explicit.
            /// If set to false, parameters with single at sign (@var)
            /// can be defined in shorter form ("var").
            /// </summary>
            public bool Explicit
            {
                get
                {
                    return (_Option & Option.Explicit) > 0;
                }
                set
                {
                    if (value)
                        _Option |= Option.Explicit;
                    else
                        _Option &= ~Option.Explicit;
                }
            }

            /// <summary>
            /// Parse null values as zero when converting to number type.
            /// </summary>
            public bool NullAsZero
            {
                get
                {
                    return (_Option & Option.NullAsZero) > 0;
                }
                set
                {
                    if (value)
                        _Option |= Option.NullAsZero;
                    else
                        _Option &= ~Option.NullAsZero;
                }
            }

            /// <summary>
            /// Parse parametrized query string.
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public string Parse(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return input;
                //Energy.Source.Query.Script dialect = _Dialect ?? Energy.Srouce.Dialect.Default;
                //Energy.Base.Format.Quote = dialect.Quote;
                Energy.Query.Format format = _Format;
                if (_Format == null)
                    format = Energy.Query.Format.Default;
                int δ = 0;
                foreach (Energy.Base.Bracket.SearchResult _ in Bracket.Search(input))
                {
                    string variable = _.Value;
                    if (!this.ContainsKey(variable))
                    {
                        if (!Explicit
                            && variable.Length > 1
                            && variable.StartsWith("@")
                            && variable[1] != '@')
                        {
                            variable = variable.Substring(1);
                            if (!this.ContainsKey(variable))
                                continue;
                        }
                        else
                            continue;
                    }
                    object value = this[variable];
                    string text = null;
                    switch (Type[variable])
                    {
                        default:
                        case Energy.Enumeration.FormatType.Text:
                            text = format.Text(value);
                            break;

                        case Energy.Enumeration.FormatType.Number:
                            text = format.Number(value, !NullAsZero);
                            break;

                        case Energy.Enumeration.FormatType.Date:
                            text = format.Date(value);
                            break;

                        case Energy.Enumeration.FormatType.Time:
                            text = format.Time(value);
                            break;

                        case Energy.Enumeration.FormatType.Stamp:
                            text = format.Stamp(value);
                            break;
                    }
                    int n = δ + _.Position;
                    string s1 = input.Substring(0, n);
                    n+= _.Length;
                    string s2 = input.Substring(n);
                    input = string.Concat(s1, text, s2);
                    δ += text.Length - _.Length;
                }
                return input;
            }

            public void Set(string key, string value, Enumeration.FormatType format)
            {
                base.Set(key, value);
                this.Type[key] = format;
            }
        }

        #endregion

        #region Option

        /// <summary>
        /// Parameter option
        /// </summary>
        [Flags]
        public enum Option
        {
            Explicit = 1,

            NullAsZero = 2,

            ZeroAsNull = 4,
        }

        #endregion
    }
}
