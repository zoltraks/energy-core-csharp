using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Query
{
    [TestClass]
    public class Text
    {
        [TestMethod]
        public void TextQuote()
        {
            string expect;
            string value;
            string result;

            value = null;
            expect = "NULL";
            result = Energy.Query.Text.Quote(value);
            Assert.AreEqual(expect, result);
            result = Energy.Query.Text.Quote(value, '\'');
            Assert.AreEqual(expect, result);
            result = Energy.Query.Text.Quote(value, "'");
            Assert.AreEqual(expect, result);

            value = "'";
            expect = "''''";
            result = Energy.Query.Text.Quote(value);
            Assert.AreEqual(expect, result);
            result = Energy.Query.Text.Quote(value, '\'');
            Assert.AreEqual(expect, result);
            result = Energy.Query.Text.Quote(value, "'");
            Assert.AreEqual(expect, result);

            value = "[]";
            expect = "[[]]]";
            result = Energy.Query.Text.Quote(value, "[]");
            Assert.AreEqual(expect, result);
            value = "";
            expect = "[]";
            result = Energy.Query.Text.Quote(value, "[]");
            Assert.AreEqual(expect, result);
        }
    }
}
