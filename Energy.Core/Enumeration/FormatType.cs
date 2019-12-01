using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Value representation format
    /// </summary>
    public enum FormatType
    {
        /// <summary>
        /// Format as text
        /// </summary>
        Text,

        /// <summary>
        /// Format as number
        /// </summary>
        Number,

        /// <summary>
        /// Format as date
        /// </summary>
        Date,

        /// <summary>
        /// Format as time
        /// </summary>
        Time,

        /// <summary>
        /// Format as timestamp
        /// </summary>
        Stamp,

        /// <summary>
        /// Format as binary
        /// </summary>
        Binary,
    }
}
