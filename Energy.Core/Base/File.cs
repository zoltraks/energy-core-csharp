using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// File system related helper functions
    /// </summary>
    public class File
    {
        #region State

        /// <summary>
        /// Represents file state and provides functions for monitoring changes
        /// </summary>
        [Energy.Attribute.Code.Improve("Object might be able to mangle Directory and Path properties together")]
        public class State : ICloneable
        {
            #region Property

            private string _Name;
            /// <summary>Name of the tracked file</summary>
            public string Name { get { return _Name; } set { _Name = value; } }

            private string _Path;
            /// <summary>Full path to the tracked file</summary>
            public string Path { get { return _Path; } set { _Path = value; } }

            private string _Directory;
            /// <summary>Directory that contains the tracked file</summary>
            public string Directory { get { return _Directory; } set { _Directory = value; } }

            private DateTime _Stamp;
            /// <summary>Cached file modification timestamp</summary>
            public DateTime Stamp { get { return _Stamp; } set { _Stamp = value; } }

            private long _Size;
            /// <summary>Cached file size in bytes</summary>
            public long Size { get { return _Size; } set { _Size = value; } }

            #endregion

            #region Constructor

            public State() { }

            public State(string file)
            {
                _Size = -1;
                _Path = file;
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }
                _Name = System.IO.Path.GetFileName(file);
                _Directory = System.IO.Path.GetDirectoryName(file);
            }

            public State(string file, bool read)
                : this(file)
            {
                if (read)
                {
                    Refresh();
                }
            }

            #endregion

            #region Private

            #endregion

            #region Refresh

            /// <summary>
            /// Refreshes cached file metadata such as timestamp and size.
            /// <br/><br/>
            /// Returns false when the path is empty, the file is missing, or an error occurs.
            /// </summary>
            /// <returns>True if metadata was refreshed</returns>
            public bool Refresh()
            {
                _Stamp = DateTime.MinValue;
                _Size = -1;
                string fileName = _Path;
                if (string.IsNullOrEmpty(fileName))
                    return false;
                try
                {
                    if (!System.IO.File.Exists(fileName))
                        return false;
                    DateTime lastWriteTime = System.IO.File.GetLastWriteTime(fileName);
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                    _Stamp = lastWriteTime;
                    _Size = fileInfo.Length;
                    return true;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E404", x);
                    return false;
                }
            }

            #endregion

            #region Exists

            /// <summary>
            /// Determines whether the tracked file currently exists on disk
            /// </summary>
            /// <returns>True when the file exists</returns>
            public bool Exists()
            {
                string fileName = _Path;
                if (string.IsNullOrEmpty(fileName))
                    return false;
                try
                {
                    bool exists = System.IO.File.Exists(fileName);
                    return exists;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E404", x);
                    return false;
                }
            }

            #endregion

            #region IsChanged

            /// <summary>
            /// Checks whether the cached timestamp or size differs from the current file metadata
            /// </summary>
            /// <returns>True when the tracked metadata changed since the last refresh</returns>
            public bool IsChanged()
            {
                string fileName = _Path;
                if (string.IsNullOrEmpty(fileName))
                    return false;
                try
                {
                    if (!System.IO.File.Exists(fileName))
                    {
                        if (_Stamp == DateTime.MinValue && _Size == -1)
                            return true;
                        else
                            return false;
                    }

                    DateTime lastWriteTime = System.IO.File.GetLastWriteTime(fileName);
                    if (_Stamp != lastWriteTime)
                        return true;
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                    if (_Size != fileInfo.Length)
                        return true;

                    return false;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E404", x);
                    return false;
                }
            }

            /// <summary>
            /// Checks whether the provided path differs from the cached path or if the file metadata changed
            /// </summary>
            /// <param name="file">File path to compare with the cached value</param>
            /// <returns>True if the path changed or the file metadata differs from the cached values</returns>
            public bool IsChanged(string file)
            {
                if (0 != string.Compare(file, _Path))
                {
                    return true;
                }
                else
                {
                    return IsChanged();
                }
            }

            #endregion

            #region GetCreateStamp

            /// <summary>
            /// Returns the creation timestamp for the tracked file.
            /// <br/><br/>
            /// Returns DateTime.MinValue when the path is empty, the file is missing, or an error occurs.
            /// </summary>
            /// <returns>Date and time of file creation</returns>
            public DateTime GetCreateStamp()
            {
                string fileName = _Path;
                if (string.IsNullOrEmpty(fileName))
                {
                    return DateTime.MinValue;
                }
                try
                {
                    if (!System.IO.File.Exists(fileName))
                    {
                        return DateTime.MinValue;
                    }
                    DateTime lastWriteTime = System.IO.File.GetCreationTime(fileName);
                    return lastWriteTime;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E403", x);
                    return DateTime.MinValue;
                }
            }

            #endregion

            #region GetWriteStamp

            /// <summary>
            /// Returns the last write timestamp for the tracked file.
            /// <br/><br/>
            /// Returns DateTime.MinValue when the path is empty, the file is missing, or an error occurs.
            /// </summary>
            /// <returns>Date and time of last modification</returns>
            public DateTime GetWriteStamp()
            {
                string fileName = _Path;
                if (string.IsNullOrEmpty(fileName))
                {
                    return DateTime.MinValue;
                }
                try
                {
                    if (!System.IO.File.Exists(fileName))
                    {
                        return DateTime.MinValue;
                    }
                    DateTime lastWriteTime = System.IO.File.GetLastWriteTime(fileName);
                    return lastWriteTime;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E403", x);
                    return DateTime.MinValue;
                }
            }

            #endregion

            #region SetWriteStamp

            /// <summary>
            /// Sets the tracked file timestamp to the provided value or to the current time when DateTime.MinValue is passed
            /// </summary>
            /// <returns>True if the timestamp was updated</returns>
            public bool SetWriteStamp(DateTime now)
            {
                string fileName = _Path;
                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }
                try
                {
                    if (!System.IO.File.Exists(_Name))
                    {
                        return false;
                    }
                    if (DateTime.MinValue == now)
                    {
                        now = DateTime.Now;
                    }
#if !NETCF
                    System.IO.File.SetLastWriteTime(fileName, now);
#endif
#if NETCF
                    using (var stream = System.IO.File.OpenWrite(fileName)) { };
#endif
                    _Stamp = System.IO.File.GetLastWriteTime(fileName);
                    return true;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E205", x);
                    return false;
                }
            }

            /// <summary>
            /// Sets the tracked file timestamp to the current time
            /// </summary>
            /// <returns>True if the timestamp was updated</returns>
            public bool SetWriteStamp()
            {
                return SetWriteStamp(DateTime.MinValue);
            }

            #endregion

            #region GetSize

            /// <summary>
            /// Returns the size in bytes of the tracked file or -1 when the path is empty, missing, or inaccessible
            /// </summary>
            /// <returns>File size in bytes or -1 on error</returns>
            public long GetSize()
            {
                if (string.IsNullOrEmpty(_Name))
                    return -1;
                try
                {
                    if (!System.IO.File.Exists(_Name))
                        return -1;
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(_Name);
                    return fileInfo.Length;
                }
                catch (Exception x)
                {
                    Energy.Core.Bug.Write("E403", x);
                    return -1;
                }
            }

            #endregion

            #region Clone

            /// <summary>
            /// Creates a shallow copy of the current state including cached metadata
            /// </summary>
            /// <returns>Cloned state instance</returns>
            public object Clone()
            {
                State o = new State(_Path)
                {
                    Size = _Size,
                    Stamp = _Stamp,
                };
                return o;
            }

            #endregion

            #region CreateFile

            /// <summary>
            /// Creates the tracked file when it does not already exist
            /// </summary>
            /// <returns>True if the file was created</returns>
            public bool CreateFile()
            {
                try
                {
                    string file = _Path;
                    using (System.IO.FileStream _ = System.IO.File.Open(file, System.IO.FileMode.CreateNew))
                    {
                        _.Close();
                    }
                    _Stamp = System.IO.File.GetLastWriteTime(file);
                    _Size = 0;
                    return true;
                }
                catch (Exception x)
                {
                    Core.Bug.Write(x);
                    return false;
                }
            }

            #endregion

            #region DeleteFile

            /// <summary>
            /// Deletes the tracked file when present and resets cached metadata
            /// </summary>
            /// <returns>True if the file was deleted or not found</returns>
            public bool DeleteFile()
            {
                try
                {
                    string file = _Path;
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                    _Stamp = DateTime.MinValue;
                    _Size = -1;
                    return true;
                }
                catch (Exception x)
                {
                    Core.Bug.Write(x);
                    return false;
                }
            }

            #endregion

            #region Touch

            /// <summary>
            /// Updates the last write time when the file exists or creates the file otherwise
            /// </summary>
            /// <returns>True if the timestamp was updated or a new file was created</returns>
            public bool Touch()
            {
                if (Exists())
                {
                    return SetWriteStamp();
                }
                else
                {
                    return CreateFile();
                }
            }

            #endregion
        }

        #endregion

        #region Naming

        #region GetName

        /// <summary>
        /// Gets the file name without the leading directory path
        /// </summary>
        /// <param name="path">Path that may include directory segments</param>
        /// <returns>File name portion without directory segments</returns>
        public static string GetName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            int position = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (position >= 0)
                path = path.Substring(position + 1);
            return path;
        }

        #endregion

        #region GetExtension

        /// <summary>
        /// Gets the filename extension including the dot while treating leading-dot names like ".gitignore" as having no extension
        /// </summary>
        /// <param name="path">File name or path to inspect for an extension</param>
        /// <returns>Extension including the dot or empty string when absent</returns>
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

        #endregion

        #region GetRoot

        /// <summary>
        /// Gets the file root portion of a path including drive letters, UNC prefixes, or protocol roots
        /// </summary>
        /// <param name="path">Path from which to extract the root portion</param>
        /// <returns>Root segment (drive, UNC prefix, or leading separator) or empty string</returns>
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

        #endregion

        #region IncludeRoot

        /// <summary>
        /// Prepends the specified root directory when the path currently lacks one
        /// </summary>
        /// <param name="path">Path missing a root component</param>
        /// <param name="root">Root directory to prepend when needed</param>
        /// <returns>Path guaranteed to include the provided root</returns>
        public static string IncludeRoot(string path, string root)
        {
            string check = path.Trim().Replace("\\", "/");
            if (check.StartsWith("/") || (new Regex("^[a-z]:/", RegexOptions.IgnoreCase)).Match(check).Success)
            {
                return path;
            }
            return IncludeSeparator(root) + path;
        }

        #endregion

        #region ExcludeRoot

        /// <summary>
        /// Removes the root portion from the provided path
        /// </summary>
        /// <param name="path">Path containing a root segment</param>
        /// <returns>Path without the leading root portion</returns>
        public static string ExcludeRoot(string path)
        {
            string root = GetRoot(path);
            if (string.IsNullOrEmpty(root))
                return path;
            return path.Substring(root.Length);
        }

        #endregion

        #region IncludeSeparator

        /// <summary>
        /// Appends a trailing directory separator when the path is missing one
        /// </summary>
        /// <param name="path">Path that should end with a directory separator</param>
        /// <returns>Path with exactly one trailing separator (or null when input is null)</returns>
        public static string IncludeSeparator(string path)
        {
            if (path == null)
                return null;
            path = path.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
            path += System.IO.Path.DirectorySeparatorChar;
            return path;
        }

        #endregion

        #region ExcludeSeparator

        /// <summary>
        /// Removes a trailing directory separator from the path
        /// </summary>
        /// <param name="path">Path that may end with a directory separator</param>
        /// <returns>Path without a trailing separator</returns>
        public static string ExcludeSeparator(string path)
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

        #endregion

        #region FileUniqueIdentity

        /// <summary>
        /// Generates a unique file path within the provided directory.
        /// <br/><br/>
        /// Creates an empty placeholder file when reserve is true.
        /// </summary>
        /// <param name="file">Base file name used to construct a unique candidate</param>
        /// <param name="path">Directory in which the uniqueness check should occur</param>
        /// <param name="reserve">When true, create the file immediately to reserve the name</param>
        /// <returns>Full path to a unique file name or null when reservation fails</returns>
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
        /// Generates a unique file path within the provided directory
        /// </summary>
        /// <param name="file">Base file name used to construct a unique candidate</param>
        /// <param name="path">Directory in which the uniqueness check should occur</param>
        /// <returns>Full path to a unique file name or null when uniqueness cannot be determined</returns>
        public static string FileUniqueIdentity(string file, string path)
        {
            return FileUniqueIdentity(file, path, false);
        }

        /// <summary>
        /// Generates a unique file path in the same directory as the supplied file while optionally reserving it
        /// </summary>
        /// <param name="file">Base file name whose directory should be used</param>
        /// <param name="reserve">When true, create the file immediately to reserve the name</param>
        /// <returns>Full path to a unique file name or null when reservation fails</returns>
        public static string FileUniqueIdentity(string file, bool reserve)
        {
            return FileUniqueIdentity(file, System.IO.Path.GetDirectoryName(file), reserve);
        }

        /// <summary>
        /// Generates a unique file path in the same directory as the supplied file
        /// </summary>
        /// <param name="file">Base file name whose directory should be used</param>
        /// <returns>Full path to a unique file name or null when uniqueness cannot be determined</returns>
        public static string FileUniqueIdentity(string file)
        {
            return FileUniqueIdentity(file, System.IO.Path.GetDirectoryName(file), false);
        }

        #endregion

        #region HasNoExtension

        /// <summary>
        /// Returns true when the file name does not contain an extension
        /// </summary>
        /// <param name="file">File path or name whose extension will be inspected</param>
        /// <returns></returns>
        public static bool HasNoExtension(string file)
        {
            string name = System.IO.Path.GetFileName(file);
            int index = name.IndexOf('.');
            return index <= 0 || index == name.Length - 1;
        }

        #endregion

        #region IsDirectory

        /// <summary>
        /// Determines whether the provided path points to a directory
        /// </summary>
        /// <param name="file">Path to inspect for directory attributes</param>
        /// <returns>bool</returns>
        public static bool IsDirectory(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }
            try
            {
#if !NETCF
                System.IO.FileAttributes attributes = System.IO.File.GetAttributes(file);
                return (attributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;
#endif
#if NETCF
                return file.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString());
#endif
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region GetAbsolutePath

        /// <summary>
        /// Get absolute path
        /// </summary>
        /// <param name="file">Relative or absolute path to normalize</param>
        /// <returns>Absolute path to the specified file or the original value when empty</returns>
        public static string GetAbsolutePath(string file)
        {
            if (String.IsNullOrEmpty(file))
                return file;
            return GetAbsolutePath(file, System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Get absolute path
        /// </summary>
        /// <param name="file">Relative path to normalize</param>
        /// <param name="current">Current directory used to resolve the relative path</param>
        /// <returns>Absolute path composed from the current directory and relative file</returns>
        public static string GetAbsolutePath(string file, string current)
        {
            if (String.IsNullOrEmpty(file) || String.IsNullOrEmpty(current))
                return file;
            if (!IsRelativePath(file))
                return file;
            string path = IncludeSeparator(current);
            return String.Concat(path, file);
        }

        #endregion

        #region IsRelativePath

        /// <summary>
        /// Check if file or directory path is relative or absolute.
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
        /// Check if file or directory path is relative or absolute.
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>bool</returns>
        public static bool IsRelativePath(string path)
        {
            return IsRelativePath(path, null);
        }

        #endregion

        #region Locate

        /// <summary>
        /// Locate file or executable in directories from PATH environment variable.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
        /// </summary>
        /// <param name="command">File or executable name to locate using PATH</param>
        /// <returns>Absolute path to the first matching file or empty string if not found</returns>
        public static string Locate(string command)
        {
            return Locate(command, Energy.Base.Path.Environment());
        }

        /// <summary>
        /// Locate file or executable in search directories.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
        /// </summary>
        /// <param name="command">Path or command name to locate</param>
        /// <param name="search">Directories to scan for the command</param>
        /// <returns>Absolute path to the first matching file or empty string if not found</returns>
        public static string Locate(string command, string[] search)
        {
            string[] executable = new string[] { "", ".exe", ".cmd", ".bat" };

            return Locate(command, search, executable, Energy.Enumeration.LocateBehaviour.Default);
        }

        /// <summary>
        /// Locate file with one of possible extensions in search directories.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
        /// </summary>
        /// <param name="file">File name with or without extension and leading path</param>
        /// <param name="search">Directory search list</param>
        /// <param name="extension">List of filename extensions to check (i.e. ".txt", "ini", ".")</param>
        /// <returns>Absolute path to the first matching file or empty string if not found</returns>
        public static string Locate(string file, string[] search, string[] extension)
        {
            return Locate(file, search, extension, Energy.Enumeration.LocateBehaviour.Default);
        }

        /// <summary>
        /// Locate file with one of possible extensions in search directories.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
        /// </summary>
        /// <param name="file">File name with or without extension and leading path</param>
        /// <param name="search">Directory search list</param>
        /// <param name="extension">List of filename extensions to check (i.e. ".txt", "ini", ".")</param>
        /// <param name="behaviour">Lookup behaviour (iterate over directories or extensions)</param>
        /// <returns>Absolute path to the first matching file or empty string if not found</returns>
        public static string Locate(string file, string[] search, string[] extension, Energy.Enumeration.LocateBehaviour behaviour)
        {
            if (string.IsNullOrEmpty(file))
            {
                return "";
            }

            file = Energy.Base.Path.ChangeSeparator(file);

            if (search == null || search.Length == 0)
            {
                search = new string[] { "" };
            }

            if (search.Length > 0)
            {
                for (int i = 0; i < search.Length; i++)
                {
                    search[i] = Energy.Base.Path.ChangeSeparator(search[i]);
                }
            }

            if (extension == null || extension.Length == 0)
            {
                extension = new string[] { "" };
            }

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
                {
                    return "";
                }
            }

            switch (behaviour)
            {
                case Energy.Enumeration.LocateBehaviour.Default:
                case Energy.Enumeration.LocateBehaviour.Directories:

                    for (int i = 0; i < search.Length; i++)
                    {
                        string directory = search[i];

                        if (string.IsNullOrEmpty(directory))
                        {
                            directory = System.IO.Directory.GetCurrentDirectory();
                        }

                        try
                        {
                            foreach (string ext in extension)
                            {
                                string candidate = System.IO.Path.Combine(directory, file);

                                if (!string.IsNullOrEmpty(ext) && 0 != string.Compare(".", ext, false))
                                {
                                    candidate = System.IO.Path.ChangeExtension(candidate, ext);
                                }

                                if (System.IO.File.Exists(candidate))
                                {
                                    return candidate;
                                }
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
                                {
                                    candidate = System.IO.Path.ChangeExtension(candidate, ext);
                                }

                                if (System.IO.File.Exists(candidate))
                                {
                                    return candidate;
                                }
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
        /// Locate file with one of possible extensions in search directory.
        /// <br/><br/>
        /// Returns empty string when nothing is found.
        /// </summary>
        /// <param name="list">Array of file names with or without extension and leading path</param>
        /// <param name="search">Directory search list</param>
        /// <param name="extension">List of filename extensions to check (i.e. ".txt", "ini", ".")</param>
        /// <param name="behaviour">Lookup behaviour (iterate over directories or extensions)</param>
        /// <returns>Empty string if file not found or full path to found one</returns>
        public static string Locate(string[] list, string[] search, string[] extension, Energy.Enumeration.LocateBehaviour behaviour)
        {
            if (null == list)
            {
                return "";
            }
            string result = "";
            foreach (string file in list)
            {
                result = Locate(file, search, extension, behaviour);
                if (string.IsNullOrEmpty(result))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        #region MakeDirectory

        /// <summary>
        /// Create directory if not exists.
        /// </summary>
        /// <param name="path">Directory path to create</param>
        /// <returns>Returns true if a directory exists or has been created</returns>
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
            catch (Exception x)
            {
                Energy.Core.Bug.Catch(x);
                return false;
            }
        }

        #endregion

        #region RemoveDirectory

        /// <summary>
        /// Remove directory if exists.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        /// <returns>Returns true if directory has been removed or not exists</returns>
        public static bool RemoveDirectory(string path, bool recursive)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            try
            {
                if (!System.IO.Directory.Exists(path))
                {
                    return true;
                }
                System.IO.Directory.Delete(path, recursive);
                return true;
            }
            catch (Exception x)
            {
                Energy.Core.Bug.Catch(x);
                return false;
            }
        }

        /// <summary>
        /// Remove directory if exists and is empty.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns true if directory has been removed or not exists</returns>
        public static bool RemoveDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            try
            {
                if (!System.IO.Directory.Exists(path))
                {
                    return true;
                }
                System.IO.Directory.Delete(path);
                return true;
            }
            catch (Exception x)
            {
                Energy.Core.Bug.Catch(x);
                return false;
            }
        }

        #endregion

        #region GetBaseDirectory

        /// <summary>
        /// Gets the base directory that the assembly resolver uses to probe for assemblies.
        /// </summary>
        /// <returns></returns>
        public static string GetBaseDirectory()
        {
#if !NETCF
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return baseDirectory;
#endif
#if NETCF
            return null;
#endif
        }

        #endregion

        #region GetBasePath

        /// <summary>
        /// Gets the base path that the assembly resolver uses to probe for assemblies.
        /// Return path with trailing directory separator.
        /// </summary>
        /// <returns></returns>
        public static string GetBasePath()
        {
            string path = GetBaseDirectory();
            int index = path.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
            if (0 > index || index < path.Length - 1)
            {
                path += System.IO.Path.DirectorySeparatorChar;
            }
            return path;
        }

        #endregion

        #region DeleteFile

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// 0 if filename was empty or file didn't exist,
        /// <br/>
        /// 1 if file was successfully deleted,
        /// <br/>
        /// -2 on I/O error,
        /// <br/>
        /// -3 on access error,
        /// <br/>
        /// -1 on other errors.
        /// </returns>
        public static int DeleteFile(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return 0;
            }
            try
            {
                if (!System.IO.File.Exists(file))
                {
                    return 0;
                }
                System.IO.File.Delete(file);
                return 1;
            }
            catch (UnauthorizedAccessException)
            {
                return -3;
            }
            catch (System.IO.IOException)
            {
                return -2;
            }

            catch
            {
                Debug.Write(null);
                return -1;
            }
        }

        #endregion

        #region Storage

        private class Storage : Energy.Interface.IFileSystem, IDisposable
        {
            private List<string> _Files = new List<string>();

            private string _Root;

            public string Root { get { return _Root; } set { _Root = value; } }

            private bool _Persistent;

            public bool Persistent { get { return _Persistent; } set { _Persistent = value; } }

            public void Dispose()
            {
                if (!_Persistent)
                {
                    for (int i = _Files.Count - 1; i <= 0; i--)
                    {
                        string file = _Files[i];
                        if (0 > Energy.Base.File.DeleteFile(file))
                        {
                            Debug.Write(null);
                        }
                        _Files.RemoveAt(i);
                    }
                }
            }
        }

        #endregion

        #region AppendText

        public static bool AppendText(string file, string content)
        {
            try
            {
#if !NETCF
                System.IO.File.AppendAllText(file, content);
#endif
                return true;
            }
            catch
            {
                Debug.Write(null);
                return false;
            }
        }

        #endregion

        #region ReadText

        public static string ReadText(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            string result = null;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch
            {
                Debug.Write(null);
            }
            return result;
        }

        public static string ReadText(string fileName, Encoding encoding)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            string result = null;
            try
            {
                using (StreamReader sr = new StreamReader(fileName, encoding))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch
            {
                Debug.Write(null);
            }
            return result;
        }

        #endregion

        #region GetHomeDirectory

        private static string _HomeDirectory;

        /// <summary>
        /// Get absolute path of home directory for current user.
        /// <br /><br />
        /// Resulting path will include trailing directory separator.
        /// </summary>
        /// <returns></returns>
        public static string GetHomeDirectory()
        {
            if (null == _HomeDirectory)
            {
                string path;
#if !NETCF
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                    path = Environment.GetEnvironmentVariable("HOME");
                else
                    path = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
#endif
#if NETCF
                path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
                path = Energy.Base.File.IncludeSeparator(path);
                _HomeDirectory = path;
            }
            return _HomeDirectory;
        }

        #endregion
    }
}
