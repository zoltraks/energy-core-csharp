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
            string strip = Energy.Base.Path.StripQuotation(filename);
            Console.WriteLine(string.Format("Strip: {0}", strip));
            string dos = Energy.Base.Path.ToDos(filename);
            Console.WriteLine(string.Format("DOS path: {0}", dos));
            string unix = Energy.Base.Path.ToUnix(strip);
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

        public static void PathSplit(string path)
        {
            string[] a = Energy.Base.Path.Split(path);
            Console.WriteLine(string.Join(" + ", a));
        }

        public static void PathShorten(string path)
        {
            Energy.Core.Tilde.WriteLine("~c~Long path: ~y~{0}", path);
            Console.WriteLine("<<< " + Energy.Base.Path.ShortLeft(path, 50, "..."));
            Console.WriteLine(">>> " + Energy.Base.Path.ShortRight(path, 50, "..."));
            Console.WriteLine("--- " + Energy.Base.Path.ShortMiddle(path, 50, "..."));
            Energy.Core.Tilde.WriteLine("~b~= = = = =");
            Console.WriteLine("<<< " + Energy.Base.Path.ShortLeft(path, 30, "..."));
            Console.WriteLine(">>> " + Energy.Base.Path.ShortRight(path, 30, "..."));
            Console.WriteLine("--- " + Energy.Base.Path.ShortMiddle(path, 30, "..."));
            Energy.Core.Tilde.WriteLine("~b~= = = = =");
            Console.WriteLine("<<< " + Energy.Base.Path.ShortLeft(path, 10, "..."));
            Console.WriteLine(">>> " + Energy.Base.Path.ShortRight(path, 10, "..."));
            Console.WriteLine("--- " + Energy.Base.Path.ShortMiddle(path, 10, "..."));
        }
    }
}
