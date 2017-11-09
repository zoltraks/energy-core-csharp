using System;
using System.Collections.Generic;
using System.Text;
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
            this.A = (byte)(value & 0xff000000 >> 24);
            this.R = (byte)(value & 0xff00ffff >> 16);
            this.G = (byte)(value & 0xffff00ff >> 8);
            this.B = (byte)(value & 0xffffff00 >> 0);
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
            return default(Color);
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
        /// <param name="hex">333, #ebebeb, etc.</param>
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
                percentage = percentage / 100;

            if (percentage > 1)
                percentage = 1;

            a = (byte)((int)(double)(percentage * color_1.A + (1 - percentage) * color_2.A) % 0x100);
            r = (byte)((int)(double)(percentage * color_1.R + (1 - percentage) * color_2.R) % 0x100);
            g = (byte)((int)(double)(percentage * color_1.G + (1 - percentage) * color_2.G) % 0x100);
            b = (byte)((int)(double)(percentage * color_1.B + (1 - percentage) * color_2.B) % 0x100);

            return new Color(a, r, g, b);
        }
    }
}
