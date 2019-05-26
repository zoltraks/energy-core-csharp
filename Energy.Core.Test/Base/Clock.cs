using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Clock
    {
        [TestMethod]
        public void HasExpired()
        {
            DateTime? d1;
            Assert.IsFalse(Energy.Base.Clock.HasExpired(null, -1, out d1, null));
            Assert.IsNull(d1);
            Assert.IsFalse(Energy.Base.Clock.HasExpired(null, -1, out d1));
            Assert.IsFalse(Energy.Base.Clock.HasExpired(null, -1));
            Assert.IsTrue(Energy.Base.Clock.HasExpired(null, 0));
            Assert.IsTrue(Energy.Base.Clock.HasExpired(d1, 0.00001, out d1));
            Assert.IsNotNull(d1);
        }
    }
}
