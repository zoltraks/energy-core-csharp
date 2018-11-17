using System;
using System.Collections.Generic;
using System.Text;

namespace SocketConnectionServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("~g~Network ~m~Server~0~ using ~y~SocketConnection");

            Energy.Core.Network.SocketServer serverConnection = new Energy.Core.Network.SocketServer();
            serverConnection.Host = "localhost";
            serverConnection.Port = 9000;
            serverConnection.OnReceive += ServerConnection_OnReceive;
            serverConnection.OnAccept += ServerConnection_OnAccept;
            serverConnection.OnListen += ServerConnection_OnListen;
            serverConnection.Listen();

            Energy.Core.Tilde.Pause();
        }

        private static void ServerConnection_OnListen(object self)
        {
            Energy.Core.Network.SocketConnection serverConnection = self as Energy.Core.Network.SocketConnection;
            Console.WriteLine("Server is listening at " + serverConnection.ToString());
        }

        private static void ServerConnection_OnReceive(object self, byte[] data)
        {
            Energy.Core.Network.SocketConnection clientConnection = self as Energy.Core.Network.SocketConnection;
            string text = Encoding.UTF8.GetString(data);
            Console.WriteLine("Client message from " + clientConnection.ToString() + ": " + text);
            clientConnection.Send(System.Text.Encoding.UTF8.GetBytes(text));
        }

        private static void ServerConnection_OnAccept(object self)
        {
            Energy.Core.Network.SocketConnection clientConnection = self as Energy.Core.Network.SocketConnection;
            string text = string.Format(Global.HelloText, Global.ClientCounter.Increment());
            clientConnection.Send(System.Text.Encoding.UTF8.GetBytes(text));
        }
    }
}
