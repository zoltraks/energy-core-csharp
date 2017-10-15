using System;
using System.Collections.Generic;
using System.Text;

namespace FileLocate
{
    class Program
    {
        static void Main(string[] args)
        {
            string helloMessage = "~w~File location example";
            helloMessage = Energy.Core.Tilde.Strip(helloMessage);
            Energy.Core.Tilde.WriteLine(ConsoleColor.Green, "~w~File location example");
            Console.ReadLine();
        }
    }
}
