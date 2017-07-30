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
            public bool Data = true;
            public string RowPrefix = "";
            public string RowSuffix = "";
            public string RowSeparator = " | ";
            public bool Frame = false;
            public bool Separator = true;
            public string FrameBetweenPrefix = "";
            public string FrameBetweenSuffix = "";
            public string FrameBetweenSeparator = " + ";
            public string FrameBetweenPattern = "=";
        }

        public class TableFormatFrame: TableFormat
        {
            public TableFormatFrame()
            {
                RowPrefix = " | ";
                RowSuffix = " | ";
                FrameBetweenPrefix = " + ";
                FrameBetweenSuffix = " + ";
            }
        }

        public static string DataTableToPlainText(DataTable t1, Energy.Base.Plain.TableFormat format)
        {
            StringBuilder s = new StringBuilder();
            Energy.Base.Table.DataTableColumnSettings columnDictionary = Energy.Base.Table.GetDataTableColumnSettings(t1);

            if (format == null)
                format = new Energy.Base.Plain.TableFormat();

            if (format.Header)
            {
                s.Append(format.RowPrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(columnDictionary[i].Name.PadRight(columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.RowSeparator);
                }
                s.Append(format.RowSuffix);
                s.AppendLine();
            }

            if (format.Separator)
            {
                s.Append(format.FrameBetweenPrefix);
                int n = columnDictionary.Count;
                for (int i = 0; i < n; i++)
                {
                    s.Append(Energy.Base.Text.Texture(format.FrameBetweenPattern, columnDictionary[i].Size));
                    bool last = i == n - 1;
                    if (!last)
                        s.Append(format.FrameBetweenSeparator);
                }
                s.Append(format.FrameBetweenSuffix);
                s.AppendLine();
            }

            if (format.Data)
            {
                for (int r = 0; r < t1.Rows.Count; r++)
                {
                    s.Append(format.RowPrefix);
                    int n = columnDictionary.Count;
                    for (int i = 0; i < n; i++)
                    {
                        string value = Energy.Base.Cast.ObjectToString(t1.Rows[r][i]) ?? "";
                        value = value.PadLeft(columnDictionary[i].Size);
                        s.Append(value);
                        bool last = i == n - 1;
                        if (!last)
                            s.Append(format.RowSeparator);
                    }
                    s.Append(format.RowSuffix);
                    s.AppendLine();
                }
            }            

            return s.ToString();
        }

        private static char GetMiddleStringPatternChar(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return '\0';
            if (pattern.Length % 2 == 0)
            {
                return '\0';
            }
            else if (pattern.Length == 1)
            {
                return pattern[0];
            }
            else
            {
                int length = pattern.Length / 2;
                return pattern.Substring(length, 1)[0];
            }
        }

        private static string GetMiddleStringPrefix(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return pattern;
            if (pattern.Length % 2 == 0)
            {
                return pattern.Substring(0, pattern.Length / 2);
            }
            else if (pattern.Length == 1)
            {
                return pattern;
            }
            else
            {
                int length = pattern.Length / 2;
                return pattern.Substring(0, length);
            }
        }

        private static string GetMiddleStringSuffix(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return pattern;
            int half = pattern.Length / 2;
            if (pattern.Length % 2 == 0)
            {
                return pattern.Substring(half, half);
            }
            else if (pattern.Length == 1)
            {
                return pattern;
            }
            else
            {
                int length = pattern.Length / 2;
                return pattern.Substring(half + 1, length);
            }
        }
    }
}
