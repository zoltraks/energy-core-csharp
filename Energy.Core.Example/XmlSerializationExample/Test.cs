using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XmlSerializationExample
{
    public class Test
    {
        public void SerializeDataTable()
        {
            DataTable dt = new DataTable();
            Energy.Base.Collection.Table<string, object> x = new Energy.Base.Collection.Table<string, object>();
            x.New().Set(0, 1).Set(1, 2);
            x.New().Set(0, 2).Set(1, 4);
            string xml;
            xml = Energy.Base.Xml.Serialize(x);
            Console.WriteLine(xml);
        }
    }
}
