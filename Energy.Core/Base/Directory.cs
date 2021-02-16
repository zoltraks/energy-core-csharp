using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// File directory class for I/O operations
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// Make directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if directory exists or was created sucesfully</returns>
        public static bool Make(string path)
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
        /// Convert slash separator to System.IO.Path.DirectorySeparatorChar including both versions.
        /// </summary>
        /// <param name="path">string</param>
        public static string SetQualifiedDirectorySeparator(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if ('/' != System.IO.Path.DirectorySeparatorChar && Energy.Base.Text.Contains(path, "/"))
                {
                    path = path.Replace('/', System.IO.Path.DirectorySeparatorChar);
                }

                if ('\\' != System.IO.Path.DirectorySeparatorChar && Energy.Base.Text.Contains(path, "\\"))
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

        public static Tree<string> Tree(string root, int depth)
        {
            if (!System.IO.Directory.Exists(root))
            {
                return null;
            }
            Tree<string> tree = new Base.Tree<string>();
            var x = tree.Children.Add("x");
            x.Children.Add("y");
           
            return null;           
        }

        public static IEnumerable<string> GetAllFiles(string path, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "*";
            }
            Stack<string> pending = new Stack<string>();
            pending.Push(path);
            while (pending.Count != 0)
            {
                path = pending.Pop();
                string[] list = null;
                try
                {
                    list = System.IO.Directory.GetFiles(path, search);
                }
                catch { }
                if (list != null && list.Length != 0)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        yield return list[i];
                    }
                }
                try
                {
                    list = System.IO.Directory.GetDirectories(path);
                }
                catch { }
                if (list != null && list.Length != 0)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        pending.Push(list[i]);
                    }
                }
            }
        }
    }
}
