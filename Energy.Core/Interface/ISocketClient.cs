using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Energy.Interface
{
    /// <summary>
    /// Common interface for database connection helper classes.
    /// </summary>
    public interface ISocketClient
    {
        event Energy.Base.Network.ExceptionDelegate OnError;

        event Energy.Base.Network.ConnectDelegate OnConnect;

        event Energy.Base.Network.CloseDelegate OnClose;

        event Energy.Base.Network.ReceiveDelegate OnReceive;

        event Energy.Base.Network.SendDelegate OnSend;

        /// <summary>
        /// Connect to remote.
        /// </summary>
        /// <returns></returns>
        bool Connect();
    }
}
