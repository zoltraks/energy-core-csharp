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
            bool first = Energy.Core.Tilde.IsConsolePresent;
            bool second = Energy.Core.Tilde.IsConsolePresent;
            Assert.AreEqual(first, second);
        }
    }
}
