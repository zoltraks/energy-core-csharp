using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Attribute
{
    /// <summary>
    /// Attributes for code organization
    /// </summary>
    public class Code
    {
        #region Abstract

        public abstract class CodeAttribute : System.Attribute
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

            private string comment;

            /// <summary>
            /// Help text
            /// </summary>
            public string Comment { get { return comment; } set { comment = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public CodeAttribute()
            {
            }
        }

        #endregion

        /// <summary>
        /// Code is temporary here and should be moved to another location
        /// </summary>
        public class TemporaryAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public TemporaryAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code is probably misplaced and should exist in another location
        /// </summary>
        public class MisplacedAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description">Description text</param>
            /// <param name="help">Help text</param>
            public MisplacedAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code is obsolete and should be removed
        /// </summary>
        public class ObsoleteAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="error"></param>
            public ObsoleteAttribute(string description, bool error)
                : this(description)
            {
                this.Error = error;
            }
        }

        /// <summary>
        /// Code is to be written in future
        /// </summary>
        public class FutureAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public FutureAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            /// <param name="expected"></param>
            public FutureAttribute(string description, string help, string expected)
                : this(description, help)
            {
                this.Expected = expected;
            }
        }

        /// <summary>
        /// Code is bad
        /// </summary>
        public class BadAttribute : CodeAttribute
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public BadAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public BadAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public BadAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code needs some additional verification
        /// </summary>
        public class VerifyAttribute : CodeAttribute
        {
            private string expected;

            /// <summary>
            /// Expected version or date to be done
            /// </summary>
            public string Expected { get { return expected; } set { expected = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public VerifyAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public VerifyAttribute(string description)
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public VerifyAttribute(string description, string help)
                : this(help)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code needs to be extended
        /// </summary>
        public class ExtendAttribute : CodeAttribute
        {
            private string expected;

            /// <summary>
            /// Expected version or date to be done
            /// </summary>
            public string Expected { get { return expected; } set { expected = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public ExtendAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public ExtendAttribute(string description)
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public ExtendAttribute(string description, string help)
                : this(help)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code is a wrapper function whose purpose is to call another funtion
        /// </summary>
        public class WrapperAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public WrapperAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code may be a reproduction of another one, which probably needs special attention
        /// </summary>
        public class DuplicationAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public DuplicationAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code needs special attention
        /// </summary>
        public class AttentionAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public AttentionAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Benchmark should be done with this code
        /// </summary>
        public class BenchmarkAttribute : CodeAttribute
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public BenchmarkAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public BenchmarkAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public BenchmarkAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code safety warning
        /// </summary>
        public class SafetyAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="error"></param>
            public SafetyAttribute(bool error)
                : this()
            {
                this.Error = error;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public SafetyAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="error"></param>
            public SafetyAttribute(string description, bool error)
                : this(description)
            {
                this.Error = error;
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
                this.Error = error;
            }
        }

        /// <summary>
        /// Code was originally used somewhere else
        /// </summary>
        public class OriginAttribute : CodeAttribute
        {
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
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public OriginAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Should be renamed
        /// </summary>
        public class RenameAttribute : CodeAttribute
        {
            private string target;

            /// <summary>
            /// Target name
            /// </summary>
            public string Target { get { return target; } set { target = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public RenameAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public RenameAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public RenameAttribute(string target, string description)
                : this()
            {
                this.Target = target;
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public RenameAttribute(string target, string description, string help)
                : this(target, description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code is just a draft and should yet to be written
        /// </summary>
        public class DraftAttribute : CodeAttribute
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DraftAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public DraftAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public DraftAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code needs to be written
        /// </summary>
        public class ImplementAttribute : CodeAttribute
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ImplementAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public ImplementAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public ImplementAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Code was abandoned and should be removed
        /// </summary>
        public class AbandonedAttribute : CodeAttribute
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public AbandonedAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public AbandonedAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public AbandonedAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }

        /// <summary>
        /// Improve that thing
        /// </summary>
        public class ImproveAttribute : CodeAttribute
        {
            private string expected;

            /// <summary>
            /// Expected version or date to be done
            /// </summary>
            public string Expected { get { return expected; } set { expected = value; } }

            /// <summary>
            /// Constructor
            /// </summary>
            public ImproveAttribute()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            public ImproveAttribute(string description)
                : this()
            {
                this.Description = description;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="description"></param>
            /// <param name="help"></param>
            public ImproveAttribute(string description, string help)
                : this(description)
            {
                this.Help = help;
            }
        }
    }
}
