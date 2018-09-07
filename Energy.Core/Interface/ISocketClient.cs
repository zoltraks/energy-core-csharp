using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Energy.Interface
{
    /// <summary>
    /// Common interface for database connection helper classes.
    /// </summary>
    public interface ISocketClient
    {
        /// <summary>
        /// Connect to remote.
        /// </summary>
        /// <returns></returns>
        bool Connect();
    }
}
