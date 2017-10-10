using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing boolean values
    /// </summary>
    public enum LocateBehaviour
    {
        /// <summary>
        /// Search in any directory for every possible filename extension
        /// </summary>
        Directories,

        /// <summary>
        /// Search for every possible filename extension in any of directory
        /// </summary>
        Extensions,

        /// <summary>
        /// Default behaviour
        /// </summary>
        Default = Directories,
    }
}
