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

            value = "nchar not    NULL  ";

            Energy.Query.Type.Definition def;
            def = Energy.Query.Type.ExtractTypeDefinition(value);

            result = Energy.Base.Text.Upper(def.Type);
            expect = "NCHAR";
            Assert.AreEqual(expect, result);

            result = Energy.Query.Type.ExtractTypeNull(value);
            expect = "NOT NULL";
            Assert.AreEqual(expect, result);

            result = Energy.Query.Type.ExtractTypeName(value);
            expect = "NCHAR";
            Assert.AreEqual(expect, result);

            result = def.Simple;
            expect = "CHAR";
            Assert.AreEqual(expect, result);

            value = "varchar(10)  not  null ";
            expect = "NOT NULL";
            result = Energy.Query.Type.ExtractTypeNull(value);
            Assert.AreEqual(expect, result);

            value = @"CHARACTER VARYING (2,3,4,5) default 'x' NOT NULL axx ""XXX"" default 'ab' abc utf-8";
            def = Energy.Query.Type.ExtractTypeDefinition(value);
            Assert.AreEqual("CHARACTER VARYING", def.Type);
            Assert.AreEqual("TEXT", def.Simple);
        }
    }
}
