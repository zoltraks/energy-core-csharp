﻿using System;
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
            string file2 = Energy.Base.File.Locate("Base/Cast", new string[] { "", "..\\.." }, new string[] { ".cs" });
            Assert.IsFalse(string.IsNullOrEmpty(file2));
            string file3 = Energy.Base.File.Locate("Base/Cast", new string[] { "", "..\\.." }, ".cs");
            Assert.IsFalse(string.IsNullOrEmpty(file3));
            string file4 = Energy.Base.File.Locate("Cast", new string[] { "", "..\\..\\Base" }, ".cs");
            Assert.IsFalse(string.IsNullOrEmpty(file4));
            string file5 = Energy.Base.File.Locate("Energy.Core.Test.txt", new string[] { "..\\.." }, ".csproj");
            Assert.IsFalse(string.IsNullOrEmpty(file5));
            string file6 = Energy.Base.File.Locate("Energy.Core.Test", new string[] { "..\\.." }, ".csproj");
            Assert.IsTrue(string.IsNullOrEmpty(file6));
            string file7 = Energy.Base.File.Locate("Energy.Core.Test.csproj", new string[] { "..\\.." });
            Assert.IsFalse(string.IsNullOrEmpty(file7));
            string file8 = Energy.Base.File.Locate("Energy.Core.Test.csproj", new string[] { "..\\.." }, "");
            Assert.IsFalse(string.IsNullOrEmpty(file8));
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
