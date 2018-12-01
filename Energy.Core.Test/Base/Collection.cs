using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Collection
    {
        [TestMethod]
        public void StringDictionaryCaseSensitive()
        {
            var dictionary = new Energy.Base.Collection.StringDictionary<object>();

            dictionary.SelectionOfDuplicates = Energy.Enumeration.MultipleBehaviour.First;

            dictionary["One"] = 1;
            dictionary["TWO"] = 2;
            dictionary["ONE"] = 1 + (int)(dictionary["two"] ?? 1);

            Assert.AreEqual("ONE=2 one=", string.Concat("", "ONE=", dictionary["ONE"], " one=", dictionary["one"]));

            dictionary.CaseSensitive = false;

            Assert.AreEqual("ONE=1 one=1", string.Concat("", "ONE=", dictionary["ONE"], " one=", dictionary["one"]));

            string result;
            string expect;
            Energy.Base.Collection.StringDictionary sd;

            sd = new Energy.Base.Collection.StringDictionary();
            sd.CaseSensitive = false;
            sd.Set("Product", "123");
            sd.Set("product", "123456789");
            result = sd.Get("Product");
            expect = "123456789";
            Assert.AreEqual(expect, result);
            sd.CaseSensitive = true;
            result = sd.Get("Product");
            expect = "123456789";
            Assert.AreEqual(expect, result);

            sd = new Energy.Base.Collection.StringDictionary();
            sd.Set("Product", "123");
            sd.Set("product", "123456789");
            sd.Set("zzz1", "abc");

            result = sd.Get("Product");
            expect = "123";
            Assert.AreEqual(expect, result);

            result = sd.Get("zzz1");
            expect = "abc";
            Assert.AreEqual(expect, result);

            sd.CaseSensitive = false;
            result = sd.Get("Product");
            expect = "123456789";
            Assert.AreEqual(expect, result);
        }

        private class StringDictionaryToObjectArrayTestClass1
        {
            public string Vorname;
            public string Name;
            public string Zuname;
        }

        [TestMethod]
        public void StringDictionaryToObjectArray()
        {
            var dictionary = new Energy.Base.Collection.StringDictionary<object>();
            dictionary["X"] = "Y";
            dictionary["TestClass1"] = new StringDictionaryToObjectArrayTestClass1[]
            {
                new StringDictionaryToObjectArrayTestClass1()
                {
                    Vorname = "Hans",
                    Name = "Bauer",
                    Zuname = "Bauer",
                },
                new StringDictionaryToObjectArrayTestClass1()
                {
                    Vorname = "Hans",
                    Name = "Bauer",
                    Zuname = "Bauer",
                },

            };
            object[] objectArray1 = dictionary.ToObjectArray();
            Assert.IsNotNull(objectArray1);
            Assert.AreNotEqual(0, objectArray1.Length);
            object[] objectArray2 = Energy.Base.Cast.StringDictionaryToObjectArray<object>(dictionary);
            Assert.IsNotNull(objectArray2);
            Assert.AreEqual(objectArray1.Length, objectArray2.Length);
        }

        [TestMethod]
        public void StringDictionaryFilter()
        {
            Energy.Base.Collection.StringDictionary x = new Energy.Base.Collection.StringDictionary();
            x.Add("111-222-333", "111-222-333");
            x.Add("111-88888888-333", "111-88888888-333");
            x.Add("111-222", "111-222");
            x.Add("555-ABC-XYZ", "555-ABC-XYZ");
            x.Add("Ąę€", "Ąę€");

            string[] filters;
            Energy.Base.Collection.StringDictionary y;

            filters = new string[] { "111-" };

            y = x.Filter(Enumeration.MatchStyle.Any, Enumeration.MatchMode.Simple, true, filters);
            Assert.AreEqual(3, y.Count);

            filters = new string[]
            {
                "111-222"
            };
            y = x.Filter(Enumeration.MatchMode.Same, true, filters);
            Assert.AreEqual(1, y.Count);
            y = x.Filter(Enumeration.MatchMode.Simple, true, filters);
            Assert.AreEqual(2, y.Count);
        }

        [TestMethod]
        public void StringList()
        {
            string[] array;
            array = new string[] { "A", "B", "a" };
            Energy.Base.Collection.StringArray stringArray = new Energy.Base.Collection.StringArray(array);
            Assert.IsFalse(stringArray.HasDuplicates(), "String array should not have duplicates");
            Assert.IsTrue(stringArray.HasDuplicates(true), "String array should have duplicates when ignoreCase is true");
        }

        [TestMethod]
        public void StringDictionaryConstructor()
        {
            Dictionary<string, object> d;

            d = new Energy.Base.Collection.StringDictionary<object>("a");

            Assert.IsNotNull(d);
            Assert.AreEqual(0, d.Count);

            d = new Energy.Base.Collection.StringDictionary<object>("a", "b");

            Assert.IsNotNull(d);
            Assert.AreEqual(1, d.Count);
            Assert.AreEqual("b", d["a"]);

            d = new Energy.Base.Collection.StringDictionary<object>("a", "b", null);

            Assert.IsNotNull(d);
            Assert.AreEqual(1, d.Count);
            Assert.AreEqual("b", d["a"]);
        }

        [TestMethod]
        public void StringArrayToDictionaryCast()
        {
            var x = Energy.Base.Cast.StringArrayToDictionary<string, object>("0", null, "a", 1, "b", false, "c", true, null);
            Assert.IsNotNull(x);
            Assert.AreEqual(4, x.Count);
            Assert.AreEqual(false, Energy.Base.Cast.AsBool(x["b"]));
            Assert.AreEqual(true, Energy.Base.Cast.AsBool(x["c"]));
        }
    }
}
