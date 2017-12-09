using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeConvertion
{
    class Program
    {
        static void Main(string[] args)
        {
            //string pattern = @"\02[^\03\s]*\s*(\d+)k.*\03";
            string input = "\u0002CW100000101 0000000058k000000000            00100000133=5>\u0003";
            string pattern = @"(\d+)k\d+";
            MatchCollection matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
            {
                Match match = matches[matches.Count - 1];
                string value = match.Groups[1].Value;
                Console.WriteLine(value);
            }
            Console.ReadLine();
        }
    }
}
