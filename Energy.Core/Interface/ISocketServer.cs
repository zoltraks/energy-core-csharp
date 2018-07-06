using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketServer
    {
        int Port { get; set; }

        bool Listen();

        bool Send(byte[] data);

        event Energy.Abstract.Network.ReceiveDelegate OnSend;

        event Energy.Abstract.Network.ReceiveDelegate OnReceive;
    }
}
