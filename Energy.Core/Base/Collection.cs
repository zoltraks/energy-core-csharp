using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Energy.Enumeration;

namespace Energy.Base
{
    public class Collection
    {
        #region Array

        /// <summary>
        /// Thread safe array of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public class Array<T> : System.Collections.Generic.List<T>
        {
            private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

            /// <summary>
            /// Gets or sets the element at the specified index
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public new T this[int index]
            {
                get
                {
                    lock (_Lock)
                    {
                        return base[index];
                    }
                }
                set
                {
                    lock (_Lock)
                    {
                        base[index] = value;
                    }
                }
            }

            /// <summary>
            /// Removes all elements from the list
            /// </summary>
            public new void Clear()
            {
                lock (_Lock)
                {
                    base.Clear();
                }
            }

            /// <summary>
            /// Gets or sets the number of elements
            /// </summary>
            /// <remarks>
            /// When setting to number which is higher than current capacity, default elements will be added to list
            /// </remarks>
            public new int Count
            {
                get
                {
                    lock (_Lock)
                    {
                        return base.Count;
                    }
                }
                set
                {
                    lock (_Lock)
                    {
                        if (value == 0)
                        {
                            if (base.Count > 0)
                            {
                                base.Clear();
                            }
                            return;
                        }
                        if (value == base.Count)
                        {
                            return;
                        }
                        else if (value > base.Count)
                        {
                            while (value > base.Count)
                            {
                                base.Add(default(T));
                            }
                        }
                        else
                        {
                            base.RemoveRange(value, base.Count - value);
                        }
                    }
                }
            }

            /// <summary>
            /// Return first element or default if list is empty
            /// </summary>
            public T First
            {
                get
                {
                    lock (_Lock)
                    {
                        return base.Count == 0 ? default(T) : base[0];
                    }
                }
            }

            /// <summary>
            /// Return last element or default if list is empty
            /// </summary>
            public T Last
            {
                get
                {
                    lock (_Lock)
                    {
                        return base.Count == 0 ? default(T) : base[base.Count - 1];
                    }
                }
            }

            /// <summary>
            /// Create and add new element to list and return
            /// </summary>
            /// <returns></returns>
            public T New()
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                lock (_Lock)
                {
                    base.Add(item);
                }
                return item;
            }

            public bool Equals(Array<T> array)
            {
                if (Count != array.Count)
                {
                    return false;
                }
                lock (_Lock)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (!this[i].Equals(array[i]))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public new T Add(T item)
            {
                lock (_Lock)
                {
                    base.Add(item);
                    return item;
                }
            }

            public new T Insert(int index, T item)
            {
                lock (_Lock)
                {
                    base.Insert(index, item);
                    return item;
                }
            }

            public new void Remove(T item)
            {
                lock (_Lock)
                {
                    base.Remove(item);
                }
            }

            public new void RemoveAt(int index)
            {
                lock (_Lock)
                {
                    base.RemoveAt(index);
                }
            }

            public new void RemoveRange(int index, int count)
            {
                lock (_Lock)
                {
                    base.RemoveRange(index, count);
                }
            }

            public new void RemoveAll(Predicate<T> match)
            {
                lock (_Lock)
                {
                    base.RemoveAll(match);
                }
            }

            public static Array<T> operator +(Array<T> left, T right)
            {
                left.Add(right);
                return left;
            }

            public static Array<T> operator -(Array<T> left, T right)
            {
                left.Remove(right);
                return left;
            }

            public new T[] ToArray()
            {
                lock (_Lock)
                {
                    return base.ToArray();
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

        #region StringArray

        public class StringArray
        {
            private string[] _Array;

            public StringArray() { }

            public StringArray(string[] array)
            {
                _Array = array;
            }

            public int TotalLength
            {
                get
                {
                    return GetTotalLength(_Array);
                }
            }

            public static int GetTotalLength(string[] array)
            {
                if (array == null || array.Length == 0)
                    return 0;
                int length = 0;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == null)
                        continue;
                    length += array[i].Length;
                }
                return length;
            }
        }

        #endregion

        #region StringDictionary

        public class StringDictionary<T> : Dictionary<string, T>, IXmlSerializable
        {
            #region Contructor

            public StringDictionary()
            {
            }

            public StringDictionary(string[] keyValuePairArray)
            {
                for (int i = 0; i < keyValuePairArray.Length - 1; i++)
                {
                    string key = keyValuePairArray[i++];
                    string value = keyValuePairArray[i];
                    this[key] = Energy.Base.Cast.StringToObject<T>(value);
                }
            }

            #endregion

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
                    lock (_Lock)
                    {
                        return _CaseSensitive;
                    }
                }
                set
                {
                    lock (_Lock)
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
            }

            private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

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

            #region Get

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

            public Energy.Base.Collection.StringDictionary<T> Get(string key, out T value)
            {
                value = Get(key);
                return this;
            }

            #endregion

            #region Set

            public Energy.Base.Collection.StringDictionary<T> Set(string key, T value)
            {
                if (string.IsNullOrEmpty(key))
                    return null;

                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        base[key] = value;
                        return this;
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

                return this;
            }

            public Energy.Base.Collection.StringDictionary<T> Set(string[] array)
            {
                lock (_Lock)
                {
                    for (int i = 0; i - 1 < array.Length; i += 2)
                    {
                        Set(array[i], Energy.Base.Cast.StringToObject<T>(array[i + 1]));
                    }
                }
                return this;
            }

            #endregion

            public new void Add(string key, T value)
            {
                Set(key, value);
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

            public object[] ToObjectArray()
            {
                List<object> list = new List<object>();
                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        foreach (string key in base.Keys)
                        {
                            list.Add((object)key);
                            list.Add((object)base[key]);
                        }
                    }
                    else
                    {
                        foreach (string indexKey in Index.Keys)
                        {
                            string key = Index[indexKey];
                            list.Add((object)key);
                            list.Add((object)base[key]);
                        };
                    }
                    return list.ToArray();
                }
            }

