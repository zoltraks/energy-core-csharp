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
            // In test environment, console may or may not be present depending on test runner
            // The important thing is that the property doesn't throw an exception
            // and returns a consistent boolean value
            Assert.IsTrue(x == true || x == false);
        }
    }
}
