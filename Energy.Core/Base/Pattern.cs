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
        /// Global property / Singleton instance pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GlobalProperty<T>
        {
            private static T _Global;

            private readonly static object _GlobalLock = new object();

            /// <summary>
            /// Static instance of class (default)
            /// </summary>
            public static T Global
            {
                get
                {
                    if (_Global == null)
                    {
                        lock (_GlobalLock)
                        {
                            if (_Global == null)
                            {
                                try
                                {
                                    _Global = Activator.CreateInstance<T>();
                                    Energy.Core.Bug.Write("Energy.Base.Pattern.Global", () =>
                                    {
                                        return string.Format("Global instance of {0} created", typeof(T).FullName);
                                    });
                                }
                                catch (Exception exception)
                                {
                                    Energy.Core.Bug.Catch(exception);
                                }
                            }
                        }
                    }
                    return _Global;
                }
            }
        }

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
                                    Energy.Core.Bug.Write("Energy.Base.Pattern.Default", () =>
                                    {
                                        return string.Format("Default instance of {0} created", typeof(T).FullName);
                                    });
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

        /// <summary>
        /// Singleton pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Singleton<T>
        {
            private static T _Instance;

            private readonly static object _InstanceLock = new object();

            public T Instance
            {
                get
                {
                    return GetInstance();
                }
            }

            public T GetInstance()
            {
                if (_Instance == null)
                {
                    lock (_InstanceLock)
                    {
                        if (_Instance == null)
                        {
                            try
                            {
                                _Instance = Activator.CreateInstance<T>();
                                Energy.Core.Bug.Write(string.Format("Singleton {0} created", typeof(T).FullName));
                            }
                            catch (Exception exception)
                            {
                                Energy.Core.Bug.Catch(exception);
                            }
                        }
                    }
                }
                return _Instance;
            }
        }
    }
}
