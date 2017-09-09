using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for markdown documentation
    /// </summary>
    public class Markdown
    {
        /// <summary>
        /// Attribute for markdown text.
        /// </summary>
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class TextAttribute : System.Attribute
        {
            /// <summary>
            /// Text
            /// </summary>
            public string Text;

            /// <summary>
            /// Constructor
            /// </summary>
            public TextAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="text"></param>
            public TextAttribute(string text)
            {
                this.Text = text;
            }
        }
    }
}
