using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// File directory class for I/O operations
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Make directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool MakeDirectory(string path)
        {
            if (String.IsNullOrEmpty(path)) return false;
            SetQualifiedDirectorySeparator(ref path);
            path = ExcludeTrailingPathSeparator(path);
            if (System.IO.Directory.Exists(path)) return true;
            string[] array = path.Split(System.IO.Path.DirectorySeparatorChar);
            int i;
            path = array[i = 0];
            do
            {
                if (!System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    catch
                    {
                        return false;
                    }
                }
                if (++i < array.Length)
                {
                    path += System.IO.Path.DirectorySeparatorChar + array[i];
                }
                else
                {
                    break;
                }
            }
            while (true);
            return true;
        }

        /// <summary>
        /// Convert slash separator to System.IO.Path.DirectorySeparatorChar
        /// </summary>
        /// <param name="path">string</param>
        public static string SetQualifiedDirectorySeparator(ref string path)
        {
            if (path == null)
            {
                path = "";
            }
            else if (path != "")
            {
                if ('/' != System.IO.Path.DirectorySeparatorChar)
                {
                    path = path.Replace('/', System.IO.Path.DirectorySeparatorChar);
                }
            }
            return path;
        }

        /// <summary>
        /// Convert slash separator to System.IO.Path.DirectorySeparatorChar
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string SetQualifiedDirectorySeparator(string path)
        {
            string result = path;
            SetQualifiedDirectorySeparator(result);
            return result;
        }

        /// <summary>
        /// Include traling path directory separator
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string IncludeTrailingPathSeparator(string path)
        {
            if (String.IsNullOrEmpty(path)) return "";
            if (System.IO.Path.DirectorySeparatorChar != '/')
            {
                path = path.Replace('/', System.IO.Path.DirectorySeparatorChar);
            }
            if (!path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                path += System.IO.Path.DirectorySeparatorChar;
            }
            return path;
        }

        /// <summary>
        /// Include leading root directory to the path if not specified
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="root">string</param>
        /// <returns>string</returns>
        public static string IncludeLeadingRoot(string path, string root)
        {
            string check = path.Trim().Replace("\\", "/");
            if (check.StartsWith("/") || (new Regex("^[a-z]:/", RegexOptions.IgnoreCase)).Match(check).Success)
            {
                return path;
            }
            return IncludeTrailingPathSeparator(root) + path;
        }

        /// <summary>
        /// Exclude traling path directory separator
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string ExcludeTrailingPathSeparator(string path)
        {
            if (System.IO.Path.DirectorySeparatorChar != '/')
            {
                path = path.Replace('/', System.IO.Path.DirectorySeparatorChar);
            }
            if (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                return path.Substring(0, path.Length - System.IO.Path.DirectorySeparatorChar.ToString().Length);
            }
            return path;
        }
    }
}
