﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Energy.Interface;
using System.Net;
using Energy.Abstract;
using System.IO;

namespace Energy.Core
{
    public class Network
    {
        #region Settings

        private static Energy.Base.Network.Settings _Settings;
        private static readonly object _SettingsLock = new object();
        public static Energy.Base.Network.Settings Settings
        {
            get
            {
                if (_Settings == null)
                {
                    lock (_SettingsLock)
                    {
                        if (_Settings == null)
                        {
                            _Settings = new Energy.Base.Network.Settings();
                        }
                    }
                }
                return _Settings;
            }
        }

        #endregion

        #region Utility

        public static string GetHostAddress(string host)
        {
            try
            {
                IPAddress ipAddress;
                if (!IPAddress.TryParse(host, out ipAddress))
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
                    ipAddress = ipHostInfo.AddressList[0];
                }
                return ipAddress.ToString();
            }
            catch (SocketException socketException)
            {
                Energy.Core.Bug.Catch(socketException);
                return null;
            }
        }

        public static AddressFamily GetAddressFamily(string address)
        {
            IPAddress ipAddress = null;
            if (IPAddress.TryParse(address, out ipAddress))
            {
                return ipAddress.AddressFamily;
            }
            return AddressFamily.Unknown;
        }

        #endregion

        #region Configure

        /// <summary>
        /// Return current socket configuration as array of strings.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string[] GetSocketConfigurationStringArray(System.Net.Sockets.Socket socket)
        {
            List<string> list = new List<string>();
            list.Add(string.Format("ReceiveBufferSize: {0}", socket.ReceiveBufferSize));
            list.Add(string.Format("ReceiveTimeout: {0}", socket.ReceiveTimeout));
            list.Add(string.Format("SendBufferSize: {0}", socket.SendBufferSize));
            list.Add(string.Format("SendTimeout: {0}", socket.SendTimeout));
            list.Add(string.Format("ExclusiveAddressUse: {0}", socket.ExclusiveAddressUse));
            list.Add(string.Format("Ttl: {0}", socket.Ttl));
            list.Add(string.Format("NoDelay: {0}", socket.NoDelay));
            list.Add(string.Format("LingerState: {0}, {1}", socket.LingerState.Enabled, socket.LingerState.LingerTime));
            list.Add(string.Format("IsBound: {0}", socket.IsBound));
            return list.ToArray();
        }

        /// <summary>
        /// Return current socket configuration as array of strings.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static string[] GetSocketConfigurationStringArray(System.Net.Sockets.TcpClient client)
        {
            List<string> list = new List<string>();
            list.Add(string.Format("ReceiveBufferSize: {0}", client.ReceiveBufferSize));
            list.Add(string.Format("ReceiveTimeout: {0}", client.ReceiveTimeout));
            list.Add(string.Format("SendBufferSize: {0}", client.SendBufferSize));
            list.Add(string.Format("SendTimeout: {0}", client.SendTimeout));
            list.Add(string.Format("ExclusiveAddressUse: {0}", client.ExclusiveAddressUse));
            list.Add(string.Format("NoDelay: {0}", client.NoDelay));
            list.Add(string.Format("LingerState: {0}, {1}", client.LingerState.Enabled, client.LingerState.LingerTime));
            //list.Add(string.Format("Available: {0}", socket.Available));
            return list.ToArray();
        }

        /// <summary>
        /// Make configuration of System.Net.Sockets.Socket object.
        /// </summary>
        /// <param name="socket">System.Net.Sockets.Socket object</param>
        /// <param name="buffer">Buffer size in bytes (typically 8192)</param>
        /// <param name="timeout">Timeout in milliseconds (10000 for 10 seconds)</param>
        /// <param name="linger">
        /// Specifies behaviour when closing socket, possible values are:
        /// negative number to disable (attempts to send pending data until the default IP protocol time-out expires),
        /// zero (discards any pending data, for connection-oriented socket Winsock resets the connection), and
        /// positive number of seconds (attempts to send pending data until the specified time-out expires,
        /// if the attempt fails, then Winsock resets the connection).
        /// </param>
        /// <param name="ttl">TTL value indicates the maximum number of routers the packet can traverse before the router discards the packet</param>
        /// <param name="exclusive">Don't allow another socket to bind to this port</param>
        /// <returns>System.Net.Sockets.Socket object itself</returns>
        public static System.Net.Sockets.Socket ConfigureSocket(System.Net.Sockets.Socket socket, int buffer, int timeout, int? linger, int? ttl, bool? exclusive)
        {
            if (exclusive != null)
            {
                // Don't allow another socket to bind to this port.
                // Should be true unless special cases.
                socket.ExclusiveAddressUse = (bool)exclusive;
            }

            if (linger != null)
            {
                // The socket will linger for specified amount of seconds after Socket.Close is called.
                // The typical reason to set a SO_LINGER timeout of zero is to avoid large numbers of connections
                // sitting in the TIME_WAIT state, tying up all the available resources on a server.
                if (linger < 0)
                {
                    socket.LingerState = new System.Net.Sockets.LingerOption(false, 0);
                }
                else if (linger == 0)
                {
                    socket.LingerState = new System.Net.Sockets.LingerOption(true, 0);
                }
                else
                {
                    socket.LingerState = new System.Net.Sockets.LingerOption(true, (int)linger);
                }
            }

            // Disable the Nagle Algorithm for this socket.
            // Sets NO_DELAY option for this socket.
            socket.NoDelay = true;

            // Set the receive buffer size
            socket.ReceiveBufferSize = buffer;

            // Set the send buffer size
            socket.SendBufferSize = buffer;

            // Set the timeout for synchronous receive methods
            socket.ReceiveTimeout = timeout;

            // Set the timeout for synchronous send methods
            socket.SendTimeout = timeout;

            if (ttl != null)
            {
                // Set the Time To Live (TTL) to specified router hops
                // or 42 by default.
                if (ttl == 0)
                {
                    socket.Ttl = 42;
                }
                else
                {
                    socket.Ttl = (short)ttl;
                }
            }

            Energy.Core.Bug.Write("Socket configuration: {0}", string.Join(", ", GetSocketConfigurationStringArray(socket)));

            return socket;
        }

