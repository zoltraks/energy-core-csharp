using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface IConnection
    {
        /// <summary>
        /// Open connection
        /// </summary>
        /// <returns></returns>
        bool Open();

        /// <summary>
        /// Close connection
        /// </summary>
        void Close();

        /// <summary>
        /// Execute SQL statement
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns>Return negative error number (&lt;0) or number of rows affected (&gt;=0)</returns>
        int Execute(string query);

        /// <summary>
        /// Fetch query result
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns></returns>
        System.Data.DataTable Fetch(string query);

        /// <summary>
        /// Fetch query result
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns></returns>
        System.Data.DataTable FetchDataTable(string query);

        Energy.Base.Variant.Value Scalar(string query);

        object Single(string query);
    }
}
