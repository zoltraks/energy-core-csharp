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

        ///// <summary>
        ///// Format as boolean value
        ///// </summary>
        //Bool = 2,

        /// <summary>
        /// Format as number
        /// </summary>
        Number = 4,

        /// <summary>
        /// Format as integer number
        /// </summary>
        Integer = 8,

        /// <summary>
        /// Format as date
        /// </summary>
        Date = 16,

        /// <summary>
        /// Format as time
        /// </summary>
        Time = 32,

        /// <summary>
        /// Format as timestamp
        /// </summary>
        Stamp = 64,

        /// <summary>
        /// Format as binary
        /// </summary>
        Binary = 128,
    }
}
