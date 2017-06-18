using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresConnection
{
    class Program
    {
        static void Main(string[] args)
        {            
            Energy.Source.Connection<Npgsql.NpgsqlConnection> c = new Energy.Source.Connection<Npgsql.NpgsqlConnection>();

            //c.Configuration.Dialect ="ansi"
            c.ConnectionString = @"Server=127.0.0.1;Port=5432;Database=trace;User Id=test;Password=test;";
            Console.WriteLine(Energy.Base.Cast.BoolToString(c.Open(), Energy.Enumeration.BooleanStyle.YesNo));
            var x = c.Fetch(@"SELECT * FROM ""Trace"".""Registration""");
            Console.WriteLine(x.ToString());

        }
    }
}
