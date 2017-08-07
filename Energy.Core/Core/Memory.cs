using System;
using System.Collections.Generic;
using System.Security.Permissions;
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
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static long GetCurrentMemoryUsage()
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            long memory = process.WorkingSet64;
            return memory;
        }
    }
}
