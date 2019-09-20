using System;

namespace WorkerSimpleControl
{
    class Program
    {
        static void Main(string[] args)
        {
            var w1 = new Worker.Service1();
            Console.WriteLine("Starting worker");
            w1.Start();
            Console.WriteLine("Enter anything to stop");
            Console.ReadLine();
            w1.Stop();
            Console.WriteLine("Enter anything to exit");
            Console.ReadLine();
            w1.Stop();
        }
    }
}
