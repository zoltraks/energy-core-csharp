using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Table
    {
        public class DataTableColumnSettings: List<DataTableColumnSettings.Item>
        {
            public class Item
            {
                public int Index;
                public string Name;
                public int Size;
                public Energy.Enumeration.TextAlign Align;
            }
        }

        public static DataTableColumnSettings GetDataTableColumnSettings(System.Data.DataTable table)
        {
            DataTableColumnSettings _ = new DataTableColumnSettings();
            int count = table.Rows.Count;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                System.Data.DataColumn c = table.Columns[i];
                string name = Energy.Base.Text.Select(c.Caption, c.ColumnName);
                int index = i;
                List<string> scan = new List<string>();
                int d = 3;
                for (int j = 0; j < d && j < count; j++)
                {
                    scan.Add(Energy.Base.Cast.ObjectToString(table.Rows[i][j]));
                }
                int n = count - 2 * d;
                if (n > 0)
                {
                    System.Random random = new System.Random();
                    if (n > 10)
                        n = 10;
                    List<int> list = new List<int>();
                    for (int j = 0; j < n; j++)
                    {
                        int x = random.Next(d, count - d);
                        if (list.Contains(x))
                            continue;
                        scan.Add(Energy.Base.Cast.ObjectToString(table.Rows[i][j]));
                    }
                }
                for (int j = count - 1; j > 3; j--)
                {
                    scan.Add(Energy.Base.Cast.ObjectToString(table.Rows[i][j]));
                }
                DataTableColumnSettings.Item item = new Base.Table.DataTableColumnSettings.Item();
                item.Name = name;
                item.Index = index;
                item.Size = name.Length;
                for (int j = 0; j < scan.Count; j++)
                {
                    if (scan[j].Length > item.Size)
                        item.Size = scan[j].Length;
                }
                _.Add(item);
            }
            return _;
        }
    }
}
