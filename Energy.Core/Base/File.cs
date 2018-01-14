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
        #region Naming

        /// <summary>
        /// Get filename without leading directory path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            int position = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (position >= 0)
                path = path.Substring(position + 1);
            return path;
        }

        /// <summary>
        /// Get filename extension. Will return with suffix like ".xml" except
        /// for names starting with dot like ".gitignore" where function will
        /// result with empty string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            path = GetName(path);
            if (string.IsNullOrEmpty(path))
                return path;
            int dot = path.LastIndexOf('.');
            if (dot <= 0)
                return "";
            path = path.Substring(dot);
            return path;
        }

        /// <summary>
        /// Get file root path. Works also with UNC and protocol paths.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRoot(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            // check for drive / protocol //
            Match matchDrive = Regex.Match(path, @"(^[A-Za-z][A-Za-z0-9]*:(?:[\\/]+)?)");
            if (matchDrive.Success)
                return matchDrive.Value;
            // check for UNC //
            Match matchUNC = Regex.Match(path, @"(^(?:\\\\)[^\\/]*(?:[\\/]+)?)");
            if (matchUNC.Success)
                return matchUNC.Value;
            // check if starts with separator //
            Match matchRoot = Regex.Match(path, @"^[\\/]+");
            if (matchRoot.Success)
                return matchRoot.Value;
            // empty root otherwise //
            return "";
        }

        /// <summary>
        /// Get directory name from file path. Returns path itself if it looks like directory.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Directory name</returns>
        public static string GetDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            if (Energy.Base.File.IsDirectory(path))
                return path;
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
        /// Get filename without extension.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetNameWithoutExtension(string path)
        {
            path = GetName(path);
            if (string.IsNullOrEmpty(path))
                return path;
            int dot = path.LastIndexOf('.');
            if (dot <= 0)
                return path;
            path = path.Substring(0, dot);
            return path;
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
        /// Exclude leading path root.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ExcludeRoot(string path)
        {
            string root = GetRoot(path);
            if (string.IsNullOrEmpty(root))
                return path;
            return path.Substring(root.Length);
        }

        /// <summary>
        /// Strip quotation from file path.
        /// Converts C:\"Program Files"\"Dir" into C:\Program Files\Dir.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string StripQuotation(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            if (!path.Contains("\""))
                return path;
            return path.Replace("\"", null);
        }

        /// <summary>
        /// Converts DOS backslashes into UNIX slashes in path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToDosPath(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            if (!path.Contains("/"))
                return path;
            return path.Replace("/", "\\");
        }

        /// <summary>
        /// Convert UNIX slashes into DOS backslashes in path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToUnixPath(string path)
        {
            if (path == null || path.Length == 0)
                return path;
            if (!path.Contains("\\"))
                return path;
            return path.Replace("\\", "/");
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

        #endregion

        /// <summary>
        /// Return unique name for file by checking it's not exists.
        /// This method may create empty file if reserve option is set true and it is
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
        /// Return true if file name does not contain expteions
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool HasNoExtension(string file)
        {
            string name = System.IO.Path.GetFileName(file);
            int index = name.IndexOf('.');
            return index <= 0 || index == name.Length - 1;
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
        /// Get absolute path
        /// </summary>
        /// <param name="file">string</param>
        /// <returns>string</returns>
        public static string GetAbsolutePath(string file)
        {
            if (String.IsNullOrEmpty(file))
                return file;
            return GetAbsolutePath(file, System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Get absolute path
        /// </summary>
        /// <param name="file">string</param>
        /// <param name="current">string</param>
        /// <returns>string</returns>
        public static string GetAbsolutePath(string file, string current)
        {
            if (String.IsNullOrEmpty(file) || String.IsNullOrEmpty(current))
                return file;
            if (!IsRelativePath(file))
                return file;
            string path = IncludeTrailingPathSeparator(current);
            return String.Concat(path, file);
        }

        /// <summary>
        /// Check if file or directory path is relative or absolute
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="separator">Separator list</param>
        /// <returns>bool</returns>
        public static bool IsRelativePath(string path, string[] separator)
        {
            if (separator == null)
                separator = new string[] { System.IO.Path.DirectorySeparatorChar.ToString() };
            if (String.IsNullOrEmpty(path))
                return true;

            string list = "";
            foreach (string _ in separator)
            {
                if (path.StartsWith(_))
                    return false;
                list += (_ == "\\" ? "\\" + _ : _);
            }
            string pattern = @"[A-Za-z]+[\d+]?:[$1]".Replace("$1", list);

            if (Regex.Match(path, pattern).Success)
                return false;

            return true;
        }

        /// <summary>
        /// Check if file or directory path is relative or absolute
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>bool</returns>
        public static bool IsRelativePath(string path)
        {
            return IsRelativePath(path, null);
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
        /// Locate command file
        /// </summary>
        /// <param name="command">string</param>
        /// <param name="search">string[]</param>
        /// <returns>string</returns>
        public static string Locate(string command, string[] search)
        {
            string[] executable = new string[] { "", ".exe", ".cmd", ".bat" };

            return Locate(command, search, executable, Energy.Enumeration.LocateBehaviour.Default);
        }

        /// <summary>
        /// Locate file with one of possible extensions in any directory and return full path to it
        /// </summary>
        /// <param name="file">File name with or without extension and leading path</param>
        /// <param name="search">Directory search list</param>
        /// <param name="extension">List of filename extensions to check (i.e. ".txt", "ini", ".")</param>
        /// <returns>Empty string if file not found or full path to found one</returns>
        public static string Locate(string file, string[] search, string[] extension)
        {
            return Locate(file, search, extension, Energy.Enumeration.LocateBehaviour.Default);
        }

        /// <summary>
        /// Locate file with one of possible extensions in any directory and return full path to it
        /// </summary>
        /// <param name="file">File name with or without extension and leading path</param>
        /// <param name="search">Directory search list</param>
        /// <param name="extension">List of filename extensions to check (i.e. ".txt", "ini", ".")</param>
        /// <param name="behaviour">Lookup behaviour (iterate over directories or extensions)</param>
        /// <returns>Empty string if file not found or full path to found one</returns>
        public static string Locate(string file, string[] search, string[] extension, Energy.Enumeration.LocateBehaviour behaviour)
        {
            if (string.IsNullOrEmpty(file))
                return "";

            file = Energy.Base.Path.ChangeSeparator(file);

            if (search == null || search.Length == 0)
            {
                search = new string[] { "" };
            }
            else
            {
                bool parse = false;
                char[] separators = new char[] { ';', ':' };
                for (int i = 0; i < search.Length; i++)
                {
                    if (search[i].IndexOfAny(separators) >= 0)
                        parse = true;
                }
                if (parse)
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < search.Length; i++)
                    {
                        list.AddRange(search[i].Split(separators));
                    }
                    search = list.ToArray();
                }
            }

            if (search.Length > 0)
            {
                for (int i = 0; i < search.Length; i++)
                {
                    search[i] = Energy.Base.Path.ChangeSeparator(search[i]);
                }
            }
            if (extension == null || extension.Length == 0)
                extension = new string[] { "" };

            string fileExtension = System.IO.Path.GetExtension(file);

            if (fileExtension.Length > 0)
            {
                string[] array = new string[extension.Length + 1];
                array[0] = fileExtension;
                Array.Copy(extension, 0, array, 1, extension.Length);
                extension = array;
            }

            if (file.EndsWith("."))
            {
                file = file.Substring(0, file.Length - 1);
                if (file.Length == 0)
                    return "";
            }

            switch (behaviour)
            {
                case Energy.Enumeration.LocateBehaviour.Directories:

                    for (int i = 0; i < search.Length; i++)
                    {
                        string directory = search[i];

                        if (string.IsNullOrEmpty(directory))
                            directory = System.IO.Directory.GetCurrentDirectory();

                        try
                        {
                            foreach (string ext in extension)
                            {
                                string candidate = System.IO.Path.Combine(directory, file);

                                if (!string.IsNullOrEmpty(ext) && 0 != string.Compare(".", ext, false))
                                    candidate = System.IO.Path.ChangeExtension(candidate, ext);

                                if (System.IO.File.Exists(candidate))
                                    return candidate;
                            }
                        }
                        catch (Exception x)
                        {
                            Energy.Core.Bug.Catch(x);
                        }
                    }

                    break;

                case Energy.Enumeration.LocateBehaviour.Extensions:

                    foreach (string ext in extension)
                    {
                        foreach (string directory in search)
                        {
                            try
                            {
                                string candidate = System.IO.Path.Combine(directory, file);

                                if (!string.IsNullOrEmpty(ext) && 0 == string.Compare(".", ext, false))
                                    candidate = System.IO.Path.ChangeExtension(candidate, ext);

                                if (System.IO.File.Exists(candidate))
                                    return candidate;
                            }
                            catch (Exception x)
                            {
                                Energy.Core.Bug.Catch(x);
                            }
                        }
                    }

                    break;

            }

            return "";
        }

        /// <summary>
        /// Create directory if not exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if directory exists or was created</returns>
        public static bool MakeDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    return true;
                }
                System.IO.DirectoryInfo dir = System.IO.Directory.CreateDirectory(path);
                return dir != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
