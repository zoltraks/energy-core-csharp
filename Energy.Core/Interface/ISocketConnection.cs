using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketConnection
    {
        event Energy.Base.Abstract.Network.ReceiveDelegate OnReceive;

        event Energy.Base.Abstract.Network.SendDelegate OnSend;
    }
}
