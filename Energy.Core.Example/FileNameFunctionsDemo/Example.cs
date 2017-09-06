using System;
using System.Collections.Generic;
using System.Text;

namespace FileNameFunctionsDemo
{
    internal static class Example
    {
        public static void PathSeparator(string filename)
        {
            Console.WriteLine(string.Format("Filename: {0}", filename));
            string strip = Energy.Base.File.StripQuotation(filename);
            Console.WriteLine(string.Format("Strip: {0}", strip));
            string dos = Energy.Base.File.ToDosPath(filename);
            Console.WriteLine(string.Format("DOS path: {0}", dos));
            string unix = Energy.Base.File.ToUnixPath(filename);
            Console.WriteLine(string.Format("UNIX path: {0}", unix));
            string file = Energy.Base.File.GetName(filename);
            Console.WriteLine(string.Format("Filename: {0}", file));
            string root = Energy.Base.File.GetRoot(filename);
            Console.WriteLine(string.Format("Root: {0}", root));

            string ext = Energy.Base.File.GetExtension(filename);
            Console.WriteLine(string.Format("Extension: {0}", ext));
            string pure = Energy.Base.File.GetNameWithoutExtension(filename);
            Console.WriteLine(string.Format("Filename without extension: {0}", pure));

            string plain = Energy.Base.File.ExcludeRoot(filename);
            Console.WriteLine(string.Format("Plain: {0}", plain));
            //string p = Energy.Base.File.IncludeLeadingRoot()
        }
    }
}
