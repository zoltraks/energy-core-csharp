using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Color
    {
        [TestMethod]
        public void ColorToString()
        {
            Energy.Base.Color c1 = "#333";
            string s1 = (string)c1;
            Debug.WriteLine(s1);
            Assert.AreEqual("#333333", s1);
            Energy.Base.Color c2 = "#ccc";
            string s2 = (string)c2;
            Debug.WriteLine(s2);
            Assert.AreEqual("#cccccc", s2);
            Energy.Base.Color c3 = Energy.Base.Color.ColorTransition(c1, c2, 0.5);
            string s3 = (string)c3;
            Debug.WriteLine(s3);
            Assert.AreEqual("#7f7f7f", s3);
        }

        [TestMethod]
        public void HslToColorTest()
        {
            // Test pure green variations
            Energy.Base.Color green1 = Energy.Base.Color.HslToColor("(120°,100%,25%)");
            Assert.AreEqual(0, green1.R);
            Assert.AreEqual(128, green1.G);
            Assert.AreEqual(0, green1.B);
            Assert.AreEqual(255, green1.A);

            Energy.Base.Color green2 = Energy.Base.Color.HslToColor("(120,100,25)");
            Assert.AreEqual(0, green2.R);
            Assert.AreEqual(128, green2.G);
            Assert.AreEqual(0, green2.B);

            Energy.Base.Color green3 = Energy.Base.Color.HslToColor("( 120 ° , 100 % , 25 % )");
            Assert.AreEqual(0, green3.R);
            Assert.AreEqual(128, green3.G);
            Assert.AreEqual(0, green3.B);

            Energy.Base.Color green4 = Energy.Base.Color.HslToColor(" 120°, 100%, 25% ");
            Assert.AreEqual(0, green4.R);
            Assert.AreEqual(128, green4.G);
            Assert.AreEqual(0, green4.B);

            // Test HSLA with alpha
            Energy.Base.Color tomato = Energy.Base.Color.HslToColor("9, 100%, 64%, 0.5");
            Assert.AreEqual(255, tomato.R);
            Assert.AreEqual(99, tomato.G);
            Assert.AreEqual(71, tomato.B);
            Assert.AreEqual(128, tomato.A);

            Energy.Base.Color tomato2 = Energy.Base.Color.HslToColor("(9°, 100%, 64%, 50%)");
            Assert.AreEqual(255, tomato2.R);
            Assert.AreEqual(99, tomato2.G);
            Assert.AreEqual(71, tomato2.B);
            Assert.AreEqual(128, tomato2.A);
        }

        [TestMethod]
        public void IsValidHslTest()
        {
            // Test valid HSL strings
            Assert.IsTrue(Energy.Base.Color.IsValidHsl("(120°,100%,25%)"));
            Assert.IsTrue(Energy.Base.Color.IsValidHsl("(120,100,25)"));
            Assert.IsTrue(Energy.Base.Color.IsValidHsl("( 120 ° , 100 % , 25 % )"));
            Assert.IsTrue(Energy.Base.Color.IsValidHsl(" 120°, 100%, 25% "));
            Assert.IsTrue(Energy.Base.Color.IsValidHsl("9, 100%, 64%, 0.5"));
            Assert.IsTrue(Energy.Base.Color.IsValidHsl("(9°, 100%, 64%, 50%)"));

            // Test invalid HSL strings
            Assert.IsFalse(Energy.Base.Color.IsValidHsl(null));
            Assert.IsFalse(Energy.Base.Color.IsValidHsl(""));
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("   "));
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("invalid"));
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("(120,100)")); // Missing lightness
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("(370,100,25)")); // Hue out of range
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("(120,150,25)")); // Saturation out of range
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("(120,100,150)")); // Lightness out of range
            Assert.IsFalse(Energy.Base.Color.IsValidHsl("(120,100,25,1.5)")); // Alpha out of range
        }
    }
}
