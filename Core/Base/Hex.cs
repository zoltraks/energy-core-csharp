using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Hexadecimal functions
    /// </summary>
    public class Hex
    {
        /// <summary>
        /// Convert byte to ASCII character
        /// </summary>
        /// <param name="b">Byte</param>
        /// <returns>Character</returns>
        public static char ByteToASCII(byte b)
        {
            if (b == 0 || b == 128 || b == 255) return ' ';
            if (b < 32) return '.';
            if (b > 128) return '.';
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
        /// Convert byte array to hex string
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="space">Optional hexadecimal separator</param>
        /// <returns>string</returns>
        public static string ArrayToHex(byte[] array, string space)
        {
            if (array == null) return null;
            string hex = BitConverter.ToString(array);
            return hex.Replace("-", space);
        }

        /// <summary>
        /// Convert byte array to hex string
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
    }
}