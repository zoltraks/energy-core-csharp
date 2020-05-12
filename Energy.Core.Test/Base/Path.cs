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
            check = new string[] { "C:\\", "A \"\"\\", "B \"\"\\" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "C:\\A \"Program Files\"/B \". .\"\\";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "C:\\", "A \"Program Files\"/", "B \". .\"\\" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "/c/folder/sub/file.txt";
            array = Energy.Base.Path.Split(path, new string[] { "/" }, new string[] { }, false, false);
            Assert.IsNotNull(array);
            check = new string[] { "/", "c/", "folder/", "sub/", "file.txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "C:\\Folder\\\\sub\\/\\file.txt";
            array = Energy.Base.Path.Split(path, null);
            Assert.IsNotNull(array);
            Assert.AreEqual(4, array.Length);
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(array, new string[] {
                "C:\\",
                "Folder\\\\",
                "sub\\/\\",
                "file.txt"
            }, false));

            path = "C:\\\"Program Files\"\\\\/\\C \"abc\" D\\";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "C:\\", "\"Program Files\"\\\\/\\", "C \"abc\" D\\" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "C:\\\\\"Program Files\"\\\\sub\\\\\"Stupid \\ \"\"My File\".txt";
            array = Energy.Base.Path.Split(path, null);
            Assert.IsNotNull(array);
            check = new string[] { "C:\\\\", "\"Program Files\"\\\\", "sub\\\\", "\"Stupid \\ \"\"My File\".txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "/c/'my folder'/'sub / slash'/file.txt";
            array = Energy.Base.Path.Split(path);
            Assert.IsNotNull(array);
            check = new string[] { "/", "c/", "'my folder'/", "'sub / slash'/", "file.txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "";
            array = Energy.Base.Path.Split(path, new string[] { }, new string[] { });
            check = new string[] { };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            path = "a/b";
            array = Energy.Base.Path.Split(path, new string[] { }, new string[] { });
            check = new string[] { "a/b" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));
        }

        [TestMethod]
        public void PathIsSeparator()
        {
            string[] array, check;
            string path;

            Assert.IsTrue(Energy.Base.Path.IsSeparator("\\"));
            Assert.IsTrue(Energy.Base.Path.IsSeparator("/"));
            Assert.IsTrue(Energy.Base.Path.IsSeparator("\\/"));

            Assert.IsFalse(Energy.Base.Path.IsSeparator("C:\\"));
            Assert.IsFalse(Energy.Base.Path.IsSeparator("c/"));
            Assert.IsFalse(Energy.Base.Path.IsSeparator("\\/home"));

            path = "C:\\Folder\\File.txt";
            array = Energy.Base.Path.Split(path, Energy.Base.Path.SplitFormat.Create("\\/", ""));
            Assert.IsNotNull(array);
            check = new string[] { "C:\\", "Folder\\", "File.txt" };
            Assert.IsTrue(0 == Energy.Base.Collection.StringArray.Compare(check, array));

            Assert.IsFalse(Energy.Base.Path.IsSeparator(array[0], "\\/"));
            Assert.IsFalse(Energy.Base.Path.IsSeparator(array[1], "\\/"));

            Assert.IsTrue(Energy.Base.Path.IsSeparator(array[0].Substring(array[0].Length - 1)));
            Assert.IsTrue(Energy.Base.Path.IsSeparator(array[1].Substring(array[1].Length - 1)));
        }

        [TestMethod]
        public void PathBuildSeparatorPattern()
        {
            string result;
            result = Energy.Base.Path.BuildSeparatorPattern(null);
            Assert.IsNull(result);
            result = Energy.Base.Path.BuildSeparatorPattern(new string[] { });
            Assert.AreEqual("", result);
            result = Energy.Base.Path.BuildSeparatorPattern(new string[] { null });
            Assert.AreEqual("", result);
            result = Energy.Base.Path.BuildSeparatorPattern(new string[] { null, null });
            Assert.AreEqual("", result);
            result = Energy.Base.Path.BuildSeparatorPattern(new string[] { "", "A", "B", "C" });
            Assert.AreEqual("[ABC]+", result);
            result = Energy.Base.Path.BuildSeparatorPattern(new string[] { "\\" });
            Assert.AreEqual("\\\\+", result);
            result = Energy.Base.Path.BuildSeparatorPattern(new string[] { "<<", "><", ">>" });
            Assert.AreEqual("(?:<<|><|>>)+", result);
        }

        [TestMethod]
        public void PathTrimLeft()
        {
            Assert.AreEqual("abc/", Energy.Base.Path.TrimLeft("/abc/"));
            Assert.AreEqual("abc", Energy.Base.Path.TrimLeft("abc"));
            Assert.AreEqual("abc\\//", Energy.Base.Path.TrimLeft("\\//abc\\//"));
            Assert.AreEqual("abc\\//", Energy.Base.Path.TrimLeft("abc\\//"));
        }

        [TestMethod]
        public void PathTrimRight()
        {
            Assert.AreEqual("/abc", Energy.Base.Path.TrimRight("/abc/"));
            Assert.AreEqual("abc", Energy.Base.Path.TrimRight("abc"));
            Assert.AreEqual("\\//abc", Energy.Base.Path.TrimRight("\\//abc\\//"));
            Assert.AreEqual("\\//abc", Energy.Base.Path.TrimRight("\\//abc"));
        }

        [TestMethod]
        public void PathTrim()
        {
            Assert.AreEqual("abc", Energy.Base.Path.Trim("/abc/"));
            Assert.AreEqual("abc", Energy.Base.Path.Trim("abc"));
            Assert.AreEqual("abc", Energy.Base.Path.Trim("\\//abc\\//"));
            Assert.AreEqual("abc", Energy.Base.Path.Trim("\\//abc"));
            Assert.AreEqual("abc", Energy.Base.Path.Trim("abc\\//"));
        }

        [TestMethod]
        public void PathWalk()
        {
            Assert.IsNull(Energy.Base.Path.Walk(null));
            Assert.AreEqual("", Energy.Base.Path.Walk(""));
            Assert.AreNotEqual(".", Energy.Base.Path.Walk("."));
            Assert.AreEqual("c:", Energy.Base.Path.Walk("c:"));
            Assert.AreEqual("c:/", Energy.Base.Path.Walk("c://"));
            Assert.AreEqual("c:\\", Energy.Base.Path.Walk("c:\\/\\/"));
            Assert.AreEqual("c:/", Energy.Base.Path.Walk("c:/\\/\\/"));
            Assert.AreEqual("c:/", Energy.Base.Path.Walk("c:/a/.."));
            Assert.AreEqual("c:/", Energy.Base.Path.Walk("c:/a/../"));
            Assert.AreEqual("c:/a/", Energy.Base.Path.Walk("c:/a/b/.."));
            Assert.AreEqual("c:/a/", Energy.Base.Path.Walk("c:/a/b/../"));
            Assert.AreEqual("c:/a/x", Energy.Base.Path.Walk("c:/a/b/../x"));
            Assert.AreEqual("c:/x", Energy.Base.Path.Walk("c:/a/../x"));
            Assert.AreEqual("c:/x", Energy.Base.Path.Walk("c:/a/../../x"));
            Assert.AreEqual("c:/x", Energy.Base.Path.Walk("c:/./a/./../b/./../x"));
            Assert.AreEqual("c:/x/", Energy.Base.Path.Walk("c:/./a/./../b/./../x/."));
            Assert.AreEqual("c:/", Energy.Base.Path.Walk("c:/./a/./../b/./../x/.."));
        }
    }
}
