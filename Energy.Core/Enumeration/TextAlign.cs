using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing boolean values
    /// </summary>
    public enum TextAlign
    {
        /// <summary>
        /// Don't use alignment
        /// </summary>
        None,

        /// <summary>
        /// Left alignment
        /// </summary>
        Left,

        /// <summary>
        /// Right alignment
        /// </summary>
        Right,

        /// <summary>
        /// Center alignment
        /// </summary>
        Center,

        /// <summary>
        /// Alias for center alignment
        /// </summary>
        Middle = Center,

        /// <summary>
        /// Justify content
        /// </summary>
        Justify,
    }
}
