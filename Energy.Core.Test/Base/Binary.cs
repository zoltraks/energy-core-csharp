using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Binary
    {
        [TestMethod]
        public void Endianess()
        {
            uint _uint32;
            int _int32;

            // 4294967296

            _uint32 = Energy.Base.Binary.GetUInt32MSB(49440);
            Assert.AreEqual((uint)49440, _uint32);
            _int32 = (int)_uint32;
            Assert.AreNotEqual(-16672, _int32);
            _uint32 = Energy.Base.Binary.GetUInt32MSB();
            Assert.AreEqual((uint)0, _uint32);
            _uint32 = Energy.Base.Binary.GetUInt32MSB(5085, 49440);
            Assert.AreEqual((uint)333300000, _uint32);
            _uint32 = Energy.Base.Binary.GetUInt32MSB(49440, 5085);
            Assert.AreEqual((uint)3240104925, _uint32);
            _int32 = (int)_uint32;
            Assert.AreEqual((int)-1054862371, _int32);
        }

        [TestMethod]
        public void Xor()
        {
            byte[] a, b, c, d;
            a = null;
            b = null;
            c = Energy.Base.Binary.Xor(a, b);
            Assert.IsNull(c);
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = null;
            c = Energy.Base.Binary.Xor(a, b);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, a));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b00 };
            c = Energy.Base.Binary.Xor(a, b);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, a));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b01 };
            c = Energy.Base.Binary.Xor(a, b);
            d = new byte[] { 0b01, 0b00, 0b11, 0b10 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b01, 0b10 };
            c = Energy.Base.Binary.Xor(a, b);
            d = new byte[] { 0b01, 0b11, 0b11, 0b01 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b01, 0b10, 0b01, 0b10, 0b11 };
            c = Energy.Base.Binary.Xor(a, b);
            d = new byte[] { 0b01, 0b11, 0b11, 0b01 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b10000000, 0b10000001, 0b10, 0b11 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            c = Energy.Base.Binary.Xor(a, b);
            d = new byte[] { 0b01, 0b11, 0b01010111, 0b10101001 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            d = new byte[] { 0b11111111, 0b11111110, 0b11111101, 0b11111100 };
            c = Energy.Base.Binary.Xor(a, new byte[] { 0xff });
        }

        [TestMethod]
        public void Not()
        {
            byte[] a, c, d;
            a = null;
            c = Energy.Base.Binary.Not(a);
            Assert.IsNull(c);
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            d = new byte[] { 0b11111111, 0b11111110, 0b11111101, 0b11111100 };
            c = Energy.Base.Binary.Not(a);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
        }

        [TestMethod]
        public void Or()
        {
            byte[] a, b, c, d;
            a = null;
            b = null;
            c = Energy.Base.Binary.Or(a, b);
            Assert.IsNull(c);
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b10000001, 0b10000011, 0b01010111, 0b10101011 };
            c = Energy.Base.Binary.Or(a, b);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b10000000, 0b10000001, 0b00010010, 0b00100011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b10000001, 0b10000011, 0b01010111, 0b10101011 };
            c = Energy.Base.Binary.Or(a, b);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
        }

        [TestMethod]
        public void And()
        {
            byte[] a, b, c, d;
            a = null;
            b = null;
            c = Energy.Base.Binary.And(a, b);
            Assert.IsNull(c);
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b00000000, 0b00000000, 0b00000000, 0b00000010 };
            c = Energy.Base.Binary.And(a, b);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b10000000, 0b10000001, 0b00010010, 0b00100011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b10000000, 0b10000000, 0b00010000, 0b00100010 };
            c = Energy.Base.Binary.And(a, b);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
        }

        [TestMethod]
        public void Ror()
        {
            byte[] a, c, d;

            a = null;
            c = Energy.Base.Binary.Ror(a, 0);
            Assert.IsNull(c);
            c = Energy.Base.Binary.Ror(a, 1);
            Assert.IsNull(c);
            c = Energy.Base.Binary.Ror(a, -1);
            Assert.IsNull(c);

            for (int i = 0; i < 16; i++)
            {
                a = new byte[] { 0b00000000 };
                c = Energy.Base.Binary.Ror(a, i);
                Assert.AreEqual(0, Energy.Base.Binary.Compare(c, a));
            }

            a = new byte[] { 0b00000001 };
            c = Energy.Base.Binary.Ror(a, 1);
            d = new byte[] { 0b10000000 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Ror(a, 9);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000001 };
            c = Energy.Base.Binary.Ror(a, 2);
            d = new byte[] { 0b01000000 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Ror(a, 10);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00000010 };
            d = new byte[] { 0b00000000, 0b10000001 };
            c = Energy.Base.Binary.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Ror(a, 17);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00000010 };
            c = Energy.Base.Binary.Ror(a, 8);
            d = new byte[] { 0b00000010, 0b00000001 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Ror(a, 24);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            d = new byte[] { 0b10000000, 0b00000000, 0b10000001, 0b00000001 };
            c = Energy.Base.Binary.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Binary.Ror(a, 2);
            d = new byte[] { 0b11000000, 0b00000000, 0b01000000, 0b10000000 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Binary.Ror(a, 3);
            d = new byte[] { 0b01100000, 0b00000000, 0b00100000, 0b01000000 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00010000, 0b00000100, 0b00000010 };
            d = new byte[] { 0b00010000, 0b00000100, 0b00000010, 0b00000001 };
            c = Energy.Base.Binary.Ror(a, 24);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b10100001, 0b01000000, 0b11010101, 0b00101010 };
            c = Energy.Base.Binary.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b01010000, 0b10100000, 0b01101010, 0b10010101 };
            c = Energy.Base.Binary.Ror(a, 2);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b00101010, 0b10100001, 0b01000000, 0b11010101 };
            c = Energy.Base.Binary.Ror(a, 9);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b11010101, 0b00101010, 0b10100001, 0b01000000 };
            c = Energy.Base.Binary.Ror(a, 17);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b00000011, 0b10111000, 0b01110000, 0b00001110 };
            c = Energy.Base.Binary.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b00000001, 0b11011100, 0b00111000, 0b00000111 };
            c = Energy.Base.Binary.Ror(a, 2);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b10000000, 0b11101110, 0b00011100, 0b00000011 };
            c = Energy.Base.Binary.Ror(a, 3);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b11000000, 0b01110111, 0b00001110, 0b00000001 };
            c = Energy.Base.Binary.Ror(a, 4);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b11100000, 0b00111011, 0b10000111, 0b00000000 };
            c = Energy.Base.Binary.Ror(a, 5);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b01110000, 0b00011101, 0b11000011, 0b10000000 };
            c = Energy.Base.Binary.Ror(a, 6);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b00111000, 0b00001110, 0b11100001, 0b11000000 };
            c = Energy.Base.Binary.Ror(a, 7);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b10000111, 0b00000000, 0b11100000, 0b00111011 };
            c = Energy.Base.Binary.Ror(a, 21);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000000, 0b00010000, 0b00000000 };
            d = new byte[] { 0b00000000, 0b00000000, 0b01000000 };
            c = Energy.Base.Binary.Ror(a, 6);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
        }

        [TestMethod]
        public void Rol()
        {
            byte[] a, c, d;

            a = null;
            c = Energy.Base.Binary.Rol(a, 0);
            Assert.IsNull(c);
            c = Energy.Base.Binary.Rol(a, 1);
            Assert.IsNull(c);
            c = Energy.Base.Binary.Rol(a, -1);
            Assert.IsNull(c);

            for (int i = 0; i < 16; i++)
            {
                a = new byte[] { 0b00000000 };
                c = Energy.Base.Binary.Rol(a, i);
                Assert.AreEqual(0, Energy.Base.Binary.Compare(c, a));
            }

            a = new byte[] { 0b10000001, 0b01000010, 0b11000011 };
            c = Energy.Base.Binary.Rol(a, 8);
            d = new byte[] { 0b01000010, 0b11000011, 0b10000001 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Rol(a, 16);
            d = new byte[] { 0b11000011, 0b10000001, 0b01000010 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000001 };
            c = Energy.Base.Binary.Rol(a, 1);
            d = new byte[] { 0b00000010 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Rol(a, 9);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            a = new byte[] { 0b00000001 };
            c = Energy.Base.Binary.Rol(a, 2);
            d = new byte[] { 0b00000100 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Rol(a, 10);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000001, 0b10000010 };
            c = Energy.Base.Binary.Rol(a, 1);
            d = new byte[] { 0b00000011, 0b00000100 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Rol(a, 17);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00000010 };
            c = Energy.Base.Binary.Rol(a, 8);
            d = new byte[] { 0b00000010, 0b00000001 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
            c = Energy.Base.Binary.Rol(a, 24);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b10000000, 0b10000001, 0b00000010, 0b10000011 };
            c = Energy.Base.Binary.Rol(a, 1);
            d = new byte[] { 0b00000001, 0b00000010, 0b00000101, 0b00000111 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b11000000, 0b00000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Binary.Rol(a, 2);
            d = new byte[] { 0b00000000, 0b00000100, 0b00001000, 0b00001111 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b01000000, 0b01000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Binary.Rol(a, 3);
            d = new byte[] { 0b00000010, 0b00001000, 0b00010000, 0b00011010 };
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00010000, 0b00000000 };
            d = new byte[] { 0b00000100, 0b00000000, 0b00000000 };
            c = Energy.Base.Binary.Rol(a, 6);
            Assert.AreEqual(0, Energy.Base.Binary.Compare(c, d));
        }

        [TestMethod]
        public void Binary_BitWriter_DefaultBitPacking()
        {
            byte[] data;
            using (MemoryStream stream = new MemoryStream())
            {
                Energy.Base.Binary.BitWriter writer = new Energy.Base.Binary.BitWriter(stream);
                int[] bits = new int[] { 1, 0, 1, 1, 0, 1, 0, 0 };
                for (int i = 0; i < bits.Length; i++)
                {
                    writer.WriteBit(bits[i]);
                }

                writer.WriteByte(0xAA);
                writer.WriteBit(1);
                writer.WriteBit(0);
                writer.Flush();

                Assert.AreEqual(0, writer.GetLastBit(), "Last bit should reflect most recent write");
                data = stream.ToArray();
            }

            Assert.AreEqual(3, data.Length, "Stream should contain two full bytes and one flushed partial byte");
            Assert.AreEqual(0xB4, data[0], "First byte should match MSB-first packed bits");
            Assert.AreEqual(0xAA, data[1], "Literal byte should be preserved");
            Assert.AreEqual(0x80, data[2], "Remaining bits should be flushed into high bits of last byte");
        }

        [TestMethod]
        public void Binary_BitReader_LsbBacktrack()
        {
            byte[] buffer;
            Energy.Base.Binary.BitWriter.Options writerOptions = new Energy.Base.Binary.BitWriter.Options
            {
                MostSignificantBitFirst = false,
                AllowLiteralInterleave = false,
                LeaveStreamOpen = true
            };

            using (MemoryStream stream = new MemoryStream())
            {
                Energy.Base.Binary.BitWriter writer = new Energy.Base.Binary.BitWriter(stream, writerOptions);
                int[] bits = new int[] { 1, 0, 1, 1, 0, 1, 0, 0 };
                for (int i = 0; i < bits.Length; i++)
                {
                    writer.WriteBit(bits[i]);
                }

                writer.WriteByte(0x11);

                int[] trailerBits = new int[] { 1, 0, 1, 0 };
                for (int i = 0; i < trailerBits.Length; i++)
                {
                    writer.WriteBit(trailerBits[i]);
                }

                writer.Flush();
                buffer = stream.ToArray();
            }

            Assert.AreEqual(3, buffer.Length, "Writer should emit two full bytes and one partial byte in LSB mode");
            Assert.AreEqual(0x2D, buffer[0], "LSB-first packed byte should match expected pattern");
            Assert.AreEqual(0x11, buffer[1], "Literal byte should match written value");
            Assert.AreEqual(0x05, buffer[2], "Trailer bits should flush into final byte in LSB order");

            Energy.Base.Binary.BitReader.Options readerOptions = new Energy.Base.Binary.BitReader.Options
            {
                MostSignificantBitFirst = false
            };

            Energy.Base.Binary.BitReader reader = new Energy.Base.Binary.BitReader(buffer, 0, buffer.Length, readerOptions);

            int[] readBits = new int[8];
            for (int i = 0; i < readBits.Length; i++)
            {
                readBits[i] = reader.ReadBit();
            }
            CollectionAssert.AreEqual(new int[] { 1, 0, 1, 1, 0, 1, 0, 0 }, readBits, "Reader should reconstruct LSB bit order correctly");

            int literal = reader.ReadByte();
            Assert.AreEqual(0x11, literal, "Literal round-trip mismatch");

            reader.SetBacktrack();
            int backtrackBit = reader.ReadBit();
            Assert.AreEqual(literal & 1, backtrackBit, "Backtrack should surface lowest bit of literal byte");

            int[] trailerRead = new int[4];
            for (int i = 0; i < trailerRead.Length; i++)
            {
                trailerRead[i] = reader.ReadBit();
            }
            CollectionAssert.AreEqual(new int[] { 1, 0, 1, 0 }, trailerRead, "Remaining trailer bits should be readable after backtrack");
        }

        [TestMethod]
        public void Binary_RolScalarVariants()
        {
            byte byteInput = 0b10000001;
            Assert.AreEqual((byte)0b00000011, Energy.Base.Binary.Rol(byteInput, 1), "Byte left rotation should move high bit to low end");
            Assert.AreEqual((byte)0b11000000, Energy.Base.Binary.Rol(byteInput, -1), "Negative count should delegate to right rotation");
            Assert.AreEqual(byteInput, Energy.Base.Binary.Rol(byteInput, 8), "Full rotations should yield original value");

            uint uintInput = 0x80000001u;
            Assert.AreEqual(0x00000006u, Energy.Base.Binary.Rol(uintInput, 2), "32-bit rotation should wrap upper bits");
            Assert.AreEqual(0x80000001u, Energy.Base.Binary.Rol(uintInput, 64), "Counts greater than width should wrap modulo bit size");
            Assert.AreEqual(0x60000000u, Energy.Base.Binary.Rol(uintInput, -2), "Negative rotation should call right rotation");

            ulong ulongInput = 0x8000000000000001ul;
            Assert.AreEqual(0x0000000000000003ul, Energy.Base.Binary.Rol(ulongInput, 1), "64-bit rotation should wrap high bit");
            Assert.AreEqual(0x8000000000000001ul, Energy.Base.Binary.Rol(ulongInput, 64), "Exact width rotations should be no-ops");
            Assert.AreEqual((byte)0b00000011, Energy.Base.Binary.Rol(byteInput), "Single-parameter byte overload should rotate by one");
            Assert.AreEqual(0x00000003u, Energy.Base.Binary.Rol(uintInput), "Single-parameter uint overload should rotate by one");
            Assert.AreEqual(0x0000000000000003ul, Energy.Base.Binary.Rol(ulongInput), "Single-parameter ulong overload should rotate by one");
        }

        [TestMethod]
        public void Binary_RorScalarVariants()
        {
            byte byteInput = 0b00000011;
            Assert.AreEqual((byte)0b10000001, Energy.Base.Binary.Ror(byteInput, 1), "Byte right rotation should move low bit to high end");
            Assert.AreEqual((byte)0b00001100, Energy.Base.Binary.Ror(byteInput, -2), "Negative count should call left rotation");

            uint uintInput = 0x00000003u;
            Assert.AreEqual(0xC0000000u, Energy.Base.Binary.Ror(uintInput, 2), "32-bit right rotation should wrap lower bits to upper bits");
            Assert.AreEqual(uintInput, Energy.Base.Binary.Ror(uintInput, 32), "Full-width rotation should be identity");

            ulong ulongInput = 0x0000000000000003ul;
            Assert.AreEqual(0xC000000000000000ul, Energy.Base.Binary.Ror(ulongInput, 2), "64-bit right rotation should wrap");
            Assert.AreEqual(0xC000000000000000ul, Energy.Base.Binary.Ror(ulongInput, 66), "Counts greater than width should wrap modulo bit size");
            Assert.AreEqual((byte)0b10000001, Energy.Base.Binary.Ror(byteInput), "Single-parameter byte overload should rotate right by one");
            Assert.AreEqual(0x80000001u, Energy.Base.Binary.Ror(uintInput), "Single-parameter uint overload should rotate right by one");
            Assert.AreEqual(0x8000000000000001ul, Energy.Base.Binary.Ror(ulongInput), "Single-parameter ulong overload should rotate right by one");
        }
    }
}
