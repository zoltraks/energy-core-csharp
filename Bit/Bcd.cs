using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Bit
{
    public class Bcd
    {
        /// <summary>
        /// Convert BCD value to byte.
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Byte value</returns>
        public static byte BcdToByte(byte value)
        {
            return (byte)(10 * (value >> 4) + (value & 0xf));
        }

        /// <summary>
        /// Convert byte to BCD value.
        /// </summary>
        /// <param name="value">Byte value</param>
        /// <returns>BCD value</returns>
        public static byte ByteToBcd(byte value)
        {
            return (byte)(((value / 10 % 10) << 4) + value % 10);
        }

        /// <summary>
        /// Convert BCD value to word.
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Word value</returns>
        public static ushort BcdToWord(ushort value)
        {
            return (ushort)(1000 * ((value >> 12) & 0xf) + 100 * ((value >> 8) & 0xf) + 10 * ((value >> 4) & 0xf) + (value & 0xf));
        }
    }
}
