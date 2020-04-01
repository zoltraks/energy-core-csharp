using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Value representation format
    /// </summary>
    //[Flags]
    public enum FormatType
    {
        /// <summary>
        /// Format as is
        /// </summary>
        Plain = 0,

        /// <summary>
        /// Format as text
        /// </summary>
        Text = 1,

        /// <summary>
        /// Format as number
        /// </summary>
        Number = 2,

        /// <summary>
        /// Format as date
        /// </summary>
        Date = 4,

        /// <summary>
        /// Format as time
        /// </summary>
        Time = 8,

        /// <summary>
        /// Format as timestamp
        /// </summary>
        Stamp = 16,

        /// <summary>
        /// Format as binary
        /// </summary>
        Binary = 32,
    }
}
