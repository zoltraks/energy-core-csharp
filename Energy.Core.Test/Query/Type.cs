using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Query
{
    [TestClass]
    public class Type
    {
        [TestMethod]
        public void TypeExtract()
        {
            string expect;
            string value;
            string result;

            value = "abc nchar not    NULL  ";

            result = Energy.Query.Type.ExtractTypeNull(value);
            expect = "NOT NULL";
            Assert.AreEqual(expect, result);

            result = Energy.Query.Type.ExtractTypeName(value);
            expect = "NCHAR";
            Assert.AreEqual(expect, result);

            result = Energy.Query.Type.Simplify(value);
            expect = "TEXT";
            Assert.AreEqual(expect, result);
        }
    }
}
