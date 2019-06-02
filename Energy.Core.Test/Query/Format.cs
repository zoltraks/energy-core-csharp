using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Query
{
    [TestClass]
    public class Format
    {
        [TestMethod]
        public void QueryFormat()
        {
            string value;
            string result;
            string expect;

            Energy.Query.Format format = new Energy.Query.Format();

            value = "1,2";
            result = format.Number(value);
            expect = "1.2";
            Assert.AreEqual(expect, result);

            value = "1.2";
            result = format.Number(value);
            expect = "1.2";
            Assert.AreEqual(expect, result);

            value = "'";
            expect = "''''";
            result = format.Text(value);
            Assert.AreEqual(expect, result);

            value = "'Database open'";
            expect = "'''Database open'''";
            result = format.Text(value);
            Assert.AreEqual(expect, result);

            format.Bracket.Object = "[]";
            format.Bracket.Object.Include = "]]";
            value = "[]";
            expect = "[[]]]";
            result = format.Object(value);
            Assert.AreEqual(expect, result);

            string[] array = new string[] { null, "", "'", "''" };
            Assert.AreEqual(0, Energy.Base.Text.Compare(format.Text(array), new string[] { "NULL", "''", "''''", "''''''" }));
        }

        [TestMethod]
        public void Binary()
        {
            byte[] b;
            b = new byte[] { 1, 2, 3, 4 };
            Energy.Query.Format format = new Energy.Query.Format();
            string s;
            s = format.Binary(b);
            Assert.IsNotNull(s);
            Assert.AreEqual("0x01020304", s);
        }
    }
}
