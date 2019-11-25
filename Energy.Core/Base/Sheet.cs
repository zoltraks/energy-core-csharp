using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    public class Sheet
    {
        public class Address
        {
            public long Row;
            
            public string Column;

            public bool StickyRow = false;
            
            public bool StickyColumn = false;

            public override string ToString()
            {
                string text = Column;
                if (StickyColumn)
                {
                    text = "$" + text;
                }
                if (0 < Row)
                {
                    if (StickyRow)
                    {
                        text += "$";
                    }
                    text += Row;
                }
                return text;
            }

            /// <summary>
            /// Because it's not structure, we will have getter returning default() or new every time.
            /// </summary>
            public static Address Empty
            {
                get
                {
                    return default(Address);
                }
            }

            public bool RowLess 
            {
                get
                {
                    return 0 >= Row;
                }
            }
            
            public bool ColumnLess
            {
                get
                {
                    return string.IsNullOrEmpty(Column);
                }
            }

            public long ColumnIndex
            {
                get
                {
                    return GetColumnIndex();
                }
            }

            private static readonly string COLUMN_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            private long GetColumnIndex()
            {
                return GetColumnIndex(Column);
            }

            public static long GetColumnIndex(string column)
            {
                long r = 0;
                char[] ca = column.ToCharArray();
                for (int i = 0; i < ca.Length; i++)
                {
                    if (0 < i)
                    {
                        r = COLUMN_LETTERS.Length * r;
                    }
                    int n = COLUMN_LETTERS.IndexOf(ca[i]);
                    if (n < 0)
                    {
                        continue;
                    }
                    r += 1 + n;
                }
                return r;
            }

            /// <summary>
            /// Create object from text representation
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static Address From(string text)
            {
                string pattern = @"(?:(?<stickyColumn>\$)?(?<column>[A-Za-z]+))?(?:(?<stickyRow>\$)?(?<row>\d+))?";
                Regex regex = new Regex(pattern, RegexOptions.None);
                Match match = regex.Match(text);
                if (!match.Success)
                {
                    return Address.Empty;
                }
                string column = match.Groups["column"].Value;
                bool stickyRow = "$" == match.Groups["stickyRow"].Value;
                bool stickyColumn = "$" == match.Groups["stickyColumn"].Value;
                long row = 0;
                long.TryParse(match.Groups["row"].Value, out row);
                return 
                    new Address() 
                    { 
                        Row = row, Column = column, 
                        StickyRow = stickyRow, StickyColumn = stickyColumn
                    };
            }
        }

        public class Range
        {
            public Range() { }

            public Range(Address begin, Address to)
            {
                this.Begin = begin;
                this.To = to;
            }

            public static Range From(Address from, Address to)
            {
                return new Range(from, to);
            }

            public Address Begin, To;

            /// <summary>
            /// Because it's not structure, we will have getter returning default() or new every time.
            /// </summary>
            public static Range Empty
            {
                get
                {
                    return default(Range);
                }
            }

            public static Range From(string text)
            {
                string pattern = @"(?:(?<stickyColumnA>\$)?(?<columnA>[A-Za-z]+))?(?:(?<stickyRowA>\$)?(?<rowA>\d+))?(?::(?:(?<stickyColumnB>\$)?(?<columnB>[A-Za-z]+))?(?:(?<stickyRowB>\$)?(?<rowB>\d+))?)?";
                Regex regex = new Regex(pattern, RegexOptions.None);
                Match match = regex.Match(text);
                if (!match.Success)
                {
                    return Range.Empty;
                }
                string columnA = match.Groups["columnA"].Value;
                bool stickyRowA = "$" == match.Groups["stickyRowA"].Value;
                bool stickyColumnA = "$" == match.Groups["stickyColumnA"].Value;
                long rowA = 0;
                long.TryParse(match.Groups["rowA"].Value, out rowA);
                string columnB = match.Groups["columnB"].Value;
                bool stickyRowB = "$" == match.Groups["stickyRowB"].Value;
                bool stickyColumnB = "$" == match.Groups["stickyColumnB"].Value;
                long rowB = 0;
                long.TryParse(match.Groups["rowB"].Value, out rowB);
                return
                    new Range()
                    {
                        Begin = new Address()
                        {
                            Row = rowA,
                            Column = columnA,
                            StickyRow = stickyRowA,
                            StickyColumn = stickyColumnA
                        },
                        To = new Address()
                        {
                            Row = rowB,
                            Column = columnB,
                            StickyRow = stickyRowB,
                            StickyColumn = stickyColumnB
                        }
                    };
            }
        }
    }
}
