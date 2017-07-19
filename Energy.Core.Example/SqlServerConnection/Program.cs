using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SqlServerConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Go(args);
            }
            catch (Exception x)
            {
                if (!string.IsNullOrEmpty(x.Message))
                {
                    Console.WriteLine("Exception: {0}", x.Message);
                }
            }
            Console.ReadLine();
        }

        private static void Go(string[] args)
        {
            var conf = new Energy.Source.Configuration();
            conf.Dialect = Energy.Enumeration.SqlDialect.SqlServer;
            conf.Catalog = "Platoon";
            conf.Server = ".\\SQLEXPRESS";
            Console.WriteLine(conf.ConnectionString);
            //new Energy.Base.ConnectionString() { Server}
            //string connectionString = 
            Energy.Source.Connection<SqlConnection> c = new Energy.Source.Connection<SqlConnection>(conf.ConnectionString);

        }
    }
}
