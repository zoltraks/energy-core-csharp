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

        #endregion

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

        #region ASCII

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

        #endregion

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
            return HexToByteArray(hex, null);
        }

        /// <summary>
        /// Convert hex to byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="prefix">Optional prefix list of hexadecimal number</param>
        /// <returns></returns>
        public static byte[] HexToByteArray(string hex, string[] prefix)
        {
            if (null == hex)
                return null;

            // TODO Compare performance with hex = Regex.Replace(hex, @"\s", "");
            hex = Energy.Base.Text.RemoveWhitespace(hex);

            if (0 == hex.Length)
            {
                return new byte[] { };
            }

            if (prefix != null && prefix.Length > 0)
            {
                bool found = false;
                for (int i = 0; i < prefix.Length; i++)
                {
                    if (hex.StartsWith(prefix[i]))
                    {
                        hex = hex.Substring(prefix[i].Length);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return null;
                }
            }

            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }

            int count = hex.Length / 2;
            byte[] data = new byte[count];
            char[] charArray = hex.ToCharArray();

            for (int i = 0, n = 0; i < hex.Length; i += 2)
            {
                int hi = HEX_STRING_24_REVERSE.IndexOf(charArray[i]);
                int lo = HEX_STRING_24_REVERSE.IndexOf(charArray[i + 1]);
                if (hi < 0)
                    hi = 0;
                if (lo < 0)
                    lo = 0;
                hi = (15 - (hi % 16)) * 16;
                lo = 15 - (lo % 16);
                data[n++] = (byte)(hi + lo);
            }

            return data;
        }

        #region IntegerToHex

        public static string IntegerToHex(int value, int size, bool uppperCase)
        {
            string hex = value.ToString(uppperCase ? "X" : "x");
            if (hex.Length < size)
                hex = hex.PadLeft(size, '0');
            else if (hex.Length > size)
                hex = hex.Substring(hex.Length - size);
            return hex;
        }

        public static string IntegerToHex(int value)
        {
            return IntegerToHex(value, 8, true);
        }

        public static string IntegerToHex(int value, bool uppperCase)
        {
            return IntegerToHex(value, 8, uppperCase);
        }

        public static string IntegerToHex(int value, int size)
        {
            return IntegerToHex(value, size, true);
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
                        b.Append(ByteToASCII(array[a]));
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
                value = Energy.Base.Text.RemoveWhitespace(value);
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
    }
}