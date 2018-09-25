using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerPersistentBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!Setup(args))
                {
                    return;
                }
                Go(args);
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

        private static bool Setup(string[] args)
        {
            throw new NotImplementedException();
        }

        private static void Go(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
