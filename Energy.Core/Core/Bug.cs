using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Energy.Core
{
    public class Bug
    {
        #region Property

        /// <summary>
        /// Trace switch
        /// </summary>
        public static Energy.Base.Switch TraceLogging;

        /// <summary>
        /// Use time when writing to System.Diagnostics.Debug
        /// </summary>
        public static Energy.Base.Switch DebugOutputTime;

        private static Energy.Core.Log _Log;
        /// <summary>Log</summary>
        public static Energy.Core.Log Log
        {
            get
            {
                if (_Log == null)
                {
                    return Core.Log.Default;
                }
                else
                {
                    return _Log;
                }
            }
            set
            {
                _Log = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Bug()
        {
            System.Diagnostics.Debug.WriteLine("BUG");
            TraceLogging = false;
            DebugOutputTime = true;
        }

        #endregion

        #region ExceptionMessage

        /// <summary>
        /// Return exception message
        /// </summary>
        /// <param name="exception">Exception object</param>
        /// <param name="trace">Include stack trace</param>
        /// <returns>string</returns>
        public static string ExceptionMessage(Exception exception, bool trace)
        {
            if (exception == null)
            {
                return "";
            }
            List<string> message = new List<string>();
            Exception x = exception;
            try
            {
                do
                {
                    message.Add(x.Message);
                    x = x.InnerException;
                }
                while (x != null);

                if (trace)
                {
                    if (exception.StackTrace != null)
                    {
                        message.Add("");
                        message.Add(new Regex(@"^\s*\w+\s*", RegexOptions.Multiline).Replace(exception.StackTrace, ""));
                    }
                    message.Add("");
                    //message.Add(exception.Data.ToString());
                    message.Add(exception.GetType().FullName);
                }
            }
            catch (Exception fault)
            {
                message.Add("");
                message.Add(fault.Message);
            }

            return string.Join(Energy.Base.Text.NL, message.ToArray()).Trim();
        }

        /// <summary>
        /// Return exception message
        /// </summary>
        /// <param name="exception">Exception object</param>
        /// <returns>string</returns>
        public static string ExceptionMessage(Exception exception)
        {
            return ExceptionMessage(exception, (bool)TraceLogging);
        }
      
        #endregion

        #region ThreadIdHex

        /// <summary>
        /// Get thread id as hex string.
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public static string ThreadIdHex(System.Threading.Thread thread)
        {
            int id = thread.ManagedThreadId;
            return Energy.Base.Hex.IntegerToHex(id);
        }

        /// <summary>
        /// Get current thread id as hex string.
        /// </summary>
        /// <returns></returns>
        public static string ThreadIdHex()
        {
            return ThreadIdHex(System.Threading.Thread.CurrentThread);
        }

        #endregion

        #region CallingMethod

        /// <summary>
        /// Return calling method name
        /// </summary>
        /// <param name="stack">int</param>
        /// <returns>string</returns>
        public static string CallingMethod(int stack)
        {
            try
            {
                string fullName = "";
                StackTrace stackTrace = new StackTrace();
                stack++;
                do
                {
                    MethodBase method = stackTrace.GetFrame(stack).GetMethod();
                    if (method == null)
                        return "";
                    if (method.Name == ".ctor")
                    {
                        fullName = method.ReflectedType.FullName;
                    }
                    else
                    {
                        fullName = string.Concat(method.ReflectedType.FullName, ".", method.Name);
                        if (fullName == "Energy.Core.Bug.CallingMethod")
                        {
                            stack++;
                            continue;
                        }
                    }
                } while (false);
                return fullName;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Return calling method name
        /// </summary>
        /// <returns>string</returns>
        public static string CallingMethod()
        {
            return CallingMethod(1);
        }

        #endregion

        #region FormatDebugOutput

        public static string FormatDebugOutput(string message)
        {
            if ((bool)DebugOutputTime)
            {
                message = string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss.fff"), message);
            }
            return message;
        }

        #endregion

        #region Catch

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="exception"></param>
        public static void Catch(Exception exception)
        {
            string message = ExceptionMessage(exception, true);
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)TraceLogging)
            {
                Log.Write(message, Enumeration.LogLevel.Bug);
            }
        }

        #endregion

        #region Write

        /// <summary>
        /// Write debug message
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)TraceLogging)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
        }

        /// <summary>
        /// Write debug message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Write(string format, params object[] args)
        {
            string message = string.Format(format, args);
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)TraceLogging)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
        }

        #endregion

        #region WriteFormat

        /// <summary>
        /// Write debug message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteFormat(string format, params object[] args)
        {
            string message = string.Format(format, args);
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)TraceLogging)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
        }

        /// <summary>
        /// Write debug message
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteFormat(IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(provider, format, args);
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)TraceLogging)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
        }

        #endregion

        #region Trap

        /// <summary>
        /// Create time trap for execution. While disposed, it Will invoke optional action.
        /// </summary>
        /// <param name="timeLimit">Time limit in seconds. When finished in shorter time, action will not be executed.</param>
        /// <param name="action">Action when time exceeds limit</param>
        /// <returns></returns>
        public static Energy.Base.Trap Trap(double timeLimit, Energy.Base.Anonymous.Function<TimeSpan> action)
        {
            Energy.Base.Trap trap = new Energy.Base.Trap(timeLimit, action);
            return trap;
        }

        /// <summary>
        /// Create time trap for execution. While disposed, it Will invoke optional action.
        /// </summary>
        /// <param name="timeLimit">Time limit in seconds. When finished in shorter time, action will not be executed.</param>
        /// <param name="action">Action when time exceeds limit</param>
        /// <returns></returns>
        public static Energy.Base.Trap Trap(double timeLimit, Energy.Base.Anonymous.Function action)
        {
            Energy.Base.Trap trap = new Energy.Base.Trap(timeLimit, action);
            return trap;
        }

        #endregion
    }
}
