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
        public static long GetCurrentMemoryUsage()
        {
            return System.Environment.WorkingSet;
        }
    }
}
