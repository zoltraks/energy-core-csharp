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
            argv = new Energy.Base.Command.Arguments(args)
                .Switch("help")
                .Slash(true)
                .Parse();
            Assert.IsNotNull(argv);
            Assert.IsFalse(argv["help"].IsEmpty);
            argv.Alias("?", "help").Parse("");
            Assert.IsNotNull(argv);
            Assert.IsTrue(argv["help"].IsEmpty);
            argv.Alias("?", "help");
            argv.Parse("/?");
            Assert.IsNotNull(argv);
            Assert.IsFalse(argv["help"].IsEmpty);

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
                //.Slash(true)
                .Parse();
            Assert.IsNotNull(argv);

            argv.Long(false).Short(false).Slash(false);
            argv.Parse();

            Assert.AreEqual(9, argv.Count);

            int n;
            n = 0;
            argv.Options.ForEach((x) =>
            {
                if (!x.IsNull)
                {
                    n++;
                }
            });
            Assert.AreEqual(0, n);

            argv.Skip(1).Parse();
            Assert.AreEqual(8, argv.Count);
        }
    }
}