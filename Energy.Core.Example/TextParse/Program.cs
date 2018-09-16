using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TextParse
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Go(args);
            }
            catch (Exception x)
            {
                if (!string.IsNullOrEmpty(x.Message))
                {
                    Console.WriteLine("Exception: {0}", x.Message);
                }
            }
            Console.ReadLine();
        }

        private static void Go(string[] args)
        {
            Random r = new Random();
            int count;

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random numbers to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            count = Energy.Core.Benchmark.Loop(() => { int next = r.Next(); string text = next.ToString(); int.TryParse(text, out next); }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~m~default ~w~TryParse~0~ " + count);
            count = Energy.Core.Benchmark.Loop(() => { int next = r.Next(); string text = next.ToString(); Energy.Base.Text.TryParse(text, out next); }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~c~Energy  ~w~TryParse~0~ " + count);

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random strings to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            count = Energy.Core.Benchmark.Loop(() => { int next;  string text = new string((char)((byte)'A' + r.Next() % 26), r.Next(10)); int.TryParse(text, out next); }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~m~default ~w~TryParse~0~ " + count);
            count = Energy.Core.Benchmark.Loop(() => { int next; string text = new string((char)((byte)'A' + r.Next() % 26), r.Next(10)); Energy.Base.Text.TryParse(text, out next); }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~c~Energy  ~w~TryParse~0~ " + count);

            Energy.Core.Benchmark.Result benchmark;
            benchmark = Energy.Core.Benchmark.Profile(() =>
            {
                long m = 0;
                for (int i = 0, n = 0; i < 100000; i++)
                {
                    string s = i.ToString();
                    if (Energy.Base.Text.TryParse(s, out n))
                    {
                        m += n;
                    }
                }
            }, 3, 1, "SumTryParse");
            Energy.Core.Tilde.WriteLine(benchmark.ToString());

            Energy.Core.Tilde.Break();
        }
    }
}
