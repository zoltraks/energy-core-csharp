using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Logging helper class
    /// </summary>
    public class Log
    {
        #region Entry

        /// <summary>
        /// Log entry
        /// </summary>
        public class Entry
        {
            /// <summary>
            /// Log entry message text
            /// </summary>
            public string Message;

            /// <summary>
            /// Log entry time
            /// </summary>
            public DateTime Stamp;

            /// <summary>
            /// Log entry error code
            /// </summary>
            public long Code;

            /// <summary>
            /// Log entry source name
            /// </summary>
            public string Source;

            /// <summary>
            /// Log entry severity level
            /// </summary>
            public Energy.Enumeration.LogLevel Level;

            /// <summary>
            /// Log entry additional context
            /// </summary>
            public object Context;

            /// <summary>
            /// Log entry exception
            /// </summary>
            public Exception Exception;

            /// <summary>
            /// Log entry store information
            /// </summary>
            public List<object> Store = new List<object>();

            public static bool IsToStringWide { get; set; }

            public static bool Bug { get; set; }

            #region Constructor

            public Entry()
            {
                Stamp = DateTime.Now;
            }

            public Entry(string message)
                : this()
            {
                Message = message;
            }

            public Entry(string message, string source)
                : this(message)
            {
                Source = source;
            }

            public Entry(string message, Energy.Enumeration.LogLevel level)
                : this(message)
            {
                Level = level;
            }

            public Entry(DateTime stamp, string message)
            {
                Stamp = stamp;
                Message = message;
            }

            public Entry(DateTime stamp, string message, Energy.Enumeration.LogLevel level)
                : this(stamp, message)
            {
                Level = level;
            }

            public Entry(DateTime stamp, string message, string source)
                : this(stamp, message)
            {
                Source = source;
            }

            public Entry(DateTime stamp, string message, string source, Energy.Enumeration.LogLevel level)
                : this(stamp, message, source)
            {
                Level = level;
            }

            public Entry(Exception exception)
                : this(Energy.Core.Bug.ExceptionMessage(exception))
            {
                Exception = exception;
            }

            #endregion

            #region Override

            public override string ToString()
            {
                List<string> list = new List<string>();
                DateTime stamp = this.Stamp != DateTime.MinValue ? this.Stamp : DateTime.Now;
                list.Add(stamp.ToString("HH:mm:ss.fff"));
                if (Code != 0)
                {
                    list.Add(Code.ToString());
                }
                if (!string.IsNullOrEmpty(Message))
                {
                    list.Add(Message);
                }
                return string.Join(" ", list.ToArray());
            }

            public string ToString(string format)
            {
                StringBuilder sb = new StringBuilder(format);
                DateTime stamp = this.Stamp != DateTime.MinValue ? this.Stamp : DateTime.Now;
                if (format.Contains("{{DATE}}"))
                    sb.Replace("{{DATE}}", stamp.ToString("yyyy-MM-dd"));
                if (format.Contains("{{TIME}}"))
                    sb.Replace("{{TIME}}", stamp.ToString("HH:mm:ss.fff").PadRight(12, '0'));
                if (format.Contains("{{MESSAGE}}"))
                    sb.Replace("{{MESSAGE}}", Message);
                if (format.Contains("{{EXCEPTION}}"))
                    sb.Replace("{{EXCEPTION}}", Energy.Core.Bug.GetExceptionMessage(Exception, true, true));
                return sb.ToString();
            }

            #endregion

            #region Implicit

            public static implicit operator Entry(string message)
            {
                return new Entry(message);
            }

            #endregion
        }

        public class Entry<T> : Entry
        {

        }

        #endregion

        #region Target

        /// <summary>
        /// Abstract class for Log target (console, file, database, etc.)
        /// </summary>
        public abstract partial class Target
        {
            /// <summary>
            /// Immediately call write on new entry
            /// </summary>
            public bool Immediate { get; set; }

            /// <summary>
            /// Work in background
            /// </summary>
            public bool Background { get; set; }

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

        #endregion

        #region Destination

        /// <summary>
        /// Log destination targets list
        /// </summary>
        public class Destination : Energy.Base.Collection.Array<Energy.Base.Log.Target>
        {
            #region Constructor

            public Destination() { }

            #endregion

            #region Accessor

            public Energy.Base.Log.Target this[Type target]
            {
                get
                {
                    return GetFirstByType(target);
                }
                private set
                {
                    SetFirstByType(target, value);
                }
            }

            private Target GetFirstByType(Type type)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].GetType() == type)
                        return this[i];
                }
                object a = Activator.CreateInstance(type);
                Energy.Base.Log.Target target = a as Energy.Base.Log.Target;
                if (target == null)
                    return null;
                else
                    this.Add(target);
                return target;
            }

            private void SetFirstByType(Type type, Target target)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].GetType() == type)
                    {
                        this[i] = target;
                        return;
                    }
                }
                this.Add(target);
            }

            #endregion

            /// <summary>
            /// Add new destination target
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            public new Target Add(Target target)
            {
                return base.Add(target);
            }

            public static Destination operator +(Destination left, Target right)
            {
                left.Add(right);
                return left;
            }

            public static Destination operator -(Destination left, Target right)
            {
                left.Remove(right);
                return left;
            }
        }

        #endregion
    }
}
