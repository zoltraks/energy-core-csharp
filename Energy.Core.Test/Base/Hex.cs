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
        public void HexToByteArray()
        {
            string hex8 = "01234567";
            string hex8a = "89abcdef";
            string hex8A = "89ABCDEF";
            string hex16 = "0123456789abcdef";
            string hex15 = "123456789abcdef";

            byte[] except, result;
            string value;

            value = hex8;
            except = new byte[] { 0x01, 0x23, 0x45, 0x67 };
            result = Energy.Base.Hex.HexToByteArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex8a;
            except = new byte[] { 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToByteArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex8A;
            except = new byte[] { 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToByteArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex16;
            except = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToByteArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);

            value = hex15;
            except = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            result = Energy.Base.Hex.HexToByteArray(value);
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(except, result), value);
        }
    }
}
