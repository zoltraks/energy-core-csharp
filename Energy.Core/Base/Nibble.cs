using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Four-bit aggregation of byte.
    /// </summary>
    public class Nibble
    {

        /// <summary>
        /// Get high part of byte (first nibble).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte High(byte value)
        {
            return (byte)(value >> 4);
        }

        /// <summary>
        /// Get low part of byte (second nibble).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Low(byte value)
        {
            return (byte)(value & 0xf);
        }

        /// <summary>
        /// Reverse nibbles in byte.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Reverse(byte value)
        {
            return (byte)((byte)((byte)(value & 0xf) << 4) + (byte)(value >> 4));
        }
    }
}
