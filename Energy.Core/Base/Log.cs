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
            public DateTime Stamp;

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
            /// Log entry store information
            /// </summary>
            public List<object> Store = new List<object>();

            public static bool IsToStringWide { get; set; }

            #region Constructor

            public Entry()
            {
                Stamp = DateTime.Now;
            }

            public Entry(string message)
            {
                Stamp = DateTime.Now;
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

        public class Entry<T>: Entry
        {

        }
    }
}
