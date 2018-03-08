using System;
using System.Collections.Generic;
using System.Text;

namespace ColorfulConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Example.StoryFragment();
            Console.ReadLine();
            Example.Reset();
            Example.TwoColor();
            Console.ReadLine();
            Example.Reset();
            Example.SeveralColors();
            Console.ReadLine();
            Example.Reset();
            Example.WithGradient();
            Console.ReadLine();
            Example.Reset();
            Console.ReadLine();
        }
    }
}
