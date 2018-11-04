using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Number
    {
        /// <summary>
        /// Constant array of powers of 10 stored as long array.
        /// </summary>
        public static readonly long[] Power10 = {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
        };

        /// <summary>
        /// Select highest absolute number with negative sign from pair of numbers.
        /// Negative numbers are stronger than positive,
        /// so it will select -3 over 3 but also 4 over -3.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static long SelectHighestNegative(long p, long q)
        {
            if (p == 0 || p == q)
                return q;
            if (q == 0)
                return p;
            long a = Math.Abs(q);
            if (p > 0)
            {
                if (p > a)
                    q = p;
            }
            else
            {
                if (-p >= a)
                    q = p;
            }
            return q;
        }

        /// <summary>
        /// Select highest absolute number with negative sign from pair of numbers.
        /// Negative numbers are stronger than positive,
        /// so it will select -3 over 3 but also 4 over -3.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static int SelectHighestNegative(int p, int q)
        {
            if (p == 0 || p == q)
                return q;
            if (q == 0)
                return p;
            int a = Math.Abs(q);
            if (p > 0)
            {
                if (p > a)
                    q = p;
            }
            else
            {
                if (-p >= a)
                    q = p;
            }
            return q;
        }

        public static long Increment(long value)
        {
            return Increment(value, 0);
        }

        public static long Increment(long value, long zeroValue)
        {
            if (value == long.MaxValue)
                return zeroValue;
            else
                return value + 1;
        }

        public static long Decrement(long value)
        {
            return Decrement(value, 0);
        }

        public static long Decrement(long value, long zeroValue)
        {
            if (value == long.MinValue)
                return zeroValue;
            else
                return value - 1;
        }

        public static int Increment(int value)
        {
            return Increment(value, 0);
        }

        public static int Increment(int value, int zeroValue)
        {
            if (value == int.MaxValue)
                return zeroValue;
            else
                return value + 1;
        }

        public static int Decrement(int value)
        {
            return Decrement(value, 0);
        }

        public static int Decrement(int value, int zeroValue)
        {
            if (value == int.MinValue)
                return zeroValue;
            else
                return value - 1;
        }
    }
}
