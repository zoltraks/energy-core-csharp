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
            public string Message { get; set; }

            /// <summary>
            /// Log entry time
            /// </summary>
            public DateTime Stamp { get; set; }

            /// <summary>
            /// Log entry error code
            /// </summary>
            public long Code { get; set; }

            /// <summary>
            /// Log entry source name
            /// </summary>
            public string Source { get; set; }

            /// <summary>
            /// Log entry severity level
            /// </summary>
            public Energy.Enumeration.LogLevel Level { get; set; }

            /// <summary>
            /// Log entry additional context
            /// </summary>
            public object Context { get; set; }

            /// <summary>
            /// Log entry exception
            /// </summary>
            public Exception Exception { get; set; }

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
                : this(exception.Message)
            {
                Exception = exception;
            }

            #endregion

            #region Override

            public string ToString(bool wide)
            {
                if (wide)
                {
                    DateTime stamp = this.Stamp != DateTime.MinValue ? this.Stamp : DateTime.Now;
                    return string.Concat(stamp.ToString("HH:mm:ss.fff "), ToString(false));
                }
                if (Code != 0)
                {
                    if (string.IsNullOrEmpty(Message))
                        return Code.ToString();
                    return Code + ": " + Message;
                }
                return Message;
            }

            public override string ToString()
            {
                return ToString(IsToStringWide);
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
                    sb.Replace("{{EXCEPTION}}", Energy.Core.Bug.ExceptionMessage(Exception, true));
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

        public class Destination : Energy.Base.Collection.Array<Target>
        {
            #region Constructor

            public Destination() { }

            #endregion

            public new Target Add(Target target)
            {
                return base.Add(target);
            }
        }

        #endregion
    }
}
