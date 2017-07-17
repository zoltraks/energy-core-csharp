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
    }
}
