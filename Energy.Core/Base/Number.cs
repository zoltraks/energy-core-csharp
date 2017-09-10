using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Number
    {
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
    }
}
