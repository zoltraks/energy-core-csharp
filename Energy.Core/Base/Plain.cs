using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Plain text functions
    /// </summary>
    public class Plain
    {
        #region DataTable

        public static string DataTableToPlainText(DataTable table, Energy.Base.Plain.TableFormat format)
        {
            StringBuilder s = new StringBuilder();
            Energy.Base.Plain.TableColumnSettings columnDictionary = Energy.Base.Plain.TableColumnSettings.Create(table);

            if (format == null)
                format = new Energy.Base.Plain.TableFormat();

            if (format.MaximumLength > 0)
                columnDictionary.SetMaximumLength(format.MaximumLength);

            if (format.MinimumLength > 0)
                columnDictionary.SetMinimumLength(format.MinimumLength);

            // frame //

            if (format.Frame)
            {
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.FramePrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(Energy.Base.Text.Texture(format.FramePattern, columnDictionary[i].Length));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.FrameSeparator);
                }
                s.Append(format.FrameSuffix);
                if (format.Tilde)
                    s.Append("~0~");
                s.AppendLine();
            }

            // header //

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
                    string caption = columnDictionary[i].Name;
                    if (format.MaximumLength > 0 && caption.Length > format.MaximumLength)
                    {
                        caption = Energy.Base.Text.Limit(caption, format.MaximumLength, format.LimitSuffix);
                    }
                    else
                    {
                        caption = caption.PadRight(columnDictionary[i].Length);
                    }
                    s.Append(caption);
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

            // frame //

            if (format.Inner)
            {
                if (format.Tilde)
                    s.Append(format.FrameTilde);
                s.Append(format.InnerPrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(Energy.Base.Text.Texture(format.InnerPattern, columnDictionary[i].Length));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.InnerSeparator);
                }
                s.Append(format.InnerSuffix);
                if (format.Tilde)
                    s.Append("~0~");
                s.AppendLine();
            }

            // content //

            if (format.Data)
            {
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    bool odd = r % 2 != 0;
                    if (format.Tilde)
                        s.Append(format.FrameTilde);
                    s.Append(format.RowPrefix);
                    int n = columnDictionary.Count;
                    for (int i = 0; i < n; i++)
                    {
                        object o = table.Rows[r][i];
                        string value = Energy.Base.Cast.ObjectToString(o) ?? "";
                        long _long;
                        if (format.FormatInteger.Length > 0 && Energy.Base.Text.TryParse<long>(value, out _long))
                        {
                            value = _long.ToString(format.FormatInteger).Trim();
                        }

                        int limit = columnDictionary[i].Length;
                        if (format.MaximumLength > 0 && format.MaximumLength < limit)
                            limit = format.MaximumLength;

                        if (limit > 0 && value.Length > limit)
                        {
                            value = Energy.Base.Text.Limit(value, limit, format.LimitSuffix, "");
                        }
                        else
                        {
                            value = value.PadLeft(columnDictionary[i].Length);
                        }
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
                    s.Append(Energy.Base.Text.Texture(format.FramePattern, columnDictionary[i].Length));
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

        #endregion

        #region TableColumnSettings

        public class TableColumnSettings : List<TableColumnSettings.Item>
        {
            public class Item
            {
                public int Index;
                public string Name;
                public int Length;
                public Energy.Enumeration.TextAlign Align;
            }

            public void SetMaximumLength(int maximum)
            {
                if (maximum <= 0)
                    return;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Length > 0 && this[i].Length > maximum)
                        this[i].Length = maximum;
                }
            }

            public void SetMinimumLength(int minimum)
            {
                if (minimum <= 0)
                    return;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Length > 0 && this[i].Length < minimum)
                        this[i].Length = minimum;
                }
            }

            /// <summary>
            /// Create table column settings from existing DataTable
            /// </summary>
            /// <param name="table"></param>
            /// <returns></returns>
            public static TableColumnSettings Create(System.Data.DataTable table)
            {
                TableColumnSettings _ = new TableColumnSettings();
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
                    TableColumnSettings.Item item = new TableColumnSettings.Item()
                    {
                        Name = name,
                        Index = c,
                        Length = name.Length,
                    };
                    for (int j = 0; j < scan.Count; j++)
                    {
                        if (scan[j].Length > item.Length)
                            item.Length = scan[j].Length;
                        //if (format.FormatInteger.Length > 0 && Energy.Base.Cast.IsLong(value, true))
                        //{
                        //    value = Energy.Base.Cast.AsLong(value).ToString(format.FormatInteger).Trim();
                        //}
                    }
                    _.Add(item);
                }
                return _;
            }
        }

        #endregion

        #region TableFormat

        public class TableFormat
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
            public int MinimumLength = 0;
            public int MaximumLength = 0;
            public string LimitSuffix = "...";
            public string FormatInteger = "";

            public TableFormat SetTildeColorScheme(string name)
            {
                switch ((name ?? "").ToUpper())
                {
                    default:
                    case "DEFAULT":
                        FrameTilde = "~8~";
                        HeaderTilde = "~10~";
                        TextTilde = "~14~";
                        TextTildeOdd = "~15~";
                        break;
                    case "GRAY":
                    case "SILVER":
                        FrameTilde = "~15~";
                        HeaderTilde = "~7~";
                        TextTilde = "~7~";
                        TextTildeOdd = "~8~";
                        break;
                    case "YELLOW":
                        FrameTilde = "~6~";
                        HeaderTilde = "~14~";
                        TextTilde = "~15~";
                        TextTildeOdd = "~14~";
                        break;
                }
                return this;
            }

            public TableFormat SetFrameStyle(string name)
            {
                Frame = true;
                switch ((name ?? "").ToUpper())
                {
                    default:
                    case "DEFAULT":
                    case "FULL":
                        RowPrefix = " | ";
                        RowSuffix = " | ";
                        FramePrefix = " + ";
                        FrameSuffix = " + ";
                        InnerPrefix = " + ";
                        InnerSuffix = " + ";
                        break;
                    case "PADDED":
                    case "PADDED4":
                        RowPrefix = " |  ";
                        InnerPrefix = " + -";
                        FramePrefix = " + =";
                        RowSuffix = "  | ";
                        InnerSuffix = "- + ";
                        FrameSuffix = "= + ";
                        break;
                }
                return this;
            }

            public class FullFrame : TableFormat
            {
                public FullFrame()
                {
                    SetFrameStyle("FULL");
                }
            }
        }

        #endregion
    }
}
