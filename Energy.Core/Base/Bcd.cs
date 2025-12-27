using System;

namespace Energy.Base
{
    /// <summary>
    /// Binary-coded decimal (BCD) class
    /// </summary>
    [Obsolete("Class moved to Energy.Base.Binary")]
    public class Bcd
    {
        /// <summary>
        /// Convert BCD value to byte
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Byte value</returns>
        [Obsolete("Use Energy.Base.Binary.Bcd.ToByte")]
        public static byte ToByte(byte value)
        {
            return Energy.Base.Binary.Bcd.ToByte(value);
        }

        /// <summary>
        /// Convert byte to BCD value
        /// </summary>
        /// <param name="value">Byte value</param>
        /// <returns>BCD value</returns>
        [Obsolete("Use Energy.Base.Binary.Bcd.FromByte")]
        public static byte FromByte(byte value)
        {
            return Energy.Base.Binary.Bcd.FromByte(value);
        }

        /// <summary>
        /// Convert BCD value to word
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Word value</returns>
        [Obsolete("Use Energy.Base.Binary.Bcd.ToWord")]
        public static ushort ToWord(ushort value)
        {
            return Energy.Base.Binary.Bcd.ToWord(value);
        }

        /// <summary>
        /// Convert word to BCD value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Bcd.FromWord")]
        public static ushort FromWord(ushort value)
        {
            return Energy.Base.Binary.Bcd.FromWord(value);
        }
    }
}
