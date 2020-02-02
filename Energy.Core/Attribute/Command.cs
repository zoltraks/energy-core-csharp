using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for command line arguments.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Attribute for command line option.
        /// Options may have additional one or more arguments and becomes program parameters.
        /// Default options with Count not specified will be treaten as program switches
        /// with corresponding boolean value - true if were set.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
        public class OptionAttribute : System.Attribute
        {
            /// <summary>
            /// Option name
            /// </summary>
            public string Name;

            /// <summary>
            /// Option single letter alternative, i.e. "?".
            /// </summary>
            public string Short;

            /// <summary>
            /// Option long name, i.e. "help".
            /// </summary>
            public string Long;

            /// <summary>
            /// Option description, prefered one line text or string identifier, i.e. "@HelpOptionDescription".
            /// </summary>
            public string Description;

            /// <summary>
            /// Option help text, used in manual.
            /// </summary>
            public string Help;

            /// <summary>
            /// Option usage example.
            /// </summary>
            public string Example;

            /// <summary>
            /// TODO Not sure if should be covered. Usage may be little bit confusing.
            /// </summary>
            public string[] Alternatives;

            /// <summary>
            /// Concatenate multiple options with specified char, i.e. "+".
            /// Otherwise last value will be taken or error may occur.
            /// </summary>
            public string Concat;

            /// <summary>
            /// May be set to 1, 2 or more values to take values from argument list (for parameters).
            /// If set to 0 option is a flag (for switches).
            /// </summary>
            public int Count;

            /// <summary>
            /// Constructor
            /// </summary>
            public OptionAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            public OptionAttribute(string name)
            {
                this.Name = name;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            public OptionAttribute(string name, string description)
                : this(name)
            {
                this.Description = name;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            /// <param name="count"></param>
            public OptionAttribute(string name, int count)
                : this(name)
            {
                this.Count = count;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            /// <param name="count"></param>
            public OptionAttribute(string name, string description, int count)
                : this(name, description)
            {
                this.Count = count;
            }
        }

        /// <summary>
        /// Argument attribute
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public class ArgumentAttribute : System.Attribute
        {
            public int Order;

            /// <summary>
            /// Constructor
            /// </summary>
            public ArgumentAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="order"></param>
            public ArgumentAttribute(int order)
            {
                this.Order = order;
            }
        }

        ///// <summary>
        ///// Welcome attribute
        ///// </summary>
        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
        //public class WelcomeAttribute : System.Attribute
        //{
        //    public string Text;

        //    /// <summary>
        //    /// Constructor
        //    /// </summary>
        //    public WelcomeAttribute()
        //    {
        //    }

        //    /// <summary>
        //    /// Constructor
        //    /// </summary>
        //    /// <param name="text"></param>
        //    public WelcomeAttribute(string text)
        //    {
        //        this.Text = text;
        //    }
        //}

        ///// <summary>
        ///// Help attribute
        ///// </summary>
        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
        //public class HelpAttribute : System.Attribute
        //{
        //    public string Text;

        //    /// <summary>
        //    /// Constructor
        //    /// </summary>
        //    public HelpAttribute()
        //    {
        //    }

        //    /// <summary>
        //    /// Constructor
        //    /// </summary>
        //    /// <param name="text"></param>
        //    public HelpAttribute(string text)
        //    {
        //        this.Text = text;
        //    }
        //}

        ///// <summary>
        ///// Switch attribute for options without values like flags, i.e. /help or --verbose.
        ///// </summary>
        //public class SwitchAttribute : System.Attribute
        //{
        //    public string Name;

        //    public string Short;

        //    public string Long;

        //    public string Description;

        //    public string[] Alternatives;

        //    /// <summary>
        //    /// Table name
        //    /// </summary>
        //    public SwitchAttribute()
        //    {
        //    }

        //    /// <summary>
        //    /// Table name
        //    /// </summary>
        //    /// <param name="name"></param>
        //    public SwitchAttribute(string name)
        //    {
        //        this.Name = name;
        //    }

        //    /// <summary>
        //    /// Table name with description
        //    /// </summary>
        //    /// <param name="name"></param>
        //    /// <param name="description"></param>
        //    public SwitchAttribute(string name, string description)
        //        : this(name)
        //    {
        //        this.Description = name;
        //    }
        //}

        ///// <summary>
        ///// Parameter attribute for options with one or more attributes.
        ///// </summary>
        //public class ParameterAttribute : System.Attribute
        //{
        //    public string Name;

        //    public string Short;

        //    public string Long;

        //    public string Description;

        //    public string Help;

        //    public string Example;

        //    public string[] Alternatives;

        //    /// <summary>
        //    /// May be set to 2 or more values to take from argument list.
        //    /// </summary>
        //    public int Count;

        //    /// <summary>
        //    /// Parameter
        //    /// </summary>
        //    public ParameterAttribute()
        //    {
        //    }

        //    /// <summary>
        //    /// Parameter
        //    /// </summary>
        //    /// <param name="name"></param>
        //    public ParameterAttribute(string name)
        //    {
        //        this.Name = name;
        //    }

        //    /// <summary>
        //    /// Parameter
        //    /// </summary>
        //    /// <param name="name"></param>
        //    /// <param name="description"></param>
        //    public ParameterAttribute(string name, string description)
        //        : this(name)
        //    {
        //        this.Description = name;
        //    }
        //}
    }
}
