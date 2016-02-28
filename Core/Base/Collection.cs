using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Collection
    {
        public class Array<T> : List<T>
        {
            public T Last
            {
                get
                {
                    return Count == 0 ? default(T) : this[Count - 1];
                }
            }

            public T Create()
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                Add(item);
                return item;
            }
        }
    }
}
