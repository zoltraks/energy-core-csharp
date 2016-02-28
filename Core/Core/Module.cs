using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Module
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="assembly">string</param>
        /// <returns>Energy.Module</returns>
        public static Module Load(string assembly)
        {
            return new Module();
        }
    }
}
