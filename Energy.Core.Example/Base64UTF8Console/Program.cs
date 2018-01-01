using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base64UTF8Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string[] array = new string[] { "Hello", "password", "", "Zażółć gęślą jaźń" };
            foreach (string input in array)
            {
                string base64 = Energy.Base.Cast.StringToBase64(input);
                string text = Energy.Base.Cast.Base64ToString(base64);
                System.Console.WriteLine("Text: \"{0}\", Base64: \"{1}\", Text: \"{2}\"", input, base64, text);
            }
            Energy.Core.Tilde.Pause();
        }
    }
}
