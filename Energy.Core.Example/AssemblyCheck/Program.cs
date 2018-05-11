using System;

namespace AssemblyCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (Energy.Core.Benchmark.Time(true))
            {
                Test.GetAllAssemblies();
            }

            Console.ReadLine();
        }
    }
}
