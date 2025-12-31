using System;
using System.Diagnostics;
using System.Reflection;

namespace PrintArgs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Energy.Base.Hello.World());
            //var command = Energy.Core.Program.GetCommandName(Assembly.GetExecutingAssembly());
            var command = Energy.Core.Program.GetCommandName();
            Console.WriteLine($"Command: {command}");
            if (Debugger.IsAttached)
                Console.ReadLine();
        }
    }
}
