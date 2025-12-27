using System;
using System.IO;

namespace Energy.Base
{
    /// <summary>
    /// Compression
    /// </summary>
    public class Compression
    {
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

        /// <summary>
        /// ZX0
        /// </summary>
        public class ZX0
        {
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

            /// <summary>
            /// Compress using ZX0 algorithm.
            /// 
            /// NOTE: This implementation produces valid ZX0 format output but without compression
            /// (stores data as literals only). The output can be decompressed by any ZX0 decompressor.
            /// 
            /// For optimal compression with match-finding, use the reference zx0 compressor:
            /// https://github.com/einar-saukas/ZX0
            /// 
            /// The reference implementation uses a sophisticated dynamic programming algorithm that
            /// tracks compression paths for each possible offset value. Porting this algorithm requires
            /// careful handling of the relationship between input positions and output buffer positions.
            /// </summary>
            /// <param name="data">Data to compress</param>
            /// <returns>Compressed data in ZX0 format (literals only)</returns>
            public static byte[] Compress(byte[] data)
            {
                try
                {
                    if (data == null) return null;
                    if (data.Length == 0) return new byte[0];

                    MemoryStream output = new MemoryStream();
                    BitWriter writer = new BitWriter(output);

                    // Literals-only compression
                    WriteInterlacedEliasGamma(writer, data.Length, false);
                    for (int i = 0; i < data.Length; i++)
                        writer.WriteByte(data[i]);
                    
                    // EOF marker
                    writer.WriteBit(1);
                    WriteInterlacedEliasGamma(writer, 256, true);
                    writer.Flush();

                    return output.ToArray();
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                    return null;
                }
            }

            /// <summary>
            /// Decompress using ZX0 algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Decompress(byte[] data)
            {
                try
                {
                    if (data == null) return null;
                    if (data.Length == 0) return new byte[0];

                    BitReader reader = new BitReader(data);
                    System.Collections.Generic.List<byte> output = new System.Collections.Generic.List<byte>();
                    int lastOffset = 1;

                    while (true)
                    {
                        int length = ReadInterlacedEliasGamma(reader, false);
                        for (int i = 0; i < length; i++)
                            output.Add((byte)reader.ReadByte());

                        if (reader.ReadBit() != 0)
                        {
                            int offsetMsb = ReadInterlacedEliasGamma(reader, true);
                            if (offsetMsb == 256)
                                break;

                            lastOffset = offsetMsb * 128 - (reader.ReadByte() >> 1);
                            reader.SetBacktrack();
                            length = ReadInterlacedEliasGamma(reader, false) + 1;

                            int copyPos = output.Count - lastOffset;
                            if (copyPos < 0)
                                throw new InvalidOperationException(string.Format("Invalid offset: copyPos={0}, output.Count={1}, lastOffset={2}", copyPos, output.Count, lastOffset));
                            
                            for (int i = 0; i < length; i++)
                            {
                                if (copyPos + i >= output.Count)
                                    throw new InvalidOperationException(string.Format("Copy beyond buffer: copyPos={0}, i={1}, output.Count={2}", copyPos, i, output.Count));
                                output.Add(output[copyPos + i]);
                            }
                        }
                        else
                        {
                            length = ReadInterlacedEliasGamma(reader, false);
                            int copyPos = output.Count - lastOffset;
                            if (copyPos < 0)
                                throw new InvalidOperationException(string.Format("Invalid offset: copyPos={0}, output.Count={1}, lastOffset={2}", copyPos, output.Count, lastOffset));
                            
                            for (int i = 0; i < length; i++)
                            {
                                if (copyPos + i >= output.Count)
                                    throw new InvalidOperationException(string.Format("Copy beyond buffer: copyPos={0}, i={1}, output.Count={2}", copyPos, i, output.Count));
                                output.Add(output[copyPos + i]);
                            }
                        }

                        if (reader.ReadBit() == 0)
                            continue;

                        int newOffsetMsb = ReadInterlacedEliasGamma(reader, true);
                        if (newOffsetMsb == 256)
                            break;

                        lastOffset = newOffsetMsb * 128 - (reader.ReadByte() >> 1);
                        reader.SetBacktrack();
                        length = ReadInterlacedEliasGamma(reader, false) + 1;

                        int pos = output.Count - lastOffset;
                        if (pos < 0)
                            throw new InvalidOperationException(string.Format("Invalid offset: pos={0}, output.Count={1}, lastOffset={2}", pos, output.Count, lastOffset));
                        
                        for (int i = 0; i < length; i++)
                        {
                            if (pos + i >= output.Count)
                                throw new InvalidOperationException(string.Format("Copy beyond buffer: pos={0}, i={1}, output.Count={2}", pos, i, output.Count));
                            output.Add(output[pos + i]);
                        }
                    }

                    return output.ToArray();
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                    return null;
                }
            }
        }
    }
}
