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
        #region Default

        public Manager Destination = new Manager();

        private static Log _Default = null;

        private static readonly object _DefaultLock = new object();

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

        public event EventHandler OnFlush;

        private static readonly object FlushStatic = new object();

        public void Flush()
        {
            lock (FlushStatic)
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
            int i = Count;
            while (--i >= 0)
            {
                Energy.Base.Log.Entry entry = this[i];
                bool remove = true;
                for (int n = 0; n < Destination.Count; n++)
                {
                    Energy.Core.Log.Target target = Destination[n];
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

        private List<string> log;

        public Log(Type type)
            : this()
        {
            if (type.IsSubclassOf(typeof(Target)))
            {
                Destination.Add((Target)Activator.CreateInstance(type));
            }
        }

        public Log()
        {
            // TODO: Complete member initialization
        }

        public Log(List<string> log)
        {
            // TODO: Complete member initialization
            this.log = log;
        }

        public Energy.Base.Log.Entry Add(Exception x)
        {
            Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry();
            entry.Level = Energy.Enumeration.LogLevel.Alert;
            entry.Message = x.Message;
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
                    Energy.Core.Log.Target target = Destination[i];
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
            Energy.Base.Log.Entry entry = new Energy.Base.Log.Entry();
            entry.Message = message;
            entry.Level = level;
            this.Add(entry);
            return entry;
        }

        public void Write(Energy.Base.Log.Entry entry)
        {
            this.Add(entry);
            for (int i = 0; i < Destination.Count; i++)
            {
                Energy.Core.Log.Target target = Destination[i];
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
        public abstract partial class Target
        {
            /// <summary>
            /// Immediately call write on new entry
            /// </summary>
            public bool Immediate { get; set; }

            /// <summary>
            /// Minimum entry log level for being accepted
            /// </summary>
            public Energy.Enumeration.LogLevel Minimum = Energy.Enumeration.LogLevel.None;

            /// <summary>
            /// Minimum entry log level for being accepted
            /// </summary>
            public Energy.Enumeration.LogLevel Maximum = Energy.Enumeration.LogLevel.None;

            /// <summary>
            /// Write list of entries
            /// </summary>
            /// <param name="log">List&lt;Entry&gt; - log</param>
            /// <returns></returns>
            public abstract bool Write(Energy.Base.Log.Entry[] log);

            /// <summary>
            /// Write single entry
            /// </summary>
            /// <param name="entry"></param>
            /// <returns></returns>
            public bool Write(Energy.Base.Log.Entry entry)
            {
                return Write(new Energy.Base.Log.Entry[] { entry });
            }

            /// <summary>
            /// Check if entry is accepted by level requirements if any
            /// </summary>
            /// <param name="entry"></param>
            /// <returns></returns>
            public bool Accept(Energy.Base.Log.Entry entry)
            {
                if (Minimum != Energy.Enumeration.LogLevel.None && entry.Level < Minimum)
                    return false;
                if (Maximum != Energy.Enumeration.LogLevel.None && entry.Level > Maximum)
                    return false;
                return true;
            }
        }
    }

    #endregion

    #region Target

    public partial class Log
    {
        public partial class Target
        {
            #region Trace

            public class Trace : Target
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
                            System.Diagnostics.Trace.Write(log[i].ToString() + Environment.NewLine);
                        }
                        log[i].Store.Add(this);
                    }

                    return true;
                }
            }
            
            #endregion

            #region File

            public class File : Target
            {
                /// <summary>
                /// File path
                /// </summary>
                public string Path;

                /// <summary>
                /// Write all messages even not accepted
                /// </summary>
                public bool All;

                public override bool Write(Energy.Base.Log.Entry[] log)
                {
                    if (log == null)
                        return false;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < log.Length; i++)
                    {
                        if (!All && !Accept(log[i]))
                            continue;
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

            public class Console : Target
            {
                public Console()
                {
                    Immediate = true;
                }

                public string Format = "{{TIME}} {{MESSAGE}}";

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
                    for (int i = 0; i < log.Length; i++)
                    {
                        if (!Accept(log[i]))
                            continue;
                        System.Console.WriteLine(log[i].ToString(Format));
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

    #region Destination

    public partial class Log
    {
        public class Manager : Energy.Base.Collection.Array<Target>
        {
            #region Constructor

            public Manager() { }

            #endregion
        }
    }

    #endregion
}
