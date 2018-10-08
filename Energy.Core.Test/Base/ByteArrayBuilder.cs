using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class ByteArrayBuilder
    {
        [TestMethod]
        public void ByteArrayBuilderSequence()
        {
            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            Assert.IsTrue(b.Empty);
            b.WriteByte(1);
            b.WriteByte(2);
            byte t;
            b.Rewind();
            t = b.ReadByte();
            Assert.AreEqual(1, t);
            b.Tail();
            b.WriteByte(3);
            b.Seek(1);
            t = b.ReadByte();
            Assert.AreEqual(2, t);
            t = b.ReadByte();
            Assert.AreEqual(3, t);
            Assert.IsTrue(b.End);
        }

        [TestMethod]
        public void ByteArrayBuilderBase64()
        {
            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            string s1 = "TWljcm9zb2Z0IFdpbmRvd3M=";
            b.WriteBase64(s1);
            b.Rewind();
            string s2 = b.ReadString();
            Assert.AreEqual("Microsoft Windows", s2);
            b.Clear();
            string s3 = "Gęś";
            b.WriteString(s3);
            b.Rewind();
            string s4 = b.ReadBase64();
            string s5 = "R8SZxZs=";
            Assert.AreEqual(s5, s4);
        }

        [TestMethod]
        public void ByteArrayBuilderToByteArrayFromInt()
        {
            int[] value;
            byte[] expect;
            byte[] result;

            value = new int[] { 1, 2, 3, 4 };
            result = Energy.Base.ByteArrayBuilder.ToByteArray(value);
            expect = new byte[] { 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 4 };
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(expect, result));
            value = new int[] { 1, 2, 3, 4 };
            result = Energy.Base.ByteArrayBuilder.ToByteArray(value, true);
            expect = new byte[] { 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0 };
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(expect, result));
            value = new int[] { 1, 2, 3, 4 };
            result = Energy.Base.ByteArrayBuilder.ToByteArray(value, 2);
            expect = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4 };
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(expect, result));
            value = new int[] { -1, -2, -3, -4 };
            result = Energy.Base.ByteArrayBuilder.ToByteArray(value, 2, true);
            expect = new byte[] { 255, 255, 254, 255, 253, 255, 252, 255 };
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(expect, result));
            result = Energy.Base.ByteArrayBuilder.ToByteArray(value, 3, false);
            expect = new byte[] { 255, 255, 255, 255, 255, 254, 255, 255, 253, 255, 255, 252 };
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(expect, result));
            value = new int[] { -2 };
            result = Energy.Base.ByteArrayBuilder.ToByteArray(value, true);
            expect = new byte[] { 254, 255, 255, 255 };
            Assert.IsTrue(Energy.Base.ByteArrayBuilder.AreEqual(expect, result));
        }
    }
}
