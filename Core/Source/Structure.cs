using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Energy.Source
{
    public abstract partial class Structure
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

        /// <summary>
        /// Column definition
        /// </summary>
        public class Column
        {
            public string Name { get; set; }
            public string Label { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            [DefaultValue(false)]
            public bool Ignore { get; set; }
            [DefaultValue(0)]
            public int Length { get; set; }

            [DefaultValue(0)]
            public double Minimum { get; set; }
            [DefaultValue(0)]
            public double Maximum { get; set; }

            public class Array : Energy.Base.Collection.Array<Column>
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

                public Column Get2(string name)
                {
                    return Find(new Predicate<Column>(delegate (Column c) { return c.Name == name; }));
                }
            }

            public static Column New(string name, string type, string label)
            {
                return new Column()
                {
                    Name = name,
                    Type = type,
                    Label = label
                };
            }

            public Column() { }

            public Column(string name, string type, string label)
                : this()
            {
                Name = name;
                Type = type;
                Label = label;
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
        
        public partial class Row: Dictionary<string, object>, IXmlSerializable
        {
            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                foreach (string key in this.Keys)
                {
                    Energy.Core.Xml.Write(writer, key, this[key]);
                }
            }

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

        [Serializable]
        public partial class Table
        {
            public string Name;
            [XmlArray]
            public Column.Array Column = new Column.Array();
            public Index.Array Index = new Index.Array();
            public string Identity;
            public Row.Array Row = new Row.Array();
            public List<string> Key = new List<string>();

            public class Array: Energy.Base.Collection.Array<Table> { }

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

        /// <summary>
        /// Schema
        /// </summary>
        public partial class Schema
        {
            /// <summary>Table</summary>
            public Energy.Base.Collection.Array<Structure.Table> Table = new Base.Collection.Array<Structure.Table>();
        }
    }
}