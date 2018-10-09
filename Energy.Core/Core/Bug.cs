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

        /// <summary>
        /// Prefix with code number when writing to System.Diagnostics.Debug
        /// </summary>
        public static Energy.Base.Switch DebugOutputCode;

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
        static Bug()
        {
            //System.Diagnostics.Debug.WriteLine("BUG");
            TraceLogging = false;
            DebugOutputTime = true;
            DebugOutputCode = false;
        }

        #endregion

        #region Class

        public class Class
        {
            #region SuppressCodeList

            public class SuppressCodeList : List<SuppressCodeList.Item>
            {
                private static SuppressCodeList _Default;
                private static readonly object _DefaultLock = new object();
                /// <summary>Singleton</summary>
                public static SuppressCodeList Default
                {
                    get
                    {
                        if (_Default == null)
                        {
                            lock (typeof(SuppressCodeList))
                            {
                                if (_Default == null)
                                {
                                    _Default = new SuppressCodeList();
                                }
                            }
                        }
                        return _Default;
                    }
                }

                public class Item
                {
                    public Code Code;

                    public bool Suppress;
                }

                public Item Find(Code code, bool ignoreCase)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (code.Match(this[i].Code, ignoreCase))
                            return this[i];
                    }
                    return null;
                }

                public Item Find(Code code)
                {
                    return Find(code, true);
                }

                public bool IsSuppressed(Code code)
                {
                    Item item = Find(code, true);
                    if (item == null)
                    {
                        return false;
                    }
                    else
                    {
                        return item.Suppress;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Code

        /// <summary>
        /// Bug message code. May be numbered, litereal or both.
        /// </summary>
        public struct Code
        {
            public string Number;

            public Code(string text)
            {
                Number = text;
            }

            public Code(long number)
            {
                Number = number.ToString();
            }

            public static implicit operator Code(long number)
            {
                return new Code(number);
            }

            public static implicit operator Code(string number)
            {
                return new Code(number);
            }

            public bool Match(Code code)
            {
                return Match(code, true);
            }

            public bool Match(Code code, bool ignoreCase)
            {
                if (0 == string.Compare(this.Number, code.Number))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region Entry

        /// <summary>
        /// Bug message entry.
        /// </summary>
        public struct Entry
        {
            public Code Code;
            public string Message;

            public Entry(string message)
            {
                this.Code = default(Code);
                this.Message = message;
            }

            public Entry(Code code, string message)
            {
                this.Code = code;
                this.Message = message;
            }
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

        #region Static

        private static Class.SuppressCodeList SuppressCodeList
        {
            get
            {
                return Class.SuppressCodeList.Default;
            }
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

        #region Last

        private static Entry _Last;

        private readonly static object _LastLock = new object();

        /// <summary>
        /// Last entry
        /// </summary>
        public static Entry Last
        {
            get
            {
                lock (_LastLock)
                {
                    return _Last;
                }
            }
            set
            {
                lock (_LastLock)
                {
                    _Last = value;
                }
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
            Last = new Entry(message);
        }

        /// <summary>
        /// Write debug message with numeric code which may be suppressed or limited.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public static void Write(Code code, string message)
        {
            if (IsSuppressed(code))
            {
                return;
            }
            string debugMessage = message;
            if ((bool)DebugOutputCode)
            {
                debugMessage = (code.Number + " " + debugMessage).Trim();
            }
            debugMessage = FormatDebugOutput(debugMessage);
            System.Diagnostics.Debug.WriteLine(debugMessage);
            if ((bool)TraceLogging)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
            Last = new Entry(code, message);
        }

        /// <summary>
        /// Write debug message with numeric code which may be suppressed or limited.
        /// Provide a function that returns message and will be invoked only if not suppressed.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="action"></param>
        public static void Write(Code code, Energy.Base.Anonymous.String action)
        {
            if (IsSuppressed(code))
            {
                return;
            }
            string message = null;
            try
            {
                message = action();
            }
            catch { }
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)TraceLogging)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
            Last = new Entry(code, message);
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
            Write(string.Format(format, args));
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
            Write(message);
        }

        #endregion

        #region Trap

        /// <summary>
        /// Create time trap for execution. While disposed, it will invoke optional action.
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
        /// Create time trap for execution. While disposed, it will invoke optional action.
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

        #region Suppress

        /// <summary>
        /// Suppress message identified by code.
        /// </summary>
        /// <param name="code"></param>
        public static void Suppress(Code code)
        {
            Class.SuppressCodeList.Item item = SuppressCodeList.Find(code);
            if (item == null)
            {
                item = new Class.SuppressCodeList.Item()
                {
                    Code = code,
                    Suppress = true,
                };
                SuppressCodeList.Add(item);
            }
        }

        /// <summary>
        /// Suppress message identified by code.
        /// Optionaly "unsuppress" by calling with suppress:false parameter.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="suppress"></param>
        public static void Suppress(Code code, bool suppress)
        {
            Class.SuppressCodeList.Item item = SuppressCodeList.Find(code);
            if (item == null)
            {
                item = new Class.SuppressCodeList.Item()
                {
                    Code = code,
                    Suppress = suppress,
                };
                SuppressCodeList.Add(item);
            }
            else
            {
                item.Suppress = suppress;
            }
        }

        public static bool IsSuppressed(Code code)
        {
            return SuppressCodeList.IsSuppressed(code);
        }

        #endregion
    }
}
