using System;
using System.Diagnostics;
using System.Reflection;

namespace PrintArgs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Energy.Base.Hello.World());
            var command = Energy.Core.Program.GetCommandName();

            var argv = new Energy.Base.Command.Arguments(args)
                .Title("Program title")
                .Usage("Usage information part one.")
                .Usage("Usage information part two.")
                .Usage("Usage information part three.")
                .About("This is information about the program.")
                .Switch("help")
                .Alias("h", "help")
                .Description("help", "Show this help")
                .Switch("version")
                .Alias("V", "version")
                .Description("version", "Show version number")
                .Switch("info")
                .Alias("i", "info")
                .Description("info", "Show information")
                .Parameter("output")
                .Alias("o", "output")
                .Description("output", "Output file name")
                .Switch("overwrite")
                .Description("overwrite", "Overwrite existing file")
                .Alias("w", "overwrite")
                .Switch("verbose")
                .Alias("v", "verbose")
                .Description("verbose", "Print additional information")
                .Note("Note line one.")
                .Note("Note line two.")
                .Greetings("Greetings line one.")
                .Greetings("Greetings line two.")
                .Repository("https://github.com/repository")
                .Copyright("2024-2026 All rights reversed")
                .Copyright("2024-2026 Another rights reversed")
                .Parse()
                ;

            Console.WriteLine($"Command: {command}");
            Console.WriteLine(argv.Print().TrimEnd());
            if (Debugger.IsAttached)
                Console.ReadLine();
        }
    }
}
