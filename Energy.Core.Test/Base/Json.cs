using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Json
    {
        [TestMethod]
        public void JsonStrip()
        {
            Assert.IsNull(Energy.Base.Json.Strip(null));
            Assert.AreEqual("", Energy.Base.Json.Strip(""));

            string needle, expect, result;

            needle = "123.34";
            expect = needle;
            result = Energy.Base.Json.Strip(needle);
            Assert.AreEqual(expect, result);

            needle = @""" \"" """;
            expect = " \" ";
            result = Energy.Base.Json.Strip(needle);
            Assert.AreEqual(expect, result);

            needle = @""" \t """;
            expect = " \t ";
            result = Energy.Base.Json.Strip(needle);
            Assert.AreEqual(expect, result);

            string json1 = @"""Hello \""Name\""!\r\nHow are you?""";
            string text1 = Energy.Base.Json.Strip(json1);
            string unescaped1 = "Hello \"Name\"!\r\nHow are you?";
            Assert.AreEqual(unescaped1, text1);
            Assert.IsNull(Energy.Base.Json.Strip(null));
            Assert.IsNull(Energy.Base.Json.Strip("null"));
            Assert.AreEqual("null", Energy.Base.Json.Strip("\"null\""));
        }

        [TestMethod]
        public void JsonEscape()
        {
            Assert.IsNull(Energy.Base.Json.Escape(null));
            Assert.AreEqual("", Energy.Base.Json.Escape(""));

            string needle, expect, result;

            needle = "123.34";
            expect = needle;
            result = Energy.Base.Json.Escape(needle);
            Assert.AreEqual(expect, result);

            string text1 = "Hello \"Name\"!\r\nHow are you?";
            string json1 = Energy.Base.Json.Escape(text1);
            string escaped1 = @"Hello \""Name\""!\r\nHow are you?";
            Assert.AreEqual(escaped1, json1);
        }

        [TestMethod]
        public void JsonCreate()
        {
            Energy.Base.Json.JsonValue json1 = new Energy.Base.Json.JsonArray();
            ((Energy.Base.Json.JsonArray)json1).Array.Add(new Energy.Base.Json.JsonString() { Value = "abc" });
            ((Energy.Base.Json.JsonArray)json1).Array.Add(new Energy.Base.Json.JsonNumber() { Value = 123.45 });
            Energy.Base.Json.JsonLayout.Default.Reset();
            Energy.Base.Json.JsonLayout.Default.Indent = false;
            string jsonString1 = json1.ToString();
            string expectedString1 = "[\"abc\",123.45]";
            Assert.AreEqual(expectedString1, jsonString1);

            Energy.Base.Json.JsonValue json2 = new Energy.Base.Json.JsonArray();
            ((Energy.Base.Json.JsonArray)json2).Array.Add(null);
            ((Energy.Base.Json.JsonArray)json2).Array.Add(new Energy.Base.Json.JsonBoolean() { Value = true });
            Energy.Base.Json.JsonLayout.Default.Reset();
            Energy.Base.Json.JsonLayout.Default.Indent = false;
            string jsonString2 = json2.ToString();
            string expectedString2 = "[null,true]";
            Assert.AreEqual(expectedString2, jsonString2);
        }

        [TestMethod]
        public void JsonSerialize()
        {
            Energy.Base.Json.JsonLayout layout = new Energy.Base.Json.JsonLayout();
            Energy.Base.Json.Document doc;
            doc = new Energy.Base.Json.Document();
            doc.Root = new Energy.Base.Json.JsonObject();
            ((Energy.Base.Json.JsonObject)doc.Root).Dictionary.Add("Name"
                , new Energy.Base.Json.JsonString() { Value = "Hello \"Henry\"." });
            string result;
            layout.Compact = true;
            layout.Indent = false;
            result = doc.Serialize(layout);
            string expect;
            expect = "{\"Name\":\"Hello \\\"Henry\\\".\"}";
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void JsonQuote()
        {
            string expect, result, value;
            value = null;
            result = Energy.Base.Json.Quote(value);
            expect = "null";
            Assert.AreEqual(expect, result);
            value = "";
            result = Energy.Base.Json.Quote(value);
            expect = "\"\"";
            Assert.AreEqual(expect, result);
            value = "\t";
            result = Energy.Base.Json.Quote(value);
            expect = "\"\\t\"";
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void JsonUnescape()
        {
            string expect, result, needle;
            needle = null;
            result = Energy.Base.Json.Unescape(needle);
            expect = null;
            Assert.AreEqual(expect, result);
            needle = "";
            result = Energy.Base.Json.Unescape(needle);
            expect = "";
            Assert.AreEqual(expect, result);
            needle = "\\t";
            result = Energy.Base.Json.Unescape(needle);
            expect = "\t";
            Assert.AreEqual(expect, result);
            needle = "\\\\\\t";
            result = Energy.Base.Json.Unescape(needle);
            expect = "\\\t";
            Assert.AreEqual(expect, result);
        }
    }
}
