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
                Energy.Core.Tilde.WriteLine("{0}{1}", "~c~", " [ TEST 01 ] ~g~Path separator change");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"C:\""Program Files""\""Dir""");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"//var/log/messages.log");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"\\SHARE\\\Dir\\\File.txt");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSeparator(@"http://localhost:8123/path/file.json");
                Energy.Core.Tilde.Break(1, "~y~----------");

                Energy.Core.Tilde.WriteLine("{0}{1}", "~m~", " [ TEST 02 ] ~g~Path split");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSplit(@"C:\""Program Files""\""Dir""");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSplit(@"//var/log/messages.log");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSplit(@"\\SHARE\\\Dir\\\File.txt");
                Energy.Core.Tilde.Break(1, "~y~----------");
                Example.PathSplit(@"http://localhost:8123/path/file.json");
                Energy.Core.Tilde.Break(1, "~y~----------");


                Energy.Core.Tilde.WriteLine("{0}{1}", "~r~", " [ TEST 03 ] ~g~Path shorten");
                Energy.Core.Tilde.Break(1, "~w~----------");
                Example.PathShorten(@"C:\""Program Files""\""Microsoft""\Folder\Subfolder\AnotherOne\MyFile.txt");
                Energy.Core.Tilde.Break(1, "~w~----------");
                Example.PathShorten(@"/usr_wider_name/var/log/some/longer/than/usual/path/to/a/file_with_very_long_name.txt");
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
