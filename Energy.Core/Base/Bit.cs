﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Collection of binary utility functions.
    /// </summary>
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
        public static ushort Reverse(ushort value)
        {
            return (ushort)((byte)(value / 256) + (value % 256) * 256);
        }

        /// <summary>
        /// Reverse order of bytes.
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

        #endregion

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

        #region Endianess

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// MSB / Big-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt32 GetUInt32MSB(params UInt16[] array)
        {
            if (array == null || array.Length == 0)
                return 0;
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

        #endregion
    }
}