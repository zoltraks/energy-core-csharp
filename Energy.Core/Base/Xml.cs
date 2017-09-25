using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// XML support
    /// </summary>
    public class Xml
    {
        private static readonly object _XmlLock = new object();

        #region Serialize

        /// <summary>
        /// Serialize object to XML
        /// <para>
        /// Object class must implement IXmlSerializable interface
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object</param>
        /// <param name="root">XML root</param>
        /// <param name="space">XML namespace</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data, string root, string space)
        {
            string xml = null;
            try
            {
                lock (_XmlLock)
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
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
                if (x.InnerException != null && x.InnerException.Message != x.Message)
                {
                    Debug.WriteLine(x.InnerException.Message);
                }
            }
            return xml;
        }

        /// <summary>
        /// Serialize object to XML
        /// <para>
        /// Object class must implement IXmlSerializable interface
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data)
        {
            return Serialize(data, "", "");
        }

        /// <summary>
        /// Serialize object to XML
        /// <para>
        /// Object class must implement IXmlSerializable interface
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object</param>
        /// <param name="root">XML root</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data, string root)
        {
            return Serialize(data, root, "");
        }

        #endregion

        #region Deserialize


        /// <summary>
        /// Deserialize object from XML, root alternatives allowed.
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
                    lock (_XmlLock)
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
                }
                catch
                {
                    continue;
                }
            }
            return null;
        }

        /// <summary>
        /// Generic XML deserialization method.
        /// </summary>
        /// <typeparam name="TDeserialize"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TDeserialize Deserialize<TDeserialize>(string content)
        {
            return (TDeserialize)Deserialize(content, typeof(TDeserialize));
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
            return Deserialize(content, type, new string[] { "" }, "");
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
            return Deserialize(content, type, new string[] { root }, space);
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
            return Deserialize(content, type, new string[] { root }, "");
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

        #endregion

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

        public static string GetXmlRoot(object _)
        {
            if (_ == null)
                return null;

            object rootObject = Energy.Base.Class.GetFieldOrPropertyValue(_, "Root", false, false);

            if (rootObject != null && rootObject is string)
                return (string)rootObject;

            return GetXmlRoot(_.GetType());
        }

        public static string GetXmlRoot(Type type)
        {
            XmlRootAttribute xmlRootAttribute = (XmlRootAttribute)
                Energy.Base.Class.GetClassAttribute(type, typeof(XmlRootAttribute));
            if (xmlRootAttribute != null)
                return xmlRootAttribute.ElementName;
            Energy.Attribute.Data.ElementAttribute attributeElement = (Energy.Attribute.Data.ElementAttribute)
                Energy.Base.Class.GetClassAttribute(type, typeof(Energy.Attribute.Data.ElementAttribute));
            if (attributeElement != null)
                return attributeElement.Name;

            return "";
        }

        /// <summary>
        /// Extract root element from XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ExtractRoot(string xml)
        {
            Match match = Regex.Match(xml, Energy.Base.Pattern.XmlRootName);
            if (!match.Success)
                return "";
            else
                return match.Groups[1].Value;
        }
    }
}
