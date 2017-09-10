using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteArrayBenchmark
{
    class Example
    {
        internal static void Test1()
        {
            Energy.Core.Tilde.SetNoColor(true);
            Energy.Core.Tilde.WriteLine(@"This test will generate byte buffer filled with random values and calculate CRC sum of it by iterating over all elements");

            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            StringBuilder s = new StringBuilder();
            s[10] = '1';
            //b.ChunkSize = 10;
        }
    }
}
