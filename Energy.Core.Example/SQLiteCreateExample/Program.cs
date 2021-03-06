﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SQLiteCreateExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Test1(args);
                //db = Test.ConnectToMemoryDatabase();
                //Test.CreateSeveralTables();
                //Console.ReadLine();
                //db.Close();

                Test1(args);
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

        private static Energy.Source.Connection db = null;

        private static void Test1(string[] args)
        {
            string connectionString = @"Data Source=:memory:;Version=3;New=True;";
            db = new Energy.Source.Connection<System.Data.SQLite.SQLiteConnection>(connectionString);        
            db.Persistent = true; // needed for :memory: !!!
            Console.WriteLine(db.Scalar("select current_timestamp;"));
            Console.WriteLine(db.Scalar("select time(time(), 'localtime');"));
            Console.WriteLine(db.Scalar("SELECT (STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW'))"));
            Energy.Interface.IDialect dialect = new Energy.Query.Dialect.SQLITE();
            Energy.Interface.IDialect script = new Energy.Query.Dialect.SQLITE();
            string query;
            query = "select " + dialect.Format.CurrentStamp;
            Console.WriteLine(query);
            Console.WriteLine(db.Scalar(query));

            Energy.Source.Structure.Table table;
            table = Energy.Source.Structure.Table.Create(typeof(UserTableRecord));
            query = dialect.CreateTable(table);

            Console.WriteLine(query);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);

            query = "INSERT INTO " + script.Format.Object("UserTable") + " ( Name , Phone ) VALUES ( 'List' , 1234 )";

            Console.WriteLine(query);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);

            query = "INSERT INTO " + script.Format.Object("UserTable") + " ( Name , Phone ) VALUES ( 'Element numer 1' , 4143141414 )";

            Console.WriteLine(query);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);

            query = "INSERT INTO " + script.Format.Object("UserTable") + " ( Name ) VALUES ( 'Po prostu element' )";

            Console.WriteLine(query);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);

            query = "INSERT INTO " + script.Format.Object("UserTable") + " ( Name , Description ) VALUES ( '===' , 'ĄćęłńóśćżŹ' )";

            Console.WriteLine(query);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);

            query = "SELECT * FROM " + script.Format.Object("UserTable");

            Console.WriteLine(query);
            DataTable dx1 = db.Load(query);
            string text = Energy.Base.Plain.DataTableToPlainText(dx1, new Energy.Base.Plain.TableFormat.FullFrame() { Tilde = true });
            Energy.Core.Tilde.WriteLine(text);
            if (db.Execute(query) < 0)
                Console.WriteLine(db.ErrorMessage);

            var tbl = db.Fetch(query);
            foreach (var rec in tbl)
            {
                Console.WriteLine(rec.ToString(": "));
            }

            Console.WriteLine(query);
        }
    }
}
