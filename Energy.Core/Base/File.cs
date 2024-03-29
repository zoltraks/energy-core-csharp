﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        #region State

        /// <summary>
        /// Represents file state and provides functions for monitoring changes.
        /// </summary>
        [Energy.Attribute.Code.Improve("Object might be able to mangle Directory and Path properties together")]
        public class State: ICloneable
        {
            #region Property

            private string _Name;
            /// <summary>Name of a file</summary>
            public string Name { get { return _Name; } set { _Name = value; } }

            private string _Path;
            /// <summary>Full path to a file</summary>
            public string Path { get { return _Path; } set { _Path = value; } }

            private string _Directory;
            /// <summary>Directory where file is located</summary>
            public string Directory { get { return _Directory; } set { _Directory = value; } }

            private DateTime _Stamp;
            /// <summary>File modification stamp</summary>
            public DateTime Stamp { get { return _Stamp; } set { _Stamp = value; } }

            private long _Size;
            /// <summary>File size</summary>
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
            /// Refresh file state
            /// </summary>
            /// <returns></returns>
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
            /// Check if file exists
            /// </summary>
            /// <returns></returns>
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
            /// Check if file was modified by checking write stamp and size.
            /// </summary>
            /// <returns></returns>
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
            /// Check if path was changed or file was modified by checking write stamp and size.
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
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
            /// Returns the creation date and time of the file.
            /// Returns DateTime.MinValue on error.
            /// </summary>
            /// <returns></returns>
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
            /// Returns the date and time the file was last written to.
            /// Returns DateTime.MinValue on error.
            /// </summary>
            /// <returns></returns>
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
            /// Set current time of last write for a file.
            /// </summary>
            /// <returns></returns>
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
            /// Set current time as time of last write for a file.
            /// </summary>
            /// <returns></returns>
            public bool SetWriteStamp()
            {
                return SetWriteStamp(DateTime.MinValue);
            }

            #endregion

            #region GetSize

            /// <summary>
            /// Returns the size in bytes of the file.
            /// Returns -1 on error.
            /// </summary>
            /// <returns></returns>
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
            /// Create a file if not exists.
            /// </summary>
            /// <returns>Return true if file was created</returns>
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
            /// Delete a file if exists.
            /// </summary>
            /// <returns>Return true if file was deleted</returns>
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
            /// If file exists, change last write time. Create new file otherwise.
            /// </summary>
            /// <returns>True if file write time was changed or file was succesfully created</returns>
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
        /// Include traling path directory separator if needed.
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string IncludeTrailingPathSeparator(string path)
        {
            if (path == null)
                return null;
            path = path.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
            path += System.IO.Path.DirectorySeparatorChar;
            return path;
        }

        /// <summary>
        /// Include traling path directory separator if needed.
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string IncludeTrailingDirectorySeparator(string path)
        {
            if (path == null)
                return null;
            path = path.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
            path += System.IO.Path.DirectorySeparatorChar;
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

        /// <summary>
        /// Get short command name from file path.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetCommandName(string file)
        {
            if (string.IsNullOrEmpty(file))
                return "";
            try
            {
                string ext = System.IO.Path.GetExtension(file);
                if (Energy.Base.Text.InArray(new string[] { ".exe", ".bat", ".cmd" }
                    , new string[] { ext, null }, true))
                {
                    string cmd = System.IO.Path.GetFileNameWithoutExtension(file);
                    return cmd;
                }
                else
                {
                    string cmd = System.IO.Path.GetFileName(file);
                    return cmd;
                }
            }
            catch (NotSupportedException)
            {
                return "";
            }
        }

        #endregion

        #region FileUniqueIdentity

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

        #endregion

        #region HasNoExtension

        /// <summary>
        /// Return true if file name does not contain extension
        /// </summary>
        /// <param name="file"></param>
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
        /// Check if path is directory
        /// </summary>
        /// <param name="file">string</param>
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
        /// <param name="command">string</param>
        /// <returns>string</returns>
        public static string Locate(string command)
        {
            return Locate(command, Energy.Base.Path.Environment());
        }

        /// <summary>
        /// Locate file or executable in search directories.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
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
        /// Locate file with one of possible extensions in search directories.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
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
        /// Locate file with one of possible extensions in search directories.
        /// <br/><br/>
        /// If file can't be found, empty string will be returned.
        /// </summary>
        /// <param name="file">File name with or without extension and leading path</param>
        /// <param name="search">Directory search list</param>
        /// <param name="extension">List of filename extensions to check (i.e. ".txt", "ini", ".")</param>
        /// <param name="behaviour">Lookup behaviour (iterate over directories or extensions)</param>
        /// <returns>Empty string if file not found or full path to found one</returns>
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
        /// If file can't be found, empty string will be returned.
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
            /// <param name="path"></param>
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
                path = Energy.Base.File.IncludeTrailingPathSeparator(path);
                _HomeDirectory = path;
            }
            return _HomeDirectory;
        }

        #endregion
    }
}
