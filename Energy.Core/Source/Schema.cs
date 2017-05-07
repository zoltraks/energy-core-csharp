using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Energy.Source
{
    /// <summary>
    /// Structure definition
    /// </summary>
    public partial class Schema
    {
        /// <summary>
        /// List of other structures to include
        /// </summary>
        [XmlElement("Include")]
        public List<string> Includes = new List<string>();

        /// <summary>Table</summary>
        [XmlElement("Table")]
        public Energy.Source.Structure.Table.Array Tables = new Energy.Source.Structure.Table.Array();

        /// <summary>
        /// Represents difference between structures
        /// </summary>
        public partial class Difference
        {
        }

        public bool Equals(Schema s)
        {
            if (s.Tables.Count != Tables.Count)
                return false;
            for (int i = 0; i < Tables.Count; i++)
            {
                if (!Tables[i].Equals(s.Tables[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}