using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Text
    {
        [TestMethod]
        public void FormatWithNull()
        {
            string value = string.Format("{0} {1}", null, null).Trim();
            Assert.AreEqual(0, value.Length);
        }

        [TestMethod]
        public void TextJoin()
        {
            string glue = "-";
            string format = "{0}{1}";
            string[] a1 = new string[] { "A" };
            string[] a2 = new string[] { "A", "A" };
            string t = Energy.Base.Text.Join(glue, format, a1, a2);
            Assert.AreEqual("AA-A", t);
        }

        [TestMethod]
        public void TextEncoding()
        {
            System.Text.Encoding encoding;
            foreach (string utf8 in new string[] { "utf8", "uTf-8", "UTF-8", "Utf8" })
            {
                encoding = Energy.Base.Text.Encoding(utf8);
                Assert.AreSame(System.Text.Encoding.UTF8, encoding);
            }
            foreach (string ucs2le in new string[] { "ucs2", "ucs-2", "UCS-2", "Ucs2", "UCS-2LE", "ucs-2 le" })
            {
                encoding = Energy.Base.Text.Encoding(ucs2le);
                Assert.AreSame(System.Text.Encoding.Unicode, encoding);
            }
            foreach (string ucs2be in new string[] { "ucs-2be", "ucs-2 be", "UCS-2BE", "ucs-2 Be" })
            {
                encoding = Energy.Base.Text.Encoding(ucs2be);
                Assert.AreSame(System.Text.Encoding.BigEndianUnicode, encoding, $"Test for {ucs2be}");
            }
        }

        [TestMethod]
        public void TextCheck()
        {
            string inputText = "555-12345678-123";
            string filterText1 = "555-";
            string filterText2 = "-123";
            string filterText3 = "666";
            bool ignoreCase = false;
            bool check;

            check = Energy.Base.Text.CheckAll(inputText, Energy.Enumeration.MatchMode.Same, ignoreCase, filterText1);
            Assert.AreEqual(false, check);

            check = Energy.Base.Text.CheckAny(inputText, Energy.Enumeration.MatchMode.Same, ignoreCase, filterText1);
            Assert.AreEqual(false, check);

            check = Energy.Base.Text.CheckOne(inputText, Energy.Enumeration.MatchMode.Same, ignoreCase, filterText1);
            Assert.AreEqual(false, check);

            check = Energy.Base.Text.CheckAll(inputText, Energy.Enumeration.MatchMode.Simple, ignoreCase, filterText1, filterText2);
            Assert.AreEqual(true, check);

            check = Energy.Base.Text.CheckAll(inputText, Energy.Enumeration.MatchMode.Simple, ignoreCase, filterText1, filterText2, filterText3);
            Assert.AreEqual(false, check);

            check = Energy.Base.Text.CheckAny(inputText, Energy.Enumeration.MatchMode.Simple, ignoreCase, filterText3, filterText2);
            Assert.AreEqual(true, check);

            check = Energy.Base.Text.CheckOne(inputText, Energy.Enumeration.MatchMode.Simple, ignoreCase, filterText1, filterText2, filterText3);
            Assert.AreEqual(false, check);

            string[] filterArray1 = new string[] { filterText1, filterText2, };
            check = Energy.Base.Text.CheckAll(inputText, Energy.Enumeration.MatchMode.Same, ignoreCase, filterArray1);
            Assert.AreEqual(false, check);

            check = Energy.Base.Text.CheckAny(inputText, Energy.Enumeration.MatchMode.Simple, ignoreCase, filterArray1);
            Assert.AreEqual(true, check);

            check = Energy.Base.Text.CheckOne(inputText, Energy.Enumeration.MatchMode.Simple, ignoreCase, filterArray1);
            Assert.AreEqual(false, check);

            string filterText4 = "[1-9]{9}";
            check = Energy.Base.Text.Check(inputText, Energy.Enumeration.MatchMode.Regex, ignoreCase, filterText4);
            Assert.AreEqual(false, check);

            string filterText5 = "[1-9]{8}";
            check = Energy.Base.Text.Check(inputText, Energy.Enumeration.MatchMode.Regex, ignoreCase, filterText5);
            Assert.AreEqual(true, check);
        }
    }
}
