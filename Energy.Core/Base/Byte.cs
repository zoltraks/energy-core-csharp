using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Byte related functions
    /// </summary>
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
        public static ushort Reverse(ushort value)
        {
            return (ushort)((byte)(value / 256) + (value % 256) * 256);
        }

        /// <summary>
        /// Reverse order of bytes
        /// </summary>
        /// <param name="value">Unsigned int (32-bit)</param>
        /// <returns>Reversed numeric value</returns>
        public static ulong Reverse(uint value)
        {
            byte b1 = (byte)((value >> 0) & 0xff);
            byte b2 = (byte)((value >> 8) & 0xff);
            byte b3 = (byte)((value >> 16) & 0xff);
            byte b4 = (byte)((value >> 24) & 0xff);

            return (ulong)(b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0);
        }
    }
}