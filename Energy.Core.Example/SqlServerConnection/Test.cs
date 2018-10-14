using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

            connection.ThreadEvent = true;

            Energy.Base.ConnectionString cs = new Energy.Base.ConnectionString(connection.ConnectionString);
            Console.WriteLine(cs.ToDsnString());

            string scalarString1 = connection.Scalar<string>("SELECT GETDATE()");

            connection.ThreadEvent = false;

            string scalarString2 = connection.Scalar<string>("SELECT GETDATE()");
        }

        private static void Connection_OnClose(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            Energy.Core.Tilde.WriteLine(Energy.Base.Clock.CurrentTimeMilliseconds + " "
                + "Connection close "
                + (sender as Energy.Source.Connection ?? Energy.Source.Connection.Empty).ToString());
        }

        private static void Connection_OnOpen(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            Energy.Core.Tilde.WriteLine(Energy.Base.Clock.CurrentTimeMilliseconds + " "
                + "Connection open "
                + (sender as Energy.Source.Connection ?? Energy.Source.Connection.Empty).ToString());
        }

        private static void Connection_OnCreate(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            Energy.Core.Tilde.WriteLine(Energy.Base.Clock.CurrentTimeMilliseconds + " "
                + "Connection create "
                + (sender as Energy.Source.Connection ?? Energy.Source.Connection.Empty).ToString());
        }
    }
}
