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

        #region Compare

        /// <summary>
        /// Compare byte arrays
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
    }
}