using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Enumeration
{
    /// <summary>
    /// SQL dialect
    /// </summary>
    public enum SqlDialect
    {
        /// <summary>Dialect not specified</summary>
        None,
        /// <summary>Generic SQL</summary>
        Generic,
        /// <summary>ANSI SQL-92 standard. Probably will not work with Microsoft SQL Server.</summary>
        ANSI,
        /// <summary>Transact-SQL (T-SQL) for Microsoft SQL Server (2008 and later)</summary>
        SqlServer,
        /// <summary>MySQL</summary>
        MySQL,
        /// <summary>PostgreSQL (PL/pgSQL)</summary>
        PostgreSQL,
        /// <summary>Oracle (PL/SQL)</summary>
        Oracle,
        /// <summary>Firebird</summary>
        Firebird,
        /// <summary>SQLite</summary>
        SQLite,
    }
}
