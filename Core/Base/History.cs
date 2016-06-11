using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// History
    /// </summary>
    public class History
    {
        /// <summary>
        /// Insert string element into history or move it to the top
        /// </summary>
        /// <param name="list">String list</param>
        /// <param name="element">String element</param>
        /// <param name="insensitive">Ignore case</param>
        /// <returns>String list</returns>
        public static List<string> Insert(List<string> list, string element, bool insensitive)
        {
            if (list == null)
                return null;
            if (String.IsNullOrEmpty(element))
                return list;
            if (!insensitive)
            {
                int index = list.IndexOf(element);
                if (index >= 0)
                    list.RemoveAt(index);
            }
            else
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (0 == String.Compare(list[i], element, insensitive))
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            int offset = 0;
            if (list.Count > 0 && list[0] == "")
                offset++;
            list.Insert(offset, element);
            return list;
        }

        /// <summary>
        /// Insert string element into history or move it to the top
        /// </summary>
        /// <param name="list">String list</param>
        /// <param name="element">String element</param>
        /// <returns>String list</returns>
        public static List<string> Insert(List<string> list, string element)
        {
            return Insert(list, element, true);
        }

        /// <summary>
        /// Insert string element into history or move it to the top
        /// </summary>
        /// <param name="array">String list</param>
        /// <param name="element">String element</param>
        /// <returns>String array</returns>
        public static string[] Insert(string[] array, string element)
        {
            if (array == null)
                return null;
            return Insert(new List<string>(array), element).ToArray();
        }
    }
}
