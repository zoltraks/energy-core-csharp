using System;
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
        /// <param name="type">Class type</param>
        /// <returns>Array of string</returns>
        public static string[] GetFieldsAndProperties(Type type)
        {
            System.Collections.Generic.List<string> field = new System.Collections.Generic.List<string>();
            // Class properties //
            foreach (PropertyInfo _ in type.GetProperties())
            {
                field.Add(_.Name);
            }
            // Class fields //
            foreach (FieldInfo _ in type.GetFields())
            {
                field.Add(_.Name);
            }
            return field.ToArray();
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
            foreach (PropertyInfo _ in type.GetProperties())
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
            return null;
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
    }
}
