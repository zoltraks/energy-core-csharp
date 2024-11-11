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
            string file1 = Energy.Base.File.Locate("Base/Cast", new string[] { ".", "..", "../.." }, new string[] { ".cs" });
            Debug.WriteLine(file1);
            bool empty = string.IsNullOrEmpty(file1);
            Assert.AreEqual(false, empty);
            string file2 = Energy.Base.File.Locate("Base/Cast", new string[] { "", "..\\.." }, new string[] { ".cs" });
            Assert.IsFalse(string.IsNullOrEmpty(file2));
            string file4 = Energy.Base.File.Locate("Cast", new string[] { "", "..\\..\\Base" }, new string[] { ".cs" });
            Assert.IsFalse(string.IsNullOrEmpty(file4));
            string file5 = Energy.Base.File.Locate("Energy.Core.Test.txt", new string[] { "..\\.." }, new string[] { ".csproj" });
            Assert.IsFalse(string.IsNullOrEmpty(file5));
            string file7 = Energy.Base.File.Locate("Energy.Core.Test.csproj", new string[] { "..\\.." });
            Assert.IsFalse(string.IsNullOrEmpty(file7));
            string file8 = Energy.Base.File.Locate("Energy.Core.Test.csproj", new string[] { "..\\.." }, null);
            Assert.IsFalse(string.IsNullOrEmpty(file8));
            Assert.IsNotNull(Energy.Base.File.Locate("BIS.instance", new string[] { @"C:\SWR\BIS" }));
            //string[] fileArray;
            //fileArray = new string[]
            //{
            //    "SQLServerManager14.msc",
            //    "SQLServerManager12.msc",
            //    "SQLServerManager10.msc",
            //    "SQLServerManager.msc",
            //};
            //string file9 = Energy.Base.File.Locate(fileArray, null, null, Enumeration.LocateBehaviour.Default);
            //Assert.IsNotNull(file9);
        }

        [TestMethod]
        public void State()
        {
            string fileName = Energy.Core.Program.GetExecutionFile();
            Energy.Base.File.State state;
            state = new Energy.Base.File.State(fileName);
            Assert.IsTrue(state.Exists());
            Assert.IsTrue(state.IsChanged());
            Assert.IsTrue(state.Refresh());
            Assert.IsFalse(state.IsChanged());
            Random random = new Random();
            state = new Energy.Base.File.State(fileName + "." + random.Next(100, 999) + ".tmp");
            Assert.IsFalse(state.Exists());
            Assert.IsTrue(state.Touch());
            Energy.Base.File.State clone = state.Clone() as Energy.Base.File.State;
            Assert.IsNotNull(clone);
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(clone.SetWriteStamp());
            Assert.IsTrue(state.IsChanged(), "Running tests may fail here");
            Assert.IsTrue(state.DeleteFile());
        }

        [TestMethod]
        public void GetHomeDirectory()
        {
            var home = Energy.Base.File.GetHomeDirectory();
            Assert.IsNotNull(home);
        }
    }
}
