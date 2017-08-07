using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Energy.Source
{
    public class Structure
    {
        #region Column

        /// <summary>
        /// Column definition
        /// </summary>
        public partial class Column
        {
            #region Private

            private string _Name = "";

            private string _Type = "";

            private string[] _Specific = null;

            private string _Label = "";

            private string _Description = "";

            private string _Format = "";

            #endregion

            #region Property

            [DefaultValue("")]
            public string Name { get { return _Name; } set { _Name = value == null ? "" : value; } }
            [DefaultValue("")]
            public string Type { get { return _Type; } set { _Type = value == null ? "" : value; } }

            [DefaultValue(null)]
            public string[] Specific { get { return _Specific; } set { _Specific = value != null && value.Length == 0 ? null : value; } }

            [DefaultValue("")]
            public string Label { get { return _Label; } set { _Label = value == null ? "" : value; } }
            [DefaultValue("")]
            public string Description { get { return _Description; } set { _Description = value == null ? "" : value; } }

            [DefaultValue("")]
            public string Format { get { return _Format; } set { _Format = value == null ? "" : value; } }

            [DefaultValue(false)]
            public bool Ignore { get; set; }

            [DefaultValue(false)]
            public bool Unique { get; set; }

            [DefaultValue(false)]
            public bool Unsigned { get; set; }

            [DefaultValue(false)]
            public bool NotNull { get; set; }

            [DefaultValue(0)]
            public int Length { get; set; }

            [DefaultValue(0)]
            public int Order { get; set; }

            [DefaultValue(false)]
            public bool Primary { get; set; }

            [DefaultValue(false)]
            public bool Index { get; set; }

            [DefaultValue(0)]
            public double Minimum { get; set; }
            [DefaultValue(0)]
            public double Maximum { get; set; }

            [DefaultValue(0)]
            public int Increment { get; set; }

            #endregion

            #region Array

            [Serializable]
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
                /// <returns></returns>
                public Column New(string name)
                {
                    Column item = new Column(name);
                    Add(item);
                    return item;
                }

                /// <summary>
                /// Find first primary column
                /// </summary>
                /// <returns></returns>
                public Column GetPrimary()
                {
                    List<Energy.Source.Structure.Column> list = new List<Energy.Source.Structure.Column>(this);
                    list.Sort(delegate (Energy.Source.Structure.Column a, Energy.Source.Structure.Column b)
                    {
                        return a.Order.CompareTo(b.Order);
                    });
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Primary)
                            return list[i];
                    }
                    return null;
                }

                /// <summary>
                /// Get first primary column name
                /// </summary>
                /// <returns></returns>
                public string GetPrimaryName()
                {
                    Column column = GetPrimary();
                    if (column == null)
                        return null;
                    return column.Name;
                }
            }

            public bool IsName(string name)
            {
                return 0 == string.Compare(this.Name, name, true);
            }

            public Column() { }

            public Column(string name, string type, string label)
            {
                Name = name;
                Type = type;
                Label = label;
            }

            public Column(string name)
            {
                Name = name;
            }

            public Column(string name, string type)
            {
                Name = name;
                Type = type;
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

            [Serializable]
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

        [Serializable]
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
                    Energy.Base.Xml.Write(writer, key, this[key]);
                }
            }

            #region Array

            [Serializable]
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
            /// <summary>
            /// Table name
            /// </summary>
            public string Name;

            /// <summary>
            /// Table name
            /// </summary>
            public string Schema;

            /// <summary>
            /// Identity column for table
            /// </summary>
            public string Identity;

            /// <summary>
            /// Table description
            /// </summary>
            public string Description;

            /// <summary>
            /// Table engine (optional)
            /// </summary>
            public string Engine;

            [XmlElement("Column")]
            public Column.Array Columns = new Column.Array();
            [XmlElement("Index")]
            public Index.Array Indexes = new Index.Array();
            [XmlElement("Key")]
            public List<string> Keys = new List<string>();

            [XmlElement("Row")]
            public Row.Array Rows = new Row.Array();

            public class Array : Energy.Base.Collection.Array<Table>
            {
                public Table Get(string name)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].ToString() == name)
                        {
                            return this[i];
                        }
                    }
                    return null;
                }

                public Table Find(string name)
                {
                    return Find((_) => 0 == string.Compare(_.Name, name, true));
                }

                public string[] GetNames()
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < this.Count; i++)
                    {
                        list.Add(this[i].Name);
                    }
                    return list.ToArray();
                }
            }

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

            public static Table Create(Type type)
            {
                Table table = new Table();

                Energy.Attribute.Data.TableAttribute attributeTable = (Energy.Attribute.Data.TableAttribute)
                       Energy.Base.Class.GetClassAttribute(type, typeof(Energy.Attribute.Data.TableAttribute));

                if (attributeTable != null)
                {
                    table.Name = attributeTable.Name;
                    table.Description = attributeTable.Description;  
                }

                string[] fields = Energy.Base.Class.GetFieldsAndProperties(type);
                for (int i = 0; i < fields.Length; i++)
                {
                    Energy.Source.Structure.Column column = table.Columns.New(fields[i]);
                    //typeof(Energy.Attribute.Data.PrimaryAttribute)
                    Energy.Attribute.Data.PrimaryAttribute attributePrimary = (Energy.Attribute.Data.PrimaryAttribute)
                        Energy.Base.Class.GetFieldOrPropertyAttribute(type, fields[i], typeof(Energy.Attribute.Data.PrimaryAttribute));
                    if (attributePrimary != null)
                        column.Primary = true;
                    Energy.Attribute.Data.ColumnAttribute attributeColumn = (Energy.Attribute.Data.ColumnAttribute)
                        Energy.Base.Class.GetFieldOrPropertyAttribute(type, fields[i], typeof(Energy.Attribute.Data.ColumnAttribute));
                    if (attributeColumn != null)
                    {
                        column.Name = attributeColumn.Name;
                    }
                    Energy.Attribute.Data.TypeAttribute attributeType = (Energy.Attribute.Data.TypeAttribute)
                        Energy.Base.Class.GetFieldOrPropertyAttribute(type, fields[i], typeof(Energy.Attribute.Data.TypeAttribute));
                    if (attributeType != null)
                    {
                        column.Type = attributeType.Name;
                    }
                    Energy.Attribute.Data.LabelAttribute attributeLabel = (Energy.Attribute.Data.LabelAttribute)
                        Energy.Base.Class.GetFieldOrPropertyAttribute(type, fields[i], typeof(Energy.Attribute.Data.LabelAttribute));
                    if (attributeLabel != null)
                    {
                        column.Label = attributeLabel.Label;
                    }
                    Energy.Attribute.Data.DescriptionAttribute attributeDescription = (Energy.Attribute.Data.DescriptionAttribute)
                        Energy.Base.Class.GetFieldOrPropertyAttribute(type, fields[i], typeof(Energy.Attribute.Data.DescriptionAttribute));
                    if (attributeDescription != null)
                    {
                        column.Description = attributeDescription.Description;
                    }
                }
                return table;
            }

            public bool Equals(Table table)
            {
                if (Name != table.Name)
                    return false;
                if (Identity != table.Identity)
                    return false;
                if (Columns.Count != table.Columns.Count || !Columns.Equals(table.Columns))
                    return false;
                if (Indexes.Count != table.Indexes.Count)
                    return false;
                if (Rows.Count != table.Rows.Count)
                    return false;
                return true;
            }
        }

        #endregion

        #region Schema

        public class Schema
        {

        }

        #endregion
    }
}
