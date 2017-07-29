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
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
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
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=BisSQL;Integrated Security=Yes;Connect Timeout=10;";
            Energy.Source.Connection<SqlConnection> db = new Energy.Source.Connection<SqlConnection>(connectionString);
            Console.WriteLine(db.Scalar("SELECT GETDATE()"));
            DataTable t1 = db.Fetch("SELECT * FROM shadow_user");
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
        }
    }
}
