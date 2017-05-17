using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing boolean values
    /// </summary>
    /// <remarks>
    /// Log level is also a number 0-9 which represents severity of messages.
    /// Lower numbers are more important and higher less and default is 5.
    /// 
    /// Three levels may be found oftenly:
    /// 
    /// Energy.Enumeration.LogLevel.Message which is default
    /// Energy.Enumeration.LogLevel.Error for normal errors
    /// Energy.Enumeration.LogLevel.Verbose for verbose messages
    /// 
    /// </remarks>
    public enum LogLevel
    {
        /// <summary>
        /// Uknown severity level / Not used
        /// </summary>
        None = 0,

        /// <summary>
        /// Stop / Critical
        /// </summary>
        Stop = 1,

        /// <summary>
        /// Alert / Fatal
        /// </summary>
        Alert = 2,

        /// <summary>
        /// Error
        /// </summary>
        Error = 3,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Message / Default
        /// </summary>
        Message = 5,

        /// <summary>
        /// Information
        /// </summary>
        Information = 6,

        /// <summary>
        /// Verbose
        /// </summary>
        /// <remarks>
        /// Be more verbose.
        /// This level may include program execution details for advanced monitoring.
        /// </remarks>
        Verbose = 7,

        /// <summary>
        /// Trace
        /// </summary>
        /// <remarks>
        /// Additional messages for tracing program execution.
        /// </remarks>
        Trace = 8,

        /// <summary>
        /// Bug
        /// </summary>
        /// <remarks>
        /// Maximum possible level intended for diagnostic messages.
        /// For debugging purposes.
        /// </remarks>
        Bug = 9,

        Critical = Stop,
        Fatal = Alert,

        Default = Message,

        /// <summary>
        /// Very important severity
        /// </summary>
        OMG = Alert,

        /// <summary>
        /// Less important message severity
        /// </summary>
        LOL = Verbose,

        /// <summary>
        /// Warning about something should not happened
        /// </summary>
        WTF = Error,

        /// <summary>
        /// Very important message
        /// </summary>
        VIP = Warning,

        /// <summary>
        /// Additional commentary
        /// </summary>
        NVM = Trace,        
    }
}
