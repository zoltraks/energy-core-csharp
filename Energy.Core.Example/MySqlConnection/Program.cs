using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySqlConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection> connection = new Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection>(Energy.Enumeration.SqlDialect.MySQL);
            connection.ConnectionString = @"Server=127.0.0.1;Database=test;Uid=test;Pwd=test;";
            connection.Open();
            Console.WriteLine(connection.Scalar("SELECT CURRENT_TIMESTAMP()"));
            Console.ReadLine();
        }
    }
}
