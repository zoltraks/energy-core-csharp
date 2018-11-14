using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Base classes for networking.
    /// </summary>
    public class Network
    {
        #region Constant

        public const int DEFAULT_SOCKET_TIMEOUT = 10000;

        public const int DEFAULT_SOCKET_BUFFER_SIZE = 8192;

        public const int DEFAULT_PING_TIMEOUT = 30000;

        #endregion

        #region Delegate

        public delegate void ConnectDelegate(object self);

        public delegate void CloseDelegate(object self);

        public delegate void ListenDelegate(object self);

        public delegate void AcceptDelegate(object self);

        public delegate void SendDelegate<T>(object self, T data);

        public delegate void SendDelegate(object self, byte[] data);

        public delegate void ReceiveDelegate<T>(object self, T data);

        public delegate void ReceiveDelegate(object self, byte[] data);

        public delegate void ErrorDelegate(object self);

        public delegate void ExceptionDelegate(object self, Exception exception);

        #endregion

        #region Settings

        /// <summary>
        /// Settings
        /// </summary>
        public class Settings
        {
            public int SocketTimeout { get; set; }

            public int SocketBufferSize { get; set; }

            public Settings()
            {
                SocketTimeout = DEFAULT_SOCKET_TIMEOUT;
                SocketBufferSize = DEFAULT_SOCKET_BUFFER_SIZE;
            }
        }

        #endregion

        #region Connection

        /// <summary>
        /// Network connection information
        /// </summary>
        public class Connection
        {
            [DefaultValue(null)]
            public string Host { get; set; }

            [DefaultValue(null)]
            public string Protocol { get; set; }

            public ProtocolType Type { get; set; }

            [DefaultValue(0)]
            public int Port { get; set; }
        }

        #endregion
    }
}
