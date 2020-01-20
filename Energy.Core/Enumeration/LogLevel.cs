using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing severity level of log messages
    /// </summary>
    /// <remarks>
    /// Severity is also a number between 0 and 90 which represents level of messages.
    /// Lower numbers are more important and higher less and default is 50.
    /// 
    /// Three basic levels are:
    /// 
    /// Energy.Enumeration.LogLevel.Message which is default
    /// Energy.Enumeration.LogLevel.Error for normal errors
    /// Energy.Enumeration.LogLevel.Verbose for verbose messages
    /// 
    /// </remarks>
    public enum LogLevel
    {
        /// <summary>
        /// Default severity level
        /// </summary>
        Default = Message,

        /// <summary>
        /// Uknown severity level / Not used
        /// </summary>
        None = 0,

        /// <summary>
        /// Critical error, can't continue
        /// </summary>
        Critical = 10,

        /// <summary>
        /// Execution stopped
        /// </summary>
        Stop = 15,

        /// <summary>
        /// Very serious error
        /// </summary>
        Fatal = 20,

        /// <summary>
        /// Alert / Fatal
        /// </summary>
        Alert = 25,

        /// <summary>
        /// Error
        /// </summary>
        Error = 30,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 40,

        /// <summary>
        /// Normal message
        /// </summary>
        Message = 50,

        /// <summary>
        /// Information message
        /// </summary>
        Information = 60,

        /// <summary>
        /// Be more verbose.
        /// </summary>
        /// <remarks>
        /// This level may include program execution details for advanced monitoring.
        /// </remarks>
        Verbose = 70,

        /// <summary>
        /// Debugging information
        /// </summary>
        /// <remarks>
        /// For debugging purposes.
        /// </remarks>
        Bug = 80,

        /// <summary>
        /// Very detailed information
        /// </summary>
        /// <remarks>
        /// Maximum possible level intended for diagnostic messages.
        /// Additional messages for tracing program execution.
        /// For development purposes.
        /// </remarks>
        Trace = 90,

        /// <summary>
        /// Alias for Information
        /// </summary>
        Info = Information,

        /// <summary>
        /// Alias for Warning
        /// </summary>
        Warn = Warning,

        /// <summary>
        /// OMG
        /// </summary>
        OMG = Alert,

        /// <summary>
        /// LOL
        /// </summary>
        LOL = Verbose,

        /// <summary>
        /// WTF
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
