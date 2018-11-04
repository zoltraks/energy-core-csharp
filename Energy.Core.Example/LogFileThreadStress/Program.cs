using System;
using System.Collections.Generic;
using System.Text;

namespace LogFileThreadStress
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Application app = new Energy.Core.Application(typeof(App));
            app.Run();
        }
    }
}
