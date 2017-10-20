using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternMutableDefault
{
    class Program
    {
        static void Main(string[] args)
        {
            Y.Default.Name = "Name";
            Console.WriteLine(Y.Default.ToString());
            Console.ReadLine();
        }

        public class Y : Energy.Base.Pattern.DefaultProperty<Y>
        {
            public string Name;

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
