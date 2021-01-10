using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XmlSerializationExample
{
    public class Test
    {
        public void SerializeBaseTable()
        {
            Energy.Base.Table t = new Energy.Base.Table();
            t.Add(new Energy.Base.Record());
            t[0]["a1"] = "b";
            t[0]["a2"] = "c";
            string xml = Energy.Base.Xml.Serialize(t);
            Console.WriteLine(xml);
        }

        public void SerializeCollectionTable()
        {
            DataTable dt = new DataTable();
            Energy.Base.Collection.Table<string, object> x = new Energy.Base.Collection.Table<string, object>();
            x.New().Set(0, 1).Set(1, 2);
            x.New().Set(0, 2).Set(1, 4);
            string xml;
            xml = Energy.Base.Xml.Serialize(x);
            Console.WriteLine(xml);
        }

        public void SerializeDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("a", typeof(int));
            dt.Columns.Add("b", typeof(string));
            dt.Columns.Add("c", typeof(DateTime));
            DataRow dr;
            dr = dt.NewRow();
            dr["a"] = 1;
            dr["b"] = "TEST";
            dr["c"] = DateTime.Now.AddDays(-1);
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["a"] = 2;
            dr["b"] = "???";
            dr["c"] = DateTime.Now.AddDays(0);
            dt.Rows.Add(dr);
            string xml;
            string error;
            xml = Energy.Base.Xml.Serialize(dt, out error);
            if (error.Length > 0)
            {
                Console.WriteLine(error);
            }
            dt.TableName = "Table1";
            xml = Energy.Base.Xml.Serialize(dt);
            Console.WriteLine(xml);

            DataTable dt1;
            dt1 = Energy.Base.Xml.Deserialize<DataTable>(xml);
            Console.WriteLine(dt1.Rows.Count);
        }
    }
}
