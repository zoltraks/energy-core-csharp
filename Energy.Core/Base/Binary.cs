using System;

namespace Energy.Base
{
    /// <summary>
    /// Binary utility functions
    /// </summary>
    public class Binary
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
        public static ushort Reverse(ushort value)
        {
            return (ushort)((byte)(value / 256) + (value % 256) * 256);
        }

        /// <summary>
        /// Reverse order of bytes.
        /// </summary>
        /// <param name="value">Unsigned int (32-bit)</param>
        /// <returns>Reversed numeric value</returns>
        public static uint Reverse(uint value)
        {
            byte b1 = (byte)((value >> 0) & 0xff);
            byte b2 = (byte)((value >> 8) & 0xff);
            byte b3 = (byte)((value >> 16) & 0xff);
            byte b4 = (byte)((value >> 24) & 0xff);

            return (uint)(b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0);
        }

        #endregion

        #region Compare

        /// <summary>
        /// Compare byte arrays.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(byte[] left, byte[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare int arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(int[] left, int[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare long arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(long[] left, long[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare double arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(double[] left, double[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare decimal arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(decimal[] left, decimal[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        #endregion

        #region Endianess

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// MSB / Big-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt32 GetUInt32MSB(params UInt16[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return 0;
            }
            UInt32 result = 0;
            switch (array.Length)
            {
                case 1:
                    result = array[0];
                    break;
                default:
                case 2:
                    result = (uint)((array[0] << 16) + array[1]);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// LSB / Little-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt32 GetUInt32LSB(params UInt16[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return 0;
            }
            UInt32 result = 0;
            switch (array.Length)
            {
                case 1:
                    result = array[0];
                    break;
                default:
                case 2:
                    result = (uint)(array[0] + (array[1] << 16));
                    break;
            }
            return result;
        }

        #endregion

        #region Not

        /// <summary>
        /// Perform bitwise NOT operation on every byte in array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static byte[] Not(byte[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return array;
            }
            int l = array.Length;
            byte[] result = new byte[l];
            for (int i = 0; i < l; i++)
            {
                result[i] = (byte)(array[i] ^ 0xff);
            }
            return result;
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
        public static byte[] Or(byte[] one, byte[] two)
        {
            if (null == one || 0 == one.Length || null == two || 0 == two.Length)
            {
                return one;
            }
            byte[] result = new byte[one.Length];
            if (two.Length >= one.Length)
            {
                for (int i = 0, l = one.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] | two[i]);
                }
            }
            else
            {
                for (int i = 0, l = one.Length, m = two.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] | two[i % m]);
                }
            }
            return result;
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
        public static byte[] And(byte[] one, byte[] two)
        {
            if (null == one || 0 == one.Length || null == two || 0 == two.Length)
            {
                return one;
            }
            byte[] result = new byte[one.Length];
            if (two.Length >= one.Length)
            {
                for (int i = 0, l = one.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] & two[i]);
                }
            }
            else
            {
                for (int i = 0, l = one.Length, m = two.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] & two[i % m]);
                }
            }
            return result;
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
        public static byte[] Xor(byte[] one, byte[] two)
        {
            if (null == one || 0 == one.Length || null == two || 0 == two.Length)
            {
                return one;
            }
            byte[] result = new byte[one.Length];
            if (two.Length >= one.Length)
            {
                for (int i = 0, l = one.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] ^ two[i]);
                }
            }
            else
            {
                for (int i = 0, l = one.Length, m = two.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] ^ two[i % m]);
                }
            }
            return result;
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
        public static byte[] Rol(byte[] array, int count)
        {
            if (count < 0)
            {
                return Ror(array, -count);
            }
            if (null == array || 0 == array.Length || 0 == count)
            {
                return array;
            }
            int n = array.Length << 3;
            if (count > n)
            {
                count %= n;
            }
            if (0 == count || n == count)
            {
                return array;
            }
            byte[] result = new byte[array.Length];
            int o = count % 8;
            int l = array.Length;

            if (0 == o)
            {
                int m = count >> 3;
                for (int i = 0, j = m; i < l; i++, j++)
                {
                    if (j >= l)
                    {
                        j = 0;
                    }
                    result[i] = array[j];
                }
            }
            else
            {
                int p = 8 - o;
                int m = count >> 3;
                for (int i = 0, j = m, k = m + 1; i < l; i++, j++, k++)
                {
                    while (j >= l)
                    {
                        j -= l;
                    }
                    while (k >= l)
                    {
                        k -= l;
                    }
                    result[i] = (byte)(((array[j] << o) & 0xff) + (array[k] >> p));
                }
            }
            return result;
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
        public static byte[] Ror(byte[] array, int count)
        {
            if (count < 0)
            {
                return Rol(array, -count);
            }
            if (null == array || 0 == array.Length || 0 == count)
            {
                return array;
            }
            int n = array.Length << 3;
            if (count > n)
            {
                count %= n;
            }
            if (0 == count || n == count)
            {
                return array;
            }
            byte[] result = new byte[array.Length];
            int o = count % 8;
            int l = array.Length;

            if (0 == o)
            {
                int m = l - (count >> 3);
                for (int i = 0, j = m; i < l; i++, j++)
                {
                    if (j >= l)
                    {
                        j = 0;
                    }
                    result[i] = array[j];
                }
            }
            else
            {
                int p = 8 - o;
                int m = count >> 3;
                for (int i = 0, j = l - m + l - 1, k = l - m; i < l; i++, j++, k++)
                {
                    while (j >= l)
                    {
                        j -= l;
                    }
                    while (k >= l)
                    {
                        k -= l;
                    }
                    result[i] = (byte)(((array[j] << p) & 0xff) + (array[k] >> o));
                }
            }
            return result;
        }

        #endregion

        #region Bcd

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

        #endregion
    }
}