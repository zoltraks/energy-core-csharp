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
        private DateTimeOffset _Create = DateTimeOffset.Now;
        /// <summary>Create stamp</summary>
        public DateTimeOffset CreateStamp { get { return _Create; } }

        public Lock()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Lock object {0} created at {1}"
                , Energy.Base.Hex.IntegerToHex(this.GetHashCode())
                , Energy.Core.Bug.CallingMethod(1)
                ));
        }
    }
}
