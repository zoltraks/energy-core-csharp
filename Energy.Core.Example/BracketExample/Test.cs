using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BracketExample
{
    class Test
    {
        internal static void Angle()
        {
            string address = "8.8.8.8";
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Ping time to {0} = {1} ms", address, Energy.Core.Network.Ping(address));
            }
        }
    }
}
