using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Enumeration
{
    [TestClass]
    public class MatchStyle
    {
        [TestMethod]
        public void MatchStyle_Values()
        {
            Assert.AreEqual(0, (int)Energy.Enumeration.MatchStyle.Any);
            Assert.AreEqual(1, (int)Energy.Enumeration.MatchStyle.All);
            Assert.AreEqual(2, (int)Energy.Enumeration.MatchStyle.One);
            Assert.AreEqual(3, (int)Energy.Enumeration.MatchStyle.Not);
        }

        [TestMethod]
        public void MatchStyle_StringRepresentation()
        {
            Assert.AreEqual("Any", Energy.Enumeration.MatchStyle.Any.ToString());
            Assert.AreEqual("All", Energy.Enumeration.MatchStyle.All.ToString());
            Assert.AreEqual("One", Energy.Enumeration.MatchStyle.One.ToString());
            Assert.AreEqual("Not", Energy.Enumeration.MatchStyle.Not.ToString());
        }

        [TestMethod]
        public void MatchStyle_ParseFromString()
        {
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchStyle>("Any", out var result));
            Assert.AreEqual(Energy.Enumeration.MatchStyle.Any, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchStyle>("All", out result));
            Assert.AreEqual(Energy.Enumeration.MatchStyle.All, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchStyle>("One", out result));
            Assert.AreEqual(Energy.Enumeration.MatchStyle.One, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchStyle>("Not", out result));
            Assert.AreEqual(Energy.Enumeration.MatchStyle.Not, result);
            
            Assert.IsFalse(Enum.TryParse<Energy.Enumeration.MatchStyle>("InvalidStyle", out result));
        }

        [TestMethod]
        public void MatchStyle_AllValues()
        {
            var allValues = Enum.GetValues(typeof(Energy.Enumeration.MatchStyle)) as Energy.Enumeration.MatchStyle[];
            Assert.IsNotNull(allValues);
            Assert.AreEqual(4, allValues.Length);
            
            // Verify all styles are present
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchStyle.Any));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchStyle.All));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchStyle.One));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchStyle.Not));
        }
    }
}
