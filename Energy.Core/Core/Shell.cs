using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    public class Shell
    {
        #region Class

        public class CommandLineStyle
        {
            /// <summary>
            /// DOS options like /? or /help.
            /// </summary>
            public bool Slash = true;

            /// <summary>
            /// POSIX / GNU options like -h for short option.
            /// </summary>
            public bool Single = true;

            /// <summary>
            /// POSIX / GNU options like --help for long option.
            /// </summary>
            public bool Double = true;

            /// <summary>
            /// Determines if colon may  be used after option name to specify value, like /opt:value.
            /// </summary>
            public bool Colon = false;

            /// <summary>
            /// Determines if equal sign may be used after option name to specify value, like /opt=value.
            /// </summary>
            public bool Equal = false;

            /// <summary>
            /// Check if parameter is an option with command line settings.
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns>True if is option, like "/opt"</returns>
            public bool IsOption(string parameter)
            {
                if (Slash && parameter.StartsWith("/"))
                    return true;
                if ((Single || Double) && parameter.StartsWith("-"))
                    return true;
                return false;
            }

            /// <summary>
            /// Check if parameter is an argument (not an option) with command line settings.
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns>True if is not an option, like "/opt"</returns>
            public bool IsArgument(string parameter)
            {
                return !IsOption(parameter);
            }
        }

        #endregion
    }
}
