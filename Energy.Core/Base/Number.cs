using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Number
    {
        #region Constants

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

        #endregion

        #region Increment

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

        #endregion

        #region Decrement

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

        #endregion

        #region SelectHigher

        /// <summary>
        /// Select higher number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int SelectHigher(params int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            int number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select higher number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static uint SelectHigher(params uint[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            uint number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select higher number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static double SelectHigher(params double[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            double number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select higher number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static decimal SelectHigher(params decimal[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            decimal number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select higher number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static long SelectHigher(params long[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            long number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select higher number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static ulong SelectHigher(params ulong[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            ulong number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > number)
                    number = numbers[i];
            }
            return number;
        }

        #endregion

        #region SelectLower

        /// <summary>
        /// Select lower number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int SelectLower(params int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            int number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select lower number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static uint SelectLower(params uint[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            uint number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select lower number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static double SelectLower(params double[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            double number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select lower number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static decimal SelectLower(params decimal[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            decimal number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select lower number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static long SelectLower(params long[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            long number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < number)
                    number = numbers[i];
            }
            return number;
        }

        /// <summary>
        /// Select lower number from arguments.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static ulong SelectLower(params ulong[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
            ulong number = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < number)
                    number = numbers[i];
            }
            return number;
        }

        #endregion

        #region SelectHigherAbsoluteNegative

        /// <summary>
        /// Select higher number by absolute value with negative sign if concerned.
        /// Negative numbers are stronger than positive,
        /// so it will select -3 over 3 but also 4 over -3.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static long SelectHigherAbsoluteNegative(long p, long q)
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
        /// Select higher number by absolute value with negative sign if concerned.
        /// Negative numbers are stronger than positive,
        /// so it will select -3 over 3 but also 4 over -3.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static int SelectHigherAbsoluteNegative(int p, int q)
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

        #endregion
    }
}
