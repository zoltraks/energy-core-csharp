using System;
using System.Collections.Generic;
using System.Text;

namespace WritePlatformVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Version: {0}", Environment.Version.ToString());
            Console.ReadLine();
        }
    }
}
