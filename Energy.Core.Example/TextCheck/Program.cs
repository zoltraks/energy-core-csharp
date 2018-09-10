using System;

namespace TextCheck
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

            Energy.Core.Tilde.WriteLine("~g~Trying to parse random numbers to integer for one second using int.TryParse and Energy.Base.Text.Parse");

            Random r = new Random();
            Loop(() => { int n = r.Next(); string s = n.ToString(); int.TryParse(s, out n); }, TimeSpan.FromSeconds(1.0));
        }

        private static void Loop(Func<object> p, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
