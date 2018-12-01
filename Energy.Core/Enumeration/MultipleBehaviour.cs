using System;

namespace Energy.Enumeration
{
    /// <summary>
        /// Selection of duplicates behaviour.
        /// Specifies behaviour for selecting one element from multiple duplicates.
        /// The default behavior is to overwrite the value and thus taking the last value.
    /// </summary>
    public enum MultipleBehaviour
    {
        /// <summary>
        /// Unspecified behaviour.
        /// </summary>
        None,

        /// <summary>
        /// Take last from duplicates.
        /// Setting value will overwrite element if exists. 
        /// </summary>
        Last,

        /// <summary>
        /// Take first from duplicates.
        /// Value may be set only once. It will be ignored if element exists.
        /// </summary>
        First,
    }
}
