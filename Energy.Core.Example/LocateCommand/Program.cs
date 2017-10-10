using System;

namespace LocateCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            Test("notepad.exe");
            Console.ReadLine();
        }

        private static void Test(string v)
        {
            string locate = Energy.Base.File.Locate(v);
            Console.WriteLine(locate);
        }
    }
}
