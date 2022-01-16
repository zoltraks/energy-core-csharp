using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Text
    {
        [TestMethod]
        public void TextTrim()
        {
            Assert.IsNull(Energy.Base.Text.Trim((string)null));
            Assert.IsNotNull(Energy.Base.Text.Trim(""));
            Assert.AreEqual("", Energy.Base.Text.Trim(" "));
            Assert.AreEqual("", Energy.Base.Text.Trim("\t"));
            Assert.AreEqual("", Energy.Base.Text.Trim("\v"));
            Assert.AreEqual("", Energy.Base.Text.Trim("\r"));
            Assert.AreEqual("", Energy.Base.Text.Trim("\n"));
            Assert.AreEqual("", Energy.Base.Text.Trim("\f"));
            Assert.AreEqual("\0", Energy.Base.Text.Trim("\0"));

            string[] a1, a2, a3;
            a1 = new string[] {
                null,
                " \tA\t ", "\r\n" + "" + '\0' + 'X' + '\0' + "\r\n",
                " \t\r\n\v\f",
                "\v\f" + "" + (char)(2) + '-' + (char)(3) + "\v\f",
                null,
            };
            a2 = Energy.Base.Text.Trim(a1);
            a3 = new string[] {
                null,
                "A",
                "" + '\0' + 'X' + '\0', 
                "",
                "" + (char)(2) + '-' + (char)(3),
                null,
            };
            Assert.AreEqual(0, Energy.Base.Collection.Compare(a2, a3));
        }

        [TestMethod]
        public void TextFormatWithNull()
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
            Dictionary<string, string> d = new Dictionary<string, string>();
            d["a"] = "B";
            d["c"] = "D";
            t = Energy.Base.Text.Join(null, "{0}{1}", d);
            Assert.IsNull(t);
            t = Energy.Base.Text.Join(".", "{0}{1}", d);
            Assert.AreEqual("aB.cD", t);
            t = Energy.Base.Text.Join("", "{1}", d);
            Assert.AreEqual("BD", t);
            t = Energy.Base.Text.Join("", null, d);
            Assert.AreEqual("aBcD", t);
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

            string filterText6 = "5?5-*";
            Energy.Core.Bug.Write(filterText6);
            check = Energy.Base.Text.Check(inputText, Energy.Enumeration.MatchMode.Wild, ignoreCase, filterText6);
            Assert.AreEqual(true, check);

            string filterText7 = "555?-*";
            Energy.Core.Bug.Write(filterText7);
            check = Energy.Base.Text.Check(inputText, Energy.Enumeration.MatchMode.Wild, ignoreCase, filterText7);
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        public void TextUniquify()
        {
            string[] array;
            array = new string[] { "A", "a", "A" };
            string[] result;
            result = Energy.Base.Text.Uniquify(array, true, 1, " - ");
            int compare;
            compare = Energy.Base.Text.Compare(array, result, false);
            Assert.AreNotEqual(0, compare, "Uniquify error");
            array = new string[] { "A", "a - 1", "A - 2" };
            compare = Energy.Base.Text.Compare(array, result, false);
            Assert.AreEqual(0, compare, "Uniquify error");
            array = new string[] { "A", "a", "A", "a:1", "a", "a:2", "a" };
            result = Energy.Base.Text.Uniquify(array, false, 0, ":");
            compare = Energy.Base.Text.Compare(array, result, false);
            Assert.AreNotEqual(0, compare, "Uniquify error");
            array = new string[] { "A", "a", "A:0", "a:1", "a:0", "a:2", "a:3" };
            compare = Energy.Base.Text.Compare(array, result, false);
            Assert.AreEqual(0, compare, "Uniquify error");
        }

        [TestMethod]
        public void TextChop()
        {
            string[] array;
            array = new string[] { "A", "B", "C" };
            string[] result;
            result = new string[] { "B", "C" };
            string chop;
            chop = Energy.Base.Text.Chop(ref array);
            Assert.AreEqual("A", chop);
            Assert.AreEqual(0, Energy.Base.Text.Compare(array, result), chop);
        }

        [TestMethod]
        public void TextQuote()
        {
            string expect;
            string value;
            string result;
            value = "";
            result = Energy.Base.Text.Quote(value);
            expect = "\"\"";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Quote(value, "'");
            expect = "''";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Quote(value, "'", "'", true);
            expect = "";
            Assert.AreEqual(expect, result);
            value = "\"";
            result = Energy.Base.Text.Quote(value);
            expect = "\"\"\"\"";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Quote(value, "'");
            expect = "'\"'";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Quote(value, "'", "'", true);
            expect = "\"";
            Assert.AreEqual(expect, result);
            string o;
            o = "12";
            result = Energy.Base.Text.Quote(o);
            expect = "\"12\"";
            Assert.AreEqual(expect, result);
            o = "12.34";
            result = Energy.Base.Text.Quote(o);
            expect = "\"12.34\"";
            Assert.AreEqual(expect, result);
            o = " 12.34 ";
            result = Energy.Base.Text.Quote(o);
            expect = "\" 12.34 \"";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Quote(o, true);
            expect = " 12.34 ";
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void TextStrip()
        {
            string expect;
            string value;
            string result;
            bool change;
            value = "";
            result = Energy.Base.Text.Strip(value);
            expect = "";
            Assert.AreEqual(expect, result);
            value = "'X\\'Y'";
            result = Energy.Base.Text.Strip(value, "'", "\\");
            expect = "X'Y";
            Assert.AreEqual(expect, result);
            value = "X\\'Y";
            result = Energy.Base.Text.Strip(value, "'", "\\");
            expect = "X'Y";
            Assert.AreEqual(expect, result);
            value = "]]]]";
            expect = "]";
            result = Energy.Base.Text.Strip(value, ']');
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Strip(value, ']', ']');
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Strip(value, "]");
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Strip(value, "]", "]");
            Assert.AreEqual(expect, result);
            value = "[]]]";
            expect = "]";
            result = Energy.Base.Text.Strip(value, "[", "]", "]");
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Strip(value, "[", "]", "]", out change);
            Assert.IsTrue(change);
            Assert.AreEqual(expect, result);

            var qa = new string[] { "@\"", "\"" };
            var qb = new string[] { "\"", "\"" };
            var qe = new string[] { "\"", "\\" };
            var s1 = Energy.Base.Text.Strip("@\"Verbatim \"\"style\"\" example", qa, qb, qe);
            var s2 = Energy.Base.Text.Strip("\"Normal \\\"style\\\" example", qa, qb, qe);
            Assert.AreEqual(@"Verbatim ""style"" example", s1);
            Assert.AreEqual(@"Normal ""style"" example", s2);
        }

        [TestMethod]
        public void TextSurround()
        {
            string text1, text2, text3;
            string q;
            q = '"'.ToString();
            text1 = "Just..." + q;
            text2 = Energy.Base.Text.Surround(text1, q, q, false);
            text3 = q + text1 + q;
            Assert.AreEqual(text3, text2);
            text2 = Energy.Base.Text.Surround(text1, q, q, true);
            text3 = q + text1;
            Assert.AreEqual(text3, text2);
        }

        [TestMethod]
        public void TextMiddleString()
        {
            string p0 = "";
            string p1 = "#";
            string p2 = "{}";
            string p3 = "{#}";
            string p4 = "<<>>";
            string p5 = "xxyzz";

            Assert.AreEqual('\0', Energy.Base.Text.GetMiddleStringPatternChar(p0));
            Assert.AreEqual('#', Energy.Base.Text.GetMiddleStringPatternChar(p1));
            Assert.AreEqual('\0', Energy.Base.Text.GetMiddleStringPatternChar(p2));
            Assert.AreEqual('#', Energy.Base.Text.GetMiddleStringPatternChar(p3));
            Assert.AreEqual('\0', Energy.Base.Text.GetMiddleStringPatternChar(p4));
            Assert.AreEqual('y', Energy.Base.Text.GetMiddleStringPatternChar(p5));

            Assert.AreEqual("", Energy.Base.Text.GetMiddleStringPrefix(p0));
            Assert.AreEqual("#", Energy.Base.Text.GetMiddleStringPrefix(p1));
            Assert.AreEqual("{", Energy.Base.Text.GetMiddleStringPrefix(p2));
            Assert.AreEqual("{", Energy.Base.Text.GetMiddleStringPrefix(p3));
            Assert.AreEqual("<<", Energy.Base.Text.GetMiddleStringPrefix(p4));
            Assert.AreEqual("xx", Energy.Base.Text.GetMiddleStringPrefix(p5));

            Assert.AreEqual("", Energy.Base.Text.GetMiddleStringSuffix(p0));
            Assert.AreEqual("#", Energy.Base.Text.GetMiddleStringSuffix(p1));
            Assert.AreEqual("}", Energy.Base.Text.GetMiddleStringSuffix(p2));
            Assert.AreEqual("}", Energy.Base.Text.GetMiddleStringSuffix(p3));
            Assert.AreEqual(">>", Energy.Base.Text.GetMiddleStringSuffix(p4));
            Assert.AreEqual("zz", Energy.Base.Text.GetMiddleStringSuffix(p5));
        }

        [TestMethod]
        public void TextInArray()
        {
            string[] a = new string[] { "a", "B", null };
            Assert.AreEqual(true, Energy.Base.Text.InArray(a, "a"));
            Assert.AreEqual(false, Energy.Base.Text.InArray(a, "A"));
            // illegal
            //Assert.AreEqual(true, Energy.Base.Text.InArray(a, null));
            Assert.AreEqual(false, Energy.Base.Text.InArray(null, "A"));
            Assert.AreEqual(true, Energy.Base.Text.InArray(a, "b", true));
        }

        [TestMethod]
        public void TextPad()
        {
            string text;
            string expect;
            string result;

            text = "X";
            expect = "X";
            result = Energy.Base.Text.Pad(text, 1, '0', Energy.Enumeration.TextPad.Left);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 0, '0', Energy.Enumeration.TextPad.Left);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, -1, '0', Energy.Enumeration.TextPad.Left);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 1, '0', Energy.Enumeration.TextPad.Center);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 0, '0', Energy.Enumeration.TextPad.Center);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, -1, '0', Energy.Enumeration.TextPad.Center);
            Assert.AreEqual(expect, result);

            result = Energy.Base.Text.Pad(text, 2, '0', Energy.Enumeration.TextPad.Left);
            expect = "0X";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 2, '0', Energy.Enumeration.TextPad.Right);
            expect = "X0";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 2, '0', Energy.Enumeration.TextPad.Center);
            expect = "0X";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 4, '0', Energy.Enumeration.TextPad.Middle);
            expect = "00X0";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Pad(text, 5, '0', Energy.Enumeration.TextPad.Center);
            expect = "00X00";
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void TextCell()
        {
            string text;
            string expect;
            string result;
            string remains;

            text = null;
            expect = "";
            result = Energy.Base.Text.Cell(text, 0, 0, Energy.Enumeration.TextPad.Left, '.', "", "", out remains);
            Assert.AreEqual(expect, result);
            expect = ".";
            result = Energy.Base.Text.Cell(text, 0, 1, Energy.Enumeration.TextPad.Left, '.', "", "", out remains);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 2, Energy.Enumeration.TextPad.Left, '.', "", "", out remains);
            expect = "..";
            Assert.AreEqual(expect, result);

            text = "X";
            expect = "X";
            result = Energy.Base.Text.Cell(text, 0, 1, Energy.Enumeration.TextPad.Left, '.', "", "", out remains);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 0, Energy.Enumeration.TextPad.Left, '.', "", "", out remains);
            expect = "";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 1, 1, Energy.Enumeration.TextPad.Left, '.', "", "", out remains);
            expect = ".";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 0, Energy.Enumeration.TextPad.Left, '.', "[", "]", out remains);
            expect = "";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 1, Energy.Enumeration.TextPad.Left, '.', "[", "]", out remains);
            expect = "X";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 1, 1, Energy.Enumeration.TextPad.Left, '.', "[", "]", out remains);
            expect = "[";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 2, Energy.Enumeration.TextPad.Left, '.', "[", "]", out remains);
            expect = "[X";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 2, Energy.Enumeration.TextPad.Right, '.', "[", "]", out remains);
            expect = "X]";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 2, Energy.Enumeration.TextPad.Left, '.', "[", "]", out remains);
            expect = "[X";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 2, Energy.Enumeration.TextPad.Right, '.', "[", "]", out remains);
            expect = "X]";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 1, 2, Energy.Enumeration.TextPad.Left, '.', "[", "]", out remains);
            expect = "[]";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 1, 2, Energy.Enumeration.TextPad.Right, '.', "[", "]", out remains);
            expect = "[]";
            Assert.AreEqual(expect, result);

            text = "Ana's Song";
            result = Energy.Base.Text.Cell(text, 2, 13 + 5 + 6, Energy.Enumeration.TextPad.Center, '-', "<div>", "</div>", out remains);
            expect = "<div>---a's Song--</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 0 + 5 + 6, Energy.Enumeration.TextPad.Center, '-', "<div>", "</div>", out remains);
            expect = "-Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 0 + 5 + 6, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song-";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 3, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song---";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 4, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song----";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 5, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "<div>Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 6, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 7, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song-</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 9, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song---</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 10, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song----</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 11, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "<div>Ana's Song</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 12, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "<div>Ana's Song-</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 13, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "<div>Ana's Song--</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 4, Energy.Enumeration.TextPad.Right, '-', "<div>", "</div>", out remains);
            expect = "Ana's Song----";
            Assert.AreEqual(expect, result);

            result = Energy.Base.Text.Cell(text, 0, 10 + 5, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 6, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>-Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 7, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>--Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 9, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>----Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 10, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>-----Ana's Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 11, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>Ana's Song</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 12, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>-Ana's Song</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 13, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>--Ana's Song</div>";
            Assert.AreEqual(expect, result);

            result = Energy.Base.Text.Cell(text, 0, 10 + 14, Energy.Enumeration.TextPad.Center, '-', "<div>", "</div>", out remains);
            expect = "<div>--Ana's Song-</div>";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 0, 10 + 14, Energy.Enumeration.TextPad.Left, '-', "<div>", "</div>", out remains);
            expect = "<div>---Ana's Song</div>";
            Assert.AreEqual(expect, result);

            expect = "Ana";
            result = Energy.Base.Text.Cell(text, 0, 3, Energy.Enumeration.TextPad.Left, '\0', null, null, out remains);
            expect = "Ana";
            Assert.AreEqual(expect, result);
            expect = "'s Song";
            Assert.AreEqual(expect, remains);
            result = Energy.Base.Text.Cell(text, -4, 4, Energy.Enumeration.TextPad.Left, '\0', null, null, out remains);
            expect = "Song";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, -4, 4, Energy.Enumeration.TextPad.Left);
            Assert.AreEqual(expect, result);
            expect = "";
            Assert.AreEqual(expect, remains);
            result = Energy.Base.Text.Cell(text, -4, 3, Energy.Enumeration.TextPad.Left, '\0', null, null, out remains);
            expect = "Son";
            Assert.AreEqual(expect, result);
            expect = "g";
            Assert.AreEqual(expect, remains);

            text = "Another";
            result = Energy.Base.Text.Cell(text, 9, '<', ' ');
            expect = "Another  ";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 9, '>', ' ');
            expect = "  Another";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Cell(text, 9, '-', ' ');
            expect = " Another ";
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void TextDecodeControlString()
        {
            string text;
            string result;
            string expect;
            text = "'Adam''s line' #13 #$0a 'Betty''s line'";
            expect = "Adam's line" + "\r\n" + "Betty's line";
            result = Energy.Base.Text.DecodeControlString(text, '\'', '\'', "#", "#$", "#0", "#%"
                , System.Text.Encoding.ASCII, true, true, true);
            Assert.AreEqual(expect, result);

            text = " \\0101 ";
            expect = "A";
            result = Energy.Base.Text.DecodeControlString(text, new Energy.Base.Text.Class.ControlStringOptions()
            {
                OctalPrefix = new string[] { "\\0", },
            });
            Assert.AreEqual(expect, result);

            text = " \\003 ą \\002 ";
            expect = "\u0003\u0002";
            result = Energy.Base.Text.DecodeControlString(text, new Energy.Base.Text.Class.ControlStringOptions()
            {
                OctalPrefix = new string[] { "\\0", },
            });
            Assert.AreEqual(expect, result);

            text = " \\003 ą \\002 ";
            expect = "\u0003ą\u0002";
            result = Energy.Base.Text.DecodeControlString(text, new Energy.Base.Text.Class.ControlStringOptions()
            {
                OctalPrefix = new string[] { "\\0", },
                IncludeUnknown = true,
            });
            Assert.AreEqual(expect, result);

            text = " \\003 ą \\002 ";
            expect = " \u0003 ą \u0002 ";
            result = Energy.Base.Text.DecodeControlString(text, new Energy.Base.Text.Class.ControlStringOptions()
            {
                OctalPrefix = new string[] { "\\0", },
                IncludeUnknown = true,
                IncludeWhite = true,
            });
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void TextFirst()
        {
            string text;
            text = null;
            Assert.AreEqual("", Energy.Base.Text.First(text));
            Assert.AreEqual("", Energy.Base.Text.First(text, 2));
            Assert.AreEqual(null, Energy.Base.Text.FirstOrNull(text));
            text = "";
            Assert.AreEqual("", Energy.Base.Text.First(text, 2));
            Assert.AreEqual(null, Energy.Base.Text.FirstOrNull(text));
            Assert.AreEqual(null, Energy.Base.Text.FirstOrNull(text, 2));
            text = "a";
            Assert.AreEqual("a", Energy.Base.Text.First(text, 2));
            Assert.AreEqual("a", Energy.Base.Text.FirstOrNull(text, 2));
            text = "ab";
            Assert.AreEqual("ab", Energy.Base.Text.First(text, 2));
            text = "abc";
            Assert.AreEqual("ab", Energy.Base.Text.First(text, 2));
        }

        [TestMethod]
        public void TextImplode()
        {
            string[] expect;
            string[] result;
            string[] array;
            string glue;
            glue = "-";
            array = null;
            expect = null;
            result = Energy.Base.Text.Implode(glue, -1, array);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Implode(glue, 0, array);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Text.Implode(glue, 1, array);
            Assert.AreEqual(expect, result);
            array = new string[] { };
            result = Energy.Base.Text.Implode(glue, -1, array);
            Assert.IsNull(result);
            result = Energy.Base.Text.Implode(glue, 0, array);
            Assert.IsNull(result);
            result = Energy.Base.Text.Implode(glue, 1, array);
            expect = new string[] { };
            Assert.AreEqual(0, Energy.Base.Collection.StringArray.Compare(expect, result));
            array = new string[] { "a", "b", "c" };
            result = Energy.Base.Text.Implode(glue, 1, array);
            expect = new string[] { "a", "b", "c" };
            Assert.AreEqual(0, Energy.Base.Collection.StringArray.Compare(expect, result));
            result = Energy.Base.Text.Implode(glue, 2, array);
            expect = new string[] { "a-b", "c" };
            Assert.AreEqual(0, Energy.Base.Collection.StringArray.Compare(expect, result));
            array = new string[] { "a", "b", "c" };
            result = Energy.Base.Text.Implode(glue, 4, array);
            expect = new string[] { "a-b-c" };
            Assert.AreEqual(0, Energy.Base.Collection.StringArray.Compare(expect, result));
        }

        [TestMethod]
        public void TextSplitArray()
        {
            string[] expect = null;
            string[] result = null;
            result = Energy.Base.Text.SplitArray(null, null, null);
            Assert.IsNull(result);
            result = Energy.Base.Text.SplitArray("1  2,3", null, null);
            expect = new string[] { "1", "2,3" };
            Assert.IsTrue(Energy.Base.Collection.Same(expect, result));
            result = Energy.Base.Text.SplitArray("1 2,3", " ", null);
            expect = new string[] { "1", "2,3" };
            Assert.IsTrue(Energy.Base.Collection.Same(expect, result));
            result = Energy.Base.Text.SplitArray("1  2,3", " ", null);
            expect = new string[] { "1", "2,3" };
            Assert.IsTrue(Energy.Base.Collection.Same(expect, result));
            result = Energy.Base.Text.SplitArray("1 2,3", ", ", null);
            expect = new string[] { "1", "2", "3" };
            Assert.IsTrue(Energy.Base.Collection.Same(expect, result));
            result = Energy.Base.Text.SplitArray("1, , \t , 2,3", ", ", null);
            expect = new string[] { "1", "2", "3" };
            Assert.IsTrue(Energy.Base.Collection.Same(expect, result));
            result = Energy.Base.Text.SplitArray("1, ' ' , '\t' , 2,3", ", ", "'");
            expect = new string[] { "1", " ", "\t", "2", "3" };
            Assert.IsTrue(Energy.Base.Collection.Same(expect, result));
        }

        [TestMethod]
        public void TextCut()
        {
            string text;
            string line;
            text = Energy.Base.Text.Cut("|", new string[] { "|" }, null);
            Assert.AreEqual("|", text);
            text = Energy.Base.Text.Cut("||", new string[] { "||" }, null);
            Assert.AreEqual("||", text);
            text = Energy.Base.Text.Cut("'|'|x", new string[] { "|" }, new string[] { "'" });
            Assert.AreEqual("'|'|", text);
            text = Energy.Base.Text.Cut("'''!'''", null, new string[] { "'" });
            Assert.AreEqual("'''!'''", text);
            text = Energy.Base.Text.Cut("a|b", new string[] { "|" }, null);
            Assert.AreEqual("a|", text);
            text = @"
A + B
'
This will be multilne,
with \'Hello world\'
'
""
Another
"" ""x""""y""
";
            line = Energy.Base.Text.Cut(text, new string[] { "\r\n", "$" }, new string[] { @"""", @"'\'" });
            Assert.AreEqual("\r\n", line);
            line = Energy.Base.Text.Cut(ref text, new string[] { "\r\n", "$" }, new string[] { @"""", @"'\'" });
            Assert.AreEqual("\r\n", line);
            line = Energy.Base.Text.Cut(ref text, new string[] { "\r\n", "$" }, new string[] { @"""", @"'\'" });
            Assert.AreEqual("A + B\r\n", line);
            line = Energy.Base.Text.Cut(ref text, new string[] { "\r\n", "$" }, new string[] { @"""", @"'\'" });
            Assert.AreEqual(@"'
This will be multilne,
with \'Hello world\'
'
", line);
            line = Energy.Base.Text.Cut(ref text, new string[] { "\r\n", "$" }, new string[] { @"""", @"'\'" });
            Assert.AreEqual(@"""
Another
"" ""x""""y""
", line);
        }

        [TestMethod]
        public void TextEmptyIfNull()
        {
            Assert.AreEqual("", Energy.Base.Text.EmptyIfNull(null));
            Assert.AreEqual("", Energy.Base.Text.EmptyIfNull(""));
            Assert.AreEqual("A", Energy.Base.Text.EmptyIfNull("A"));
        }

        [TestMethod]
        public void TextQuotation()
        {
            Energy.Base.Text.Quotation q;
            q = Energy.Base.Text.Quotation.From("");
            Assert.IsNull(q);
            q = Energy.Base.Text.Quotation.From(null);
            Assert.IsNull(q);
            q = Energy.Base.Text.Quotation.From("%");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("%%", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("%%");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("%%", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("%%%");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("%%", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("%+%");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("+%", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("<<>>");
            Assert.IsNotNull(q);
            Assert.AreEqual("<<", q.Prefix);
            Assert.AreEqual(">>", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual(">>>>", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("<<->>");
            Assert.IsNotNull(q);
            Assert.AreEqual("<<", q.Prefix);
            Assert.AreEqual(">>", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("->>", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("% %% %");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("%%", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From(@"% %% \% %");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(2, q.Escape.Length);
            Assert.AreEqual("%%", q.Escape[0]);
            Assert.AreEqual(@"\%", q.Escape[1]);
            q = Energy.Base.Text.Quotation.From("% %");
            Assert.IsNotNull(q);
            Assert.AreEqual("%", q.Prefix);
            Assert.AreEqual("%", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("%%", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From(" ");
            Assert.IsNotNull(q);
            Assert.AreEqual(" ", q.Prefix);
            Assert.AreEqual(" ", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("  ", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From("  ");
            Assert.IsNotNull(q);
            Assert.AreEqual(" ", q.Prefix);
            Assert.AreEqual(" ", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual("  ", q.Escape[0]);
            q = Energy.Base.Text.Quotation.From(@" \ ");
            Assert.IsNotNull(q);
            Assert.AreEqual(" ", q.Prefix);
            Assert.AreEqual(" ", q.Suffix);
            Assert.IsNotNull(q.Escape);
            Assert.AreEqual(1, q.Escape.Length);
            Assert.AreEqual(@"\ ", q.Escape[0]);
        }

        [TestMethod]
        public void TextIfEmpty()
        {
            string s;
            s = Energy.Base.Text.IfEmpty(null, "", " ", "X");
            Assert.AreEqual(" ", s);
            s = Energy.Base.Text.IfEmpty(null);
            Assert.AreEqual("", s);
            s = Energy.Base.Text.IfEmpty();
            Assert.AreEqual("", s);
        }

        [TestMethod]
        public void TextIfWhite()
        {
            string s;
            s = Energy.Base.Text.IfWhite(null, "", " ", "\t", "\t\r\n\v", ".", "!");
            Assert.AreEqual(".", s);
            s = Energy.Base.Text.IfWhite(null);
            Assert.AreEqual("", s);
            s = Energy.Base.Text.IfWhite();
            Assert.AreEqual("", s);
        }

        [TestMethod]
        public void TextFindAnyWord()
        {
            string[] a;
            a = new string[] { };
            string x;
            x = Energy.Base.Text.FindAnyWord(null, a);
            Assert.IsNull(x);
            x = Energy.Base.Text.FindAnyWord("", a);
            Assert.IsNull(x);
            a = null;
            x = Energy.Base.Text.FindAnyWord("", a);
            Assert.IsNull(x);
            a = new string[] { "a|b", "[]" };
            x = Energy.Base.Text.FindAnyWord(null, a);
            Assert.IsNull(x);
            x = Energy.Base.Text.FindAnyWord("", a);
            Assert.IsNull(x);
            x = Energy.Base.Text.FindAnyWord("a|b", a);
            Assert.AreEqual("a|b", x);
            x = Energy.Base.Text.FindAnyWord("[]", a);
            Assert.AreEqual("[]", x);
            a = new string[] { "." };
            x = Energy.Base.Text.FindAnyWord("", a);
            Assert.IsNull(x);
            x = Energy.Base.Text.FindAnyWord("a", a);
            Assert.IsNull(x);
            x = Energy.Base.Text.FindAnyWord(".", a);
            Assert.AreEqual(".", x);
        }

        [TestMethod]
        public void TextIsInteger()
        {
            Assert.IsFalse(Energy.Base.Text.IsInteger(null));
            Assert.IsFalse(Energy.Base.Text.IsInteger(""));
            Assert.IsFalse(Energy.Base.Text.IsInteger(" "));
            Assert.IsFalse(Energy.Base.Text.IsInteger("-"));
            Assert.IsFalse(Energy.Base.Text.IsInteger("+"));
            Assert.IsTrue(Energy.Base.Text.IsInteger(" +1"));
            Assert.IsTrue(Energy.Base.Text.IsInteger(" -1"));
            Assert.IsFalse(Energy.Base.Text.IsInteger("+ 1"));
            Assert.IsFalse(Energy.Base.Text.IsInteger("- 1"));

            Assert.IsTrue(Energy.Base.Text.IsInteger("+0"));
            Assert.IsTrue(Energy.Base.Text.IsInteger("-0"));
            Assert.IsTrue(Energy.Base.Text.IsInteger("1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsInteger("-1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsInteger("+1234567890123456789012345678901234567890"));
        }

        [TestMethod]
        public void TextIsNumber()
        {
            Assert.IsFalse(Energy.Base.Text.IsNumber(null));
            Assert.IsFalse(Energy.Base.Text.IsNumber(""));
            Assert.IsFalse(Energy.Base.Text.IsNumber(" "));
            Assert.IsFalse(Energy.Base.Text.IsNumber("-"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("+"));
            Assert.IsFalse(Energy.Base.Text.IsNumber(" +1"));
            Assert.IsFalse(Energy.Base.Text.IsNumber(" -1"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("+ 1"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("- 1"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("12e"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("12. e"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("e"));
            Assert.IsFalse(Energy.Base.Text.IsNumber("e0"));
            Assert.IsFalse(Energy.Base.Text.IsNumber(".e0"));

            Assert.IsTrue(Energy.Base.Text.IsNumber("+0"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("-0"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("-1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("+1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("1234567890123456789012345678901234567890.1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("-1234567890123456789012345678901234567890.1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("+1234567890123456789012345678901234567890.1234567890123456789012345678901234567890"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("0e0"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("0.e0"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("+12.e0"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("+12.e3"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("+12.e+3"));
            Assert.IsTrue(Energy.Base.Text.IsNumber("-12.e-3"));
        }

        [TestMethod]
        public void TextUpper()
        {
            Assert.AreEqual("AAÀÀŹŹŢŢǬǬΔΔДД", Energy.Base.Text.Upper("AaÀàŹźŢţǬǭΔδДд"));
        }

        [TestMethod]
        public void TextLower()
        {
            Assert.AreEqual("aaààźźţţǭǭδδдд", Energy.Base.Text.Lower("AaÀàŹźŢţǬǭΔδДд"));
        }

        [TestMethod]
        public void TextSplitLine()
        {
            Assert.IsNull(Energy.Base.Text.SplitLine(null));
            Assert.AreEqual(0, Energy.Base.Text.SplitLine("").Length);
            string content;
            string[] array;
            content = "a";
            array = Energy.Base.Text.SplitLine(content);
            Assert.AreEqual(0, Energy.Base.Collection.Compare(new string[] { "a" }, array));
            content = "a\r\n\n\rb";
            array = Energy.Base.Text.SplitLine(content);
            Assert.AreEqual(0, Energy.Base.Collection.Compare(new string[] { "a", "", "", "b" }, array));
            array = Energy.Base.Text.SplitLine(content, false);
            Assert.AreEqual(0, Energy.Base.Collection.Compare(new string[] { "a", "", "", "b" }, array));
            array = Energy.Base.Text.SplitLine(content, true);
            Assert.AreEqual(0, Energy.Base.Collection.Compare(new string[] { "a", "b" }, array));
        }

        [TestMethod]
        public void TryParse()
        {
            Assert.IsNull(Energy.Base.Text.TryParse<object>(null));
            Assert.AreEqual("", Energy.Base.Text.TryParse<string>(""));
            Assert.AreEqual("", Energy.Base.Text.TryParse<object>(""));

            bool b;
            string s;

            #region byte

            byte o_byte;

            Assert.IsFalse(Energy.Base.Text.TryParse<byte>("-1", out o_byte));
            Assert.AreEqual(0, Energy.Base.Text.TryParse<byte>("-1"));

            b = byte.TryParse("1", out o_byte);
            Assert.IsTrue(b);
            Assert.AreEqual(1, o_byte);
            Assert.IsTrue(Energy.Base.Text.TryParse<byte>("1", out o_byte));
            Assert.AreEqual(1, o_byte);
            Assert.AreEqual(1, Energy.Base.Text.TryParse<byte>("1"));

            b = byte.TryParse(" 1 ", out o_byte);
            Assert.IsTrue(b);
            Assert.AreEqual(1, o_byte);
            Assert.IsTrue(Energy.Base.Text.TryParse<byte>(" 1 ", out o_byte));
            Assert.AreEqual(1, o_byte);
            Assert.AreEqual(1, Energy.Base.Text.TryParse<byte>(" 1 "));

            b = byte.TryParse(" \t\r\n 1 \t\r\n ", out o_byte);
            Assert.IsTrue(b);
            Assert.AreEqual(1, o_byte);
            Assert.IsTrue(Energy.Base.Text.TryParse<byte>(" \t\r\n 1 \t\r\n ", out o_byte));
            Assert.AreEqual(1, o_byte);
            Assert.AreEqual(1, Energy.Base.Text.TryParse<byte>(" \t\r\n 1 \t\r\n "));

            b = byte.TryParse("256", out o_byte);
            Assert.IsFalse(b);
            Assert.IsFalse(Energy.Base.Text.TryParse<byte>("256", out o_byte));
            Assert.AreEqual(0, Energy.Base.Text.TryParse<byte>("256"));

            b = byte.TryParse("1.0", out o_byte);
            Assert.IsFalse(b);
            Assert.IsFalse(Energy.Base.Text.TryParse<byte>("1.0", out o_byte));
            Assert.AreEqual(0, Energy.Base.Text.TryParse<byte>("1.0"));

            b = byte.TryParse("255", out o_byte);
            Assert.IsTrue(b);
            Assert.AreEqual(255, o_byte);
            Assert.IsTrue(Energy.Base.Text.TryParse<byte>("255", out o_byte));
            Assert.AreEqual(255, o_byte);
            Assert.AreEqual(255, Energy.Base.Text.TryParse<byte>("255"));

            b = byte.TryParse(" +255 ", out o_byte);
            Assert.IsTrue(b);
            Assert.AreEqual(255, o_byte);
            Assert.IsTrue(Energy.Base.Text.TryParse<byte>(" +255 ", out o_byte));
            Assert.AreEqual(255, o_byte);
            Assert.AreEqual(255, Energy.Base.Text.TryParse<byte>(" +255 "));

            b = byte.TryParse(" + 2 ", out o_byte);
            Assert.IsFalse(b);
            Assert.IsFalse(Energy.Base.Text.TryParse<byte>(" + 2", out o_byte));
            Assert.AreEqual(0, Energy.Base.Text.TryParse<byte>(" + 2"));

            #endregion

            #region sbyte

            sbyte o_sbyte;

            foreach (var v_sbyte in new sbyte[] { 0, 1, -1, sbyte.MinValue, sbyte.MaxValue, 2, -2 })
            {
                s = v_sbyte.ToString();

                b = sbyte.TryParse(s, out o_sbyte);
                Assert.IsTrue(b);
                Assert.AreEqual(v_sbyte, o_sbyte);
                Assert.IsTrue(Energy.Base.Text.TryParse<sbyte>(s, out o_sbyte));
                Assert.AreEqual(v_sbyte, o_sbyte);
                Assert.AreEqual(v_sbyte, Energy.Base.Text.TryParse<sbyte>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = sbyte.TryParse(s, out o_sbyte);
                Assert.IsTrue(b);
                Assert.AreEqual(v_sbyte, o_sbyte);
                Assert.IsTrue(Energy.Base.Text.TryParse<sbyte>(s, out o_sbyte));
                Assert.AreEqual(v_sbyte, o_sbyte);
                Assert.AreEqual(v_sbyte, Energy.Base.Text.TryParse<sbyte>(s));
            }

            foreach (var s_sbyte in new string[] { "128", "-129", "+ 127", "- 128", "127.0" })
            {
                s = s_sbyte;

                b = sbyte.TryParse(s, out o_sbyte);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<sbyte>(s, out o_sbyte));
                Assert.AreEqual(0, Energy.Base.Text.TryParse<sbyte>(s));
            }

            #endregion

            #region ushort

            ushort o_ushort;

            foreach (var v_ushort in new ushort[] { 0, 1, ushort.MinValue, ushort.MaxValue, 2, })
            {
                s = v_ushort.ToString();

                b = ushort.TryParse(s, out o_ushort);
                Assert.IsTrue(b);
                Assert.AreEqual(v_ushort, o_ushort);
                Assert.IsTrue(Energy.Base.Text.TryParse<ushort>(s, out o_ushort));
                Assert.AreEqual(v_ushort, o_ushort);
                Assert.AreEqual(v_ushort, Energy.Base.Text.TryParse<ushort>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = ushort.TryParse(s, out o_ushort);
                Assert.IsTrue(b);
                Assert.AreEqual(v_ushort, o_ushort);
                Assert.IsTrue(Energy.Base.Text.TryParse<ushort>(s, out o_ushort));
                Assert.AreEqual(v_ushort, o_ushort);
                Assert.AreEqual(v_ushort, Energy.Base.Text.TryParse<ushort>(s));
            }

            foreach (var s_ushort in new string[] { "65536", "-32768", "65535.0" })
            {
                s = s_ushort;

                b = ushort.TryParse(s, out o_ushort);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<ushort>(s, out o_ushort));
                Assert.AreEqual(0, Energy.Base.Text.TryParse<ushort>(s));
            }

            #endregion

            #region short

            short o_short;

            foreach (var v_short in new short[] { 0, 1, -1, short.MinValue, short.MaxValue, 2, -2 })
            {
                s = v_short.ToString();

                b = short.TryParse(s, out o_short);
                Assert.IsTrue(b);
                Assert.AreEqual(v_short, o_short);
                Assert.IsTrue(Energy.Base.Text.TryParse<short>(s, out o_short));
                Assert.AreEqual(v_short, o_short);
                Assert.AreEqual(v_short, Energy.Base.Text.TryParse<short>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = short.TryParse(s, out o_short);
                Assert.IsTrue(b);
                Assert.AreEqual(v_short, o_short);
                Assert.IsTrue(Energy.Base.Text.TryParse<short>(s, out o_short));
                Assert.AreEqual(v_short, o_short);
                Assert.AreEqual(v_short, Energy.Base.Text.TryParse<short>(s));
            }

            foreach (var s_short in new string[] { "32768", "-32769", "+ 32767", "- 32768", "32767.0" })
            {
                s = s_short;

                b = short.TryParse(s, out o_short);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<short>(s, out o_short));
                Assert.AreEqual(0, Energy.Base.Text.TryParse<short>(s));
            }

            #endregion

            #region uint

            uint o_uint;

            foreach (var v_uint in new uint[] { 0, 1, uint.MinValue, uint.MaxValue, 2, })
            {
                s = v_uint.ToString();

                b = uint.TryParse(s, out o_uint);
                Assert.IsTrue(b);
                Assert.AreEqual(v_uint, o_uint);
                Assert.IsTrue(Energy.Base.Text.TryParse<uint>(s, out o_uint));
                Assert.AreEqual(v_uint, o_uint);
                Assert.AreEqual(v_uint, Energy.Base.Text.TryParse<uint>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = uint.TryParse(s, out o_uint);
                Assert.IsTrue(b);
                Assert.AreEqual(v_uint, o_uint);
                Assert.IsTrue(Energy.Base.Text.TryParse<uint>(s, out o_uint));
                Assert.AreEqual(v_uint, o_uint);
                Assert.AreEqual(v_uint, Energy.Base.Text.TryParse<uint>(s));
            }

            foreach (var s_uint in new string[] { "4294967296", "-1", "4294967295.0" })
            {
                s = s_uint;

                b = uint.TryParse(s, out o_uint);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<uint>(s, out o_uint));
                Assert.AreEqual((uint)0, Energy.Base.Text.TryParse<uint>(s));
            }

            #endregion

            #region int

            int o_int;

            foreach (var v_int in new int[] { 0, 1, -1, int.MinValue, int.MaxValue, 2, -2 })
            {
                s = v_int.ToString();

                b = int.TryParse(s, out o_int);
                Assert.IsTrue(b);
                Assert.AreEqual(v_int, o_int);
                Assert.IsTrue(Energy.Base.Text.TryParse<int>(s, out o_int));
                Assert.AreEqual(v_int, o_int);
                Assert.AreEqual(v_int, Energy.Base.Text.TryParse<int>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = int.TryParse(s, out o_int);
                Assert.IsTrue(b);
                Assert.AreEqual(v_int, o_int);
                Assert.IsTrue(Energy.Base.Text.TryParse<int>(s, out o_int));
                Assert.AreEqual(v_int, o_int);
                Assert.AreEqual(v_int, Energy.Base.Text.TryParse<int>(s));
            }

            foreach (var s_int in new string[] { "2147483648", "-2147483649", "+ 2147483647", "- 2147483648", "2147483647.0" })
            {
                s = s_int;

                b = int.TryParse(s, out o_int);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<int>(s, out o_int));
                Assert.AreEqual(0, Energy.Base.Text.TryParse<int>(s));
            }

            #endregion

            #region ulong

            ulong o_ulong;

            foreach (var v_ulong in new ulong[] { 0, 1, ulong.MinValue, ulong.MaxValue, 2, })
            {
                s = v_ulong.ToString();

                b = ulong.TryParse(s, out o_ulong);
                Assert.IsTrue(b);
                Assert.AreEqual(v_ulong, o_ulong);
                Assert.IsTrue(Energy.Base.Text.TryParse<ulong>(s, out o_ulong));
                Assert.AreEqual(v_ulong, o_ulong);
                Assert.AreEqual(v_ulong, Energy.Base.Text.TryParse<ulong>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = ulong.TryParse(s, out o_ulong);
                Assert.IsTrue(b);
                Assert.AreEqual(v_ulong, o_ulong);
                Assert.IsTrue(Energy.Base.Text.TryParse<ulong>(s, out o_ulong));
                Assert.AreEqual(v_ulong, o_ulong);
                Assert.AreEqual(v_ulong, Energy.Base.Text.TryParse<ulong>(s));
            }

            foreach (var s_ulong in new string[] { "18446744073709551616", "-1", "18446744073709551615.0" })
            {
                s = s_ulong;

                b = ulong.TryParse(s, out o_ulong);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<ulong>(s, out o_ulong));
                Assert.AreEqual((ulong)0, Energy.Base.Text.TryParse<ulong>(s));
            }

            #endregion

            #region long

            long o_long;

            foreach (var v_long in new long[] { 0, 1, -1, long.MinValue, long.MaxValue, 2, -2 })
            {
                s = v_long.ToString();

                b = long.TryParse(s, out o_long);
                Assert.IsTrue(b);
                Assert.AreEqual(v_long, o_long);
                Assert.IsTrue(Energy.Base.Text.TryParse<long>(s, out o_long));
                Assert.AreEqual(v_long, o_long);
                Assert.AreEqual(v_long, Energy.Base.Text.TryParse<long>(s));

                s = " \t\r\n\r " + s + " \t\r\n\r ";

                b = long.TryParse(s, out o_long);
                Assert.IsTrue(b);
                Assert.AreEqual(v_long, o_long);
                Assert.IsTrue(Energy.Base.Text.TryParse<long>(s, out o_long));
                Assert.AreEqual(v_long, o_long);
                Assert.AreEqual(v_long, Energy.Base.Text.TryParse<long>(s));
            }

            foreach (var s_long in new string[] { "9223372036854775808", "-9223372036854775809", "+ 9223372036854775807", "- 9223372036854775808", "9223372036854775807.0" })
            {
                s = s_long;

                b = long.TryParse(s, out o_long);
                Assert.IsFalse(b);
                Assert.IsFalse(Energy.Base.Text.TryParse<long>(s, out o_long));
                Assert.AreEqual(0, Energy.Base.Text.TryParse<long>(s));
            }

            #endregion
        }

        [TestMethod]
        public void SingleLine()
        {
            string a, r;
            a = null;
            r = Energy.Base.Text.SingleLine(a, null, false);
            Assert.IsNull(r);
            a = "";
            r = Energy.Base.Text.SingleLine(a, null, false);
            Assert.IsNotNull(r);
            Assert.AreEqual("", r);
            a = "\r";
            r = Energy.Base.Text.SingleLine(a, null, false);
            Assert.IsNotNull(r);
            Assert.AreEqual("", r);
            r = Energy.Base.Text.SingleLine(a, "\t", false);
            Assert.AreEqual("\t", r);
            a = "\r\r";
            r = Energy.Base.Text.SingleLine(a, null, false);
            Assert.IsNotNull(r);
            Assert.AreEqual("", r);
            r = Energy.Base.Text.SingleLine(a, "\t", false);
            Assert.AreEqual("\t\t", r);
        }

        [TestMethod]
        public void IncludeTrailing()
        {
            Assert.IsNotNull(Energy.Base.Text.IncludeTrailing(null, null));
            Assert.IsNotNull(Energy.Base.Text.IncludeTrailing("", null));
            Assert.IsNotNull(Energy.Base.Text.IncludeTrailing(null, ""));
            Assert.IsNotNull(Energy.Base.Text.IncludeTrailing(null, '\0'));
            Assert.AreEqual("", Energy.Base.Text.IncludeTrailing(null, null));
            Assert.AreEqual("", Energy.Base.Text.IncludeTrailing("", null));
            Assert.AreEqual("", Energy.Base.Text.IncludeTrailing(null, ""));
            Assert.AreEqual('\0'.ToString(), Energy.Base.Text.IncludeTrailing(null, '\0'));
            Assert.AreEqual("/", Energy.Base.Text.IncludeTrailing(null, "/"));
            Assert.AreEqual("/", Energy.Base.Text.IncludeTrailing("", "/"));
            Assert.AreEqual("/", Energy.Base.Text.IncludeTrailing(null, "/"));
            Assert.AreEqual('/'.ToString(), Energy.Base.Text.IncludeTrailing(null, '/'));
            Assert.AreEqual("/", Energy.Base.Text.IncludeTrailing("/", "/"));
            Assert.AreEqual("/a/", Energy.Base.Text.IncludeTrailing("/a", "/"));
            Assert.AreEqual("/a/", Energy.Base.Text.IncludeTrailing("/a/", "/"));
            Assert.AreEqual("a//", Energy.Base.Text.IncludeTrailing("a//", "/"));
            Assert.AreEqual('/'.ToString(), Energy.Base.Text.IncludeTrailing("/", '/'));
        }

        [TestMethod]
        public void IncludeLeading()
        {
            Assert.IsNotNull(Energy.Base.Text.IncludeLeading(null, null));
            Assert.IsNotNull(Energy.Base.Text.IncludeLeading("", null));
            Assert.IsNotNull(Energy.Base.Text.IncludeLeading(null, ""));
            Assert.IsNotNull(Energy.Base.Text.IncludeLeading(null, '\0'));
            Assert.AreEqual("", Energy.Base.Text.IncludeLeading(null, null));
            Assert.AreEqual("", Energy.Base.Text.IncludeLeading("", null));
            Assert.AreEqual("", Energy.Base.Text.IncludeLeading(null, ""));
            Assert.AreEqual('\0'.ToString(), Energy.Base.Text.IncludeLeading(null, '\0'));
            Assert.AreEqual("/", Energy.Base.Text.IncludeLeading(null, "/"));
            Assert.AreEqual("/", Energy.Base.Text.IncludeLeading("", "/"));
            Assert.AreEqual("/", Energy.Base.Text.IncludeLeading(null, "/"));
            Assert.AreEqual('/'.ToString(), Energy.Base.Text.IncludeLeading(null, '/'));
            Assert.AreEqual("/", Energy.Base.Text.IncludeLeading("/", "/"));
            Assert.AreEqual("/a", Energy.Base.Text.IncludeLeading("/a", "/"));
            Assert.AreEqual("/a/", Energy.Base.Text.IncludeLeading("a/", "/"));
            Assert.AreEqual("/a//", Energy.Base.Text.IncludeLeading("a//", "/"));
            Assert.AreEqual('/'.ToString(), Energy.Base.Text.IncludeLeading("/", '/'));
        }

        [TestMethod]
        public void RemoveEmptyElements()
        {
            Assert.IsNull(Energy.Base.Text.RemoveEmptyElements((List<string>)null));
            Assert.IsNull(Energy.Base.Text.RemoveEmptyElements((string[])null));
            Assert.AreEqual(0, Energy.Base.Text.RemoveEmptyElements(new string[] { "", null, "" }).Length);
            Assert.AreEqual(1, Energy.Base.Text.RemoveEmptyElements(new string[] { "", null, "X" }).Length);
            Assert.AreEqual(1, Energy.Base.Text.RemoveEmptyElements(new string[] { "X", null, "" }).Length);
            Assert.AreEqual(2, Energy.Base.Text.RemoveEmptyElements(new string[] { "X", null, "X" }).Length);
        }

        [TestMethod]
        public void RemoveEmptyLines()
        {
            Assert.IsNull(Energy.Base.Text.RemoveEmptyLines(null));
            Assert.IsNotNull(Energy.Base.Text.RemoveEmptyLines(""));
            Assert.AreEqual("", Energy.Base.Text.RemoveEmptyLines(""));
            Assert.AreEqual("", Energy.Base.Text.RemoveEmptyLines("\n"));
            Assert.AreEqual("", Energy.Base.Text.RemoveEmptyLines("\r\n"));
            Assert.AreEqual("", Energy.Base.Text.RemoveEmptyLines("\r\n\r\n"));
            Assert.AreEqual("A\n", Energy.Base.Text.RemoveEmptyLines("\nA\n"));
            Assert.AreEqual("A", Energy.Base.Text.RemoveEmptyLines("\r\nA"));
            Assert.AreEqual("A\r\n", Energy.Base.Text.RemoveEmptyLines("\r\nA\r\n"));
            Assert.AreEqual("A\r\nB\r\n", Energy.Base.Text.RemoveEmptyLines("A\r\nB\r\n"));
            Assert.AreEqual("A\r\nB\r\n", Energy.Base.Text.RemoveEmptyLines("\r\nA\r\nB\r\n"));
            Assert.AreEqual("A\r\nB\n", Energy.Base.Text.RemoveEmptyLines("\n\r\nA\r\n\nB\n\r\n"));
        }

        [TestMethod]
        public void Contains()
        {
            Assert.IsFalse(Energy.Base.Text.Contains(null, null));
            Assert.IsFalse(Energy.Base.Text.Contains("", null));
            Assert.IsFalse(Energy.Base.Text.Contains(null, null, false));
            Assert.IsFalse(Energy.Base.Text.Contains("", null, true));
            int _int = 123;
            Assert.IsFalse(Energy.Base.Text.Contains(_int, null));
            Assert.IsFalse(Energy.Base.Text.Contains(_int, "4"));
            Assert.IsTrue(Energy.Base.Text.Contains(_int, "3", true));
            string _string = "abc";
            Assert.IsFalse(Energy.Base.Text.Contains(_string, "A", false));
            Assert.IsTrue(Energy.Base.Text.Contains(_string, "A", true));
        }

        [TestMethod]
        public void EscapeExpression()
        {
            Assert.IsNull(Energy.Base.Text.EscapeExpression((string)null));
            Assert.AreEqual("", Energy.Base.Text.EscapeExpression(""));
            Assert.AreEqual("abc", Energy.Base.Text.EscapeExpression("abc"));
            Assert.AreEqual(@"a\$b\\c", Energy.Base.Text.EscapeExpression(@"a$b\c"));
            Assert.AreEqual(@"\.\$\^\{\[\(\|\)\*\+\?\\", Energy.Base.Text.EscapeExpression(@".$^{[(|)*+?\"));
        }
    }
}