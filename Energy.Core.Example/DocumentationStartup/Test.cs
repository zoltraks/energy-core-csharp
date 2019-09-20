using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationStartup
{
    class Test
    {
        internal static void Test1()
        {
            string s1 = " 0,123515 ";
            string s2 = "\t0.3456\n";
            string s3 = s1 + s2;

            Console.WriteLine(Energy.Base.Cast.StringToDouble(s1));
            Console.WriteLine(Energy.Base.Cast.StringToDouble(s2));
            Console.WriteLine(Energy.Base.Cast.StringToDouble(s3));
        }

        internal static void Test7()
        {
            Energy.Base.Class.Information c = Energy.Base.Class.Information.Create(typeof(Customer));
            foreach (Energy.Base.Class.Information.Field f in c)
            {
                Energy.Core.Syntax syntax = new Energy.Core.Syntax();
                
                //System.Console.Write($"Name: {f.Name} ");
                //System.Console.Write($"Type: {f.Type} ");
                System.Console.WriteLine();
            }
            System.Console.WriteLine(c);
        }

        internal static void Test9()
        {
            string l1 = "1,2,3";
            string l2 = "abc def xyz";
            string l3 = "A(12) B(13)";
            string[] a1 = Energy.Base.Text.SplitArray(l1, ",;", "\"");
            Console.WriteLine("a1[] = " + string.Join(", ", a1));
            string[] a2 = Energy.Base.Text.SplitArray(l2, " ", "\"");
            Console.WriteLine("a2[] = " + string.Join(", ", a2));
            string[] a3 = Energy.Base.Text.SplitArray(l3, " ", "\"");
            Console.WriteLine("a3[] = " + string.Join(", ", a3));
        }

        internal static void Test6()
        {
            string s = "Hello";
            Console.WriteLine(Energy.Base.Hash.CRC(s));
            Console.WriteLine(Energy.Base.Hash.CRC2(s));
            Console.WriteLine(Energy.Base.Hash.PJW(s));
            Console.WriteLine(Energy.Base.Hash.MD5(s));
            Console.WriteLine(Energy.Base.Hash.SHA1(s));
        }

        internal static void Test5()
        {
            //string city = "52°30′26″N 13°8′45″E";
            //Energy.Base.Geo.Point point = Energy.Base.Geo.Point.Create(city);
            Energy.Base.Geo.Point point = "52.507320, 13.145812";
            Console.WriteLine(Energy.Base.Cast.DoubleToString(point.Latitude, 6) + " " 
                + Energy.Base.Cast.DoubleToString(point.Longitude, 6));
            Console.WriteLine(point.ToDMS());
        }

        internal static void Test2()
        {
            string s = " 0,123515 ";
            double d = Energy.Base.Cast.StringToDouble(s);
            Console.WriteLine(Energy.Base.Cast.DoubleToString(d));
        }

        internal static void Test3()
        {
            string s = " 1'133'244 ";
            double d = Energy.Base.Cast.StringToDoubleSmart(s);
            Console.WriteLine(Energy.Base.Cast.DoubleToString(d));
        }

        public enum Articles
        {
            Books,
            Newspapers,
            Cups,
        }

        internal static void Test4()
        {
            string s1 = "Cups";
            Console.WriteLine(Energy.Base.Cast.StringToEnum(s1, typeof(Articles)));
            string s2 = " booKS ";
            Console.WriteLine(Energy.Base.Cast.StringToEnum(s2, typeof(Articles)));
        }

        public static void Test8(string[] args)
        {
            Energy.Core.Shell.ArgumentList _ = Energy.Core.Shell.ArgumentList.Create(args);
            CommandLineOptions opt = Energy.Core.Configuration.Create(args, typeof(CommandLineOptions), new Energy.Core.Shell.OptionStyle()
            {
                Slash = true
            }) as CommandLineOptions;
            Console.WriteLine(Energy.Base.Xml.Serialize(opt));
        }

        internal static void TestForRegularExpressionEscape(string p)
        {
            string x = "My expression of ^[\\s\\t\\v\\ ]*";
            Console.WriteLine(x);
            x = Energy.Base.Text.EscapeExpression(x);
            Console.WriteLine(x);
        }

        internal static void Test10()
        {
            string p = "%Percentage: 30%%.% %%%% Or other variable like %ProgramFiles%... ";
            Energy.Base.Bracket b = new Energy.Base.Bracket();
            b.Enclosure = @"%";
            string[] a = b.FindArray(p);
            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine(a[i]);
            }
        }
        
        internal static void Test11()
        {
            string p = "Lorem ipsum [[[special]]]... [[[ quote ]]]]]] ]]] [[[bad formed [[[]]] ]]] ";
            Console.WriteLine();
            Console.WriteLine(p);
            Energy.Base.Bracket b = new Energy.Base.Bracket(@"[[[]]]");
            
            b.Include = "";
            foreach (string v in b.Find(p))
                Console.WriteLine(v);

            Console.WriteLine();
            Console.WriteLine(p);
            
            b.Include = null;
            foreach (string v in b.Find(p))
                Console.WriteLine(v);
        }

        internal static void Test12()
        {
            string p = "DECLARE @name VARCHAR = '@@name'";

            Energy.Query.Parameter.Bag bag = new Energy.Query.Parameter.Bag();
            bag.Add("@@name", 12345678);
            bag.Type["@@name"] = Energy.Enumeration.FormatType.Number;
            bag.Set("name", "my name", Energy.Enumeration.FormatType.Text);
            string text = bag.Parse(p);
            Console.WriteLine(text);
        }
    }

    [Energy.Attribute.Command.Welcome("Hello")]
    public class CommandLineOptions
    {
        [Energy.Attribute.Command.Option(Name = "help", Alternatives = new string[] { "?" })]
        public bool Help;

        [Energy.Attribute.Command.Option(Name = "?")]
        public bool Question;

        [Energy.Attribute.Command.Argument]
        public string File;

        //[Energy.Attribute.Command.Count(1)]
        //[Energy.Attribute.Command.Description(Text = "Array of ")]
        [Energy.Attribute.Command.Option(Name = "array", Count = 3)]
        [Energy.Attribute.Command.Option(Name = "array2", Count = 2)]
        public string[] Array;

        [Energy.Attribute.Command.Option(Name = "array", Count = 2)]
        public string[] Array2;
    }

    public class Customer
    {
        public string Name;

        public bool Active;
    }
}
