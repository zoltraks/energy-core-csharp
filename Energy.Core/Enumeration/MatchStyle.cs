using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Matching style
    /// </summary>
    public enum MatchStyle
    {
        /// <summary>
        /// At least one element has to be resolved
        /// </summary>
        Any,

        /// <summary>
        /// All elements must be resolved
        /// </summary>
        All,

        /// <summary>
        /// Only one element need to be resolved
        /// </summary>
        One,

        /// <summary>
        /// Every element must not be resolved
        /// </summary>
        Not,
    }
}
