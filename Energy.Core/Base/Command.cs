﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Command line helper class
    /// </summary>
    public static class Command
    {
        #region Arguments

        /// <summary>
        /// Command line arguments utility class.
        /// </summary>
        public class Arguments: Energy.Base.Pattern.Builder<Arguments>
        {
            #region Constructor

            public Arguments(string[] args)
            {
                if (null != args)
                {
                    _List.AddRange(args);
                }
            }

            public Arguments(string line)
            {
                this.Line(line);
            }

            public Arguments()
            {

            }

            #endregion

            #region Private

            private List<string> _List = new List<string>();

            private Option.Array _Option = new Option.Array();

            private List<string> _Heap = new List<string>();

            private int _Skip = 0;

            private bool _Slash = false;

            private bool _Short = true;

            private bool _Long = true;

            private bool _Strict = false;

            private bool _UnknownAsParameter = false;

            #endregion

            #region Accessor

            public string this[int index]
            {
                get
                {
                    if (0 > index || null == _Heap || index >= _Heap.Count)
                    {
                        return null;
                    }
                    else
                    {
                        return _Heap[index];
                    }
                }
            }

            public Option this[string key]
            {
                get
                {
                    if (null == _Option)
                    {
                        return null;
                    }
                    else
                    {
                        return _Option.Find(key);
                    }
                }
            }

            #endregion

            #region Property

            ///// <summary>
            ///// Represents array of parameters (not options) taken from command line.
            ///// </summary>
            //public List<string> Items { get => _List; }

            /// <summary>
            /// Represents state of command line options.
            /// </summary>
            public Option.Array Options { get => _Option; }

            /// <summary>
            /// Number of non option elements from command line argument list.
            /// </summary>
            public int Count { get => _Heap.Count; }

            #endregion

            #region Parameter

            /// <summary>
            /// Add command line parameter that will consume one
            /// or more trailing arguments and provide them as values.
            /// <br/><br/>
            /// If you set count to 0, it will become switch (flag).
            /// </summary>
            /// <param name="name"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public Arguments Parameter(string name, int count)
            {
                Option opt = new Option()
                {
                    Name = name,
                    Count = count,
                };
                _Option.Add(opt);
                return this;
            }

            /// <summary>
            /// Add command line parameter that will consume one
            /// next trailing argument and provide it as value.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Arguments Parameter(string name)
            {
                return Parameter(name, 1);
            }

            /// <summary>
            /// Treat all unknown options as parametered.
            /// </summary>
            /// <returns></returns>
            public Arguments Parameter()
            {
                _UnknownAsParameter = true;
                return this;
            }

            /// <summary>
            /// Treat all unknown options as parametered or not.
            /// </summary>
            /// <returns></returns>
            public Arguments Parameter(bool parameter)
            {
                _UnknownAsParameter = parameter;
                return this;
            }

            #endregion

            #region Switch

            /// <summary>
            /// Add single command line switch option, known also as flag.
            /// Like "-v", or "--version".
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Arguments Switch(string name)
            {
                return Parameter(name, 0);
            }

            /// <summary>
            /// Treat all unknown options as simple switches.
            /// </summary>
            /// <returns></returns>
            public Arguments Switch()
            {
                _UnknownAsParameter = false;
                return this;
            }

            #endregion

            #region Add

            /// <summary>
            /// Add arguments from string array.
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public Arguments Add(string[] args)
            {
                _List.AddRange(args);
                return this;
            }

            /// <summary>
            /// Add arguments from text line.
            /// <br/><br/>
            /// Arguments are divided by any whitespace character.
            /// Arguments may use double quote (") character to include whitespace,
            /// and multiple quoting is allowed within one argument.
            /// <br/><br/>
            /// For example: C:\"Documents and settings"\"Program Files"\
            /// will be considered as one argument.
            /// </summary>
            /// <param name="line"></param>
            /// <returns></returns>
            public Arguments Add(string line)
            {
                _List.AddRange(Explode(line));
                return this;
            }

            #endregion

            #region Line

            /// <summary>
            /// Add arguments from text line.
            /// <br/><br/>
            /// Arguments are divided by any whitespace character.
            /// Arguments may use double quote (") character to include whitespace,
            /// and multiple quoting is allowed within one argument.
            /// <br/><br/>
            /// For example: C:\"Documents and settings"\"Program Files"\
            /// will be considered as one argument.
            /// </summary>
            /// <param name="line"></param>
            /// <returns></returns>
            public Arguments Line(string line)
            {
                _List.AddRange(Explode(line));
                return this;
            }

            #endregion

            #region Explode

            /// <summary>
            /// Explode arguments from single line.
            ///
            /// Arguments are divided by any whitespace character.
            /// Arguments may use double quote (") character to include whitespace,
            /// and multiple quoting is allowed within one argument.
            ///
            /// For example: C:\"Documents and settings"\"Program Files"\
            /// will be considered as one argument.
            /// </summary>
            /// <param name="line"></param>
            /// <returns></returns>
            public static string[] Explode(string line)
            {
                List<string> l = new List<string>();
                if (!string.IsNullOrEmpty(line))
                {
                    Regex re;
                    //re = new Regex(@"((?:""(?:""""|[^""])*"")|(?:[^\s]+))", RegexOptions.None);
                    // This will match following string:
                    // **1 param1 C:\"Documents and settings"\"Program Files"\**
                    // as exactly three elements.
                    // --[[ (((?:[^\s"]*(?:"(?:""|[^"])*")[^\s"]*)+)|(?:[^\s]+)) ]]--
                    re = new Regex(@"((?:""(?:""""|[^""])*"")|(?:[^\s]+))", RegexOptions.None);
                    foreach (Match m in re.Matches(line))
                    {
                        l.Add(m.Groups[1].Value);
                    }
                }
                return l.ToArray();
            }

            #endregion

            #region Skip

            /// <summary>
            /// Skip first n entries when parsing arguments.
            /// </summary>
            /// <param name="skip"></param>
            /// <returns></returns>
            public Arguments Skip(int skip)
            {
                _Skip = skip;
                return this;
            }

            #endregion

            #region Strict

            /// <summary>
            /// Set strict mode.
            /// When strict mode is set, exception will be thrown on unrecognized option name.
            /// </summary>
            /// <param name="strict"></param>
            /// <returns></returns>
            public Arguments Strict(bool strict)
            {
                _Strict = strict;
                return this;
            }

            #endregion

            #region Slash

            /// <summary>
            /// Allow usage of slash options (starting with "/").
            /// Allows to use DOS style options like "/?".
            /// <br/><br/>
            /// It's RSX-11 (and other similar DEC systems), through CP/M to MS-DOS legacy.
            /// <br/><br/>
            /// Should be avoided probably.
            /// </summary>
            /// <param name="slash"></param>
            /// <returns></returns>
            public Arguments Slash(bool slash)
            {
                _Slash = slash;
                return this;
            }

            #endregion

            #region Short

            /// <summary>
            /// Allow usage of short options (starting with "-").
            /// Turned on as default.
            /// </summary>
            /// <param name="enable">Enable short options</param>
            /// <returns></returns>
            public Arguments Short(bool enable)
            {
                _Short = enable;
                return this;
            }

            #endregion

            #region Long

            /// <summary>
            /// Allow usage of long options (starting with "--").
            /// Turned on as default.
            /// </summary>
            /// <param name="enable">Enable short options</param>
            /// <returns></returns>
            public Arguments Long(bool enable)
            {
                _Long = enable;
                return this;
            }

            #endregion

            #region Alias

            /// <summary>
            /// Add alias to parameter key.
            /// </summary>
            /// <param name="alias"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public Arguments Alias(string alias, string name)
            {
                _Option.Alias[alias] = name;
                return this;
            }

            #endregion

            #region Help

            /// <summary>
            /// Add help description for parameter key.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            /// <returns></returns>
            public Arguments Help(string name, string description)
            {
                _Option.Help["name"] = description;
                return this;
            }

            #endregion

            #region Parse

            /// <summary>
            /// Parse arguments and set values for options.
            /// </summary>
            /// <returns></returns>
            public Arguments Parse()
            {
                return Parse(_List.ToArray());
            }

            /// <summary>
            /// Parse arguments and set values for options.
            /// </summary>
            /// <returns></returns>
            public Arguments Parse(string line)
            {
                return Parse(Explode(line));
            }

            /// <summary>
            /// Parse arguments and set values for options.
            /// </summary>
            /// <returns></returns>
            public Arguments Parse(string[] args)
            {
                bool enableSlash = _Slash;
                bool enableShort = _Short;
                bool enableLong = _Long;
                bool strictMode = _Strict;
                int skipCount = _Skip;

                _Heap.Clear();
                _Option.Zero();

                int length = args.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i < skipCount)
                    {
                        continue;
                    }
                    bool last = i < length - 1;
                    string arg = args[i];
                    if (string.IsNullOrEmpty(arg))
                    {
                        continue;
                    }
                    if ("--" == arg || "-" == arg)
                    {
                        _Heap.Add(arg);
                        continue;
                    }
                    string key = "";
                    //bool isShort = false;
                    if (enableSlash && '/' == arg[0])
                    {
                        if (1 == arg.Length)
                        {
                            _Heap.Add(arg);
                            continue;
                        }
                        key = arg.Substring(1);
                    }
                    else if (enableLong && arg.StartsWith("--"))
                    {
                        key = arg.Substring(2);
                    }
                    else if (enableShort && arg.StartsWith("-"))
                    {
                        //isShort = true;
                        key = arg.Substring(1);
                    }
                    if (0 == key.Length)
                    {
                        _Heap.Add(arg);
                        continue;
                    }
                    string target = _Option.Target(key);
                    Option opt = _Option.Find(target);
                    if (null == opt)
                    {
                        if (strictMode)
                        {
                            string m = $"Unrecognized command line option: {key}";
                            if (Debugger.IsAttached)
                            {
                                Debug.WriteLine($"Strict mode error: $m");
                            }
                            throw new Exception(m);
                        }
                        else
                        {
                            opt = new Option()
                            { 
                                Name = target,
                                Count = 0,
                                Value = key,
                                Temporary = true,
                            };
                            _Option.Add(opt);
                            continue;
                        }
                    }
                    if (0 == opt.Count)
                    {
                        opt.Value = key;
                        continue;
                    }
                    else if (1 == opt.Count)
                    {
                        if (i < length - 1)
                        {
                            i++;
                            opt.Value = args[i];
                        }
                        continue;
                    }
                    else
                    {
                        List<string> v = new List<string>();
                        int m = Math.Min(length, i + opt.Count);
                        for (int j = i + 1; j < m; j++)
                        {
                            v.Add(args[j]);
                        }
                        opt.Values = v.ToArray();
                        i += v.Count;
                        continue;
                    }
                }
                return this;
            }

            #endregion
        }

        #endregion

        #region Option

        public class Option
        {
            #region Array

            public class Array : List<Option> 
            {
                private Dictionary<string, string> _Alias = new Dictionary<string, string>();

                private Dictionary<string, string> _Help = new Dictionary<string, string>();

                public Dictionary<string, string> Alias { get => _Alias; }

                public Dictionary<string, string> Help { get => _Help; }

                /// <summary>
                /// Nullify all existing values.
                /// </summary>
                /// <returns></returns>
                public Array Zero()
                {
                    int i = this.Count;
                    while (0 <= --i)
                    {
                        if (this[i].Temporary)
                        {
                            this.RemoveAt(i);
                            continue;
                        }
                        this[i].Value = null;
                    }
                    return this;
                }

                /// <summary>
                /// Find element by name.
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public Option Find(string name)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (0 == string.Compare(this[i].Name, name, false))
                        {
                            return this[i];
                        }
                    }
                    return null;
                }

                /// <summary>
                /// Find target name by checking if it exists or has an alias.
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public string Target(string name)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (0 == string.Compare(this[i].Name, name, false))
                        {
                            return name;
                        }
                    }
                    if (_Alias.ContainsKey(name))
                    {
                        return _Alias[name];
                    }
                    return name;
                }
            }

            #endregion

            public string Name;

            public int Count;

            public bool Temporary;

            public string[] Values;

            public string Value { get => GetValue(); set => SetValue(value); }

            public bool Empty { get => IsEmpty(); }

            public bool Null { get => IsNull(); }

            public string GetValue()
            {
                return GetValue(" ");
            }

            public string GetValue(string join)
            {
                if (null == Values)
                {
                    return null;
                }
                if (0 == Values.Length)
                {
                    return "";
                }
                if (1 == Values.Length)
                {
                    return Values[0];
                }
                {
                    return string.Join(join, Values);
                }
            }

            public void SetValue(string value)
            {
                if (null == value)
                {
                    Values = null;
                }
                else if (0 == value.Length)
                {
                    Values = new string[] { };
                }
                else
                {
                    Values = new string[] { value };
                }
            }

            public override string ToString()
            {
                return ""
                    + this.Name
                    + (!string.IsNullOrEmpty(this.Name) ? " " : "")
                    + "("
                    + (this.Count == 0 ? "switch" : "parameter")
                    + ")"
                    + (null == this.Values ? "" : " = " + GetValue())
                    ;
            }

            public bool IsEmpty()
            {
                if (null == Values)
                {
                    return true;
                }
                if (0 == Values.Length)
                {
                    return true;
                }
                for (int i = 0; i < Values.Length; i++)
                {
                    if (null == Values[i])
                    {
                        continue;
                    }
                    if (0 < Values[i].Length)
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool IsNull()
            {
                if (null == Values)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
}
