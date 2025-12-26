using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Enumeration
{
    [TestClass]
    public class FormatType
    {
        [TestMethod]
        public void FormatType_Values()
        {
            Assert.AreEqual(0, (int)Energy.Enumeration.FormatType.Plain);
            Assert.AreEqual(1, (int)Energy.Enumeration.FormatType.Text);
            Assert.AreEqual(4, (int)Energy.Enumeration.FormatType.Number);
            Assert.AreEqual(8, (int)Energy.Enumeration.FormatType.Integer);
            Assert.AreEqual(16, (int)Energy.Enumeration.FormatType.Date);
            Assert.AreEqual(32, (int)Energy.Enumeration.FormatType.Time);
            Assert.AreEqual(64, (int)Energy.Enumeration.FormatType.Stamp);
            Assert.AreEqual(128, (int)Energy.Enumeration.FormatType.Binary);
        }

        [TestMethod]
        public void FormatType_StringRepresentation()
        {
            Assert.AreEqual("Plain", Energy.Enumeration.FormatType.Plain.ToString());
            Assert.AreEqual("Text", Energy.Enumeration.FormatType.Text.ToString());
            Assert.AreEqual("Number", Energy.Enumeration.FormatType.Number.ToString());
            Assert.AreEqual("Integer", Energy.Enumeration.FormatType.Integer.ToString());
            Assert.AreEqual("Date", Energy.Enumeration.FormatType.Date.ToString());
            Assert.AreEqual("Time", Energy.Enumeration.FormatType.Time.ToString());
            Assert.AreEqual("Stamp", Energy.Enumeration.FormatType.Stamp.ToString());
            Assert.AreEqual("Binary", Energy.Enumeration.FormatType.Binary.ToString());
        }

        [TestMethod]
        public void FormatType_ParseFromString()
        {
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Plain", out var result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Plain, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Text", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Text, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Number", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Number, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Integer", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Integer, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Date", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Date, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Time", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Time, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Stamp", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Stamp, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.FormatType>("Binary", out result));
            Assert.AreEqual(Energy.Enumeration.FormatType.Binary, result);
            
            Assert.IsFalse(Enum.TryParse<Energy.Enumeration.FormatType>("InvalidType", out result));
        }

        [TestMethod]
        public void FormatType_AllValues()
        {
            var allValues = Enum.GetValues(typeof(Energy.Enumeration.FormatType)) as Energy.Enumeration.FormatType[];
            Assert.IsNotNull(allValues);
            Assert.AreEqual(8, allValues.Length);
            
            // Verify all types are present
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Plain));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Text));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Number));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Integer));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Date));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Time));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Stamp));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.FormatType.Binary));
        }
    }
}
