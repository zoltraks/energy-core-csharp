using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Energy.Base
{
    /// <summary>
    /// Hexadecimal functions
    /// </summary>
    public class Hex
    {
        #region ByteArray

        /// <summary>
        /// Byte array which can be serialized and deserialized with hexadecimal string
        /// </summary>
        public class ByteArray
        {
            private byte[] _Data;
            /// <summary>Data</summary>
            [XmlIgnore]
            public byte[] Data
            {
                get
                {
                    lock (Sync)
                    {
                        return _Data;
                    }
                }
                set
                {
                    lock (Sync)
                    {
                        _Data = value;
                        UpdateHex = true;
                    }
                }
            }

            public int Length
            {
                get
                {
                    lock (Sync)
                    {
                        if (_Data == null) return 0;
                        return _Data.Length;
                    }
                }
            }

            private static readonly object Sync = new object();

            private bool UpdateHex;

            private string _Hex;
            /// <summary>Hex</summary>
            public string Hex
            {
                get
                {
                    lock (Sync)
                    {
                        if (UpdateHex)
                        {
                            _Hex = ByteArrayToHex(_Data);
                            UpdateHex = false;
                        }
                        return _Hex;
                    }
                }
                set
                {
                    lock (Sync)
                    {
                        _Hex = value;
                        _Data = HexToByteArray(_Hex);
                        UpdateHex = false;
                    }
                }
            }

            /// <summary>
            /// Array of byte accessor
            /// </summary>
            /// <param name="offset"></param>
            /// <returns></returns>
            public byte this[int offset]
            {
                get
                {
                    return _Data[offset];
                }
                set
                {
                    _Data[offset] = value;
                    // which one is better?
                    UpdateHex = true;
                    // or this?
                    if (!UpdateHex) UpdateHex = true;
                }
            }

            public static implicit operator ByteArray(string hex)
            {
                ByteArray o = new ByteArray();
                o.Hex = hex;
                return o;
            }

            public static implicit operator ByteArray(byte[] data)
            {
                ByteArray o = new ByteArray();
                o.Data = data;
                return o;
            }
        }

        #endregion

        /// <summary>
        /// Convert byte to ASCII character
        /// </summary>
        /// <param name="b">Byte</param>
        /// <returns>Character</returns>
        public static char ByteToASCII(byte b)
        {
            if (b == 0 || b == 128 || b == 255) return ' ';
            if (b < 32) return '.';
            if (b > 126) return '.';
            return (char)b;
        }

        /// <summary>
        /// Convert hexadecimal string to byte array
        /// </summary>
        /// <param name="hex">Hexadecimal string</param>
        /// <returns>Byte array</returns>
        public static byte[] HexToArray(string hex)
        {
            byte[] array = new byte[hex.Length / 2];
            int p = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                byte b = 0;
                if (hex[i] >= '0' && hex[i] <= '9')
                    b = (byte)(hex[i] - '0' << 4);
                else if (hex[i] >= 'A' && hex[i] <= 'F')
                    b = (byte)(10 + hex[i] - 'A' << 4);
                else if (hex[i] >= 'a' && hex[i] <= 'f')
                    b = (byte)(10 + hex[i] - 'a' << 4);
                i++;
                if (hex[i] >= '0' && hex[i] <= '9')
                    b += (byte)(hex[i] - '0');
                else if (hex[i] >= 'A' && hex[i] <= 'F')
                    b += (byte)(10 + hex[i] - 'A');
                else if (hex[i] >= 'a' && hex[i] <= 'f')
                    b += (byte)(10 + hex[i] - 'a');
                array[p++] = b;
            }
            return array;
        }

        /// <summary>
        /// Convert byte array to hexadecimal string
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="space">Optional hexadecimal separator</param>
        /// <returns>string</returns>
        public static string ArrayToHex(byte[] array, string space)
        {
            if (array == null)
                return null;
            if (array.Length == 0)
                return "";
            string hex = BitConverter.ToString(array);
            return hex.Replace("-", space);
        }

        /// <summary>
        /// Convert byte array to hexadecimal string
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <returns>string</returns>
        public static string ArrayToHex(byte[] array)
        {
            return ArrayToHex(array, "");
        }

        /// <summary>
        /// Represent byte as two-digit hexadecimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ByteToHex(byte value)
        {
            return value.ToString("X2");
        }

        /// <summary>
        /// Represent bytes as hexadecimal string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ByteArrayToHex(byte[] array)
        {
            if (array == null) return null;
            if (array.Length == 0) return "";
            return BitConverter.ToString(array).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Convert hex to byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToByteArray(string hex)
        {
            if (hex == null) return null;
            hex = Regex.Replace(hex, @"\s", "");
            if (hex.Length < 2) return new byte[] { };
            int count = hex.Length / 2;
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = (byte)(
                   "0123456789ABCDEF".IndexOf(hex.Substring(i * 2, 1), StringComparison.InvariantCultureIgnoreCase) * 16 +
                   "0123456789ABCDEF".IndexOf(hex.Substring(1 + i * 2, 1), StringComparison.InvariantCultureIgnoreCase)
                );
            }
            return data;
        }

        public static string IntegerToHex(int value)
        {
            string hex = value.ToString("X");
            if (hex.Length < 8)
                hex = hex.PadLeft(8, '0');
            return hex;
        }

        public static string IntegerToHex(int value, int size)
        {
            string hex = value.ToString("X");
            if (hex.Length < size)
                hex = hex.PadLeft(size, '0');
            return hex;
        }

        #region Parse

        /// <summary>
        /// Parse hexadecimal string up to 64-bit value
        /// </summary>
        /// <param name="hex">Hexadecimal</param>
        /// <returns>Long number</returns>
        public static ulong Parse(string hex)
        {
            ulong value = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                if (value > 0) value = value << 8;
                byte b = 0;
                if (hex[i] >= '0' && hex[i] <= '9')
                    b = (byte)(hex[i] - '0' << 4);
                else if (hex[i] >= 'A' && hex[i] <= 'F')
                    b = (byte)(10 + hex[i] - 'A' << 4);
                else if (hex[i] >= 'a' && hex[i] <= 'f')
                    b = (byte)(10 + hex[i] - 'a' << 4);
                i++;
                if (hex[i] >= '0' && hex[i] <= '9')
                    b += (byte)(hex[i] - '0');
                else if (hex[i] >= 'A' && hex[i] <= 'F')
                    b += (byte)(10 + hex[i] - 'A');
                else if (hex[i] >= 'a' && hex[i] <= 'f')
                    b += (byte)(10 + hex[i] - 'a');
                value += b;
            }
            return value;
        }

        #endregion

        #region Print

        /// <summary>
        /// Pretty print byte array in hexadecimal form
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="size">Number of bytes in one line</param>
        /// <param name="divide">Put extra space between each division columns</param>
        /// <param name="representation">Character representation</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int size, int divide, bool representation)
        {
            StringBuilder b = new StringBuilder();
            string s = BitConverter.ToString(array).Replace("-", "").ToLower();
            int o = 0;
            for (int i = 0; i < s.Length; i += 2)
            {
                b.Append(s.Substring(i, 2));
                b.Append(" ");
                if (size > 0 && (i / 2 + 1) % size == 0)
                {
                    if (representation)
                    {
                        b.Append("  ");
                        for (int j = 0; j < size; j++)
                        {
                            b.Append(ByteToASCII(array[o + j]));
                        }
                        o += size;
                    }
                    b.AppendLine();
                }
                else if (divide > 0 && (i / 2 + 1) % divide == 0)
                {
                    b.Append(" ");
                }
            }
            if (representation)
            {
                if (size == 0)
                {
                    b.Append("  ");
                    for (int j = 0; j < array.Length; j++)
                    {
                        b.Append(ByteToASCII(array[j]));
                    }
                }
                else if (array.Length % size != 0)
                {
                    b.Append(new String(' ', 2 + 3 * (size - (array.Length % size))));
                    if (divide > 0) b.Append(new String(' ', (int)(size - (array.Length % size)) / divide));
                    for (int j = 0; j < array.Length % size; j++)
                    {
                        b.Append(ByteToASCII(array[o + j]));
                    }
                }
            }
            return b.ToString().Trim();
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form with text represenation
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="size">Number of bytes in one line</param>
        /// <param name="divide">Put extra space between each division columns</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int size, int divide)
        {
            return Print(array, size, divide, true);
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form with text represenation
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="size">Number of bytes in one line</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int size)
        {
            return Print(array, size, 4, true);
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form with text represenation
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array)
        {
            return Print(array, 16, 4, true);
        }

        #endregion

        #region Random

        /// <summary>
        /// Generate random hexadecimal number.
        /// </summary>
        /// <param name="length">Number of hexadecimal characters</param>
        /// <returns></returns>
        public static string GetRandomHex(int length)
        {
            return Energy.Base.Random.GetRandomHex(length);
        }

        /// <summary>
        /// Generate random hexadecimal number.
        /// </summary>        
        /// <returns></returns>
        public static string Random()
        {
            return Energy.Base.Random.GetRandomHex();
        }

        #endregion
    }
}