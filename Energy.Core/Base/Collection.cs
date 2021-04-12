using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;

namespace Energy.Base
{
    public class Collection
    {
        #region Array

        /// <summary>
        /// Thread safe array of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public class Array<T> : System.Collections.Generic.List<T>
        {
            private readonly object _Lock = new object();

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

            public static bool IsNullOrEmpty(Array<T> array)
            {
                return null == array || 0 == array.Count;
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

            /// <summary>
            /// Exclude every element from source array.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="exclude"></param>
            /// <returns></returns>
            public static T[] Exclude(T[] array, T[] exclude)
            {
                if (null == array || null == exclude || 0 == array.Length || 0 == exclude.Length)
                {
                    return array;
                }
                int capacity = array.Length;
                if (capacity > 100)
                {
                    capacity = 100;
                }
                List<T> list = capacity > 1 ? new List<T>(capacity) : new List<T>();
                for (int i = 0; i < array.Length; i++)
                {
                    bool found = false;
                    for (int n = 0; n < exclude.Length; n++)
                    {
                        if (array[i].Equals(exclude[n]))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        continue;
                    }
                    list.Add(array[i]);
                }
                return list.ToArray();
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

            public int Count
            {
                get
                {
                    return _Array.Length;
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

            public static bool IsNullOrEmpty(string[] array)
            {
                return null == array || 0 == array.Length;
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

            /// <summary>
            /// Remove from first array every element from second array by comparisation.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="exclude"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static string[] Exclude(string[] array, string[] exclude, bool ignoreCase)
            {
                if (null == array || null == exclude || 0 == array.Length || 0 == exclude.Length)
                {
                    return array;
                }
                int capacity = array.Length;
                if (capacity > 100)
                {
                    capacity = 100;
                }
                List<string> list = capacity > 1 ? new List<string>(capacity) : new List<string>();
                for (int i = 0; i < array.Length; i++)
                {
                    bool found = false;
                    for (int n = 0; n < exclude.Length; n++)
                    {
                        if (0 == string.Compare(array[i], exclude[n], ignoreCase))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        continue;
                    }
                    list.Add(array[i]);
                }
                return list.ToArray();
            }

            #region IndexOf

            public int IndexOf(string element)
            {
                return IndexOf(element, 0, false);
            }

            public int IndexOf(string element, int index)
            {
                return IndexOf(element, index, false);
            }

            public int IndexOf(string element, bool ignoreCase)
            {
                return IndexOf(element, 0, ignoreCase);
            }

            public int IndexOf(string element, bool ignoreCase, int index)
            {
                return IndexOf(element, index, ignoreCase);
            }

            public int IndexOf(string element, int index, bool ignoreCase)
            {
                int length;
                if (null == _Array || 0 == (length = _Array.Length))
                {
                    return -1;
                }
                for (int i = index; i < length; i++)
                {
                    if (0 == string.Compare(element, _Array[i], ignoreCase))
                    {
                        return i;
                    }
                }
                return -1;
            }

            public static int IndexOf(string[] array, string element)
            {
                return IndexOf(array, element, 0, false);
            }

            public static int IndexOf(string[] array, string element, int index)
            {
                return IndexOf(array, element, index, false);
            }

            public static int IndexOf(string[] array, string element, bool ignoreCase)
            {
                return IndexOf(array, element, 0, ignoreCase);
            }

            public static int IndexOf(string[] array, string element, bool ignoreCase, int index)
            {
                return IndexOf(array, element, index, ignoreCase);
            }

            public static int IndexOf(string[] array, string element, int index, bool ignoreCase)
            {
                if (null == array || 0 == array.Length)
                {
                    return -1;
                }
                int length = array.Length;
                for (int i = index; i < length; i++)
                {
                    if (0 == string.Compare(element, array[i], ignoreCase))
                    {
                        return i;
                    }
                }
                return -1;
            }

            #endregion

            #region ToCSharpObject

            public static string ToCSharpObject(string[] array, string name)
            {
                if (null == name)
                {
                    name = "string[] stringArray";
                }
                string code = "";
                if (0 < name.Length)
                {
                    code += name + " = ";
                }
                code += "new string[] { ";
                var t1 = new List<string>();
                for (int i = 0, l = array.Length; i < l; i++)
                {
                    string s = array[i];
                    if (0 < s.Length)
                    {
                        s = s
                            .Replace("\\", "\\\\")
                            .Replace("\"", "\\\"")
                            ;
                    }
                    s = "\"" + s + "\"";
                    t1.Add(s);
                }
                code += string.Join(", ", t1.ToArray());
                code += " };";
                return code;
            }

            #endregion
        }

        #endregion

        #region StringDictionary

        public class StringDictionary<T> : Dictionary<string, T>, IXmlSerializable
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
            private Dictionary<string, string> Index = null;

            private Energy.Enumeration.MultipleBehaviour _SelectionOfDuplicates = Energy.Enumeration.MultipleBehaviour.Last;

            /// <summary>
            /// Specifies behaviour for selecting one element from multiple duplicates
            /// when case sensitive option is set to false.
            /// </summary>
            public Energy.Enumeration.MultipleBehaviour SelectionOfDuplicates
            {
                get { lock (_Lock) return _SelectionOfDuplicates; }
                set { lock (_Lock) _SelectionOfDuplicates = value; }
            }

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
                        {
                            return;
                        }
                        _CaseSensitive = value;
                        if (value)
                        {
                            Index = null;
                        }
                        else
                        {
                            RebuildIndex();
                        }
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
                    string map = key.ToUpper(CultureInfo.InvariantCulture);
                    if (_SelectionOfDuplicates == Energy.Enumeration.MultipleBehaviour.Last)
                    {
                        Index[map] = key;
                    }
                    else if (!Index.ContainsKey(map))
                    {
                        Index.Add(map, key);
                    }
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
                {
                    return default(T);
                }

                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        if (base.ContainsKey(key))
                        {
                            return base[key];
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                    else
                    {
                        string map = key.ToUpper(CultureInfo.InvariantCulture);
                        if (Index != null && Index.Count == 0 && base.Count > 0)
                        {
                            Index = null;
                        }
                        if (Index == null)
                        {
                            RebuildIndex();
                        }
                        if (Index != null && Index.ContainsKey(map))
                        {
                            return base[Index[map]];
                        }
                        else
                        {
                            return default(T);
                        }
                    }
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
                {
                    return null;
                }

                lock (_Lock)
                {
                    if (CaseSensitive)
                    {
                        base[key] = value;
                        return this;
                    }
                    string map = key.ToUpper(CultureInfo.InvariantCulture);
                    if (Index != null && Index.ContainsKey(map))
                    {
                        string link = Index[map];
                        base[link] = value;
                    }
                    else
                    {
                        base[key] = value;
                        if (Index == null)
                        {
                            Index = new Dictionary<string, string>();
                        }
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
                if (Energy.Base.Text.Contains(value, _XmlParentSeparator))
                {
                    return value.Replace(_XmlParentSeparator, _XmlEscapeString);
                }
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
                    {
                        return base.ContainsKey(key);
                    }
                    if (Index == null)
                    {
                        return false;
                    }
                    string map = key.ToUpper(CultureInfo.InvariantCulture);
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
            public StringDictionary<T> Filter(Energy.Enumeration.MatchStyle matchStyle, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, string[] filters)
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
            public StringDictionary<T> Filter(Energy.Enumeration.MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                return Filter(Energy.Enumeration.MatchStyle.Any, matchMode, ignoreCase, filters);
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
            public new StringDictionary Filter(Energy.Enumeration.MatchStyle matchStyle, Energy.Enumeration.MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                StringDictionary<string> filteredDictionary = base.Filter(Energy.Enumeration.MatchStyle.Any, matchMode, ignoreCase, filters);
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
            public new StringDictionary Filter(Energy.Enumeration.MatchMode matchMode, bool ignoreCase, string[] filters)
            {
                return Filter(Energy.Enumeration.MatchStyle.Any, matchMode, ignoreCase, filters);
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

            public int IndexOf(string element, bool ignoreCase)
            {
                return IndexOf(element, 0, ignoreCase);
            }

            public int IndexOf(string element, bool ignoreCase, int index)
            {
                return IndexOf(element, index, ignoreCase);
            }

            public int IndexOf(string element, int index, bool ignoreCase)
            {
                int length;
                if (0 == (length = this.Count))
                {
                    return -1;
                }
                for (int i = index; i < length; i++)
                {
                    if (0 == string.Compare(element, this[i], ignoreCase))
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        #endregion

        #region SerializableDictionary

        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
            , System.Xml.Serialization.IXmlSerializable
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

        #region Static utility functions

        #region IsNullOrEmpty

        /// <summary>
        /// Returns true if array is null or empty.
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="array">Array of elements</param>
        /// <returns>True if array is null or empty</returns>
        public static bool IsNullOrEmpty<T>(T[] array)
        {
            return null == array || 0 == array.Length;
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

        #region Compare

        public static int Compare(string[] left, string[] right, bool ignoreCase)
        {
            return StringArray.Compare(left, right, ignoreCase);
        }

        public static int Compare(string[] left, string[] right)
        {
            return StringArray.Compare(left, right, false);
        }

        #endregion

        #region Same

        public static bool Same(string[] left, string[] right, bool ignoreCase)
        {
            return 0 == StringArray.Compare(left, right, ignoreCase);
        }

        public static bool Same(string[] left, string[] right)
        {
            return 0 == StringArray.Compare(left, right, false);
        }

        #endregion

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

        #region

        public class Table
        {
            public class Column<TKey, TValue> : Energy.Base.Collection.Table<TKey, TValue>.Column
            {
            }


            #region Row

            //public class Row<TKey, TValue> : Energy.Base.Collection.Table<TKey, TValue>.Row
            //{
            //}

            public class Row<TKey, TValue> : Energy.Interface.IRow<TKey, TValue>
            {
                private IList<TValue> _List = new List<TValue>();

                private IDictionary<TKey, int> _Index = new Dictionary<TKey, int>();

                public TValue this[TKey key]
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

                public TValue this[int index]
                {
                    get 
                    {
                        return Get(index);
                    }
                    set 
                    {
                        Set(index, value);
                    }
                }

                public TValue New()
                {
                    TValue o = default(TValue);
                    _List.Add(o);
                    return o;
                }

                public TValue Get(int index)
                {
                    if (index < 0 || index < _List.Count)
                    {
                        return default(TValue);
                    }
                    else
                    {
                        return _List[index];
                    }
                }

                public TValue Get(TKey key)
                {
                    if (!_Index.ContainsKey(key))
                    {
                        return default(TValue);
                    }
                    else
                    {
                        return _List[_Index[key]];
                    }
                }

                public Energy.Interface.IRow<TKey, TValue> Append(TKey key, TValue value)
                {
                    int index = _List.Count;
                    _List.Add(value);
                    _Index[key] = index;
                    return this;
                }

                public Energy.Interface.IRow<TKey, TValue> Set(TKey key, TValue value)
                {
                    if (_Index.ContainsKey(key))
                    {
                        _List[_Index[key]] = value;
                    }
                    else
                    {
                        Append(key, value);
                    }
                    return this;
                }

                public Energy.Interface.IRow<TKey, TValue> Set(int index, TValue value)
                {
                    if (index < 0 || index < _List.Count)
                    {
                        return this;
                    }
                    else
                    {
                        _List[index] = value;
                    }
                    return this;
                }

                public override string ToString()
                {
                    List<string> l = new List<string>();
                    foreach (TKey key in _Index.Keys)
                    {
                        TValue value = _List[_Index[key]];
                        string k = Energy.Base.Text.AddSlashes(Energy.Base.Cast.ObjectToString(key));
                        string v = Energy.Base.Text.AddSlashes(Energy.Base.Cast.ObjectToString(value));
                        l.Add("\"" + k + "\": \"" + v + "\"");
                    }
                    string r = string.Join(", ", l.ToArray());
                    return r;
                }
            }

            #endregion
        }

        public class Table<TValue> : Table<object, TValue>
        {

        }

        public class Table<TKey, TValue> : Energy.Interface.ITable<TKey, TValue>
            , IList<Energy.Interface.IRow<TKey, TValue>>
        {
            public class Column : Energy.Interface.IColumn<TKey, TValue>
            {
                public string Name { get; set; }

                public int Index { get; set; }
            }

            private IList<Energy.Interface.IRow<TKey, TValue>> _Rows = new List<Energy.Interface.IRow<TKey, TValue>>();

            private IList<Energy.Interface.IColumn<TKey, TValue>> _Columns = new List<Energy.Interface.IColumn<TKey, TValue>>();

            //public IRow<TKey, TValue> this[int index]
            //{
            //    get => throw new NotImplementedException();
            //    set => throw new NotImplementedException();
            //}

            //public IRow<TKey, TValue> this[TKey key]
            //{
            //    get => throw new NotImplementedException();
            //    set => throw new NotImplementedException();
            //}

            public Energy.Interface.IRow<TKey, TValue> this[int index]
            {
                get 
                {
                    return GetByIndex(index);
                }
                set 
                {
                    SetByIndex(index, value);
                }
            }

            public IList<Energy.Interface.IRow<TKey, TValue>> Rows
            {
                get 
                {
                    return _Rows;
                }
            }

            public IList<Energy.Interface.IColumn<TKey, TValue>> Columns
            {
                get 
                {
                    return _Columns;
                }
            }

            public int Count 
            {
                get 
                {
                    return _Rows.Count;
                }
            }

            public bool IsReadOnly 
            {
                get
                {
                    return false;
                }
            }

            public static bool IsNullOrEmpty(Table<TKey, TValue> array)
            {
                return null == array || 0 == array.Count;
            }

            public void Add(Energy.Interface.IRow<TKey, TValue> item) 
            {
                _Rows.Add(item);
            }

            public void Clear()
            {
                _Rows.Clear();
            }

            public bool Contains(Energy.Interface.IRow<TKey, TValue> item) 
            {
                return _Rows.Contains(item);
            }

            public void CopyTo(Energy.Interface.IRow<TKey, TValue>[] array, int arrayIndex) 
            {
                _Rows.CopyTo(array, arrayIndex);
            }

            public IEnumerator<Energy.Interface.IRow<TKey, TValue>> GetEnumerator() 
            {
                return _Rows.GetEnumerator();
            }

            public int IndexOf(Energy.Interface.IRow<TKey, TValue> item) 
            {
                return _Rows.IndexOf(item);
            }

            public void Insert(int index, Energy.Interface.IRow<TKey, TValue> item) 
            {
                Insert(index, item);
            }

            public bool Remove(Energy.Interface.IRow<TKey, TValue> item)
            {
                return _Rows.Remove(item);
            }

            public void RemoveAt(int index) 
            {
                _Rows.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator() 
            {
                return _Rows.GetEnumerator();
            }

            private Energy.Interface.IRow<TKey, TValue> GetByIndex(int index)
            {
                return _Rows[index];
            }

            private void SetByIndex(int index, Energy.Interface.IRow<TKey, TValue> value)
            {
                _Rows[index] = value;
            }

            public Energy.Interface.IRow<TKey, TValue> New()
            {
                Energy.Interface.IRow<TKey, TValue> o;
                o = new Energy.Base.Collection.Table.Row<TKey, TValue>();
                _Rows.Add(o);
                return o;
            }
        }

        #endregion
    }
}
