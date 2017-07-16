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
    public partial class Log : List<Energy.Base.Log.Entry>
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

        #region Static

        private static Log _Static = null;

        private static readonly object _StaticLock = new object();

        /// <summary>
        /// Singleton
        /// </summary>
        public static Log Static
        {
            get
            {
                if (_Static == null)
                {
                    lock (_StaticLock)
                    {
                        if (_Static == null)
                        {
                            _Static = new Log();
                        }
                    }
                }

                return _Static;
            }
            set
            {
                lock (_StaticLock)
                {
                    _Static = value;
                }
            }
        }

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
                RemoveAll(delegate(Energy.Base.Log.Entry _) { return true; });
                return;
            }
            int i = Count;
            while (--i >= 0)
            {
                Energy.Base.Log.Entry entry = this[i];
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
                    this.RemoveAt(i);
                    continue;
                }
            }
        }

        public Energy.Base.Log.Entry Add(Exception x)
        {
            Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry(x);
            this.Add(entry);
            return entry;
        }

        public void Write(Exception x)
        {
            Write(Add(x));
        }

        public virtual void Save()
        {
            for (int n = 0; n < this.Count; n++)
            {                
                Energy.Base.Log.Entry entry = this[n];
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

        public void Write(Energy.Base.Log.Entry entry)
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
        }

        public void Write(string message, Energy.Enumeration.LogLevel level)        
        {
            Write(Add(message, level));
        }

        public void Write(string message)
        {
            Write(Add(message, Energy.Enumeration.LogLevel.Default));
        }

        public void Write(string message, string source, long code)
        {
            Write(message, source, code, Energy.Enumeration.LogLevel.Default);
        }

        public void Write(string message, string source, long code, Energy.Enumeration.LogLevel level)
        {
            Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry()
            {
                Message = message,
                Source = source,
                Code = code,
                Level = level,
            };
            Write(entry);
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
                            System.Diagnostics.Trace.WriteLine(log[i].ToString(true));
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
        }
    }

    #endregion
}
