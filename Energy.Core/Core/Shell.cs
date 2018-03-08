using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Energy.Core
{
    /// <summary>
    /// Shell related classes including command line options
    /// </summary>
    public class Shell
    {        
        #region OptionStyle

        public class OptionStyle
        {
            /// <summary>
            /// DOS options like /? or /help.
            /// </summary>
            public bool Slash = true;

            /// <summary>
            /// Single dash options like -h or -help.
            /// </summary>
            public bool Single = true;

            /// <summary>
            /// POSIX / GNU options like --help for long option.
            /// </summary>
            public bool Double = true;

            /// <summary>
            /// Determines if colon may be used after option name to specify value, like /opt:value.
            /// </summary>
            public bool Colon = false;

            /// <summary>
            /// Determines if equal sign may be used after option name to specify value, like /opt=value.
            /// </summary>
            public bool Equal = false;

            /// <summary>
            /// Option without any prefix. Name should be considered "as is" in argument list.
            /// </summary>
            public bool Pure = false;

            /// <summary>
            /// Multiple elements for slash (/a/b/c) and short options (-abc).
            /// </summary>
            public bool Multiple = true;

            /// <summary>
            /// Allow value directly after short option like in "-pPassword".
            /// </summary>
            public bool Empty = false;

            /// <summary>
            /// Should single dash options be considered as short (single letter) options?
            /// </summary>
            public bool Short = true;

            private static OptionStyle _Default;
            private static readonly object _DefaultLock = new object();
            /// <summary>Singleton</summary>
            public static OptionStyle Default
            {
                get
                {
                    if (_Default == null)
                    {
                        lock (_DefaultLock)
                        {
                            if (_Default == null)
                            {
                                _Default = new OptionStyle();
                            }
                        }
                    }
                    return _Default;
                }
            }

            /// <summary>
            /// Check if parameter is an option with command line settings.
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns>True if is option, like "/opt"</returns>
            public bool IsOption(string parameter)
            {
                if (string.IsNullOrEmpty(parameter))
                    return false;
                if (this.Double && parameter.StartsWith("--"))
                    return parameter.Length > 2;
                if (this.Slash && parameter.StartsWith("/") || this.Single && parameter.StartsWith("-"))
                    return parameter.Length > 1;
                return false;
            }

            /// <summary>
            /// Check if parameter is an argument (not an option) with command line settings.
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns>True if is not an option, like "/opt"</returns>
            public bool IsArgument(string parameter)
            {
                return !IsOption(parameter);
            }
        }

        #endregion

        #region ArgumentList

        /// <summary>
        /// Argument list with recognized options from command line parameters.
        /// </summary>
        public class ArgumentList
        {
            /// <summary>
            /// List of options
            /// </summary>
            public List<string> Options = new List<string>();

            /// <summary>
            /// List of arguments
            /// </summary>
            public List<string> Arguments = new List<string>();

            /// <summary>
            /// List of option values if colon or equal sign used in option name like "/parameter=value"
            /// </summary>
            public Dictionary<string, string> Values = new Dictionary<string, string>();

            /// <summary>
            /// Create argument list from string array with specified command options style.
            /// </summary>
            /// <param name="args"></param>
            /// <param name="style"></param>
            /// <returns></returns>
            public static ArgumentList Create(string[] args, Energy.Core.Shell.OptionStyle style)
            {
                Energy.Base.Anonymous.Function<string, string> testDelA = delegate (string s) { return s; };
                Energy.Base.Anonymous.Function<string> testDelB = delegate (string s) { Console.WriteLine(s); };

                ArgumentList list = new Core.Shell.ArgumentList();
                List<string> o = new List<string>();
                for (int i = 0; i < args.Length; i++)
                {
                    string s = args[i];
                    if (false) { }
                    // Double dash
                    else if (style.Double && s.StartsWith("--"))
                    {
                        if (s.Length == 2)
                        {
                            list.Arguments.Add(s);
                            continue;
                        }
                        s = s.Substring(2);
                    }
                    // Slash
                    else if (style.Slash && s.StartsWith("/"))
                    {
                        if (s.Length == 1)
                        {
                            list.Arguments.Add(s);
                            continue;
                        }
                        s = s.Substring(1);
                        if (style.Multiple && s.Contains("/"))
                        {
                            string[] a = s.Split('/');
                            o.AddRange(a);
                        }
                        else
                        {
                            o.Add(s);
                        }
                    }
                    // Single dash
                    else if (style.Single && s.StartsWith("-"))
                    {
                        if (s.Length == 1)
                        {
                            list.Arguments.Add(s);
                            continue;
                        }
                        s = s.Substring(1);
                        if (s.Length == 1)
                        {
                            o.Add(s);
                        }
                        else if (!style.Multiple)
                        {
                            o.Add(s);
                        }
                        else if (style.Short)
                        {
                            foreach (char c in s.ToCharArray())
                            {
                                if (c == '-')
                                    continue;
                                o.Add(c.ToString());
                            }
                        }
                        else
                        {
                            string[] a = s.Split('-');
                            for (int n = 0; n < a.Length; n++)
                            {
                                o.Add(a[n]);
                            }
                        }
                    }
                    else
                    {
                        list.Arguments.Add(s);
                    }
                    for (int n = 0; n < o.Count; n++)
                    {
                        s = o[n];
                        if (false) { }
                        else if (style.Colon && s.Contains(":"))
                        {
                            int x = s.IndexOf(":");
                            string v = s.Substring(x + 1);
                            s = s.Substring(0, x - 1);
                            list.Values[s] = v;
                        }
                        else if (style.Equal && s.Contains("="))
                        {
                            int x = s.IndexOf("=");
                            string v = s.Substring(x + 1);
                            s = s.Substring(0, x - 1);
                            list.Values[s] = v;
                        }
                        list.Options.Add(s);
                    }
                    if (o.Count > 0)
                        o.Clear();
                }
                return list;
            }

            /// <summary>
            /// Create argument list from string array with default command options style.
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public static ArgumentList Create(string[] args)
            {
                return Create(args, Core.Shell.OptionStyle.Default);
            }
        }

        #endregion

        #region Parameter<TValue>

        public class Parameter<TValue>
        {
            public string Name;

            public string Short;

            public string Long;

            public string Description;

            public string Help;

            public string Example;

            public string[] Alternatives;

            public TValue Value;

            public TValue[] Array;

        }

        #endregion

        #region Parameter

        public class Parameter: Energy.Attribute.Command.OptionAttribute
        {
            public class List: System.Collections.Generic.List<Parameter>
            {
                public Parameter Find(string name)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].IsLongEqual(name))
                            return this[i];
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].IsNameEqual(name))
                            return this[i];
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].IsShortEqual(name))
                            return this[i];
                    }
                    return null;
                }

                public Parameter[] FindAll(string name)
                {
                    System.Collections.Generic.List<Parameter> list = new System.Collections.Generic.List<Parameter>();
                    for (int i = 0; i < Count; i++)
                    {
                        if (list.Contains(this[i]))
                            continue;
                        if (this[i].IsLongEqual(name))
                            list.Add(this[i]);
                    }                    
                    for (int i = 0; i < Count; i++)
                    {
                        if (list.Contains(this[i]))
                            continue;
                        if (this[i].IsNameEqual(name))
                            list.Add(this[i]);
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        if (list.Contains(this[i]))
                            continue;
                        if (this[i].IsShortEqual(name))
                            list.Add(this[i]);
                    }
                    return list.ToArray();
                }
            }

            private bool IsShortEqual(string name)
            {
                if (string.IsNullOrEmpty(this.Short))
                    return false;
                if (0 == string.Compare(name, this.Short))
                    return true;
                return false;
            }

            private bool IsLongEqual(string name)
            {
                if (string.IsNullOrEmpty(this.Long))
                    return false;
                if (0 == string.Compare(name, this.Long))
                    return true;
                return false;
            }

            private bool IsNameEqual(string name)
            {
                if (string.IsNullOrEmpty(this.Name))
                    return false;
                if (0 == string.Compare(name, this.Name))
                    return true;
                return false;
            }
        }

        #endregion
    }
}
