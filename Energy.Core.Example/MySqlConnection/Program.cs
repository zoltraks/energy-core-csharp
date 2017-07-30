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
            Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection> db = new Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection>(Energy.Enumeration.SqlDialect.MySQL);
            db.ConnectionString = @"Server=127.0.0.1;Database=test;Uid=test;Pwd=test;";
            db.Open();
            Console.WriteLine(db.Scalar("SELECT CURRENT_TIMESTAMP()"));

            db.ConnectionString = @"Server=127.0.0.1;Database=platoon;Uid=platoon;Pwd=platoon;";
            db.Open();
            Console.WriteLine(db.Scalar("SELECT CURRENT_TIMESTAMP()"));

            Energy.Source.Structure.Table table = Energy.Source.Structure.Table.Create(typeof(UserTableRecord));
            Energy.Query.Script script = new Energy.Query.Script.MySQL();
            
            string query;

            query = script.CreateDescription(table);
            Console.WriteLine(query);

            query = script.DropTable(table.Name);
            if (db.Execute(query) < 0)
            {
                Console.WriteLine(db.ErrorStatus);
            }
            Console.WriteLine(query);
            query = script.CreateTable(table);
            if (db.Execute(query) < 0)
            {
                Console.WriteLine(db.ErrorStatus);
            }
            Console.WriteLine(query);

            Console.ReadLine();
        }
    }
}
