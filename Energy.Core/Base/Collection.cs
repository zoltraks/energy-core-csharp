﻿using System;
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
            public T First
            {
                get
                {
                    lock (this)
                    {
                        return base.Count == 0 ? default(T) : this[0];
                    }
                }
            }

            public T Last
            {
                get
                {
                    lock (this)
                    {
                        return base.Count == 0 ? default(T) : this[base.Count - 1];
                    }
                }
            }

            public T New()
            {
                lock (this)
                {
                    T item = (T)Activator.CreateInstance(typeof(T));
                    base.Add(item);
                    return item;
                }
            }

            public bool Equals(Array<T> array)
            {
                lock (this)
                {
                    if (base.Count != array.Count)
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

            public new T Add(T item)
            {
                lock (this)
                {
                    base.Add(item);
                    return item;
                }
            }

            public new T Insert(int index, T item)
            {
                lock (this)
                {
                    base.Insert(index, item);
                    return item;
                }
            }

            public new void Remove(T item)
            {
                lock (this)
                {
                    base.Remove(item);
                }
            }

            public new void RemoveAt(int index)
            {
                lock (this)
                {
                    base.RemoveAt(index);
                }
            }

            public new void RemoveAll(Predicate<T> match)
            {
                lock (this)
                {
                    base.RemoveAll(match);
                }
            }
        }

        #endregion

        #region Associative

        /// <summary>
        /// Serializable string dictionary of objects
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        [XmlRoot]
        [Serializable]
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

        #region StringDictionary

        [XmlRoot]
        [Serializable]
        public class StringDictionary<T> : Dictionary<string, T>, IXmlSerializable
        {
            /// <summary>
            /// Index of keys for case insensitive option
            /// </summary>
            public Dictionary<string, string> Index = null;

            private bool _CaseSensitive = true;

            /// <summary>
            /// Should keys be case sensitive (default) or case insensitive.
            /// When set to false lower key names will be treated as upper.
            /// </summary>
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

            private readonly object _Lock = new object();

            private string _XmlParentSeparator = ".";
            private readonly object _XmlParentSeparatorLock = new object();
            /// <summary>XmlParentSeparator</summary>
            public string XmlParentSeparator { get { lock (_XmlParentSeparatorLock) return _XmlParentSeparator; } set { lock (_XmlParentSeparatorLock) _XmlParentSeparator = value; } }

            private string _XmlEscapeString = "_";
            private readonly object _XmlSpecialCharacterLock = new object();
            /// <summary>XmlSpecialCharacter</summary>
            public string XmlEscapeString { get { lock (_XmlSpecialCharacterLock) return _XmlEscapeString; } set { lock (_XmlSpecialCharacterLock) _XmlEscapeString = value; } }

            private void RebuildIndex()
            {
                Index = new Dictionary<string, string>();
                foreach (string key in base.Keys)
                {
                    string map = key.ToUpperInvariant();
                    if (!Index.ContainsKey(map))
                        Index.Add(map, key);
                }
            }

            public new T this[string key]
            {
                get
                {
                    return Get(key);
                }
                set
                {
                    Set(key, value);
                }
            }

            public T Get(string key)
            {
                if (string.IsNullOrEmpty(key))
                    return default(T);

                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        if (!base.ContainsKey(key))
                            return default(T);
                        return base[key];
                    }
                    string map = key.ToUpperInvariant();
                    if (Index == null || !Index.ContainsKey(map))
                        return default(T);
                    return base[Index[map]];
                }
            }

            public void Get(string key, out T value)
            {                
                value = Get(key);
            }

            public void Set(string key, T value)
            {
                if (string.IsNullOrEmpty(key))
                    return;

                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        base[key] = value;
                        return;
                    }
                    string map = key.ToUpperInvariant();
                    if (Index != null && Index.ContainsKey(map))
                    {
                        string link = Index[map];
                        base[link] = value;
                    }
                    else
                    {
                        base[key] = value;
                        if (Index == null)
                            Index = new Dictionary<string, string>();
                        Index.Add(map, key);
                    }
                }
            }

            public string[] ToArray(string separator)
            {
                List<string> list = new List<string>();
                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        foreach (string key in base.Keys)
                        {
                            list.Add(string.Concat(key, separator, Energy.Base.Cast.ObjectToString(base[key])));
                        }
                    }
                    else
                    {
                        foreach (string key in Index.Keys)
                        {
                            list.Add(string.Concat(Index[key], separator, Energy.Base.Cast.ObjectToString(base[Index[key]])));
                        }
                    }
                    return list.ToArray();
                }
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                string xmlParentSeparator = XmlParentSeparator;
                string xmlEscapeString = XmlEscapeString;
                List<string> parentList = new List<string>();
                string currentElement = null;
                Dictionary<string, T> d = new Dictionary<string, T>();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            parentList.Add(currentElement);
                            currentElement = reader.Name;
                            break;

                        case XmlNodeType.Text:
                            string[] array = Energy.Base.Text.Convert(parentList.ToArray(), XmlEscapeName);
                            string element = string.Join(_XmlParentSeparator, parentList.ToArray());
                            
                            this[currentElement] = Energy.Base.Cast.As<T>((object)reader.Value);
                            break;

                        case XmlNodeType.EndElement:
                            if (parentList.Count == 0)
                                currentElement = null;
                            else
                            {
                                int index = parentList.Count - 1;
                                currentElement = parentList[index];
                                parentList.RemoveAt(index);
                            }
                            break;
                    }
                }
            }

            private string XmlEscapeName(string value)
            {
                if (value.Contains(_XmlParentSeparator))
                    return value.Replace(_XmlParentSeparator, _XmlEscapeString);
                return value;
            }

            public void WriteXml(XmlWriter writer)
            {
                string[] array = ToStringArray();
                for (int i = 0; i < array.Length; i += 2)
                {
                    int v = i + 1;
                    writer.WriteStartElement(XmlEscapeName(array[i]));
                    if (array[v] != null)
                        writer.WriteString(array[v]);
                    writer.WriteEndElement();
                }
            }

            private string[] ToStringArray()
            {
                List<string> list = new List<string>();
                if (CaseSensitive)
                {
                    foreach (KeyValuePair<string, T> e in this)
                    {
                        list.Add(e.Key);
                        list.Add(Energy.Base.Cast.AsString((object)e.Value));
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, string> x in this.Index)
                    {
                        list.Add(x.Key);
                        list.Add(Energy.Base.Cast.AsString((object)this[x.Value]));
                    }
                }
                return list.ToArray();
            }
        }

        public class StringDictionary : StringDictionary<string>
        {
        }

        #endregion
    }
}
