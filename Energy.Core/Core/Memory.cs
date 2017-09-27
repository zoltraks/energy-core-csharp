using System;
using System.Collections.Generic;
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
#if (!NETSTANDARD)
        [System.Security.Permissions.EnvironmentPermissionAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Unrestricted = true)]
#endif
        public static long GetCurrentMemoryUsage()
        {
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
        }
    }
}
