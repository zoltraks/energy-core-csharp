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
            url = url.Overwrite(new Energy.Base.Url() { Host = "127.0.0.1" }, true);
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
            url = url.Overwrite(new Energy.Base.Url() { Scheme = "http", Host = "127.0.0.1" }, false);
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
            url = "http://localhost:1234/path";
            url = Energy.Base.Url.SetPort(url.ToString(), 0);
            Assert.AreEqual("http://localhost/path", url.ToString());
            url = Energy.Base.Url.SetPort(url.ToString(), 12345);
            Assert.AreEqual("http://localhost:12345/path", url.ToString());
            url = Energy.Base.Url.SetPort(url.ToString(), 99);
            Assert.AreEqual("http://localhost:99/path", url.ToString());
            url = Energy.Base.Url.SetPort(url.ToString(), 99999);
            Assert.AreEqual("http://localhost/path", url.ToString());
            url = url.Overwrite(new Energy.Base.Url() { Port = "1" }, false);
            Assert.AreEqual("http://localhost:1/path", url.ToString());
            url = url.Overwrite(new Energy.Base.Url() { Port = "2" }, false);
            Assert.AreEqual("http://localhost:1/path", url.ToString());
            url = url.Overwrite(new Energy.Base.Url() { Port = "2" }, true);
            Assert.AreEqual("http://localhost:2/path", url.ToString());
        }

        [TestMethod]
        public void UrlCombine()
        {
            string url;
            url = Energy.Base.Url.Combine(null);
            Assert.IsNull(url);
            url = Energy.Base.Url.Combine(new string[] { });
            Assert.AreEqual("", url);
            url = Energy.Base.Url.Combine("http://localhost:123", "path1", "path2");
            Assert.AreEqual("http://localhost:123/path1/path2", url);
            url = Energy.Base.Url.Combine("http://localhost:123/", "/path1/", "/path2");
            Assert.AreEqual("http://localhost:123//path1//path2", url);
            url = Energy.Base.Url.Combine("http://localhost:123", "/path1", "/path2");
            Assert.AreEqual("http://localhost:123/path1/path2", url);
            url = Energy.Base.Url.Combine("http://localhost:123", "/path1", "/path2?a", "b=c");
            Assert.AreEqual("http://localhost:123/path1/path2?a&b=c", url);
            url = Energy.Base.Url.Combine("http://localhost:123", "/path1", "/path2?", "a", "b");
            Assert.AreEqual("http://localhost:123/path1/path2?&a&b", url);
        }

        [TestMethod]
        public void UrlMake()
        {
            string url;
            url = Energy.Base.Url.Make("localhost:1234", "ftp", null, "33", "path", null, null, null, null, null);
            Assert.AreEqual("ftp://localhost:33/path", url);
            url = Energy.Base.Url.Make("a@b", "mailto:", null, null, null, null, null, null, null, null);
            Assert.AreEqual("mailto:a@b", url);
        }

        [TestMethod]
        public void UrlEncode()
        {
            string a, b;

            Assert.IsNull(Energy.Base.Url.Encode(null));
            Assert.AreEqual("", Energy.Base.Url.Encode(""));

            a = "❤";
            b = "%E2%9D%A4";
            Assert.AreEqual(b, Energy.Base.Url.Encode(a));

            a = "a + b";
            b = "a%20%2B%20b";
            Assert.AreEqual(b, Energy.Base.Url.Encode(a));
        }

        [TestMethod]
        public void UrlDecode()
        {
            string a, b;

            Assert.IsNull(Energy.Base.Url.Encode(null));
            Assert.AreEqual("", Energy.Base.Url.Decode(""));

            a = "%E2%9D%A4";
            b = "❤";
            Assert.AreEqual(b, Energy.Base.Url.Decode(a));

            a = "a%20%2B%20b";
            b = "a + b";
            Assert.AreEqual(b, Energy.Base.Url.Decode(a));

            a = "Águila";
            b = "Águila";
            Assert.AreEqual(b, Energy.Base.Url.Decode(a));

            a = "10 % 2";
            b = "10 % 2";
            Assert.AreEqual(b, Energy.Base.Url.Decode(a));
        }
    }
}
