using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Energy.Interface
{
    /// <summary>
    /// Common interface for database connection helper classes.
    /// </summary>
    public interface ISourceConnection
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// DbConnection vendor class for database connection.
        /// </summary>
        Type Vendor { get; set; }

        /// <summary>
        /// Open new connection.
        /// </summary>
        /// <returns></returns>
        IDbConnection Open();

        /// <summary>
        /// Execute SQL statement
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns>Return negative error number (&lt;0) or number of rows affected (&gt;=0)</returns>
        int Execute(string query);

        ///// <summary>
        ///// Fetch query result
        ///// </summary>
        ///// <param name="query">SQL query</param>
        ///// <returns></returns>
        //System.Data.DataTable Fetch(string query);

        ///// <summary>
        ///// Fetch query result
        ///// </summary>
        ///// <param name="query">SQL query</param>
        ///// <returns></returns>
        //System.Data.DataTable FetchDataTable(string query);

        object Scalar(string query);

        //object Single(string query);
    }
}
