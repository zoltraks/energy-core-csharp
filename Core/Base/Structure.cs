using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Energy.Base
{
    public partial class Structure
    {
        //public class Array<T> : List<T>
        //{
        //    public T Get(string name)
        //    {
        //        FieldInfo field = (new T()).GetType().GetField("Name");

        //        for (int i = 0; i < Count; i++)
        //        {
        //            if (field.GetValue(this[i]) == name)
        //                return this[i];
        //        }
        //        return default(T);
        //    }

        //    public T Add(T item)
        //    {
        //        base.Add(item);
        //        return item;
        //    }
        //}

        public class Column
        {
            public class Array : Collection.Array<Column>
            {
                public Column Get(string name)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].Name == name)
                            return this[i];
                    }
                    return null;
                }

                public Column New()
                {
                    Column item = new Column();
                    base.Add(item);
                    return item;
                }

                public new Column Add(Column item)
                {
                    base.Add(item);
                    return item;
                }
            }

            public string Name;
            public string Label;
            public string Description;
            public string Type;
            public bool Ignore;

            public static Column Create(string name, string type, string label)
            {
                return new Column()
                {
                    Name = name,
                    Type = type,
                    Label = label
                };
            }
        }

        public class Index
        {
            public class Array : List<Index>
            {
                public Index Get(string name)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].Name == name)
                            return this[i];
                    }
                    return null;
                }

                public new Index Add(Index item)
                {
                    base.Add(item);
                    return item;
                }
            }

            public string Name;
            public Column.Array Column = new Column.Array();
            public Column.Array Include = new Column.Array();
        }
        
        public partial class Row: Dictionary<string, object>
        {
            public class Array : List<Row>
            {
                public new Row Add(Row item)
                {
                    base.Add(item);
                    return item;
                }

                public Row First
                {
                    get
                    {
                        return this.Count == 0 ? null : this[0];
                    }
                }

                public Row Last
                {
                    get
                    {
                        return this.Count == 0 ? null : this[this.Count - 1];
                    }
                }
            }

            //public object this[string key]
            //{
            //    get
            //    {
            //        return base[key];
            //    }
            //}
        }

        public partial class Table
        {
            public string Name;
            public Column.Array Column = new Column.Array();
            public Index.Array Index = new Index.Array();
            public string Identity;
            public Row.Array Row = new Row.Array();
            public List<string> Key = new List<string>();

            public object New(Type type)
            {
                if (type == typeof(Structure.Column))
                {
                    Structure.Column column = new Structure.Column();
                    Column.Add(column);
                    return column;
                }

                if (type == typeof(Structure.Index))
                {
                    Structure.Index index = new Structure.Index();
                    Index.Add(index);
                    return index;
                }

                if (type == typeof(Structure.Row))
                {
                    Structure.Row row = new Structure.Row();
                    Row.Add(row);
                    return row;
                }

                throw new NotImplementedException("Unsupported class of new element");
            }
        }
    }
}