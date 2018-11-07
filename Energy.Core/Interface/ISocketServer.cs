using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketServer
    {
        event Energy.Base.Network.ErrorDelegate OnError;

        event Energy.Base.Network.ListenDelegate OnListen;

        event Energy.Base.Network.AcceptDelegate OnAccept;
    }
}
