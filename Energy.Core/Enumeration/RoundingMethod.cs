using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Method of rounding numbers.
    /// </summary>
    public enum RoundingMethod
    {
        /// <summary>
        /// None
        /// </summary>
        None = Floor,

        /// <summary>
        /// Standard method of rounding (HalfUp)
        /// </summary>
        Standard = HalfUp,

        /// <summary>
        /// Round down
        /// </summary>
        Floor = 0,

        /// <summary>
        /// Round up
        /// </summary>
        Ceil = 0,

        /// <summary>
        /// Half Round Up
        /// This method is commonly used.
        /// Example: 7.6 rounds up to 8, 7.5 rounds up to 8, 7.4 rounds down to 7.
        /// However, some programming languages (such as Java, Python) define their half up as round half away from zero here.
        /// </summary>
        HalfUp = 4,

        /// <summary>
        /// Half Round Down
        /// Example: 7.6 rounds up to 8, 7.5 rounds down to 7, 7.4 rounds down to 7.
        /// Do you remember that "5/4" option in vintage calculators?
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
