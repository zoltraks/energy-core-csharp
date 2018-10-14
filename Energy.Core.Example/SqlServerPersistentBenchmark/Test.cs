using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlServerPersistentBenchmark
{
    public class Test
    {
        internal static void GetDate()
        {
            string connStr = "Data Source=.\\SQLEXPRESS;Initial Catalog=master;Integrated Security=Yes;Connect Timeout=10;";
            connStr = Energy.Core.Tilde.Ask("Connection string", connStr);
            Energy.Source.Connection<SqlConnection> source = new Energy.Source.Connection<SqlConnection>(connectionString: connStr);
            source.Persistent = false;
            int num1 = Energy.Core.Benchmark.Loop(() => { string result = source.Scalar<string>("SELECT GETDATE()"); }, 1);
            source.Persistent = true;
            int num2 = Energy.Core.Benchmark.Loop(() => { string result = source.Scalar<string>("SELECT GETDATE()"); }, 1);
            Console.WriteLine($"Dynamic {num1} Persistent {num2}");
        }
    }
}
