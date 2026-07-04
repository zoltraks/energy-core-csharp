using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Http
    {
        [TestMethod]
        public void HttpRequestDataBody()
        {
            Energy.Base.Http.Request request = new Energy.Base.Http.Request();
            request.Data = new byte[] { (byte)'A', (byte)'B', };
            Assert.AreEqual("AB", request.Body);
        }
    }
}
