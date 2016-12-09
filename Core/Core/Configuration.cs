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
        public class Option
        {
            public string Name;
            public bool Switch;
            public string Value;

            public class List : List<Option>
            {
                /// <summary>
                /// Add an argument to a list or return existing one if found.
                /// </summary>
                /// <param name="name">Argument name</param>
                /// <returns>Argument object</returns>
                public Option Add(string name)
                {
                    Option item = Find(x => x.Name == name);
                    if (item == null)
                    {                        
                        item = new Core.Configuration.Option() { Name = name };
                        base.Add(item);
                    }
                    return item;
                }
            }
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        private Option.List _Options = new Option.List();
        /// <summary>Arguments</summary>
        public Option.List Options { get { return _Options; } set { _Options = value; } }

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
