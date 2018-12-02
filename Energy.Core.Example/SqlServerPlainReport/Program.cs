using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SqlServerPlainReport
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Application.SetDefaultLanguage();
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

        private static Energy.Source.Connection<SqlConnection> db = null;

        private static void Go(string[] args)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=platoon;Integrated Security=Yes;Connect Timeout=10;";
			connectionString = @"Data Source=10.0.2.2;Initial Catalog=platoon;Integrated Security=No;User Id=platoon;Password=platoon;Connect Timeout=10;";
            //connectionString = @"Data Source=W101-SQL01;Initial Catalog=BisSQL;Integrated Security=False;User ID=bissqluser;Password=B1sSqLP@ssW0rd;MultipleActiveResultSets=True;Connect Timeout=10";
            //connectionString = @"Data Source=W103-FS02;Initial Catalog=BisSQLTest;Integrated Security=False;User ID=SZCbistestuser;Password=f4c!S7CzeCInSQLT3st!;MultipleActiveResultSets=True;Connect Timeout=30";
            db = new Energy.Source.Connection<SqlConnection>(connectionString);

            Console.WriteLine(db.Scalar("SELECT GETDATE()"));

            Test1();
            Test2();
        }

        private static void Test1()
        {
            Energy.Source.Structure.Table table = Energy.Source.Structure.Table.Create(typeof(UserTableRecord));
            string query;
            Energy.Interface.IDialect script = new Energy.Query.Dialect.MYSQL();

			query = script.DropTable(table.Name);
            if (db.Execute(query) < 0)
            {
                Console.WriteLine(db.ErrorMessage);
            }
            Console.WriteLine(query);
			Energy.Query.Script scriptBuilder = new Energy.Query.Script ();
			scriptBuilder.Dialect = Energy.Enumeration.SqlDialect.SQLSERVER;

			Energy.Query.Format format = Energy.Enumeration.SqlDialect.MYSQL;

			query = script.CreateTable(table);
            if (db.Execute(query) < 0)
            {
                Console.WriteLine(db.ErrorMessage);
            }
            Console.WriteLine(query);
            Console.ReadLine();
        }

        private static void Test2()
        {
            DataTable t1 = db.Read("SELECT * FROM UserTable");
            string text = Energy.Base.Plain.DataTableToPlainText(t1, null);

            for (int j = 0; j < t1.Columns.Count; j++)
            {
                Console.Write(t1.Columns[j].ColumnName);
                Console.Write(" | ");
            }
            Console.WriteLine();
            for (int i = 0; i < t1.Rows.Count; i++)
            {
                for (int j = 0; j < t1.Columns.Count; j++)
                {
                    Console.Write(Energy.Base.Cast.ObjectToString(t1.Rows[i][j]));
                    Console.Write(" | ");
                }
                Console.WriteLine();
            }

            string query = "SELECT TOP 1000 * FROM LogTable ORDER BY id DESC";
            Console.WriteLine();
            Console.WriteLine("--- Benchmark 1 ---");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Memory: {0}", Energy.Core.Memory.GetCurrentMemoryUsage());

            Energy.Core.Benchmark.Result benchmark;
            Console.WriteLine("Using FetchDataTable()");
            benchmark = Energy.Core.Benchmark.Profile(() =>
            {
                DataTable t10 = db.Read(query);
                if (t10 == null)
                    Console.WriteLine(db.GetErrorText());
                else
                    Console.WriteLine(string.Format("Result count: {0}", t10.Rows.Count));
            }, 3, 1, "FetchDataTableLoad");
            Console.WriteLine(benchmark.ToString());

            Console.WriteLine("Memory: {0}", Energy.Core.Memory.GetCurrentMemoryUsage());

            Console.WriteLine("Using FetchDataTable2()");
            benchmark = Energy.Core.Benchmark.Profile(() =>
            {
                DataTable t10 = db.Read(query);
                if (t10 == null)
                    Console.WriteLine(db.GetErrorText());
                else
                    Console.WriteLine(string.Format("Result count: {0}", t10.Rows.Count));
            }, 3, 1, "FetchDataTableRead");
            Console.WriteLine(benchmark.ToString());

            Console.WriteLine("Memory: {0}", Energy.Core.Memory.GetCurrentMemoryUsage());
        }
    }
}
