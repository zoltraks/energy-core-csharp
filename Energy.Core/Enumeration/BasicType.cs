using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    public enum BasicType
    {
        /// <summary>
        /// Text value
        /// </summary>
        Text,

        /// <summary>
        /// Any value which is number
        /// </summary>
        Number,

        /// <summary>
        /// Integer numbers only
        /// </summary>
        Integer,

        /// <summary>
        /// True / False
        /// </summary>
        Bool,

        /// <summary>
        /// Date
        /// </summary>
        Date,

        /// <summary>
        /// Time
        /// </summary>
        Time,

        /// <summary>
        /// Date and time
        /// </summary>
        Stamp,
    }
}
