using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Counter
    {
        [TestMethod]
        public void Decrement()
        {
            Energy.Base.Counter counter1 = new Energy.Base.Counter(1);
            Assert.AreEqual(1, counter1.Value);
            counter1.Decrement();
            Assert.AreEqual(1, counter1.Value);
            Energy.Base.Counter counter2 = new Energy.Base.Counter(15, 30, 5);
            Assert.AreEqual(5, counter2.Value);
            counter2.Decrement();
            Assert.AreEqual(15, counter2.Value);
            counter2.Minimum = 0;
            counter2.Maximum = 100;
            counter2.Decrement();
            Assert.AreEqual(14, counter2.Value);
            counter2.Value = 29;
            counter2.Increment();
            Assert.AreEqual(30, counter2.Value);
        }

        [TestMethod]
        public void Reset()
        {
            Energy.Base.Counter counter1 = new Energy.Base.Counter();
            Assert.AreEqual(counter1.Overflow, false);
            counter1.Decrement();
            Assert.AreEqual(counter1.Overflow, true);
            counter1.Reset();
            Assert.AreEqual(counter1.Overflow, false);
        }
    }
}
