using System;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Network
    {
        [TestMethod]
        public void GetHostAddress()
        {
            Assert.IsNull(Energy.Core.Network.GetHostAddress(null));
            Assert.AreEqual("", Energy.Core.Network.GetHostAddress(""));
            string result;
            result = Energy.Core.Network.GetHostAddress("localhost");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAddressFamily()
        {
            Assert.AreEqual(AddressFamily.Unknown, Energy.Core.Network.GetAddressFamily(null));
            Assert.AreEqual(AddressFamily.Unknown, Energy.Core.Network.GetAddressFamily(""));
            Assert.AreEqual(AddressFamily.InterNetwork, Energy.Core.Network.GetAddressFamily("8.8.8.8"));
            Assert.AreEqual(AddressFamily.InterNetworkV6, Energy.Core.Network.GetAddressFamily("[2607:f0d0:1002:51::4]"));
        }
    }
}
