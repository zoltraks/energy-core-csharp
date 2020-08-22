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
        /// Represents bag of parameters and their values.
        /// Use it to define parameters for parametrized query and to parse it.
        /// </summary>
        public class Bag
        {
            private Energy.Base.Collection.StringDictionary<object> _Values;
            /// <summary>Values</summary>
            public Energy.Base.Collection.StringDictionary<object> Values { get { return _Values; } }

            private Energy.Base.Collection.StringDictionary<Energy.Enumeration.FormatType> _Types;
            /// <summary>Types</summary>
            public Energy.Base.Collection.StringDictionary<Energy.Enumeration.FormatType> Types { get { return _Types; } }

            private readonly string patternVariable = @"
--[^\n]*\r?\n?
|
\[(?:\]\]|[^\]])*\]
|
'(?:''|[^'])*'
|
""(?:""""|[^""])*""
|
`(?:``|[^`])*`
|
(?<![\w0-9_])(?<name>@[\w@_][\w@_0-9]*)
";

            private Energy.Query.Format _Format;
            /// <summary>Format</summary>
            public Energy.Query.Format Format { get { return _Format; } set { _Format = value; } }

            #region Constructor

            public Bag()
            {
                _Values = new Energy.Base.Collection.StringDictionary<object>()
                {
                    CaseSensitive = false,
                };
                _Types = new Energy.Base.Collection.StringDictionary<Energy.Enumeration.FormatType>()
                {
                    CaseSensitive = false,
                };
            }

            #endregion

            #region Clear

            public Energy.Query.Parameter.Bag Clear()
            {
                _Values.Clear();
                _Types.Clear();
                return this;
            }

            #endregion

            #region Indexer

            public object this[string name]
            {
                get
                {
                    return GetValue(name);
                }
                set
                {
                    SetValue(name, value);
                }
            }

            #endregion

            #region Value

            public object GetValue(string name)
            {
                if (null == name)
                {
                    return null;
                }
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }
                return _Values[name];
            }

            public Energy.Query.Parameter.Bag SetValue(string name, object value)
            {
                if (null == name)
                {
                    return this;
                }
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }
                _Values[name] = value;
                return this;
            }

            #endregion

            #region Type

            public Energy.Enumeration.FormatType GetType(string name)
            {
                if (null == name)
                {
                    return Enumeration.FormatType.Text;
                }
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }
                if (!_Types.ContainsKey(name))
                {
                    return Enumeration.FormatType.Text;
                }
                return _Types[name];
            }

            public Energy.Query.Parameter.Bag SetType(string name, Energy.Enumeration.FormatType type)
            {
                if (null == name)
                {
                    return this;
                }
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }
                _Types[name] = type;
                return this;
            }

            #endregion

            #region Contains

            public bool Contains(string name)
            {
                if (null == name)
                {
                    return false;
                }
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }
                return _Values.ContainsKey(name);
            }

            #endregion

            #region Get

            public object Get(string name)
            {
                return GetValue(name);
            }

            #endregion

            #region Set

            public Energy.Query.Parameter.Bag Set(string name, object value, Enumeration.FormatType type)
            {
                SetValue(name, value);
                SetType(name, type);
                return this;
            }

            public Energy.Query.Parameter.Bag Set(string name, object value, string format)
            {
                Energy.Enumeration.FormatType type = Energy.Base.Cast.StringToEnum<Energy.Enumeration.FormatType>(format);
                SetValue(name, value);
                SetType(name, type);
                return this;
            }

            public Energy.Query.Parameter.Bag Set(string name, object value)
            {
                Energy.Enumeration.FormatType type = Energy.Enumeration.FormatType.Text;
                SetValue(name, value);
                SetType(name, type);
                return this;
            }

            #endregion

            #region Add

            public Energy.Query.Parameter.Bag Add(string name, object value, Enumeration.FormatType type)
            {
                SetValue(name, value);
                SetType(name, type);
                return this;
            }

            public Energy.Query.Parameter.Bag Add(string name, object value, string format)
            {
                Energy.Enumeration.FormatType type = Energy.Base.Cast.StringToEnum<Energy.Enumeration.FormatType>(format);
                SetValue(name, value);
                SetType(name, type);
                return this;
            }

            public Energy.Query.Parameter.Bag Add(string name, object value)
            {
                Energy.Enumeration.FormatType type = Energy.Enumeration.FormatType.Text;
                SetValue(name, value);
                SetType(name, type);
                return this;
            }

            #endregion

            #region Remove

            public Energy.Query.Parameter.Bag Remove(string name)
            {
                if (name == null)
                {
                    return this;
                }
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }
                _Values.Remove(name);
                _Types.Remove(name);
                return this;
            }

            #endregion

            #region Count

            public int Count
            {
                get
                {
                    return _Values.Count;
                }
            }

            #endregion

            ///// <summary>
            ///// Parameter format dictionary.
            ///// </summary>
            //public class TypeDictionary: Energy.Base.Collection.StringDictionary<Energy.Enumeration.FormatType>
            //{
            //    public TypeDictionary()
            //    {
            //        this.CaseSensitive = false;
            //    }

            //    /// <summary>
            //    /// Value representation format indexer.
            //    /// </summary>
            //    /// <param name="index"></param>
            //    /// <returns></returns>
            //    public new Energy.Enumeration.FormatType this[string index]
            //    {
            //        get
            //        {
            //            if (null == index)
            //            {
            //                return Enumeration.FormatType.Text;
            //            }
            //            if (!index.StartsWith("@"))
            //            {
            //                index = "@" + index;
            //            }
            //            if (!base.ContainsKey(index))
            //            {
            //                return Enumeration.FormatType.Text;
            //            }
            //            return base[index];
            //        }
            //        set
            //        {
            //            if (null == index)
            //            {
            //                return;
            //            }
            //            if (!index.StartsWith("@"))
            //            {
            //                index = "@" + index;
            //            }
            //            base[index] = value;
            //        }
            //    }
            //}

            //private Energy.Base.Bracket _Bracket;

            ///// <summary>Bracket</summary>
            //public Energy.Base.Bracket Bracket
            //{
            //    get
            //    {
            //        if (_Bracket == null)
            //        {
            //            _Bracket = new Energy.Base.Bracket("@", "", @"\d\w_@");
            //        }
            //        return _Bracket;
            //    }
            //}

            //public TypeDictionary Type = new TypeDictionary();

            private Option _Option;

            /// <summary>
            /// Parser options
            /// </summary>
            public Option Option { get { return _Option; } set { _Option = value; } }

            ///// <summary>
            ///// Parameter names must be explicit.
            ///// If set to false, parameters can be added in shorter form 
            ///// without leading @.
            ///// </summary>
            //public bool Explicit
            //{
            //    get
            //    {
            //        return (_Option & Option.Explicit) > 0;
            //    }
            //    set
            //    {
            //        if (value)
            //        {
            //            _Option |= Option.Explicit;
            //        }
            //        else
            //        {
            //            _Option &= ~Option.Explicit;
            //        }
            //    }
            //}

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
                    {
                        _Option |= Option.NullAsZero;
                    }
                    else
                    {
                        _Option &= ~Option.NullAsZero;
                    }
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
                    {
                        _Option |= Option.Unicode;
                    }
                    else
                    {
                        _Option &= ~Option.Unicode;
                    }
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

            #region Parse

            /// <summary>
            /// Parse parametrized query string.
            /// </summary>
            /// <param name="query"></param>
            /// <returns></returns>
            public string Parse(string query)
            {
                if (string.IsNullOrEmpty(query))
                {
                    return query;
                }

                bool optionUnicode = this.Unicode;
                bool optionUnknownAsEmpty = this.UnknownAsEmpty;
                bool optionUnknownAsNull = this.UnknownAsNull;
                bool optionNullAsZero = this.NullAsZero;

                bool allowUnknow = optionUnknownAsEmpty || optionUnknownAsNull;

                Energy.Query.Format format = _Format ?? Energy.Query.Format.Default;

                RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
                Regex regexVariable = new Regex(patternVariable, regexOptions);
                int Δ = 0;
                foreach (Match matchVariable in regexVariable.Matches(query))
                {
                    string name = matchVariable.Groups["name"].Value;
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    string value;

                    if (Contains(name))
                    {
                        object o = GetValue(name);
                        Energy.Enumeration.FormatType f = GetType(name);
                        if (optionNullAsZero && o == null)
                        {
                            o = 0;
                        }
                        value = format.Value(o, f, optionUnicode);
                    }
                    else if (!allowUnknow)
                    {
                        continue;
                    }
                    else if (optionUnknownAsEmpty)
                    {
                        value = format.Text("");
                    }
                    else if (optionNullAsZero)
                    {
                        value = "0";
                    }
                    else
                    {
                        value = "NULL";
                    }

                    int p = matchVariable.Index;
                    int l = matchVariable.Length;
                    query = ""
                        + (p + Δ > 0 ? query.Substring(0, p + Δ) : "")
                        + value
                        + query.Substring(p + l + Δ)
                        ;
                    Δ += value.Length - l;
                }
                return query;
            }

            #endregion

            #region ToString

            public override string ToString()
            {
                return this.ToString("=");
            }

            public string ToString(string glue)
            {
                Energy.Query.Format format = _Format ?? Energy.Query.Format.Default;
                bool unicode = this.Unicode;
                List<string> l = new List<string>();
                foreach (KeyValuePair<string, object> o in _Values)
                {
                    l.Add(o.Key + glue + format.Value(o.Value, GetType(o.Key), unicode));
                }
                return string.Join(Environment.NewLine, l.ToArray());
            }

            #endregion
        }

        #endregion

        #region List

        /// <summary>
        /// Represents list of parameters and their values.
        /// Use it to define parameters for parametrized query and to parse it.
        /// Alias for Energy.Query.Parameter.Bag class.
        /// </summary>
        public class List : Bag { }

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
                {
                    return template;
                }

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
