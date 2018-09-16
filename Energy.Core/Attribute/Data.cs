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
            /// <summary>
            /// Table name.
            /// </summary>
            public string Name;

            /// <summary>
            /// Table description.
            /// </summary>
            public string Description;

            /// <summary>
            /// Identity column.
            /// </summary>
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

        public class LabelAttribute : System.Attribute
        {
            private string _Label;
            /// <summary>Caption</summary>
            public string Label { get { return _Label; } set { _Label = value; } }

            public LabelAttribute() { }

            public LabelAttribute(string caption)
            {
                this.Label = caption;
            }
        }

        public class DescriptionAttribute : System.Attribute
        {
            private string _Description;
            /// <summary>Description</summary>
            public string Description { get { return _Description; } set { _Description = value; } }

            public DescriptionAttribute() { }

            public DescriptionAttribute(string description)
            {
                this.Description = description;
            }
        }

        public class IncrementAttribute : System.Attribute
        {
            private int _Increment = 1;
            /// <summary>Increment</summary>
            public int Increment { get { return _Increment; } set { _Increment = value; } }

            public IncrementAttribute() { }

            public IncrementAttribute(int increment)
            {
                this.Increment = increment;
            }
        }

        /// <summary>
        /// Used for marking item as abstract that cannot have implementation.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
        public class AbstractAttribute : System.Attribute
        {
        }

        /// <summary>
        /// Model attribute
        /// </summary>
        public class ModelAttribute : System.Attribute
        {
            private string name;

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get { return name; } set { name = value; } }

            /// <summary>
            /// Model attribute with custom name
            /// </summary>
            /// <param name="value"></param>
            public ModelAttribute(string value)
            {
                name = value;
            }
        }

        /// <summary>
        /// What to do with null values?
        /// </summary>
        public class NullAttribute : System.Attribute
        {
            private bool ignore;

            /// <summary>
            /// Ignore column if value is null when scripting.
            /// </summary>
            public bool Ignore { get { return ignore; } set { ignore = value; } }

            private bool zero;

            /// <summary>
            /// Zero value to default when value was NULL.
            /// </summary>
            public bool Zero { get { return zero; } set { zero = value; } }

            public NullAttribute()
            {
            }
        }
    }
}
