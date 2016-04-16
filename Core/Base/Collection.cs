using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Energy.Base
{
    public class Collection
    {
        public class Array<T> : List<T>
        {
            public T First
            {
                get
                {
                    return Count == 0 ? default(T) : this[0];
                }
            }

            public T Last
            {
                get
                {
                    return Count == 0 ? default(T) : this[Count - 1];
                }
            }

            public T New()
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                base.Add(item);
                return item;
            }
        }
    }
}
