using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class File
    {
        [TestMethod]
        public void Locate()
        {
            string file1 = Energy.Base.File.Locate("Base/Cast", new string[] { ".;..;../.." }, new string[] { ".cs" });
            Debug.WriteLine(file1);
            bool empty = string.IsNullOrEmpty(file1);
            Assert.AreEqual(false, empty);
        }

        [TestMethod]
        public void State()
        {
            string fileName = Energy.Core.Program.GetExecutionFile();
            Energy.Base.File.State state = new Energy.Base.File.State(fileName);
            Assert.IsTrue(state.Exists());
            Assert.IsTrue(state.IsChanged());
            state.Refresh();
            Assert.IsFalse(state.IsChanged());
        }
    }
}
