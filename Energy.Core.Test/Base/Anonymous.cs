using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Anonymous
    {
        [TestMethod]
        public void Anonymous_StringDelegate()
        {
            // Test parameterless string delegate
            Energy.Base.Anonymous.String stringFunc = () => "Hello World";
            string result = stringFunc();
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod]
        public void Anonymous_GenericStringDelegate()
        {
            // Test generic string delegate with parameter
            Energy.Base.Anonymous.String<int> intToStringFunc = (int x) => $"Number: {x}";
            string result = intToStringFunc(42);
            Assert.AreEqual("Number: 42", result);

            Energy.Base.Anonymous.String<string> stringToStringFunc = (string s) => s.ToUpper();
            result = stringToStringFunc("hello");
            Assert.AreEqual("HELLO", result);
        }

        [TestMethod]
        public void Anonymous_StateDelegate()
        {
            // Test state delegate (same input and output type)
            Energy.Base.Anonymous.State<int> doubleFunc = (int x) => x * 2;
            int intResult = doubleFunc(21);
            Assert.AreEqual(42, intResult);

            Energy.Base.Anonymous.State<string> reverseFunc = (string s) => new string(s.Reverse().ToArray());
            string stringResult = reverseFunc("hello");
            Assert.AreEqual("olleh", stringResult);
        }

        [TestMethod]
        public void Anonymous_FunctionDelegate_NoParameters()
        {
            // Test function delegate with no parameters
            Energy.Base.Anonymous.Function<int> getRandomNumber = () => 42;
            int result = getRandomNumber();
            Assert.AreEqual(42, result);

            Energy.Base.Anonymous.Function<string> getString = () => "test";
            string stringResult = getString();
            Assert.AreEqual("test", stringResult);
        }

        [TestMethod]
        public void Anonymous_FunctionDelegate_OneParameter()
        {
            // Test function delegate with one parameter
            Energy.Base.Anonymous.Function<int, int> squareFunc = (int x) => x * x;
            int result = squareFunc(5);
            Assert.AreEqual(25, result);

            Energy.Base.Anonymous.Function<int, string> lengthFunc = (string s) => s.Length;
            result = lengthFunc("hello");
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Anonymous_FunctionDelegate_TwoParameters()
        {
            // Test function delegate with two parameters
            Energy.Base.Anonymous.Function<int, int, int> addFunc = (int a, int b) => a + b;
            int result = addFunc(20, 22);
            Assert.AreEqual(42, result);

            Energy.Base.Anonymous.Function<string, string, string> concatFunc = (string a, string b) => a + " " + b;
            string stringResult = concatFunc("Hello", "World");
            Assert.AreEqual("Hello World", stringResult);

            Energy.Base.Anonymous.Function<bool, int, int> greaterThanFunc = (int a, int b) => a > b;
            bool boolResult = greaterThanFunc(10, 5);
            Assert.IsTrue(boolResult);
        }

        [TestMethod]
        public void Anonymous_DelegateChaining()
        {
            // Test delegate chaining with multiple functions
            Energy.Base.Anonymous.Function<int, int> multiplyBy2 = (int x) => x * 2;
            Energy.Base.Anonymous.Function<int, int> add10 = (int x) => x + 10;
            
            // Chain delegates - only the last delegate's return value is used
            Energy.Base.Anonymous.Function<int, int> chained = multiplyBy2 + add10;
            int result = chained(5); // Should execute both but return only add10 result: 5+10=15
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void Anonymous_DelegateAsParameter()
        {
            // Test passing delegates as parameters
            string ProcessNumber(Energy.Base.Anonymous.Function<string, int> processor, int number)
            {
                return processor(number);
            }

            Energy.Base.Anonymous.Function<string, int> numberToText = (int x) => $"Number {x} is processed";
            string result = ProcessNumber(numberToText, 42);
            Assert.AreEqual("Number 42 is processed", result);
        }

        [TestMethod]
        public void Anonymous_DelegateNullHandling()
        {
            // Test null delegate handling
            Energy.Base.Anonymous.String nullStringFunc = null;
            Assert.IsNull(nullStringFunc);

            // Test invoking null delegate should throw
            Assert.ThrowsException<NullReferenceException>(() =>
            {
                var result = nullStringFunc();
            });
        }

        [TestMethod]
        public void Anonymous_DelegateInvocationList()
        {
            // Test delegate invocation list
            Energy.Base.Anonymous.Function<int, int> func = null;
            int callCount = 0;

            func += (int x) => { callCount++; return x * 2; };
            func += (int x) => { callCount++; return x + 10; };

            int result = func(5);
            Assert.AreEqual(15, result); // Last delegate in list determines return value
            Assert.AreEqual(2, callCount); // Both delegates were called
        }
    }
}
