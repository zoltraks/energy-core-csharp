using System;
using System.Collections.Generic;
using System.Data;
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
            for (int c = 0; c < table.Columns.Count; c++)
            {
                System.Data.DataColumn column = table.Columns[c];
                string name = Energy.Base.Text.Select(column.Caption, column.ColumnName);
                List<string> scan = new List<string>();
                int d = 3;
                for (int r = 0; r < d && r < count; r++)
                {
                    scan.Add(Energy.Base.Cast.ObjectToString(table.Rows[r][c]));
                }
                int n = count - 2 * d;
                if (n > 0)
                {
                    System.Random random = new System.Random();
                    if (n > 10)
                        n = 10;
                    List<int> list = new List<int>();
                    for (int i = 0; i < n; i++)
                    {
                        int r = random.Next(d, count - d);
                        if (list.Contains(r))
                            continue;
                        scan.Add(Energy.Base.Cast.ObjectToString(table.Rows[r][c]));
                    }
                }
                for (int r = count - 1; r > 3; r--)
                {
                    scan.Add(Energy.Base.Cast.ObjectToString(table.Rows[r][c]));
                }
                DataTableColumnSettings.Item item = new Base.Table.DataTableColumnSettings.Item()
                {
                    Name = name,
                    Index = c,
                    Size = name.Length,
                };
                for (int j = 0; j < scan.Count; j++)
                {
                    if (scan[j].Length > item.Size)
                        item.Size = scan[j].Length;
                }
                _.Add(item);
            }
            return _;
        }

        public class PlainFormat
        {
            public bool Header = true;
            public bool Data = true;
            public string RowPrefix = "";
            public string RowSuffix = "";
            public string RowSeparator = " | ";
            public bool Frame = false;
            public string FramePrefix = "";
            public string FrameSuffix = "";
            public string FrameSeparator = " + ";
            public string FramePattern = "=";
            public bool Inner = true;
            public string InnerPrefix = "";
            public string InnerSuffix = "";
            public string InnerSeparator = " + ";
            public string InnerPattern = "-";
            public string FrameTilde = "~8~";
            public string HeaderTilde = "~10~";
            public string TextTilde = "~14~";
            public string TextTildeOdd = "~15~";
            public bool Tilde = false;

            public class FullFrame : PlainFormat
            {
                public FullFrame()
                {
                    Frame = true;
                    RowPrefix = " | ";
                    RowSuffix = " | ";
                    FramePrefix = " + ";
                    FrameSuffix = " + ";
                    InnerPrefix = " + ";
                    InnerSuffix = " + ";
                }
            }
        }

        public static string DataTableToPlainText(DataTable t1, Energy.Base.Table.PlainFormat format)
        {
            StringBuilder s = new StringBuilder();
            Energy.Base.Table.DataTableColumnSettings columnDictionary = Energy.Base.Table.GetDataTableColumnSettings(t1);

            if (format == null)
                format = new Energy.Base.Table.PlainFormat();

            if (format.Frame)
            {
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.FramePrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(Energy.Base.Text.Texture(format.FramePattern, columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.FrameSeparator);
                }
                s.Append(format.FrameSuffix);
                if (format.Tilde)
                    s.Append("~0~");
                s.AppendLine();
            }

            if (format.Header)
            {
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.RowPrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    if (format.Tilde)
                        s.Append(format.HeaderTilde);
                    s.Append(columnDictionary[i].Name.PadRight(columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                    {
                        if (format.Tilde)
                            s.Append(format.FrameTilde);
                        s.Append(format.RowSeparator);
                    }
                }
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.RowSuffix);
                if (format.Tilde)
                    s.Append("~0~");
                s.AppendLine();
            }

            if (format.Inner)
            {
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.InnerPrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(Energy.Base.Text.Texture(format.InnerPattern, columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.InnerSeparator);
                }
                s.Append(format.InnerSuffix);
                if (format.Tilde)
                    s.Append("~0~");
                s.AppendLine();
            }

            if (format.Data)
            {
                for (int r = 0; r < t1.Rows.Count; r++)
                {
                    bool odd = r % 2 != 0;
                    if (format.Tilde)
                        s.Append(format.FrameTilde);
                    s.Append(format.RowPrefix);
                    int n = columnDictionary.Count;
                    for (int i = 0; i < n; i++)
                    {
                        string value = Energy.Base.Cast.ObjectToString(t1.Rows[r][i]) ?? "";
                        value = value.PadLeft(columnDictionary[i].Size);
                        if (format.Tilde)
                            s.Append(odd ? format.TextTildeOdd : format.TextTilde);
                        s.Append(value);
                        bool last = i == n - 1;
                        if (!last)
                        {
                            if (format.Tilde)
                                s.Append(format.FrameTilde);
                            s.Append(format.RowSeparator);
                        }
                    }
                    if (format.Tilde)
                        s.Append(format.FrameTilde);
                    s.Append(format.RowSuffix);
                    if (format.Tilde)
                        s.Append("~0~");
                    s.AppendLine();
                }
            }

            if (format.Frame)
            {
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.FramePrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(Energy.Base.Text.Texture(format.FramePattern, columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.FrameSeparator);
                }
                s.Append(format.FrameSuffix);
                if (format.Tilde)
                    s.Append("~0~");
                s.AppendLine();
            }

            return s.ToString();
        }
    }
}
