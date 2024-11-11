using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static Energy.Base.Hash.Algorithm;

namespace Energy.Base
{
    /// <summary>
    /// Hashing functions for sequences of characters or bytes.
    /// </summary>
    // TODO Implement PBKDF2, Bcrypt, HMAC/SHA1 and possibly legacy DES or AES 3
    public class Hash
    {
        #region Algorithm

        public class Algorithm
        {
            #region Crc32

            public class Crc32 : HashAlgorithm
            {
                public const uint DEFAULT_POLYNOMINAL = 0xedb88320;

                private uint checksum;

                private readonly uint[] table;

                public Crc32() : this(DEFAULT_POLYNOMINAL) { }

                public Crc32(uint polynomial)
                {
                    HashSizeValue = 32;

                    Initialize();

                    table = new uint[256];

                    for (uint i = 0; i < 256; i++)
                    {
                        uint next = i;
                        for (uint j = 0; j < 8; j++)
                        {
                            if ((next & 1) == 1)
                            {
                                next = (next >> 1) ^ polynomial;
                            }
                            else
                            {
                                next >>= 1;
                            }
                        }
                        table[i] = next;
                    }
                }

                public override void Initialize()
                {
                    checksum = 0xffffffff;
                }

                protected override void HashCore(byte[] buffer, int start, int length)
                {
                    for (int i = start; i < length; i++)
                    {
                        byte next = buffer[i];
                        byte index = (byte)(checksum ^ next);
                        checksum = (checksum >> 8) ^ table[index];
                    }
                }

                protected override byte[] HashFinal()
                {
                    checksum = ~checksum;
                    return BitConverter.GetBytes(checksum);
                }
            }

            #endregion
        }

        #endregion

        #region CRC32

        /// <summary>
        /// For each characters do a 5-bit left circular shift
        /// and XOR in character numeric value (CRC variant).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint CRC(string value)
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
        /// For each characters do a 5-bit left circular shift
        /// and XOR in character numeric value (CRC variant).
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static uint CRC(byte[] data)
        {
            uint k;
            uint h = 0;
            for (int i = 0; i < data.Length; i++)
            {
                k = data[i];
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

        #endregion

        /// <summary>
        /// Compute CRC32 hash using IEEE 802.3 standard
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static uint CRC32(byte[] data)
        {
            Algorithm.Crc32 checksum = new Algorithm.Crc32();
            byte[] hash = checksum.ComputeHash(data);
            uint result = BitConverter.ToUInt32(hash, 0);
            return result;
        }

        /// <summary>
        /// Compute CRC32 hash using custom polynominal
        /// </summary>
        /// <param name="data"></param>
        /// <param name="polynomial"></param>
        /// <returns></returns>
        public static uint CRC32(byte[] data, uint polynomial)
        {
            Algorithm.Crc32 checksum = new Algorithm.Crc32(polynomial);
            byte[] hash = checksum.ComputeHash(data);
            uint result = BitConverter.ToUInt32(hash, 0);
            return result;
        }

        #region CRC16CCITT

        /// <summary>
        /// Calculate 16-bit CRC-CCITT checksum with specified polynominal and initial value.
        /// </summary>
        /// <param name="array">Input byte array</param>
        /// <param name="poly">Truncated polynomial (default is 0x1021)</param>
        /// <param name="init">Initial value (default is 0xffff)</param>
        /// <returns></returns>
        public static ushort CRC16CCITT(byte[] array, ushort poly, ushort init)
        {
            ushort[] t = new ushort[256];
            ushort x;
            ushort y;
            ushort c;
            c = init;
            for (int i = 0; i < t.Length; ++i)
            {
                x = 0;
                y = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((x ^ y) & 0x8000) != 0)
                    {
                        x = (ushort)((x << 1) ^ poly);
                    }
                    else
                    {
                        x <<= 1;
                    }
                    y <<= 1;
                }
                t[i] = x;
            }
            for (int i = 0; i < array.Length; ++i)
            {
                c = (ushort)((c << 8) ^ t[(c >> 8) ^ (0xff & array[i])]);
            }
            return c;
        }

