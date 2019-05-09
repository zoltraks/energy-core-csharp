using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        #region Utility

        /// <summary>
        /// Get current assembly from GetExecutingAssembly or GetCallingAssembly.
        /// This function will not throw any exception, returning null on any error.
        /// </summary>
        /// <returns></returns>
        public static System.Reflection.Assembly GetAssembly()
        {
            System.Reflection.Assembly assembly;

            try
            {
                assembly = System.Reflection.Assembly.GetCallingAssembly();
                if (null != assembly)
                    return assembly;
            }
            catch
            { }

            try
            {
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if (null != assembly)
                    return assembly;
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get execution file from current working assembly (calling or executing).
        /// </summary>
        /// <returns></returns>
        public static string GetExecutionFile()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            if (null == assembly)
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return assembly.Location;
        }

        /// <summary>
        /// Get execution directory name from the assembly location.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetExecutionPath(System.Reflection.Assembly assembly)
        {
            if (assembly == null)
                return null;
            return System.IO.Path.GetDirectoryName(assembly.Location);
        }

        /// <summary>
        /// Get execution directory name from executing assembly location.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutionPath()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            if (null == assembly)
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return System.IO.Path.GetDirectoryName(assembly.Location);
        }

        /// <summary>
        /// Get short command name from assembly location.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetCommandName(System.Reflection.Assembly assembly)
        {
            try
            {
                string location = assembly.Location;
                return Energy.Base.File.GetCommandName(location);
            }
            catch (NotSupportedException)
            {
                return "";
            }
        }

        /// <summary>
        /// Get short command name from current assembly location.
        /// </summary>
        /// <returns></returns>
        public static string GetCommandName()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            if (null == assembly)
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return GetCommandName(assembly);
        }

        public static System.Globalization.CultureInfo SetLanguage(string culture)
        {
            try
            {
                System.Globalization.CultureInfo cultureInfo;
                cultureInfo = new System.Globalization.CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
                return cultureInfo;
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Write(exception);
                throw;
            }
        }

        public static System.Globalization.CultureInfo SetLanguage()
        {
            return SetLanguage("en-US");
        }

        public static System.Globalization.CultureInfo GetCultureInfo()
        {
            System.Globalization.CultureInfo cultureInfo;
            try
            {
                cultureInfo = new System.Globalization.CultureInfo("en-US");
                return cultureInfo;
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Write("E015", exception);
                throw;
            }
        }

        public static void SetConsoleEncoding(System.Text.Encoding encoding)
        {
            try
            {
                Console.InputEncoding = encoding;
                Console.OutputEncoding = encoding;
            }
            catch (System.Security.SecurityException)
            { }
            catch (Exception x)
            {
                Energy.Core.Bug.Catch(x);
            }
        }

        public static void SetConsoleEncoding(string encoding)
        {
            SetConsoleEncoding(Energy.Base.Text.Encoding(encoding));
        }

        public static void SetConsoleEncoding()
        {
            SetConsoleEncoding(System.Text.Encoding.UTF8);
        }

        #endregion

    }
}
