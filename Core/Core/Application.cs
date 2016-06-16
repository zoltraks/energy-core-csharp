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
        public Variable Syntax { get; set; }

        private volatile Log _Log;
        
        public Log Log
        { 
            get
            {
                if (_Log == null)
                {
                    lock (typeof(Log))
                    {
                        if (_Log == null)
                        {
                            _Log = new Log();
                        }
                    }
                }
                return _Log;
            }
            private set 
            {
                _Log = value; 
            } 
        }

        public Energy.Core.Configuration Configuration { get; set; }

        public Energy.Source.Connection Connection { get; set; }

        public Energy.Core.Locale Locale { get; set; }

        public Application()
        {
            // create //
        }

        public Application(string name)
        {
            Name = name;
        }

        public Application(System.Reflection.Assembly assembly)
        {
            Name = assembly.FullName;
            Directory = Information.GetAssemblyDirectory(assembly);
        }

        public Application Create()
        {
            Application application = new Application();
            return application;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
