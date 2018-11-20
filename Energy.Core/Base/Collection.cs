﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Energy.Enumeration;
using Energy.Interface;
using Energy.Query;

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
                set
                {
                    lock (_Lock)
                    {
                        if (base.Count == 0)
                            base.Add(value);
                        else
                            base[0] = value;
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
                set
                {
                    lock (_Lock)
                    {
                        if (base.Count == 0)
                            base.Add(value);
                        else
                            this[base.Count - 1] = value;
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

            /// <summary>
            /// Get sub array from an existing one.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public static T[] SubArray(T[] array, int start, int count)
            {
                return SubArray(array, start, count, false);
            }

            /// <summary>
            /// Get sub array from an existing one.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <param name="pad"></param>
            /// <returns></returns>
            public static T[] SubArray(T[] array, int start, int count, bool pad)
            {
                if (array == null)
                    return null;
                if (start == 0 && array.Length == count)
                    return array;
                bool enough = array.Length >= start + count;
                if (!enough && !pad)
                    count = array.Length - start;
                T[] data = new T[count];
                if (!enough && pad)
                    count = array.Length - start;
                Array.Copy(array, start, data, 0, count);
                return data;
            }
        }

        #endregion

        #region GetFirstOrDefault

        public static T GetFirstOrDefault<T>(params T[][] array)
        {
            if (array == null || array.Length == 0)
                return default(T);
            for (int i = 0; i < array.Length; i++)
            {
                T[] sub = array[i];
                if (sub == null || sub.Length == 0)
                    continue;
                return sub[0];
            }
            return default(T);
        }

        #endregion

        #region Static utility functions

        public static TValue GetDictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (key == null)
                return default(TValue);
            if (dictionary == null || dictionary.Count == 0)
                return default(TValue);
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            else
                return default(TValue);
        }

        public static TValue GetStringDictionaryValue<TValue>(Dictionary<string, TValue> dictionary, string key, bool ignoreCase)
        {
            if (key == null)
                return default(TValue);
            if (dictionary == null || dictionary.Count == 0)
                return default(TValue);
            if (ignoreCase)
            {
                string[] keys = new string[dictionary.Count];
                dictionary.Keys.CopyTo(keys, 0);
                for (int i = 0; i < keys.Length; i++)
                {
                    if (0 == string.Compare(keys[i], key, true))
                        return dictionary[keys[i]];
                }
                return default(TValue);
            }
            else
            {
                if (dictionary.ContainsKey(key))
                    return dictionary[key];
                else
                    return default(TValue);
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
        public class Associative<T> : Dictionary<string, T>, Energy.Interface.IXmlSerializable
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

        public class StringArray : Energy.Interface.IStringList
        {
            private string[] _Array;

            public StringArray() { }

            public StringArray(string[] array)
            {
                _Array = array;
            }

            public StringArray(List<string> list)
            {
                if (list != null)
                {
                    _Array = list.ToArray();
                }
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

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <returns></returns>
            public bool HasDuplicates()
            {
                return HasDuplicates(_Array, false);
            }

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public bool HasDuplicates(bool ignoreCase)
            {
                return HasDuplicates(_Array, ignoreCase);
            }

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <param name="array"></param>
            /// <returns></returns>
            public static bool HasDuplicates(string[] array)
            {
                return HasDuplicates(array, false);
            }

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static bool HasDuplicates(string[] array, bool ignoreCase)
            {
                if (array == null || array.Length < 2)
                    return false;

                List<string> check = new List<string>();
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (i > 0 && check.Count > 0 && check.Contains(array[i]))
                        continue;
                    for (int j = i + 1; j < array.Length; j++)
                    {
                        if (0 == string.Compare(array[i], array[j], ignoreCase))
                            return true;
                    }
                    check.Add(array[i]);
                }
                return false;
            }

            /// <summary>
            /// Compare two string arrays.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static int Compare(string[] left, string[] right)
            {
                return Compare(left, right, false);
            }

            /// <summary>
            /// Compare two string arrays.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static int Compare(string[] left, string[] right, bool ignoreCase)
            {
                if (left == null || right == null)
                {
                    if (left == null && right == null)
                        return 0;
                    if (left == null)
                        return -1;
                    else
                        return 1;
                }
                if (left.Length < right.Length)
                    return -1;
                else if (left.Length > right.Length)
                    return 1;
                else
                {
                    for (int i = 0; i < left.Length; i++)
                    {
                        int result = string.Compare(left[i], right[i], ignoreCase);
                        if (result != 0)
                            return result;
                    }
                }
                return 0;
            }
        }

        #endregion

        #region StringDictionary

        public class StringDictionary<T> : Dictionary<string, T>, Energy.Interface.IXmlSerializable
        {
            #region Contructor

            public StringDictionary()
            {
            }

            public StringDictionary(params string[] keyValuePairArray)
            {
                for (int i = 0; i < keyValuePairArray.Length - 1; i++)
                {
                    string key = keyValuePairArray[i++];
                    string value = keyValuePairArray[i];
                    this[key] = Energy.Base.Cast.StringToObject<T>(value);
                }
            }

            public StringDictionary(params object[] keyValuePairArray)
            {
                for (int i = 0; i < keyValuePairArray.Length - 1; i++)
                {
                    string key = Energy.Base.Cast.ObjectToString(keyValuePairArray[i++]);
                    T value = Energy.Base.Cast.As<T>(keyValuePairArray[i]);
                    this[key] = value;
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
                return string.Join(Energy.Base.Text.NL, list.ToArray());
            }

            /// <summary>
            /// Get array of keys
            /// </summary>
            /// <returns></returns>
            public string[] GetKeyArray()
            {
                lock (_Lock)
                {
                    string[] array = new string[Count];
                    this.Keys.CopyTo(array, 0);
                    return array;
                }
            }

            /// <summary>
            /// Get array of values
            /// </summary>
            /// <returns></returns>
            public T[] GetValueArray()
            {
                lock (_Lock)
                {
                    T[] array = new T[Count];
                    this.Values.CopyTo(array, 0);
                    return array;
                }
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
                lock (_Lock)
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
                StringDictionary<string> filteredDictionary = base.Filter(MatchStyle.Any, matchMode, ignoreCase, filters);
                StringDictionary dictionary = new StringDictionary();
                foreach (KeyValuePair<string, string> pair in filteredDictionary)
                {
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

        public class StringList : System.Collections.Generic.List<string>, Energy.Interface.IStringList
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

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <returns></returns>
            public bool HasDuplicates()
            {
                return HasDuplicates(this, false);
            }

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public bool HasDuplicates(bool ignoreCase)
            {
                return HasDuplicates(this, ignoreCase);
            }

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <param name="array"></param>
            /// <returns></returns>
            public static bool HasDuplicates(System.Collections.Generic.List<string> array)
            {
                return HasDuplicates(array, false);
            }

            /// <summary>
            /// Check if list contains any duplicates.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static bool HasDuplicates(System.Collections.Generic.List<string> array, bool ignoreCase)
            {
                if (array == null || array.Count < 2)
                    return false;

                List<string> check = new List<string>();
                for (int i = 0; i < array.Count - 1; i++)
                {
                    if (i > 0 && check.Count > 0 && check.Contains(array[i]))
                        continue;
                    for (int j = i + 1; j < array.Count; j++)
                    {
                        if (0 == string.Compare(array[i], array[j], ignoreCase))
                            return true;
                    }
                    check.Add(array[i]);
                }
                return false;
            }
        }

        #endregion

        #region SerializableDictionary

        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, Energy.Interface.IXmlSerializable
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

        #region Circular

        public class Circular<T> : IList<T>, Energy.Interface.IArray<T>, Energy.Interface.IStack<T>
        {
            private List<T> _List = new List<T>();

            public T this[int index]
            {
                get
                {
                    return _List[index];
                }
                set
                {
                    _List[index] = value;
                }
            }

            public int Count { get { return _List.Count; } set { SetCount(value); } }

            private void SetCount(int value)
            {
                if (value <= 0)
                {
                    _List.Clear();
                }
                else if (value < _List.Count)
                {
                    _List.RemoveRange(value, _List.Count - value);
                }
                else
                {
                    while (_List.Count < value)
                    {
                        _List.Add(default(T));
                    }
                }
            }

            public bool IsReadOnly { get { return false; } }

            public T First
            {
                get
                {
                    return _List.Count > 0 ? _List[0] : default(T);
                }
                set
                {
                    if (_List.Count == 0)
                        _List.Add(value);
                    else
                        _List[0] = value;
                }
            }

            public T Last
            {
                get
                {
                    return _List.Count > 0 ? _List[_List.Count - 1] : default(T);
                }
                set
                {
                    if (_List.Count == 0)
                        _List.Add(value);
                    else
                        _List[_List.Count - 1] = value;
                }
            }

            private int _Limit = 0;

            public int Limit { get { return GetLimit(); } set { SetLimit(value); } }

            private int GetLimit()
            {
                return _Limit;
            }

            private void SetLimit(int value)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Limit must be a positive number");
                _Limit = value;
            }

            public void Add(T item)
            {
                _List.Add(item);
                Scroll();
            }

            public void Clear()
            {
                _List.Clear();
            }

            public bool Contains(T item)
            {
                return _List.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _List.CopyTo(array, arrayIndex);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _List.GetEnumerator();
            }

            public int IndexOf(T item)
            {
                return _List.IndexOf(item);
            }

            public void Insert(int index, T item)
            {
                _List.Insert(index, item);
                Scroll();
            }

            public bool Remove(T item)
            {
                return _List.Remove(item);
            }

            public void RemoveAt(int index)
            {
                _List.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _List.GetEnumerator();
            }

            private void Scroll()
            {
                if (_Limit == 0)
                {
                    return;
                }
                if (_List.Count > _Limit)
                {
                    _List.RemoveRange(0, _List.Count - Limit);
                }
            }

            public T[] ToArray()
            {
                return _List.ToArray();
            }

            public void Push(T item)
            {
                Add(item);
            }

            public T Pull()
            {
                if (_List.Count == 0)
                    return default(T);

                T item = _List[0];
                _List.RemoveAt(0);
                return item;
            }
        }

        #endregion

        #region KeyValuePairList

        public class KeyValuePairList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
        {
            public bool Empty { get { return IsEmpty(); } }

            private bool IsEmpty()
            {
                return this.Count == 0;
            }

            public KeyValuePair<TKey, TValue> Take()
            {
                if (this.Count == 0)
                {
                    return default(KeyValuePair<TKey, TValue>);
                }
                KeyValuePair<TKey, TValue> result = this[0];
                this.RemoveAt(0);
                return result;
            }
        }

        #endregion
    }
}
