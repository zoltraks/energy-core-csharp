﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketConnection
    {
        event Energy.Base.Network.ConnectDelegate OnConnect;

        event Energy.Base.Network.CloseDelegate OnClose;

        event Energy.Base.Network.ListenDelegate OnListen;

        event Energy.Base.Network.AcceptDelegate OnAccept;

        event Energy.Base.Network.ReceiveDelegate OnReceive;

        event Energy.Base.Network.SendDelegate OnSend;

        event Energy.Base.Network.TimeoutDelegate OnTimeout;
    }
}
