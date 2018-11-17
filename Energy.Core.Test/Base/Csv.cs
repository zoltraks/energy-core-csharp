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

            line = "=1,='0'";
            result = Energy.Base.Csv.Explode(line, ",", "'");
            expect = new string[] { "=1", "='0'" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = "=1,='0'";
            result = Energy.Base.Csv.Explode(line, ",", "'", true);
            expect = new string[] { "=1", "0" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = null;
            result = Energy.Base.Csv.Explode(line, ",");
            Assert.IsNull(result);
            line = "";
            result = Energy.Base.Csv.Explode(line, ",");
            Assert.IsNotNull(result);

            line = "a,b,c";
            result = Energy.Base.Csv.Explode(line, ",");
            expect = new string[] { "a", "b", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = "a;,";
            result = Energy.Base.Csv.Explode(line, ";,");
            expect = new string[] { "a", "", "" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = "a;b;c";
            result = Energy.Base.Csv.Explode(line, ",;");
            expect = new string[] { "a", "b", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " a,\tb,c";
            result = Energy.Base.Csv.Explode(line, ",", null, false);
            expect = new string[] { " a", "\tb", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " a,' b'";
            result = Energy.Base.Csv.Explode(line, ",", "'", true, false);
            expect = new string[] { "a", " b" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " ' b'";
            result = Energy.Base.Csv.Explode(line, ",", "'", true, true);
            expect = new string[] { " b" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " a, ' b'";
            result = Energy.Base.Csv.Explode(line, ",", "'", false, false);
            expect = new string[] { "a", "' b'" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " a,' b'";
            result = Energy.Base.Csv.Explode(line, ",", "'", true, false);
            expect = new string[] { "a", " b" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " \" a\",\t\"\tb\",c";
            result = Energy.Base.Csv.Explode(line, ",", "\"");
            expect = new string[] { "\" a\"", "\"\tb\"", "c" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = "\"\",\"\",\"\"\"\"\"\",\",\",\"\",";
            result = Energy.Base.Csv.Explode(line, new char[] { ',' }, new char[] { '"' }, true);
            expect = new string[] { "", "", "\"\"", ",", "", "" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = ";\"\"\"\",'''''','\"\"';";
            result = Energy.Base.Csv.Explode(line, new char[] { ';', ',' }, new char[] { '"', '\'', }, true);
            expect = new string[] { "", "\"", "''", "\"\"", ""};
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
