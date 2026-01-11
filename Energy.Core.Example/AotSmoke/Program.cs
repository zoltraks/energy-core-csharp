using System;

namespace AotSmoke
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            // Exercise a handful of Energy.Core APIs that must remain AOT-friendly.
            var parsed = Energy.Base.Cast.StringToInteger("42");
            var normalized = Energy.Base.Cast.DoubleToString(3.1415d);
            var hex = Energy.Base.Hex.Print(new byte[] { 0, 1, 2, 3, 4, 5 });

            if (parsed != 42 || normalized != "3.1415" || hex.Length == 0)
            {
                Console.Error.WriteLine("Energy.Core AOT smoke test failed");
                return 1;
            }

            Console.WriteLine("Energy.Core AOT smoke test succeeded");
            return 0;
        }
    }
}
