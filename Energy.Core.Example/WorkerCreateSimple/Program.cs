using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WorkerCreateSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            //Energy.Core.Worker<MyState> worker = Energy.Core.Worker<MyState>.Create();
            //worker.Start(Run);

            MyThread m1 = new MyThread();
            Thread t1 = new Thread(m1.Run);
            t1.Start("");

            Console.WriteLine("Waiting");

            if (!Energy.Core.Worker.WaitForExit(t1, 1000))
            {
                Console.WriteLine("Aborting");
                Thread.Sleep(1000);                
                t1.Abort();
            }
            Console.ReadLine();
        }

        public class MyState
        {
            //public bool Modify;
        }

        public class MyThread
        {
            public void Run(object p)
            {
                try
                {
                    while (true)
                    {
                        Console.Write(".");
                        System.Threading.Thread.Sleep(250);
                    }
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Aborted");
                }
            }
        }
    }
}
