using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Matching style
    /// </summary>
    public enum MatchMode
    {
        /// <summary>
        /// Default value for not matching at all
        /// </summary>
        None,

        /// <summary>
        /// Values must be same in their text representation
        /// </summary>
        Same,

        /// <summary>
        /// Match simple way. Value must be equal to filtered, start or end with to be matched.
        /// </summary>
        Simple,

        /// <summary>
        /// Match using shell style wildcards (*?)
        /// </summary>
        Wild,

        /// <summary>
        /// Match using regular expressions
        /// </summary>
        Regex,
    }
}
