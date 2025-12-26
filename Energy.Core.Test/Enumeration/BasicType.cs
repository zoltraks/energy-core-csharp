using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Enumeration
{
    [TestClass]
    public class BasicType
    {
        [TestMethod]
        public void BasicType_Values()
        {
            Assert.AreEqual(0, (int)Energy.Enumeration.BasicType.Text);
            Assert.AreEqual(1, (int)Energy.Enumeration.BasicType.Number);
            Assert.AreEqual(2, (int)Energy.Enumeration.BasicType.Integer);
            Assert.AreEqual(3, (int)Energy.Enumeration.BasicType.Bool);
            Assert.AreEqual(4, (int)Energy.Enumeration.BasicType.Date);
            Assert.AreEqual(5, (int)Energy.Enumeration.BasicType.Time);
            Assert.AreEqual(6, (int)Energy.Enumeration.BasicType.Stamp);
        }

        [TestMethod]
        public void BasicType_StringRepresentation()
        {
            Assert.AreEqual("Text", Energy.Enumeration.BasicType.Text.ToString());
            Assert.AreEqual("Number", Energy.Enumeration.BasicType.Number.ToString());
            Assert.AreEqual("Integer", Energy.Enumeration.BasicType.Integer.ToString());
            Assert.AreEqual("Bool", Energy.Enumeration.BasicType.Bool.ToString());
            Assert.AreEqual("Date", Energy.Enumeration.BasicType.Date.ToString());
            Assert.AreEqual("Time", Energy.Enumeration.BasicType.Time.ToString());
            Assert.AreEqual("Stamp", Energy.Enumeration.BasicType.Stamp.ToString());
        }

        [TestMethod]
        public void BasicType_Comparison()
        {
            var textType = Energy.Enumeration.BasicType.Text;
            var numberType = Energy.Enumeration.BasicType.Number;
            
            Assert.IsTrue(textType == Energy.Enumeration.BasicType.Text);
            Assert.IsFalse(textType == numberType);
            Assert.IsTrue(textType != numberType);
            
            // Test ordering
            Assert.IsTrue(textType < numberType);
            Assert.IsTrue(numberType > textType);
        }

        [TestMethod]
        public void BasicType_ParseFromString()
        {
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.BasicType>("Text", out var result));
            Assert.AreEqual(Energy.Enumeration.BasicType.Text, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.BasicType>("Number", out result));
            Assert.AreEqual(Energy.Enumeration.BasicType.Number, result);
            
            Assert.IsFalse(Enum.TryParse<Energy.Enumeration.BasicType>("InvalidType", out result));
        }
    }
}
