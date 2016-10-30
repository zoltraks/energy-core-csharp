﻿using System;
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
        /// <returns>True if directory exists or was created sucesfully</returns>
        public static bool MakeDirectory(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }
            path = SetQualifiedDirectorySeparator(path);
            path = ExcludeTrailingPathSeparator(path);
            if (System.IO.Directory.Exists(path))
            {
                return true;
            }
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
        public static string SetQualifiedDirectorySeparator(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if ('/' != System.IO.Path.DirectorySeparatorChar && path.Contains("/"))
                {
                    path = path.Replace('/', System.IO.Path.DirectorySeparatorChar);
                }

                if ('\\' != System.IO.Path.DirectorySeparatorChar && path.Contains("\\"))
                {
                    path = path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
                }
            }
            return path;
        }

        /// <summary>
        /// Include trailing path directory separator if not exists.
        /// To avoid platform specific directory separator character problem,
        /// like adding backslash after slash, use SetQualifiedDirectorySeparator
        /// method on path string before.
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string IncludeTrailingPathSeparator(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (!path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    path += System.IO.Path.DirectorySeparatorChar;
                }
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
            if (!String.IsNullOrEmpty(path))
            {
                string s = System.IO.Path.DirectorySeparatorChar.ToString();
                if (path.EndsWith(s))
                {
                    path = path.Substring(0, path.Length - s.Length);
                }
            }
            return path;
        }

        /// <summary>
        /// Get directory path for a file without trailing directory separator.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetDirectory(string file)
        {
            if (file == null) return null;
            if (file.EndsWith("\\") || file.EndsWith("/"))
            {
                return file.Substring(0, file.Length - 1);
            }
            return System.IO.Path.GetDirectoryName(file);
        }
    }
}
