using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Abstract
{
    /// <summary>
    /// Abstract classes for networking
    /// </summary>
    public abstract class Network
    {
        public delegate bool SendDelegate(byte[] data);

        public delegate bool ReceiveDelegate(byte[] data);

        public abstract class SocketServer : Energy.Interface.ISocketServer
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
