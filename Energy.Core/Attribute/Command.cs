using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for command line arguments
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Switch attribute
        /// </summary>
        public class SwitchAttribute : System.Attribute
        {
            public string Name;

            public string Description;

            /// <summary>
            /// Table name
            /// </summary>
            public SwitchAttribute()
            {
            }

            /// <summary>
            /// Table name
            /// </summary>
            /// <param name="name"></param>
            public SwitchAttribute(string name)
            {
                this.Name = name;
            }

            /// <summary>
            /// Table name with description
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            public SwitchAttribute(string name, string description)
                : this(name)
            {
                this.Description = name;
            }
        }

        /// <summary>
        /// Parameter attribute
        /// </summary>
        public class ParameterAttribute : System.Attribute
        {
            public string Name;

            public string Description;

            /// <summary>
            /// Parameter
            /// </summary>
            public ParameterAttribute()
            {
            }

            /// <summary>
            /// Parameter
            /// </summary>
            /// <param name="name"></param>
            public ParameterAttribute(string name)
            {
                this.Name = name;
            }

            /// <summary>
            /// Parameter
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            public ParameterAttribute(string name, string description)
                : this(name)
            {
                this.Description = name;
            }
        }
    }
}
