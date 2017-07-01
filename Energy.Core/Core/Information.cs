﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Energy.Core
{
    public class Information
    {
        public static string GetAssemblyDirectory(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return System.IO.Path.GetDirectoryName(path);
        }
    }
}