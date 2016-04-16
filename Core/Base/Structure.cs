using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Energy.Base
{
    public class Structure
    {

        #region Column

        /// <summary>
        /// Column definition
        /// </summary>
        public partial class Column
        {
            public string Name { get; set; }
            public string Label { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            [DefaultValue(false)]
            public bool Ignore { get; set; }
            [DefaultValue(0)]
            public int Length { get; set; }
            public string Format { get; set; }

            [DefaultValue(0)]
            public double Minimum { get; set; }
            [DefaultValue(0)]
            public double Maximum { get; set; }

            #region Array

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

                /// <summary>
                /// Create new element
                /// </summary>
                /// <param name="name"></param>
                /// <param name="type"></param>
                /// <param name="label"></param>
                /// <returns></returns>
                public Column New(string name, string type = "", string label = "")
                {
                    Column item = new Column(name, type, label);
                    Add(item);
                    return item;
                }
            }

            public Column() { }

            public Column(string name, string type = "", string label = "")
                : this()
            {
                Name = name;
                Type = type;
                Label = label;
            }

            #endregion
        }

        #endregion

        #region Index

        public partial class Index
        {
            public string Name;

            [XmlElement("Column")]
            public Column.Array Columns = new Column.Array();

            [XmlElement("Include")]
            public Column.Array Includes = new Column.Array();

            #region Array

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

            #endregion
        }

        #endregion

        #region Row

        public partial class Row : Dictionary<string, object>, IXmlSerializable
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

            #region Array

            public class Array : Energy.Base.Collection.Array<Row>
            {
            }

            #endregion
        }

        #endregion

        #region Table

        [Serializable]
        public partial class Table
        {
            public string Name;
            public string Identity;
            [XmlElement("Column")]
            public Column.Array Columns = new Column.Array();
            [XmlElement("Index")]
            public Index.Array Indexes = new Index.Array();
            [XmlElement("Row")]
            public Row.Array Rows = new Row.Array();
            [XmlElement("Keys")]
            public List<string> Keys = new List<string>();

            public class Array : Energy.Base.Collection.Array<Table> { }

            public object New(Type type)
            {
                if (type == typeof(Structure.Column))
                {
                    Structure.Column column = new Structure.Column();
                    Columns.Add(column);
                    return column;
                }

                if (type == typeof(Structure.Index))
                {
                    Structure.Index index = new Structure.Index();
                    Indexes.Add(index);
                    return index;
                }

                if (type == typeof(Structure.Row))
                {
                    Structure.Row row = new Structure.Row();
                    Rows.Add(row);
                    return row;
                }

                throw new NotImplementedException("Unsupported class of new element");
            }
        }

        #endregion
    }
}
