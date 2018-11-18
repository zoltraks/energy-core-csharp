using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for specyfing custom values
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Attribute for application module.
        /// </summary>
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class DictionaryAttribute : System.Attribute
        {
            /// <summary>
            /// Key
            /// </summary>
            public object Key;

            /// <summary>
            /// Value
            /// </summary>
            public object Value;

            /// <summary>
            /// Constructor
            /// </summary>
            public DictionaryAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public DictionaryAttribute(object key, object value)
            {
                this.Key = key;
                this.Value = key;
            }
        }
    }
}
