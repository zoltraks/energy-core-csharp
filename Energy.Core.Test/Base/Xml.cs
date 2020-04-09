using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Xml
    {
        [TestMethod]
        public void XmlExtractRootNodeLine()
        {
            string xml;
            string expect;
            string result;
            xml = @" < ?xml encoding utf > < !doctype aaa> < ro_ot  xmlns:a=""dfaf"" option_1 xmlns:b=""asdfadsf"" abc / >";
            Energy.Base.Xml.Class.XmlTagLine n;
            n = Energy.Base.Xml.ExtractRootNodeLine(xml);
            result = n.Name;
            expect = "ro_ot";
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.BoolToString(Energy.Base.Text.IsWhite(n.Attribute));
            expect = "0";
            Assert.AreEqual(expect, result);
            xml = @" < ?xml encoding utf > < !doctype aaa> < s / >";
            n = Energy.Base.Xml.ExtractRootNodeLine(xml);
            result = Energy.Base.Cast.BoolToString(Energy.Base.Text.IsWhite(n.Attribute));
            expect = "1";
            Assert.AreEqual(expect, result);
            Assert.IsTrue(n.IsSimple);
        }

        [TestMethod]
        public void XmlSerialize()
        {
            //var x = new { Text = "Text1", num = 123 };
            //var s = Energy.Base.Xml.Serialize(x);
            //Assert.IsNull(s);

            var d = new DataTable("t");
            d.Columns.Add(new DataColumn("a", typeof(string)));
            d.Columns.Add(new DataColumn("b", typeof(int)));
            DataRow dr = d.NewRow();
            dr["a"] = "Text1";
            dr["b"] = 123;
            d.Rows.Add(dr);
            var s = Energy.Base.Xml.Serialize(d);
            Assert.IsNotNull(s);
        }
    }
}
