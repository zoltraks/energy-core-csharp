using System;
using System.Collections.Generic;
using System.Text;

namespace TildeColorText
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("~white~White text");
            Energy.Core.Tilde.WriteLine("~yellow~Yellow text");
            Energy.Core.Tilde.WriteLine("~green~Green text");
            Console.WriteLine();
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.ExampleColorPalleteTildeString);
            Console.BackgroundColor = backgroundColor;
            Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.ExampleColorPalleteTildeString);
            string example = "Text ~green~green ~9~blue~0~ ~~~ ~#escaped ## ~green~#~...";
            Console.WriteLine("Example text (raw):");
            Console.WriteLine(example);
            Console.WriteLine("Example text (tilde):");
            Energy.Core.Tilde.WriteLine(example);
            Console.WriteLine("Example text (strip):");
            Console.WriteLine(Energy.Core.Tilde.Strip(example));
            Console.WriteLine("Example text length = {0} (raw), length = {1} (tilde)"
                , example.Length, (int)Energy.Core.Tilde.Length(example));
            Energy.Core.Tilde.WriteLine(ConsoleColor.Yellow, example);

            try
            {
                throw new NotSupportedException();
            }
            catch (Exception exception)
            {
                Energy.Core.Tilde.WriteException(exception, true);
            }

            Energy.Core.Tilde.Break(2);

            Energy.Core.Tilde.Pause();
        }
    }
}
