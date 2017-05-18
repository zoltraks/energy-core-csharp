using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for data binding
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Column attribute
        /// </summary>
        public class ColumnAttribute : System.Attribute
        {
            private string name;

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get { return name; } set { name = value; } }

            /// <summary>
            /// Column attribute with custom column name
            /// </summary>
            /// <param name="value"></param>
            public ColumnAttribute(string value)
            {
                name = value;
            }
        }

        /// <summary>
        /// Table attribute
        /// </summary>
        public class TableAttribute : System.Attribute
        {
            private string name;

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get { return name; } set { name = value; } }

            /// <summary>
            /// Column attribute with custom column name
            /// </summary>
            /// <param name="value"></param>
            public TableAttribute(string value)
            {
                name = value;
            }
        }

        /// <summary>
        /// Element attribute. This would act like simple DataContract attribute without .NET 4.
        /// </summary>
        public class ElementAttribute : System.Attribute
        {
            private string name;

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get { return name; } set { name = value; } }

            /// <summary>
            /// Column attribute with custom column name
            /// </summary>
            /// <param name="value"></param>
            public ElementAttribute(string value)
            {
                name = value;
            }
        }

        /// <summary>
        /// Length attribute
        /// </summary>
        public class LengthAttribute : System.Attribute
        {
            private int length;

            /// <summary>
            /// Length
            /// </summary>
            public int Length { get { return length; } set { length = value; } }

            /// <summary>
            /// Column attribute with custom length
            /// </summary>
            /// <param name="length"></param>
            public LengthAttribute(int length)
            {
                this.length = length;
            }
        }

        /// <summary>
        /// Type attribute
        /// </summary>
        public class TypeAttribute : System.Attribute
        {
            private string name;

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get { return name; } set { name = value; } }

            /// <summary>
            /// Column attribute with custom column name
            /// </summary>
            /// <param name="value"></param>
            public TypeAttribute(string value)
            {
                name = value;
            }
        }

        /// <summary>
        /// Primary key attribute
        /// </summary>
        public class PrimaryAttribute : System.Attribute
        {
        }
    }
}
