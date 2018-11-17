using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface IXmlSerializable
    {
        System.Xml.Schema.XmlSchema GetSchema();

        void ReadXml(System.Xml.XmlReader reader);

        void WriteXml(System.Xml.XmlWriter writer);
    }
}
