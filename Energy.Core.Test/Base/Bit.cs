using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Bit
    {
        [TestMethod]
        public void Endianess()
        {
            uint _uint32;
            int _int32;

            // 4294967296

            _uint32 = Energy.Base.Bit.GetUInt32MSB(49440);
            Assert.AreEqual((uint)49440, _uint32);
            _int32 = (int)_uint32;
            Assert.AreNotEqual(-16672, _int32);
            _uint32 = Energy.Base.Bit.GetUInt32MSB();
            Assert.AreEqual((uint)0, _uint32);
            _uint32 = Energy.Base.Bit.GetUInt32MSB(5085, 49440);
            Assert.AreEqual((uint)333300000, _uint32);
            _uint32 = Energy.Base.Bit.GetUInt32MSB(49440, 5085);
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
            c = Energy.Base.Bit.Xor(a, b);
            Assert.IsNull(c);
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = null;
            c = Energy.Base.Bit.Xor(a, b);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, a));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b00 };
            c = Energy.Base.Bit.Xor(a, b);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, a));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b01 };
            c = Energy.Base.Bit.Xor(a, b);
            d = new byte[] { 0b01, 0b00, 0b11, 0b10 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b01, 0b10 };
            c = Energy.Base.Bit.Xor(a, b);
            d = new byte[] { 0b01, 0b11, 0b11, 0b01 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00, 0b01, 0b10, 0b11 };
            b = new byte[] { 0b01, 0b10, 0b01, 0b10, 0b11 };
            c = Energy.Base.Bit.Xor(a, b);
            d = new byte[] { 0b01, 0b11, 0b11, 0b01 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b10000000, 0b10000001, 0b10, 0b11 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            c = Energy.Base.Bit.Xor(a, b);
            d = new byte[] { 0b01, 0b11, 0b01010111, 0b10101001 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            d = new byte[] { 0b11111111, 0b11111110, 0b11111101, 0b11111100 };
            c = Energy.Base.Bit.Xor(a, new byte[] { 0xff });
        }

        [TestMethod]
        public void Not()
        {
            byte[] a, c, d;
            a = null;
            c = Energy.Base.Bit.Not(a);
            Assert.IsNull(c);
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            d = new byte[] { 0b11111111, 0b11111110, 0b11111101, 0b11111100 };
            c = Energy.Base.Bit.Not(a);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
        }

        [TestMethod]
        public void Or()
        {
            byte[] a, b, c, d;
            a = null;
            b = null;
            c = Energy.Base.Bit.Or(a, b);
            Assert.IsNull(c);
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b10000001, 0b10000011, 0b01010111, 0b10101011 };
            c = Energy.Base.Bit.Or(a, b);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b10000000, 0b10000001, 0b00010010, 0b00100011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b10000001, 0b10000011, 0b01010111, 0b10101011 };
            c = Energy.Base.Bit.Or(a, b);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
        }

        [TestMethod]
        public void And()
        {
            byte[] a, b, c, d;
            a = null;
            b = null;
            c = Energy.Base.Bit.And(a, b);
            Assert.IsNull(c);
            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b00000000, 0b00000000, 0b00000000, 0b00000010 };
            c = Energy.Base.Bit.And(a, b);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b10000000, 0b10000001, 0b00010010, 0b00100011 };
            b = new byte[] { 0b10000001, 0b10000010, 0b01010101, 0b10101010 };
            d = new byte[] { 0b10000000, 0b10000000, 0b00010000, 0b00100010 };
            c = Energy.Base.Bit.And(a, b);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
        }

        [TestMethod]
        public void Ror()
        {
            byte[] a, c, d;

            a = null;
            c = Energy.Base.Bit.Ror(a, 0);
            Assert.IsNull(c);
            c = Energy.Base.Bit.Ror(a, 1);
            Assert.IsNull(c);
            c = Energy.Base.Bit.Ror(a, -1);
            Assert.IsNull(c);

            for (int i = 0; i < 16; i++)
            {
                a = new byte[] { 0b00000000 };
                c = Energy.Base.Bit.Ror(a, i);
                Assert.AreEqual(0, Energy.Base.Bit.Compare(c, a));
            }

            a = new byte[] { 0b00000001 };
            c = Energy.Base.Bit.Ror(a, 1);
            d = new byte[] { 0b10000000 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Ror(a, 9);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000001 };
            c = Energy.Base.Bit.Ror(a, 2);
            d = new byte[] { 0b01000000 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Ror(a, 10);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00000010 };
            d = new byte[] { 0b00000000, 0b10000001 };
            c = Energy.Base.Bit.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Ror(a, 17);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00000010 };
            c = Energy.Base.Bit.Ror(a, 8);
            d = new byte[] { 0b00000010, 0b00000001 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Ror(a, 24);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            d = new byte[] { 0b10000000, 0b00000000, 0b10000001, 0b00000001 };
            c = Energy.Base.Bit.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Bit.Ror(a, 2);
            d = new byte[] { 0b11000000, 0b00000000, 0b01000000, 0b10000000 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Bit.Ror(a, 3);
            d = new byte[] { 0b01100000, 0b00000000, 0b00100000, 0b01000000 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00010000, 0b00000100, 0b00000010 };
            d = new byte[] { 0b00010000, 0b00000100, 0b00000010, 0b00000001 };
            c = Energy.Base.Bit.Ror(a, 24);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b10100001, 0b01000000, 0b11010101, 0b00101010 };
            c = Energy.Base.Bit.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b01010000, 0b10100000, 0b01101010, 0b10010101 };
            c = Energy.Base.Bit.Ror(a, 2);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b00101010, 0b10100001, 0b01000000, 0b11010101 };
            c = Energy.Base.Bit.Ror(a, 9);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b01000010, 0b10000001, 0b10101010, 0b01010101 };
            d = new byte[] { 0b11010101, 0b00101010, 0b10100001, 0b01000000 };
            c = Energy.Base.Bit.Ror(a, 17);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b00000011, 0b10111000, 0b01110000, 0b00001110 };
            c = Energy.Base.Bit.Ror(a, 1);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b00000001, 0b11011100, 0b00111000, 0b00000111 };
            c = Energy.Base.Bit.Ror(a, 2);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b10000000, 0b11101110, 0b00011100, 0b00000011 };
            c = Energy.Base.Bit.Ror(a, 3);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b11000000, 0b01110111, 0b00001110, 0b00000001 };
            c = Energy.Base.Bit.Ror(a, 4);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b11100000, 0b00111011, 0b10000111, 0b00000000 };
            c = Energy.Base.Bit.Ror(a, 5);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b01110000, 0b00011101, 0b11000011, 0b10000000 };
            c = Energy.Base.Bit.Ror(a, 6);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b00111000, 0b00001110, 0b11100001, 0b11000000 };
            c = Energy.Base.Bit.Ror(a, 7);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000111, 0b01110000, 0b11100000, 0b00011100 };
            d = new byte[] { 0b10000111, 0b00000000, 0b11100000, 0b00111011 };
            c = Energy.Base.Bit.Ror(a, 21);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000000, 0b00010000, 0b00000000 };
            d = new byte[] { 0b00000000, 0b00000000, 0b01000000 };
            c = Energy.Base.Bit.Ror(a, 6);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
        }

        [TestMethod]
        public void Rol()
        {
            byte[] a, c, d;

            a = null;
            c = Energy.Base.Bit.Rol(a, 0);
            Assert.IsNull(c);
            c = Energy.Base.Bit.Rol(a, 1);
            Assert.IsNull(c);
            c = Energy.Base.Bit.Rol(a, -1);
            Assert.IsNull(c);

            for (int i = 0; i < 16; i++)
            {
                a = new byte[] { 0b00000000 };
                c = Energy.Base.Bit.Rol(a, i);
                Assert.AreEqual(0, Energy.Base.Bit.Compare(c, a));
            }

            a = new byte[] { 0b10000001, 0b01000010, 0b11000011 };
            c = Energy.Base.Bit.Rol(a, 8);
            d = new byte[] { 0b01000010, 0b11000011, 0b10000001 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Rol(a, 16);
            d = new byte[] { 0b11000011, 0b10000001, 0b01000010 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000001 };
            c = Energy.Base.Bit.Rol(a, 1);
            d = new byte[] { 0b00000010 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Rol(a, 9);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            a = new byte[] { 0b00000001 };
            c = Energy.Base.Bit.Rol(a, 2);
            d = new byte[] { 0b00000100 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Rol(a, 10);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000001, 0b10000010 };
            c = Energy.Base.Bit.Rol(a, 1);
            d = new byte[] { 0b00000011, 0b00000100 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Rol(a, 17);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000001, 0b00000010 };
            c = Energy.Base.Bit.Rol(a, 8);
            d = new byte[] { 0b00000010, 0b00000001 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
            c = Energy.Base.Bit.Rol(a, 24);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b10000000, 0b10000001, 0b00000010, 0b10000011 };
            c = Energy.Base.Bit.Rol(a, 1);
            d = new byte[] { 0b00000001, 0b00000010, 0b00000101, 0b00000111 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b11000000, 0b00000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Bit.Rol(a, 2);
            d = new byte[] { 0b00000000, 0b00000100, 0b00001000, 0b00001111 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b01000000, 0b01000001, 0b00000010, 0b00000011 };
            c = Energy.Base.Bit.Rol(a, 3);
            d = new byte[] { 0b00000010, 0b00001000, 0b00010000, 0b00011010 };
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));

            a = new byte[] { 0b00000000, 0b00010000, 0b00000000 };
            d = new byte[] { 0b00000100, 0b00000000, 0b00000000 };
            c = Energy.Base.Bit.Rol(a, 6);
            Assert.AreEqual(0, Energy.Base.Bit.Compare(c, d));
        }
    }
}