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
    }
}