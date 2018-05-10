using System;

namespace AssemblyCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Test.GetAllAssemblies();

            Console.ReadLine();
        }
    }
}
