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
        /// <summary>ANSI SQL-92 standard. Probably will not work with Microsoft SQL Server.</summary>
        ANSI,
        /// <summary>Transact-SQL (T-SQL) for Microsoft SQL Server (2008 and later)</summary>
        SQLSERVER,
        /// <summary>MySQL</summary>
        MYSQL,
        /// <summary>PostgreSQL (PL/pgSQL)</summary>
        POSTGRESQL,
        /// <summary>Oracle (PL/SQL)</summary>
        ORACLE,
        /// <summary>Firebird</summary>
        FIREBIRD,
        /// <summary>SQLite</summary>
        SQLITE,
    }
}
