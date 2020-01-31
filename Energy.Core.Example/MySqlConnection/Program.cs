using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySqlConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("~y~MySQL ~w~connection example");

            Energy.Core.Log.Default.Destination.Add(new Energy.Core.Log.Target.Console());

            Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection> db;
            db = new Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection>();
            db.ConnectionString = @"Server=127.0.0.1;Database=test;Uid=test;Pwd=test;";

            db.OnCreate += Db_OnCreate;
            db.OnClose += Db_OnClose;

            string query = "SELECT * FROM INFORMATION_SCHEMA.PROCESSLIST";
            //Console.WriteLine(db.Read))
            Console.WriteLine(db.Fetch(query).ToPlain());

            Energy.Core.Tilde.WriteLine("~c~Using 3 threads to retreive current timestamp 3 times with delay of 1 second each");
            Energy.Core.Tilde.WriteLine("~y~Starting in persistent mode (one connection only)");

            db.Persistent = true;

            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(Work);
                thread.Start(new Tuple<int, Energy.Source.Connection>(i, db));
            }

            Energy.Core.Tilde.Pause();

            Energy.Core.Tilde.WriteLine("~y~Now in not persistent mode (opening new connection per each statement)");

            db.Persistent = false;

            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(Work);
                thread.Start(new Tuple<int, Energy.Source.Connection>(i, db));
            }

            Energy.Core.Tilde.Pause();

            db.ConnectionString = @"Server=127.0.0.1;Database=platoon;Uid=platoon;Pwd=platoon;";
            db.Open();
            Console.WriteLine(db.Scalar("SELECT CURRENT_TIMESTAMP()"));

            Energy.Source.Structure.Table table = Energy.Source.Structure.Table.Create(typeof(UserTableRecord));
            Energy.Interface.IDialect script = new Energy.Query.Dialect.MYSQL();

            query = script.CreateDescription(table);
            Console.WriteLine(query);

            query = script.DropTable(table.Name);
            if (db.Execute(query) < 0)
            {
                Console.WriteLine(db.ErrorMessage);
            }
            Console.WriteLine(query);
            query = script.CreateTable(table);
            if (db.Execute(query) < 0)
            {
                Console.WriteLine(db.ErrorMessage);
            }
            Console.WriteLine(query);

            Console.ReadLine();
        }

        private static void Db_OnCreate(object sender, EventArgs e)
        {
            Energy.Core.Tilde.WriteLine("~ds~Creating database connection");
        }

        private static void Db_OnClose(object sender, EventArgs e)
        {
            Energy.Core.Tilde.WriteLine("~ds~Closing database connection");
        }

        public static void Work(object parameter)
        {
            Tuple<int, Energy.Source.Connection> data = (Tuple<int, Energy.Source.Connection>)parameter;
            int number = data.Item1;
            Energy.Source.Connection connection = data.Item2;
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Thread:Iteration {0}:{1} {2}"
                    , number + 1
                    , i + 1
                    , connection.Scalar<string>("DO SLEEP(1); SELECT CURRENT_TIMESTAMP()")
                    );
            }
        }
    }
}
