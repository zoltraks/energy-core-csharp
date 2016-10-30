using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Source.Interface
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
        /// Kill connection
        /// </summary>
        void Kill();
        
        /// <summary>
        /// Execute SQL statement
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns>Return -1 on error or number of rows affected</returns>
        int Execute(string query);

        /// <summary>
        /// Fetch query result
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns></returns>
        object Fetch(string query);
    }
}
