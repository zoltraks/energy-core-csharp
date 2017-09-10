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
            Energy.Core.Tilde.SetColorless(true);
            string description = @"

~w~This ~y~test ~w~will generate byte buffer filled with random values and calculate CRC
sum of it by iterating over all elements.


                ";

            Energy.Core.Tilde.WriteLine(Energy.Base.Text.RemoveEmptyLines(description).Trim());

            Example1();

            Example2();

            Console.WriteLine("Press any key to start benchmark");
            Console.ReadLine();

            int size = 10000000;
            Console.WriteLine("GC.Collect()");
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());

            Energy.Core.Benchmark.Result benchmark;

            Console.WriteLine();
            Console.WriteLine("Custom ByteArrayBuilder1 (MemoryStream, copy before calculation)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_CustomByteArrayBuilder1_A(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Energy.Base.ByteArrayBuilder (MemoryStream)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_EnergyByteArrayBuilder_1(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Energy.Base.ByteArrayBuilder (MemoryStream, copy before calculation)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_EnergyByteArrayBuilder_1(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);


            Console.WriteLine();
            Console.WriteLine("Energy.Base.ByteArrayBuilder (MemoryStream)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_EnergyByteArrayBuilder_1(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Custom ByteArrayBuilder1 (MemoryStream, accessing buffer)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_CustomByteArrayBuilder1_B(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Custom ByteArrayBuilder2 (List of byte arrays, accessing buffer)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_CustomByteArrayBuilder2_3(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Energy.Base.ByteArrayBuilder (MemoryStream)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_EnergyByteArrayBuilder_1(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Custom ByteArrayBuilder1 (MemoryStream, accessing buffer)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_CustomByteArrayBuilder1_B(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine();
            Console.WriteLine("Energy.Base.ByteArrayBuilder (MemoryStream)");
            benchmark = Energy.Core.Benchmark.Profile(() => { Func_EnergyByteArrayBuilder_1(size); }, 5, 1);
            Console.WriteLine(benchmark.ToString());
            Console.WriteLine(Energy.Core.Memory.GetCurrentMemoryUsage());
            GC.Collect();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            //b.ChunkSize = 10;
        }

        private static void Example2()
        {
            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            b.Append((byte)1);
            b.Append((byte)2);
            b.Append((int)1);
            b.Append((int)2);
            b.Append("Zurück");
            b.Encoding = System.Text.Encoding.Unicode;
            b.Append("Zurück");
            b.Append(Guid.NewGuid());
            b.Rewind();
            Console.WriteLine(Energy.Base.Hex.Print(b.ToArray()));
            b.Rewind();
            b.Skip(2 + 8);
            string s1 = b.ReadString(System.Text.Encoding.UTF8);
            string s2 = b.ReadString(System.Text.Encoding.Unicode);
            Console.WriteLine("{0} {1}", s1, s2);
            Guid guid = b.ReadGuid();
            Console.WriteLine(Energy.Base.Hex.Print(guid.ToByteArray()));
        }

        private static void Example1()
        {
            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            int m = 5;
            for (int i = 0; i < 5; i++)
            {
                b.Capacity = m;
                Console.WriteLine("Setting capacity for minimum of " + m + " bytes, final capacity set to " + b.Capacity);
                m = 10 * m;
            }
        }

        private static void Func_EnergyByteArrayBuilder_1(int max)
        {
            Random random = new Random();
            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            for (int i = 0; i < max; i++)
            {
                b.Append((byte)(random.Next(256)));
            }
            byte crc = 0;
            for (int i = 0; i < max; i++)
            {
                crc = (byte)((b[i] + crc) % 256);
            }
            Console.WriteLine("CRC of " + max + " elements = " + crc);
        }

        private static void Func_CustomByteArrayBuilder1_A(int max)
        {
            Random random = new Random();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ByteArrayBuilder1 b = new ByteArrayBuilder1();
            for (int i = 0; i < max; i++)
            {
                b.Append((byte)(random.Next(256)));
            }
            byte[] d = b.ToArray();
            byte crc = 0;
            for (int i = 0; i < max; i++)
            {
                crc = (byte)((d[i] + crc) % 256);
            }
            Console.WriteLine("CRC of " + max + " elements = " + crc);
        }

        private static void Func_CustomByteArrayBuilder1_B(int max)
        {
            Random random = new Random();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ByteArrayBuilder1 b = new ByteArrayBuilder1();
            for (int i = 0; i < max; i++)
            {
                b.Append((byte)(random.Next(256)));
            }
            byte[] d = b.GetBuffer();
            byte crc = 0;
            for (int i = 0; i < max; i++)
            {
                crc = (byte)((d[i] + crc) % 256);
            }
            Console.WriteLine("CRC of " + max + " elements = " + crc);
        }

        private static void Func_CustomByteArrayBuilder2_3(int max)
        {
            Random random = new Random();
            ByteArrayBuilder2 b = new ByteArrayBuilder2();
            for (int i = 0; i < max; i++)
            {
                b.Append((byte)(random.Next(256)));
            }
            byte crc = 0;
            for (int i = 0; i < max; i++)
            {
                crc = (byte)((b[i] + crc) % 256);
            }
            Console.WriteLine("CRC of " + max + " elements = " + crc);
        }


        private static void Func_EnergyByteArrayBuilder_2(int max)
        {
            Random random = new Random();
            Energy.Base.ByteArrayBuilder b = new Energy.Base.ByteArrayBuilder();
            for (int i = 0; i < max; i++)
            {
                b.Append((byte)(random.Next(256)));
            }
            byte crc = 0;
            byte[] d = b.ToArray();
            for (int i = 0; i < max; i++)
            {
                crc = (byte)((d[i] + crc) % 256);
            }
            Console.WriteLine("CRC of " + max + " elements = " + crc);
        }
    }
}
