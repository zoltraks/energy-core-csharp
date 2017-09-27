using System;

namespace WritePlatformVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("~yellow~Welcome to ~white~Energy.Core ~yellow~library and ~white~.NET platform ~yellow~version ~cyan~check~yellow~...");
            Console.WriteLine();
            Console.WriteLine("Platform: \t{0}", Environment.Version.ToString());
            Console.WriteLine("Library:  \t{0}", Energy.Core.Version.LibraryVersion);
            Console.WriteLine();
        }
    }
}
