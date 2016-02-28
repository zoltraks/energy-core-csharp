using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Energy.Base
{
    /// <summary>
    /// File system related functions
    /// </summary>
    public class File
    {
        /// <summary>
        /// Get directory name from file path. Returns path itself if it looks like directory.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Directory name</returns>
        public static string GetDirectory(string path)
        {
            if (String.IsNullOrEmpty(path)) return "";
            if (IsDirectory(path)) return path;
            try
            {
                return System.IO.Path.GetDirectoryName(path);
            }
            catch
            {
                return "";
            }
        }
        
        /// <summary>
                 /// Return unique name for file
                 /// </summary>
                 /// <param name="file">string</param>
                 /// <param name="path">string</param>
                 /// <param name="reserve">bool</param>
                 /// <returns>string</returns>
        public static string FileUniqueIdentity(string file, string path, bool reserve)
        {
            if (String.IsNullOrEmpty(path)) return null;
            if (!System.IO.Directory.Exists(path)) return null;

            string extension = System.IO.Path.GetExtension(file);
            string simple = System.IO.Path.GetFileNameWithoutExtension(file);
            string iterator, unique;

            lock (one)
            {
                int n = 0;
                do
                {
                    iterator = n < 1 ? "" : n.ToString();
                    unique = System.IO.Path.Combine(path, simple + iterator + extension);
                    n++;
                }
                while (System.IO.File.Exists(unique));

                if (reserve)
                {
                    try
                    {
                        System.IO.File.Create(unique).Close();
                    }
                    catch
                    {
                        return null;
                    }
                }

                return unique;
            }
        }

        /// <summary>
        /// Return unique name for file
        /// </summary>
        /// <param name="file">string</param>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string FileUniqueIdentity(string file, string path)
        {
            return FileUniqueIdentity(file, path, false);
        }

        /// <summary>
        /// Return unique name for file
        /// </summary>
        /// <param name="file">string</param>
        /// <param name="reserve">bool</param>
        /// <returns>string</returns>
        public static string FileUniqueIdentity(string file, bool reserve)
        {
            return FileUniqueIdentity(file, System.IO.Path.GetDirectoryName(file), reserve);
        }

        /// <summary>
        /// Return unique name for file
        /// </summary>
        /// <param name="file">string</param>
        /// <returns>string</returns>
        public static string FileUniqueIdentity(string file)
        {
            return FileUniqueIdentity(file, System.IO.Path.GetDirectoryName(file), false);
        }

        /// <summary>
        /// Check if path is directory
        /// </summary>
        /// <param name="file">string</param>
        /// <returns>bool</returns>
        public static bool IsDirectory(string file)
        {
            try
            {
                System.IO.FileAttributes attributes = System.IO.File.GetAttributes(file);
                return (attributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Include traling path directory separator if needed
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string IncludeTrailingPathSeparator(string path)
        {
            if (String.IsNullOrEmpty(path)) return "";
            if (System.IO.Path.DirectorySeparatorChar != '/' && path.Contains("/"))
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

        private static readonly object one = new object();
        private static Random random = new Random();

        /// <summary>
        /// Return unique file name
        /// </summary>
        /// <param name="file">string</param>
        /// <returns>string</returns>
        public static string UniqueFileName(string file)
        {
            return UniqueFileName(System.IO.Path.GetDirectoryName(file), System.IO.Path.GetFileNameWithoutExtension(file) + "-"
                , System.IO.Path.GetExtension(file), true, false);
        }

        /// <summary>
        /// Return unique file name
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="prefix">string</param>
        /// <param name="suffix">string</param>
        /// <param name="randomize">bool</param>
        /// <param name="reserve">bool</param>
        /// <returns>string</returns>
        public static string UniqueFileName(string path, string prefix, string suffix, bool randomize, bool reserve)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (!System.IO.Directory.Exists(path))
                {
                    return null;
                }
                path = IncludeTrailingPathSeparator(path);
            }

            int iterator = randomize ? 1 + random.Next(9999) : 0;
            int life = 100;

            lock (one)
            {
                while (true)
                {
                    string append = randomize ? String.Format("_{0:0000}", iterator) : (iterator > 0 ? "_" + iterator : null);

                    string unique = String.Format("{0}{1}{2:yyMMdd_HHmmss}{3}{4}", path, prefix, DateTime.Now, append, suffix);

                    if (System.IO.File.Exists(unique) || System.IO.File.Exists(unique))
                    {
                        if (randomize)
                        {
                            iterator = 1 + random.Next(9999);
                        }
                        else
                        {
                            iterator++;
                        }
                    }
                    else
                    {
                        if (reserve)
                        {
                            try
                            {
                                System.IO.File.Create(unique).Close();
                            }
                            catch (UnauthorizedAccessException)
                            {
                                return null;
                            }
                            catch
                            {
                                if (--life < 0)
                                {
                                    return null;
                                }
                                else
                                {
                                    Thread.Sleep(0);
                                    continue;
                                }
                            }
                        }

                        return unique;
                    }
                }
            }
        }

        /// <summary>
        /// Return unique file name
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="prefix">string</param>
        /// <param name="suffix">string</param>
        /// <returns>string</returns>
        public static string UniqueFileName(string path, string prefix, string suffix)
        {
            return UniqueFileName(path, prefix, suffix, false, false);
        }

        /// <summary>
        /// Get absolute path
        /// </summary>
        /// <param name="file">string</param>
        /// <returns>string</returns>
        public static string AbsolutePath(string file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get absolute path
        /// </summary>
        /// <param name="file">string</param>
        /// <param name="current">string</param>
        /// <returns>string</returns>
        public static string AbsolutePath(string file, string current)
        {          
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove useless files from list (zero sized or files that not exist)
        /// </summary>
        /// <param name="files">string[]</param>
        /// <returns>string[]</returns>
        public static string[] FileRemoveUseless(string[] files)
        {
            if (files == null)
            {
                return null;
            }

            List<string> list = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    if (System.IO.File.Exists(files[i]))
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(files[i]);
                        if (info.Length > 0)
                        {
                            list.Add(files[i]);
                        }
                    }
                }
                catch { }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Locate file
        /// </summary>
        /// <param name="command">string</param>
        /// <returns>string</returns>
        public static string Locate(string command)
        {
            return Locate(command, System.Environment.GetEnvironmentVariable("PATH").Split(';'));
        }

        /// <summary>
        /// Locate file
        /// </summary>
        /// <param name="command">string</param>
        /// <param name="search">string[]</param>
        /// <returns>string</returns>
        public static string Locate(string command, string[] search)
        {
            if (System.IO.File.Exists(command)) return command;
            if (search == null || search.Length == 0) return "";
            bool extension = command.Contains(".");
            string separator = System.IO.Path.DirectorySeparatorChar.ToString();
            string[] executable = new string[] { ".exe", ".cmd", ".bat" };
            string candidate;
            foreach (string directory in search)
            {
                string path = directory;
                if (!path.EndsWith(separator)) path += separator;
                if (extension)
                {
                    if (System.IO.File.Exists(candidate = path + command))
                    {
                        return candidate;
                    }
                }
                else
                {
                    foreach (string suffix in executable)
                    {
                        if (System.IO.File.Exists(candidate = path + command + suffix))
                        {
                            return candidate;
                        }
                    }
                }
            }
            return "";
        }
    }
}
