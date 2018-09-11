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
            string s;
            int i;
            long l;
            double d;
            decimal n;


            Random r = new Random();
            int count;

            Energy.Base.Text.TryParse("123", out i);

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random numbers to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            count = 0;
            Loop(() => { int next = r.Next(); string text = next.ToString(); int.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~TryParse~0~ " + count);
            count = 0;
            Loop(() => { int next = r.Next(); string text = next.ToString(); Energy.Base.Text.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~Parse   ~0~ " + count);

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random strings to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            count = 0;
            Loop(() => { int next;  string text = new string((char)((byte)'A' + r.Next() % 26), r.Next(10)); int.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~TryParse~0~ " + count);
            count = 0;
            Loop(() => { int next; string text = new string((char)((byte)'A' + r.Next() % 26), r.Next(10)); Energy.Base.Text.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~Parse   ~0~ " + count);

            Energy.Core.Tilde.Break();

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random numbers to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            count = 0;
            Loop(() => { int next = r.Next(); string text = next.ToString(); int.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~TryParse~0~ " + count);
            count = 0;
            Loop(() => { int next = r.Next(); string text = next.ToString(); Energy.Base.Text.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~Parse   ~0~ " + count);

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random strings to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            count = 0;
            Loop(() => { int next; string text = new string((char)((byte)'A' + r.Next() % 26), r.Next(10)); int.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~TryParse~0~ " + count);
            count = 0;
            Loop(() => { int next; string text = new string((char)((byte)'A' + r.Next() % 26), r.Next(10)); Energy.Base.Text.TryParse(text, out next); count++; }, TimeSpan.FromSeconds(1.0));
            Energy.Core.Tilde.WriteLine("Done ~w~Parse   ~0~ " + count);
        }

        private static void Loop(Energy.Base.Anonymous.Function function, TimeSpan timeSpan)
        {
            ManualResetEvent reset = new ManualResetEvent(false);
            Thread thread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        function();
                    }
                }
                catch (ThreadAbortException)
                { }
            });
            thread.Start();
            reset.WaitOne(timeSpan);
            thread.Abort();
        }
    }
}
