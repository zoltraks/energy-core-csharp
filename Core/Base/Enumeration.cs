﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Enumeration
    {
        /// <summary>
        /// Character casing
        /// </summary>
        public enum CharacterCasing
        {
            /// <summary>None</summary>
            None,
            /// <summary>Lower</summary>
            Lower,
            /// <summary>Upper</summary>
            Upper,
        }

        /// <summary>
        /// Matching style
        /// </summary>
        public enum MatchStyle
        {
            /// <summary>
            /// At least one element has to be resolved
            /// </summary>
            Any,

            /// <summary>
            /// All elements must be resolved
            /// </summary>
            All,

            /// <summary>
            /// Only one element need to be resolved
            /// </summary>
            One,

            /// <summary>
            /// Every element must not be resolved
            /// </summary>
            Not,
        }

        /// <summary>
        /// SQL dialect
        /// </summary>
        public enum SQL
        {
            None,
            ANSI,
            MSSQL,
            MySQL,
            PostgreSQL,
            SQLite,
        }

        /// <summary>
        /// Style of representing boolean values
        /// </summary>
        public enum BooleanStyle
        {
            /// <summary>
            /// X for true
            /// </summary>
            X,
            /// <summary>
            /// V for true
            /// </summary>
            V,
            /// <summary>
            /// 0/1
            /// </summary>
            B,
            /// <summary>
            /// Y/N
            /// </summary>
            /// <remarks>Localised</remarks>
            Y,
            /// <summary>
            /// T/F
            /// </summary>
            /// <remarks>Localised</remarks>
            T,
            /// <summary>
            /// Yes/No
            /// </summary>
            /// <remarks>Localised</remarks>
            YesNo,
            /// <summary>
            /// True/False
            /// </summary>
            /// <remarks>Localised</remarks>
            TrueFalse,
        }
    }
}
