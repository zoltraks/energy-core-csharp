using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class ConnectionString
    {
        [TestMethod]
        public void ConnectionStringSafe()
        {
            string connStr;
            string result;
            string expect;
            connStr = "";
            result = Energy.Base.ConnectionString.Safe(connStr);
            expect = "";
            Assert.AreEqual(expect, result);
            connStr = "password=123";
            result = Energy.Base.ConnectionString.Safe(connStr);
            expect = "password=(***)";
            Assert.AreEqual(expect, result);
        }
    }
}
