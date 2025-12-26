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
                if (alphaMatch.Success && alphaMatch.Value.Contains("%"))
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
                throw new System.FormatException($"Invalid HSL format: '{hsl}'. Expected format: (H°, S%, L%) or (H°, S%, L%, A)");

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
                if (alphaMatch.Success && alphaMatch.Value.Contains("%"))
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
                double Hue2Rgb(double p1, double q1, double t1)
                {
                    if (t1 < 0) t1 += 1;
                    if (t1 > 1) t1 -= 1;
                    if (t1 < 1.0 / 6.0) return p1 + (q1 - p1) * 6 * t1;
                    if (t1 < 1.0 / 2.0) return q1;
                    if (t1 < 2.0 / 3.0) return p1 + (q1 - p1) * (2.0 / 3.0 - t1) * 6;
                    return p1;
                }

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
}
