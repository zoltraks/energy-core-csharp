using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerCreateSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Worker<MyState> worker = Energy.Core.Worker<MyState>.Create();
            worker.Start();
        }

        public class MyState
        {
            public bool Modify;
        }
    }
}
