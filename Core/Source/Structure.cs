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
    public partial class Structure
    {
        /// <summary>Table</summary>
        [XmlElement("Table")]
        public Energy.Base.Structure.Table.Array Tables = new Base.Structure.Table.Array();

        /// <summary>
        /// Represents difference between schemas
        /// </summary>
        public partial class Difference
        {
        }

        public bool Equals(Structure s)
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