using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlServerCreateAndPopulate
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
        }

        private static void Go(string[] args)
        {
            string defaultConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Test;Integrated Security=Yes;Connect Timeout=10;";
            Console.WriteLine("Default connection string is {0}", defaultConnectionString);
            Console.Write("Connection string (enter \".\" to use default): ");
            string connectionString = "."; // Console.ReadLine();
            if (connectionString == ".")
                connectionString = defaultConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                return;
            Energy.Source.Connection<System.Data.SqlClient.SqlConnection> x
                = new Energy.Source.Connection<System.Data.SqlClient.SqlConnection>();
            x.ConnectionString = connectionString;
            do
            {
                using (var c = x.Copy())
                {
                    if (c == null)
                        break;

                    var outScalar = c.Scalar("SELECT * FROM logTable1");

                    //c.Pooling = true;
                    //if (c.Open() == null)
                    //Console.WriteLine("Cannot open connection");
                }
            } while (false);


            //using (var c = x.Clone())
            //{
            //    c.Pooling = false;
            //    if (c.Open() == null)
            //        Console.WriteLine("Cannot open connection");
            //}
            //Core.Source.ConnectionString = connectionString;
            //Core.Source.Open();
            //if (!Core.Source.Active)
            //{
            //    Console.WriteLine("Cannot open connection");
            //    return;
            //}
            //Core.Source.Close();
            //Core.Log.SystemLogTable = "";
            //Core.Log.ConsoleLog = true;
            //Core.Log.AutoFlush = false;

            Pool pool = new Pool();
            int count = 3;
            count = (int)Energy.Core.Tilde.Input<int>("Enter thread count [~y~{0}~0~] : ~c~", count);
            for (int i = 0; i < count; i++)
            {
                Worker worker = new Worker();
                worker.Name = Energy.Base.Hex.Random();
                pool.Add(worker);
            }
            Energy.Core.Tilde.WriteLine("Input string is {0}", count);

            //Core.Log.Push("Starting... enter anything to stop");
            //Core.Log.Flush();

            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].Thread.Start();
            }

            while (true)
            {
                System.Threading.Thread.Sleep(100);
                //Core.Log.Flush();
                string input = Energy.Core.Tilde.ReadLine();
                if (input == null)
                    continue;
                break;
            }

            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].Thread.Abort();
            }
        }

        public class Worker
        {
            private System.Threading.Thread _Thread;
            /// <summary>Thread</summary>
            public System.Threading.Thread Thread { get { return _Thread; } set { _Thread = value; } }

            private string _Name;
            /// <summary>Name</summary>
            public string Name { get { return _Name; } set { _Name = value; } }

            public Worker()
            {
                _Thread = new System.Threading.Thread(Work);
            }

            public void Work()
            {
                try
                {
                    //Core.Log.Push(_Name + " " + "Start");
                    //Core.Log.Push(_Name + " " + Core.Source.ConnectionString);
                    //if (!Core.Source.Active && !Core.Source.Open())
                    //    return;
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    Core.Log.Push(_Name + " " + Core.Source.FetchScalar("SELECT GETDATE()"));
                    //    if (null == Core.Source.Execute("WAITFOR DELAY '00:00:03'"))
                    //    {
                    //        Core.Log.Push(_Name + " " + Core.Source.Status);
                    //    }
                    //}
                    //Core.Source.Close();
                }
                catch (System.Threading.ThreadAbortException)
                {
                    //Core.Log.Push(_Name + " " + "Abort");
                }
            }
        }

        public class Pool : List<Worker>
        {
        }
    }
}
