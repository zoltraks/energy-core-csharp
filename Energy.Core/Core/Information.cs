using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Energy.Core
{
    public class Information
    {
        public static string GetAssemblyDirectory(System.Reflection.Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return System.IO.Path.GetDirectoryName(path);
        }

        [System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentNamespace()
        {
            return System.Reflection.Assembly.GetCallingAssembly().EntryPoint.DeclaringType.Namespace;
        }
    }
}