            public object[] ToArray<TToArray>()
            {
                return ToObjectArray();
            }

            public string[] ToArray()
            {
                List<string> list = new List<string>();
                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        foreach (string key in base.Keys)
                        {
                            list.Add(key);
                            list.Add(Energy.Base.Cast.ObjectToString(base[key]));
                        }
                    }
                    else
                    {
                        foreach (string key in Index.Keys)
                        {
                            list.Add(Index[key]);
                            list.Add(Energy.Base.Cast.ObjectToString(base[Index[key]]));
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
                        list.Add(Energy.Base.Cast.ObjectToString((object)e.Value));
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, string> x in this.Index)
                    {
                        list.Add(x.Key);
                        list.Add(Energy.Base.Cast.ObjectToString((object)this[x.Value]));
                    }
                }
                return list.ToArray();
            }

            public new bool ContainsKey(string key)
            {
                lock (_Lock)
                {
                    if (CaseSensitive)
                        return base.ContainsKey(key);
                    if (Index == null)
                        return false;
                    string map = key.ToUpperInvariant();
                    return Index.ContainsKey(map);
                }
            }

            /// <summary>
            /// Return array as multiline string containing key value pairs
            /// concatenated with glue string.
            /// </summary>
            /// <param name="glue"></param>
            /// <returns></returns>
            public string ToString(string glue)
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                foreach (KeyValuePair<string, T> e in this)
                {
                    list.Add(string.Concat(e.Key, glue, Energy.Base.Cast.ObjectToString(e.Value)));
                }
                return string.Join(Environment.NewLine, list.ToArray());
            }

            /// <summary>
            /// Filter out dictionary by one or more filters
            /// using specified matching style and mode.
            /// </summary>
            /// <param name="matchStyle"></param>
            /// <param name="matchMode"></param>
            /// <param name="ignoreCase"></param>
            /// <param name="filters"></param>
            /// <returns></returns>
            public StringDictionary<T> Filter(MatchStyle matchStyle, MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                StringDictionary<T> dictionary = new StringDictionary<T>();
                foreach (KeyValuePair<string, T> pair in this)
                {
                    if (!Energy.Base.Text.Check(pair.Key, matchStyle, matchMode, ignoreCase, filters))
                        continue;
                    dictionary.Add(pair.Key, pair.Value);
                }
                return dictionary;
            }

            /// <summary>
            /// Filter out dictionary by one or more filters
            /// using specified matching mode.
            /// </summary>
            /// <param name="matchMode"></param>
            /// <param name="ignoreCase"></param>
            /// <param name="filters"></param>
            /// <returns></returns>
            public StringDictionary<T> Filter(MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                return Filter(MatchStyle.Any, matchMode, ignoreCase, filters);
            }
        }

        [Serializable]
        public class StringDictionary : StringDictionary<string>
        {
            /// <summary>
            /// Filter out dictionary by one or more filters
            /// using specified matching style and mode.
            /// </summary>
            /// <param name="matchStyle"></param>
            /// <param name="matchMode"></param>
            /// <param name="ignoreCase"></param>
            /// <param name="filters"></param>
            /// <returns></returns>
            public new StringDictionary Filter(MatchStyle matchStyle, MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                StringDictionary dictionary = new StringDictionary();
                foreach (KeyValuePair<string, string> pair in this)
                {
                    if (!Energy.Base.Text.Check(pair.Key, matchStyle, matchMode, ignoreCase, filters))
                        continue;
                    dictionary.Add(pair.Key, pair.Value);
                }
                return dictionary;
            }

            /// <summary>
            /// Filter out dictionary by one or more filters
            /// using specified matching mode.
            /// </summary>
            /// <param name="matchMode"></param>
            /// <param name="ignoreCase"></param>
            /// <param name="filters"></param>
            /// <returns></returns>
            public new StringDictionary Filter(MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                return Filter(MatchStyle.Any, matchMode, ignoreCase, filters);
            }
        }

        #endregion

        #region StringList

        public class StringList : System.Collections.Generic.List<string>
        {
            public int TotalLength
            {
                get
                {
                    return GetTotalLength(this);
                }
            }

            public static int GetTotalLength(System.Collections.Generic.List<string> list)
            {
                if (list == null || list.Count == 0)
                    return 0;
                int length = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == null)
                        continue;
                    length += list[i].Length;
                }
                return length;
            }
        }

        #endregion

        #region SerializableDictionary

        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
        {
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                bool wasEmpty = reader.IsEmptyElement;
                reader.Read();

                if (wasEmpty)
                    return;

                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("Item");

                    reader.ReadStartElement("Key");
                    TKey key = Energy.Base.Cast.StringToObject<TKey>(reader.ReadString());
                    reader.ReadEndElement();

                    reader.ReadStartElement("Value");
                    TValue value = Energy.Base.Cast.StringToObject<TValue>(reader.ReadString());
                    reader.ReadEndElement();

                    this.Add(key, value);

                    reader.ReadEndElement();
                    reader.MoveToContent();
                }

                reader.ReadEndElement();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

                foreach (TKey key in this.Keys)
                {
                    writer.WriteStartElement("Item");

                    writer.WriteStartElement("Key");
                    writer.WriteString(Energy.Base.Cast.ObjectToString(key));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Value");
                    TValue value = this[key];
                    writer.WriteString(Energy.Base.Cast.ObjectToString(value));
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
        }

        #endregion
    }
}
