using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Plain text
    /// </summary>
    public class Plain
    {
        public class TableFormat
        {
            public bool Header = true;
            public string RowBegin = "";
            public string RowTrail = "";
            public string ColumnSeparator = " | ";
            public string FrameBefore = "+-+";
            public string FrameBetween = "+ -=- +";
        }

        public static string DataTableToPlainText(DataTable t1, Energy.Base.Plain.TableFormat format)
        {
            StringBuilder s = new StringBuilder();
            Energy.Base.Table.DataTableColumnSettings columnDictionary = Energy.Base.Table.GetDataTableColumnSettings(t1);

            if (format == null)
                format = new Energy.Base.Plain.TableFormat();

            if (format.Header)
            {
                s.Append(format.RowBegin);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(columnDictionary[i].Name.PadLeft(columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.ColumnSeparator);
                }
                s.Append(format.RowTrail);
                s.AppendLine();
            }

            if (!string.IsNullOrEmpty(format.FrameBetween))
            {
                s.Append(format.RowBegin);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(columnDictionary[i].Name.PadLeft(columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.ColumnSeparator);
                }
                s.Append(format.RowTrail);
                s.AppendLine();
            }

            //Dictionary<int, string> columnNameDictionary = new Dictionary<int, string>();
            //Dictionary<int, int> columnSizeDictionary = new Dictionary<int, int>();
            //Dictionary<int, Energy.Enumeration.TextAlign> columnAlignDictionary = new Dictionary<int, Enumeration.TextAlign>();
            //for (int i = 0; i < t1.Columns.Count; i++)
            //{
            //    DataColumn c = t1.Columns[i];
            //    string name = null;
            //    if (format.TableUseCaption && !string.IsNullOrEmpty(c.Caption))
            //        name = c.Caption;
            //    if (name == null && !string.IsNullOrEmpty(c.ColumnName))
            //        name = c.ColumnName;
            //    columnNameDictionary[i] = name;

            //    // column scan
            //    int size = 0;
            //    t1.Rows[0][columnIndex]
            //    columnSizeDictionary[i] = DataTableColumnLengthScan();
            //}
            return s.ToString();
        }
    }
}
