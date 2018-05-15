﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for code organization
    /// </summary>
    public class Code
    {
        /// <summary>
        /// Code is temporary here and should be moved to another location
        /// </summary>
        public class TemporaryAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public TemporaryAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public TemporaryAttribute(string description)
                : this()
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public TemporaryAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }
        }

        /// <summary>
        /// Code is probably misplaced and should exist in another location
        /// </summary>
        public class MisplacedAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public MisplacedAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description">Description text</param>
            public MisplacedAttribute(string description)
                : this()
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description">Description text</param>
            /// <param name="help">Help text</param>
            public MisplacedAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }
        }

        /// <summary>
        /// Code is obsolete and should be removed
        /// </summary>
        public class ObsoleteAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            private bool error;

            /// <summary>
            /// Treat as error
            /// </summary>
            public bool Error { get { return error; } set { error = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public ObsoleteAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public ObsoleteAttribute(string description)
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="error"></param>
            public ObsoleteAttribute(string description, bool error)
                : this(description)
            {
                this.error = error;
            }
        }

        /// <summary>
        /// Code is to be written in future
        /// </summary>
        public class FutureAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            private string expected;

            /// <summary>
            /// Expected version or date to be done
            /// </summary>
            public string Expected { get { return expected; } set { expected = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public FutureAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public FutureAttribute(string description)
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="expected"></param>
            public FutureAttribute(string description, string expected)
                : this(description)
            {
                this.expected = expected;
            }
        }

        /// <summary>
        /// Code is a wrapper function whose purpose is to call another funtion
        /// </summary>
        public class WrapperAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public WrapperAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public WrapperAttribute(string description)
                : this()
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public WrapperAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }
        }

        /// <summary>
        /// Code may be a reproduction of another one, which probably needs special attention
        /// </summary>
        public class DuplicationAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public DuplicationAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public DuplicationAttribute(string description)
                : this()
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public DuplicationAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }
        }

        /// <summary>
        /// Code needs special attention
        /// </summary>
        public class AttentionAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public AttentionAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public AttentionAttribute(string description)
                : this()
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public AttentionAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }
        }

        /// <summary>
        /// Code safety warning
        /// </summary>
        public class SafetyAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            private bool error;

            /// <summary>
            /// Treat as error
            /// </summary>
            public bool Error { get { return error; } set { error = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public SafetyAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public SafetyAttribute(string description)
                : this()
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="error"></param>
            public SafetyAttribute(bool error)
                : this()
            {
                this.error = error;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public SafetyAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="error"></param>
            public SafetyAttribute(string description, bool error)
                : this(description)
            {
                this.error = error;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            /// <param name="error"></param>
            public SafetyAttribute(string description, string help, bool error)
                : this(description, help)
            {
                this.error = error;
            }
        }

        /// <summary>
        /// Code was originally used somewhere else
        /// </summary>
        public class OriginAttribute : System.Attribute
        {
            private string help;

            /// <summary>
            /// Help text
            /// </summary>
            public string Help { get { return help; } set { help = value; } }

            private string description;

            /// <summary>
            /// Description text
            /// </summary>
            public string Description { get { return description; } set { description = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public OriginAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public OriginAttribute(string description)
            {
                this.description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public OriginAttribute(string description, string help)
                : this(description)
            {
                this.help = help;
            }
        }
    }
}