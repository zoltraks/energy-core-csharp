using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSerializationExample
{
    public class Program
    {
        public static void Main()
        {
            Energy.Core.Program.SetLanguage();
            Console.WriteLine("Hello World");
            Item _ = new Item() { Name = "Example name", Count = 12345, };
            Console.WriteLine(Energy.Base.Xml.Serialize(_));

            Energy.Base.Collection.SerializableDictionary<string, int> x
                = new Energy.Base.Collection.SerializableDictionary<string, int>();
            x.Add("test", 12);
            x.Add("xyz", 321);

            string xml1 = Energy.Base.Xml.Serialize(x);
            Console.WriteLine(xml1);
            Energy.Base.Collection.SerializableDictionary<string, int> x2
                = Energy.Base.Xml.Deserialize<Energy.Base.Collection.SerializableDictionary<string, int>>(xml1);
            Console.WriteLine(x2.Count);
            Energy.Base.Collection.StringDictionary<Item> s = new Energy.Base.Collection.StringDictionary<Item>();
            s.Add("x", _);
            _.Name = "Example name 2";
            s.Add("y", _);
            Console.WriteLine(Energy.Base.Xml.Serialize(s));
            Console.WriteLine();

            Test test = new Test();

            test.SerializeBaseTable();

            test.SerializeDataTable();

            Console.ReadLine();
        }
    }
}
