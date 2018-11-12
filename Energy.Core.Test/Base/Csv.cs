using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Csv
    {
        [TestMethod]
        public void CsvExplode()
        {
            string[] expect;
            string[] result;
            string line;

            line = "a,b,c";
            result = Energy.Base.Csv.Explode(line, ",");
            expect = new string[] { "a", "b", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " a,\tb,c";
            result = Energy.Base.Csv.Explode(line, ",", null, null);
            expect = new string[] { " a", "\tb", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " a,\tb,c";
            result = Energy.Base.Csv.Explode(line, ",", null, " \t");
            expect = new string[] { "a", "b", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " \" a\",\t\"\tb\",c";
            result = Energy.Base.Csv.Explode(line, ",", "\"", " \t");
            expect = new string[] { " a", "\tb", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = "\"\",\"\",\"\"\"\"\"\",\",\",\"\"";
            result = Energy.Base.Csv.Explode(line, new char[] { ',' }, new char[] { '"' }, null);
            expect = new string[] { null, "", "\"\"", ",", null };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));
        }

        [TestMethod]
        public void CsvImplode()
        {
            string[] array;
            string expect;
            string result;
            array = new string[] { null, "", "\"\"", ",", null };
            result = Energy.Base.Csv.Implode(array, ',', true);
            expect = "\"\",\"\",\"\"\"\"\"\",\",\",\"\"";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Csv.Implode(array, ',', false);
            expect = ",,\"\"\"\"\"\",\",\",";
            Assert.AreEqual(expect, result);
        }
    }
}
