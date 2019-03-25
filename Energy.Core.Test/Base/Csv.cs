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

            // This line is illegal...
            // =0123, ="0789",= 0123,= '0456', = '0789'
            // It will somehow work anyway...
            // Glue will not work...

            line = null;
            result = Energy.Base.Csv.Explode(line, ",");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);

            line = ",'";
            result = Energy.Base.Csv.Explode(line, ",", "'");
            expect = new string[] { "", "'" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = " ='";
            result = Energy.Base.Csv.Explode(line, ",", "'");
            expect = new string[] { " ='" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = ", ='0', ''";
            result = Energy.Base.Csv.Explode(line, ",", "'");
            expect = new string[] { "", "='0'", "''" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            line = "=1, ='0'";
            result = Energy.Base.Csv.Explode(line, ",", "'", true);
            expect = new string[] { "=1", "0" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

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

        [TestMethod]
        public void CsvSplit()
        {
            string content;
            string[] result;
            string[] expect;

            content = "\n" + "\r\n" + "\r";
            expect = new string[] { "", "", "\r" };
            result = Energy.Base.Csv.Split(content);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            content = "\r\n";
            expect = new string[] { "" };
            result = Energy.Base.Csv.Split(content);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            content = "A" + "\r" + "B" + "\r\n" + "C" + "\n" + "\r\n";
            expect = new string[] { "A\rB", "C", "" };
            result = Energy.Base.Csv.Split(content);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            content = ""
                + "" + "\r\n"
                + "0" + "\r"
                + "1" + "\n"
                + "2" + "\r\n"
                + @"""3" + "\r\n" + "4" + "\r" + "5" + "\n" + @"6""" + "\r\n"
                + "7" + "\r\n"
                + "'8" + "\r\n" + "9'"
                ;

            result = Energy.Base.Csv.Split(content, new char[] { '"', '\'' });
            expect = new string[]
            {
                "",
                "0\r1",
                "2",
                "\"3\r\n4\r5\n6\"",
                "7",
                "'8\r\n9'",
            };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            result = Energy.Base.Csv.Split(content, new char[] { '"' });
            expect = new string[]
            {
                "",
                "0\r1",
                "2",
                "\"3\r\n4\r5\n6\"",
                "7",
                "'8",
                "9'",
            };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            result = Energy.Base.Csv.Split(content, new char[] { '\'' });
            expect = new string[]
            {
                "",
                "0\r1",
                "2",
                "\"3",
                "4\r5",
                "6\"",
                "7",
                "'8\r\n9'",
            };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            result = Energy.Base.Csv.Split(content, new char[] { });
            expect = new string[]
            {
                "",
                "0\r1",
                "2",
                "\"3",
                "4\r5",
                "6\"",
                "7",
                "'8",
                "9'",
            };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            result = Energy.Base.Csv.Split(content, (char[])null);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));
        }
    }
}
