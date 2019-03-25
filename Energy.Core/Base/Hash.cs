﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Hashing functions for sequences of characters or bytes
    /// </summary>
    // TODO Implement PBKDF2, Bcrypt, HMAC/SHA1 and possibly legacy DES or AES 3
    public class Hash
    {
        /// <summary>
        /// For each characters do a 5-bit left circular shift
        /// and XOR in character numeric value (CRC variant)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>32-bit hash value for a string (uint)</returns>
        public static uint CRC(string value)
        {
            uint h = 0;
            for (int i = 0; i < value.Length; i++)
            {
                h = (h << 5) ^ ((h & 0xf8000000) >> 27) ^ value[i];
            }
            return h;
        }

        /// <summary>
        /// For each characters do a 5-bit left circular shift
        /// and XOR in character numeric value (CRC variant)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint CRC2(string value)
        {
            uint k;
            uint h = 0;
            for (int i = 0; i < value.Length; i++)
            {
                k = value[i];
                // extract high-order 5 bits from h
                // 0xf8000000 is the hexadecimal representation
                // for the 32-bit number with the first five
                // bits = 1 and the other bits = 0
                var s = h & 0xf8000000;
                // shift h left by 5 bits
                h = h << 5;
                // move the highorder 5 bits to the low-order end and XOR into h
                h = h ^ (s >> 27);
                //h = h ^ s;
                // end and XOR into h
                h = h ^ k;
            }
            return h;
        }

        /// <summary>
        /// For each characters add character numeric value and left shift by 4 bits (PJW hash)
        /// </summary>
        /// <remarks>
        /// Aho, Sethi, and Ullman pp. 434-438
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>24-bit hash value for a string (uint)</returns>
        public static uint PJW(string value)
        {
            uint h = 0, g;
            for (int i = 0; i < value.Length; i++)
            {
                h = (h << 4) + value[i];
                // check if top 4 bits of h aren't zero
                if ((g = h & 0xf0000000) != 0)
                {
                    // move them to the low end of h
                    h = h ^ (g >> 24);
                    // zero top 4 bits of h
                    h = h ^ g;
                }
            }
            return h;
        }

        /// <summary>
        /// Return MD5 for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>string</returns>
        public static string MD5(string text, Encoding encoding)
        {
            if (text == null || encoding == null)
                return null;
            using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] array = md5.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ByteArrayToHex(array);
            }
        }

        /// <summary>
        /// Return MD5 for a string.
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>string</returns>
        public static string MD5(string text)
        {
            return MD5(text, Encoding.UTF8);
        }

        /// <summary>
        /// Return SHA1 for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA1(string text, Encoding encoding)
        {
            if (text == null || encoding == null)
                return null;
            using (System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                byte[] array = sha.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ByteArrayToHex(array);
            }
        }

        /// <summary>
        /// Return SHA1 for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA1(string text)
        {
            return SHA1(text, Encoding.UTF8);
        }

        /// <summary>
        /// Return SHA1 for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA256(string text, Encoding encoding)
        {
            if (text == null)
                return null;
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] array = sha256.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ByteArrayToHex(array);
            }
        }

        /// <summary>
        /// Return SHA1 for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA256(string text)
        {
            return SHA256(text, Encoding.UTF8);
        }
    }
}
