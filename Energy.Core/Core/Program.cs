using System;

namespace Energy.Core
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        #region GetAssembly

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
                {
                    return assembly;
                }
            }
            catch
            { }

            try
            {
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if (null != assembly)
                {
                    return assembly;
                }
            }
            catch
            { }

            return null;
        }

        #endregion

        #region GetExecutionFile

        /// <summary>
        /// Get execution file location from current working assembly (calling or executing).
        /// </summary>
        /// <returns></returns>
        public static string GetExecutionFile()
        {
            System.Reflection.Assembly assembly = null;

            try
            {
                if (null == assembly)
                {
                    assembly = System.Reflection.Assembly.GetCallingAssembly();
                }
            }
            catch { }

            try
            {
                if (null == assembly)
                {
                    assembly = System.Reflection.Assembly.GetExecutingAssembly();
                }
            }
            catch { }

            return Energy.Base.Class.GetAssemblyFile(assembly);
        }

        #endregion

        #region GetExecutionDirectory

        /// <summary>
        /// Get execution directory from the assembly location.
        /// <br /><br />
        /// Resulting directory will contain trailing path separator.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetExecutionDirectory(System.Reflection.Assembly assembly)
        {
            if (null == assembly)
            {
                return null;
            }
            string file, directory;
            file = Energy.Base.Class.GetAssemblyFile(assembly);
            directory = System.IO.Path.GetDirectoryName(file);
            directory = Energy.Base.Path.IncludeTrailingSeparator(directory);
            return directory;
        }

        /// <summary>
        /// Get execution directory from current executing assembly location.
        /// <br /><br />
        /// Resulting directory will contain trailing path separator.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutionDirectory()
        {
            System.Reflection.Assembly assembly;
            try
            {
                assembly = System.Reflection.Assembly.GetCallingAssembly();
                if (null != assembly)
                {
                    return GetExecutionDirectory(assembly);
                }
            }
            catch
            { }
            try
            {
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if (null != assembly)
                {
                    return GetExecutionDirectory(assembly);
                }
            }
            catch
            { }
            return null;
        }

        #endregion

        #region GetExecutionPath

        /// <summary>
        /// Get execution directory from the assembly location.
        /// <br /><br />
        /// Resulting directory will not contain trailing path separator.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetExecutionPath(System.Reflection.Assembly assembly)
        {
            if (null == assembly)
            {
                return null;
            }
            string file, directory;
            file = Energy.Base.Class.GetAssemblyFile(assembly);
            directory = System.IO.Path.GetDirectoryName(file);
            return directory;
        }

        /// <summary>
        /// Get execution directory from current executing assembly location.
        /// <br /><br />
        /// Resulting directory will not contain trailing path separator.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutionPath()
        {
            System.Reflection.Assembly assembly;
            try
            {
                assembly = System.Reflection.Assembly.GetCallingAssembly();
                if (null != assembly)
                {
                    return GetExecutionPath(assembly);
                }
            }
            catch
            { }
            try
            {
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if (null != assembly)
                {
                    return GetExecutionPath(assembly);
                }
            }
            catch
            { }
            return null;
        }

        #endregion

        #region GetCommandName

        /// <summary>
        /// Get short command name from assembly location.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetCommandName(System.Reflection.Assembly assembly)
        {
            try
            {
                string location = Energy.Base.Class.GetAssemblyFile(assembly);
                return System.IO.Path.GetFileNameWithoutExtension(location);
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
            System.Reflection.Assembly assembly;

            try
            {
                assembly = System.Reflection.Assembly.GetCallingAssembly();
                if (null != assembly)
                {
                    return GetCommandName(assembly);
                }
            }
            catch
            { }

            try
            {
                assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if (null != assembly)
                {
                    return GetCommandName(assembly);
                }
            }
            catch
            { }

            return null;
        }

        #endregion

        #region SetLanguage

        /// <summary>
        /// Set specified language for program.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static System.Globalization.CultureInfo SetLanguage(string culture)
        {
            try
            {
                System.Globalization.CultureInfo cultureInfo;
                cultureInfo = new System.Globalization.CultureInfo(culture);
#if !NETCF
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
#endif
#if NETCF
                // Compact Framework does not allow for changing CultureInfo of UI at runtime.
#endif
                return cultureInfo;
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Write(exception);
                throw;
            }
        }

        /// <summary>
        /// Set default language for program (en-US).
        /// </summary>
        /// <returns></returns>
        public static System.Globalization.CultureInfo SetLanguage()
        {
            return SetLanguage("en-US");
        }

        #endregion

        #region GetCultureInfo

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

        #endregion

        #region SetConsoleEncoding

        /// <summary>
        /// Set specified encoding for console.
        /// </summary>
        /// <param name="encoding"></param>
        public static System.Text.Encoding SetConsoleEncoding(System.Text.Encoding encoding)
        {
#if !NETCF
            try
            {
                Console.InputEncoding = encoding;
                Console.OutputEncoding = encoding;
            }
            catch (System.Security.SecurityException)
            { }
            catch (Exception x)
            {
                Energy.Core.Bug.Catch("Energy.Core.Program.SetConsoleEncoding", x);
            }
#endif
            return encoding;
        }

        /// <summary>
        /// Set specified encoding for console.
        /// </summary>
        /// <param name="encoding"></param>
        public static System.Text.Encoding SetConsoleEncoding(string encoding)
        {
            return SetConsoleEncoding(Energy.Base.Text.Encoding(encoding));
        }

        /// <summary>
        /// Set encoding for console to UTF-8.
        /// </summary>
        public static System.Text.Encoding SetConsoleEncoding()
        {
            return SetConsoleEncoding(System.Text.Encoding.UTF8);
        }

        #endregion
    }
}
