using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AsynchronousNetworkClient
{
    public class StateObject
    {
        public Socket Socket;

        public int Capacity = 8192;

        public byte[] Buffer;

        public MemoryStream Stream;

        public int ConnectionRepeat { get; internal set; }
        public string Host { get; internal set; }
        public int Port { get; internal set; }
    }
}
