using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query
{
    public class Configuration
    {
        #region Singleton

        private static Configuration _Default;
        /// <summary>Singleton</summary>
        public static Configuration Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (typeof(Configuration))
                    {
                        if (_Default == null)
                        {
                            _Default = new Configuration();
                        }
                    }
                }
                return _Default;
            }
        }

        #endregion

        /// <summary>
        /// Identity column type
        /// </summary>
        public string IdentityType = "BIGINT";

        /// <summary>
        /// Identity column name
        /// </summary>
        public string IdentityName = "id";
    }
}
