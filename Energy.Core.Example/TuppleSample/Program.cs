using System;

namespace TuppleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            (string country, string capital, double gdpPerCapita) = ("Malawi", "Lilongwe", 226.50);

            Console.WriteLine($"Country: {country} Capital: {capital} GDP: {gdpPerCapita}");

            (string a, _, string b) = GetAbc();

            Console.WriteLine($"A: {a} B: {b}");

            Console.ReadLine();
        }

        public static Tuple<string, string, string> GetAbc()
        {
            return new Tuple<string, string, string>("a", "b", "c");
        }
    }
}
