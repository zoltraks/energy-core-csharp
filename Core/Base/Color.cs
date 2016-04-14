using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Color class
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Private System.Drawing.Color value
        /// </summary>
        private System.Drawing.Color value = System.Drawing.Color.Transparent;

        /// <summary>
        /// Public System.Drawing.Color value
        /// </summary>
        public System.Drawing.Color Value { get; set; }

        /// <summary>
        /// Implicit operator from System.Drawing.Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static implicit operator Color(System.Drawing.Color color)
        {
            return new Color(color);
        }

        /// <summary>
        /// Implicit operator from string
        /// </summary>
        /// <param name="value">Color text</param>
        /// <returns></returns>
        public static implicit operator Color(string value)
        {
            if (value == null) return null;
            value = value.Trim();
            if (value == "") return new Color();
            if (value.Length == 4 && value.StartsWith("#") || value.Length == 3 || value.Length == 6 || value.Length == 7 && value.StartsWith("#"))
            {
                System.Drawing.Color result = HexToColor(value);
                if (result != System.Drawing.Color.Transparent)
                    return new Color() { Value = result };
            }

            return new Color(System.Drawing.Color.FromName(value));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Color()
        {
            Value = System.Drawing.Color.Transparent;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>
        public Color(System.Drawing.Color color)
        {
            Value = color;
        }

        /// <summary>
        /// Represent as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!IsSet()) return "";
            return ColorToHtml(Value);
        }

        /// <summary>
        /// Check if color was set
        /// </summary>
        /// <returns></returns>
        public bool IsSet()
        {
            return Value != System.Drawing.Color.Transparent;
        }

        /// <summary>
        /// Convert hexadecimal string or HTML color to System.Drawing.Color
        /// </summary>
        /// <param name="hex">333, #ebebeb, etc.</param>
        /// <returns>System.Drawing.Color equivalent</returns>
        public static System.Drawing.Color HexToColor(string hex)
        {
            Regex r = new Regex("\\#?(?:(?<r>[a-fA-F0-9]{2})(?<g>[a-fA-F0-9]{2})(?<b>[a-fA-F0-9]{2})|(?<r>[a-fA-F0-9])(?<g>[a-fA-F0-9])(?<b>[a-fA-F0-9]))");
            Match m = r.Match(hex);
            if (!m.Success) return System.Drawing.Color.Transparent;
            string R = m.Groups["r"].Value;
            string G = m.Groups["g"].Value;
            string B = m.Groups["b"].Value;
            if (R.Length == 1) R = new string(R[0], 2);
            if (G.Length == 1) G = new string(G[0], 2);
            if (B.Length == 1) B = new string(B[0], 2);
            byte[] b = Hex.HexToArray(R + G + B);
            return System.Drawing.Color.FromArgb(255, b[0], b[1], b[2]);
        }

        /// <summary>
        /// Represent color as HTML value
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>CSS string</returns>
        public static string ColorToHtml(System.Drawing.Color color)
        {
            byte[] rgb = new byte[3] { color.R, color.B, color.B };
            return "#" + Hex.ArrayToHex(rgb).ToLower();
        }

        /// <summary>
        /// Color transition
        /// </summary>
        /// <param name="color_1">System.Drawing.Color</param>
        /// <param name="color_2">System.Drawing.Color</param>
        /// <param name="percentage">double</param>
        /// <returns>System.Drawing.Color</returns>
        public static System.Drawing.Color ColorTransition(System.Drawing.Color color_1, System.Drawing.Color color_2, double percentage)
        {
            int a, r, g, b;

            if (percentage < 0) percentage = 0;
            if (percentage > 1) percentage = percentage / 100;
            if (percentage > 1) percentage = 1;

            a = (int)(double)(percentage * color_1.A + (1 - percentage) * color_2.A);
            r = (int)(double)(percentage * color_1.R + (1 - percentage) * color_2.R);
            g = (int)(double)(percentage * color_1.G + (1 - percentage) * color_2.G);
            b = (int)(double)(percentage * color_1.B + (1 - percentage) * color_2.B);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
    }
}
