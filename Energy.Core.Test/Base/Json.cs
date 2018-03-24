﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Json
    {
        [TestMethod]
        public void JsonStrip()
        {
            string json1 = @"""Hello \""Name\""!\r\nHow are you?""";
            string text1 = Energy.Base.Json.Strip(json1);
            string unescaped1 = "Hello \"Name\"!\r\nHow are you?";
            Assert.AreEqual(unescaped1, text1);
        }

        [TestMethod]
        public void JsonEscape()
        {
            string text1 = "Hello \"Name\"!\r\nHow are you?";
            string json1 = Energy.Base.Json.Escape(text1);
            string escaped1 = @"""Hello \""Name\""!\r\nHow are you?""";
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
    }
}