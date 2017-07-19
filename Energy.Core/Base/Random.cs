using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Random
    {
        private static readonly object _GetRandomGuidLock = new object();

        /// <summary>
        /// Get random GUID identifier
        /// </summary>
        /// <returns></returns>
        public static string GetRandomGuid()
        {
            lock (_GetRandomGuidLock)
            {
                return System.Guid.NewGuid().ToString("D").ToUpper();
            }
        }
    }
}
