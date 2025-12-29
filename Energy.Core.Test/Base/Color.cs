using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Color
    {
        private static readonly (Energy.Base.Color Color, string Code)[] KnownRalSamples = new (Energy.Base.Color, string)[]
        {
            (new Energy.Base.Color(255, 56, 62, 66), "RAL 7016"),
            (new Energy.Base.Color(255, 14, 14, 16), "RAL 9005"),
            (new Energy.Base.Color(255, 241, 236, 225), "RAL 9010"),
            (new Energy.Base.Color(255, 161, 161, 160), "RAL 9006"),
            (new Energy.Base.Color(255, 197, 199, 196), "RAL 7035"),
            (new Energy.Base.Color(255, 17, 66, 50), "RAL 6005"),
            (new Energy.Base.Color(255, 187, 30, 16), "RAL 3020"),
            (new Energy.Base.Color(255, 0, 79, 124), "RAL 5010"),
            (new Energy.Base.Color(255, 68, 47, 41), "RAL 8017"),
            (new Energy.Base.Color(255, 246, 182, 0), "RAL 1021")
        };

        private static readonly string[] KnownRalCodes = new string[]
        {
            "RAL 7016",
            "RAL 9005",
            "RAL 9010",
            "RAL 9006",
            "RAL 7035",
            "RAL 6005",
            "RAL 3020",
            "RAL 5010",
            "RAL 8017",
            "RAL 1021"
        };

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
            Energy.Base.Color green1 = Energy.Base.Color.HSL.HslToColor("(120°,100%,25%)");
            Assert.AreEqual(0, green1.R);
            Assert.AreEqual(128, green1.G);
            Assert.AreEqual(0, green1.B);
            Assert.AreEqual(255, green1.A);

            Energy.Base.Color green2 = Energy.Base.Color.HSL.HslToColor("(120,100,25)");
            Assert.AreEqual(0, green2.R);
            Assert.AreEqual(128, green2.G);
            Assert.AreEqual(0, green2.B);

            Energy.Base.Color green3 = Energy.Base.Color.HSL.HslToColor("( 120 ° , 100 % , 25 % )");
            Assert.AreEqual(0, green3.R);
            Assert.AreEqual(128, green3.G);
            Assert.AreEqual(0, green3.B);

            Energy.Base.Color green4 = Energy.Base.Color.HSL.HslToColor(" 120°, 100%, 25% ");
            Assert.AreEqual(0, green4.R);
            Assert.AreEqual(128, green4.G);
            Assert.AreEqual(0, green4.B);

            // Test HSLA with alpha
            Energy.Base.Color tomato = Energy.Base.Color.HSL.HslToColor("9, 100%, 64%, 0.5");
            Assert.AreEqual(255, tomato.R);
            Assert.AreEqual(99, tomato.G);
            Assert.AreEqual(71, tomato.B);
            Assert.AreEqual(128, tomato.A);

            Energy.Base.Color tomato2 = Energy.Base.Color.HSL.HslToColor("(9°, 100%, 64%, 50%)");
            Assert.AreEqual(255, tomato2.R);
            Assert.AreEqual(99, tomato2.G);
            Assert.AreEqual(71, tomato2.B);
            Assert.AreEqual(128, tomato2.A);
        }

        [TestMethod]
        public void IsValidHslTest()
        {
            // Test valid HSL strings
            Assert.IsTrue(Energy.Base.Color.HSL.IsValidHsl("(120°,100%,25%)"));
            Assert.IsTrue(Energy.Base.Color.HSL.IsValidHsl("(120,100,25)"));
            Assert.IsTrue(Energy.Base.Color.HSL.IsValidHsl("( 120 ° , 100 % , 25 % )"));
            Assert.IsTrue(Energy.Base.Color.HSL.IsValidHsl(" 120°, 100%, 25% "));
            Assert.IsTrue(Energy.Base.Color.HSL.IsValidHsl("9, 100%, 64%, 0.5"));
            Assert.IsTrue(Energy.Base.Color.HSL.IsValidHsl("(9°, 100%, 64%, 50%)"));

            // Test invalid HSL strings
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl(null));
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl(""));
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("   "));
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("invalid"));
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("(120,100)")); // Missing lightness
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("(370,100,25)")); // Hue out of range
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("(120,150,25)")); // Saturation out of range
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("(120,100,150)")); // Lightness out of range
            Assert.IsFalse(Energy.Base.Color.HSL.IsValidHsl("(120,100,25,1.5)")); // Alpha out of range
        }

        [TestMethod]
        public void ColorToRalMatchesReferencePalette()
        {
            foreach (var sample in KnownRalSamples)
            {
                string closest = Energy.Base.Color.RAL.ColorToRal(sample.Color);
                Assert.AreEqual(sample.Code, closest, "Color {0} should map to {1}", sample.Color, sample.Code);
            }
        }

        [TestMethod]
        public void ColorToRalHandlesTransparency()
        {
            Energy.Base.Color fullyTransparentRed = new Energy.Base.Color(0, 204, 6, 5);
            string ralFromTransparent = Energy.Base.Color.RAL.ColorToRal(fullyTransparentRed);
            Assert.AreEqual("RAL 9016", ralFromTransparent, "Transparent colors should blend toward white");

            Energy.Base.Color semiTransparentGrey = new Energy.Base.Color(128, 56, 62, 66);
            string ralFromHalfGrey = Energy.Base.Color.RAL.ColorToRal(semiTransparentGrey);
            Assert.AreEqual("RAL 7040", ralFromHalfGrey, "Half transparent anthracite should shift toward a lighter grey tone");
        }

        [TestMethod]
        public void FlattenAlphaBlendsAgainstWhiteBackground()
        {
            Assert.AreEqual(255d, Energy.Base.Color.FlattenAlpha(0, 0), 1e-9, "Fully transparent should return white");

            double halfBlend = Energy.Base.Color.FlattenAlpha(0, 128);
            double expectedHalfBlend = 255d * (1d - 128d / 255d);
            Assert.AreEqual(expectedHalfBlend, halfBlend, 1e-9, "Half transparent black should approach computed blend");

            double opaque = Energy.Base.Color.FlattenAlpha(200, 255);
            Assert.AreEqual(200d, opaque, 1e-9, "Fully opaque should remain unchanged");
        }

        [TestMethod]
        public void RalToColorParsesKnownCodes()
        {
            (string code, Energy.Base.Color expected)[] samples = new (string, Energy.Base.Color)[]
            {
                ("RAL 7016", new Energy.Base.Color(255, 56, 62, 66)),
                ("RAL 9005", new Energy.Base.Color(255, 14, 14, 16)),
                ("RAL 9010", new Energy.Base.Color(255, 241, 236, 225)),
                ("RAL 9006", new Energy.Base.Color(255, 161, 161, 160)),
                ("RAL 7035", new Energy.Base.Color(255, 197, 199, 196)),
                ("RAL 6005", new Energy.Base.Color(255, 17, 66, 50)),
                ("RAL 3020", new Energy.Base.Color(255, 187, 30, 16)),
                ("RAL 5010", new Energy.Base.Color(255, 0, 79, 124)),
                ("RAL 8017", new Energy.Base.Color(255, 68, 47, 41)),
                ("RAL 1021", new Energy.Base.Color(255, 246, 182, 0))
            };

            foreach (var sample in samples)
            {
                Energy.Base.Color actual = Energy.Base.Color.RAL.RalToColor(sample.code);
                Assert.AreEqual(sample.expected.R, actual.R, $"Expected R component for {sample.code}");
                Assert.AreEqual(sample.expected.G, actual.G, $"Expected G component for {sample.code}");
                Assert.AreEqual(sample.expected.B, actual.B, $"Expected B component for {sample.code}");
                Assert.AreEqual(sample.expected.A, actual.A, $"Expected A component for {sample.code}");
            }
        }

        [TestMethod]
        public void RalToColorAcceptsFlexibleFormatting()
        {
            string[] variations = new[]
            {
                "ral7016",
                " RAL 7016 ",
                "RaL-7016",
                "ral 7016",
                "7016"
            };

            foreach (string input in variations)
            {
                Energy.Base.Color color = Energy.Base.Color.RAL.RalToColor(input);
                Assert.AreEqual(56, color.R, "Flexible input should still map to RAL 7016");
                Assert.AreEqual(62, color.G);
                Assert.AreEqual(66, color.B);
            }
        }

        [TestMethod]
        public void RalToColorRejectsUnknownCodes()
        {
            Assert.ThrowsException<System.ArgumentException>(() => Energy.Base.Color.RAL.RalToColor(null));
            Assert.ThrowsException<System.ArgumentException>(() => Energy.Base.Color.RAL.RalToColor("RAL7A16"));
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => Energy.Base.Color.RAL.RalToColor("RAL 9999"));
        }

        [TestMethod]
        public void TryParseHexColorSupportsAlphaFormats()
        {
            byte alpha;
            byte red;
            byte green;
            byte blue;

            Assert.IsTrue(Energy.Base.Color.TryParseHexColor("#80FFFF00", out alpha, out red, out green, out blue));
            Assert.AreEqual(128, alpha);
            Assert.AreEqual(255, red);
            Assert.AreEqual(255, green);
            Assert.AreEqual(0, blue);

            Assert.IsTrue(Energy.Base.Color.TryParseHexColor("#f000", out alpha, out red, out green, out blue));
            Assert.AreEqual(255, alpha);
            Assert.AreEqual(0, red);
            Assert.AreEqual(0, green);
            Assert.AreEqual(0, blue);

            Assert.IsTrue(Energy.Base.Color.TryParseHexColor("369", out alpha, out red, out green, out blue));
            Assert.AreEqual(255, alpha);
            Assert.AreEqual(51, red);
            Assert.AreEqual(102, green);
            Assert.AreEqual(153, blue);

            Assert.IsFalse(Energy.Base.Color.TryParseHexColor("invalid", out alpha, out red, out green, out blue));
        }
    }
}
