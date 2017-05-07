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

        public enum Level
        {
            None = 0,
            Stop = 1,
            Alert = 2,
            Error = 3,
            Warning = 4,
            Message = 5,
            Information = 6,
            Verbose = 7,
            Trace = 8,
            Bug = 9,
            Default = Message,
        }

        public class Entry
        {
            public string Message;
            public Level Level;
            public List<object> Store = new List<object>();
            
            public override string ToString()
            {
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
            entry.Level = Level.Alert;
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

        public Entry Add(string message, Level level)
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

        public void Write(string message, Level level)        
        {
            Write(Add(message, level));
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
            public Level Minimum = Level.None;

            /// <summary>
            /// Minimum entry log level for being accepted
            /// </summary>
            public Level Maximum = Level.None;

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
                if (Minimum != Level.None && entry.Level < Minimum)
                    return false;
                if (Maximum != Level.None && entry.Level > Maximum)
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
