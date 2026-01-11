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

            Assert.Contains("Usage line one.", text, "First usage line should be printed.");
            Assert.Contains("Usage line two.", text, "Second usage line should be printed.");
            Assert.Contains("Greetings line one.", text, "First greetings line should be printed.");
            Assert.Contains("Greetings line two.", text, "Second greetings line should be printed.");
        }

        [TestMethod]
        public void PrintContainsAuthorSection()
        {
            var singleAuthor = new Energy.Base.Command.Arguments(new string[0])
                .Author("Single author");

            var singleText = singleAuthor.Print();

            Assert.Contains("AUTHOR", singleText, "Single author section header should be printed.");
            Assert.Contains("Single author", singleText, "Single author entry should be printed.");

            var multipleAuthors = new Energy.Base.Command.Arguments(new string[0])
                .Author("First author")
                .Author("Second author");

            var multipleText = multipleAuthors.Print();

            Assert.Contains("AUTHORS", multipleText, "Multiple author section header should be printed.");
            Assert.Contains("First author", multipleText, "First author entry should be printed.");
            Assert.Contains("Second author", multipleText, "Second author entry should be printed.");

            multipleAuthors.Author(null);
            var clearedText = multipleAuthors.Print();

            Assert.DoesNotContain("AUTHOR", clearedText, "Clearing authors should remove the author section.");
        }

        [TestMethod]
        public void PrintContainsLicenseSection()
        {
            var singleLicense = new Energy.Base.Command.Arguments(new string[0])
                .License("MIT License");

            var singleText = singleLicense.Print();

            Assert.Contains("LICENSE", singleText, "Single license section header should be printed.");
            Assert.Contains("MIT License", singleText, "Single license entry should be printed.");

            var multipleLicenses = new Energy.Base.Command.Arguments(new string[0])
                .License("MIT License")
                .License("Apache License 2.0");

            var multipleText = multipleLicenses.Print();

            Assert.Contains("LICENSES", multipleText, "Multiple license section header should be printed.");
            Assert.Contains("MIT License", multipleText, "First license entry should be printed.");
            Assert.Contains("Apache License 2.0", multipleText, "Second license entry should be printed.");

            multipleLicenses.License(null);
            var clearedText = multipleLicenses.Print();

            Assert.DoesNotContain("LICENSE", clearedText, "Clearing licenses should remove the license section.");
        }
    }
}