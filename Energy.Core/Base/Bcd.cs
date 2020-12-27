namespace Energy.Base
{
    /// <summary>
    /// Binary-coded decimal (BCD) class
    /// </summary>
    public class Bcd
    {
        /// <summary>
        /// Convert BCD value to byte
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Byte value</returns>
        public static byte ToByte(byte value)
        {
            return (byte)(10 * (value >> 4) + (value & 0xf));
        }

        /// <summary>
        /// Convert byte to BCD value
        /// </summary>
        /// <param name="value">Byte value</param>
        /// <returns>BCD value</returns>
        public static byte FromByte(byte value)
        {
            return (byte)(((value / 10 % 10) << 4) + value % 10);
        }

        /// <summary>
        /// Convert BCD value to word
        /// </summary>
        /// <param name="value">BCD value</param>
        /// <returns>Word value</returns>
        public static ushort ToWord(ushort value)
        {
            return (ushort)(1000 * ((value >> 12) & 0xf) + 100 * ((value >> 8) & 0xf) + 10 * ((value >> 4) & 0xf) + (value & 0xf));
        }

        /// <summary>
        /// Convert word to BCD value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort FromWord(ushort value)
        {
            return (ushort)(((value / 1000 % 10) << 12) + ((value / 100 % 10) << 8) + ((value / 10 % 10) << 4) + value % 10);
        }
    }
}
