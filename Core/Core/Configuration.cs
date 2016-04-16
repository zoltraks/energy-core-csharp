using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Configuration
    /// </summary>
    public class Configuration : Energy.Interface.ILoadFromFile
    {
        public class Element
        {
            public string Name;
            public object Value;
            //public string this() { get; set; }
        }

        public bool Load(string file)
        {
            //Configuration configuration = new Configuration();
            return true;
        }

        public bool Load(Energy.Source.Configuration connection)
        {
            //connection.Fetch(connection.Query.Create(table));
            //Configuration configuration = new Configuration();
            return true;            
        }

        public Element this[string key]
        {
            get
            {
                return new Element();
            }
        }

        public Element Select(params string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Element item = this[array[0]];
                if (item != null)
                    return item;
            }
            return null;
        }
    }
}
