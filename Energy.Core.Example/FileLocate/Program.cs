using System;
using System.Collections.Generic;
using System.Text;

namespace FileLocate
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine(ConsoleColor.Green, "File location example");
            string input = Energy.Core.Tilde.Input("~w~Enter command to find: ~y~", "notepad");
            Console.WriteLine(input);
            Console.ReadLine();
        }
    }
}
