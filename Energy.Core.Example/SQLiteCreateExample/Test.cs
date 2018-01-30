using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteCreateExample
{
    public class Test
    {
        internal static Energy.Source.Connection ConnectToMemoryDatabase()
        {
            Energy.Source.Connection db = null;
            string connectionString = @"Data Source=:memory:;Version=3;New=True;";
            db = new Energy.Source.Connection<System.Data.SQLite.SQLiteConnection>(connectionString);
            // needed for :memory: !!!
            //db.Pooling = false;
            if (db.Bool("select current_timestamp;"))
            {
                Console.WriteLine("Connection to " + connectionString + " open");
            }
            else
            {
                Console.WriteLine("Connection error " + db.ErrorStatus);
                // Dispose() will invoke Close()
                db.Dispose();
                db = null;
            }
            return db;
        }

        internal static void CreateSeveralTables()
        {
            Energy.Source.Structure.Table table;
            Energy.Query.Script q = new Energy.Query.Script.SQLite();
            string query;
            table = Energy.Source.Structure.Table.Create(typeof(BaseRecord));
            query = q.CreateTable(table);
            Console.WriteLine(query);
            table = Energy.Source.Structure.Table.Create(typeof(AddressRecord));
            query = q.CreateTable(table);
            Console.WriteLine(query);
            table = Energy.Source.Structure.Table.Create(typeof(InvoiceMasterRecord));
            query = q.CreateTable(table);
            Console.WriteLine(query);
            table = Energy.Source.Structure.Table.Create(typeof(InvoiceDataRecord));
            query = q.CreateTable(table);
            Console.WriteLine(query);
        }
    }
}
