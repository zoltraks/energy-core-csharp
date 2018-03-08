using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Random
    /// </summary>
    public class Random
    {
        #region Private

        private static readonly object _GetRandomGuidLock = new object();

        private static System.Random _RandomObject;

        private static readonly object _RandomObjectLock = new object();

        private static readonly string _DefaultRandomTextCharacterList = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        #endregion

        #region Guid

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

        #endregion

        #region Integer

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

        #endregion

        #region Text

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
        /// Generate random text.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomText()
        {
            return GetRandomText(_DefaultRandomTextCharacterList, 3, 10);
        }

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <param name="minimum">Minimum number of characters</param>
        /// <param name="maximum">Maximum number of characters</param>
        /// <returns></returns>
        public static string GetRandomText(int minimum, int maximum)
        {
            return GetRandomText(_DefaultRandomTextCharacterList, minimum, maximum);
        }

        /// <summary>
        /// Generate random text.
        /// </summary>
        /// <param name="length">Number of characters</param>
        /// <returns></returns>
        public static string GetRandomText(int length)
        {
            return GetRandomText(_DefaultRandomTextCharacterList, length, length);
        }

        #endregion

        #region Hex

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

        #endregion

        #region ByteArray

        /// <summary>
        /// Get byte array filled with random values
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] GetRandomByteArray(int size)
        {
            byte[] buffer = new byte[size];
            lock (_RandomObjectLock)
            {
                if (_RandomObject == null)
                    _RandomObject = new System.Random();
                _RandomObject.NextBytes(buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Get byte array filled with random values within specified range
        /// </summary>
        /// <param name="size"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static byte[] GetRandomByteArray(int size, byte minimum, byte maximum)
        {
            byte[] buffer = GetRandomByteArray(size);
            byte width = (byte)(maximum >= minimum ? maximum - minimum : minimum - maximum);
            if (width > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    buffer[i] = (byte)(minimum + buffer[i] % (maximum - minimum));
                }
            }
            return buffer;
        }

        #endregion
    }
}
