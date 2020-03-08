using System;

namespace TildeColorText
{
    class Program
    {
        static void Main(string[] args)
        {
            TryThread();

            Energy.Core.Tilde.WriteLine("~white~White text");
            Energy.Core.Tilde.WriteLine("~yellow~Yellow text");
            Energy.Core.Tilde.WriteLine("~green~Green text");
            Console.WriteLine();
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.Example.ColorPalleteTildeString);
            Console.BackgroundColor = backgroundColor;
            Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.Example.ColorPalleteTildeString);
            string example = "~green~Green or ~9~blue~0~? Two tildes: ~~. ~`escaped `` ~green~`~. One tilde ~`~`~.";
            Energy.Core.Tilde.WriteLine("Example text (~lc~tilde~0~):");
            Energy.Core.Tilde.WriteLine(example);
            Energy.Core.Tilde.WriteLine("Example text (~lc~escape~0~):");
            Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.Escape(example));
            Energy.Core.Tilde.WriteLine("Example text (~lc~strip~0~):");
            Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.Strip(example));
            Energy.Core.Tilde.WriteLine("Example text length = {0} (raw), length = {1} (tilde)"
                , example.Length, (int)Energy.Core.Tilde.Length(example));
            Energy.Core.Tilde.WriteLine(ConsoleColor.Yellow, example);
            Energy.Core.Tilde.WriteLine("~`~`~ ~`~0~``~0~`~ ~`~`~");

            TryException();

            Energy.Core.Tilde.Break(2);

            Energy.Core.Tilde.Pause();
        }

        private static void TryThread()
        {
            string input;
            input = Energy.Core.Tilde.Input("How many iterations?", "1");
            int n = Energy.Base.Cast.AsInteger(input);
            input = Energy.Core.Tilde.Input("Delay between?", "0");
            int d = Energy.Base.Cast.AsInteger(input);

            Energy.Core.Tilde.RunInThread = true;
            for (int i = 0; i < n; i++)
            {
                Energy.Core.Tilde.WriteLine(Energy.Core.Tilde.Utility.RainbowLine("-=-", Energy.Core.Tilde.Width - 1, i));
                System.Threading.Thread.Sleep(d);
            }

            Energy.Core.Tilde.Pause();
        }

        private static void TryException()
        {
            try
            {
                throw new NotSupportedException();
            }
            catch (Exception exception)
            {
                Energy.Core.Tilde.WriteException(exception, true);
            }
        }
    }
}
