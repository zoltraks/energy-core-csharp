using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net.Sockets;
using System.Text;

namespace Energy.Abstract
{
    /// <summary>
    /// Abstract classes for networking
    /// </summary>
    public abstract class Network
    {
        public delegate bool SendDelegate<T>(T data);

        public delegate bool SendDelegate(byte[] data);

        public delegate bool ReceiveDelegate<T>(T data);

        public delegate bool ReceiveDelegate(byte[] data);

        public delegate bool ConnectDelegate(object self);

        public abstract class SocketConnection : Energy.Interface.ISocketConnection
        {
            [DefaultValue(false)]
            public bool Active;

            [DefaultValue(null)]
            public string Host;

            [DefaultValue(0)]
            public int Port;

            [DefaultValue(default(AddressFamily))]
            public AddressFamily Family;

            [DefaultValue(default(ProtocolType))]
            public ProtocolType Protocol;

            public event ReceiveDelegate OnReceive;

            public event SendDelegate OnSend;

            public event ConnectDelegate OnConnect;
        }

        public abstract class SocketClient : Energy.Abstract.Network.SocketConnection, Energy.Interface.ISocketClient
        {
            public virtual bool Connect()
            {
                throw new NotImplementedException();
            }
        }

        public abstract class SocketServer : Energy.Interface.ISocketServer, Energy.Interface.ISocketConnection
        {
            public virtual int Port
            {
                get;
                set;
            }

            public virtual event SendDelegate OnSend
            {
                add
                {
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
                }
            }

            public virtual event ReceiveDelegate OnReceive
            {
                add
                {
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
                }
            }

            public virtual bool Send(byte[] data)
            {
                throw new NotImplementedException();
            }

            public virtual bool Listen()
            {
                throw new NotImplementedException();
            }
        }
    }
}
