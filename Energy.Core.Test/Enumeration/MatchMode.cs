using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Enumeration
{
    [TestClass]
    public class MatchMode
    {
        [TestMethod]
        public void MatchMode_Values()
        {
            Assert.AreEqual(0, (int)Energy.Enumeration.MatchMode.None);
            Assert.AreEqual(1, (int)Energy.Enumeration.MatchMode.Same);
            Assert.AreEqual(2, (int)Energy.Enumeration.MatchMode.Simple);
            Assert.AreEqual(3, (int)Energy.Enumeration.MatchMode.Wild);
            Assert.AreEqual(4, (int)Energy.Enumeration.MatchMode.Regex);
        }

        [TestMethod]
        public void MatchMode_StringRepresentation()
        {
            Assert.AreEqual("None", Energy.Enumeration.MatchMode.None.ToString());
            Assert.AreEqual("Same", Energy.Enumeration.MatchMode.Same.ToString());
            Assert.AreEqual("Simple", Energy.Enumeration.MatchMode.Simple.ToString());
            Assert.AreEqual("Wild", Energy.Enumeration.MatchMode.Wild.ToString());
            Assert.AreEqual("Regex", Energy.Enumeration.MatchMode.Regex.ToString());
        }

        [TestMethod]
        public void MatchMode_ParseFromString()
        {
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchMode>("None", out var result));
            Assert.AreEqual(Energy.Enumeration.MatchMode.None, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchMode>("Same", out result));
            Assert.AreEqual(Energy.Enumeration.MatchMode.Same, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchMode>("Simple", out result));
            Assert.AreEqual(Energy.Enumeration.MatchMode.Simple, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchMode>("Wild", out result));
            Assert.AreEqual(Energy.Enumeration.MatchMode.Wild, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.MatchMode>("Regex", out result));
            Assert.AreEqual(Energy.Enumeration.MatchMode.Regex, result);
            
            Assert.IsFalse(Enum.TryParse<Energy.Enumeration.MatchMode>("InvalidMode", out result));
        }

        [TestMethod]
        public void MatchMode_AllValues()
        {
            var allValues = Enum.GetValues(typeof(Energy.Enumeration.MatchMode)) as Energy.Enumeration.MatchMode[];
            Assert.IsNotNull(allValues);
            Assert.AreEqual(5, allValues.Length);
            
            // Verify all modes are present
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchMode.None));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchMode.Same));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchMode.Simple));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchMode.Wild));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.MatchMode.Regex));
        }
    }
}
