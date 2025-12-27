using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Byte related functions
    /// </summary>
    [Obsolete("Class moved to Energy.Base.Binary")]
    public class Byte
    {
        /// <summary>
        /// Reverse order of bytes
        /// <para>
        /// Exchange lower byte with higher one
        /// </para>
        /// </summary>
        /// <param name="value">Unsigned short (16-bit)</param>
        /// <returns>Reversed numeric value</returns>
        [Obsolete("Use Energy.Base.Binary.Reverse")]
        public static ushort Reverse(ushort value)
        {
            return Energy.Base.Binary.Reverse(value);
        }

        /// <summary>
        /// Reverse order of bytes
        /// </summary>
        /// <param name="value">Unsigned int (32-bit)</param>
        /// <returns>Reversed numeric value</returns>
        [Obsolete("Use Energy.Base.Binary.Reverse")]
        public static uint Reverse(uint value)
        {
            return Energy.Base.Binary.Reverse(value);
        }

        #region Compare

        /// <summary>
        /// Compare byte arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Compare")]
        public static int Compare(byte[] left, byte[] right)
        {
            return Energy.Base.Binary.Compare(left, right);
        }

        /// <summary>
        /// Compare int arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Compare")]
        public static int Compare(int[] left, int[] right)
        {
            return Energy.Base.Binary.Compare(left, right);
        }

        /// <summary>
        /// Compare long arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Compare")]
        public static int Compare(long[] left, long[] right)
        {
            return Energy.Base.Binary.Compare(left, right);
        }

        /// <summary>
        /// Compare double arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Compare")]
        public static int Compare(double[] left, double[] right)
        {
            return Energy.Base.Binary.Compare(left, right);
        }

        /// <summary>
        /// Compare decimal arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Compare")]
        public static int Compare(decimal[] left, decimal[] right)
        {
            return Energy.Base.Binary.Compare(left, right);
        }

        #endregion
    }
}