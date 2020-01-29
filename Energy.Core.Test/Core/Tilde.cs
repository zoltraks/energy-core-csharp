using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Tilde
    {
        [TestMethod]
        public void ConsolePresent()
        {
            bool x = Energy.Core.Tilde.IsConsolePresent;
            Assert.IsFalse(x);
        }
    }
}
