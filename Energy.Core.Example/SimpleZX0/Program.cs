using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleZX0
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var argv = new Energy.Base.Command.Arguments(args)
                    .Switch("decompress")
                    .Switch("force")
                    .Alias("d", "decompress")
                    .Alias("f", "force")
                    .Parse();

                if (argv.Count < 1)
                {
                    Console.WriteLine("No input file specified");
                    return 1;
                }

                var inputFile = argv[0];
                var outputFile = argv[1];

                if (string.IsNullOrEmpty(outputFile))
                {
                    if (argv["decompress"].IsFalse)
                    {
                        outputFile = argv[0] + ".zx0";
                    }
                    else
                    {
                        if (argv[0].EndsWith(".zx0", StringComparison.InvariantCultureIgnoreCase))
                        {
                            outputFile = argv[0].Substring(0, argv[0].Length - 4);
                        }
                        else
                        {
                            outputFile = argv[0] + ".raw";
                        }
                    }
                }

                if (File.Exists(outputFile))
                {
                    if (argv["force"].IsFalse)
                    {
                        Console.WriteLine("Error: Output file exists");
                        return 1;
                    }
                    Console.WriteLine("Output file exists");
                }

                var inputData = File.ReadAllBytes(inputFile);
                byte[] outputData;

                if (argv["decompress"].IsTrue)
                {
                    outputData = Energy.Base.Compression.ZX0.Decompress(inputData);
                }
                else
                {
                    outputData = Energy.Base.Compression.ZX0.Compress(inputData);
                }

                File.WriteAllBytes(outputFile, outputData);

                return 0;
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                return 1;
            }
            finally
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
