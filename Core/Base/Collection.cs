using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Energy.Base
{
    public class Collection
    {
        #region Array

        public class Array<T> : List<T>
        {
            public T First
            {
                get
                {
                    return Count == 0 ? default(T) : this[0];
                }
            }

            public T Last
            {
                get
                {
                    return Count == 0 ? default(T) : this[Count - 1];
                }
            }

            public T New()
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                base.Add(item);
                return item;
            }

            public bool Equals(Array<T> array)
            {
                if (this.Count != array.Count)
                {
                    return false;
                }
                for (int i = 0; i < this.Count; i++)
                {
                    if (!this[i].Equals(array[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region Associative

        /// <summary>
        /// Serializable string dictionary of objects
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        [XmlRoot]
        public class Associative<T> : Dictionary<string, T>, IXmlSerializable
        {
            #region IXmlSerializable Members

            /// <summary>
            /// XML Schema
            /// </summary>
            /// <returns></returns>
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            /// <summary>
            /// Read XML
            /// </summary>
            /// <param name="reader">XmlReader</param>
            public void ReadXml(System.Xml.XmlReader reader)
            {
                Clear();

                bool empty = reader.IsEmptyElement;

                reader.Read();

                if (empty) return;

                reader.MoveToContent();

                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    {
                        reader.Read();
                        continue;
                    }
                    string key = reader.Name;
                    if (reader.IsEmptyElement)
                    {
                        Add(key, default(T));
                        reader.Read();
                    }
                    else
                    {
                        reader.ReadStartElement();
                        Add(key, (T)(object)reader.ReadString());
                        reader.ReadEndElement();
                    }
                }

                reader.ReadEndElement();
            }

            /// <summary>
            /// Write XML
            /// </summary>
            /// <param name="writer">XmlWriter</param>
            public void WriteXml(System.Xml.XmlWriter writer)
            {
                foreach (string key in this.Keys)
                {
                    writer.WriteStartElement(key.ToString());
                    if (this[key] != null) writer.WriteValue(this[key]);
                    writer.WriteEndElement();
                }
            }

            #endregion
        }

        #endregion
    }
}
