﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Bug
    {
        [TestMethod]
        public void CoreBugSuppress()
        {
            Energy.Core.Bug.Suppress("C002", false);
            Energy.Base.Lock lock1 = new Energy.Base.Lock();
            Assert.IsTrue(Energy.Core.Bug.Last.Code.Equals("C002"));
            Energy.Core.Bug.Entry defaultEntry = default(Energy.Core.Bug.Entry);
            Energy.Core.Bug.Last = defaultEntry;
            Energy.Core.Bug.Clear();
            Energy.Core.Bug.Suppress("C002");
            Energy.Base.Lock lock2 = new Energy.Base.Lock();
            Assert.IsFalse(Energy.Core.Bug.Last.Code.Equals("C002"));
        }
    }
}
