using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Functions related to classes, objects and assemblies.
    /// </summary>
    public class Class
    {
        #region Utility functions

        #region GetDefault

        /// <summary>
        /// Get default value of specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefault(System.Type type)
        {
            object o = Activator.CreateInstance(type);
            return o;
        }

        /// <summary>
        /// Get default value of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDefault<T>()
        {
            return default(T);
        }

        #endregion

        #region GetFieldsAndProperties

        /// <summary>
        /// Get list of names of all fields and properties of specified class type.
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
        /// Get list of names of all fields and properties of specified class type.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <param name="includePrivate"></param>
        /// <returns>Array of string</returns>
        public static string[] GetFieldsAndProperties(Type type, bool includePrivate)
        {
            return GetFieldsAndProperties(type, includePrivate, true);
        }

        /// <summary>
        /// Get list of names of public fields and properties of specified class type.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns>Array of string</returns>
        public static string[] GetFieldsAndProperties(Type type)
        {
            return GetFieldsAndProperties(type, false, true);
        }

        #endregion

        #region GetFieldOrPropertyAttribute...

        /// <summary>
        /// Get first attribute for a field or property of desired class.
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
            BindingFlags binding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo _ in type.GetProperties(binding))
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
        /// Get first attribute for a field or property of desired class.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <param name="name">Field or property name</param>
        /// <returns>Attribute or null if not found</returns>
        public static T GetFieldOrPropertyAttribute<T>(Type type, string name)
        {
            return (T)GetFieldOrPropertyAttribute(type, name, typeof(T));
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

        #endregion

        #region FindFieldInfo / FindPropertyInfo

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

        #endregion

        #region GetFieldValue / GetPropertyValue / GetFieldOrPropertyValue

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

        #endregion

        #region ObjectToStringArray

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

        #endregion

        #region GetClassAttribute

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
        /// Get desired attribute for a class or null if not found.
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <returns></returns>
        public static T GetClassAttribute<T>(Type type)
        {
            System.Reflection.MemberInfo info = type;
            object[] attributes = info.GetCustomAttributes(true);
            System.Type attributeType = typeof(T);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == attributeType)
                {
                    return (T)attributes[i];
                }
            }
            return default(T);
        }

        #endregion

        #region GetValueWithAttribute / GetValuesWithAttribute

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

        #endregion

        #region GetObjectsOfType

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

        #endregion

        #region GetClassInterface

        public static Type GetClassInterface(Type classType, Type interfaceType)
        {
            if (classType == interfaceType)
                return interfaceType;
            Type[] interfaceTypes = classType.GetInterfaces();
            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                if (interfaceTypes[i] == interfaceType)
                    return interfaceTypes[i];
            }
            return null;
        }

        public static Type GetClassInterface(Type classType, string interfaceName)
        {
            if (classType.Name == interfaceName)
                return classType;
            Type type = classType.GetInterface(interfaceName);
            return type;
        }

        #endregion

        #region GetTypes

        public static System.Type[] GetTypes(Assembly[] assemblies, Energy.Base.Anonymous.Function<bool, System.Type> filter)
        {
            if (assemblies == null)
                return null;
            if (assemblies.Length == 0)
                return new System.Type[] { };
            List<System.Type> list = new List<System.Type>();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                System.Type[] typeList;
                try
                {
                    typeList = assembly.GetTypes();
                }
                catch (System.Reflection.ReflectionTypeLoadException)
                {
                    continue;
                }
                if (filter == null)
                {
                    list.AddRange(typeList);
                }
                else
                {
                    for (int n = 0; n < typeList.Length; n++)
                    {
                        Type type = typeList[n];
                        if (!filter(type))
                            continue;
                        list.Add(type);
                    }
                }
            }
            return list.ToArray();
        }

        public static Type[] GetTypes(Assembly[] assemblies)
        {
            return GetTypes(assemblies, null);
        }

        public static Type[] GetTypes(Assembly assembly)
        {
            return GetTypes(new Assembly[] { assembly }, null);
        }

        public static Type[] GetTypes(Assembly assembly, Energy.Base.Anonymous.Function<bool, System.Type> filter)
        {
            return GetTypes(new Assembly[] { assembly }, filter);
        }

        #endregion

        #region GetTypeWithInterface

        /// <summary>
        /// Get first type that implements specified interface.
        /// Return null if not found.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static System.Type GetTypeWithInterface(System.Type[] types, System.Type interfaceType)
        {
            if (types == null || interfaceType == null || types.Length == 0)
                return null;
            foreach (System.Type type in types)
            {
                System.Type[] interfaces = type.GetInterfaces();
                foreach (System.Type needle in interfaces)
                {
                    if (needle == interfaceType)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        #endregion

        #region GetTypesWithInterface

        /// <summary>
        /// Filter types that implements specified interface.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static System.Type[] GetTypesWithInterface(System.Type[] types, System.Type interfaceType)
        {
            if (types == null || interfaceType == null)
                return null;
            if (types.Length == 0)
                return types;
            List<System.Type> typeList = new List<System.Type>();
            foreach (System.Type type in types)
            {
                System.Type[] interfaces = type.GetInterfaces();
                foreach (System.Type needle in interfaces)
                {
                    if (needle == interfaceType)
                    {
                        typeList.Add(type);
                        break; // foreach
                    }
                }
            }
            return typeList.ToArray();
        }

        #endregion

        #region GetTypeWithAttribute

        /// <summary>
        /// Get first type having specified attribute.
        /// Return null if not found.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static System.Type GetTypeWithAttribute(System.Type[] types, System.Type attributeType)
        {
            if (types == null || attributeType == null || types.Length == 0)
                return null;
            foreach (System.Type type in types)
            {
                object[] look = type.GetCustomAttributes(attributeType, true);
                if (look.Length > 0)
                {
                    return type;
                }
            }
            return null;
        }

        #endregion

        #region GetTypesWithAttribute

        /// <summary>
        /// Filter types that have specified attribute.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static System.Type[] GetTypesWithAttribute(System.Type[] types, System.Type attributeType)
        {
            if (types == null || attributeType == null)
                return null;
            if (types.Length == 0)
                return types;
            List<System.Type> typeList = new List<System.Type>();
            foreach (System.Type type in types)
            {
                object[] look = type.GetCustomAttributes(attributeType, true);
                if (look.Length > 0)
                {
                    typeList.Add(type);
                }
            }
            return typeList.ToArray();
        }

        #endregion

        #region IsStatic

        /// <summary>
        /// True if class is static and cannot be instantiated.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStatic(Type type)
        {
            if (null == type)
                return false;

            if (type.IsAbstract && type.IsSealed)
                return true;

            return false;
        }

        #endregion

        #region IsInstance

        /// <summary>
        /// True if object of specified class can be created.
        /// At least one public constructor needs to be found.
        /// Function will result false for for static class type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInstance(Type type)
        {
            if (null == type)
                return false;

            if (type.IsAbstract)
                return false;

            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length > 0)
                return true;

            return false;
        }

        #endregion

        #region HasParameterlessConstructor

        /// <summary>
        /// True if class has public parameterless constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasParameterlessConstructor(Type type)
        {
            if (null == type)
                return false;

            if (type.IsAbstract)
                return false;

            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

            if (null == constructor)
                return false;

            if (constructor.IsPublic)
                return true;

            return false;
        }

        #endregion

        #region HasParameteredConstructor

        /// <summary>
        /// True if class has at least one public constructor with specified number parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="maximumParameterCount"></param>
        /// <param name="minimumParameterCount"></param>
        /// <returns></returns>
        public static bool HasParameteredConstructor(Type type, int minimumParameterCount, int maximumParameterCount)
        {
            if (null == type)
                return false;

            if (type.IsAbstract)
                return false;

            ConstructorInfo[] constructors = type.GetConstructors();

            if (null == constructors)
                return false;

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                if (minimumParameterCount > 0 && parameters.Length < minimumParameterCount)
                    continue;
                if (maximumParameterCount > 0 && parameters.Length > maximumParameterCount)
                    continue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// True if class has constructor with one or more parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasParameteredConstructor(Type type)
        {
            return HasParameteredConstructor(type, 1, 0);
        }

        #endregion

        #endregion

        #region Object management

        #region Mangle

        /// <summary>
        /// Mangle object by applying a function to each field and property of specified type.
        /// Returns number of fields and properties affected.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="includePrivate"></param>
        /// <param name="includePublic"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static int Mangle<T>(object subject, bool includePrivate, bool includePublic
            , Energy.Base.Anonymous.Function<T, T> function)
        {
            if (subject == null || function == null)
                return 0;

            int count = 0;

            Type objectType = subject.GetType();
            Type filterType = typeof(T);

            BindingFlags f = BindingFlags.Instance;
            if (includePrivate)
                f |= BindingFlags.NonPublic;
            if (includePublic)
                f |= BindingFlags.Public;

            T value;

            foreach (FieldInfo _ in objectType.GetFields(f))
            {
                if (filterType != _.FieldType)
                    continue;
                value = (T)_.GetValue(subject);
                value = function(value);
                _.SetValue(subject, value);
                count++;
            }

            foreach (PropertyInfo _ in objectType.GetProperties(f))
            {
                if (filterType != _.PropertyType)
                    continue;
                if (null == _.GetSetMethod())
                    continue;
                value = (T)_.GetValue(subject, null);
                value = function(value);
                _.SetValue(subject, value, null);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Mangle object by applying a function to a field or property of specified class type.
        /// Returns 1 if value was changed, 0 if not found or read only.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject">Object</param>
        /// <param name="name">Field or property name</param>
        /// <param name="includePrivate"></param>
        /// <param name="includePublic"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static int Mangle<T>(object subject, string name, bool includePrivate, bool includePublic
            , Energy.Base.Anonymous.Function<T, T> function)
        {
            if (subject == null || function == null)
                return 0;

            if (string.IsNullOrEmpty(name))
                return 0;

            Type objectType = subject.GetType();
            Type filterType = typeof(T);

            BindingFlags f = BindingFlags.Instance;
            if (includePrivate)
                f |= BindingFlags.NonPublic;
            if (includePublic)
                f |= BindingFlags.Public;

            T value;

            FieldInfo fieldInfo = objectType.GetField(name, f);

            if (fieldInfo != null)
            {
                if (filterType != fieldInfo.FieldType)
                    return 0;

                value = (T)fieldInfo.GetValue(subject);
                value = function(value);
                fieldInfo.SetValue(subject, value);
                return 1;
            }

            PropertyInfo propertyInfo = objectType.GetProperty(name, f);

            if (propertyInfo != null)
            {
                if (filterType != propertyInfo.PropertyType)
                    return 0;
                if (null == propertyInfo.GetSetMethod())
                    return 0;
                value = (T)propertyInfo.GetValue(subject, null);
                value = function(value);
                propertyInfo.SetValue(subject, value, null);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Mangle object by applying a function to a field or property of specified class type.
        /// Returns 1 if value was changed, 0 if not found or read only.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject">Object</param>
        /// <param name="name">Field or property name</param>
        /// <param name="includePrivate"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static int Mangle<T>(object subject, string name, bool includePrivate
            , Energy.Base.Anonymous.Function<T, T> function)
        {
            return Mangle(subject, name, includePrivate, true, function);
        }

        /// <summary>
        /// Mangle object by applying a function to public field or property of specified class type.
        /// Returns 1 if value was changed, 0 if not found or read only.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject">Object</param>
        /// <param name="name">Field or property name</param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static int Mangle<T>(object subject, string name
            , Energy.Base.Anonymous.Function<T, T> function)
        {
            return Mangle(subject, name, false, true, function);
        }

        /// <summary>
        /// Mangle object by applying a function to each field and property of specified type.
        /// Returns number of fields and properties affected.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="includePrivate"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static int Mangle<T>(object subject, bool includePrivate
            , Energy.Base.Anonymous.Function<T, T> function)
        {
            return Mangle<T>(subject, includePrivate, true, function);
        }

        /// <summary>
        /// Mangle object by applying a function to each public field and property of specified type.
        /// Returns number of fields and properties affected.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static int Mangle<T>(object subject
            , Energy.Base.Anonymous.Function<T, T> function)
        {
            return Mangle<T>(subject, false, true, function);
        }

        #endregion

        #endregion

        #region Class information

        /// <summary>
        /// Object class information.
        /// </summary>
        public class Information : List<Information.Field>
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
            #region Private

            private readonly Energy.Base.Lock _Lock = new Energy.Base.Lock();

            private Dictionary<System.Type, Energy.Base.Class.Information> _Information;

            #endregion

            #region Property

            public Dictionary<System.Type, Energy.Base.Class.Information> Information { get { return _Information; } set { _Information = value; } }

            public Energy.Base.Class.Information this[System.Type type]
            {
                get
                {
                    return Get(type);
                }
            }

            #endregion

            #region Constructor

            public Repository()
            {
                _Information = new Dictionary<System.Type, Energy.Base.Class.Information>();
            }

            #endregion

            #region Global

            private static Repository _Global;

            private static readonly Energy.Base.Lock _GlobalLock = new Energy.Base.Lock();

            /// <summary>Global</summary>
            public static Repository Global
            {
                get
                {
                    if (_Global == null)
                    {
                        lock (_GlobalLock)
                        {
                            if (_Global == null)
                            {
                                _Global = new Repository();
                            }
                        }
                    }
                    return _Global;
                }
            }

            #endregion

            #region Scan

            /// <summary>
            /// Scan type and store information in a repository.
            /// </summary>
            /// <param name="type"></param>
            /// <returns>Returns information object</returns>
            public Energy.Base.Class.Information Scan(System.Type type)
            {
                lock (_Lock)
                {
                    Energy.Base.Class.Information information = Energy.Base.Class.Information.Create(type);
                    _Information[type] = information;
                    return information;
                }
            }

            /// <summary>
            /// Scan types and store information in a repository.
            /// </summary>
            /// <param name="types"></param>
            /// <returns></returns>
            public Energy.Base.Class.Information[] Scan(System.Type[] types)
            {
                List<Energy.Base.Class.Information> list = new List<Energy.Base.Class.Information>();
                Dictionary<Energy.Base.Class.Information, System.Type> map = new Dictionary<Energy.Base.Class.Information, System.Type>();
                for (int i = 0; i < types.Length; i++)
                {
                    System.Type type = types[i];
                    Energy.Base.Class.Information information = Energy.Base.Class.Information.Create(type);
                    list.Add(information);
                    map[information] = type;
                }
                lock (_Lock)
                {
                    foreach (KeyValuePair<Energy.Base.Class.Information, System.Type> e in map)
                    {
                        Energy.Base.Class.Information information = e.Key;
                        System.Type type = e.Value;
                        _Information[type] = information;
                    }
                }
                return list.ToArray();
            }

            /// <summary>
            /// Scan types from assembly and store information in a repository.
            /// </summary>
            /// <param name="assembly"></param>
            /// <returns></returns>
            public Energy.Base.Class.Information[] Scan(Assembly assembly)
            {
                return Scan(assembly.GetTypes());
            }

            #endregion

            #region Get

            /// <summary>
            ///
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public Energy.Base.Class.Information Get(System.Type type)
            {
                lock (_Information)
                {
                    if (_Information.ContainsKey(type))
                        return _Information[type];
                }
                return Scan(type);
            }

            #endregion
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
        /// <param name="matchMode"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static System.Reflection.Assembly[] GetAssemblies(Energy.Enumeration.MatchMode matchMode, params string[] filters)
        {
            List<System.Reflection.Assembly> list = new List<System.Reflection.Assembly>();
            bool ignoreCase = true;
            foreach (System.Reflection.Assembly assembly in GetAssemblies())
            {
                string input = assembly.FullName;
                bool check = Energy.Base.Text.Check(input, Energy.Enumeration.MatchStyle.Any
                    , matchMode, ignoreCase, filters);
                if (!check)
                    continue;
                list.Add(assembly);
            }
            return list.ToArray();
        }


        /// <summary>
        /// Get list of assemblies of current application domain filtered...
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static System.Reflection.Assembly[] GetAssemblies(params string[] filters)
        {
            return GetAssemblies(Energy.Enumeration.MatchMode.Simple, filters);
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

        /// <summary>
        /// Returns the major/minor version number for an assembly formatted as a string.
        /// </summary>
        public static string GetAssemblySoftwareVersion(System.Reflection.Assembly assembly)
        {
#if NETSTANDARD
            AssemblyInformationalVersionAttribute attributeAssemblyInformationalVersion
                = Energy.Base.Class.GetClassAttribute<AssemblyInformationalVersionAttribute>(assembly.GetType());
            return attributeAssemblyInformationalVersion.InformationalVersion;
#else
            return null;
#endif
        }

        /// <summary>
        /// Returns the build/revision number for an assembly formatted as a string.
        /// </summary>
        public static string GetAssemblyBuildNumber(System.Reflection.Assembly assembly)
        {
#if NETSTANDARD
            return assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
#else
            return assembly.ImageRuntimeVersion;
#endif
        }

        /// <summary>
        /// Returns the linker timestamp for an assembly. 
        /// </summary>
        public static DateTime GetAssemblyTimestamp(System.Reflection.Assembly assembly)
        {
            try
            {
                return System.IO.File.GetLastWriteTimeUtc(assembly.Location);
            }
            catch
            { }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Get filename of assembly.
        /// This is just a simple alias for Location field of assembly object.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyFile(System.Reflection.Assembly assembly)
        {
            if (null == assembly)
            {
                return null;
            }
            string fileName = assembly.Location;
            return fileName;
        }

        /// <summary>
        /// Get directory name of assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyDirectory(System.Reflection.Assembly assembly)
        {
            if (null == assembly)
            {
                return null;
            }
            string fileName = assembly.Location;
            string directoryName = System.IO.Path.GetDirectoryName(fileName);
            return directoryName;
        }

        #endregion
    }
}
