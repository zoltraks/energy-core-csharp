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
            Assert.AreEqual(counter1.Value, 1);
            counter1.Decrement();
            Assert.AreEqual(counter1.Value, 0);
            counter1.Decrement();
            Assert.AreEqual(counter1.Value, 0);
            Energy.Base.Counter counter2 = new Energy.Base.Counter(15, 30, 5);
            counter2.Minimum = -15;
            counter2.Maximum = 100;
            counter2.Decrement();
            Assert.AreEqual(counter2.Value, (int)-1);
            Assert.AreEqual(counter2.Value, (long)-1);
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
