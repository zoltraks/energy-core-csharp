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
        public void StringArrayHasDuplicates()
        {
            string[] array;
            array = new string[] { "A", "B", "a" };
            Energy.Base.Collection.StringArray stringArray = new Energy.Base.Collection.StringArray(array);
            Assert.IsFalse(stringArray.HasDuplicates(), "String array should not have duplicates");
            Assert.IsTrue(stringArray.HasDuplicates(true), "String array should have duplicates when ignoreCase is true");
        }

        [TestMethod]
        public void StringArrayIndexOf()
        {
            string[] array;
            array = new string[] { "A", "B", "a" };
            Assert.AreEqual(2, Energy.Base.Collection.StringArray.IndexOf(array, "a"));
            Assert.AreEqual(2, Energy.Base.Collection.StringArray.IndexOf(array, "a", false));
            Assert.AreEqual(0, Energy.Base.Collection.StringArray.IndexOf(array, "a", true));
            Assert.AreEqual(2, Energy.Base.Collection.StringArray.IndexOf(array, "a", true, 1));
            Assert.AreEqual(0, Energy.Base.Collection.StringArray.IndexOf(array, "a", true, 0));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, null));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "c"));
            array = new string[] { };
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a"));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", false));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", true));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", true, 1));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", true, 0));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, null));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "c"));
            array = null;
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a"));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", false));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", true));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", true, 1));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "a", true, 0));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, null));
            Assert.AreEqual(-1, Energy.Base.Collection.StringArray.IndexOf(array, "c"));
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

        [TestMethod]
        public void StringArrayExclude()
        {
            string[] array, exclude, expect, result;

            array = new string[] { "a", "b", "c", null };
            exclude = new string[] { null };

            expect = new string[] { "a", "b", "c" };
            result = Energy.Base.Collection.StringArray.Exclude(array, exclude, false);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            exclude = new string[] { "a", "B", null };
            expect = new string[] { "b", "c" };
            result = Energy.Base.Collection.StringArray.Exclude(array, exclude, false);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));

            expect = new string[] { "c" };
            result = Energy.Base.Collection.StringArray.Exclude(array, exclude, true);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(expect, result));
        }

        [TestMethod]
        public void TableCreation()
        {
            Energy.Base.Collection.Table<string, object> t = new Energy.Base.Collection.Table<string, object>();
            var r1 = t.New();
            r1["a"] = "b";
            r1[0] = "x";
            r1["b"] = "c";
            r1.Append("", "X");
            r1.Append("", "Y");
            //r1.Add("x");
            //var v1 = r1.New();
            //v1 = "A";
            var v2 = r1[0];
            r1[0] = "B";
            var v3 = r1[0];
            bool eq1 = //v1 == v2 && 
                v2 == v3;
            Assert.IsTrue(eq1);
            var r2 = t.New();
            Assert.IsNotNull(t);
            Assert.IsNotNull(r1);
            Assert.IsNotNull(r2);
        }
    }
}
