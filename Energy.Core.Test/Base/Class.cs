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
            Type[] classList;
            classList = Energy.Base.Class.GetTypes(Energy.Base.Class.GetAssemblies()
                , delegate (Type e) { return e.FullName.StartsWith("Energy"); });
            Assert.IsTrue(classList.Length > 0);
            classList = Energy.Base.Class.GetTypes(Energy.Base.Class.GetAssemblies()
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

            var o2 = new { Text = "READ ONLY" };
            m = Energy.Base.Class.Mangle<string>(o2, delegate (string _) { return (_ as string ?? "").Trim(); });
            Assert.AreEqual(0, m);

            var o3 = new TestClass1() { Decimal1 = 1.2345m };
            m = Energy.Base.Class.Mangle<decimal>(o3, delegate (decimal _) { return _ - 1m; });
            Assert.AreEqual(1, m);
            Assert.AreEqual(0.2345m, o3.Decimal1);

            var msg = new TextClass() { Text = "" };
            if (1 == Energy.Base.Class.Mangle<string>(msg, "Text", delegate { return "Hello"; }))
            {
                Debug.WriteLine("I just set Text field or property to " + msg.Text);
            }
            var anon = new { Text = (string)null };
            if (0 == Energy.Base.Class.Mangle<string>(anon, "Text", delegate { return "Hello"; }))
            {
                Debug.WriteLine("I can't change anything in anonymous object");
            }

            m = Energy.Base.Class.Mangle<string>(o3, "Text1", delegate (string x) { return "Value1"; });
            Assert.AreEqual(1, m);
            Assert.AreEqual("Value1", o3.Text1);
        }

        public class TextClass
        {
            public string Text { get; set; }
        }

        public class TestClass1
        {
            public string Text1;
            public string Text2 { get; set; }
            public decimal Decimal1;
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

        [TestMethod]
        public void ClassAssembly()
        {
            var a = System.Reflection.Assembly.GetExecutingAssembly();
            var x = Energy.Base.Class.GetAssemblyFile(a);
            // TODO Something, something, etc...
        }

        [TestMethod]
        public void ClassGetObjectValue()
        {
            object o1, o2;
            o1 = new
            {
                Name = "Name",
                Index = 16,
            };
            o2 = Energy.Base.Class.GetObjectPropertyOrFieldValue(o1, "Name");
            Assert.IsNotNull(o2);
            o2 = Energy.Base.Class.GetObjectPropertyOrFieldValue(o1, "name");
            Assert.IsNull(o2);
            o2 = Energy.Base.Class.GetObjectPropertyOrFieldValue(o1, "name", true, false);
            Assert.IsNotNull(o2);
            o2 = Energy.Base.Class.GetObjectPropertyOrFieldValue(o1, "Index");
            Assert.AreEqual(16, o2);

            o2 = Energy.Base.Class.GetObjectFieldOrPropertyValue(o1, "Name");
            Assert.IsNotNull(o2);
            o2 = Energy.Base.Class.GetObjectFieldOrPropertyValue(o1, "name");
            Assert.IsNull(o2);
            o2 = Energy.Base.Class.GetObjectFieldOrPropertyValue(o1, "name", true, false);
            Assert.IsNotNull(o2);
            o2 = Energy.Base.Class.GetObjectFieldOrPropertyValue(o1, "Index");
            Assert.AreEqual(16, o2);
        }

        [TestMethod]
        public void ClassSetObjectValue()
        {
            object o1;
            o1 = new
            {
                Name = "Name",
                Index = 16,
            };
            Assert.AreEqual("Name", Energy.Base.Class.GetObjectFieldOrPropertyValue(o1, "Name"));
            Energy.Base.Class.SetObjectFieldOrPropertyValue(o1, "Name", "Name1");
            Assert.AreEqual("Name1", Energy.Base.Class.GetObjectFieldOrPropertyValue(o1, "Name"));
        }
    }
}
