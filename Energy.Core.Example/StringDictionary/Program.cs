using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StringDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("This example demonstrates usage of ~w~Energy~0~.~w~Base~0~.~w~Collection~0~.~y~StringDictionary~0~.");

            PrintMemoryUsage();

            TestB1();
            TestB2();
            TestB3();

            string[] dictionary = new string[] { "key1", "value1", "key2", "value2" };
            var x = new Energy.Base.Collection.StringDictionary<string>(dictionary);
            Console.WriteLine(x.ToString("="));

            Energy.Core.Benchmark.Result result;

            result = Energy.Core.Benchmark.Profile(TestA1, 5);
            Console.WriteLine(result.ToString(false, "0.0000000"));

            PrintMemoryUsage();

            result = Energy.Core.Benchmark.Profile(TestA2, 5);
            Console.WriteLine(result.ToString(false, "0.0000000"));

            PrintMemoryUsage();

            result = Energy.Core.Benchmark.Profile(TestA1, 5);
            Console.WriteLine(result.ToString(false, "0.0000000"));

            PrintMemoryUsage();

            result = Energy.Core.Benchmark.Profile(TestA2, 5);
            Console.WriteLine(result.ToString(false, "0.0000000"));

            PrintMemoryUsage();

            Console.ReadLine();   
        }

        private static void PrintMemoryUsage()
        {
            Console.WriteLine("Memory usage: {0:### ### ###} ", Energy.Core.Memory.GetCurrentMemoryUsage());
        }

        private static void TestA1()
        {
            Random random = new Random();
            Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();
            d.CaseSensitive = false;
            for (int i = 0; i < 100000; i++)
            {
                string key = random.Next(10000).ToString();
                string value = random.Next(10000).ToString();
                d.Set(key, value);
            }
        }

        private static void TestA2()
        {
            Random random = new Random();
            Dictionary<string, string> d = new Dictionary<string, string>();
            for (int i = 0; i < 100000; i++)
            {
                string key = random.Next(10000).ToString();
                string value = random.Next(10000).ToString();
                d[key] = value;
            }
        }

        private static void TestB1()
        {
            Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();
            d.CaseSensitive = true;
            Console.WriteLine("Setting \"Option\" to \"Value\"");
            d.Set("Option", "Value");
            string value;
            value = d.Get("option");
            Console.WriteLine("Value of \"option\" when CaseSensitive is true: {0}", value);
            d.CaseSensitive = false;
            value = d.Get("option");
            Console.WriteLine("Value of \"option\" when CaseSensitive is false: {0}", value);
        }

        private static void TestB2()
        {
            Energy.Base.Collection.StringDictionary d = new Energy.Base.Collection.StringDictionary();
            d.CaseSensitive = true;
            Console.WriteLine("Setting \"Option\", \"option\" and \"OPTION\" to different values");
            d["Option"] = "Value1";
            d["option"] = "Value2";
            d["OPTION"] = "Value3";
            string value;
            value = d.Get("option");
            Console.WriteLine("Value of \"option\" when CaseSensitive is true: {0}", value);
            d.CaseSensitive = false;
            value = d.Get("option");
            Console.WriteLine("Value of \"option\" when CaseSensitive is false: {0}", value);
            d.CaseSensitive = true;
            Energy.Core.Tilde.WriteLine("~y~Array when CaseSensitive is true");
            Console.WriteLine(string.Join(", ", d.ToArray("=")));
            d.CaseSensitive = false;
            Energy.Core.Tilde.WriteLine("~y~Array when CaseSensitive is false");
            Console.WriteLine(string.Join(", ", d.ToArray("=")));
        }

        private static void TestB3()
        {
            var dictionary = new Energy.Base.Collection.StringDictionary<object>();

            dictionary["One"] = 1;
            dictionary["TWO"] = 2;
            dictionary["ONE"] = 1 + (int)(dictionary["two"] ?? 1);

            Console.WriteLine(string.Concat("", "ONE=", dictionary["ONE"], " one=", dictionary["one"]));

            dictionary.CaseSensitive = false;

            Console.WriteLine(string.Concat("", "ONE=", dictionary["ONE"], " one=", dictionary["one"]));

            string xml = Energy.Base.Xml.Serialize(dictionary, "Dictionary");

            Console.WriteLine(xml);
        }
    }
}
