using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing boolean values
    /// </summary>
    public enum BooleanStyle
    {
        /// <summary>
        /// X for true
        /// </summary>
        X,
        /// <summary>
        /// V for true
        /// </summary>
        V,
        /// <summary>
        /// 0/1
        /// </summary>
        B,
        /// <summary>
        /// Y/N
        /// </summary>
        Y,
        /// <summary>
        /// T/F
        /// </summary>
        T,

        /// <summary>
        /// Yes/No
        /// </summary>
        /// <remarks>Localised</remarks>
        YesNo,
        /// <summary>
        /// True/False
        /// </summary>
        /// <remarks>Localised</remarks>
        TrueFalse,
        /// <summary>
        /// 0/1
        /// </summary>
        /// <remarks>Localised</remarks>
        Bit = B,
        /// <summary>
        /// Y/N
        /// </summary>
        /// <remarks>Localised</remarks>
        YN = Y,
        /// <summary>
        /// T/F
        /// </summary>
        /// <remarks>Localised</remarks>
        TF = T,
    }
}
