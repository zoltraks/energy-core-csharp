#if CFNET
    //
#elif WindowsCE || PocketPC || WINDOWS_PHONE
    //
#define CFNET
#elif COMPACT_FRAMEWORK
//
#define CFNET
#else
    //
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Memory diagnostics
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// Get current memory usage
        /// </summary>
        /// <returns></returns>
#if !NETSTANDARD && !NETCF
        [System.Security.Permissions.EnvironmentPermissionAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Unrestricted = true)]
#endif
        public static long GetCurrentMemoryUsage()
        {
#if !NETCF
            try
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
                long memory = process.WorkingSet64;
                return memory;
            }
            catch (System.Security.SecurityException)
            {
                return 0;
            }
#endif
#if NETCF
            return 0;
#endif
        }

        /// <summary>
        /// Clear MemoryStream object.
        /// </summary>
        /// <param name="stream"></param>
        public static void Clear(MemoryStream stream)
        {
            byte[] buffer = stream.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            stream.Position = 0;
            stream.SetLength(0);
        }
    }
}
