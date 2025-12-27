using System;

using System.Collections.Generic;
using System.Windows.Forms;

namespace Energy.Core.Compact.Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            System.Windows.Forms.Application.Run(new View.Start());
        }
    }
}