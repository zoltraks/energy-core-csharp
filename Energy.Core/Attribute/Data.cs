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
            public string Name;

            public string Description;

            public string Identity;

            /// <summary>
            /// Table name
            /// </summary>
            public TableAttribute()
            {
            }

            /// <summary>
            /// Table name
            /// </summary>
            /// <param name="name"></param>
            public TableAttribute(string name)
            {
                this.Name = name;
            }

            /// <summary>
            /// Table name with description
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            public TableAttribute(string name, string description)
                : this(name)
            {
                this.Description = name;
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

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class AttributeAttribute : System.Attribute
        {
            private string name;

            private string value;

            public AttributeAttribute(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }
    }
}
