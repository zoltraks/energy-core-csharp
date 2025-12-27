using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Enumeration
{
    [TestClass]
    public class BooleanStyle
    {
        [TestMethod]
        public void BooleanStyle_Values()
        {
            Assert.AreEqual(0, (int)Energy.Enumeration.BooleanStyle.B);
            Assert.AreEqual(1, (int)Energy.Enumeration.BooleanStyle.X);
            Assert.AreEqual(2, (int)Energy.Enumeration.BooleanStyle.V);
            Assert.AreEqual(3, (int)Energy.Enumeration.BooleanStyle.Y);
            Assert.AreEqual(4, (int)Energy.Enumeration.BooleanStyle.T);
            Assert.AreEqual(5, (int)Energy.Enumeration.BooleanStyle.YesNo);
            Assert.AreEqual(6, (int)Energy.Enumeration.BooleanStyle.TrueFalse);
        }

        [TestMethod]
        public void BooleanStyle_Aliases()
        {
            // Test that aliases have same values
            Assert.AreEqual((int)Energy.Enumeration.BooleanStyle.B, (int)Energy.Enumeration.BooleanStyle.Bit);
            Assert.AreEqual((int)Energy.Enumeration.BooleanStyle.Y, (int)Energy.Enumeration.BooleanStyle.YN);
            Assert.AreEqual((int)Energy.Enumeration.BooleanStyle.T, (int)Energy.Enumeration.BooleanStyle.TF);
        }

        [TestMethod]
        public void BooleanStyle_StringRepresentation()
        {
            Assert.AreEqual("Bit", Energy.Enumeration.BooleanStyle.B.ToString());
            Assert.AreEqual("X", Energy.Enumeration.BooleanStyle.X.ToString());
            Assert.AreEqual("V", Energy.Enumeration.BooleanStyle.V.ToString());
            Assert.AreEqual("YN", Energy.Enumeration.BooleanStyle.Y.ToString());
            Assert.AreEqual("TF", Energy.Enumeration.BooleanStyle.T.ToString());
            Assert.AreEqual("YesNo", Energy.Enumeration.BooleanStyle.YesNo.ToString());
            Assert.AreEqual("TrueFalse", Energy.Enumeration.BooleanStyle.TrueFalse.ToString());
        }

        [TestMethod]
        public void BooleanStyle_Comparison()
        {
            var styleB = Energy.Enumeration.BooleanStyle.B;
            var styleX = Energy.Enumeration.BooleanStyle.X;
            var styleBit = Energy.Enumeration.BooleanStyle.Bit;
            
            Assert.IsTrue(styleB == Energy.Enumeration.BooleanStyle.B);
            Assert.IsFalse(styleB == styleX);
            Assert.IsTrue(styleB != styleX);
            
            // Test that aliases are equal
            Assert.IsTrue(styleB == styleBit);
            
            // Test ordering
            Assert.IsTrue(styleB < styleX);
            Assert.IsTrue(styleX > styleB);
        }

        [TestMethod]
        public void BooleanStyle_ParseFromString()
        {
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.BooleanStyle>("B", out var result));
            Assert.AreEqual(Energy.Enumeration.BooleanStyle.B, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.BooleanStyle>("YesNo", out result));
            Assert.AreEqual(Energy.Enumeration.BooleanStyle.YesNo, result);
            
            Assert.IsTrue(Enum.TryParse<Energy.Enumeration.BooleanStyle>("Bit", out result));
            Assert.AreEqual(Energy.Enumeration.BooleanStyle.Bit, result);
            
            Assert.IsFalse(Enum.TryParse<Energy.Enumeration.BooleanStyle>("InvalidStyle", out result));
        }

        [TestMethod]
        public void BooleanStyle_AllValues()
        {
            var allValues = Enum.GetValues(typeof(Energy.Enumeration.BooleanStyle)) as Energy.Enumeration.BooleanStyle[];
            Assert.IsNotNull(allValues);
            Assert.IsTrue(allValues.Length >= 7); // At least the main values
            
            // Verify all main styles are present
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.B));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.X));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.V));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.Y));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.T));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.YesNo));
            Assert.IsTrue(System.Array.Exists(allValues, x => x == Energy.Enumeration.BooleanStyle.TrueFalse));
        }
    }
}
