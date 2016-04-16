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
        /// Insert element into history
        /// </summary>
        /// <param name="list"></param>
        /// <param name="element"></param>
        /// <param name="insensitive"></param>
        /// <returns></returns>
        public static List<string> Insert(List<string> list, string element, bool insensitive = false)
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
                    if (0 == String.Compare(list[i], element, true))
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
        /// Insert element into history
        /// </summary>
        /// <param name="array"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string[] Insert(string[] array, string element)
        {
            if (array == null)
                return null;
            return Insert(new List<string>(array), element).ToArray();
        }
    }
}
