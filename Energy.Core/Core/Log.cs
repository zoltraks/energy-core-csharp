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
        public class Logger: Energy.Interface.ILogger
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

            private readonly List<Energy.Base.Log.Entry> _List = new List<Base.Log.Entry>();

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
                    {
                        OnFlush(this, null);
                    }
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

            /// <summary>
            /// Add message to log
            /// </summary>
            /// <param name="message"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Add(string message, Energy.Enumeration.LogLevel level)
            {
                return Add(new Energy.Base.Log.Entry(message, level));
            }

            /// <summary>
            /// Add message to log
            /// </summary>
            /// <param name="message"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Add(string message, int level)
            {
                return Add(new Energy.Base.Log.Entry(message, level));
            }

            #endregion

            #region Write

            public Energy.Base.Log.Entry Write(Energy.Base.Log.Entry entry)
            {
                this.Add(entry);
                for (int i = 0; i < Destination.Count; i++)
                {
                    Energy.Base.Log.Target target = Destination[i];
                    if (!target.Enable)
                    {
                        continue;
                    }
                    if (!target.Immediate)
                    {
                        continue;
                    }
                    if (entry.Store.Contains(target))
                    {
                        continue;
                    }
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

            /// <summary>
            /// Write exception
            /// </summary>
            /// <param name="exception"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(Exception exception)
            {
                return Write(new Energy.Base.Log.Entry(exception, Energy.Enumeration.LogLevel.Error));
            }

            /// <summary>
            /// Write exception with source or category name
            /// </summary>
            /// <param name="exception"></param>
            /// <param name="source"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(Exception exception, string source)
            {
                return Write(new Energy.Base.Log.Entry(exception, source, Enumeration.LogLevel.Error));
            }

            /// <summary>
            /// Write message with severity level
            /// </summary>
            /// <param name="message"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, Energy.Enumeration.LogLevel level)
            {
                return Write(new Energy.Base.Log.Entry(message, level));
            }

            /// <summary>
            /// Write message with severity level
            /// </summary>
            /// <param name="message"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, int level)
            {
                return Write(new Energy.Base.Log.Entry(message, level));
            }

            /// <summary>
            /// Write message with source or category name and severity level
            /// </summary>
            /// <param name="message"></param>
            /// <param name="source"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, string source, Energy.Enumeration.LogLevel level)
            {
                return Write(new Energy.Base.Log.Entry(message, source, level));
            }

            /// <summary>
            /// Write message with source or category name and severity level
            /// </summary>
            /// <param name="message"></param>
            /// <param name="source"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, string source, int level)
            {
                return Write(new Energy.Base.Log.Entry(message, source, level));
            }

            /// <summary>
            /// Write message
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message)
            {
                return Write(new Energy.Base.Log.Entry(message));
            }

            /// <summary>
            /// Write message with source or category name and optional code
            /// </summary>
            /// <param name="message"></param>
            /// <param name="source"></param>
            /// <param name="code"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, string source, string code)
            {
                return Write(new Energy.Base.Log.Entry(message, source, code, Energy.Enumeration.LogLevel.Default));
            }

            /// <summary>
            /// Write message with source or category name and optional code and severity level
            /// </summary>
            /// <param name="message"></param>
            /// <param name="source"></param>
            /// <param name="code"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, string source, string code, Energy.Enumeration.LogLevel level)
            {
                return Write(new Energy.Base.Log.Entry(message, source, code, (int)level));
            }

            /// <summary>
            /// Write message with source or category name and optional code and severity level
            /// </summary>
            /// <param name="message"></param>
            /// <param name="source"></param>
            /// <param name="code"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Energy.Base.Log.Entry Write(string message, string source, string code, int level)
            {
                return Write(new Energy.Base.Log.Entry(message, source, code, level));
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
                    {
                        return false;
                    }
                    for (int i = 0; i < log.Length; i++)
                    {
                        if (log[i].Store.Contains(this))
                        {
                            continue;
                        }
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
                private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

                private readonly ManualResetEvent _ActivateResetEvent = new ManualResetEvent(false);

                private const double SLEEP_DELAY = 0.5;

                private TimeSpan _SleepDelay = TimeSpan.FromSeconds(SLEEP_DELAY);

                private const double INACTIVITY_LIMIT = 30.0;

                private TimeSpan _InactivityLimit = TimeSpan.FromSeconds(INACTIVITY_LIMIT);

                private readonly List<Energy.Base.Log.Entry> _Buffer = new List<Energy.Base.Log.Entry>();

                private Thread _Thread;

                private DateTime _LastWriteStamp;

                public DateTime LastWriteStamp { get { return GetLastWriteStamp(); } }

                /// <summary>
                /// File path
                /// </summary>
                public string Path { get; set; }

                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    if (!Background)
                    {
                        return WriteUnsafe(log);
                    }
                    else
                    {
                        lock (_Lock)
                        {
                            _Buffer.AddRange(log);
                        }
                        EnsureWriteThreadRunning();
                        return true;
                    }
                }

                private void EnsureWriteThreadRunning()
                {
                    bool activate = true;

                    if (_Thread == null)
                    {
                        lock (_Lock)
                        {
                            if (_Thread == null)
                            {
                                Thread thread = new Thread(() => { WriteThread(); })
                                {
                                    IsBackground = true,
                                };
#if !NETCF
                                thread.CurrentUICulture = Energy.Core.Program.GetCultureInfo();
#endif
                                _Thread = thread;
                                _ActivateResetEvent.Reset();
                                activate = false;
                                thread.Start();
                            }
                        }
                    }
                    if (activate)
                    {
                        _ActivateResetEvent.Set();
                    }
                }

                public DateTime GetLastWriteStamp()
                {
                    lock (_Lock)
                    {
                        return _LastWriteStamp;
                    }
                }

                private void WriteThread()
                {
                    try
                    {
                        while (true)
                        {
                            Energy.Base.Log.Entry[] entries = null;
                            lock (_Lock)
                            {
                                entries = _Buffer.ToArray();
                                if (entries.Length > 0)
                                {
                                    _Buffer.RemoveRange(0, entries.Length);
                                }
                            }
                            if (entries == null)
                            {
                                if (DateTime.Now - GetLastWriteStamp() > _InactivityLimit)
                                {
                                    Energy.Core.Bug.Write("E8005", "File Log Inactivity");
                                    break;
                                }
                            }
                            if (WriteUnsafe(entries))
                            {
                                lock (_Lock)
                                {
                                    _LastWriteStamp = DateTime.Now;
                                }
                                continue;
                            }
                            else
                            {
                                lock (_Lock)
                                {
                                    _Buffer.InsertRange(0, entries);
                                }
                            }
                            if (_ActivateResetEvent.WaitOne((int)_SleepDelay.TotalMilliseconds, true))
                                continue;
                            else
                                continue;
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        Energy.Core.Bug.Write("E8007", "File Log Abort");
                    }
                }

                private bool WriteUnsafe(Energy.Base.Log.Entry[] log)
                {
                    if (log == null)
                        return false;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < log.Length; i++)
                    {
                        sb.Append(log[i].ToString());
                        sb.Append(Energy.Base.Text.NL);
                    }
                    if (sb.Length != 0)
                    {
                        string file = Path;
                        string content = sb.ToString();
                        try
                        {
                            Energy.Base.File.AppendText(file, content);
                            //System.IO.File.AppendAllText(file, content);
                        }
                        catch (Exception exception)
                        {
                            Energy.Core.Bug.Write("E6006", exception);
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
                private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

                private ManualResetEvent _ActivateResetEvent = new ManualResetEvent(false);

                private const double SLEEP_DELAY = 0.5;

                private TimeSpan _SleepDelay = TimeSpan.FromSeconds(SLEEP_DELAY);

                private const double INACTIVITY_LIMIT = 30.0;

                private TimeSpan _InactivityLimit = TimeSpan.FromSeconds(INACTIVITY_LIMIT);

                private List<Energy.Base.Log.Entry> _Buffer = new List<Energy.Base.Log.Entry>();

                private Thread _Thread;

                private DateTime _LastWriteStamp;

                public DateTime LastWriteStamp { get { return GetLastWriteStamp(); } }

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

                public DateTime GetLastWriteStamp()
                {
                    lock (_Lock)
                    {
                        return _LastWriteStamp;
                    }
                }

                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    if (!Background)
                    {
                        return WriteUnsafe(log);
                    }
                    else
                    {
                        lock (_Lock)
                        {
                            _Buffer.AddRange(log);
                        }
                        EnsureWriteThreadRunning();
                        return true;
                    }
                }

                private void EnsureWriteThreadRunning()
                {
                    bool activate = true;

                    if (_Thread == null)
                    {
                        lock (_Lock)
                        {
                            if (_Thread == null)
                            {
                                Thread thread = new Thread(() => { WriteThread(); })
                                {
                                    IsBackground = true,
                                };
#if !NETCF
                                thread.CurrentUICulture = Energy.Core.Program.GetCultureInfo();
#endif
                                _Thread = thread;
                                _ActivateResetEvent.Reset();
                                activate = false;
                                thread.Start();
                            }
                        }
                    }
                    if (activate)
                    {
                        _ActivateResetEvent.Set();
                    }
                }

                public bool WriteUnsafe(Energy.Base.Log.Entry[] log)
                {
                    for (int i = 0; i < log.Length; i++)
                    {
                        System.Console.WriteLine(log[i].ToString(Format));
                    }
                    for (int i = 0; i < log.Length; i++)
                    {
                        log[i].Store.Add(this);
                    }
                    return true;
                }

                private Energy.Core.Syntax _Syntax;

                private Energy.Core.Syntax GetSyntaxReset()
                {
                    lock (_Lock)
                    {
                        if (_Syntax == null)
                        {
                            _Syntax = new Energy.Core.Syntax();
                        }
                        else
                        {
                            _Syntax.Reset();
                        }
                        return _Syntax;
                    }
                }

                private void WriteThread()
                {
                    try
                    {
                        while (true)
                        {
                            Energy.Base.Log.Entry[] entries = null;
                            lock (_Lock)
                            {
                                entries = _Buffer.ToArray();
                                if (entries.Length > 0)
                                {
                                    _Buffer.RemoveRange(0, entries.Length);
                                }
                            }
                            if (entries == null)
                            {
                                if (DateTime.Now - GetLastWriteStamp() > _InactivityLimit)
                                {
                                    Energy.Core.Bug.Write("Energy.Core.Log.Target.Console.WriteThread"
                                        , "Console Log Inactivity"
                                        );
                                    break;
                                }
                            }
                            if (WriteUnsafe(entries))
                            {
                                lock (_Lock)
                                {
                                    _LastWriteStamp = DateTime.Now;
                                }
                                continue;
                            }
                            else
                            {
                                lock (_Lock)
                                {
                                    _Buffer.InsertRange(0, entries);
                                }
                            }
                            if (_ActivateResetEvent.WaitOne((int)_SleepDelay.TotalMilliseconds, true))
                            {
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        Energy.Core.Bug.Write("E8002", "Console Log Abort");
                    }
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
                        };
#if !NETCF
                        thread.CurrentUICulture = Energy.Core.Program.GetCultureInfo();
#endif
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
