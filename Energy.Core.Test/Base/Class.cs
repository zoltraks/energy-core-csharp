using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Class
    {
        private struct ExampleStruct
        {
            public string Value;
        }

        [TestMethod]
        public void ClassGetDefault()
        {
            object result;
            object expect;
            result = Energy.Base.Class.GetDefault(typeof(DateTime));
            expect = DateTime.MinValue;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Class.GetDefault<DateTime>();
            Assert.AreEqual(expect, result);
            result = Energy.Base.Class.GetDefault(typeof(ExampleStruct));
            expect = new ExampleStruct() { Value = null };
            Assert.AreEqual(expect, result);
            result = Energy.Base.Class.GetDefault<ExampleStruct>();
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void ClassFilter()
        {
            Type[] classList = Energy.Base.Class.GetClasses(Energy.Base.Class.GetAssemblies()
                , delegate (Type e) { return e.Name.StartsWith("Energy"); });
        }
    }
}
