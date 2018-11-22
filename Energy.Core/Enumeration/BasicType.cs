using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Basic data type.
    /// </summary>
    /// <remarks>
    /// Generalisation of the type of data used in data exchange systems, SQL databases, etc.
    /// The assumption is to use as few variants as possible, so there is no single character type.
    /// Instead, you should use a text type with a character limit. 
    /// If necessary, a single character can be treated as an integer. 
    /// An integer is both an integer and a negative number, as well as a real number.
    /// Integers are of course also general numbers. 
    /// Date and time are represented by 3 values depending on whether it is date (YYYY-MM-DD),
    /// time (hh:mm:ss) or full time stamp (YYYY-MM-DD hh:mm:ss).
    /// </remarks>
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
