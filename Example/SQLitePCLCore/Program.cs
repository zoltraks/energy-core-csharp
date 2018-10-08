using System;

namespace SQLitePCLCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("Hello World!");

            Initialize(args);

            Energy.Core.Tilde.Pause();
        }

        private static void Initialize(string[] args)
        {
            SQLitePCL.Batteries_V2.Init();

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        }
    }
}
