using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
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

        #region Static

        /// <summary>
        /// Get product version for assembly
        /// </summary>
        /// <param name="assembly">System.Reflection.Assembly</param>
        /// <returns>string</returns>
        public static string GetProduct(Assembly assembly)
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
        public static string GetCompilation(Assembly assembly)
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

        #endregion

        /// <summary>
        /// Set product and compilation information
        /// </summary>
        /// <param name="assembly">System.Reflection.Assembly</param>
        public void Set(Assembly assembly)
        {
            Product = GetProduct(assembly);
            Compilation = GetCompilation(assembly);
        }

        /// <summary>
        /// Compare two versions
        /// </summary>
        /// <param name="a">string</param>
        /// <param name="b">string</param>
        /// <returns>int</returns>
        public static int Compare(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && !String.IsNullOrEmpty(b)) return -1;
            if (!String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b)) return 1;
            char[] separator = new char[] { '.', '-', '_' };
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

        #region Library version information

        private static string _LibraryProduct;
        /// <summary>Library version</summary>
        public static string LibraryVersion
        {
            get
            {
                if (_LibraryProduct == null)
                {
                    _LibraryProduct = GetProduct(Assembly.GetExecutingAssembly());
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
                    _LibraryCompilation = GetCompilation(Assembly.GetExecutingAssembly());
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
