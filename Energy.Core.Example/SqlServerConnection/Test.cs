using System;
using System.Collections.Generic;
using System.Text;
using Energy.Source;

namespace SqlServerConnection
{
    public class Test
    {
        public static void UseEvents(Connection connection)
        {
            connection.OnCreate += Connection_OnCreate;
            connection.OnOpen += Connection_OnOpen;
            connection.OnClose += Connection_OnClose;

            Energy.Base.ConnectionString cs = new Energy.Base.ConnectionString(connection.ConnectionString);
            Console.WriteLine(cs.ToDsnString());

            string scalarString = connection.Scalar<string>("SELECT GETDATE()");
        }

        private static void Connection_OnClose(object sender, EventArgs e)
        {
            Energy.Core.Tilde.WriteLine("Connection close "
                + (sender as Energy.Source.Connection ?? Energy.Source.Connection.Empty).ToString());
        }

        private static void Connection_OnOpen(object sender, EventArgs e)
        {
            Energy.Core.Tilde.WriteLine("Connection open "
               + (sender as Energy.Source.Connection ?? Energy.Source.Connection.Empty).ToString());
        }

        private static void Connection_OnCreate(object sender, EventArgs e)
        {
            Energy.Core.Tilde.WriteLine("Connection create "
                 + (sender as Energy.Source.Connection ?? Energy.Source.Connection.Empty).ToString());
        }
    }
}
