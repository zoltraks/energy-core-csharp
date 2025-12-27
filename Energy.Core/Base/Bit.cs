using System;

namespace Energy.Base
{
    /// <summary>
    /// Collection of binary utility functions.
    /// </summary>
    [Obsolete("Class moved to Energy.Base.Binary")]
    public class Bit
    {

        #region Reverse

        /// <summary>
        /// Reverse order of bytes.
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
        /// Reverse order of bytes.
        /// </summary>
        /// <param name="value">Unsigned int (32-bit)</param>
        /// <returns>Reversed numeric value</returns>
        [Obsolete("Use Energy.Base.Binary.Reverse")]
        public static uint Reverse(uint value)
        {
            return Energy.Base.Binary.Reverse(value);
        }

        #endregion

        #region Compare

        /// <summary>
        /// Compare byte arrays.
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

        #region Endianess

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// MSB / Big-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.GetUInt32MSB")]
        public static UInt32 GetUInt32MSB(params UInt16[] array)
        {
            return Energy.Base.Binary.GetUInt32MSB(array);
        }

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// LSB / Little-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.GetUInt32LSB")]
        public static UInt32 GetUInt32LSB(params UInt16[] array)
        {
            return Energy.Base.Binary.GetUInt32LSB(array);
        }

        #endregion

        #region Not

        /// <summary>
        /// Perform bitwise NOT operation on every byte in array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Not")]
        public static byte[] Not(byte[] array)
        {
            return Energy.Base.Binary.Not(array);
        }

        #endregion

        #region Or

        /// <summary>
        /// Perform bitwise OR operation on every byte in array by second array.
        /// <br /><br />
        /// Second array is treated as ring buffer when shorter than first one.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Or")]
        public static byte[] Or(byte[] one, byte[] two)
        {
            return Energy.Base.Binary.Or(one, two);
        }

        #endregion

        #region And

        /// <summary>
        /// Perform bitwise AND operation on every byte in array by second array.
        /// <br /><br />
        /// Second array is treated as ring buffer when shorter than first one.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.And")]
        public static byte[] And(byte[] one, byte[] two)
        {
            return Energy.Base.Binary.And(one, two);
        }

        #endregion

        #region Xor

        /// <summary>
        /// Perform bitwise XOR operation on every byte in array by second array.
        /// <br /><br />
        /// Second array is treated as ring buffer when shorter than first one.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Xor")]
        public static byte[] Xor(byte[] one, byte[] two)
        {
            return Energy.Base.Binary.Xor(one, two);
        }

        #endregion

        #region Rol

        /// <summary>
        /// Rotate bits left in an array by given bit count.
        /// <br /><br />
        /// When negative number of bits is given, right rotation will be performed instead.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Rol")]
        public static byte[] Rol(byte[] array, int count)
        {
            return Energy.Base.Binary.Rol(array, count);
        }

        #endregion

        #region Ror

        /// <summary>
        /// Rotate bits right in an array by given bit count.
        /// <br /><br />
        /// When negative number of bits is given, left rotation will be performed instead.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Obsolete("Use Energy.Base.Binary.Ror")]
        public static byte[] Ror(byte[] array, int count)
        {
            return Energy.Base.Binary.Ror(array, count);
        }

        #endregion
    }
}