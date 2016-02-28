using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Energy.Base
{
    /// <summary>
    /// Serializable string dictionary of objects
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    [XmlRoot]
    public class AssociativeArray<T> : Dictionary<string, T>, IXmlSerializable
    {
        #region IXmlSerializable Members

        /// <summary>
        /// XML Schema
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Read XML
        /// </summary>
        /// <param name="reader">XmlReader</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            Clear();

            bool empty = reader.IsEmptyElement;

            reader.Read();

            if (empty) return;

            reader.MoveToContent();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                if (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                {
                    reader.Read();
                    continue;
                }
                string key = reader.Name;
                if (reader.IsEmptyElement)
                {
                    Add(key, default(T));
                    reader.Read();
                }
                else
                {
                    reader.ReadStartElement();
                    Add(key, (T)(object)reader.ReadString());
                    reader.ReadEndElement();
                }
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// Write XML
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {            
            foreach (string key in this.Keys)
            {
                writer.WriteStartElement(key.ToString());
                if (this[key] != null) writer.WriteValue(this[key]);
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