        /// <summary>
        /// Make configuration of System.Net.Sockets.Socket object.
        /// </summary>
        /// <param name="socket">System.Net.Sockets.Socket object</param>
        /// <param name="buffer">Buffer size in bytes (typically 8192)</param>
        /// <param name="timeout">Timeout in milliseconds (10000 for 10 seconds)</param>
        /// <param name="linger">Linger option</param>
        /// <param name="ttl">Time To Live (TTL)</param>
        /// <param name="exclusive">Don't allow another socket to bind to this port</param>
        /// <returns>System.Net.Sockets.Socket object itself</returns>
        public static System.Net.Sockets.TcpClient ConfigureTcpClient(System.Net.Sockets.TcpClient socket, int buffer, int timeout, int? ttl, int? linger, bool? exclusive)
        {
            if (exclusive != null)
            {
                socket.ExclusiveAddressUse = (bool)exclusive;
            }

            if (linger != null)
            {
                if (linger < 0)
                {
                    socket.LingerState = new System.Net.Sockets.LingerOption(false, 0);
                }
                else if (linger == 0)
                {
                    socket.LingerState = new System.Net.Sockets.LingerOption(true, 0);
                }
                else
                {
                    socket.LingerState = new System.Net.Sockets.LingerOption(true, (int)linger);
                }
            }

            socket.NoDelay = true;
            socket.ReceiveBufferSize = buffer;
            socket.SendBufferSize = buffer;
            socket.ReceiveTimeout = timeout;
            socket.SendTimeout = timeout;

            Energy.Core.Bug.Write("TcpClient configuration: {0}", string.Join(", ", GetSocketConfigurationStringArray(socket)));

            return socket;
        }

        /// <summary>
        /// Make configuration of socket. For more details look for full ConfigureSocket().
        /// </summary>
        /// <param name="socket">Socket object</param>
        /// <param name="bufferSize">Buffer size in bytes (typically 8192)</param>
        /// <param name="timeout">Timeout in milliseconds (10000 for 10 seconds)</param>
        /// <param name="linger">Linger option</param>
        /// <param name="ttl">Time To Live (TTL)</param>
        /// <param name="exclusive">Don't allow another socket to bind to this port</param>
        /// <returns>System.Net.Sockets.Socket object itself</returns>
        public static System.Net.Sockets.TcpListener ConfigureTcpListener(System.Net.Sockets.TcpListener socket, int bufferSize, int timeout, int? ttl, int? linger, bool? exclusive)
        {
            ConfigureSocket(socket.Server, bufferSize, timeout, linger, ttl, exclusive);
            return socket;
        }

        /// <summary>
        /// Make configuration of System.Net.Sockets.Socket. For more details look for full ConfigureSocket().
        /// </summary>
        /// <param name="socket">Socket object</param>
        /// <returns>System.Net.Sockets.Socket object itself</returns>
        public static System.Net.Sockets.Socket ConfigureSocket(System.Net.Sockets.Socket socket)
        {
            return ConfigureSocket(socket, Settings.SocketBufferSize, Settings.SocketTimeout, null, null, null);
        }

        /// <summary>
        /// Make configuration of System.Net.Sockets.TcpClient. For more details look for full ConfigureSocket().
        /// </summary>
        /// <param name="socket">Socket object</param>
        /// <param name="bufferSize">Buffer size in bytes (typically 8192)</param>
        /// <returns>System.Net.Sockets.Socket object itself</returns>
        public static System.Net.Sockets.TcpClient ConfigureTcpClient(System.Net.Sockets.TcpClient socket, int bufferSize)
        {
            return ConfigureTcpClient(socket, bufferSize, Settings.SocketTimeout, null, null, null);
        }

        public static System.Net.Sockets.TcpClient ConfigureTcpClient(System.Net.Sockets.TcpClient socket, int bufferSize, int timeout)
        {
            return ConfigureTcpClient(socket, bufferSize, timeout, null, null, null);
        }

