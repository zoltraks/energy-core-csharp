using System;
using System.Collections.Generic;
using System.Text;

namespace FileNameFunctionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Energy.Core.Tilde.WriteLine("{0}{1}", "~c~", "TEST 01");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"C:\""Program Files""\""Dir""");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"//var/log/messages.log");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"\\SHARE\\\Dir\\\File.txt");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"http://localhost:8123/path/file.json");
                Energy.Core.Tilde.Break(1, "~y~----------");
            }
            catch (Exception x)
            {
                if (!string.IsNullOrEmpty(x.Message))
                {
                    Console.WriteLine("Exception: {0}", x.Message);
                }
            }
            Console.ReadLine();
        }
    }
}
