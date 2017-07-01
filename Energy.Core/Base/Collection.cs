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

        /// <summary>
        /// Thread safe array of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Array<T> : System.Collections.Generic.List<T>
        {
            private readonly object Lock = new object();

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

        #region KeyValueDictionary

        public class KeyValueDictionary : Dictionary<string, string>
        {
            private bool _CaseSensitive = true;

            public Dictionary<string, string> Index = null;

            public bool CaseSensitive
            {
                get
                {
                    return _CaseSensitive;
                }
                set
                {
                    if (_CaseSensitive == value)
                        return;
                    _CaseSensitive = value;
                    if (value)
                        Index = null;
                    else
                        RebuildIndex();
                }
            }

            public void RebuildIndex()
            {
                Index = new Dictionary<string, string>();
                foreach (string key in this.Keys)
                {
                    string map = key.ToUpperInvariant();
                    if (!Index.ContainsKey(map))
                        Index.Add(map, key);
                }
            }

            public void Set(string key, string value)
            {
                if (CaseSensitive)
                {
                    this[key] = value;
                    return;
                }
                string map = key.ToUpperInvariant();
                if (Index != null && Index.ContainsKey(map))
                {
                    string link = Index[map];
                    this[link] = value;
                }
                else
                {
                    this[key] = value;
                    if (Index == null)
                        Index = new Dictionary<string, string>();
                    Index.Add(map, key);
                }
            }

            public string[] ToArray(string separator)
            {
                List<string> list = new List<string>();
                foreach (string key in this.Keys)
                {
                    list.Add(string.Concat(key, separator, this[key]));
                }
                return list.ToArray();
            }
        }

        #endregion
    }
}
