using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// Form mode (New/Edit/View)
    /// </summary>
    public enum FormMode
    {
        /// <summary>New element</summary>
        New,
        /// <summary>Existing element</summary>
        Edit,
        /// <summary>View element (read only)</summary>
        View
    }
}
