using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Energy.Base
{
    public class Class
    {
        /// <summary>
        /// Get list of fields and propeties for specified class type
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
    }
}
