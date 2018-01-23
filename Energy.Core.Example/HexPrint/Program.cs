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
            Console.WriteLine(Energy.Core.Web.Get("https://www.google.com/search?q=q=Energy"));
            //Console.WriteLine(Energy.Base.Hex.Print(Energy.Base.Random.GetRandomByteArray(40)));
            
            Test(new byte[1] { 77 });
            byte[] array21 = new byte[21];
            for (int i = 0; i < array21.Length; i++)
            {
                array21[i] = (byte)(i + 40);
            }
            Test(array21);
            Test(Energy.Base.Random.GetRandomByteArray(60, 41, 99));
            Energy.Core.Tilde.Pause();
        }

        private static void Test(byte[] array)
        {
            string print;

            Energy.Base.Hex.PrintFormatSettings fmt = new Energy.Base.Hex.PrintFormatSettings();

            Energy.Core.Tilde.WriteLine("Standard HEX format settings");
            print = Energy.Base.Hex.Print(array, fmt);
            Energy.Core.Tilde.WriteLine(print);
            Energy.Core.Tilde.WriteLine("Using 16-bit words");
            fmt.WordSize = 2;        
            print = Energy.Base.Hex.Print(array, fmt);
            Energy.Core.Tilde.WriteLine(print);
            Energy.Core.Tilde.WriteLine("Group every 5 bytes");
            fmt.GroupSize = 5;
            print = Energy.Base.Hex.Print(array, fmt);
            Energy.Core.Tilde.WriteLine(print);

            fmt = new Energy.Base.Hex.PrintFormatSettings();
            fmt.OffsetSize = 2;
            fmt.OffsetSeparator = "~w~ : ~c~";
            fmt.ElementSeparator = "~ds~.~c~";
            fmt.GroupSeparator = " ~g~| ~c~";
            fmt.RepresentationSeparator = " ~s~... ~m~";
            fmt.WordSize = 1;
            Energy.Core.Tilde.WriteLine("Color version");
            fmt.GroupSize = 4;
            print = Energy.Base.Hex.Print(array, fmt);
            Energy.Core.Tilde.WriteLine(print);
        }
    }
}
