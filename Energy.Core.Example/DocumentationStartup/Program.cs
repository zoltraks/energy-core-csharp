using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationStartup
{
    class Program
    {
        static bool Test17 =false;

        static void Main(string[] args)
        {
            if (Test17)
            {
                Test.Test1();
                Test.Test2();
                Test.Test3();
                Test.Test4();
                Test.Test5();
                Test.Test6();
                Test.Test7();
            }
            //Test.Test8(args);
            Test.Test9();
            Test.Test10();
            Console.ReadLine();
        }
    }
}
