using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Color class
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Alpha
        /// </summary>
        public readonly byte A;

        /// <summary>
        /// Red
        /// </summary>
        public readonly byte R;

        /// <summary>
        /// Green
        /// </summary>
        public readonly byte G;

        /// <summary>
        /// Blue
        /// </summary>
        public readonly byte B;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            this.A = alpha;
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(byte red, byte green, byte blue)
        {
            this.A = 0;
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public Color(uint value)
        {
            this.A = (byte)((value & 0xff000000) >> 24);
            this.R = (byte)((value & 0x00ff0000) >> 16);
            this.G = (byte)((value & 0x0000ff00) >> 8);
            this.B = (byte)((value & 0x000000ff) >> 0);
        }

        #endregion

        #region Implicit

        /// <summary>
        /// Implicit operator
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Energy.Base.Color(uint value)
        {
            return new Color(value);
        }

        /// <summary>
        /// Implicit operator from string
        /// </summary>
        /// <param name="value">Color text</param>
        /// <returns></returns>
        public static implicit operator Energy.Base.Color(string value)
        {
            if (value == null) return null;
            value = value.Trim();
            if (value == "") return new Color();
            if (value.Length == 4 && value.StartsWith("#") || value.Length == 3 || value.Length == 6 || value.Length == 7 && value.StartsWith("#"))
            {
                return HexToColor(value);
            }
            //return new Color(System.Drawing.Color.FromName(value));

#if !NETCF
#pragma warning disable IDE0034 // Simplify 'default' expression
#endif
            return default(Color);
#if !NETCF
#pragma warning restore IDE0034 // Simplify 'default' expression
#endif
        }

        #endregion

        #region Explicit

        public static explicit operator string(Energy.Base.Color value)
        {
            return value.ToString();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Represent as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!IsSet)
                return "";
            return ColorToHtml(this);
        }

        #endregion

        #region IsSet

        /// <summary>
        /// Check if color was set
        /// </summary>
        /// <returns></returns>
        public bool IsSet
        {
            get
            {
                return R != 0 || G != 0 || B != 0 || A != 0;
            }
        }

        #endregion

        #region HexToColor

        /// <summary>
        /// Convert hexadecimal string or HTML color to System.Drawing.Color
        /// </summary>
        /// <param name="hex">"333", "#ebebeb", etc.</param>
        /// <returns>System.Drawing.Color equivalent</returns>
        public static Energy.Base.Color HexToColor(string hex)
        {
            Regex r = new Regex("\\#?(?:(?<r>[a-fA-F0-9]{2})(?<g>[a-fA-F0-9]{2})(?<b>[a-fA-F0-9]{2})|(?<r>[a-fA-F0-9])(?<g>[a-fA-F0-9])(?<b>[a-fA-F0-9]))");
            Match m = r.Match(hex);
            if (!m.Success) return 0;
            string R = m.Groups["r"].Value;
            string G = m.Groups["g"].Value;
            string B = m.Groups["b"].Value;
            if (R.Length == 1) R = new string(R[0], 2);
            if (G.Length == 1) G = new string(G[0], 2);
            if (B.Length == 1) B = new string(B[0], 2);
            byte[] b = Hex.HexToArray(R + G + B);
            return new Energy.Base.Color(b[0], b[1], b[2]);
        }

        #endregion

        #region ColorToHtml

        /// <summary>
        /// Represent color as HTML value
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>CSS string</returns>
        public static string ColorToHtml(Energy.Base.Color color)
        {
            byte[] rgb = new byte[3] { color.R, color.G, color.B };
            return "#" + Hex.ArrayToHex(rgb).ToLower();
        }

        #endregion

        #region ColorTransition

        /// <summary>
        /// Color transition
        /// </summary>
        /// <param name="color_1">System.Drawing.Color</param>
        /// <param name="color_2">System.Drawing.Color</param>
        /// <param name="percentage">double</param>
        /// <returns>System.Drawing.Color</returns>
        public static Energy.Base.Color ColorTransition(Energy.Base.Color color_1, Energy.Base.Color color_2, double percentage)
        {
            byte a, r, g, b;

            if (percentage == 0)
                return color_1;

            if (percentage < 0)
            {
                Energy.Base.Color c = color_1;
                color_1 = color_2;
                color_2 = c;
                percentage = -percentage;
            }

            if (percentage > 1)
                percentage /= 100;

            if (percentage > 1)
                percentage = 1;

            a = (byte)((int)(double)(percentage * color_1.A + (1 - percentage) * color_2.A) % 0x100);
            r = (byte)((int)(double)(percentage * color_1.R + (1 - percentage) * color_2.R) % 0x100);
            g = (byte)((int)(double)(percentage * color_1.G + (1 - percentage) * color_2.G) % 0x100);
            b = (byte)((int)(double)(percentage * color_1.B + (1 - percentage) * color_2.B) % 0x100);

            return new Color(a, r, g, b);
        }

        #endregion

        #region FlattenAlpha

        /// <summary>
        /// Flatten a single RGB channel against a white background using the provided alpha value.
        /// <br/><br/>
        /// Returns the resulting blended channel in double precision to avoid rounding loss.
        /// </summary>
        /// <param name="sourceChannel">Original channel value</param>
        /// <param name="alpha">Alpha component</param>
        /// <returns>Flattened channel value</returns>
        public static double FlattenAlpha(byte sourceChannel, byte alpha)
        {
            double normalizedAlpha = alpha / 255.0;
            return sourceChannel * normalizedAlpha + 255.0 * (1.0 - normalizedAlpha);
        }

        #endregion

        #region HSL

        /// <summary>
        /// HSL conversion helpers.
        /// </summary>
        public static class HSL
        {
            #region Private

            private static double Hue2Rgb(double p1, double q1, double t1)
            {
                if (t1 < 0) t1 += 1;
                if (t1 > 1) t1 -= 1;
                if (t1 < 1.0 / 6.0) return p1 + (q1 - p1) * 6 * t1;
                if (t1 < 1.0 / 2.0) return q1;
                if (t1 < 2.0 / 3.0) return p1 + (q1 - p1) * (2.0 / 3.0 - t1) * 6;
                return p1;
            }

            #endregion

            #region IsValidHsl

            /// <summary>
            /// Validates HSL/HSLA color string format and ranges
            /// </summary>
            /// <param name="hsl">HSL color string to validate</param>
            /// <returns>True if valid, false otherwise</returns>
            public static bool IsValidHsl(string hsl)
            {
                if (hsl == null || hsl.Trim() == "")
                    return false;

                // Validate input format - should contain H, S, L values and optionally A
                string pattern = @"^\s*\(?\s*(\d+)\s*°?\s*,\s*(\d+)\s*%?\s*,\s*(\d+)\s*%?\s*(?:,\s*(\d+(?:\.\d+)?)(?:%?)\s*)?\)?\s*$";

                if (!Regex.IsMatch(hsl, pattern))
                    return false;

                // Extract numeric values
                var matches = Regex.Matches(hsl, @"\d+(?:\.\d+)?");

                if (matches.Count < 3)
                    return false;

                // Parse HSL values
                int h = int.Parse(matches[0].Value);
                int s = int.Parse(matches[1].Value);
                int l = int.Parse(matches[2].Value);

                // Validate ranges
                if (h < 0 || h > 360)
                    return false;
                if (s < 0 || s > 100)
                    return false;
                if (l < 0 || l > 100)
                    return false;

                // Validate alpha range if present
                if (matches.Count >= 4)
                {
                    double alpha = double.Parse(matches[3].Value);
                    // Check if the original string has % after the alpha value
                    var alphaMatch = Regex.Match(hsl, @"(\d+(?:\.\d+)?)(?:%?)\s*\)?\s*$");
                    if (alphaMatch.Success && alphaMatch.Value.IndexOf("%") != -1)
                        alpha = alpha / 100.0;

                    if (alpha < 0.0 || alpha > 1.0)
                        return false;
                }

                return true;
            }

            #endregion

            #region HslToColor

            /// <summary>
            /// Converts HSL/HSLA color string to Color object
            /// </summary>
            /// <param name="hsl">HSL color string in various formats</param>
            /// <returns>New Color object with RGB values</returns>
            public static Energy.Base.Color HslToColor(string hsl)
            {
                if (hsl == null || hsl.Trim() == "")
                    throw new System.FormatException("HSL string cannot be null or empty");

                if (!IsValidHsl(hsl))
                    throw new System.FormatException("Invalid HSL format: '" + hsl + "'. Expected format: (H°, S%, L%) or (H°, S%, L%, A)");

                // Extract numeric values
                var matches = Regex.Matches(hsl, @"\d+(?:\.\d+)?");

                // Parse HSL values
                int h = int.Parse(matches[0].Value);
                int s = int.Parse(matches[1].Value);
                int l = int.Parse(matches[2].Value);

                // Parse alpha if present
                double alpha = 1.0;
                if (matches.Count >= 4)
                {
                    alpha = double.Parse(matches[3].Value);
                    // Check if the original string has % after the alpha value
                    var alphaMatch = Regex.Match(hsl, @"(\d+(?:\.\d+)?)(?:%?)\s*\)?\s*$");
                    if (alphaMatch.Success && alphaMatch.Value.IndexOf("%") != -1)
                        alpha = alpha / 100.0;
                    else
                        alpha = System.Math.Min(1.0, System.Math.Max(0.0, alpha));
                }

                // Convert HSL to RGB
                double hNormalized = h / 360.0;
                double sNormalized = s / 100.0;
                double lNormalized = l / 100.0;

                double r, g, b;

                if (sNormalized == 0)
                {
                    // Achromatic (gray)
                    r = g = b = lNormalized;
                }
                else
                {
                    double q = lNormalized < 0.5 ? lNormalized * (1 + sNormalized) : lNormalized + sNormalized - lNormalized * sNormalized;
                    double p = 2 * lNormalized - q;
                    r = Hue2Rgb(p, q, hNormalized + 1.0 / 3.0);
                    g = Hue2Rgb(p, q, hNormalized);
                    b = Hue2Rgb(p, q, hNormalized - 1.0 / 3.0);
                }

                // Convert to 0-255 range
                int rByte = (int)System.Math.Round(r * 255);
                int gByte = (int)System.Math.Round(g * 255);
                int bByte = (int)System.Math.Round(b * 255);
                int aByte = (int)System.Math.Round(alpha * 255);

                return new Energy.Base.Color((byte)aByte, (byte)rByte, (byte)gByte, (byte)bByte);
            }

            #endregion
        }

        #endregion

        #region RAL

        /// <summary>
        /// RAL conversion helpers.
        /// </summary>
        public static class RAL
        {
            #region Private

            private static double PivotLab(double value)
            {
                return value > 0.008856 ? System.Math.Pow(value, 1.0 / 3.0) : (7.787 * value) + (16.0 / 116.0);
            }

            private static LabColor RgbToLab(double r, double g, double b)
            {
                double rNorm = r / 255.0;
                double gNorm = g / 255.0;
                double bNorm = b / 255.0;

                double rLinear = rNorm <= 0.04045 ? rNorm / 12.92 : System.Math.Pow((rNorm + 0.055) / 1.055, 2.4);
                double gLinear = gNorm <= 0.04045 ? gNorm / 12.92 : System.Math.Pow((gNorm + 0.055) / 1.055, 2.4);
                double bLinear = bNorm <= 0.04045 ? bNorm / 12.92 : System.Math.Pow((bNorm + 0.055) / 1.055, 2.4);

                double x = rLinear * 0.4124 + gLinear * 0.3576 + bLinear * 0.1805;
                double y = rLinear * 0.2126 + gLinear * 0.7152 + bLinear * 0.0722;
                double z = rLinear * 0.0193 + gLinear * 0.1192 + bLinear * 0.9505;

                double xRef = x / 0.95047;
                double yRef = y / 1.00000;
                double zRef = z / 1.08883;

                double fx = PivotLab(xRef);
                double fy = PivotLab(yRef);
                double fz = PivotLab(zRef);

                double l = (116 * fy) - 16;
                double a = 500 * (fx - fy);
                double bLab = 200 * (fy - fz);

                return new LabColor(l, a, bLab);
            }

            private static double DeltaE(LabColor lab, RalEntry entry)
            {
                double dL = lab.L - entry.L;
                double dA = lab.A - entry.A;
                double dB = lab.B - entry.B;
                return System.Math.Sqrt(dL * dL + dA * dA + dB * dB);
            }

            private static RalEntry CreateRalEntry(string code, byte r, byte g, byte b)
            {
                LabColor lab = RgbToLab(r, g, b);
                string digits = ExtractDigits(code);
                Energy.Base.Color color = new Energy.Base.Color(255, r, g, b);
                return new RalEntry(code, digits, color, lab.L, lab.A, lab.B);
            }

            private static string ExtractDigits(string text)
            {
                char[] buffer = new char[text.Length];
                int position = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    char ch = text[i];
                    if (char.IsDigit(ch))
                    {
                        buffer[position++] = ch;
                    }
                }
                return new string(buffer, 0, position);
            }

            private struct LabColor
            {
                private readonly double l;
                private readonly double a;
                private readonly double b;

                public LabColor(double l, double a, double b)
                {
                    this.l = l;
                    this.a = a;
                    this.b = b;
                }

                public double L { get { return l; } }
                public double A { get { return a; } }
                public double B { get { return b; } }
            }

            private struct RalEntry
            {
                private readonly string code;
                private readonly string digits;
                private readonly Energy.Base.Color color;
                private readonly double l;
                private readonly double a;
                private readonly double b;

                public RalEntry(string code, string digits, Energy.Base.Color color, double l, double a, double b)
                {
                    this.code = code;
                    this.digits = digits;
                    this.color = color;
                    this.l = l;
                    this.a = a;
                    this.b = b;
                }

                public string Code { get { return code; } }
                public string Digits { get { return digits; } }
                public Energy.Base.Color Color { get { return color; } }
                public double L { get { return l; } }
                public double A { get { return a; } }
                public double B { get { return b; } }
            }

            private static readonly RalEntry[] RalPalette = new RalEntry[]
            {
                CreateRalEntry("RAL 7016", 56, 62, 66),
                CreateRalEntry("RAL 9005", 10, 10, 10),
                CreateRalEntry("RAL 9010", 244, 244, 244),
                CreateRalEntry("RAL 9006", 165, 165, 165),
                CreateRalEntry("RAL 7035", 215, 215, 215),
                CreateRalEntry("RAL 6005", 47, 69, 56),
                CreateRalEntry("RAL 3020", 204, 6, 5),
                CreateRalEntry("RAL 5010", 14, 70, 127),
                CreateRalEntry("RAL 8017", 69, 50, 46),
                CreateRalEntry("RAL 1021", 246, 182, 0)
            };

            private static bool TryNormalizeCode(string value, out string digits)
            {
                digits = null;
                if (value == null)
                    return false;

                string trimmed = value.Trim();
                if (trimmed == "")
                    return false;

                char[] buffer = new char[trimmed.Length];
                int position = 0;

                for (int i = 0; i < trimmed.Length; i++)
                {
                    char ch = trimmed[i];
                    if (char.IsDigit(ch))
                    {
                        buffer[position++] = ch;
                    }
                    else if (char.IsWhiteSpace(ch)
                        || ch == 'R' || ch == 'r'
                        || ch == 'A' || ch == 'a'
                        || ch == 'L' || ch == 'l'
                        || ch == '-' || ch == '_')
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (position != 4)
                    return false;

                digits = new string(buffer, 0, position);
                return true;
            }

            #endregion

            #region ColorToRal

            /// <summary>
            /// Convert RGBA color to the closest RAL catalog entry.
            /// <br/><br/>
            /// Applies alpha flattening against a white background, converts to CIELAB, and finds the smallest Delta E.
            /// </summary>
            /// <param name="color">Input color</param>
            /// <returns>RAL code string</returns>
            public static string ColorToRal(Energy.Base.Color color)
            {
                double rFlat = FlattenAlpha(color.R, color.A);
                double gFlat = FlattenAlpha(color.G, color.A);
                double bFlat = FlattenAlpha(color.B, color.A);
                LabColor lab = RgbToLab(rFlat, gFlat, bFlat);

                string closest = RalPalette[0].Code;
                double smallestDistance = System.Double.MaxValue;

                for (int i = 0; i < RalPalette.Length; i++)
                {
                    double distance = DeltaE(lab, RalPalette[i]);
                    if (distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        closest = RalPalette[i].Code;
                    }
                }

                return closest;
            }

            #endregion

            #region RalToColor

            /// <summary>
            /// Convert a RAL code back to an RGB color sample.
            /// <br/><br/>
            /// Accepts inputs such as "RAL 7016" or "7016".
            /// </summary>
            /// <param name="code">RAL identifier</param>
            /// <returns>Energy.Base.Color instance describing the RAL sample</returns>
            public static Energy.Base.Color RalToColor(string code)
            {
                string digits;
                if (!TryNormalizeCode(code, out digits))
                    throw new System.ArgumentException("Invalid RAL code format.", nameof(code));

                for (int i = 0; i < RalPalette.Length; i++)
                {
                    if (RalPalette[i].Digits == digits)
                        return RalPalette[i].Color;
                }

                throw new System.ArgumentOutOfRangeException(nameof(code), "Unknown RAL code.");
            }

            #endregion
        }

        #endregion
    }
}
