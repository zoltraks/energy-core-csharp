using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing boolean values.
    /// </summary>
    public enum RoundingMethod
    {
        /// <summary>
        /// Standard method of rounding (HalfUp)
        /// </summary>
        Standard = HalfUp,
        /// <summary>
        /// None
        /// </summary>
        None = Floor,
        /// <summary>
        /// Round down
        /// </summary>
        Floor = 0,
        /// <summary>
        /// Round up
        /// </summary>
        Ceil = 0,
        /// <summary>
        /// Half Round Up (the common method of rounding)
        /// Example: 7.6 rounds up to 8, 7.5 rounds up to 8, 7.4 rounds down to 7.
        /// </summary>
        HalfUp = 4,
        /// <summary>
        /// X for true
        /// </summary>
        HalfDown = 5,
        /// <summary>
        /// Round to Even (Banker's Rounding)
        /// Example: 7.5 rounds up to 8 (because 8 is an even number) but 6.5 rounds down to 6 (because 6 is an even number).
        /// </summary>
        ToEven = 2,
        /// <summary>
        /// Round to Odd.
        /// Example: 7.5 rounds down to 7 (because 7 is an odd number) but 6.5 rounds up to 7 (because 7 is an odd number).
        /// </summary>
        ToOdd = 3,
    }
}
