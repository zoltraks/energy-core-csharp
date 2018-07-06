using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketServer: ISocketConnection
    {
        int Port { get; set; }

        bool Listen();

        bool Send(byte[] data);
    }
}
