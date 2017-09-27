using System;

namespace ClassLibraryStandard
{
    public class Class1
    {
        public string TestParameterBag()
        {
            Energy.Query.Parameter.Bag bag = new Energy.Query.Parameter.Bag();
            bag.Add("name", "My name");
            string query = "INSERT INTO Table1 ( Name ) VALUES ( @name )";
            return bag.Parse(query);
        }
    }
}
