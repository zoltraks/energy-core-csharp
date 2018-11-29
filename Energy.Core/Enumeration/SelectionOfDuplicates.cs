using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Specifies behaviour for selecting one element from multiple duplicates.
    /// </summary>
    public enum SelectionOfDuplicates
    {
        /// <summary>
        /// Unspecified behaviour.
        /// </summary>
        None,

        /// <summary>
        /// Take last from duplicates.
        /// </summary>
        Last,

        /// <summary>
        /// Take first from duplicates.
        /// </summary>
        First,
    }
}
