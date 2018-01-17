using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Energy.Base
{
    public class Table: List<Energy.Base.Record>
    {
        public Energy.Base.Record New()
        {
            Energy.Base.Record _ = new Energy.Base.Record();
            this.Add(_);
            return _;
        }

        public DataTable ToDataTable(Type type)
        {
            DataTable data = new DataTable();
            for (int i = 0; i < this.Count; i++)
            {
                DataRow row = data.NewRow();
                foreach (KeyValuePair<string, object> column in this[i])
                {
                    if (!data.Columns.Contains(column.Key))
                    {
                        if (type != null)
                            data.Columns.Add(column.Key, type);
                        else
                            data.Columns.Add(column.Key);
                    }
                    if (type == null)
                        row[column.Key] = Energy.Base.Cast.ObjectToString(column.Value);
                    else
                        row[column.Key] = column.Value;
                }
                data.Rows.Add(row);
            }
            return data;
        }

        public DataTable ToDataTable()
        {
            return ToDataTable(null);
        }

        public string ToPlain()
        {
            return Energy.Base.Plain.DataTableToPlainText(ToDataTable(), null);
        }
    }
}
