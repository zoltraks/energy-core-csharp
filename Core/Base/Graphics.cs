using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Graphics
    /// </summary>
    public class Graphics
    {
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

        /// <summary>
        /// Represent color as CSS value
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>CSS string</returns>
        public static string ColorToCss(System.Drawing.Color color)
        {
            byte[] rgb = new byte[3] { color.R, color.B, color.B };
            return "#" + Energy.Base.Hex.ByteArrayToHex(rgb).ToLower();
        }
    }
}
