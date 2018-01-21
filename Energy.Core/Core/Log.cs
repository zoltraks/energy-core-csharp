using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Energy.Core
{
    #region Log

    /// <summary>
    /// Log
    /// </summary>
    public partial class Log
    {
        /// <summary>
        /// Destination
        /// </summary>
        public Energy.Base.Log.Destination Destination = new Energy.Base.Log.Destination();

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Log()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        public Log(Type type)
            : this()
        {
            if (type.IsSubclassOf(typeof(Energy.Base.Log.Target)))
            {
                Destination.Add((Energy.Base.Log.Target)Activator.CreateInstance(type));
            }
        }

        #endregion

        #region Lock

        private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

        #endregion

        #region Buffer

        private List<Energy.Base.Log.Entry> _List = new List<Base.Log.Entry>();

        #endregion

        #region Default

        private static Log _Default = null;

        private static readonly Energy.Base.Lock _DefaultLock = new Energy.Base.Lock();

        /// <summary>
        /// Singleton
        /// </summary>
        public static Log Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_DefaultLock)
                    {
                        if (_Default == null)
                        {
                            _Default = new Log();
                        }
                    }
                }
                return _Default;
            }
            set
            {
                lock (_DefaultLock)
                {
                    _Default = value;
                }
            }
        }

        #endregion

        #region Property

        private int _Maximum;

        /// <summary>
        /// Maximum number of entries
        /// </summary>
        public int Maximum { get { lock (_Lock) return _Maximum; } set { lock (_Lock) _Maximum = value; } }

        #endregion

        public event EventHandler OnFlush;

        private static readonly object FlushLock = new object();

        public void Flush()
        {
            lock (FlushLock)
            {
                if (OnFlush != null)
                    OnFlush(this, null);
                Save();
                Clean();
            }
        }

        /// <summary>
        /// Remove entries that were sucesfully written to all destinations.
        /// If no destination set, all entries will be removed.
        /// </summary>
        public void Clean()
        {
            if (Destination.Count == 0)
            {
                lock (_Lock)
                {
                    _List.Clear();
                }
                return;
            }

            int i = _List.Count;
            while (--i >= 0)
            {
                Energy.Base.Log.Entry entry = _List[i];
                bool remove = true;
                for (int n = 0; n < Destination.Count; n++)
                {
                    Energy.Base.Log.Target target = Destination[n];
                    if (!entry.Store.Contains(target))
                    {
                        remove = false;
                        break;
                    }
                }
                if (remove)
                {
                    _List.RemoveAt(i);
                    continue;
                }
            }
        }

        /// <summary>
        /// Add entry to log
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public Energy.Base.Log.Entry Add(Energy.Base.Log.Entry entry)
        {
            lock (_Lock)
            {
                _List.Add(entry);
            }
            return entry;
        }

        /// <summary>
        /// Add exception to log
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public Energy.Base.Log.Entry Add(Exception exception)
        {
            return Add(new Energy.Base.Log.Entry(exception));
        }

        /// <summary>
        /// Write exception
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Energy.Base.Log.Entry Write(Exception x)
        {
            return Write(Add(x));
        }

        public virtual void Save()
        {
            int m = _List.Count;
            for (int n = 0; n < m; n++)
            {
                Energy.Base.Log.Entry entry = _List[n];
                for (int i = 0; i < Destination.Count; i++)
                {
                    Energy.Base.Log.Target target = Destination[i];
                    if (!target.Immediate)
                        continue;
                    if (entry.Store.Contains(target))
                        continue;
                    if (target.Accept(entry))
                    {
                        target.Write(entry);
                        if (!entry.Store.Contains(target))
                        {
                            entry.Store.Add(target);
                        }
                    }
                }
            }
        }

        public Energy.Base.Log.Entry Add(string message, Energy.Enumeration.LogLevel level)
        {
            Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry(message, level);
            this.Add(entry);
            return entry;
        }

        public Energy.Base.Log.Entry Write(Energy.Base.Log.Entry entry)
        {
            this.Add(entry);
            for (int i = 0; i < Destination.Count; i++)
            {
                Energy.Base.Log.Target target = Destination[i];
                if (!target.Immediate)
                    continue;
                if (entry.Store.Contains(target))
                    continue;
                if (target.Accept(entry))
                {
                    target.Write(entry);
                    if (!entry.Store.Contains(target))
                    {
                        entry.Store.Add(target);
                    }
                }
            }
            return entry;
        }

        public Energy.Base.Log.Entry Write(string message, Energy.Enumeration.LogLevel level)
        {
            return Write(Add(message, level));
        }

        public Energy.Base.Log.Entry Write(string message)
        {
            return Write(Add(message, Energy.Enumeration.LogLevel.Default));
        }

        public Energy.Base.Log.Entry Write(string message, string source, long code)
        {
            return Write(message, source, code, Energy.Enumeration.LogLevel.Default);
        }

        public Energy.Base.Log.Entry Write(string message, string source, long code, Energy.Enumeration.LogLevel level)
        {
            Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry()
            {
                Message = message,
                Source = source,
                Code = code,
                Level = level,
            };
            return Write(entry);
        }
    }

    #endregion

    #region Target

    public partial class Log
    {
        public partial class Target
        {
            #region Trace

            /// <summary>
            /// Trace log target
            /// </summary>
            public class Trace : Energy.Base.Log.Target
            {
                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    if (log == null)
                        return false;
                    for (int i = 0; i < log.Length; i++)
                    {
                        if (log[i].Store.Contains(this))
                            continue;
                        if (Accept(log[i]))
                        {
                            System.Diagnostics.Trace.WriteLine(log[i].ToString());
                        }
                        log[i].Store.Add(this);
                    }

                    return true;
                }
            }

            #endregion

            #region File

            public class File : Energy.Base.Log.Target
            {
                /// <summary>
                /// File path
                /// </summary>
                public string Path;

                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    if (log == null)
                        return false;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < log.Length; i++)
                    {
                        sb.AppendLine(log[i].ToString());
                    }
                    if (sb.Length != 0)
                    {
                        string file = Path;
                        string content = sb.ToString();
                        try
                        {
                            System.IO.File.AppendAllText(file, content);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < log.Length; i++)
                    {
                        log[i].Store.Add(this);
                    }

                    return true;
                }
            }

            #endregion

            #region Console

            public class Console : Energy.Base.Log.Target
            {
                public Console()
                {
                    Immediate = true;
                }

                private string _Format = "{{TIME}} {{MESSAGE}}";

                public string Format
                {
                    get
                    {
                        return _Format;
                    }
                    set
                    {
                        _Format = value;
                    }
                }

                private static Console _Default;

                private static readonly object _DefaultLock = new object();

                public static Console Default
                {
                    get
                    {
                        if (_Default == null)
                        {
                            lock (_DefaultLock)
                            {
                                if (_Default == null)
                                {
                                    _Default = new Console()
                                    {
                                        Immediate = true,
                                    };
                                }
                            }
                        }
                        return _Default;
                    }
                }

                /// <summary>
                /// Use color on consolewrite
                /// </summary>
                public bool Color { get; set; }

                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    Energy.Core.Syntax syntax = new Energy.Core.Syntax(Format);
                    syntax["Date"] = "2017-07-00";
                    if (!Color)
                    {
                        for (int i = 0; i < log.Length; i++)
                        {
                            System.Console.WriteLine(log[i].ToString(Format));
                        }
                    }
                    else
                    {
                        syntax["Date"] = "2017-07-01";
                        syntax["DATE"] = "2017-07-02";
                        System.Console.WriteLine(string.Join("\n", syntax.ToArray(": ")));
                    }

                    for (int i = 0; i < log.Length; i++)
                    {
                        log[i].Store.Add(this);
                    }
                    return true;
                }
            }

            #endregion

            #region System

            #endregion

            #region Event

            #endregion
        }
    }

    #endregion
}
