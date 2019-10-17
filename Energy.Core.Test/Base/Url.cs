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
            input = "host:1";
            url = input;
            output = url.ToString();
            Assert.AreEqual(output, input);
            Assert.AreEqual("host", url.Host);
            Assert.AreEqual("1", url.Port);
            input = "localhost/path";
            url = input;
            Assert.AreEqual("localhost", url.Host);
            Assert.AreEqual("/path", url.Path);
            url = url.Combine(new Energy.Base.Url() { Scheme = "http", Host = "127.0.0.1" }, false);
            Assert.AreEqual("localhost", url.Host);
            Assert.AreEqual("http", url.Scheme);
            input = ".";
            url = Energy.Base.Url.Explode(input);
            Assert.AreEqual(".", url.Host);
            Assert.AreEqual("", url.Port);
            Assert.AreEqual("", url.Scheme);
            input = "mailto:a@b.pl";
            url = Energy.Base.Url.Explode(input);
            Assert.AreEqual(url.ToString(), input);
            Assert.AreEqual("b.pl", url.Host);
            Assert.AreEqual("a", url.User);
            input = "http://df.ws/123";
            url = input;
            Assert.AreEqual(url.ToString(), input);
            input = "http://userid:password@example.com:8080";
            url = input;
            Assert.AreEqual(url.ToString(), input);
            input = "http://userid:password@example.com:8080/";
            url = input;
            Assert.AreEqual(url.ToString(), input);
            input = "http://a.ws/䨹";
            url = input;
            Assert.AreEqual(url.ToString(), input);
        }

        [TestMethod]
        public void UrlSet()
        {
            Energy.Base.Url url;
            url = "";
            Assert.IsTrue(url.IsEmpty);
            url = Energy.Base.Url.SetHostAndPort(url.ToString(), ".", null);
            Assert.IsFalse(url.IsEmpty);
            Assert.AreEqual(".", url.Host);
            Assert.AreEqual("", url.Port);
            url = Energy.Base.Url.SetHostAndPort(url.ToString(), ".", "");
            Assert.AreEqual("", url.Port);
            url = Energy.Base.Url.SetHostAndPort(url.ToString(), ".", "123");
            Assert.AreEqual("123", url.Port);
            url = Energy.Base.Url.SetHost(url.ToString(), "");
            Assert.AreEqual("", url.Host);
            url = Energy.Base.Url.SetHost(url.ToString(), "a");
            Assert.AreEqual("a", url.Host);
            url = Energy.Base.Url.SetPort(url.ToString(), "80");
            Assert.AreEqual("80", url.Port);
            url = Energy.Base.Url.SetPort(url.ToString(), null);
            Assert.AreEqual("", url.Port);
        }
    }
}
