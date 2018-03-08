using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ReadLineLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("~yellow~Welcome to ~blue~ReadLine ~yellow~example~white~!");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Energy.Core.Tilde.Write("Write ~c~something~0~: ~magenta~");
            while (true)
            {
                string input = Energy.Core.Tilde.ReadLine();
                if (input == null)
                {
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    Energy.Core.Tilde.WriteLine("You have written ~green~{0}~0~...", input);
                    break;
                }
            }
            Energy.Core.Tilde.Pause();
        }
    }
}
