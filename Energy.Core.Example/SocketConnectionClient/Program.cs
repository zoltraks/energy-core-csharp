using System;
using System.Collections.Generic;
using System.Text;

namespace SocketConnectionClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("Welcome to ~lm~Network ~lc~Client~0~ using ~y~Energy.Core.Network.~lc~SocketClient");

            Energy.Core.Network.SocketClient connection = new Energy.Core.Network.SocketClient();
            connection.Host = "localhost";
            connection.Port = 9000;
            connection.OnReceive += Connection_OnReceive;
            connection.OnSend += Connection_OnSend;
            connection.OnConnect += Connection_OnConnect;
            connection.OnException += Connection_OnException;

            connection.Timeout = 5000;
            connection.Retry = 1;

            connection.Connect();

            Energy.Core.Tilde.Pause();
        }

        private static bool Connection_OnException(object self, Exception exception)
        {
            Energy.Core.Tilde.WriteLine(string.Format("~y~{0} ~r~Error ~w~{1}"
                , Energy.Base.Clock.CurrentTimeSeconds, exception.Message));
            Energy.Core.Tilde.WriteException(exception, true);
            return true;
        }

        private static void Connection_OnConnect(object self)
        {
            Energy.Core.Network.SocketClient connection = self as Energy.Core.Network.SocketClient;
            if (connection == null)
            {
                string tilde = "~r~ERROR~0~ ~lm~Object is null in ~y~Connection_OnConnect";
                Energy.Core.Tilde.WriteLine(tilde);
                return;
            }
            int size = 10000;
            if (!connection.Send(new string('A', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('B', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('C', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('D', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Receive())
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Receive error in ~y~Connection_OnConnect");
            }
            else
            {
                Energy.Core.Tilde.WriteLine("~r~OK~0~ ~lc~Start receiving in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('A', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('B', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('C', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
            if (!connection.Send(new string('D', size) + "\r\n"))
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Send error in ~y~Connection_OnConnect");
            }
        }

        private static void Connection_OnSend(object self, byte[] data)
        {
            var connection = self as Energy.Core.Network.SocketClient;
            string message = Energy.Base.Text.Limit(connection.Encoding.GetString(data).Trim(), 13, "...");
            string time = Energy.Base.Cast.TimeSpanToStringMicroseconds(DateTime.Now - connection.ConnectStamp);
            Energy.Core.Tilde.WriteLine($"~y~{time} ~g~SEND~0~ ~w~{message}");
        }

        private static void Connection_OnReceive(object self, byte[] data)
        {
            var connection = self as Energy.Core.Network.SocketClient;
            string message = Energy.Base.Text.Limit(connection.Encoding.GetString(data).Trim(), 13, "...");
            string time = Energy.Base.Cast.TimeSpanToStringMicroseconds(DateTime.Now - connection.ConnectStamp);
            Energy.Core.Tilde.WriteLine($"~y~{time} ~g~RECEIVE~0~ ~w~{message}");
        }
    }
}
