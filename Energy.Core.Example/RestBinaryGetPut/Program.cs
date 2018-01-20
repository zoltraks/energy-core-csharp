﻿using System;
using System.Collections.Generic;
using System.Text;
using static Energy.Base.Log;

namespace RestBinaryGetPut
{
    class Program
    {
        public class X
        {
            public string Name;
        }

        public class Y : Energy.Base.Collection.Array<X> { }

        static void Main(string[] args)
        {
            Energy.Core.Log log = Energy.Core.Log.Default;
            log.Write("1");
            Energy.Base.Collection.Array<X> x1 = new Energy.Base.Collection.Array<X>();
            x1 += new X() { Name = "First" };
            Y y = new Y();
            y += new X() { Name = "First" };
            try
            {
                Energy.Core.Web.IgnoreCertificateValidation = true;
                Console.WriteLine(Energy.Core.Web.Get("https://atom.alyx.pl/api/test/echo/XYZ"));
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
            byte[] array = new byte[258];
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
    }
}
