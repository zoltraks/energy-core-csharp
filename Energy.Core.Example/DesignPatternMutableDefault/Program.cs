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

        public class Y : X<Y>
        {
            public string Name;

            public override string ToString()
            {
                return Name;
            }
        }

        public class X<T>
        {
            private static T _Default;

            public static T Default
            {
                get
                {
                    if (_Default == null)
                        _Default = Activator.CreateInstance<T>();
                    return _Default;
                }
            }
        }
    }
}
