using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketConnection
    {
        event Energy.Abstract.Network.ReceiveDelegate OnReceive;

        event Energy.Abstract.Network.SendDelegate OnSend;
    }
}
