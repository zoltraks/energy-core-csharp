using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class File
    {
        [TestMethod]
        public void Locate()
        {
            string file1 = Energy.Base.File.Locate("Cast", new string[] { ".;..;../.." }, new string[] { ".cs" });
            Assert.AreEqual("AA-A", file1);
        }
    }
}
