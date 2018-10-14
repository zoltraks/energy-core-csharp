using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Url
    {
        [TestMethod]
        public void UrlCheck()
        {
            string input, output;
            input = "http://localhost:80/path?query#fragment";
            Energy.Base.Url url;
            url = input;
            Assert.AreEqual("localhost", url.Host);
            Assert.AreEqual("80", url.Port);
            Assert.AreEqual("/path", url.Path);
            Assert.AreEqual("query", url.Query);
            Assert.AreEqual("fragment", url.Fragment);
            output = url.ToString();
            Assert.AreEqual(input, output);
            url = url.Combine(new Energy.Base.Url() { Host = "127.0.0.1" }, true);
            output = url.ToString();
            Assert.AreEqual("http://127.0.0.1:80/path?query#fragment", output);
            input = "localhost/path";
            url = input;
            Assert.AreEqual("localhost", url.Host);
            Assert.AreEqual("/path", url.Path);
            url = url.Combine(new Energy.Base.Url() { Scheme = "http", Host = "127.0.0.1" }, false);
            Assert.AreEqual("localhost", url.Host);
            Assert.AreEqual("http", url.Scheme);
        }
    }
}
