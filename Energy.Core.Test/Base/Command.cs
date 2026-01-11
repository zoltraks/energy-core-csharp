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

        [TestMethod]
        public void Enumeration()
        {
            var args = new string[] { "first", "--", "second" };
            var argv = new Energy.Base.Command.Arguments(args);
            int i = 0;
            foreach (string arg in argv)
            {
                Assert.AreEqual(arg, args[i++]);
            }
        }

        [TestMethod]
        public void PrintContainsUsageAndGreetingsLines()
        {
            var argv = new Energy.Base.Command.Arguments(new string[0])
                .Usage("Usage line one.")
                .Usage("Usage line two.")
                .Greetings("Greetings line one.")
                .Greetings("Greetings line two.");

            var text = argv.Print();

            Assert.IsTrue(text.Contains("Usage line one."), "First usage line should be printed.");
            Assert.IsTrue(text.Contains("Usage line two."), "Second usage line should be printed.");
            Assert.IsTrue(text.Contains("Greetings line one."), "First greetings line should be printed.");
            Assert.IsTrue(text.Contains("Greetings line two."), "Second greetings line should be printed.");
        }

        [TestMethod]
        public void PrintContainsAuthorSection()
        {
            var singleAuthor = new Energy.Base.Command.Arguments(new string[0])
                .Author("Single author");

            var singleText = singleAuthor.Print();

            Assert.IsTrue(singleText.Contains("AUTHOR"), "Single author section header should be printed.");
            Assert.IsTrue(singleText.Contains("Single author"), "Single author entry should be printed.");

            var multipleAuthors = new Energy.Base.Command.Arguments(new string[0])
                .Author("First author")
                .Author("Second author");

            var multipleText = multipleAuthors.Print();

            Assert.IsTrue(multipleText.Contains("AUTHORS"), "Multiple author section header should be printed.");
            Assert.IsTrue(multipleText.Contains("First author"), "First author entry should be printed.");
            Assert.IsTrue(multipleText.Contains("Second author"), "Second author entry should be printed.");

            multipleAuthors.Author(null);
            var clearedText = multipleAuthors.Print();

            Assert.IsFalse(clearedText.Contains("AUTHOR"), "Clearing authors should remove the author section.");
        }
    }
}