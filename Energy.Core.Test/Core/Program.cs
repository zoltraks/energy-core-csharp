using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Program
    {
        [TestMethod]
        public void ProgramGetCommandName()
        {
            System.Reflection.Assembly asm = Energy.Core.Program.GetAssembly();
            string cmd = Energy.Core.Program.GetCommandName();
            Assert.IsNotNull(cmd);
        }

        [TestMethod]
        public void ProgramGetExecution()
        {
            string directory = Energy.Core.Program.GetExecutionDirectory();
            string file = Energy.Core.Program.GetExecutionFile();
            Assert.IsNotNull(directory);
            Assert.IsNotNull(file);
            Assert.IsTrue(file.EndsWith(".dll"));
            Assert.IsTrue(directory.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()));
            Assert.IsFalse(directory.EndsWith(".dll" + System.IO.Path.DirectorySeparatorChar));
        }
    }
}
