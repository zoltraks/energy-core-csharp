using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Energy.Base
{
    public class Class
    {
        /// <summary>
        /// Get list of names of all fields and propeties for specified class type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includePrivate"></param>
        /// <param name="includePublic"></param>
        /// <returns></returns>
        public static string[] GetFieldsAndProperties(Type type, bool includePrivate, bool includePublic)
        {
            List<string> list = new List<string>();

            foreach (FieldInfo _ in type.GetFields())
            {
                if (!includePrivate && _.IsPrivate)
                    continue;
                if (!includePublic && _.IsPublic)
                    continue;
                list.Add(_.Name);
            }
            BindingFlags f = BindingFlags.Instance;
            if (includePrivate)
                f |= BindingFlags.NonPublic;
            if (includePublic)
                f |= BindingFlags.Public;
            foreach (PropertyInfo _ in type.GetProperties(f))
            {
                list.Add(_.Name);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Get list of names of all fields and propeties for specified class type.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns>Array of string</returns>
        public static string[] GetFieldsAndProperties(Type type)
        {
            return GetFieldsAndProperties(type, true, true);
        }

        /// <summary>
        /// Get list of names of all fields and propeties for specified class type.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <param name="includePrivate"></param>
        /// <returns>Array of string</returns>
        public static string[] GetFieldsAndProperties(Type type, bool includePrivate)
        {
            return GetFieldsAndProperties(type, includePrivate, true);
        }

        /// <summary>
        /// Get attribute for a field or property of desired class.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <param name="name">Field or property name</param>
        /// <param name="attribute">Attribute class type</param>
        /// <returns>Attribute or null if not found</returns>
        public static object GetFieldOrPropertyAttribute(Type type, string name, Type attribute)
        {
            foreach (FieldInfo _ in type.GetFields())
            {
                if (_.Name != name) continue;
                object[] a = _.GetCustomAttributes(true);
                foreach (object x in a)
                {
                    if (x.GetType() == attribute)
                    {
                        return x;
                    }
                }
            }
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo _ in type.GetProperties(bf))
            {
                if (_.Name != name) continue;
                object[] a = _.GetCustomAttributes(true);
                foreach (object x in a)
                {
                    if (x.GetType() == attribute)
                    {
                        return x;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get custom attributes of field or property of a class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="filter"></param>
        /// <param name="inherit"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static object[] GetFieldOrPropertyAttributes(Type type, string field, Type filter, bool inherit, bool ignoreCase)
        {
            List<object> list = new List<object>();
            foreach (FieldInfo _ in type.GetFields())
            {
                if (0 != string.Compare(_.Name, field, ignoreCase))
                    continue;
                object[] a = _.GetCustomAttributes(inherit);
                foreach (object x in a)
                {
                    if (filter != null)
                    {
                        if (x.GetType() != filter)
                            continue;
                    }
                    list.Add(x);
                }
            }
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo _ in type.GetProperties(bf))
            {
                if (0 != string.Compare(_.Name, field, ignoreCase))
                    continue;
                object[] a = _.GetCustomAttributes(inherit);
                foreach (object x in a)
                {
                    if (filter != null)
                    {
                        if (x.GetType() != filter)
                            continue;
                    }
                    list.Add(x);
                }
            }
            return list.Count == 0 ? null : list.ToArray();
        }

        /// <summary>
        /// Get custom attributes of field or property of a class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="inherit"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T[] GetFieldOrPropertyAttributes<T>(Type type, string field, bool inherit, bool ignoreCase)
        {
            Type filter = typeof(T);
            List<T> list = new List<T>();
            foreach (FieldInfo _ in type.GetFields())
            {
                if (0 != string.Compare(_.Name, field, ignoreCase))
                    continue;
                object[] a = _.GetCustomAttributes(inherit);
                foreach (object x in a)
                {
                    if (filter != null)
                    {
                        if (x.GetType() != filter)
                            continue;
                    }
                    list.Add((T)x);
                }
            }
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo _ in type.GetProperties(bf))
            {
                if (0 != string.Compare(_.Name, field, ignoreCase))
                    continue;
                object[] a = _.GetCustomAttributes(inherit);
                foreach (object x in a)
                {
                    if (filter != null)
                    {
                        if (x.GetType() != filter)
                            continue;
                    }
                    list.Add((T)x);
                }
            }
            return list.Count == 0 ? null : list.ToArray();
        }

        /// <summary>
        /// Get custom attributes of field or property of a class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object[] GetFieldOrPropertyAttributes(Type type, string field, Type filter)
        {
            return GetFieldOrPropertyAttributes(type, field, filter, true, false);
        }

        /// <summary>
        /// Find field in a class optionally including private or return null if field was not found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="includePrivate"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static FieldInfo FindFieldInfo(Type type, string name, bool includePrivate, bool ignoreCase)
        {
            FieldInfo[] search = type.GetFields();
            for (int i = 0; i < search.Length; i++)
            {
                FieldInfo _ = search[i];
                if (0 == string.Compare(_.Name, name, ignoreCase))
                {
                    if (!includePrivate && _.IsPrivate)
                        return null;
                    return _;
                }
            }
            return null;
        }

        /// <summary>
        /// Find property in a class optionally including private or return null if field was not found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="includePrivate"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static PropertyInfo FindPropertyInfo(Type type, string name, bool includePrivate, bool ignoreCase)
        {
            BindingFlags f = BindingFlags.Instance | BindingFlags.Public;
            if (includePrivate)
                f |= BindingFlags.NonPublic;
            PropertyInfo[] search = type.GetProperties(f);
            for (int i = 0; i < search.Length; i++)
            {
                PropertyInfo _ = search[i];
                if (0 == string.Compare(_.Name, name, ignoreCase))
                    return _;
            }
            return null;
        }

        /// <summary>
        /// Get field value of object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <param name="includePrivate"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static object GetFieldValue(object o, string name, bool includePrivate, bool ignoreCase)
        {
            if (o == null)
                return null;
            Type type = o.GetType();
            FieldInfo _ = FindFieldInfo(type, name, includePrivate, ignoreCase);
            if (_ == null)
                return null;
            return _.GetValue(o);
        }

        /// <summary>
        /// Get property value of object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <param name="includePrivate"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object o, string name, bool includePrivate, bool ignoreCase, object index)
        {
            if (o == null)
                return null;
            Type type = o.GetType();
            PropertyInfo _ = FindPropertyInfo(type, name, includePrivate, ignoreCase);
            if (_ == null)
                return null;
            if (index == null)
                return _.GetValue(o, null);
            else
                return _.GetValue(o, new object[] { index });
        }

        /// <summary>
        /// Get field or property value of object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <param name="includePrivate"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static object GetFieldOrPropertyValue(object o, string name, bool includePrivate, bool ignoreCase)
        {
            if (o == null)
                return null;
            Type type = o.GetType();
            FieldInfo field = FindFieldInfo(type, name, includePrivate, ignoreCase);
            if (field != null)
                return field.GetValue(o);
            PropertyInfo property = FindPropertyInfo(type, name, includePrivate, ignoreCase);
            if (property != null)
                return property.GetValue(o, null);
            return null;
        }

        /// <summary>
        /// Get field or property value of object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <param name="includePrivate"></param>
        /// <returns></returns>
        public static object GetFieldOrPropertyValue(object o, string name, bool includePrivate)
        {
            return GetFieldOrPropertyValue(o, name, includePrivate, false);
        }

        /// <summary>
        /// Get field or property value of object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetFieldOrPropertyValue(object o, string name)
        {
            return GetFieldOrPropertyValue(o, name, true, false);
        }

        /// <summary>
        /// Represent values of fields and properties of object as string array.
        /// If names should be included, array will be returned as a set of key and value pairs
        /// one by another.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="includePrivate"></param>
        /// <param name="includeName"></param>
        /// <returns></returns>
        public static string[] ObjectToStringArray(object o, bool includePrivate, bool includeName)
        {
            Type type = o.GetType();
            List<string> list = new List<string>();
            foreach (FieldInfo _ in type.GetFields())
            {
                if (!includePrivate && _.IsPrivate)
                    continue;
                if (includeName)
                    list.Add(_.Name);
                list.Add(Energy.Base.Cast.ObjectToString(_.GetValue(o)));
            }
            BindingFlags f = BindingFlags.Instance | BindingFlags.Public;
            if (includePrivate)
                f |= BindingFlags.NonPublic;
            foreach (PropertyInfo _ in type.GetProperties(f))
            {
                if (includeName)
                    list.Add(_.Name);
                list.Add(Energy.Base.Cast.ObjectToString(_.GetValue(o, null)));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Get desired attribute for a class or null if not found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static object GetClassAttribute(Type type, Type attribute)
        {
            System.Reflection.MemberInfo info = type;
            object[] attributes = info.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == attribute)
                {
                    return attributes[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Get value of a first field or property of object with custom attribute.
        /// </summary>
        /// <param name="o">Any object</param>
        /// <param name="attribute">Attribute class type</param>
        /// <returns>Value or null if not found</returns>
        public static object GetValueWithAttribute(object o, Type attribute)
        {
            object[] value = GetValuesWithAttribute(o, attribute, true, 1);
            if (value == null || value.Length == 0)
                return value;
            return value[0];
        }

        /// <summary>
        /// Get values of all fields and properties of object with custom attribute.
        /// </summary>
        /// <param name="item">Any object</param>
        /// <param name="attribute">Attribute class type</param>
        /// <param name="includePrivate">Include private fields and properties</param>
        /// <param name="max">Maximum number of fields</param>
        /// <returns>Value or null if not found</returns>
        public static object[] GetValuesWithAttribute(object item, Type attribute, bool includePrivate, int max)
        {
            if (item == null)
                return null;
            List<object> list = new List<object>();
            System.Type type = item.GetType();
            foreach (FieldInfo _ in type.GetFields())
            {
                if (!includePrivate && _.IsPrivate)
                    continue;
                object[] a = _.GetCustomAttributes(true);
                foreach (object x in a)
                {
                    if (x.GetType() == attribute)
                    {
                        list.Add(_.GetValue(item));
                        break;
                    }
                }
                if (max > 0 && list.Count >= max)
                    return list.ToArray();
            }
            BindingFlags f = BindingFlags.Instance | BindingFlags.Public;
            if (includePrivate)
                f |= BindingFlags.NonPublic;
            foreach (PropertyInfo _ in type.GetProperties(f))
            {
                object[] a = _.GetCustomAttributes(true);
                foreach (object x in a)
                {
                    if (x.GetType() == attribute)
                    {
                        list.Add(_.GetValue(item, null));
                        break;
                    }
                    if (max > 0 && list.Count >= max)
                        return list.ToArray();
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Get list of objects of specified type from list.
        /// </summary>
        /// <param name="list">List of objects</param>
        /// <param name="type">Type to filter</param>
        /// <returns>List of filtered objects</returns>
        public static IEnumerable GetObjectsOfType(IEnumerable list, Type type)
        {
            List<object> l = new List<object>();
            foreach (object o in list)
            {
                Type t = o.GetType();
                if (t == type || t.IsSubclassOf(type))
                {
                    l.Add(o);
                }
            }
            return l;
        }

        #region Class information

        /// <summary>
        /// Object class information.
        /// </summary>
        public class Information: List<Information.Field>
        {
            public Type Type { get; set; }

            public string Name { get; set; }

            private object[] _Attributes;

            public object[] Attributes
            {
                get
                {
                    if (_Attributes == null)
                    {
                        _Attributes = Type.GetCustomAttributes(true);
                    }
                    return _Attributes;
                }
                set
                {
                    _Attributes = value;
                }
            }

            private Dictionary<string, Information.Field> _Index = new Dictionary<string, Field>();
            /// <summary>Index</summary>
            public Dictionary<string, Information.Field> Index { get { return _Index; } set { _Index = value; } }

            #region Class field information

            /// <summary>
            /// Class field information
            /// </summary>
            public class Field
            {
                public string Name { get { return GetName(); } }

                public object Object { get; set; }

                public Type Type { get; set; }

                private object[] _Attributes;

                public object[] Attributes
                {
                    get
                    {
                        if (_Attributes == null)
                        {
                            _Attributes = Type.GetCustomAttributes(true);
                        }
                        return _Attributes;
                    }
                    set
                    {
                        _Attributes = value;
                    }
                }

                public override string ToString()
                {
                    return string.Concat(Type, " ", Name).Trim();
                }

                public string GetName()
                {
                    if (Object == null)
                        return null;
                    if (Object is FieldInfo)
                        return ((FieldInfo)Object).Name;
                    if (Object is PropertyInfo)
                        return ((PropertyInfo)Object).Name;
                    return null;
                }
            }

            #endregion

            #region Override

            public new void Add(Information.Field field)
            {
                base.Add(field);
                Index[field.Name] = field;
            }

            public new void Remove(Information.Field field)
            {
                Index.Remove(field.Name);
                base.Remove(field);
            }

            public new void RemoveAt(int index)
            {
                Index.Remove(this[index].Name);
                base.RemoveAt(index);
            }

            public new void Clear()
            {
                Index.Clear();
                base.Clear();
            }

            #endregion

            public Information.Field this[string field]
            {
                get
                {
                    if (!Index.ContainsKey(field))
                        return null;
                    return Index[field];
                }
            }

            /// <summary>
            /// Create class information from type.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static Energy.Base.Class.Information Create(Type type)
            {
                Energy.Base.Class.Information information = new Energy.Base.Class.Information();
                information.Name = type.Name;
                information.Type = type;
                FieldInfo[] fa = type.GetFields();
                for (int i = 0; i < fa.Length; i++)
                {
                    Energy.Base.Class.Information.Field field = new Energy.Base.Class.Information.Field();
                    field.Object = fa[i];
                    field.Type = fa[i].FieldType;
                    information.Add(field);
                }
                BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                PropertyInfo[] pa = type.GetProperties(bf);
                for (int i = 0; i < pa.Length; i++)
                {
                    Energy.Base.Class.Information.Field field = new Energy.Base.Class.Information.Field();
                    field.Object = pa[i];
                    field.Type = pa[i].PropertyType;
                    information.Add(field);
                }
                return information;
            }

            /// <summary>
            /// Create class information from existing object.
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public static Energy.Base.Class.Information Create(object o)
            {
                if (o == null)
                    return null;
                return Create(o.GetType());
            }
        }

        #endregion

        #region Class information repository

        /// <summary>
        /// Class information repository
        /// </summary>
        public class Repository
        {
            private Dictionary<System.Type, Energy.Base.Class.Information> _Information;
            /// <summary>Information</summary>
            public Dictionary<System.Type, Energy.Base.Class.Information> Information { get { return _Information; } set { _Information = value; } }

            public Repository()
            {
                _Information = new Dictionary<System.Type, Energy.Base.Class.Information>();
            }

            public void Scan(System.Type type)
            {
                Energy.Base.Class.Information information = Energy.Base.Class.Information.Create(type);
                _Information[type] = information;
            }
        }

        #endregion

        #region Assembly management

        /// <summary>
        /// Get list of assemblies of current application domain
        /// </summary>
        /// <returns></returns>
        public static System.Reflection.Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Get list of assemblies of current application domain filtered...
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static System.Reflection.Assembly[] GetAssemblies(params string[] filters)
        {
            List<System.Reflection.Assembly> list = new List<System.Reflection.Assembly>();
            bool ignoreCase = true;
            foreach (System.Reflection.Assembly assembly in GetAssemblies())
            {
                string input = assembly.FullName;
                bool check = Energy.Base.Text.Check(input
                    , Enumeration.MatchStyle.Any, Energy.Enumeration.MatchMode.Simple, ignoreCase
                    , filters);
                if (!check)
                    continue;
                list.Add(assembly);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Create string dictionary of assemblies by their short names.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static Energy.Base.Collection.StringDictionary<Assembly> GetAssembliesDictionaryByShortName(Assembly[] assemblies)
        {
            if (assemblies == null) return null;
            Energy.Base.Collection.StringDictionary<Assembly> dictionary = new Collection.StringDictionary<Assembly>();
            string pattern = "[^,]+";
            System.Text.RegularExpressions.RegexOptions options = System.Text.RegularExpressions.RegexOptions.None;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, options);
            foreach (Assembly assembly in assemblies)
            {
                string name = assembly.FullName;
                System.Text.RegularExpressions.Match match = regex.Match(name);
                if (match.Success)
                {
                    name = match.Value;
                }
                if (dictionary.ContainsKey(name))
                {
                    Energy.Core.Bug.Write("Assembly identified by " + name + " found more than one in a list");
                    continue;
                }
                dictionary[name] = assembly;
            }
            return dictionary;
        }

        /// <summary>
        /// Create string dictionary of assemblies by their full names.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static Energy.Base.Collection.StringDictionary<Assembly> GetAssembliesDictionaryByFullName(Assembly[] assemblies)
        {
            if (assemblies == null) return null;
            Energy.Base.Collection.StringDictionary<Assembly> dictionary = new Collection.StringDictionary<Assembly>();
            foreach (Assembly assembly in assemblies)
            {
                string name = assembly.FullName;
                if (dictionary.ContainsKey(name))
                {
                    Energy.Core.Bug.Write("Assembly identified by " + name + " found more than one in a list");
                    continue;
                }
                dictionary[name] = assembly;
            }
            return dictionary;
        }

        /// <summary>
        /// Create string dictionary of short assembly name as key and version as value.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAssemblyVersionsDictionaryByShortName(System.Reflection.Assembly[] assemblies)
        {
            if (assemblies == null) return null;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string patternName = "[^,]+";
            string patternVersion = "(?:^|[,\\s])(?:Version=)([^,\\s]+)";
            System.Text.RegularExpressions.RegexOptions options = System.Text.RegularExpressions.RegexOptions.None;
            System.Text.RegularExpressions.Regex regexName = new System.Text.RegularExpressions.Regex(patternName, options);
            System.Text.RegularExpressions.Regex regexVersion = new System.Text.RegularExpressions.Regex(patternVersion, options);
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                string name = assembly.FullName;
                string version = "";
                System.Text.RegularExpressions.Match match;
                match = regexVersion.Match(name);
                if (match.Success)
                {
                    version = match.Groups[1].Value;
                }
                match = regexName.Match(name);
                if (match.Success)
                {
                    name = match.Value;
                }
                if (dictionary.ContainsKey(name))
                {
                    System.Diagnostics.Debug.WriteLine(Energy.Base.Clock.CurrentTime + " Assembly identified by " + name + " found more than one in a list");
                    continue;
                }
                dictionary[name] = version;
            }
            return dictionary;
        }

        #endregion
    }
}
