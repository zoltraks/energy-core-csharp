using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParameterList
{
    public class Test
    {
        public static void NullNumberExample()
        {
            Energy.Query.Parameter.List bag = new Energy.Query.Parameter.List();
            bag["@x"] = "1";
            bag["x"] = "2";
            bag["y"] = null;
            bag.SetType("y", Energy.Enumeration.FormatType.Number);
            Console.WriteLine(bag.ToString(": "));
            Console.WriteLine();
            string query = "PRINT @y , @x";
            Console.WriteLine(bag.Parse(query));
            bag.Option |= Energy.Query.Parameter.Option.NullAsZero;
            Console.WriteLine(bag.Parse(query));
        }
    }
}
