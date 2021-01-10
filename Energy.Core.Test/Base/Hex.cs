using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Hex
    {
        [TestMethod]
        public void HexToArray()
        {
            string hex1 = "a";
            string hex3 = "abc";
            string hex8 = "01234567";
            string hex8a = "89abcdef";
            string hex8A = "89ABCDEF";
            string hex16 = "0123456789abcdef";
            string hex15 = "123456789abcdef";
            string hexBad = "z 1az";

            byte[] except, result;
            string value;

            result = Energy.Base.Hex.HexToArray(null);
            Assert.IsNull(result);

            result = Energy.Base.Hex.HexToArray("");
            Assert.AreEqual(0, result.Length);

            value = hex1;
            except = new byte[] { 0xa };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex3;
            except = new byte[] { 0xa, 0xbc };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex8;
            except = new byte[] { 0x01, 0x23, 0x45, 0x67 };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex8a;
            except = new byte[] { 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex8A;
            except = new byte[] { 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex16;
            except = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex15;
            except = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hexBad;
            except = new byte[] { 0x0, 0x1, 0xa0 };
            result = Energy.Base.Hex.HexToArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hexBad;
            except = new byte[] { 0x1, 0xa0 };
            result = Energy.Base.Hex.HexToArray(value, true);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);
        }

        [TestMethod]
        public void ArrayToHex()
        {
            byte[] b;
            string s;
            b = null;
            s = Energy.Base.Hex.ArrayToHex(b);
            Assert.AreEqual(null, s);
            b = new byte[] { };
            s = Energy.Base.Hex.ArrayToHex(b);
            Assert.AreEqual("", s);
            b = new byte[] { 0 };
            s = Energy.Base.Hex.ArrayToHex(b);
            Assert.AreEqual("00", s);
            b = new byte[] { 0, 127 };
            s = Energy.Base.Hex.ArrayToHex(b);
            Assert.AreEqual("007F", s);
        }

        [TestMethod]
        public void HexToNumberType()
        {
            object expect;
            object result;

            string hex;

            hex = "ffffffff";
            expect = -1;
            result = Energy.Base.Hex.HexToInteger(hex);
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void HexRemovePrefix()
        {
            string s;
            s = Energy.Base.Hex.RemovePrefix(null);
            Assert.IsNull(s);
            s = Energy.Base.Hex.RemovePrefix("");
            Assert.AreEqual("", s);
            s = Energy.Base.Hex.RemovePrefix("$");
            Assert.AreEqual("", s);
            s = Energy.Base.Hex.RemovePrefix("$dd");
            Assert.AreEqual("dd", s);
            s = Energy.Base.Hex.RemovePrefix("0x1");
            Assert.AreEqual("1", s);
            s = Energy.Base.Hex.RemovePrefix("0X");
            Assert.AreEqual("", s);
            s = Energy.Base.Hex.RemovePrefix("0X55");
            Assert.AreEqual("55", s);
        }

        [TestMethod]
        public void HexToBin()
        {
            string s;
            s = Energy.Base.Hex.HexToBin(null);
            Assert.AreEqual(null, s);
            s = Energy.Base.Hex.HexToBin("");
            Assert.AreEqual("", s);
            s = Energy.Base.Hex.HexToBin("0");
            Assert.AreEqual("0000", s);
            s = Energy.Base.Hex.HexToBin("00");
            Assert.AreEqual("00000000", s);
            s = Energy.Base.Hex.HexToBin("0xFF");
            Assert.AreEqual("11111111", s);
            s = Energy.Base.Hex.HexToBin("$c4");
            Assert.AreEqual("11000100", s);
        }

        [TestMethod]
        public void BinToHex()
        {
            string s;
            s = Energy.Base.Hex.BinToHex(null);
            Assert.AreEqual(null, s);
            s = Energy.Base.Hex.BinToHex("");
            Assert.AreEqual("", s);
            s = Energy.Base.Hex.BinToHex("0");
            Assert.AreEqual("0", s);
            s = Energy.Base.Hex.BinToHex("1");
            Assert.AreEqual("1", s);
            s = Energy.Base.Hex.BinToHex("01011000");
            Assert.AreEqual("58", s);
            s = Energy.Base.Hex.BinToHex("111101");
            Assert.AreEqual("3D", s);
        }

        [TestMethod]
        public void ByteToHex()
        {
            string s;
            s = Energy.Base.Hex.ByteToHex(0);
            Assert.AreEqual("00", s);
            s = Energy.Base.Hex.ByteToHex(127);
            Assert.AreEqual("7F", s);
        }

        [TestMethod]
        public void HexIsHex()
        {
            Assert.IsFalse(Energy.Base.Hex.IsHex(null));
            Assert.IsFalse(Energy.Base.Hex.IsHex(""));
            Assert.IsFalse(Energy.Base.Hex.IsHex("G"));
            Assert.IsFalse(Energy.Base.Hex.IsHex("0a-0b"));
            Assert.IsTrue(Energy.Base.Hex.IsHex("0123456789abcdefABCDEF"));
        }

        [TestMethod]
        public void HexToInteger()
        {
            string hex;
            foreach (string s in new string[] { null, "", "0", "00", "0000", "00000000", "ZZZ", "00000000000000000000000000000000" })
            {
                hex = s;
                Assert.AreEqual(0, Energy.Base.Hex.HexToInteger(hex));
            }
            hex = "   ffFFffFF  ";
            Assert.AreEqual(-1, Energy.Base.Hex.HexToInteger(hex));
            hex = "  100000000  ";
            Assert.AreEqual(0, Energy.Base.Hex.HexToInteger(hex));
        }

        [TestMethod]
        public void HexToByte()
        {
            string hex;
            foreach (string s in new string[] { null, "", "0", "00", "0000", "00000000", "ZZZ", "00000000000000000000000000000000" })
            {
                hex = s;
                Assert.AreEqual(0, Energy.Base.Hex.HexToByte(hex));
            }
            hex = "   ff  ";
            Assert.AreEqual(255, Energy.Base.Hex.HexToByte(hex));
            hex = "  100  ";
            Assert.AreEqual(0, Energy.Base.Hex.HexToByte(hex));
        }

        [TestMethod]
        public void HexToShort()
        {
            string hex;
            foreach (string s in new string[] { null, "", "0", "00", "0000", "00000000", "ZZZ", "00000000000000000000000000000000" })
            {
                hex = s;
                Assert.AreEqual(0, Energy.Base.Hex.HexToShort(hex));
            }
            hex = "   ffFF  ";
            Assert.AreEqual(-1, Energy.Base.Hex.HexToShort(hex));
            hex = "  10000  ";
            Assert.AreEqual(0, Energy.Base.Hex.HexToShort(hex));
        }

        [TestMethod]
        public void HexToLong()
        {
            string hex;
            foreach (string s in new string[] { null, "", "0", "00", "0000", "00000000", "ZZZ", "00000000000000000000000000000000" })
            {
                hex = s;
                Assert.AreEqual(0, Energy.Base.Hex.HexToLong(hex));
            }
            hex = "   ffFFffFFffFFffFF  ";
            Assert.AreEqual(-1, Energy.Base.Hex.HexToLong(hex));
            hex = "  10000000000000000  ";
            Assert.AreEqual(0, Energy.Base.Hex.HexToLong(hex));
        }
    }
}
