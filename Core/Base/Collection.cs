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
        public class Array<T> : List<T>, IXmlSerializable
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

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                for (int i = 0; i < Count; i++)
                {
                    serializer.Serialize(writer, this[i]);
                    //Energy.Core.Xml.Write(writer, this[i]);
                }
            }
        }
    }
}
