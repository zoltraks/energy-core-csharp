using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemorySizeInKB
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (long x in new long[]
            {
                401511551,
                12341401511551,
                0,
                1,
                1023,
                1024,
                2047,
            })
            {
                Console.WriteLine(x);
                Console.Write(Energy.Base.Cast.MemorySizeToString(x, 1, true));
                Console.SetCursorPosition(20, Console.CursorTop);
                Console.Write(Energy.Base.Cast.MemorySizeToString(x, 2, true));
                Console.SetCursorPosition(40, Console.CursorTop);
                Console.Write(Energy.Base.Cast.MemorySizeToString(x, 2, true));
                Console.WriteLine();
                Console.Write(Energy.Base.Cast.MemorySizeToString(x, 1, false));
                Console.SetCursorPosition(20, Console.CursorTop);
                Console.Write(Energy.Base.Cast.MemorySizeToString(x, 2, false));
                Console.SetCursorPosition(40, Console.CursorTop);
                Console.Write(Energy.Base.Cast.MemorySizeToString(x, 2, false));
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
