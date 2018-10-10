using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Energy.Source;

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
            Energy.Core.Tilde.WriteLine("Current time is ~y~{0}~0~", Energy.Base.Cast.DateTimeToStringDate(DateTime.Now));
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Test;Integrated Security=Yes;Connect Timeout=10;";
            connectionString = Energy.Core.Tilde.Ask("Connection string", connectionString);
            Energy.Source.Connection connection = new Energy.Source.Connection<SqlConnection>(connectionString);
            Energy.Core.Tilde.WriteLine("Connection vendor is ~g~{0}~0~", connection.Vendor.Name);
            string result = connection.Scalar<string>("SELECT GETDATE()");
            if (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Error getting time from server, aborting process...");
                string error = connection.GetErrorText();
                if (!string.IsNullOrEmpty(error))
                    Console.WriteLine(error);
                return;
            }
            Console.WriteLine("Time from server: {0}", result);
            DateTime stamp = connection.Scalar<DateTime>("SELECT GETDATE()");
            Console.WriteLine("Time from server: {0}", Energy.Base.Cast.DateTimeToString(stamp));

            Test.UseEvents(connection);
        }
    }
}
