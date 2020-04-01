using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Query
{
    /// <summary>
    /// Support class for parametrized queries.
    /// </summary>
    public class Parameter
    {
        #region Bag

        /// <summary>
        /// Represents list of parameters and their values.
        /// Use it to define parameters for parametrized query and to parse it.
        /// </summary>
        public class Bag : List { }

        #endregion

        #region List

        /// <summary>
        /// Represents list of parameters and their values.
        /// Use it to define parameters for parametrized query and to parse it.
        /// </summary>
        public class List : Energy.Base.Collection.StringDictionary<object>
        {
            private Energy.Query.Format _Format;
            /// <summary>Format</summary>
            public Energy.Query.Format Format { get { return _Format; } set { _Format = value; } }

            #region Constructor

            public List()
            {
                this.CaseSensitive = false;
            }

            #endregion

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
            /// If set to false, parameters can be added in shorter form 
            /// without leading @.
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
            /// Use N prefix for all non empty texts (Unicode).
            /// </summary>
            public bool Unicode
            {
                get
                {
                    return (Option & Option.Unicode) > 0;
                }
                set
                {
                    if (value)
                        _Option |= Option.Unicode;
                    else
                        _Option &= ~Option.Unicode;
                }
            }

            /// <summary>
            /// Parse unknown parameters as empty texts.
            /// Does not apply to parameters with names with leading @@ (double at sign).
            /// </summary>
            public bool UnknownAsEmpty
            {
                get
                {
                    return (_Option & Option.UnknownAsEmpty) > 0;
                }
                set
                {
                    if (value)
                    {
                        _Option |= Option.UnknownAsEmpty;
                        _Option &= ~Option.UnknownAsNull;
                    }
                    else
                    {
                        _Option &= ~Option.UnknownAsEmpty;
                    }
                }
            }

            /// <summary>
            /// Parse unknown parameters as NULL.
            /// Does not apply to parameters with names with leading @@ (double at sign).
            /// </summary>
            public bool UnknownAsNull
            {
                get
                {
                    return (_Option & Option.UnknownAsNull) > 0;
                }
                set
                {
                    if (value)
                    {
                        _Option |= Option.UnknownAsNull;
                        _Option &= ~Option.UnknownAsEmpty;
                    }
                    else
                    {
                        _Option &= ~Option.UnknownAsNull;
                    }
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
                {
                    return input;
                }

                bool optionUnicode = this.Unicode;
                bool optionUnknownAsEmpty = this.UnknownAsEmpty;
                bool optionUnknownAsNull = this.UnknownAsNull;
                bool optionNullAsZero = this.NullAsZero;

                bool allowUnknow = optionUnknownAsEmpty || optionUnknownAsNull;

                Energy.Query.Format format = _Format;
                if (_Format == null)
                {
                    format = Energy.Query.Format.Default;
                }
                int δ = 0;
                foreach (Energy.Base.Bracket.SearchResult _ in Bracket.Search(input))
                {
                    string variable = _.Value;
                    bool found = this.ContainsKey(variable);

                    if (!found)
                    {
                        if (true
                            && !Explicit
                            && variable.Length > 1
                            && variable.StartsWith("@")
                            && variable[1] != '@'
                        )
                        {
                            variable = variable.Substring(1);
                            found = this.ContainsKey(variable);
                        }
                    }

                    if (!found)
                    {
                        if (!allowUnknow)
                        {
                            continue;
                        }
                        else
                        {
                            if (variable.StartsWith("@@"))
                            {
                                continue;
                            }
                        }
                    }

                    object value = null;
                    Energy.Enumeration.FormatType type;
                    if (found)
                    {
                        value = this[variable];
                        type = Type[variable];
                    }
                    else
                    {
                        type = Energy.Enumeration.FormatType.Text;
                        value = optionUnknownAsEmpty ? "" : null;
                    }

                    string text = null;
                    switch (type)
                    {
                        default:
                        case Energy.Enumeration.FormatType.Text:
                            if (optionUnicode)
                            {
                                text = format.Unicode(value);
                            }
                            else
                            {
                                text = format.Text(value);
                            }
                            break;

                        case Enumeration.FormatType.Plain:
                            text = Energy.Base.Cast.ObjectToString(value);
                            break;

                        case Energy.Enumeration.FormatType.Number:
                            text = format.Number(value, !optionNullAsZero);
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

                        case Enumeration.FormatType.Binary:
                            text = format.Binary(value);
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

            public void Set(string key, object value, Enumeration.FormatType format)
            {
                base.Set(key, value);
                this.Type[key] = format;
            }

            public void Set(string key, object value, string format)
            {
                Set(key, value, (Enumeration.FormatType)Energy.Base.Cast.StringToEnum(format, typeof(Enumeration.FormatType)));
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
            /// <summary>
            /// Parameters must be explicitly defined.
            /// </summary>
            Explicit = 1,

            /// <summary>
            /// Parse null values as numeric zero.
            /// </summary>
            NullAsZero = 2,

            /// <summary>
            /// Use N prefix for all non empty texts (Unicode).
            /// </summary>
            Unicode = 8,

            /// <summary>
            /// Parse unknown parameters as empty texts.
            /// Does not apply to parameters with names with leading @@ (double at sign).
            /// </summary>
            UnknownAsEmpty = 16,

            /// <summary>
            /// Parse unknown parameters as NULL.
            /// Does not apply to parameters with names with leading @@ (double at sign).
            /// </summary>
            UnknownAsNull = 32,
        }

        #endregion

        #region Template

        /// <summary>
        /// Support for query templates.
        /// </summary>
        public class Template
        {
            #region Static

            /// <summary>
            /// Convert SQL query template which uses angle brackets
            /// to parameterized query which uses at sign to define parameters.
            /// </summary>
            /// <param name="template"></param>
            /// <returns></returns>
            public static string ConvertToParameterizedQuery(string template)
            {
                if (string.IsNullOrEmpty(template))
                    return template;

                StringBuilder sb = new StringBuilder();

                string pattern = @"<([^\r\n,>]+)(?:\s*,\s*[^\r\n\s,>]*)*>";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(template);

                int p = 0;

                while (match.Success)
                {
                    int index = match.Index;
                    int length = match.Length;
                    if (index > p)
                    {
                        sb.Append(template.Substring(p, index - p));
                    }
                    p = index + length;
                    string variable = match.Groups[1].Value;
                    if (variable.Length > 0)
                    {
                        variable = variable.Trim();
                        variable = Energy.Base.Text.ReplaceWhite(variable, "_");
                        sb.Append("@");
                        sb.Append(variable);
                    }
                    match = match.NextMatch();
                }

                if (p < template.Length)
                {
                    sb.Append(template.Substring(p));
                }

                return sb.ToString();
            }

            #endregion
        }

        #endregion
    }
}
