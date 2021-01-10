using System;

namespace Energy.Base
{
    /// <summary>
    /// Design patterns class repository
    /// </summary>
    public static class Pattern
    {
        #region GlobalObject

        /// <summary>
        /// Represents class for "GLOBAL OBJECT" pattern.
        /// Singleton instance pattern.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GlobalObject<T>
        {
            private static T _Global;

            private readonly static object _GlobalLock = new object();

            /// <summary>
            /// Global, single, static instance of class.
            /// The object will be instantiated at first time it is used
            /// and will remain only one for all in a process until
            /// it is destroyed internally.
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
                set
                {
                    lock (_GlobalLock)
                    {
                        _Global = value;
                    }
                }
            }
        }

        #endregion

        #region GlobalDestroy

        /// <summary>
        /// Represents class for "GLOBAL DESTROYABLE OBJECT" pattern.
        /// Singleton instance pattern.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GlobalDestroy<T>
        {
            private static T _Global;

            private readonly static object _GlobalLock = new object();

            /// <summary>
            /// Global, single, static instance of class.
            /// The object will be instantiated at first time it is used
            /// and will remain only one for all in a process until
            /// it is destroyed internally.
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
                set
                {
                    lock (_GlobalLock)
                    {
                        _Global = value;
                    }
                }
            }

            /// <summary>
            /// Destroy global single instance. 
            /// Calls Dispose() if is disposable.
            /// Global single object will be created at next use.
            /// </summary>
            public static void Destroy()
            {
                if (_Global == null)
                {
                    return;
                }

                lock (_GlobalLock)
                {
                    if (_Global == null)
                    {
                        return;
                    }
                    else
                    {
                        if (_Global is IDisposable)
                        {
                            try
                            {
                                (_Global as IDisposable).Dispose();
                            }
                            catch (Exception exception)
                            {
                                Energy.Core.Bug.Catch(exception);
                            }
                        }

                        _Global = default(T);

                        Energy.Core.Bug.Write("Energy.Base.Pattern.Global", () =>
                        {
                            return string.Format("Global instance of {0} destroyed", typeof(T).FullName);
                        });
                    }
                }
            }
        }

        #endregion

        #region DefaultProperty

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

        #endregion

        #region Singleton

        /// <summary>
        /// Represents class for "SINGLETON" pattern.
        /// Singleton instance pattern.
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

                                Energy.Core.Bug.Write("Energy.Base.Pattern.Default", () =>
                                {
                                    return string.Format("Singleton {0} created", typeof(T).FullName);
                                });
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

        #endregion

        #region Builder

        /// <summary>
        /// Builder pattern.
        /// Includes static Create() method which returns new instance of a class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Builder<T>
        {
            /// <summary>
            /// Create instance of class
            /// </summary>
            public static T Create()
            {
                T o = Activator.CreateInstance<T>();
                return o;
            }
        }

        #endregion
    }
}
