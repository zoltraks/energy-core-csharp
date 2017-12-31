using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Energy.Base;

namespace CounterOverflow
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Base.Counter counter;

            Console.WriteLine("Using counter of 1");
            counter = new Energy.Base.Counter(1);
            WriteState(counter);
            counter.Decrement();
            WriteState(counter);
            counter.Reset(counter.Value);
            counter.Minimum = 0;
            WriteState(counter);
            counter.Decrement();
            WriteState(counter);
            counter.Minimum = -1;
            counter.Reset(0);
            WriteState(counter);
            counter.Decrement();
            WriteState(counter);
            counter.Decrement();
            WriteState(counter);

            Console.WriteLine("Using counter 1:32768");
            counter = new Energy.Base.Counter(1, 32768);
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            counter.Value = 32767;
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            Console.WriteLine("Clearing overflow state");
            counter.Overflow = false;
            WriteState(counter);

            Console.WriteLine("Using counter -1:1 with 0 as default value");
            counter.Minimum = -1;
            counter.Maximum = 1;
            counter.Reset(0);
            WriteState(counter);

            Console.WriteLine("Decrement x 2");
            counter.Decrement();
            WriteState(counter);
            counter.Decrement();
            WriteState(counter);

            counter.Overflow = false;
            Console.WriteLine("Increment x 5");
            counter.Increment();
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            counter.Increment();
            WriteState(counter);
            counter.Increment();
            WriteState(counter);

            Energy.Core.Tilde.Pause();
        }

        private static void WriteState(Counter counter)
        {
            Energy.Core.Tilde.WriteLine("Counter value: ~y~{0} ~r~{1}", counter
                , Energy.Base.Cast.BoolToString(counter.Overflow, Energy.Enumeration.BooleanStyle.X)
                );
        }
    }
}
