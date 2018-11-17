using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketConnection
    {
        event Energy.Base.Network.ExceptionDelegate OnException;

        event Energy.Base.Network.CloseDelegate OnClose;

        event Energy.Base.Network.ReceiveDelegate OnReceive;

        event Energy.Base.Network.SendDelegate OnSend;
    }
}
