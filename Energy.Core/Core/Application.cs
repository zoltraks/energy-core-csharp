using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Energy.Interface;

namespace Energy.Core
{
    /// <summary>
    /// Application
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Application name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Application assembly
        /// </summary>
        public System.Reflection.Assembly Assembly { get; private set; }

        /// <summary>
        /// Application directory
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Application syntax
        /// </summary>
        public Energy.Core.Syntax Syntax { get; set; }

        /// <summary>
        /// Application logger
        /// </summary>
        public Energy.Core.Log Log { get; set; }

        /// <summary>
        /// Application configuration
        /// </summary>
        public Energy.Core.Configuration Configuration { get; set; }

        /// <summary>
        /// Application connection
        /// </summary>
        public Energy.Source.Connection Connection { get; set; }

        /// <summary>
        /// Application locale
        /// </summary>
        public Energy.Core.Locale Locale { get; set; }

        private ICommandProgram _CommandProgram { get; set; }

        public string[] Arguments { get; private set; }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Application()
        {
            // create //
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Application(System.Type type)
            : base()
        {
            // create //
            if (type == null)
                return;
            if (null != Energy.Base.Class.GetClassInterface(type, typeof(Energy.Interface.ICommandProgram)))
            {
                Energy.Interface.ICommandProgram app = Activator.CreateInstance(type)
                    as Energy.Interface.ICommandProgram;
                this._CommandProgram = app;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public Application(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assembly"></param>
        public Application(System.Reflection.Assembly assembly)
        {
            Name = assembly.FullName;
            Directory = Information.GetAssemblyDirectory(assembly);
        }

        #endregion

        #region Create

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public static Application Create()
        {
            Application application = new Application();
            return application;
        }

        #endregion

        #region Run

        /// <summary>
        /// Run
        /// </summary>
        public bool Run()
        {
            try
            {
                string[] args = this.Arguments;
                if (_CommandProgram != null)
                {
                    return Run(_CommandProgram, args);
                }
                return true;
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Write(exception);
            }
            return false;
        }

        /// <summary>
        /// Run
        /// </summary>
        public bool Run(ICommandProgram commandProgram, string[] args)
        {
            try
            {
                if (commandProgram != null)
                {
                    if (!commandProgram.Setup(args))
                        return false;
                    if (!commandProgram.Initialize(args))
                        return false;
                    if (!commandProgram.Run(args))
                        return false;
                }
            }
            catch (Exception fallBackException)
            {
                Energy.Core.Log.Default.Write(fallBackException);
                throw;
            }

            return true;
        }

        #endregion

        #region Static

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

        public static System.Globalization.CultureInfo SetDefaultLanguage()
        {
            return SetLanguage("en-US");
        }

        public static System.Globalization.CultureInfo GetDefaultCultureInfo()
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

        public static string GetExecutionPath(System.Reflection.Assembly assembly)
        {
            if (assembly == null)
                return null;
            return System.IO.Path.GetDirectoryName(assembly.Location);
        }

        public static string GetExecutionPath()
        {
            return GetExecutionPath(System.Reflection.Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}
