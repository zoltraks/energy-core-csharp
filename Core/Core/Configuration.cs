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
    }
}
