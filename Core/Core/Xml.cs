using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace Energy.Core
{
    /// <summary>
    /// XML
    /// </summary>
    public class Xml
    {
        /// <summary>
        /// Serialize object to XML
        /// <para>
        /// Object class must implement IXmlSerializable interface
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject);
        /// </code>
        /// </summary>
        /// <param name="data">object</param>
        /// <param name="root">string</param>
        /// <param name="space">string</param>
        /// <returns>string</returns>
        public static string Serialize(object data, string root = "", string space = "")
        {
            string xml = null;
            try
            {
                XmlRootAttribute xra = new XmlRootAttribute(root);
                xra.Namespace = space;
                XmlSerializer xs = new XmlSerializer(data.GetType(), xra);
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings xws = new XmlWriterSettings() { OmitXmlDeclaration = true };
                xws.Indent = true;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", space);
                XmlWriter xw = XmlWriter.Create(sb, xws);
                xs.Serialize(xw, data, ns);
                xml = sb.ToString();
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
            }
            return xml;
        }

        /// <summary>
        /// Deserialize object from XML
        /// </summary>
        /// <code>
        ///     MyDictionary x = (MyDictionary)Energy.Base.Xml.Deserialize(xml, typeof(MyDictionary));
        /// </code>
        /// <param name="content">string</param>
        /// <param name="type">System.Type</param>
        /// <returns>object</returns>
        public static object Deserialize(string content, Type type)
        {
            if (content == null || content.Trim().Length == 0)
            {
                return null;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                StringReader stream = new StringReader(content);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CheckCharacters = false;
                XmlReader reader = XmlReader.Create(stream, settings);
                return serializer.Deserialize(reader);
            }
            catch (InvalidOperationException)
            {
                Debug.WriteLine("Invalid operation");
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Deserialize object from XML
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="type">System.Type</param>
        /// <param name="root">string</param>
        /// <param name="space">string</param>
        /// <returns>object</returns>
        public static object Deserialize(string content, Type type, string root, string space)
        {
            try
            {
                XmlRootAttribute attribute = new XmlRootAttribute(root) { Namespace = space };
                XmlSerializer serializer = new XmlSerializer(type, attribute);
                StringReader stream = new StringReader(content);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CheckCharacters = false;
                XmlReader reader = XmlReader.Create(stream, settings);
                return serializer.Deserialize(reader);
            }
            catch
            {                
                return null;
            }
        }

        /// <summary>
        /// Deserialize object from XML
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="type">System.Type</param>
        /// <param name="root">string</param>
        /// <returns>object</returns>
        public static object Deserialize(string content, Type type, string root)
        {
            return Deserialize(content, type, root, "");
        }

        /// <summary>
        /// Deserialize object from XML, root alternatives allowed
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="type">System.Type</param>
        /// <param name="root">string[]</param>
        /// <param name="space">string</param>
        /// <returns>object</returns>
        public static object Deserialize(string content, Type type, string[] root, string space)
        {
            foreach (string element in root)
            {
                try
                {
                    XmlRootAttribute attribute = new XmlRootAttribute(element);
                    attribute.Namespace = space;
                    XmlSerializer serializer = new XmlSerializer(type, attribute);
                    StringReader stream = new StringReader(content);
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.CheckCharacters = false;
                    XmlReader reader = XmlReader.Create(stream, settings);
                    return serializer.Deserialize(reader);
                }
                catch
                {
                    continue;
                }
            }
            return null;
        }

        /// <summary>
        /// Deserialize object from XML, root alternatives allowed
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="type">System.Type</param>
        /// <param name="root">string[]</param>
        /// <returns>object</returns>
        public static object Deserialize(string content, Type type, string[] root)
        {
            return Deserialize(content, type, root, "");
        }

        /// <summary>
        /// Read XML string element
        /// </summary>
        /// <param name="reader">XmlReader</param>
        public static string ReadXmlString(XmlReader reader)
        {
            return reader.ReadString().Replace("\n", "\r\n");
        }

        /// <summary>
        /// Write XML string element
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="key">string</param>
        /// <param name="value">string</param>
        public static void WriteXmlString(XmlWriter writer, string key, string value)
        {
            if (String.IsNullOrEmpty(value)) return;
            writer.WriteStartElement(key);
            writer.WriteString(value);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Write XML string element
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="key">string</param>
        /// <param name="value">object</param>
        public static void WriteXmlString(XmlWriter writer, string key, object value)
        {
            if (value == null) return;
            string content = value.ToString();
            if (content.Length == 0) return;
            writer.WriteStartElement(key);
            writer.WriteString(content);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Write XML string element
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="key">string</param>
        /// <param name="value">object</param>
        /// <param name="empty">bool</param>
        public static void WriteXmlString(XmlWriter writer, string key, object value, bool empty)
        {
            if (!empty)
            {
                if (value is int && (int)value == 0) return;
                if (value is long && (long)value == 0) return;
                if ((value is float || value is double) && (double)value == 0) return;
                if (value.ToString().Length == 0) return;
            }
            WriteXmlString(writer, key, value);
        }

        /// <summary>
        /// Write XML element
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="key">string</param>
        /// <param name="value">object</param>
        public static void Write(XmlWriter writer, string key, object value)
        {
            if (value == null) return;
            string content = value.ToString();
            if (content.Length == 0) return;
            writer.WriteStartElement(key);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }
    }
}