        public static System.Net.Sockets.TcpClient ConfigureTcpClient(TcpClient socket)
        {
            return ConfigureTcpClient(socket, Settings.SocketBufferSize, Settings.SocketTimeout, null, null, null);
        }

        [Energy.Attribute.Code.Obsolete]
        public static System.Net.Sockets.Socket ConfigureTcpSocket(Socket socket, int bufferSize)
        {
            return ConfigureSocket(socket, Settings.SocketBufferSize, Settings.SocketTimeout, null, null, null);
        }

        public static System.Net.Sockets.Socket ConfigureSocket(Socket socket, int bufferSize)
        {
            return ConfigureSocket(socket, Settings.SocketBufferSize, Settings.SocketTimeout, null, null, null);
        }

        public static System.Net.Sockets.Socket ConfigureSocket(Socket socket, int bufferSize, int timeout)
        {
            return ConfigureSocket(socket, Settings.SocketBufferSize, timeout, null, null, null);
        }

        #endregion

        #region SocketConnection

        public class SocketConnection: Energy.Abstract.Network.SocketConnection
        {
            public class StateObject
            {
                public Socket Socket;

                public int Capacity = 8192;

                public byte[] Buffer;

                public MemoryStream Stream;

                public int Repeat;

                public StateObject()
                {
                    Buffer = new byte[Capacity];
                    Stream = new MemoryStream(Capacity);
                }

                public StateObject(int repeat): this()
                {
                    Repeat = repeat;
                }
            }

            public volatile bool Active;

            public int Repeat;

            public StateObject State;

            private int _Backlog = 100;
            /// <summary>Backlog</summary>
            public int Backlog { get { return _Backlog; } set { _Backlog = value; } }

            public bool Listen()
            {
                string address;
                address = Energy.Core.Network.GetHostAddress(this.Host);
                AddressFamily family = this.Family;
                if (family == default(AddressFamily))
                {
                    family = Energy.Core.Network.GetAddressFamily(address);
                }
                Socket socket = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = Energy.Core.Network.GetHostAddress(Dns.GetHostName());
                    }
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(address), 11000);

                    socket.Bind(localEndPoint);
                    socket.Listen(Backlog);

                    this.State = new StateObject();
                    this.State.Socket = socket;

                    socket.BeginAccept(new AsyncCallback(AcceptCallback), this.State);
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Catch(e);
                    return false;
                }
                return true;
            }

            private void AcceptCallback(IAsyncResult ar)
            {
                StateObject state = ar.AsyncState as StateObject;
                try
                {
                    // Get the socket that handles the client request.
                    Socket socket = state.Socket;
                    Socket client = socket.EndAccept(ar);
                    SocketConnection connection = new SocketConnection();
                    connection.State = new StateObject();
                    connection.State.Socket = client;
                    //client.BeginReceive(connection.)
                    //// Create the state object.
                    ////StateObject state = new StateObject();
                    //state.workSocket = handler;
                    //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReadCallback), state);
                }
                catch
                { }
            }
        }

        #endregion

        #region SocketClient

        /// <summary>
        /// Client
        /// </summary>
        [Energy.Attribute.Code.Future]
        public class SocketClient : Energy.Abstract.Network.SocketClient
        {
            public override bool Connect()
            {
                return base.Connect();
            }
        }

        #endregion

        #region SocketServer

        /// <summary>
        /// Server
        /// </summary>
        [Energy.Attribute.Code.Future]
        public class SocketServer : Energy.Abstract.Network.SocketServer
        {
            public override int Port
            {
                get;
                set;
            }

            private event Energy.Abstract.Network.SendDelegate _OnSend;

            public override event Energy.Abstract.Network.SendDelegate OnSend
            {
                add
                {
                    _OnSend += value;
                }

                remove
                {
                    _OnSend -= value;
                }
            }

            private event Energy.Abstract.Network.ReceiveDelegate _OnReceive;

            public override event Energy.Abstract.Network.ReceiveDelegate OnReceive
            {
                add
                {
                    _OnReceive += value;
                }

                remove
                {
                    _OnReceive -= value;
                }
            }

            public override bool Send(byte[] data)
            {
                return false;
            }

            public override bool Listen()
            {
                return false;
            }
        }

        #endregion

        #region Ping

        public static int Ping(string address, int timeout, out System.Net.NetworkInformation.IPStatus status)
        {
            status = System.Net.NetworkInformation.IPStatus.Unknown;
            System.Net.NetworkInformation.Ping request = new System.Net.NetworkInformation.Ping();
            try
            {
                System.Net.NetworkInformation.PingReply response = request.Send(address, timeout);
                status = response.Status;
                if (response.Status == IPStatus.Success)
                {
                    return (int)response.RoundtripTime;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception x)
            {
                Energy.Core.Bug.Catch(x);
                return -1;
            }
        }

        public static int Ping(string address, int timeout)
        {
            System.Net.NetworkInformation.IPStatus status;
            return Ping(address, timeout, out status);
        }

        public static int Ping(string address)
        {
            System.Net.NetworkInformation.IPStatus status;
            return Ping(address, Energy.Base.Network.DEFAULT_PING_TIMEOUT, out status);
        }

        #endregion
    }
}
