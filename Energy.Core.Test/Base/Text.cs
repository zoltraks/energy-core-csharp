﻿using System;
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
            object o;
            o = 12;
            result = Energy.Base.Text.Quote(o);
            expect = "\"12\"";
            Assert.AreEqual(expect, result);
            o = 12.34;
            result = Energy.Base.Text.Quote(o);
            expect = "\"12.34\"";
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void TextStrip()
        {
            string expect;
            string value;
            string result;
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
            Assert.AreEqual(true, Energy.Base.Text.InArray(a, null));
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
    }
}