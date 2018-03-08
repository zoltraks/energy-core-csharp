using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Energy.Base.Log;
using System.Linq;

namespace RestBinaryGetPut
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Energy.Core.Application.SetConsoleEncoding();
                Test.S();
                Test.Make();
            }
            catch (Exception x)
            {
                Energy.Core.Tilde.Exception(x, true);
            }
            finally
            {
                Console.ReadLine();
            }

            string currentDirectory = System.IO.Directory.GetCurrentDirectory();
            currentDirectory = Energy.Base.Path.ToUnix(currentDirectory);
            Console.WriteLine($"Current directory: {currentDirectory}");
            string randomText = Energy.Base.Random.GetRandomText(8);
            Console.WriteLine($"Random text: {randomText}");
            string makeDirectory = Energy.Base.Path.IncludeTrailingSeparator(System.IO.Path.Combine(currentDirectory, randomText));
            makeDirectory = Energy.Base.Path.ChangeSeparator(makeDirectory);
            Console.WriteLine($"Make directory: {makeDirectory}");
            Energy.Base.File.MakeDirectory(makeDirectory);
            byte[] array = new byte[25800];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(i % 256);
            }
            string filePath = System.IO.Path.Combine(makeDirectory, "file.data");
            System.IO.File.WriteAllBytes(filePath, array);

            Console.WriteLine(Energy.Base.Hex.Print(array, 16, 8));

            string url;
            byte[] responseData;
            string responseString;

            url = "http://localhost:6000/api/storage/xyz/";

            string[] responseHeaders;

            Energy.Core.Web.Head(url, out responseHeaders);

            for (int i = 0; i < responseHeaders.Length / 2; i += 2)
            {
                if (responseHeaders[i] == "Stamp")
                {
                    DateTime dt = Energy.Base.Cast.StringToDateTime(responseHeaders[i + 1]);
                    dt = dt.ToLocalTime();
                    Energy.Core.Tilde.WriteLine("~y~Timestamp: ~r~{0}"
                        , Energy.Base.Cast.DateTimeToString(dt));
                    break;
                }
            }

            Console.WriteLine(string.Join("\n", responseHeaders));
            Console.ReadLine();

            //responseData = Energy.Core.Web.Put(url, array);

            Energy.Core.Web.Post(url, array, out responseString);

            Console.WriteLine(responseString);

            url = "http://localhost:6000/api/storage/xyz/path/to/file.data";

            responseData = Energy.Core.Web.Put(url, array);

            Console.WriteLine(Energy.Base.Hex.Print(responseData, 16, 8));

            Energy.Core.Tilde.Pause();



            Energy.Base.File.RemoveDirectory(makeDirectory, true);
            Energy.Core.Tilde.Pause();
            //Energy.Base.Path.Current
            //string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Storage", "Data");

           // string baseUrl = "http://localhost/";
        }

        private static void TestPutExe()
        {
            byte[] data = File.ReadAllBytes(@"C:\DATA\example.exe");
            string url = "http://localhost:6000/api/storage/xyz/path/to/file.data";
            string json;
            int responseCode = Energy.Core.Web.Post(url, data, out json);
            Console.WriteLine(responseCode);
            Console.WriteLine(json);
            Console.ReadLine();
        }

        private static void TestLog()
        {
            Energy.Core.Log log = Energy.Core.Log.Default;
            Energy.Core.Bug.Trace.On();
            log.Write("1");
            log.Flush();
            log.Exception(new Exception("ERROR"));
            log.Write("2");
            Console.WriteLine(log.ToString());
            log.Destination += new Energy.Core.Log.Target.Console() { Color = false, Immediate = false };
            log.Write("3");
            log.Flush();
            log.Write("4");
            Console.ReadLine();
        }
    }
}
