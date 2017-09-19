using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Three state type of sort order (None/Ascending/Descending).
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// X for true
        /// </summary>
        None = 0,
        /// <summary>
        /// V for true
        /// </summary>
        Ascending = 1,
        /// <summary>
        /// 0/1
        /// </summary>
        Descending = 2,
    }
}
