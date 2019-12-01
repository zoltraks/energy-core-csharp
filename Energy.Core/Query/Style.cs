using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query
{
    public class Style: Energy.Base.Pattern.GlobalObject<Style>
    {
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
