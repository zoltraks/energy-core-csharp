using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Basic grammatical gender type in masculine–feminine–neuter system
    /// </summary>
    public enum GrammaticalGender
    {
        /// <summary>
        /// Masculine
        /// </summary>
        Masculine,

        /// <summary>
        /// Feminine
        /// </summary>
        Feminine,

        /// <summary>
        /// Neuter
        /// </summary>
        Neuter,

        /// <summary>
        /// Short form of Masculine
        /// </summary>
        M = Masculine,

        /// <summary>
        /// Short form of Feminine
        /// </summary>
        F = Feminine,

        /// <summary>
        /// Short form of Neuter
        /// </summary>
        N = Neuter,
    }
}
