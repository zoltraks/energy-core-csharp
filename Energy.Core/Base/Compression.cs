using System;
using System.IO;

namespace Energy.Base
{
    /// <summary>
    /// Compression functions
    /// </summary>
    public class Compression
    {
        #region Deflate

        /// <summary>
        /// Deflate
        /// </summary>
        public class Deflate
        {
            /// <summary>
            /// Compress using deflate algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Compress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (System.IO.Compression.DeflateStream compressionStream = new System.IO.Compression.DeflateStream(memoryStream
                        , System.IO.Compression.CompressionMode.Compress))
                    {
                        compressionStream.Write(data, 0, data.Length);
                        compressionStream.Close();
                        return memoryStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }

            /// <summary>
            /// Decompress using deflate algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Decompress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream inputStream = new MemoryStream(data))
                    using (MemoryStream outputStream = new MemoryStream())
                    using (System.IO.Compression.DeflateStream decompressionStream = new System.IO.Compression.DeflateStream(inputStream
                        , System.IO.Compression.CompressionMode.Decompress))
                    {
                        int count = 2048;
                        byte[] buffer = new byte[count];
                        for (; ; )
                        {
                            int length = decompressionStream.Read(buffer, 0, count);
                            if (length == 0)
                                break;
                            outputStream.Write(buffer, 0, length);
                        }
                        return outputStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }
        }

        #endregion

        #region GZip

        /// <summary>
        /// GZip
        /// </summary>
        public class GZip
        {
            /// <summary>
            /// Compress using gzip algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Compress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (System.IO.Compression.GZipStream compressionStream = new System.IO.Compression.GZipStream(memoryStream
                        , System.IO.Compression.CompressionMode.Compress))
                    {
                        compressionStream.Write(data, 0, data.Length);
                        compressionStream.Close();
                        return memoryStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }

            /// <summary>
            /// Decompress using gzip algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Decompress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream inputStream = new MemoryStream(data))
                    using (MemoryStream outputStream = new MemoryStream())
                    using (System.IO.Compression.GZipStream decompressionStream = new System.IO.Compression.GZipStream(inputStream
                        , System.IO.Compression.CompressionMode.Decompress))
                    {
                        int count = 2048;
                        byte[] buffer = new byte[count];
                        for (; ; )
                        {
                            int length = decompressionStream.Read(buffer, 0, count);
                            if (length == 0)
                                break;
                            outputStream.Write(buffer, 0, length);
                        }
                        return outputStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }
        }

        #endregion

        #region ZX0

        /// <summary>
        /// ZX0
        /// </summary>
        public class ZX0
        {
            private const int INITIAL_OFFSET = 1;
            private const int MAX_OFFSET_ZX0 = 32640;

            private class Block
            {
                public int Bits;
                public int Index;
                public int Offset;
                public Block Chain;

                public Block(int bits, int index, int offset, Block chain)
                {
                    this.Bits = bits;
                    this.Index = index;
                    this.Offset = offset;
                    this.Chain = chain;
                }
            }

            private static Block Allocate(int bits, int index, int offset, Block chain)
            {
                return new Block(bits, index, offset, chain);
            }

            private static int OffsetCeiling(int index, int offsetLimit)
            {
                if (index > offsetLimit)
                    return offsetLimit;
                if (index < INITIAL_OFFSET)
                    return INITIAL_OFFSET;
                return index;
            }

            private static int EliasGammaBits(int value)
            {
                int bits = 1;
                while ((value >>= 1) != 0)
                    bits += 2;
                return bits;
            }

            private static Block Optimize(byte[] inputData, int inputSize, int skip, int offsetLimit)
            {
                int maxOffset = OffsetCeiling(inputSize - 1, offsetLimit);

                Block[] lastLiteral = new Block[maxOffset + 1];
                Block[] lastMatch = new Block[maxOffset + 1];
                Block[] optimal = new Block[inputSize];
                int[] matchLength = new int[maxOffset + 1];
                int[] bestLength = new int[inputSize];

                if (inputSize > 2)
                    bestLength[2] = 2;

                lastMatch[INITIAL_OFFSET] = Allocate(-1, skip - 1, INITIAL_OFFSET, null);

                for (int index = skip; index < inputSize; index++)
                {
                    int bestLengthSize = 2;
                    maxOffset = OffsetCeiling(index, offsetLimit);

                    for (int offset = 1; offset <= maxOffset; offset++)
                    {
                        if (index != skip && index >= offset && inputData[index] == inputData[index - offset])
                        {
                            Block lastLiteralBlock = lastLiteral[offset];
                            if (lastLiteralBlock != null)
                            {
                                int length = index - lastLiteralBlock.Index;
                                int bits = lastLiteralBlock.Bits + 1 + EliasGammaBits(length);
                                Block candidate = Allocate(bits, index, offset, lastLiteralBlock);
                                lastMatch[offset] = candidate;
                                Block optimalIndexBlock = optimal[index];
                                if (optimalIndexBlock == null || optimalIndexBlock.Bits > bits)
                                    optimal[index] = candidate;
                            }

                            int currentMatchLength = ++matchLength[offset];
                            if (currentMatchLength > 1)
                            {
                                if (bestLengthSize < currentMatchLength)
                                {
                                    int bits = optimal[index - bestLength[bestLengthSize]].Bits + EliasGammaBits(bestLength[bestLengthSize] - 1);
                                    int bits2;
                                    do
                                    {
                                        bestLengthSize++;
                                        bits2 = optimal[index - bestLengthSize].Bits + EliasGammaBits(bestLengthSize - 1);
                                        if (bits2 <= bits)
                                        {
                                            bestLength[bestLengthSize] = bestLengthSize;
                                            bits = bits2;
                                        }
                                        else
                                        {
                                            bestLength[bestLengthSize] = bestLength[bestLengthSize - 1];
                                        }
                                    }
                                    while (bestLengthSize < currentMatchLength);
                                }

                                int length = bestLength[currentMatchLength];
                                int bitsForMatch = optimal[index - length].Bits + 8 + EliasGammaBits((offset - 1) / 128 + 1) + EliasGammaBits(length - 1);
                                Block lastMatchBlock = lastMatch[offset];
                                if (lastMatchBlock == null || lastMatchBlock.Index != index || lastMatchBlock.Bits > bitsForMatch)
                                {
                                    Block candidate = Allocate(bitsForMatch, index, offset, optimal[index - length]);
                                    lastMatch[offset] = candidate;
                                    Block optimalIndexBlock = optimal[index];
                                    if (optimalIndexBlock == null || optimalIndexBlock.Bits > bitsForMatch)
                                        optimal[index] = candidate;
                                }
                            }
                        }
                        else
                        {
                            matchLength[offset] = 0;
                            Block lastMatchBlock = lastMatch[offset];
                            if (lastMatchBlock != null)
                            {
                                int length = index - lastMatchBlock.Index;
                                int bits = lastMatchBlock.Bits + 1 + EliasGammaBits(length) + length * 8;
                                Block candidate = Allocate(bits, index, 0, lastMatchBlock);
                                lastLiteral[offset] = candidate;
                                Block optimalIndexBlock = optimal[index];
                                if (optimalIndexBlock == null || optimalIndexBlock.Bits > bits)
                                    optimal[index] = candidate;
                            }
                        }
                    }
                }

                return optimal[inputSize - 1];
            }

            private sealed class EncoderState
            {
                public byte[] OutputData;
                public int OutputIndex;
                public int InputIndex;
                public int BitIndex;
                public int BitMask;
                public bool Backtrack;
            }

            private static byte[] CompressInternal(Block optimal, byte[] inputData, int inputSize, int skip, bool backwardsMode, bool invertMode)
            {
                if (optimal == null)
                    return new byte[0];

                int outputSize = (optimal.Bits + 25) / 8;
                byte[] output = new byte[outputSize];

                EncoderState state = new EncoderState();
                state.OutputData = output;
                state.OutputIndex = 0;
                state.InputIndex = skip;
                state.BitMask = 0;
                state.Backtrack = true;

                Block prev = null;
                Block next;

                while (optimal != null)
                {
                    next = optimal.Chain;
                    optimal.Chain = prev;
                    prev = optimal;
                    optimal = next;
                }

                int lastOffset = INITIAL_OFFSET;

                for (Block current = prev.Chain; current != null; )
                {
                    Block block = current;
                    current = current.Chain;

                    int length = block.Index - prev.Index;

                    if (block.Offset == 0)
                    {
                        WriteBit(state, 0);
                        WriteInterlacedEliasGamma(state, length, backwardsMode, false);
                        for (int i = 0; i < length; i++)
                        {
                            WriteByte(state, inputData[state.InputIndex]);
                            state.InputIndex++;
                        }
                    }
                    else if (block.Offset == lastOffset)
                    {
                        WriteBit(state, 0);
                        WriteInterlacedEliasGamma(state, length, backwardsMode, false);
                        state.InputIndex += length;
                    }
                    else
                    {
                        WriteBit(state, 1);
                        WriteInterlacedEliasGamma(state, ((block.Offset - 1) / 128) + 1, backwardsMode, invertMode);
                        if (backwardsMode)
                            WriteByte(state, ((block.Offset - 1) % 128) << 1);
                        else
                            WriteByte(state, (127 - ((block.Offset - 1) % 128)) << 1);
                        state.Backtrack = true;
                        WriteInterlacedEliasGamma(state, length - 1, backwardsMode, false);
                        state.InputIndex += length;
                        lastOffset = block.Offset;
                    }

                    prev = block;
                }

                WriteBit(state, 1);
                WriteInterlacedEliasGamma(state, 256, backwardsMode, invertMode);

                return output;
            }

            private static void WriteByte(EncoderState state, int value)
            {
                state.OutputData[state.OutputIndex++] = (byte)value;
            }

            private static void WriteBit(EncoderState state, int value)
            {
                if (state.Backtrack)
                {
                    if (value != 0)
                    {
                        int index = state.OutputIndex - 1;
                        state.OutputData[index] = (byte)(state.OutputData[index] | 1);
                    }
                    state.Backtrack = false;
                }
                else
                {
                    if (state.BitMask == 0)
                    {
                        state.BitMask = 128;
                        state.BitIndex = state.OutputIndex;
                        WriteByte(state, 0);
                    }

                    if (value != 0)
                    {
                        state.OutputData[state.BitIndex] = (byte)(state.OutputData[state.BitIndex] | state.BitMask);
                    }
                    state.BitMask >>= 1;
                }
            }

            private static void WriteInterlacedEliasGamma(EncoderState state, int value, bool backwardsMode, bool invertMode)
            {
                int i = 2;
                while (i <= value)
                {
                    i <<= 1;
                }
                i >>= 1;
                while ((i >>= 1) != 0)
                {
                    WriteBit(state, backwardsMode ? 1 : 0);
                    int bitValue = (value & i) != 0 ? 1 : 0;
                    if (invertMode)
                    {
                        bitValue = bitValue == 0 ? 1 : 0;
                    }
                    WriteBit(state, bitValue);
                }
                WriteBit(state, backwardsMode ? 0 : 1);
            }

            private class BitWriter
            {
                private MemoryStream stream;
                private int bitMask;
                private int bitValue;
                private int lastBit;

                public BitWriter(MemoryStream stream)
                {
                    this.stream = stream;
                    this.bitMask = 128;
                    this.bitValue = 0;
                    this.lastBit = 0;
                }

                public void WriteBit(int bit)
                {
                    lastBit = bit;
                    if (bit != 0)
                        bitValue |= bitMask;
                    bitMask >>= 1;
                    if (bitMask == 0)
                    {
                        stream.WriteByte((byte)bitValue);
                        bitMask = 128;
                        bitValue = 0;
                    }
                }

                public int GetLastBit()
                {
                    return lastBit;
                }

                public void WriteByte(byte value)
                {
                    // Write byte directly to stream
                    // In ZX0, literal bytes and bit-packed data are interleaved in the stream
                    stream.WriteByte(value);
                }

                public void Flush()
                {
                    if (bitMask != 128)
                        stream.WriteByte((byte)bitValue);
                }
            }

            private class BitReader
            {
                private byte[] data;
                private int index;
                private int bitMask;
                private int bitValue;
                private int lastByte;
                private bool backtrack;

                public BitReader(byte[] data)
                {
                    this.data = data;
                    this.index = 0;
                    this.bitMask = 0;
                    this.backtrack = false;
                }

                public int ReadByte()
                {
                    if (index >= data.Length)
                        throw new InvalidOperationException("Unexpected end of data");
                    lastByte = data[index++];
                    return lastByte;
                }

                public int ReadBit()
                {
                    if (backtrack)
                    {
                        backtrack = false;
                        return lastByte & 1;
                    }
                    bitMask >>= 1;
                    if (bitMask == 0)
                    {
                        bitMask = 128;
                        bitValue = ReadByte();
                    }
                    return (bitValue & bitMask) != 0 ? 1 : 0;
                }

                public void SetBacktrack()
                {
                    backtrack = true;
                }
            }

            private static void WriteInterlacedEliasGamma(BitWriter writer, int value, bool inverted)
            {
                int bits = 0;
                int temp = value;
                while (temp > 1)
                {
                    temp >>= 1;
                    bits++;
                }

                for (int i = bits - 1; i >= 0; i--)
                {
                    writer.WriteBit(0);
                    int bit = (value >> i) & 1;
                    writer.WriteBit(inverted ? bit ^ 1 : bit);
                }
                writer.WriteBit(1);
            }

            private static int ReadInterlacedEliasGamma(BitReader reader, bool inverted)
            {
                int value = 1;
                while (reader.ReadBit() == 0)
                {
                    int bit = reader.ReadBit();
                    value = (value << 1) | (inverted ? bit ^ 1 : bit);
                }
                return value;
            }

            private static void WriteBytes(System.Collections.Generic.List<byte> output, int offset, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    int pos = output.Count - offset;
                    if (pos < 0)
                        throw new InvalidOperationException(string.Format("Invalid offset: pos={0}, output.Count={1}, offset={2}", pos, output.Count, offset));
                    if (pos >= output.Count)
                        throw new InvalidOperationException(string.Format("Copy beyond buffer: pos={0}, i={1}, output.Count={2}", pos, i, output.Count));
                    output.Add(output[pos]);
                }
            }

            /// <summary>
            /// Compress using the ZX0 algorithm (forward, non-classic mode).
            /// This implementation is a C# port of the official ZX0 optimizer and encoder
            /// and produces byte-identical output to the reference zx0 tool for this mode.
            /// </summary>
            /// <param name="data">Uncompressed data to compress.</param>
            /// <returns>Compressed data in ZX0 format that can be decompressed by any compatible ZX0 decompressor.</returns>
            public static byte[] Compress(byte[] data)
            {
                try
                {
                    if (data == null) return null;
                    if (data.Length == 0) return new byte[0];

                    int inputSize = data.Length;
                    int skip = 0;
                    int offsetLimit = MAX_OFFSET_ZX0;

                    Block optimal = Optimize(data, inputSize, skip, offsetLimit);
                    if (optimal == null)
                        return new byte[0];

                    bool backwardsMode = false;
                    bool classicMode = false;
                    bool invertMode = !classicMode && !backwardsMode;

                    return CompressInternal(optimal, data, inputSize, skip, backwardsMode, invertMode);
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                    return null;
                }
            }

            /// <summary>
            /// Decompress data previously compressed with the ZX0 algorithm (forward, non-classic mode).
            /// Implements the same state machine as the official dzx0.c reference decoder, including
            /// support for overlapping back-references.
            /// </summary>
            /// <param name="data">Data in ZX0 compressed format.</param>
            /// <returns>Decompressed original data.</returns>
            public static byte[] Decompress(byte[] data)
            {
                try
                {
                    if (data == null) return null;
                    if (data.Length == 0) return new byte[0];

                    BitReader reader = new BitReader(data);
                    System.Collections.Generic.List<byte> output = new System.Collections.Generic.List<byte>();
                    int lastOffset = INITIAL_OFFSET;
                    bool classicMode = false;

                    while (true)
                    {
                        // COPY_LITERALS
                        int length = ReadInterlacedEliasGamma(reader, false);
                        for (int i = 0; i < length; i++)
                            output.Add((byte)reader.ReadByte());

                        if (reader.ReadBit() != 0)
                        {
                            // COPY_FROM_NEW_OFFSET sequence
                            while (true)
                            {
                                int offsetMsb = ReadInterlacedEliasGamma(reader, !classicMode);
                                if (offsetMsb == 256)
                                    return output.ToArray();

                                lastOffset = offsetMsb * 128 - (reader.ReadByte() >> 1);
                                reader.SetBacktrack();
                                length = ReadInterlacedEliasGamma(reader, false) + 1;
                                WriteBytes(output, lastOffset, length);

                                if (reader.ReadBit() == 0)
                                    break; // back to literals
                            }

                            continue;
                        }

                        // COPY_FROM_LAST_OFFSET
                        length = ReadInterlacedEliasGamma(reader, false);
                        WriteBytes(output, lastOffset, length);

                        if (reader.ReadBit() != 0)
                        {
                            // fall-through into COPY_FROM_NEW_OFFSET chain
                            while (true)
                            {
                                int offsetMsb = ReadInterlacedEliasGamma(reader, !classicMode);
                                if (offsetMsb == 256)
                                    return output.ToArray();

                                lastOffset = offsetMsb * 128 - (reader.ReadByte() >> 1);
                                reader.SetBacktrack();
                                length = ReadInterlacedEliasGamma(reader, false) + 1;
                                WriteBytes(output, lastOffset, length);

                                if (reader.ReadBit() == 0)
                                    break; // back to literals
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                    return null;
                }
            }
        }

        #endregion
    }
}
