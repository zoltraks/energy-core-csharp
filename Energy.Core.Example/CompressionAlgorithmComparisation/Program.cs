using System;
using System.Collections.Generic;
using System.Text;

namespace CompressionAlgorithmComparisation
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 4096;
            byte[] buffer = new byte[size];
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                //buffer[i] = (byte)random.Next(byte.MaxValue);
                buffer[i] = (byte)random.Next((byte)'A', (byte)'Z');
            }

            Energy.Core.Tilde.WriteLine("~y~Uncompressed ~w~size {0}", buffer.Length);
            Console.WriteLine(Energy.Base.Hex.Print((new Energy.Base.ByteArrayBuilder(buffer)).ToArray(0, 128)));

            byte[] deflateData = Energy.Base.Compression.Deflate.Compress(buffer);

            Energy.Core.Tilde.WriteLine("~y~Compressed using ~c~Deflate ~y~algorithm ~w~size {0}", deflateData.Length);
            Console.WriteLine(Energy.Base.Hex.Print((new Energy.Base.ByteArrayBuilder(deflateData)).ToArray(0, 128)));

            byte[] gzipData = Energy.Base.Compression.GZip.Compress(buffer);

            Energy.Core.Tilde.WriteLine("~y~Compressed using ~c~GZip ~y~algorithm ~w~size {0}", gzipData.Length);
            Console.WriteLine(Energy.Base.Hex.Print((new Energy.Base.ByteArrayBuilder(gzipData)).ToArray(0, 128)));

            byte[] deflateUncompressedData = Energy.Base.Compression.Deflate.Decompress(deflateData);

            Energy.Core.Tilde.WriteLine("~y~Decompressed using ~c~Deflate ~y~algorithm ~w~size {0}", deflateUncompressedData.Length);
            Console.WriteLine(Energy.Base.Hex.Print((new Energy.Base.ByteArrayBuilder(deflateUncompressedData)).ToArray(0, 128)));

            byte[] gzipUncompressedData = Energy.Base.Compression.GZip.Decompress(gzipData);

            Energy.Core.Tilde.WriteLine("~y~Decompressed using ~c~GZip ~y~algorithm ~w~size {0}", gzipUncompressedData.Length);
            Console.WriteLine(Energy.Base.Hex.Print((new Energy.Base.ByteArrayBuilder(gzipUncompressedData)).ToArray(0, 128)));

            Console.ReadLine();
        }
    }
}
