using System;
using System.Text;
using System.Xml;
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
        #region Lock

        private static readonly object _XmlLock = new Energy.Base.Lock();

        #endregion

        #region Class

        public class Class
        {
            /// <summary>
            /// Class representing XML TAG line
            /// </summary>
            public class XmlTagLine
            {
                /// <summary>
                /// XML Tag Name
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// XML Attribute Line (needs to be processed)
                /// </summary>
                public string Attribute { get; set; }

                /// <summary>
                /// XML Tag Index
                /// </summary>
                public int Index { get; set; }

                /// <summary>
                /// XML Tag Length
                /// </summary>
                public int Length { get; set; }

                /// <summary>
                /// XML Tag Ending
                /// </summary>
                public string End { get; set; }

                /// <summary>
                /// Returns true if tag is without any attributes.
                /// </summary>
                public bool IsSimple { get { return Energy.Base.Text.IsWhite(this.Attribute); } }
            }
        }

        #endregion

        #region Pattern

        public class Pattern
        {
            public static string InformalXmlTagLine = @"<\s*(?<name>[\?!a-zA-Z:_][a-zA-Z0-9:.\-\u00B7\u0300-\u036F\u203F-\u2040_]*)(?<attribute>(?:(?:\s+(?:""(?:""""|\\""|[^""])*""|[^=>/?\s]+)(?:\s*=\s*(?:""(?:""""|\\""|[^""])*)""|[^>/?\s]+)))*)(?<end>\s*(?:[/?]?\s*)>)?";
        }

        #endregion

        #region Serialize

        /// <summary>
        /// Serialize object to XML.
        /// <para>
        /// Object class must implement IXmlSerializable interface.
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="root">Root element</param>
        /// <param name="space">Namespace</param>
        /// <param name="error">Serialization error message. Empty when no error.</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data, string root, string space, out string error)
        {
            error = "";
            string xml = null;
            try
            {
                lock (_XmlLock)
                {
                    System.Xml.Serialization.XmlRootAttribute xra = new System.Xml.Serialization.XmlRootAttribute(root);
                    xra.Namespace = space;
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(data.GetType(), xra);
                    StringBuilder sb = new StringBuilder();
                    XmlWriterSettings xws = new XmlWriterSettings() { OmitXmlDeclaration = true };
                    xws.Indent = true;
                    System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                    ns.Add("", space);
                    XmlWriter xw = XmlWriter.Create(sb, xws);
                    xs.Serialize(xw, data, ns);
                    xml = sb.ToString();
                }
            }
            catch (Exception x)
            {
                if (null != x.InnerException)
                {
                    error = x.InnerException.Message;
                }
                else
                {
                    error = x.Message;
                }
                Energy.Core.Bug.Catch(x);
            }
            return xml;
        }

        /// <summary>
        /// Serialize object to XML.
        /// <para>
        /// Object class must implement IXmlSerializable interface.
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="root">Root element</param>
        /// <param name="space">Namespace</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data, string root, string space)
        {
            return Serialize(data, root, space, out string error);
        }

        /// <summary>
        /// Serialize object to XML.
        /// <para>
        /// Object class must implement IXmlSerializable interface.
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="error">Serialization error message. Empty when no error.</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data, out string error)
        {
            return Serialize(data, "", "", out error);
        }

        /// <summary>
        /// Serialize object to XML.
        /// <para>
        /// Object class must implement IXmlSerializable interface.
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data)
        {
            return Serialize(data, "", "", out string error);
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
            return Serialize(data, root, "", out string error);
        }

        /// <summary>
        /// Serialize object to XML.
        /// <para>
        /// Object class must implement IXmlSerializable interface.
        /// </para>
        /// <code>
        /// string xml = Energy.Base.Xml.Serialize(myObject, "Root", "org.example.xns");
        /// </code>
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="root">Root element</param>
        /// <param name="error">Serialization error message. Empty when no error.</param>
        /// <returns>XML string</returns>
        public static string Serialize(object data, string root, out string error)
        {
            return Serialize(data, root, "", out error);
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
                        System.Xml.Serialization.XmlRootAttribute attribute = new System.Xml.Serialization.XmlRootAttribute(element);
                        attribute.Namespace = space;
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type, attribute);
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
        /// Deserialize object from XML, root alternatives allowed.
        /// Generic XML deserialization method.
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="type">System.Type</param>
        /// <param name="root">string[]</param>
        /// <param name="space">string</param>
        /// <returns>object</returns>
        public static TDeserialize Deserialize<TDeserialize>(string content, Type type, string[] root, string space)
        {
            return (TDeserialize)Deserialize(content, typeof(TDeserialize), root, space);
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
        /// Deserialize object from XML.
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

        #region Helper

        /// <summary>
        /// Read XML string element.
        /// </summary>
        /// <param name="reader">XmlReader</param>
        public static string ReadXmlString(XmlReader reader)
        {
            return reader.ReadString().Replace("\n", "\r\n");
        }

        /// <summary>
        /// Write XML string element.
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
        /// Write XML string element.
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
        /// Write XML string element.
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
        /// Write XML element.
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

        #endregion

        #region GetXmlRoot

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
            System.Xml.Serialization.XmlRootAttribute xmlRootAttribute = (System.Xml.Serialization.XmlRootAttribute)
                Energy.Base.Class.GetClassAttribute(type, typeof(System.Xml.Serialization.XmlRootAttribute));
            if (xmlRootAttribute != null)
                return xmlRootAttribute.ElementName;
            Energy.Attribute.Data.ElementAttribute attributeElement = (Energy.Attribute.Data.ElementAttribute)
                Energy.Base.Class.GetClassAttribute(type, typeof(Energy.Attribute.Data.ElementAttribute));
            if (attributeElement != null)
                return attributeElement.Name;

            return "";
        }

        #endregion

        #region Extract

        /// <summary>
        /// Extract root tag information from XML content.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Class.XmlTagLine ExtractRootNodeLine(string xml)
        {
            Match m = Regex.Match(xml, Pattern.InformalXmlTagLine, RegexOptions.IgnorePatternWhitespace);
            while (true)
            {
                if (!m.Success)
                    return null;
                if (m.Groups["name"].Value.StartsWith("?") || m.Groups["name"].Value.StartsWith("!"))
                {
                    m = m.NextMatch();
                    continue;
                }
                return new Class.XmlTagLine()
                {
                    Name = m.Groups["name"].Value,
                    Attribute = m.Groups["attribute"].Value,
                    Index = m.Index,
                    Length = m.Length,
                    End = m.Groups["end"].Value,
                };
            }
        }

        /// <summary>
        /// Extract root element from XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ExtractRoot(string xml)
        {
            Match match = Regex.Match(xml, Energy.Base.Expression.XmlRootName);
            if (!match.Success)
                return "";
            else
                return match.Groups[1].Value;
        }

        /// <summary>
        /// Extract root element without namespace from XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ExtractRootShort(string xml)
        {
            string root = ExtractRoot(xml);
            if (string.IsNullOrEmpty(root) || !root.Contains(":"))
                return root;
            return root.Substring(root.LastIndexOf(":") + 1);
        }

        #endregion
    }
}
