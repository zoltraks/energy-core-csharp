using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Energy.Core.Tilde.Pause();
        }

        private static void Test()
        {
            byte[] array = Energy.Base.Random.GetRandomByteArray(60, 41, 99);
            string print;
            print = Energy.Base.Hex.Print(array);
            Energy.Core.Tilde.WriteLine(print);
        }
    }
}
