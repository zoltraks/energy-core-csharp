using System;
using System.Collections.Generic;
using System.Text;

namespace ChatSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Interface.ISocketServer server = new Energy.Core.Network.SocketServer();
            server.Port = 8080;
        }
    }
}
