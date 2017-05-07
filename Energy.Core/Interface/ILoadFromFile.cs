using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    /// <summary>
    /// Supports loading from file
    /// </summary>
    public interface ILoadFromFile
    {
        /// <summary>
        /// Load from file
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <returns>True on read success</returns>
        bool Load(string file);
    }
}
