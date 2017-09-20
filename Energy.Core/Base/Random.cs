using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Random
    {
        private static readonly object _GetRandomGuidLock = new object();

        /// <summary>
        /// Get random GUID identifier
        /// </summary>
        /// <returns></returns>
        public static string GetRandomGuid()
        {
            lock (_GetRandomGuidLock)
            {
                return System.Guid.NewGuid().ToString("D").ToUpper();
            }
        }

        private static System.Random _RandomObject;

        private static readonly object _RandomObjectLock = new object();

        public static int GetNextInteger(int minimum, int maximum)
        {
            lock (_RandomObjectLock)
            {
                if (_RandomObject == null)
                    _RandomObject = new System.Random();
                return _RandomObject.Next(minimum, maximum);
            }
        }

        public static int GetNextInteger()
        {
            return GetNextInteger(int.MinValue, int.MaxValue);
        }

        public static int GetNextInteger(int count)
        {
            return GetNextInteger(0, count - 1);
        }

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomText()
        {
            return GetRandomText("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 3, 10);
        }

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <param name="available">Available characters for generating random text</param>
        /// <param name="minimum">Minimum number of characters</param>
        /// <param name="maximum">Maximum number of characters</param>
        /// <returns></returns>
        public static string GetRandomText(string available, int minimum, int maximum)
        {
            if (string.IsNullOrEmpty(available))
                return available;
            int count = GetNextInteger(minimum, maximum);
            StringBuilder s = new StringBuilder(count);
            for (int i = 0; i < count; i++)
                s.Append(available[GetNextInteger(available.Length)]);
            return s.ToString();
        }

        /// <summary>
        /// Generate random hexadecimal number.
        /// </summary>
        /// <param name="length">Number of hexadecimal characters</param>
        /// <returns></returns>
        public static string GetRandomHex(int length)
        {
            if (length < 1)
                return "";
            string hex = "0123456789ABCDEF";
            StringBuilder s = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                s.Append(hex[GetNextInteger(16)]);
            return s.ToString();
        }

        /// <summary>
        /// Generate random hexadecimal number.
        /// </summary>        
        /// <returns></returns>
        public static string GetRandomHex()
        {
            return GetRandomHex(8);
        }
    }
}
