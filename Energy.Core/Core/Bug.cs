using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;

namespace Energy.Core
{
    public class Bug
    {
        /// <summary>
        /// Trace switch
        /// </summary>
        public readonly static Energy.Base.Switch Trace = false;

        /// <summary>
        /// Use time when writing to System.Diagnostics.Debug
        /// </summary>
        public readonly static Energy.Base.Switch DebugOutputTime = true;

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

            return String.Join(Environment.NewLine, message.ToArray()).Trim();
        }

        /// <summary>
        /// Return exception message
        /// </summary>
        /// <param name="exception">Exception object</param>
        /// <returns>string</returns>
        public static string ExceptionMessage(Exception exception)
        {
            return ExceptionMessage(exception, (bool)Trace);
        }

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

        public static string FormatDebugOutput(string message)
        {
            if ((bool)DebugOutputTime)
            {
                message = string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss.fff"), message);
            }
            return message;
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="exception"></param>
        public static void Catch(Exception exception)
        {
            string message = ExceptionMessage(exception, true);
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)Trace)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
        }

        /// <summary>
        /// Write debug message
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormatDebugOutput(message));
            if ((bool)Trace)
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
            if ((bool)Trace)
            {
                Energy.Core.Log.Default.Write(message, Enumeration.LogLevel.Bug);
            }
        }
    }
}
