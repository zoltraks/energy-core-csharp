using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for application
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Attribute for application module.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class ModuleAttribute : System.Attribute
        {
            /// <summary>
            /// Module name
            /// </summary>
            public string Name;

            /// <summary>
            /// Constructor
            /// </summary>
            public ModuleAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            public ModuleAttribute(string name)
            {
                this.Name = name;
            }
        }

        /// <summary>
        /// Attribute for application plugin.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class PluginAttribute : System.Attribute
        {
            /// <summary>
            /// Module name
            /// </summary>
            public string Name;

            /// <summary>
            /// Constructor
            /// </summary>
            public PluginAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            public PluginAttribute(string name)
            {
                this.Name = name;
            }
        }

        /// <summary>
        /// Attribute for application extension.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class ExtensionAttribute : System.Attribute
        {
            /// <summary>
            /// Module name
            /// </summary>
            public string Name;

            /// <summary>
            /// Constructor
            /// </summary>
            public ExtensionAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            public ExtensionAttribute(string name)
            {
                this.Name = name;
            }
        }

        /// <summary>
        /// Attribute for application library.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class LibraryAttribute : System.Attribute
        {
            /// <summary>
            /// Module name
            /// </summary>
            public string Name;

            /// <summary>
            /// Constructor
            /// </summary>
            public LibraryAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            public LibraryAttribute(string name)
            {
                this.Name = name;
            }
        }
    }
}
