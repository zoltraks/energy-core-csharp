using System;
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

            dictionary["One"] = 1;
            dictionary["TWO"] = 2;
            dictionary["ONE"] = 1 + (int)(dictionary["two"] ?? 1);

            Assert.AreEqual("ONE=2 one=", string.Concat("", "ONE=", dictionary["ONE"], " one=", dictionary["one"]));

            dictionary.CaseSensitive = false;

            Assert.AreEqual("ONE=1 one=1", string.Concat("", "ONE=", dictionary["ONE"], " one=", dictionary["one"]));
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
    }
}
