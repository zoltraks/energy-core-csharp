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
            Type[] classList = Energy.Base.Class.GetTypes(Energy.Base.Class.GetAssemblies()
                , delegate (Type e) { return e.FullName.StartsWith("Energy"); });
            Assert.IsTrue(classList.Length > 0);
        }

        [TestMethod]
        public void ClassMangle()
        {
            int m;

            var o1 = new TestClass1()
            {
                Text1 = " a ",
                Text2 = " b ",
            };

            m = Energy.Base.Class.Mangle<string>(o1, delegate (string _) { return (_ as string ?? "").Trim(); });
            Assert.AreEqual(2, m);
            Assert.AreEqual("a", o1.Text1);
            Assert.AreEqual("b", o1.Text2);

            var o = new { Text = "READ ONLY" };
            m = Energy.Base.Class.Mangle<string>(o, delegate (string _) { return (_ as string ?? "").Trim(); });
            Assert.AreEqual(0, m);
        }

        public class TestClass1
        {
            public string Text1;
            public string Text2 { get; set; }
            public void Function1() { }
            public static void Function2() { }
            public TestClass1() { }
        }

        public static class TestClass2
        {
            public static int Integer1;
            public static long Long1;
            public static void Function1()
            {
            }
        }

        public class TestClass3
        {
            public string Text1;
            public TestClass3(string text1) { }
            public TestClass3(string text1, bool useless) { }
        }

        public class TestClass4
        {
            public string Text1;
            private TestClass4() { }
        }

        [TestMethod]
        public void ClassIs()
        {
            Assert.IsFalse(Energy.Base.Class.IsStatic(typeof(TestClass1)));
            Assert.IsTrue(Energy.Base.Class.IsStatic(typeof(TestClass2)));
            Assert.IsTrue(Energy.Base.Class.IsInstance(typeof(TestClass1)));
            Assert.IsFalse(Energy.Base.Class.IsInstance(typeof(TestClass2)));
            Assert.IsTrue(Energy.Base.Class.IsInstance(typeof(TestClass3)));
            Assert.IsFalse(Energy.Base.Class.IsInstance(typeof(TestClass4)));
            Assert.IsTrue(Energy.Base.Class.HasParameterlessConstructor(typeof(TestClass1)));
            Assert.IsFalse(Energy.Base.Class.HasParameterlessConstructor(typeof(TestClass2)));
            Assert.IsFalse(Energy.Base.Class.HasParameterlessConstructor(typeof(TestClass3)));
            Assert.IsFalse(Energy.Base.Class.HasParameterlessConstructor(typeof(TestClass4)));
            Assert.IsFalse(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass1)));
            Assert.IsFalse(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass2)));
            Assert.IsTrue(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass3)));
            Assert.IsFalse(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass4)));
            Assert.IsTrue(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass3), 2, 0));
            Assert.IsFalse(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass3), 3, 0));
            Assert.IsTrue(Energy.Base.Class.HasParameteredConstructor(typeof(TestClass3), 0, 1));
        }
    }
}
