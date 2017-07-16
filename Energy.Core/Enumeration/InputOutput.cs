using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    [Flags]
    public enum InputOutput
    {
        /// <summary>None</summary>
        None = 0,
        /// <summary>Input</summary>
        Input = 1,
        /// <summary>Output</summary>
        Output = 2,
        /// <summary>Both</summary>
        Both = Input | Output,
    }
}
