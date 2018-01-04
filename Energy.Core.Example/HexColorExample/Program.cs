using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HexColorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Drawing.Color c1 = System.Drawing.Color.FromName("Blue");
            Console.Write("c1.ToString() = ");
            Console.WriteLine(c1.ToString());

            object o1 = c1;
            System.Drawing.Color c3 = (System.Drawing.Color)o1;
            Console.Write("((System.Drawing.Color)(object)c1).ToString() = ");
            Console.WriteLine(c3.ToString());


            int n = c1.ToArgb();
            Console.Write("c1.ToArgb() = ");
            Console.WriteLine(n);
            uint n1 = (uint)n;
            Console.Write("(uint)c1.ToArgb() = ");
            Console.WriteLine(n1);
            Energy.Base.Color x = (Energy.Base.Color)(uint)c1.ToArgb();
            var html = x.ToString();
            Console.Write("(string)(Energy.Base.Color) = ");
            Console.WriteLine(html);
            Energy.Base.Color x1 = new Energy.Base.Color((uint)c1.ToArgb());
            var html1 = x1.ToString();
            Console.Write("new Energy.Base.Color((uint)c.ToArgb()) = ");
            Console.WriteLine(html1);

            Console.ReadLine();
        }
    }
}
