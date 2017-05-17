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
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Public;
            if (includePrivate)
                bf |= BindingFlags.NonPublic;
            PropertyInfo[] search = type.GetProperties(bf);
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
    }
}
