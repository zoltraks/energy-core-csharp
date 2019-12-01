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
        //private const int BUG_STACK_OFFSET = 3;

        //public static bool DEBUG = false;

        private DateTime _Create;
        /// <summary>Create stamp</summary>
        public DateTime Create { get { return _Create; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Lock()
        {
            _Create = DateTime.Now;
            //if (DEBUG)
            //{
            //    Energy.Core.Bug.Write("C002", () =>
            //    {
            //        string callingMethod = Energy.Core.Bug.CallingMethod(BUG_STACK_OFFSET);
            //        if (string.IsNullOrEmpty(callingMethod))
            //            callingMethod = Energy.Core.Bug.CallingMethod(BUG_STACK_OFFSET - 1);
            //        return string.Format("Lock created {0} {1}"
            //            , Energy.Base.Hex.IntegerToHex(this.GetHashCode())
            //            , callingMethod
            //            );
            //    });
            //}
        }
    }
}
