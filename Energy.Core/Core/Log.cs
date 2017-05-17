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
    public partial class Log : List<Log.Entry>
    {
        #region Default

        public static Log Default
        {
            get
            {
                if (_default == null)
                {
                    lock (typeof(Log))
                    {
                        if (_default == null)
                        {
                            _default = new Log();
                        }
                    }

                }

                return _default;
            }
            set
            {
                lock (typeof(Log))
                {
                    _default = value;
                }
            }
        }

        private static volatile Log _default = null;

        #endregion
      
        public class Entry
        {
            public string Message { get; set; }

            public long Code { get; set; }

            public string Source { get; set; }

            public Energy.Enumeration.LogLevel Level { get; set; }

            public List<object> Store = new List<object>();
            
            public override string ToString()
            {
                if (Code != 0)
                {
                    if (string.IsNullOrEmpty(Message))
                        return Code.ToString();
                    return Code + ": " + Message;
                }
                return Message;
            }
        }

        public Energy.Base.Collection.Array<Target> Destination = new Energy.Base.Collection.Array<Target>();
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

        public Entry Add(Exception x)
        {
            Entry entry = new Entry();
            entry.Level = Energy.Enumeration.LogLevel.Alert;
            entry.Message = x.Message;
            //entry.Trace = 
            this.Add(entry);
            return entry;
        }

        public void Write(Exception x)
        {
            Write(Add(x));
        }

        public virtual void Save()
        {
            for (int i = 0; i < Destination.Count; i++)
            {
                Destination[i].Write(this);
            }
        }

        public Entry Add(string message, Energy.Enumeration.LogLevel level)
        {
            Entry entry = new Entry();
            entry.Message = message;
            entry.Level = level;
            this.Add(entry);
            return entry;
        }

        public void Write(Entry entry)
        {
            for (int i = 0; i < Destination.Count; i++)
            {
                if (Destination[i].Immediate && Destination[i].Accept(entry))
                {
                    Destination[i].Write(entry);
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
            Entry entry = new Entry()
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
            public bool Immediate;

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
            public abstract bool Write(List<Entry> log);

            public bool Write(Entry entry)
            {
                return Write(new List<Entry>(new Entry[] { entry }));
            }

            /// <summary>
            /// Check if entry is accepted by level requirements if any
            /// </summary>
            /// <param name="entry"></param>
            /// <returns></returns>
            public bool Accept(Entry entry)
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

    public partial class Log
    {
        public partial class Target
        {
            #region Trace

            public class Trace : Target
            {
                public override bool Write(List<Entry> log)
                {
                    for (int i = 0; i < log.Count; i++)
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
                public string Path;

                public override bool Write(List<Entry> log)
                {
                    string file = Path;
                    string content = String.Join(Environment.NewLine, (new List<Entry>(log)).ConvertAll<string>(delegate(Entry e) { return e.ToString(); }).ToArray())
                        + Environment.NewLine;
                    System.IO.File.AppendAllText(file, content);
                    for (int i = 0; i < log.Count; i++)
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

                public string Format;

                public override bool Write(List<Entry> log)
                {
                    for (int i = 0; i < log.Count; i++)
                    {
                        if (log[i].Store.Contains(this))
                            continue;
                        if (Accept(log[i]))
                        {
                            System.Console.WriteLine(log[i].ToString());
                        }
                        log[i].Store.Add(this);
                    }
               
                    return true;
                }
            }

            #endregion
        }
    }
}
