using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Design patterns class repository
    /// </summary>
    public class Pattern
    {
        /// <summary>
        /// Default property / Static instance pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class DefaultProperty<T>
        {
            private static T _Default;

            private readonly static object _DefaultLock = new object();

            /// <summary>
            /// Single static instance of class (default)
            /// </summary>
            public static T Default
            {
                get
                {
                    if (_Default == null)
                    {
                        lock (_DefaultLock)
                        {
                            if (_Default == null)
                            {
                                try
                                {
                                    _Default = Activator.CreateInstance<T>();
                                    Energy.Core.Bug.Write("Default instance of {0} created", typeof(T).FullName);
                                }
                                catch (Exception exception)
                                {
                                    Energy.Core.Bug.Catch(exception);
                                }
                            }
                        }
                    }
                    return _Default;
                }
            }
        }
    }
}
