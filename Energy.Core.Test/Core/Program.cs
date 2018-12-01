using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Program
    {
        [TestMethod]
        public void CoreProgramClass()
        {
            Energy.Base.Lock lock1 = new Energy.Base.Lock();
            Assert.IsTrue(Energy.Core.Bug.Last.Code.Equals("C002"));
            Energy.Core.Bug.Entry emptyEntry = Energy.Core.Bug.Entry.Empty;
            Energy.Core.Bug.Last = emptyEntry;
            Energy.Core.Bug.Suppress("C002");
            Energy.Base.Lock lock2 = new Energy.Base.Lock();
            Assert.IsFalse(Energy.Core.Bug.Last.Code.Equals("C002"));
        }
    }
}
