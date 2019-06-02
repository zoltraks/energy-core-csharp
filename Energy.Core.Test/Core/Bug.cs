using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Bug
    {
        [TestMethod]
        public void BugSuppress()
        {
            Energy.Core.Bug.Suppress("C000", false);
            var lock1 = new Class.ExampleBug();
            Assert.IsNotNull(lock1);
            Assert.IsTrue(Energy.Core.Bug.Last.Code.Equals("C000"));
#pragma warning disable IDE0034 // Simplify 'default' expression
            Energy.Core.Bug.Entry defaultEntry = default(Energy.Core.Bug.Entry);
#pragma warning restore IDE0034 // Simplify 'default' expression
            Energy.Core.Bug.Last = defaultEntry;
            Energy.Core.Bug.Clear();
            Energy.Core.Bug.Suppress("C000");
            Energy.Base.Lock lock2 = new Energy.Base.Lock();
            Assert.IsNotNull(lock2);
            Assert.IsFalse(Energy.Core.Bug.Last.Code.Equals("C000"));
        }

        public class Class
        {
            public class ExampleBug
            {
                public ExampleBug()
                {
                    Energy.Core.Bug.Write("C000", "Something in my mind");
                }
            }
        }
    }
}
