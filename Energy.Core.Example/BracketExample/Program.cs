using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BracketExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Welcome();
            Test.Angle();
            Test.Dollar();
            Test.Percentage();
            Energy.Core.Tilde.Pause();
        }

        private static void Welcome()
        {
            Energy.Core.Tilde.WriteLine("~c~Welcome to bracket parsing example.");
            Energy.Core.Tilde.WriteLine("~m~Using ~y~Energy~g~.~y~Base~g~.~w~Bracket ~m~class.");
            Energy.Core.Tilde.Break();
        }
    }
}
