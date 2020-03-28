using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Path
    {
        [TestMethod]
        public void PathVariableSplit()
        {
            string[] array;
            array = Energy.Base.Path.Environment.Split(System.Environment.GetEnvironmentVariable("path"), null);
            Assert.IsNotNull(array);
            array = Energy.Base.Path.Environment.AsArray;
            Assert.IsNotNull(array);
        }

        [TestMethod]
        public void PathSplit()
        {
            string[] array, check;
            string path;

            path = "C:\\A \"\"\\B \"\"\\";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "C:", "A \"\"", "B \"\"" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "C:\\A \"Program Files\"\\B \". .\"\\";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "C:", "A \"Program Files\"", "B \". .\"" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "/c/folder/sub/file.txt";
            array = Energy.Base.Path.Split(path, new string[] { "/" }, new string[] { }, false, false, false, false);
            Assert.IsNotNull(array);
            check = new string[] { "c", "folder", "sub", "file.txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "C:\\Folder\\\\sub\\/\\file.txt";
            array = Energy.Base.Path.Split(path, null, true, false);
            Assert.IsNotNull(array);
            Assert.AreEqual(7, array.Length);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(array, new string[] {
                "C:",
                "\\",
                "Folder",
                "\\\\",
                "sub",
                "\\/\\",
                "file.txt"
            }, false));

            path = "C:\\\"Program Files\"\\\\/\\C \"abc\" D\\";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "C:", "\"Program Files\"", "C \"abc\" D" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "C:\\\\\"Program Files\"\\\\sub\\\\\"Stupid \\ \"\"My File\".txt";
            array = Energy.Base.Path.Split(path, null, true, true);
            Assert.IsNotNull(array);
            check = new string[] { "C:", "\\\\", "\"Program Files\"", "\\\\", "sub", "\\\\", "\"Stupid \\ \"\"My File\".txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "/c/'my folder'/'sub / slash'/file.txt";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "c", "'my folder'", "'sub / slash'", "file.txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));
        }
    }
}
