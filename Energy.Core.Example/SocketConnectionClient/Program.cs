using System;
using System.Collections.Generic;
using System.Text;

namespace SocketConnectionClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("~g~Network ~c~Client~0~ using ~y~SocketConnection");

            Energy.Core.Network.SocketConnection connection = new Energy.Core.Network.SocketConnection();
            connection.Host = "localhost";
            connection.Port = 9000;
            connection.OnReceive += Connection_OnReceive;
            connection.OnSend += Connection_OnSend;
            connection.OnConnect += Connection_OnConnect;
            Energy.Core.Tilde.Pause();
        }

        private static bool Connection_OnConnect(object self)
        {
            Energy.Core.Network.SocketConnection connection = self as Energy.Core.Network.SocketConnection;
            if (connection == null)
            {
                Energy.Core.Tilde.WriteLine("~r~ERROR~0~ ~lm~Object is null in ~y~Connection_OnConnect");
                return false;
            }
        }

        private static bool Connection_OnSend(byte[] data)
        {
            throw new NotImplementedException();
        }

        private static bool Connection_OnReceive(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
