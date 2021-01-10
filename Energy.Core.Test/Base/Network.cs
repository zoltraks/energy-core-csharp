using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Network
    {
        [TestMethod]
        public void IsValidAddress()
        {
            Assert.IsFalse(Energy.Base.Network.IsValidAddress(null));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress(""));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("localhost"));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("."));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("300.300.300.300"));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("1.1.1"));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("1.1"));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("1.1.1.1."));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("..."));
            Assert.IsFalse(Energy.Base.Network.IsValidAddress("256.256.256.256"));

            Assert.IsTrue(Energy.Base.Network.IsValidAddress("0.0.0.0"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("1.1.1.1"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("01.01.01.01"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("001.001.001.001"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("10.10.10.10"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("255.255.255.255"));

            Assert.IsTrue(Energy.Base.Network.IsValidAddress("2001:0db8:85a3:0000:0000:8a2e:0370:7334"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("FE80:0000:0000:0000:0202:B3FF:FE1E:8329"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("::1"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("::"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("[2001:0db8:85a3:0000:0000:8a2e:0370:7334]"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("[FE80:0000:0000:0000:0202:B3FF:FE1E:8329]"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("[::1]"));
            Assert.IsTrue(Energy.Base.Network.IsValidAddress("[::]"));
        }
    }
}