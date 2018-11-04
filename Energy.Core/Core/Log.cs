using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Energy.Core
{
    #region Log

    public partial class Log
    {
        #region Default

        private static Logger _Default = null;

        private static readonly Energy.Base.Lock _DefaultLock = new Energy.Base.Lock();

        /// <summary>
        /// Singleton
        /// </summary>
        [Energy.Attribute.Code.Rename(Target = "Global", Comment = "Change design pattern 'Default' into 'Global'")]
        public static Logger Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_DefaultLock)
                    {
                        if (_Default == null)
                        {
                            _Default = new Logger();
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
    }

    #endregion

    #region Logger

    public partial class Log
    {
        /// <summary>
        /// Logger
        /// </summary>
        public class Logger
        {
            #region Destination

            /// <summary>
            /// Destination
            /// </summary>
            public Energy.Base.Log.Destination Destination = new Energy.Base.Log.Destination();

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            public Logger()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="type"></param>
            public Logger(Type type)
                : this()
            {
                if (type.IsSubclassOf(typeof(Energy.Base.Log.Target)))
                {
                    this.Destination.Add((Energy.Base.Log.Target)Activator.CreateInstance(type));
                }
            }

            #endregion

            #region Lock

            private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

            #endregion

            #region Buffer

            private List<Energy.Base.Log.Entry> _List = new List<Base.Log.Entry>();

            #endregion

            #region Property

            private int _Maximum;

            /// <summary>
            /// Maximum number of entries
            /// </summary>
            public int Maximum { get { lock (_Lock) return _Maximum; } set { lock (_Lock) _Maximum = value; } }

            #endregion

            #region Event

            public event EventHandler OnFlush;

            #endregion

            #region Setup

            /// <summary>
            /// Make default log setup
            /// </summary>
            /// <param name="file"></param>
            /// <param name="console"></param>
            /// <param name="eventHandler"></param>
            public bool Setup(string file, bool console, EventHandler eventHandler)
            {
                if (console)
                {
                    Energy.Core.Log.Target.Console consoleTarget = new Energy.Core.Log.Target.Console()
                    {
                        Immediate = true,
                        Background = true,
                    };
                    this.Destination.Add(consoleTarget);
                }
                if (!string.IsNullOrEmpty(file))
                {
                    Energy.Core.Log.Target.File fileTarget = new Energy.Core.Log.Target.File()
                    {
                        Path = file,
                        Immediate = true,
                    };
                    this.Destination.Add(fileTarget);
                }
                if (eventHandler != null)
                {
                    Energy.Core.Log.Target.Event eventTarget = new Energy.Core.Log.Target.Event(eventHandler)
                    {
                        Immediate = true,
                    };
                    this.Destination.Add(eventTarget);
                }
                return true;
            }

            #endregion

            #region ToString

            public override string ToString()
            {
                List<string> list = new List<string>();
                lock (_Lock)
                {
                    for (int i = 0; i < _List.Count; i++)
                    {
                        list.Add(_List[i].ToString());
                    }
                }
                return string.Join(Energy.Base.Text.NL, list.ToArray());
            }

            #endregion

            #region Flush

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

            #endregion

            #region Clean
         
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
           
            #endregion

            #region Add

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

            public Energy.Base.Log.Entry Add(string message, Energy.Enumeration.LogLevel level)
            {
                Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry(message, level);
                this.Add(entry);
                return entry;
            }

            #endregion

            #region Write

            /// <summary>
            /// Write exception
            /// </summary>
            /// <param name="exception"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(Exception exception)
            {
                return Write(Add(exception));
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

            #endregion

            #region Save

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

            #endregion

            #region Exception

            public Energy.Base.Log.Entry Exception(Exception exception)
            {
                return Write(Add(exception));
            }

            #endregion
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
                public string Path { get; set; }

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
                        System.Console.WriteLine(string.Join("\n", syntax.Dictionary.ToArray(": ")));
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

            public class Event : Energy.Base.Log.Target
            {
                public Event(EventHandler eventHandler)
                {
                    EventHandler = eventHandler;
                }

                public EventHandler EventHandler { get; set; }

                private bool WriteUnsafe(Energy.Base.Log.Entry[] log)
                {
                    if (log == null)
                        return false;
                    if (EventHandler == null)
                        return true;
                    for (int i = 0; i < log.Length; i++)
                    {
                        Energy.Base.Log.Entry entry = log[i];
                        if (entry.Store.Contains(this))
                            continue;
                        if (Accept(log[i]))
                        {
                            try
                            {
                                this.EventHandler(log[i], null);
                            }
                            catch (Exception exception)
                            {
                                Energy.Core.Bug.Write("E0017", exception);
                                return false;
                            }
                        }
                        entry.Store.Add(this);
                    }
                    return true;
                }

                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    if (!Background)
                    {
                        return WriteUnsafe(log);
                    }
                    else
                    {
                        Thread thread = new Thread(() => { WriteUnsafe(log); })
                        {
                            IsBackground = true,
                            CurrentUICulture = Energy.Core.Application.GetCurrentUICulture(),
                        };
                        thread.Start();
                        return true;
                    }
                }
            }

            #endregion
        }
    }

    #endregion
}
