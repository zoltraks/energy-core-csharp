using System;
using System.IO;

namespace Energy.Base
{
    /// <summary>
    /// Binary utility functions
    /// </summary>
    public class Binary
    {
        #region Reverse

        /// <summary>
        /// Reverse order of bytes.
        /// <para>
        /// Exchange lower byte with higher one
        /// </para>
        /// </summary>
        /// <param name="value">Unsigned short (16-bit)</param>
        /// <returns>Reversed numeric value</returns>
        public static ushort Reverse(ushort value)
        {
            return (ushort)((byte)(value / 256) + (value % 256) * 256);
        }

        /// <summary>
        /// Reverse order of bytes.
        /// </summary>
        /// <param name="value">Unsigned int (32-bit)</param>
        /// <returns>Reversed numeric value</returns>
        public static uint Reverse(uint value)
        {
            byte b1 = (byte)((value >> 0) & 0xff);
            byte b2 = (byte)((value >> 8) & 0xff);
            byte b3 = (byte)((value >> 16) & 0xff);
            byte b4 = (byte)((value >> 24) & 0xff);

            return (uint)(b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0);
        }

        #endregion

        #region Compare

        /// <summary>
        /// Compare byte arrays.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(byte[] left, byte[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare int arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(int[] left, int[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare long arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(long[] left, long[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare double arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(double[] left, double[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compare decimal arrays
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(decimal[] left, decimal[] right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                return -1;
            }
            if (right == null)
            {
                if (left == null)
                    return 0;
                return 1;
            }
            if (left.Length != right.Length)
            {
                return left.Length < right.Length ? -1 : 1;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] < right[i])
                    return -1;
                if (left[i] > right[i])
                    return 1;
            }
            return 0;
        }

        #endregion

        #region Endianess

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// MSB / Big-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt32 GetUInt32MSB(params UInt16[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return 0;
            }
            UInt32 result = 0;
            switch (array.Length)
            {
                case 1:
                    result = array[0];
                    break;
                default:
                case 2:
                    result = (uint)((array[0] << 16) + array[1]);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Take up to two 16-bit unsigned words and return them as 32-bit unsigned word.
        /// LSB / Little-Endian
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt32 GetUInt32LSB(params UInt16[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return 0;
            }
            UInt32 result = 0;
            switch (array.Length)
            {
                case 1:
                    result = array[0];
                    break;
                default:
                case 2:
                    result = (uint)(array[0] + (array[1] << 16));
                    break;
            }
            return result;
        }

        #endregion

        #region Not

        /// <summary>
        /// Perform bitwise NOT operation on every byte in array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static byte[] Not(byte[] array)
        {
            if (null == array || 0 == array.Length)
            {
                return array;
            }
            int l = array.Length;
            byte[] result = new byte[l];
            for (int i = 0; i < l; i++)
            {
                result[i] = (byte)(array[i] ^ 0xff);
            }
            return result;
        }

        #endregion

        #region Or

        /// <summary>
        /// Perform bitwise OR operation on every byte in array by second array.
        /// <br/><br/>
        /// Second array is treated as ring buffer when shorter than first one.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static byte[] Or(byte[] one, byte[] two)
        {
            if (null == one || 0 == one.Length || null == two || 0 == two.Length)
            {
                return one;
            }
            byte[] result = new byte[one.Length];
            if (two.Length >= one.Length)
            {
                for (int i = 0, l = one.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] | two[i]);
                }
            }
            else
            {
                for (int i = 0, l = one.Length, m = two.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] | two[i % m]);
                }
            }
            return result;
        }

        #endregion

        #region And

        /// <summary>
        /// Perform bitwise AND operation on every byte in array by second array.
        /// <br/><br/>
        /// Second array is treated as ring buffer when shorter than first one.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static byte[] And(byte[] one, byte[] two)
        {
            if (null == one || 0 == one.Length || null == two || 0 == two.Length)
            {
                return one;
            }
            byte[] result = new byte[one.Length];
            if (two.Length >= one.Length)
            {
                for (int i = 0, l = one.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] & two[i]);
                }
            }
            else
            {
                for (int i = 0, l = one.Length, m = two.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] & two[i % m]);
                }
            }
            return result;
        }

        #endregion

        #region Xor

        /// <summary>
        /// Perform bitwise XOR operation on every byte in array by second array.
        /// <br/><br/>
        /// Second array is treated as ring buffer when shorter than first one.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static byte[] Xor(byte[] one, byte[] two)
        {
            if (null == one || 0 == one.Length || null == two || 0 == two.Length)
            {
                return one;
            }
            byte[] result = new byte[one.Length];
            if (two.Length >= one.Length)
            {
                for (int i = 0, l = one.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] ^ two[i]);
                }
            }
            else
            {
                for (int i = 0, l = one.Length, m = two.Length; i < l; i++)
                {
                    result[i] = (byte)(one[i] ^ two[i % m]);
                }
            }
            return result;
        }

        #endregion

        #region Rol

        /// <summary>
        /// Rotate bits left in an array by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, right rotation will be performed instead.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte Rol(byte value, int count)
        {
            if (count < 0)
            {
                return Ror(value, -count);
            }
            if (count == 0)
            {
                return value;
            }
            int bits = 8;
            count %= bits;
            if (count == 0)
            {
                return value;
            }
            return (byte)(((value << count) | (value >> (bits - count))) & 0xff);
        }

        /// <summary>
        /// Rotate bits left in a single byte by exactly one bit.
        /// </summary>
        public static byte Rol(byte value)
        {
            return Rol(value, 1);
        }

        /// <summary>
        /// Rotate bits left in an unsigned 32-bit integer by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, right rotation will be performed instead.
        /// </summary>
        public static uint Rol(uint value, int count)
        {
            if (count < 0)
            {
                return Ror(value, -count);
            }
            if (count == 0)
            {
                return value;
            }
            int bits = 32;
            count %= bits;
            if (count == 0)
            {
                return value;
            }
            return (value << count) | (value >> (bits - count));
        }

        /// <summary>
        /// Rotate bits left in an unsigned 32-bit integer by exactly one bit.
        /// </summary>
        public static uint Rol(uint value)
        {
            return Rol(value, 1);
        }

        /// <summary>
        /// Rotate bits left in an unsigned 64-bit integer by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, right rotation will be performed instead.
        /// </summary>
        public static ulong Rol(ulong value, int count)
        {
            if (count < 0)
            {
                return Ror(value, -count);
            }
            if (count == 0)
            {
                return value;
            }
            int bits = 64;
            count %= bits;
            if (count == 0)
            {
                return value;
            }
            return (value << count) | (value >> (bits - count));
        }

        /// <summary>
        /// Rotate bits left in an unsigned 64-bit integer by exactly one bit.
        /// </summary>
        public static ulong Rol(ulong value)
        {
            return Rol(value, 1);
        }

        /// <summary>
        /// Rotate bits left in an array by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, right rotation will be performed instead.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] Rol(byte[] array, int count)
        {
            if (count < 0)
            {
                return Ror(array, -count);
            }
            if (null == array || 0 == array.Length || 0 == count)
            {
                return array;
            }
            int n = array.Length << 3;
            if (count > n)
            {
                count %= n;
            }
            if (0 == count || n == count)
            {
                return array;
            }
            byte[] result = new byte[array.Length];
            int o = count % 8;
            int l = array.Length;

            if (0 == o)
            {
                int m = count >> 3;
                for (int i = 0, j = m; i < l; i++, j++)
                {
                    if (j >= l)
                    {
                        j = 0;
                    }
                    result[i] = array[j];
                }
            }
            else
            {
                int p = 8 - o;
                int m = count >> 3;
                for (int i = 0, j = m, k = m + 1; i < l; i++, j++, k++)
                {
                    while (j >= l)
                    {
                        j -= l;
                    }
                    while (k >= l)
                    {
                        k -= l;
                    }
                    result[i] = (byte)(((array[j] << o) & 0xff) + (array[k] >> p));
                }
            }
            return result;
        }

        #endregion

        #region Ror

        /// <summary>
        /// Rotate bits right in an array by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, left rotation will be performed instead.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte Ror(byte value, int count)
        {
            if (count < 0)
            {
                return Rol(value, -count);
            }
            if (count == 0)
            {
                return value;
            }
            int bits = 8;
            count %= bits;
            if (count == 0)
            {
                return value;
            }
            return (byte)(((value >> count) | (value << (bits - count))) & 0xff);
        }

        /// <summary>
        /// Rotate bits right in a single byte by exactly one bit.
        /// </summary>
        public static byte Ror(byte value)
        {
            return Ror(value, 1);
        }

        /// <summary>
        /// Rotate bits right in an unsigned 32-bit integer by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, left rotation will be performed instead.
        /// </summary>
        public static uint Ror(uint value, int count)
        {
            if (count < 0)
            {
                return Rol(value, -count);
            }
            if (count == 0)
            {
                return value;
            }
            int bits = 32;
            count %= bits;
            if (count == 0)
            {
                return value;
            }
            return (value >> count) | (value << (bits - count));
        }

        /// <summary>
        /// Rotate bits right in an unsigned 32-bit integer by exactly one bit.
        /// </summary>
        public static uint Ror(uint value)
        {
            return Ror(value, 1);
        }

        /// <summary>
        /// Rotate bits right in an unsigned 64-bit integer by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, left rotation will be performed instead.
        /// </summary>
        public static ulong Ror(ulong value, int count)
        {
            if (count < 0)
            {
                return Rol(value, -count);
            }
            if (count == 0)
            {
                return value;
            }
            int bits = 64;
            count %= bits;
            if (count == 0)
            {
                return value;
            }
            return (value >> count) | (value << (bits - count));
        }

        /// <summary>
        /// Rotate bits right in an unsigned 64-bit integer by exactly one bit.
        /// </summary>
        public static ulong Ror(ulong value)
        {
            return Ror(value, 1);
        }

        /// <summary>
        /// Rotate bits right in an array by given bit count.
        /// <br/><br/>
        /// When negative number of bits is given, left rotation will be performed instead.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] Ror(byte[] array, int count)
        {
            if (count < 0)
            {
                return Rol(array, -count);
            }
            if (null == array || 0 == array.Length || 0 == count)
            {
                return array;
            }
            int n = array.Length << 3;
            if (count > n)
            {
                count %= n;
            }
            if (0 == count || n == count)
            {
                return array;
            }
            byte[] result = new byte[array.Length];
            int o = count % 8;
            int l = array.Length;

            if (0 == o)
            {
                int m = l - (count >> 3);
                for (int i = 0, j = m; i < l; i++, j++)
                {
                    if (j >= l)
                    {
                        j = 0;
                    }
                    result[i] = array[j];
                }
            }
            else
            {
                int p = 8 - o;
                int m = count >> 3;
                for (int i = 0, j = l - m + l - 1, k = l - m; i < l; i++, j++, k++)
                {
                    while (j >= l)
                    {
                        j -= l;
                    }
                    while (k >= l)
                    {
                        k -= l;
                    }
                    result[i] = (byte)(((array[j] << p) & 0xff) + (array[k] >> o));
                }
            }
            return result;
        }

        #endregion

        #region BitReader

        /// <summary>
        /// Streaming bit reader with configurable ordering and optional ZX0-style backtracking.
        /// </summary>
        public class BitReader
        {
            /// <summary>
            /// Options for configuring <see cref="BitReader" /> behavior.
            /// </summary>
            public class Options
            {
                /// <summary>
                /// Gets or sets whether bits are read most-significant-bit first.
                /// </summary>
                public bool MostSignificantBitFirst { get; set; } = true;

                /// <summary>
                /// Gets or sets whether the ZX0-style backtrack flag is enabled.
                /// </summary>
                public bool EnableBacktrack { get; set; } = true;

                /// <summary>
                /// Gets or sets whether reading past the end of the buffer or stream should throw.
                /// </summary>
                public bool ThrowOnEndOfStream { get; set; } = true;
            }

            private readonly byte[] buffer;
            private readonly int startIndex;
            private readonly int length;
            private readonly Stream stream;
            private readonly Options options;
            private int index;
            private int bitMask;
            private int bitValue;
            private int lastByte;
            private bool backtrack;

            /// <summary>
            /// Initializes a new reader over a byte array.
            /// </summary>
            /// <param name="data">Source data.</param>
            public BitReader(byte[] data)
                : this(data, 0, data != null ? data.Length : 0, null)
            {
            }

            /// <summary>
            /// Initializes a new reader over a byte array segment.
            /// </summary>
            /// <param name="data">Source data.</param>
            /// <param name="index">Offset.</param>
            /// <param name="count">Number of bytes to read.</param>
            public BitReader(byte[] data, int index, int count)
                : this(data, index, count, null)
            {
            }

            /// <summary>
            /// Initializes a new reader over a byte array segment with custom options.
            /// </summary>
            public BitReader(byte[] data, int index, int count, Options options)
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data));
                if (index < 0 || index > data.Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (count < 0 || index + count > data.Length)
                    throw new ArgumentOutOfRangeException(nameof(count));

                this.buffer = data;
                this.startIndex = index;
                this.length = count;
                this.options = options ?? new Options();
                this.bitMask = 0;
                this.index = 0;
            }

            /// <summary>
            /// Initializes a new reader over a stream.
            /// </summary>
            /// <param name="stream">Input stream.</param>
            public BitReader(Stream stream)
                : this(stream, null)
            {
            }

            /// <summary>
            /// Initializes a new reader over a stream with custom options.
            /// </summary>
            /// <param name="stream">Input stream.</param>
            /// <param name="options">Reader options.</param>
            public BitReader(Stream stream, Options options)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));

                this.stream = stream;
                this.options = options ?? new Options();
                this.bitMask = 0;
            }

            /// <summary>
            /// Gets the options associated with this reader.
            /// </summary>
            public Options Settings
            {
                get { return this.options; }
            }

            /// <summary>
            /// Gets the most recently read raw byte.
            /// </summary>
            public int LastByte
            {
                get { return lastByte; }
            }

            /// <summary>
            /// Reads the next byte from the underlying buffer or stream.
            /// </summary>
            /// <returns>Byte value, or -1 when the end is reached and <see cref="Options.ThrowOnEndOfStream" /> is <c>false</c>.</returns>
            public int ReadByte()
            {
                int value = ReadByteInternal();
                if (value < 0 && options.ThrowOnEndOfStream)
                    throw new InvalidOperationException("Unexpected end of data");
                return value;
            }

            /// <summary>
            /// Attempts to read the next bit using the configured ordering.
            /// </summary>
            /// <returns>Bit value (0 or 1).</returns>
            public int ReadBit()
            {
                if (options.EnableBacktrack && backtrack)
                {
                    backtrack = false;
                    return lastByte & 1;
                }

                AcquireBitMask();
                return (bitValue & bitMask) != 0 ? 1 : 0;
            }

            /// <summary>
            /// Enables ZX0-style backtracking so the next <see cref="ReadBit" /> returns the least significant bit of the last byte.
            /// </summary>
            public void SetBacktrack()
            {
                if (options.EnableBacktrack)
                    backtrack = true;
            }

            private void AcquireBitMask()
            {
                if (options.MostSignificantBitFirst)
                {
                    bitMask >>= 1;
                    if (bitMask == 0)
                    {
                        bitMask = 128;
                        bitValue = ReadByte();
                    }
                }
                else
                {
                    if (bitMask == 0)
                    {
                        bitMask = 1;
                        bitValue = ReadByte();
                    }
                    else
                    {
                        bitMask <<= 1;
                        if (bitMask >= 256)
                        {
                            bitMask = 1;
                            bitValue = ReadByte();
                        }
                    }
                }
            }

            private int ReadByteInternal()
            {
                int value;
                if (buffer != null)
                {
                    if (index >= length)
                        value = -1;
                    else
                    {
                        value = buffer[startIndex + index];
                        index++;
                    }
                }
                else
                {
                    value = stream.ReadByte();
                }

                if (value >= 0)
                    lastByte = value;
                else if (options.ThrowOnEndOfStream)
                    throw new InvalidOperationException("Unexpected end of data");

                return value;
            }
        }

        #endregion

        #region BitWriter

        /// <summary>
        /// Streaming bit writer with configurable bit ordering and literal interleaving semantics.
        /// <br/><br/>
        /// Defaults mimic the ZX0 encoder so existing callers receive identical output.
        /// </summary>

        public class BitWriter : IDisposable
        {
            /// <summary>
            /// Options for configuring <see cref="BitWriter" /> behavior.
            /// </summary>
            public class Options
            {
                /// <summary>
                /// Gets or sets whether bits should be written most-significant-bit first.
                /// </summary>
                public bool MostSignificantBitFirst { get; set; } = true;

                /// <summary>
                /// Gets or sets whether literal bytes may be interleaved with the current bit buffer without forcing alignment.
                /// </summary>
                public bool AllowLiteralInterleave { get; set; } = true;

                /// <summary>
                /// Gets or sets whether the underlying stream should remain open when the writer is disposed.
                /// </summary>
                public bool LeaveStreamOpen { get; set; }
            }

            private readonly Stream stream;
            private readonly Options options;
            private readonly int initialBitMask;
            private int bitMask;
            private int bitValue;
            private int lastBit;
            private bool disposed;

            /// <summary>
            /// Initializes a new instance targeting the provided stream.
            /// </summary>
            /// <param name="stream">Output stream.</param>
            public BitWriter(Stream stream)
                : this(stream, null)
            {
            }

            /// <summary>
            /// Initializes a new instance targeting the provided stream with custom options.
            /// </summary>
            /// <param name="stream">Output stream.</param>
            /// <param name="options">Optional writer options.</param>
            public BitWriter(Stream stream, Options options)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));

                this.stream = stream;
                this.options = options ?? new Options();
                this.initialBitMask = this.options.MostSignificantBitFirst ? 128 : 1;
                this.bitMask = 0;
                this.bitValue = 0;
            }

            /// <summary>
            /// Gets the options associated with this writer.
            /// </summary>
            public Options Settings
            {
                get { return this.options; }
            }

            /// <summary>
            /// Writes a single bit to the stream using the configured ordering.
            /// </summary>
            /// <param name="bit">Bit value (0 or 1).</param>
            public void WriteBit(int bit)
            {
                EnsureNotDisposed();

                if (bit != 0 && bit != 1)
                    throw new ArgumentOutOfRangeException(nameof(bit), "Bit value must be 0 or 1.");

                PrepareBitMask();
                lastBit = bit;

                if (bit != 0)
                    bitValue |= bitMask;

                AdvanceBitMask();
            }

            /// <summary>
            /// Writes a literal byte directly to the underlying stream.
            /// </summary>
            /// <param name="value">Byte to write.</param>
            public void WriteByte(byte value)
            {
                EnsureNotDisposed();

                if (!options.AllowLiteralInterleave)
                    Flush();

                stream.WriteByte(value);
            }

            /// <summary>
            /// Returns the most recently written bit value.
            /// </summary>
            /// <returns>Bit value.</returns>
            public int GetLastBit()
            {
                return lastBit;
            }

            /// <summary>
            /// Flushes any pending partial byte to the stream.
            /// </summary>
            public void Flush()
            {
                EnsureNotDisposed();

                if (bitMask == 0 || bitMask == initialBitMask)
                    return;

                FlushCurrentByte();
            }

            /// <inheritdoc />
            public void Dispose()
            {
                if (disposed)
                    return;

                Flush();

                if (!options.LeaveStreamOpen)
                    stream.Dispose();

                disposed = true;
            }

            private void PrepareBitMask()
            {
                if (bitMask == 0)
                    bitMask = initialBitMask;
            }

            private void AdvanceBitMask()
            {
                if (options.MostSignificantBitFirst)
                {
                    bitMask >>= 1;
                    if (bitMask == 0)
                    {
                        FlushCurrentByte();
                    }
                }
                else
                {
                    bitMask <<= 1;
                    if (bitMask >= 256 || bitMask == 0)
                    {
                        FlushCurrentByte();
                    }
                }

                if (bitMask == 0)
                    bitMask = initialBitMask;
            }

            private void FlushCurrentByte()
            {
                stream.WriteByte((byte)bitValue);
                bitValue = 0;
                bitMask = 0;
            }

            private void EnsureNotDisposed()
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(BitWriter));
            }
        }

        #endregion

        #region Bcd

        /// <summary>
        /// Binary-coded decimal (BCD) class
        /// </summary>
        public class Bcd
        {
            /// <summary>
            /// Convert BCD value to byte
            /// </summary>
            /// <param name="value">BCD value</param>
            /// <returns>Byte value</returns>
            public static byte ToByte(byte value)
            {
                return (byte)(10 * (value >> 4) + (value & 0xf));
            }

            /// <summary>
            /// Convert byte to BCD value
            /// </summary>
            /// <param name="value">Byte value</param>
            /// <returns>BCD value</returns>
            public static byte FromByte(byte value)
            {
                return (byte)(((value / 10 % 10) << 4) + value % 10);
            }

            /// <summary>
            /// Convert BCD value to word
            /// </summary>
            /// <param name="value">BCD value</param>
            /// <returns>Word value</returns>
            public static ushort ToWord(ushort value)
            {
                return (ushort)(1000 * ((value >> 12) & 0xf) + 100 * ((value >> 8) & 0xf) + 10 * ((value >> 4) & 0xf) + (value & 0xf));
            }

            /// <summary>
            /// Convert word to BCD value
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static ushort FromWord(ushort value)
            {
                return (ushort)(((value / 1000 % 10) << 12) + ((value / 100 % 10) << 8) + ((value / 10 % 10) << 4) + value % 10);
            }
        }

        #endregion
    }
}