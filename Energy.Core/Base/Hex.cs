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
        #region Constant

        internal const string HEX_STRING = "0123456789ABCDEF";
        internal const string HEX_STRING_UPPER = "0123456789ABCDEF";
        internal const string HEX_STRING_LOWER = "0123456789abcdef";
        internal const string HEX_STRING_32 = "0123456789ABCDEF0123456789abcdef";
        internal const string HEX_STRING_24_REVERSE = "FEDCBA9876543210fedcba";
        internal static readonly IList<string> HEX_BIN_16 = new List<string>(new string[]
        {
            "0000", "0001", "0010", "0011",
            "0100", "0101", "0110", "0111",
            "1000", "1001", "1010", "1011",
            "1100", "1101", "1110", "1111",
        });

        #endregion

        #region HexToArray

        /// <summary>
        /// Convert hexadecimal string to byte array.
        /// This is strict version of conversion function.
        /// Hexadecimal string should contain only digits and small or upper letters A-F.
        /// Any other character is treated as zero.
        /// </summary>
        /// <param name="hex">Hexadecimal string</param>
        /// <returns>Byte array</returns>
        public static byte[] HexToArray(string hex)
        {
            if (null == hex)
            {
                return null;
            }
            if (0 == hex.Length)
            {
                return new byte[] { };
            }
            int l = hex.Length;
            if (0 != l % 2)
            {
                l++;
                hex = "0" + hex;
            }
            byte[] array = new byte[l / 2];
            int p = 0;
            for (int i = 0; i < l; i++)
            {
                byte b = 0;
                if (hex[i] >= '0' && hex[i] <= '9')
                {
                    b = (byte)(hex[i] - '0');
                }
                else if (hex[i] >= 'A' && hex[i] <= 'F')
                {
                    b = (byte)(10 + hex[i] - 'A');
                }
                else if (hex[i] >= 'a' && hex[i] <= 'f')
                {
                    b = (byte)(10 + hex[i] - 'a');
                }
                i++;
                b = (byte)(b << 4);
                if (hex[i] >= '0' && hex[i] <= '9')
                {
                    b += (byte)(hex[i] - '0');
                }
                else if (hex[i] >= 'A' && hex[i] <= 'F')
                {
                    b += (byte)(10 + hex[i] - 'A');
                }
                else if (hex[i] >= 'a' && hex[i] <= 'f')
                {
                    b += (byte)(10 + hex[i] - 'a');
                }
                array[p++] = b;
            }
            return array;
        }

        /// <summary>
        /// Convert hexadecimal string to byte array.
        /// This version of conversion function allows to use
        /// prefixes (like "0x" or "$") and whitespace characters.
        /// Hexadecimal string should contain only digits and small or upper letters A-F.
        /// Any other character is treated as zero.
        /// </summary>
        /// <param name="hex">Hexadecimal string</param>
        /// <param name="ignoreWhite">Ignore whitespace</param>
        /// <param name="prefix">Array of possible prefixes</param>
        /// <returns>Byte array</returns>
        public static byte[] HexToArray(string hex, bool ignoreWhite, string[] prefix)
        {
            if (null == hex)
            {
                return null;
            }
            if (ignoreWhite)
            {
                hex = Regex.Replace(hex, "\\s", "");
            }
            if (null != prefix)
            {
                for (int i = 0; i < prefix.Length; i++)
                {
                    if (hex.StartsWith(prefix[i]))
                    {
                        hex = hex.Substring(prefix[i].Length);
                        break;
                    }
                }
            }
            return HexToArray(hex);
        }


        /// <summary>
        /// Convert hexadecimal string to byte array.
        /// This version of conversion function allows to use whitespace characters.
        /// Hexadecimal string should contain only digits and small or upper letters A-F.
        /// Any other character is treated as zero.
        /// </summary>
        /// <param name="hex">Hexadecimal string</param>
        /// <param name="ignoreWhite">Ignore whitespace</param>
        /// <returns>Byte array</returns>
        public static byte[] HexToArray(string hex, bool ignoreWhite)
        {
            return HexToArray(hex, ignoreWhite, null);
        }

        #endregion

        #region ArrayToHex

        /// <summary>
        /// Convert byte array to hexadecimal string.
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="space">Optional separator</param>
        /// <returns>string</returns>
        public static string ArrayToHex(byte[] array, string space)
        {
            if (null == array)
            {
                return null;
            }
            if (0 == array.Length)
            {
                return "";
            }
            if (null == space)
            {
                space = "";
            }
            string hex = BitConverter.ToString(array);
            return hex.Replace("-", space);
        }

        /// <summary>
        /// Convert byte array to hexadecimal string.
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <returns>string</returns>
        public static string ArrayToHex(byte[] array)
        {
            return ArrayToHex(array, "");
        }

        #endregion

        #region ByteToHex

        /// <summary>
        /// Represent byte as two-digit hexadecimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ByteToHex(byte value)
        {
            return value.ToString("X2");
        }

        #endregion

        #region ByteToPrintable

        /// <summary>
        /// Convert byte to printable character.
        /// <br/><br/>
        /// All non-ASCII characters will be represented as dot character.
        /// Bytes 0, 128 and 255 will be represented as space.
        /// </summary>
        /// <param name="b">Byte</param>
        /// <returns>Character</returns>
        public static char ByteToPrintable(byte b)
        {
            if (b == 0 || b == 128 || b == 255) return ' ';
            if (b < 32) return '.';
            if (b > 126) return '.';
            return (char)b;
        }

        #endregion

        #region IntegerToHex

        /// <summary>
        /// Convert integer to hexadecimal value.
        /// 
        /// Resulting string will have count specified by size of digits or letters (A-F).
        /// If number representation will be larger than size, it will be truncated to the last characters.
        /// Example: IntegerToHex(100000, 4) will result with "86a0" instead of "186a0" or "186a".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="capitalize"></param>
        /// <returns></returns>
        public static string IntegerToHex(int value, int size, bool capitalize)
        {
            string hex = value.ToString(capitalize ? "X" : "x");
            if (hex.Length < size)
                hex = hex.PadLeft(size, '0');
            else if (hex.Length > size)
                hex = hex.Substring(hex.Length - size);
            return hex;
        }

        /// <summary>
        /// Convert integer to hexadecimal value.
        /// 
        /// Resulting string will have always 8 digits or letters (A-F).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntegerToHex(int value)
        {
            return IntegerToHex(value, 8, true);
        }

        /// <summary>
        /// Convert integer to hexadecimal value.
        /// 
        /// Resulting string will have always 8 digits or letters (A-F).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="uppperCase"></param>
        /// <returns></returns>
        public static string IntegerToHex(int value, bool uppperCase)
        {
            return IntegerToHex(value, 8, uppperCase);
        }

        /// <summary>
        /// Convert integer to hexadecimal value.
        /// 
        /// Resulting string will have count specified by size of digits or letters (A-F).
        /// If number representation will be larger than size, it will be truncated to the last characters.
        /// Example: IntegerToHex(100000, 4) will result with "86a0" instead of "186a0" or "186a".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string IntegerToHex(int value, int size)
        {
            return IntegerToHex(value, size, true);
        }

        #endregion

        #region HexToInteger

        /// <summary>
        /// Convert hexadecimal string to integer value (System.Int32).
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToInteger(string hex)
        {
            int value;
            if (Int32.TryParse(hex, System.Globalization.NumberStyles.HexNumber
                , System.Globalization.CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            return value;
        }

        #endregion

        #region BinToHex

        /// <summary>
        /// Convert binary string to hexadecimal string.
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string BinToHex(string bin)
        {
            if (string.IsNullOrEmpty(bin))
            {
                return bin;
            }
            if (0 < bin.Length % 4)
            {
                bin = bin.PadLeft(4 * (1 + (int)bin.Length / 4), '0');
            }
            int length = bin.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i += 4)
            {
                string s = bin.Substring(i, 4);
                int n = HEX_BIN_16.IndexOf(s);
                if (n < 0)
                {
                    continue;
                }
                sb.Append(HEX_STRING_UPPER[n]);
            }
            return sb.ToString();
        }

        #endregion

        #region HexToBin

        /// <summary>
        /// Convert hexadecimal string to binary string.
        /// <br/><br/>
        /// Note that hexadecimal "0" will be represented with leading zeroes as "0000" in binary.
        /// Resulting binary string will always have a length divisible by 4.
        /// <br/><br/>
        /// Works also when hexadecimal string starts with "0x" or "$".
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string HexToBin(string hex)
        {
            if (null == hex)
            {
                return null;
            }
            hex = RemovePrefix(hex);
            if (0 == hex.Length)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            int length = hex.Length;
            for (int i = 0; i < length; i++)
            {
                int o = HEX_STRING_32.IndexOf(hex[i]);
                if (o < 0)
                {
                    continue;
                }
                sb.Append(HEX_BIN_16[o % 16]);
            }
            return sb.ToString();
        }

        #endregion

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
        /// Format settings for printing
        /// </summary>
        public class PrintFormatSettings
        {
            /// <summary>Number of bytes in one line</summary>
            public int LineSize;
            /// <summary>Number of bytes in every group</summary>
            public int GroupSize;
            /// <summary>Use uppercase letters in hexadecimal representation</summary>
            public bool UpperCase;
            /// <summary>Group bytes together, i.e. 2 for 16-bit, 4 for 32-bit, etc.</summary>
            public int WordSize;
            /// <summary>Use offset prefix of hexadecimals length</summary>
            public int OffsetSize;
            /// <summary>Include optional representation</summary>
            public string RepresentationMode;
            /// <summary>Element separator</summary>
            public string ElementSeparator;
            /// <summary>Group separator</summary>
            public string GroupSeparator;
            /// <summary>Offset separator</summary>
            public string OffsetSeparator;
            /// <summary>Representation separator</summary>
            public string RepresentationSeparator;

            /// <summary>
            /// Constructor
            /// </summary>
            public PrintFormatSettings()
            {
                LineSize = 16;
                ElementSeparator = " ";
                WordSize = 1;
                GroupSize = 4;
                GroupSeparator = "  ";
                OffsetSize = 0;
                OffsetSeparator = "  ";
                UpperCase = false;
                RepresentationMode = "ASCII";
                RepresentationSeparator = "   ";
            }
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form
        /// </summary>
        /// <param name="array">Input byte array</param>
        /// <param name="printFormatSettings">Format settings</param>
        /// <returns></returns>
        public static string Print(byte[] array, PrintFormatSettings printFormatSettings)
        {
            return Print(array
                , printFormatSettings.LineSize
                , printFormatSettings.GroupSize
                , printFormatSettings.UpperCase
                , printFormatSettings.WordSize
                , printFormatSettings.OffsetSize
                , printFormatSettings.RepresentationMode
                , printFormatSettings.ElementSeparator
                , printFormatSettings.GroupSeparator
                , printFormatSettings.OffsetSeparator
                , printFormatSettings.RepresentationSeparator
                );
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form
        /// </summary>
        /// <param name="array">Input byte array</param>
        /// <param name="lineSize">Number of bytes in one line</param>
        /// <param name="groupSize">Number of bytes in every group</param>
        /// <param name="upperCase">Use uppercase letters in hexadecimal representation</param>
        /// <param name="wordSize">Group bytes together, i.e. 2 for 16-bit, 4 for 32-bit, etc.</param>
        /// <param name="offsetSize">Use offset prefix of hexadecimals length</param>
        /// <param name="representationMode">Include optional representation</param>
        /// <param name="elementSeparator">Element separator</param>
        /// <param name="groupSeparator">Group separator</param>
        /// <param name="offsetSeparator">Offset separator</param>
        /// <param name="representationSeparator">Representation separator</param>
        /// <returns></returns>
        public static string Print(byte[] array
            , int lineSize
            , int groupSize
            , bool upperCase
            , int wordSize
            , int offsetSize
            , string representationMode
            , string elementSeparator
            , string groupSeparator
            , string offsetSeparator
            , string representationSeparator
            )
        {
            if (array == null || array.Length == 0)
                return "";
            StringBuilder b = new StringBuilder();
            string s = BitConverter.ToString(array).Replace("-", "");
            if (!upperCase)
                s = s.ToLower();

            if (wordSize < 1)
                wordSize = 1;

            if (lineSize < 1)
                lineSize = 1;

            if (offsetSeparator == null)
                offsetSeparator = "";

            if (groupSeparator == null)
                groupSeparator = "";

            if (elementSeparator == null)
                elementSeparator = "";

            if (representationSeparator == null)
                representationSeparator = "";

            int arrayLength = array.Length;

            bool includeRepresentation = !string.IsNullOrEmpty(representationMode);

            int lineCount = (int)Math.Ceiling(1.0 * arrayLength / lineSize);

            for (int o = 0, n = 0; n < lineCount; n++)
            {
                if (n > 0)
                    b.AppendLine();
                if (offsetSize > 0)
                {
                    b.Append(IntegerToHex(o, offsetSize, upperCase));
                    if (offsetSeparator.Length > 0)
                        b.Append(offsetSeparator);
                }
                bool empty = false;
                for (int i = 0; i < lineSize; i++)
                {
                    if (i > 0)
                    {
                        if (groupSize > 0 && 0 == i % groupSize)
                        {
                            if (groupSeparator.Length > 0)
                                b.Append(groupSeparator);
                        }
                        else if (wordSize > 1 && 0 != i % wordSize)
                        {
                        }
                        else
                        {
                            if (elementSeparator.Length > 0)
                                b.Append(elementSeparator);
                        }
                    }
                    int a = o + i;
                    if (!empty && a >= arrayLength)
                    {
                        if (!includeRepresentation)
                            break;
                        else
                            empty = true;
                    }
                    b.Append(empty ? "  " : s.Substring(a << 1, 2));
                }
                if (includeRepresentation)
                {
                    if (representationSeparator.Length > 0)
                        b.Append(representationSeparator);
                    for (int i = 0; i < lineSize; i++)
                    {
                        int a = o + i;
                        if (a >= arrayLength)
                            break;
                        b.Append(ByteToPrintable(array[a]));
                    }
                }
                o += lineSize;
            }
            return b.ToString();
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="lineSize">Number of bytes in one line</param>
        /// <param name="groupSize">Put extra space between each division columns</param>
        /// <param name="offsetSize">Include offset with specified dimension in bytes or -1 for autosize</param>
        /// <param name="wordSize">Group bytes together 2 for words, 4 for double words, etc.</param>
        /// <param name="representation">Include character representation</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int lineSize, int groupSize, int offsetSize, int wordSize, bool representation)
        {
            return Print(array, new PrintFormatSettings()
            {
                LineSize = lineSize,
                GroupSize = groupSize,
                OffsetSize = offsetSize,
                WordSize = wordSize,
                RepresentationMode = representation ? "ASCII" : "",
            });
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="lineSize">Number of bytes in one line</param>
        /// <param name="groupSize">Put extra space between each division columns</param>
        /// <param name="representation">Character representation</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int lineSize, int groupSize, bool representation)
        {
            return Print(array, new PrintFormatSettings()
            {
                LineSize = lineSize,
                GroupSize = groupSize,
                RepresentationMode = representation ? "ASCII" : "",
            });
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form with text represenation
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="lineSize">Number of bytes in one line</param>
        /// <param name="groupSize">Put extra space between each division columns</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int lineSize, int groupSize)
        {
            return Print(array, new PrintFormatSettings()
            {
                LineSize = lineSize,
                GroupSize = groupSize,
            });
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form with text represenation
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <param name="lineSize">Number of bytes in one line</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array, int lineSize)
        {
            return Print(array, new PrintFormatSettings()
            {
                LineSize = lineSize,
            });
        }

        /// <summary>
        /// Pretty print byte array in hexadecimal form with text represenation
        /// </summary>
        /// <param name="array">Byte array</param>
        /// <returns>Text representation</returns>
        public static string Print(byte[] array)
        {
            return Print(array, new PrintFormatSettings());
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

        /// <summary>
        /// Check if text is a hexadecimal number string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="prefixArray"></param>
        /// <param name="removeWhite"></param>
        /// <returns></returns>
        public static bool IsHex(string value, string[] prefixArray, bool removeWhite)
        {
            if (removeWhite)
            {
                value = Energy.Base.Text.RemoveWhite(value);
            }
            if (null == prefixArray || 0 == prefixArray.Length)
            {
                return IsHex(value);
            }
            for (int i = 0; i < prefixArray.Length; i++)
            {
                string prefix = prefixArray[i];
                if (!value.StartsWith(prefix))
                {
                    continue;
                }
                value = value.Substring(prefix.Length);
                return IsHex(value);
            }
            return false;
        }

        /// <summary>
        /// Check if text is a hexadecimal number string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="prefixArray"></param>
        /// <returns></returns>
        public static bool IsHex(string value, string[] prefixArray)
        {
            return IsHex(value, prefixArray, false);
        }

        /// <summary>
        /// Check if text is a hexadecimal number string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsHex(string value)
        {
            if (null == value)
                return false;
            return IsHex(value.ToCharArray());
        }

        /// <summary>
        /// Check if text is a hexadecimal number string.
        /// </summary>
        /// <param name="charArray"></param>
        /// <returns></returns>
        private static bool IsHex(char[] charArray)
        {
            if (charArray == null)
                return false;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (0 < HEX_STRING_24_REVERSE.IndexOf(charArray[i]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region RemovePrefix

        /// <summary>
        /// Remove leading prefix "0x", "0X" or "$" from hexadecimal string.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string RemovePrefix(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return hex;
            }
            else if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return hex.Substring(2);
            }
            else if (hex.StartsWith("$"))
            {
                return hex.Substring(1);
            }
            else
            {
                return hex;
            }
        }

        #endregion
    }
}