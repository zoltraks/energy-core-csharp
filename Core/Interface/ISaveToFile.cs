using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    /// <summary>
    /// Supports writing to file
    /// </summary>
    public interface ISaveToFile
    {
        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <returns>True on write success</returns>
        bool Save(string file = null);
    }
}
