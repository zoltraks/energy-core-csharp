using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Energy.Base
{
    /// <summary>
    /// Simple lock object for thread safe function with additional debugging information.
    /// </summary>
    public class Lock
    {
        private DateTime _Create = DateTime.Now;
        /// <summary>Create stamp</summary>
        public DateTime CreateStamp { get { return _Create; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Lock()
        {
            Energy.Core.Bug.Write("Lock {0} created at {1}", this.GetHashCode(), Energy.Core.Bug.CallingMethod(1));
        }
    }
}
