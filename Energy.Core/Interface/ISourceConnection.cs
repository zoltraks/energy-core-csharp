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

        ///// <summary>
        ///// Open new connection.
        ///// </summary>
        ///// <returns></returns>
        //IDbConnection Open();

        /// <summary>
        /// Execute SQL statement and return number of rows affected.
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

        /// <summary>
        /// Execute query and return the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        object Scalar(string query);

        /// <summary>
        /// Execute query and return the first column of the first row in the result set returned by the query
        /// converted to desired type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        T Scalar<T>(string query);

        //object Single(string query);
    }
}
