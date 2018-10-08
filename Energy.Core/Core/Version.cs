#if CFNET
    //
#elif WindowsCE || PocketPC || WINDOWS_PHONE
    //
#define CFNET
#elif COMPACT_FRAMEWORK
//
#define CFNET
#else
    //
#endif

using System;
using System.Diagnostics;

namespace Energy.Core
{
    /// <summary>
    /// Version information
    /// </summary>
    public class Version
    {
        /// <summary>
        /// Get product version
        /// </summary>
        /// <returns>string</returns>
        public string Product { get; private set; }

        /// <summary>
        /// Get compilation date
        /// </summary>
        /// <returns>string</returns>
        public string Compilation { get; private set; }

        private string description;
        /// <summary>
        /// Public version description
        /// </summary>
        public string Description
        {
            get
            {
                if (!String.IsNullOrEmpty(description)) return description;
                return String.Format("Version {0} Compilation {1}", Product, Compilation);
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Set product and compilation information
        /// </summary>
        /// <param name="assembly">System.Reflection.Assembly</param>
#if !NETSTANDARD && !CFNET
        [System.Security.Permissions.EnvironmentPermissionAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Unrestricted = true)]
#endif
        public void Set(System.Reflection.Assembly assembly)
        {
            Product = GetProduct(assembly);
            Compilation = GetCompilation(assembly);
        }

        #region Static

/// <summary>
/// Get product version for assembly
/// </summary>
/// <param name="assembly">System.Reflection.Assembly</param>
        /// <returns>string</returns>
#if !NETSTANDARD && !CFNET
        [System.Security.Permissions.EnvironmentPermissionAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Unrestricted = true)]
#endif
        public static string GetProduct(System.Reflection.Assembly assembly)
        {
            try
            {
                return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get compilation date for assembly
        /// </summary>
        /// <param name="assembly">System.Reflection.Assembly</param>
        /// <returns>string</returns>
        public static string GetCompilation(System.Reflection.Assembly assembly)
        {
            try
            {
                System.Version version = assembly.GetName().Version;
                return String.Format("{0:0000}-{1:00}-{2:00}", version.Major, version.Minor, version.Build);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Compare two versions
        /// </summary>
        /// <param name="a">First</param>
        /// <param name="b">Second</param>
        /// <param name="separator">Separator character list</param>
        /// <returns>int</returns>
        public static int Compare(string a, string b, char[] separator)
        {
            if (String.IsNullOrEmpty(a) && !String.IsNullOrEmpty(b)) return -1;
            if (!String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return 1;
            string[] sa = a.Split(separator);
            string[] sb = b.Split(separator);
            for (int i = 0; i < sa.Length; i++)
            {
                int ia = Energy.Base.Cast.StringToInteger(sa[i]);
                int ib = sb.Length > i ? Energy.Base.Cast.StringToInteger(sb[i]) : 0;
                if (ia < ib) return +1;
                if (ia > ib) return -1;
            }
            return 0;
        }

        /// <summary>
        /// Compare two versions
        /// </summary>
        /// <param name="a">string</param>
        /// <param name="b">string</param>
        /// <returns>int</returns>
        public static int Compare(string a, string b)
        {
            return Compare(a, b, new char[] { '.', '-', '_' });
        }

#endregion

        #region Library version information

        private static string _LibraryProduct;
        /// <summary>Library version</summary>
        public static string LibraryVersion
        {
#if !NETSTANDARD && !CFNET
            [System.Security.Permissions.EnvironmentPermissionAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Unrestricted = true)]
#endif
            get
            {
                if (_LibraryProduct == null)
                {
                    _LibraryProduct = GetProduct(System.Reflection.Assembly.GetExecutingAssembly());
                }
                return _LibraryProduct;
            }
            set
            {
                _LibraryProduct = value;
            }
        }

        private static string _LibraryCompilation;
        /// <summary>Library compilation</summary>
        public static string LibraryCompilation
        {
            get
            {
                if (_LibraryCompilation == null)
                {
                    _LibraryCompilation = GetCompilation(System.Reflection.Assembly.GetExecutingAssembly());
                }
                return _LibraryCompilation;
            }
            set
            {
                _LibraryCompilation = value;
            }
        }

#endregion
    }
}
