using System;
using System.Collections.Generic;
using System.Text;

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
        public void Run()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Static

        public static void SetLanguage(string culture)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            }
            catch
            { }
        }

        public static void SetDefaultLanguage()
        {
            SetLanguage("en-US");
        }

        #endregion
    }
}
