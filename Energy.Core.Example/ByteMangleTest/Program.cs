using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteMangleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Energy.Core {0}", Energy.Core.Version.LibraryVersion);

            Console.WriteLine();

            byte e = 0x89;
            Console.WriteLine("Value:      {0}", Energy.Base.Hex.ByteToHex(e));
            Console.WriteLine("High:       {0}", Energy.Base.Nibble.High(e));
            Console.WriteLine("Low:        {0}", Energy.Base.Nibble.Low(e));
            Console.WriteLine("Reverse:    {0}", Energy.Base.Hex.ByteToHex(Energy.Base.Nibble.Reverse(e)));

            byte eb = Energy.Base.Binary.Bcd.ToByte(e);
            Console.WriteLine("BCD as HEX: {0}", Energy.Base.Hex.ByteToHex(eb));
            Console.WriteLine("Decimal:    {0}", eb);

            Console.WriteLine();

            byte g = 0x58;
            Console.WriteLine("Value:      {0}", Energy.Base.Hex.ByteToHex(g));
            Console.WriteLine("Decimal:    {0}", g);
            Console.WriteLine("BCD:        {0}", Energy.Base.Hex.ByteToHex(Energy.Base.Binary.Bcd.FromByte(g)));

            Console.WriteLine();

            ushort w = 0x1234;
            Console.WriteLine("Word:       {0}", w);
            Console.WriteLine("BCD:        {0}", Energy.Base.Binary.Bcd.ToWord(w));

            Console.ReadLine();
        }
    }
}
