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

            // Optimal-parse graph node. Stored in a flat arena (see Optimizer) and
            // addressed by integer index rather than by object reference, so the hot
            // optimizer loop allocates no per-edge objects and creates no GC pressure.
            private struct Block
            {
                public int Bits;
                public int Index;
                public int Offset;
                public int Chain;       // arena index of the chained node, -1 when none
                public int References;  // live reference count, drives recycling
                public int GhostChain;  // free-list link while recycled, -1 when none
            }

            // Upper bound for the number of graph nodes simultaneously alive: every
            // input position keeps one optimal node, plus up to one pending node per
            // candidate offset. Seeding the arena to this size avoids growth in the
            // common case; the arena still grows on demand if the estimate is low.
            private static int InitialArenaCapacity(int inputSize, int offsetLimit)
            {
                int window = OffsetCeiling(inputSize - 1, offsetLimit);
                long capacity = (long)inputSize + window + 2;
                if (capacity < 16)
                    capacity = 16;
                if (capacity > int.MaxValue)
                    capacity = int.MaxValue;
                return (int)capacity;
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

            // Per-call optimal parser. Holds the block arena and the recycling free
            // list, so a single Compress call performs all its bookkeeping without any
            // shared mutable static state - which keeps Compress thread-safe.
            private sealed class Optimizer
            {
                private Block[] _arena;
                private int _count;
                private int _ghostRoot;

                public Optimizer(int capacity)
                {
                    if (capacity < 16)
                        capacity = 16;
                    _arena = new Block[capacity];
                    _count = 0;
                    _ghostRoot = -1;
                }

                // Reserve a node. Mirrors the reference allocator: reuse a recycled node
                // when one is available (releasing its old chain, which may cascade),
                // otherwise take the next free arena slot, growing only when exhausted.
                private int Allocate(int bits, int index, int offset, int chain)
                {
                    int node;
                    if (_ghostRoot != -1)
                    {
                        node = _ghostRoot;
                        _ghostRoot = _arena[node].GhostChain;
                        int oldChain = _arena[node].Chain;
                        if (oldChain != -1 && --_arena[oldChain].References == 0)
                        {
                            _arena[oldChain].GhostChain = _ghostRoot;
                            _ghostRoot = oldChain;
                        }
                    }
                    else
                    {
                        if (_count == _arena.Length)
                        {
                            Block[] grown = new Block[_arena.Length * 2];
                            System.Array.Copy(_arena, grown, _arena.Length);
                            _arena = grown;
                        }
                        node = _count++;
                    }
                    _arena[node].Bits = bits;
                    _arena[node].Index = index;
                    _arena[node].Offset = offset;
                    _arena[node].Chain = chain;
                    _arena[node].References = 0;
                    if (chain != -1)
                        _arena[chain].References++;
                    return node;
                }

                // Repoint a slot to a node. Mirrors the reference assign(): the new node
                // gains a reference and the previous occupant loses one, recycling it
                // onto the free list when its last reference is gone.
                private void Assign(ref int slot, int node)
                {
                    _arena[node].References++;
                    int old = slot;
                    if (old != -1 && --_arena[old].References == 0)
                    {
                        _arena[old].GhostChain = _ghostRoot;
                        _ghostRoot = old;
                    }
                    slot = node;
                }

                // Build the optimal parse and return the arena index of the final node,
                // or -1 for empty input. The selection logic matches the reference ZX0
                // optimizer exactly, so the encoded output stays byte-identical.
                public int Optimize(byte[] inputData, int inputSize, int skip, int offsetLimit)
                {
                    int maxOffset = OffsetCeiling(inputSize - 1, offsetLimit);

                    int[] lastLiteral = new int[maxOffset + 1];
                    int[] lastMatch = new int[maxOffset + 1];
                    int[] optimal = new int[inputSize];
                    int[] matchLength = new int[maxOffset + 1];
                    int[] bestLength = new int[inputSize];

                    for (int i = 0; i <= maxOffset; i++)
                    {
                        lastLiteral[i] = -1;
                        lastMatch[i] = -1;
                    }
                    for (int i = 0; i < inputSize; i++)
                        optimal[i] = -1;

                    if (inputSize > 2)
                        bestLength[2] = 2;

                    Assign(ref lastMatch[INITIAL_OFFSET], Allocate(-1, skip - 1, INITIAL_OFFSET, -1));

                    for (int index = skip; index < inputSize; index++)
                    {
                        int bestLengthSize = 2;
                        maxOffset = OffsetCeiling(index, offsetLimit);

                        for (int offset = 1; offset <= maxOffset; offset++)
                        {
                            if (index != skip && index >= offset && inputData[index] == inputData[index - offset])
                            {
                                if (lastLiteral[offset] != -1)
                                {
                                    int length = index - _arena[lastLiteral[offset]].Index;
                                    int bits = _arena[lastLiteral[offset]].Bits + 1 + EliasGammaBits(length);
                                    int candidate = Allocate(bits, index, offset, lastLiteral[offset]);
                                    Assign(ref lastMatch[offset], candidate);
                                    if (optimal[index] == -1 || _arena[optimal[index]].Bits > bits)
                                        Assign(ref optimal[index], candidate);
                                }

                                int currentMatchLength = ++matchLength[offset];
                                if (currentMatchLength > 1)
                                {
                                    if (bestLengthSize < currentMatchLength)
                                    {
                                        int bits = _arena[optimal[index - bestLength[bestLengthSize]]].Bits + EliasGammaBits(bestLength[bestLengthSize] - 1);
                                        int bits2;
                                        do
                                        {
                                            bestLengthSize++;
                                            bits2 = _arena[optimal[index - bestLengthSize]].Bits + EliasGammaBits(bestLengthSize - 1);
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
                                    int bitsForMatch = _arena[optimal[index - length]].Bits + 8 + EliasGammaBits((offset - 1) / 128 + 1) + EliasGammaBits(length - 1);
                                    if (lastMatch[offset] == -1 || _arena[lastMatch[offset]].Index != index || _arena[lastMatch[offset]].Bits > bitsForMatch)
                                    {
                                        int candidate = Allocate(bitsForMatch, index, offset, optimal[index - length]);
                                        Assign(ref lastMatch[offset], candidate);
                                        if (optimal[index] == -1 || _arena[optimal[index]].Bits > bitsForMatch)
                                            Assign(ref optimal[index], candidate);
                                    }
                                }
                            }
                            else
                            {
                                matchLength[offset] = 0;
                                if (lastMatch[offset] != -1)
                                {
                                    int length = index - _arena[lastMatch[offset]].Index;
                                    int bits = _arena[lastMatch[offset]].Bits + 1 + EliasGammaBits(length) + length * 8;
                                    int candidate = Allocate(bits, index, 0, lastMatch[offset]);
                                    Assign(ref lastLiteral[offset], candidate);
                                    if (optimal[index] == -1 || _arena[optimal[index]].Bits > bits)
                                        Assign(ref optimal[index], candidate);
                                }
                            }
                        }
                    }

                    return optimal[inputSize - 1];
                }

                // Emit the ZX0 bit stream for the parse rooted at the given node.
                public byte[] Encode(int root, byte[] inputData, int skip, bool backwardsMode, bool invertMode)
                {
                    if (root == -1)
                        return new byte[0];

                    int outputSize = (_arena[root].Bits + 25) / 8;
                    byte[] output = new byte[outputSize];

                    EncoderState state = new EncoderState();
                    state.OutputData = output;
                    state.OutputIndex = 0;
                    state.InputIndex = skip;
                    state.BitMask = 0;
                    state.Backtrack = true;

                    // Reverse the chain in place so it can be walked front to back.
                    int previous = -1;
                    int current = root;
                    while (current != -1)
                    {
                        int next = _arena[current].Chain;
                        _arena[current].Chain = previous;
                        previous = current;
                        current = next;
                    }

                    int lastOffset = INITIAL_OFFSET;
                    int prevNode = previous;

                    for (int node = _arena[prevNode].Chain; node != -1;)
                    {
                        int thisNode = node;
                        node = _arena[node].Chain;

                        int length = _arena[thisNode].Index - _arena[prevNode].Index;
                        int blockOffset = _arena[thisNode].Offset;

                        if (blockOffset == 0)
                        {
                            WriteBit(state, 0);
                            WriteInterlacedEliasGamma(state, length, backwardsMode, false);
                            for (int i = 0; i < length; i++)
                            {
                                WriteByte(state, inputData[state.InputIndex]);
                                state.InputIndex++;
                            }
                        }
                        else if (blockOffset == lastOffset)
                        {
                            WriteBit(state, 0);
                            WriteInterlacedEliasGamma(state, length, backwardsMode, false);
                            state.InputIndex += length;
                        }
                        else
                        {
                            WriteBit(state, 1);
                            WriteInterlacedEliasGamma(state, ((blockOffset - 1) / 128) + 1, backwardsMode, invertMode);
                            if (backwardsMode)
                                WriteByte(state, ((blockOffset - 1) % 128) << 1);
                            else
                                WriteByte(state, (127 - ((blockOffset - 1) % 128)) << 1);
                            state.Backtrack = true;
                            WriteInterlacedEliasGamma(state, length - 1, backwardsMode, false);
                            state.InputIndex += length;
                            lastOffset = blockOffset;
                        }

                        prevNode = thisNode;
                    }

                    WriteBit(state, 1);
                    WriteInterlacedEliasGamma(state, 256, backwardsMode, invertMode);

                    return output;
                }
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

            private static int ReadInterlacedEliasGamma(Energy.Base.Binary.BitReader reader, bool inverted)
            {
                int value = 1;
                while (reader.ReadBit() == 0)
                {
                    int bit = reader.ReadBit();
                    value = (value << 1) | (inverted ? bit ^ 1 : bit);
                }
                return value;
            }

            // Grow a decode buffer to at least the required length (power-of-two growth).
            private static byte[] Grow(byte[] buffer, int required)
            {
                int size = buffer.Length < 256 ? 256 : buffer.Length;
                while (size < required)
                    size <<= 1;
                byte[] grown = new byte[size];
                System.Array.Copy(buffer, grown, buffer.Length);
                return grown;
            }

            // Copy a back-reference of the given length from "offset" bytes back, one
            // byte at a time so that overlapping runs expand correctly. Returns the new
            // output length.
            private static int CopyMatch(ref byte[] output, int outputLength, int offset, int length)
            {
                if (offset <= 0 || offset > outputLength)
                    throw new InvalidOperationException("ZX0 invalid back-reference offset");
                if (outputLength + length > output.Length)
                    output = Grow(output, outputLength + length);
                int source = outputLength - offset;
                for (int i = 0; i < length; i++)
                    output[outputLength++] = output[source++];
                return outputLength;
            }

            // Return a byte[] trimmed to the produced length, avoiding a copy when exact.
            private static byte[] Trim(byte[] output, int outputLength)
            {
                if (outputLength == output.Length)
                    return output;
                byte[] result = new byte[outputLength];
                System.Array.Copy(output, result, outputLength);
                return result;
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

                    Optimizer optimizer = new Optimizer(InitialArenaCapacity(inputSize, offsetLimit));
                    int optimal = optimizer.Optimize(data, inputSize, skip, offsetLimit);
                    if (optimal == -1)
                        return new byte[0];

                    bool backwardsMode = false;
                    bool classicMode = false;
                    bool invertMode = !classicMode && !backwardsMode;

                    return optimizer.Encode(optimal, data, skip, backwardsMode, invertMode);
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

                    Energy.Base.Binary.BitReader reader = new Energy.Base.Binary.BitReader(data);
                    byte[] output = new byte[256];
                    int outputLength = 0;
                    int lastOffset = INITIAL_OFFSET;
                    bool classicMode = false;

                    while (true)
                    {
                        // COPY_LITERALS
                        int length = ReadInterlacedEliasGamma(reader, false);
                        if (outputLength + length > output.Length)
                            output = Grow(output, outputLength + length);
                        for (int i = 0; i < length; i++)
                            output[outputLength++] = (byte)reader.ReadByte();

                        if (reader.ReadBit() != 0)
                        {
                            // COPY_FROM_NEW_OFFSET sequence
                            while (true)
                            {
                                int offsetMsb = ReadInterlacedEliasGamma(reader, !classicMode);
                                if (offsetMsb == 256)
                                    return Trim(output, outputLength);

                                lastOffset = offsetMsb * 128 - (reader.ReadByte() >> 1);
                                reader.SetBacktrack();
                                length = ReadInterlacedEliasGamma(reader, false) + 1;
                                outputLength = CopyMatch(ref output, outputLength, lastOffset, length);

                                if (reader.ReadBit() == 0)
                                    break; // back to literals
                            }

                            continue;
                        }

                        // COPY_FROM_LAST_OFFSET
                        length = ReadInterlacedEliasGamma(reader, false);
                        outputLength = CopyMatch(ref output, outputLength, lastOffset, length);

                        if (reader.ReadBit() != 0)
                        {
                            // fall-through into COPY_FROM_NEW_OFFSET chain
                            while (true)
                            {
                                int offsetMsb = ReadInterlacedEliasGamma(reader, !classicMode);
                                if (offsetMsb == 256)
                                    return Trim(output, outputLength);

                                lastOffset = offsetMsb * 128 - (reader.ReadByte() >> 1);
                                reader.SetBacktrack();
                                length = ReadInterlacedEliasGamma(reader, false) + 1;
                                outputLength = CopyMatch(ref output, outputLength, lastOffset, length);

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

        #region LZ4

        /// <summary>
        /// LZ4 block format compression and decompression.
        /// <br/><br/>
        /// Implements the raw LZ4 block format as specified by the reference implementation.
        /// </summary>
        public class LZ4
        {
            #region Constants

            private const int HASH_LOG = 14;
            private const int HASH_SIZE = 1 << HASH_LOG;
            private const int HASH_MASK = HASH_SIZE - 1;
            private const int MIN_MATCH = 4;
            private const int LAST_LITERALS = 5;
            private const int MIN_MATCH_LENGTH = 4;
            private const int MAX_OFFSET = 65535;
            private const int ML_BITS = 4;
            private const int ML_MASK = (1 << ML_BITS) - 1;
            private const int RUN_BITS = 4;
            private const int RUN_MASK = (1 << RUN_BITS) - 1;

            #endregion

            #region Compress

            /// <summary>
            /// Compress data using the LZ4 block format.
            /// </summary>
            /// <param name="data">Uncompressed input data</param>
            /// <returns>Compressed LZ4 block</returns>
            public static byte[] Compress(byte[] data)
            {
                if (data == null)
                {
                    return null;
                }
                if (data.Length == 0)
                {
                    return new byte[0];
                }
                if (data.Length < 13)
                {
                    return CompressLiteralsOnly(data);
                }

                try
                {
                    int inputSize = data.Length;
                    int[] hashTable = new int[HASH_SIZE];
                    for (int i = 0; i < hashTable.Length; i++)
                    {
                        hashTable[i] = -1;
                    }

                    System.Collections.Generic.List<byte> output = new System.Collections.Generic.List<byte>();
                    int ip = 0;
                    int literalStart = 0;
                    int lastMatchPos = inputSize - LAST_LITERALS;

                    while (ip < lastMatchPos)
                    {
                        int bestMatchLen = 0;
                        int bestMatchOff = 0;

                        if (ip + MIN_MATCH <= inputSize)
                        {
                            int hash = Hash4(data, ip);
                            int refPos = hashTable[hash];
                            hashTable[hash] = ip;

                            if (refPos >= 0 && refPos < ip && (ip - refPos) <= MAX_OFFSET)
                            {
                                int matchLen = CountMatch(data, refPos, ip, inputSize);
                                if (matchLen >= MIN_MATCH)
                                {
                                    bestMatchLen = matchLen;
                                    bestMatchOff = ip - refPos;
                                }
                            }
                        }

                        if (bestMatchLen >= MIN_MATCH)
                        {
                            int literalLen = ip - literalStart;
                            int matchLenCode = bestMatchLen - MIN_MATCH;
                            int litToken = literalLen < RUN_MASK ? literalLen : RUN_MASK;
                            int mlToken = matchLenCode < ML_MASK ? matchLenCode : ML_MASK;
                            output.Add((byte)((litToken << ML_BITS) | mlToken));

                            if (literalLen >= RUN_MASK)
                            {
                                WriteLength(output, literalLen - RUN_MASK);
                            }
                            for (int i = literalStart; i < ip; i++)
                            {
                                output.Add(data[i]);
                            }

                            output.Add((byte)(bestMatchOff & 0xFF));
                            output.Add((byte)((bestMatchOff >> 8) & 0xFF));

                            if (matchLenCode >= ML_MASK)
                            {
                                WriteLength(output, matchLenCode - ML_MASK);
                            }

                            ip += bestMatchLen;
                            literalStart = ip;

                            // Update hash table for skipped positions
                            for (int i = literalStart - bestMatchLen + 1; i < literalStart && i < lastMatchPos; i++)
                            {
                                hashTable[Hash4(data, i)] = i;
                            }
                        }
                        else
                        {
                            ip++;
                        }
                    }

                    // Final literals
                    int finalLen = inputSize - literalStart;
                    int finalLitToken = finalLen < RUN_MASK ? finalLen : RUN_MASK;
                    output.Add((byte)(finalLitToken << ML_BITS));
                    if (finalLen >= RUN_MASK)
                    {
                        WriteLength(output, finalLen - RUN_MASK);
                    }
                    for (int i = literalStart; i < inputSize; i++)
                    {
                        output.Add(data[i]);
                    }

                    return output.ToArray();
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                    return null;
                }
            }

            private static byte[] CompressLiteralsOnly(byte[] data)
            {
                System.Collections.Generic.List<byte> output = new System.Collections.Generic.List<byte>();
                int len = data.Length;
                int token = len < RUN_MASK ? len : RUN_MASK;
                output.Add((byte)(token << ML_BITS));
                if (len >= RUN_MASK)
                {
                    WriteLength(output, len - RUN_MASK);
                }
                for (int i = 0; i < len; i++)
                {
                    output.Add(data[i]);
                }
                return output.ToArray();
            }

            private static int Hash4(byte[] data, int index)
            {
                uint v = (uint)(data[index] | (data[index + 1] << 8) | (data[index + 2] << 16) | (data[index + 3] << 24));
                return (int)((v * 2654435761U) >> (32 - HASH_LOG)) & HASH_MASK;
            }

            private static int CountMatch(byte[] data, int refPos, int ip, int limit)
            {
                int matchLen = 0;
                int maxMatch = limit - ip;
                while (matchLen < maxMatch && data[refPos + matchLen] == data[ip + matchLen])
                {
                    matchLen++;
                }
                return matchLen;
            }

            private static void WriteLength(System.Collections.Generic.List<byte> output, int length)
            {
                while (length >= 255)
                {
                    output.Add(255);
                    length -= 255;
                }
                output.Add((byte)length);
            }

            #endregion

            #region Decompress

            /// <summary>
            /// Decompress data previously compressed with the LZ4 block format.
            /// </summary>
            /// <param name="data">Compressed LZ4 block</param>
            /// <returns>Decompressed original data</returns>
            public static byte[] Decompress(byte[] data)
            {
                if (data == null)
                {
                    return null;
                }
                if (data.Length == 0)
                {
                    return new byte[0];
                }

                try
                {
                    System.Collections.Generic.List<byte> output = new System.Collections.Generic.List<byte>();
                    int ip = 0;
                    int inputSize = data.Length;

                    while (ip < inputSize)
                    {
                        int token = data[ip++];
                        int literalLen = token >> ML_BITS;
                        if (literalLen == RUN_MASK)
                        {
                            literalLen += ReadLength(data, ref ip);
                        }

                        // Copy literals
                        for (int i = 0; i < literalLen; i++)
                        {
                            if (ip >= inputSize) break;
                            output.Add(data[ip++]);
                        }

                        // Check if this is the last sequence (only literals)
                        if (ip >= inputSize)
                        {
                            break;
                        }

                        // Read offset
                        if (ip + 1 >= inputSize) break;
                        int offset = data[ip] | (data[ip + 1] << 8);
                        ip += 2;

                        if (offset == 0)
                        {
                            break; // Invalid/corrupted block
                        }

                        // Read match length
                        int matchLen = (token & ML_MASK) + MIN_MATCH;
                        if ((token & ML_MASK) == ML_MASK)
                        {
                            matchLen += ReadLength(data, ref ip);
                        }

                        // Copy match
                        int copyFrom = output.Count - offset;
                        if (copyFrom < 0)
                        {
                            break; // Invalid offset
                        }

                        for (int i = 0; i < matchLen; i++)
                        {
                            output.Add(output[copyFrom + i]);
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

            private static int ReadLength(byte[] data, ref int ip)
            {
                int length = 0;
                while (ip < data.Length)
                {
                    int b = data[ip++];
                    length += b;
                    if (b < 255)
                    {
                        break;
                    }
                }
                return length;
            }

            #endregion
        }

        #endregion
    }
}