        /// <summary>
        /// Calculate 16-bit CRC-CCITT checksum with specified polynominal and initial value.
        /// </summary>
        /// <param name="text">Input text (UTF-8)</param>
        /// <param name="poly">Truncated polynomial (default is 0x1021)</param>
        /// <param name="init">Initial value (default is 0xffff)</param>
        /// <returns></returns>
        public static ushort CRC16CCITT(string text, ushort poly, ushort init)
        {
            if (null == text)
            {
                text = "";
            }
            byte[] array = Encoding.UTF8.GetBytes(text);
            return CRC16CCITT(array, poly, init);
        }

        #endregion

        #region PJW

        /// <summary>
        /// For each characters add character numeric value and left shift by 4 bits (PJW hash).
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
                    h ^= (g >> 24);
                    // zero top 4 bits of h
                    h ^= g;
                }
            }
            return h;
        }

        #endregion

        #region MD5

        /// <summary>
        /// Return MD5 hash for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>string</returns>
        public static string MD5(string text, Encoding encoding)
        {
            if (text == null || encoding == null)
            {
                return null;
            }
            using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] array = md5.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ArrayToHex(array).ToLower();
            }
        }

        /// <summary>
        /// Return MD5 hash for UTF-8 string.
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>string</returns>
        public static string MD5(string text)
        {
            return MD5(text, Encoding.UTF8);
        }

        #endregion

        #region SHA1

        /// <summary>
        /// Return SHA-1 hash for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA1(string text, Encoding encoding)
        {
            if (text == null || encoding == null)
            {
                return null;
            }
            using (System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                byte[] array = sha.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ArrayToHex(array).ToLower();
            }
        }

        /// <summary>
        /// Return SHA-1 for UTF-8 string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA1(string text)
        {
            return SHA1(text, Encoding.UTF8);
        }

        #endregion

        #region SHA256

        /// <summary>
        /// Return SHA-256 (SHA-2) for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA256(string text, Encoding encoding)
        {
            if (text == null)
            {
                return null;
            }
#if !NETCF
            using (System.Security.Cryptography.SHA256 cipher = System.Security.Cryptography.SHA256.Create())
            {
                byte[] array = cipher.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ArrayToHex(array).ToLower();
            }
#endif
#if NETCF
            return null;
#endif
        }

        /// <summary>
        /// Return SHA-256 (SHA-2) for a UTF-8 string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA256(string text)
        {
            return SHA256(text, Encoding.UTF8);
        }

        #endregion

        #region SHA384

        /// <summary>
        /// Return SHA-384 (SHA-2) for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA384(string text, Encoding encoding)
        {
            if (text == null)
            {
                return null;
            }
#if !NETCF
            using (System.Security.Cryptography.SHA384 cipher = System.Security.Cryptography.SHA384.Create())
            {
                byte[] array = cipher.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ArrayToHex(array).ToLower();
            }
#endif
#if NETCF
            return null;
#endif
        }

        /// <summary>
        /// Return SHA-384 (SHA-2) for UTF-8 string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA384(string text)
        {
            return SHA384(text, Encoding.UTF8);
        }

        #endregion

        #region SHA512

        /// <summary>
        /// Return SHA-512 (SHA-2) for a string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA512(string text, Encoding encoding)
        {
            if (text == null)
            {
                return null;
            }
#if !NETCF
            using (System.Security.Cryptography.SHA512 cipher = System.Security.Cryptography.SHA512.Create())
            {
                byte[] array = cipher.ComputeHash(encoding.GetBytes(text));
                return Energy.Base.Hex.ArrayToHex(array).ToLower();
            }
#endif
#if NETCF
            return null;
#endif
        }

        /// <summary>
        /// Return SHA-512 (SHA-2) for UTF-8 string.
        /// </summary>
        /// <param name="text">Source text to calculate hash from</param>
        /// <returns>SHA1 hash in hex format</returns>
        public static string SHA512(string text)
        {
            return SHA512(text, Encoding.UTF8);
        }

        #endregion
    }
}
