using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Command
    {
        [TestMethod]
        public void Arguments()
        {
            string args;
            Energy.Base.Command.Arguments argv;
            args = "/help";
            argv = new Energy.Base.Command.Arguments()
                .Switch("help")
                .Slashes(true)
                .Parse();
            Assert.IsNotNull(argv);
            Assert.IsFalse(argv["help"].Empty);
            argv.Alias("?", "help").Parse("");
            Assert.IsNotNull(argv);
            Assert.IsTrue(argv["help"].Empty);
            argv.Alias("?", "help").Parse("/?");
            Assert.IsNotNull(argv);
            Assert.IsFalse(argv["help"].Empty);

            args = @"-q --b -abc /? param1 ""param -x 2"" -- param3 /opt1/opt2";
            argv = new Energy.Base.Command.Arguments(args)
                .Alias("q", "quiet")
                .Alias("?", "help")
                .Switch("help")
                .Switch("quiet")
                .Parameter("help", 0)
                .Parameter("input")
                .Parameter("output")
                .Alias("i", "input")
                .Help("input", "Input file")
                //.Strict(true)
                //.Slashes(true)
                .Parse();
            Assert.IsNotNull(argv);
        }
    }
}