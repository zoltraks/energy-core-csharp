﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Threading;
#if !NETCF
using System.Net.NetworkInformation;
#endif

namespace Energy.Core
{
    public class Network
    {
        #region Utility

        #region GetHostAddress

        public static string GetHostAddress(string host, AddressFamily addressFamily)
        {
            //if (host == null)
            //{
            //    return null;
            //}

            //if (host == "")
            //{
            //    return "";
            //}

            if (string.IsNullOrEmpty(host))
            {
                return host;
            }

            if (0 == string.Compare(host, "localhost", true) || host == ".")
            {
                return IPAddress.Parse("127.0.0.1").ToString();
            }

            if (Energy.Base.Network.IsValidAddress(host))
            {
                try
                {
                    IPAddress ipAddress = IPAddress.Parse(host);
                    return ipAddress.ToString();
                }
                catch (FormatException)
                {
                }
            }


            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                if (ipHostInfo.AddressList.Length > 0 && addressFamily != AddressFamily.Unspecified)
                {
                    Energy.Core.Bug.Write("Energy.Core.Network.GetHostAddress", () =>
                    {
                        List<string> ls = new List<string>();
                        for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
                        {
                            ls.Add(ipHostInfo.AddressList[i].ToString());
                        }
                        string msg = string.Join(" , ", ls.ToArray());
                        return msg;
                    });
                    for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
                    {
                        ipAddress = ipHostInfo.AddressList[i];
                        if (ipAddress.AddressFamily == addressFamily)
                        {
                            return ipAddress.ToString();
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Energy.Core.Bug.Catch(socketException);
            }

            return null;
        }

        public static string GetHostAddress(string host)
        {
            AddressFamily addressFamily = Settings.AddressFamily;
            if (addressFamily == AddressFamily.Unspecified)
            {
                addressFamily = GetAddressFamily(host);
            }
            return GetHostAddress(host, addressFamily);
        }

        #endregion

        /// <summary>
        /// Return System.Net.Sockets.AddressFamily appropriate to host address.
        /// This function will try parse IPAddress or use Dns.GetHostEntry to find correct value.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>
        /// AddressFamily.Unknown might be returned if correct value could not be determined.
        /// </returns>
        public static AddressFamily GetAddressFamily(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return AddressFamily.Unknown;
            }

            if (address == "localhost" || address == "127.0.0.1")
            {
                return AddressFamily.InterNetwork;
            }

            if (address == "::1" || address == "[::1]" || address == "::" || address == "[::]")
            {
                return AddressFamily.InterNetworkV6;
            }

            if (Energy.Base.Network.IsValidAddress(address))
            {
                try
                {
                    IPAddress ipAddress = IPAddress.Parse(address);
                    return ipAddress.AddressFamily;
                }
                catch (FormatException formatException)
                {
                    Energy.Core.Bug.Catch("Energy.Core.Network.GetAddressFamily", formatException);
                }
            }

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(address);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                return ipAddress.AddressFamily;
            }
            catch (SocketException socketException)
            {
                Energy.Core.Bug.Catch("Energy.Core.Network.GetAddressFamily", socketException);
            }

            return AddressFamily.Unknown;
        }

        public static SocketType GetSocketType(ProtocolType protocol, AddressFamily family)
        {
            switch (protocol)
            {
                default:
                    return SocketType.Unknown;

                case ProtocolType.Tcp:
                    switch (family)
                    {
                        default:
                            return SocketType.Unknown;
                        case AddressFamily.InterNetwork:
                        case AddressFamily.InterNetworkV6:
                            return SocketType.Stream;
                    }

                case ProtocolType.Udp:
                    switch (family)
                    {
                        default:
                            return SocketType.Unknown;
                        case AddressFamily.InterNetwork:
                        case AddressFamily.InterNetworkV6:
                            return SocketType.Dgram;
                    }

                case ProtocolType.Igmp:
                    return SocketType.Raw;

            }
        }

        public static void Shutdown(Socket socket)
        {
            if (socket == null)
            {
                return;
            }
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                //socket.Disconnect(true);
                //socket.Disconnect(false);
            }
            catch (SocketException socketException)
            {
                Energy.Core.Bug.Catch(socketException);
            }
            catch (ObjectDisposedException exceptionObjectDisposed)
            {
                Energy.Core.Bug.Catch(exceptionObjectDisposed);
            }
            try
            {
                socket.Close();
            }
            catch (Exception exception)
            {
                Energy.Core.Bug.Catch("Energy.Core.Network.Shutdown", exception);
            }
        }

        #endregion

        #region GetHostName

        /// <summary>
        /// Get host name of the machine process is running on.
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            string name = "";
#if !NETCF
            name = Environment.MachineName;
#endif
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
            try
            {
                name = System.Net.Dns.GetHostName();
            }
            catch (SocketException exceptionSocket)
            {
                Energy.Core.Bug.Catch("Energy.Core.Network.GetHostName", exceptionSocket);
            }
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
#if !NETCF
            name = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
#endif
            return name;
        }

        #endregion

        #region IsConnected

        /// <summary>
        /// Check if socket is connected.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static bool IsConnected(Socket socket)
        {
            if (socket == null)
            {
                return false;
            }
            else if (!socket.Connected)
            {
                return false;
            }
            else if (socket.Available > 0)
            {
                return true;
            }
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }

        #endregion

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

        #region Configure

        /// <summary>
        /// Return current socket configuration as array of strings.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string[] GetSocketConfigurationStringArray(System.Net.Sockets.Socket socket)
        {
            List<string> list = new List<string>();
#if !NETCF
            list.Add(string.Format("ReceiveBufferSize: {0}", socket.ReceiveBufferSize));
            list.Add(string.Format("ReceiveTimeout: {0}", socket.ReceiveTimeout));
            list.Add(string.Format("SendBufferSize: {0}", socket.SendBufferSize));
            list.Add(string.Format("SendTimeout: {0}", socket.SendTimeout));
            list.Add(string.Format("ExclusiveAddressUse: {0}", socket.ExclusiveAddressUse));
            list.Add(string.Format("Ttl: {0}", socket.Ttl));
            list.Add(string.Format("NoDelay: {0}", socket.NoDelay));
            list.Add(string.Format("LingerState: {0}, {1}", socket.LingerState.Enabled, socket.LingerState.LingerTime));
            list.Add(string.Format("IsBound: {0}", socket.IsBound));
#endif
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
#if !NETCF
            list.Add(string.Format("ExclusiveAddressUse: {0}", client.ExclusiveAddressUse));
#endif
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
#if !NETCF
                // Don't allow another socket to bind to this port.
                // Should be true unless special cases.
                socket.ExclusiveAddressUse = (bool)exclusive;
#endif
            }

            if (linger != null)
            {
#if !NETCF
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
#endif
            }

#if !NETCF
            // Disable the Nagle Algorithm for this socket.
            // Sets NO_DELAY option for this socket.
            socket.NoDelay = true;
#endif

#if !NETCF
            // Set the receive buffer size
            socket.ReceiveBufferSize = buffer;
#endif

#if !NETCF
            // Set the send buffer size
            socket.SendBufferSize = buffer;
#endif

#if !NETCF
            // Set the timeout for synchronous receive methods
            socket.ReceiveTimeout = timeout;
#endif

#if !NETCF
            // Set the timeout for synchronous send methods
            socket.SendTimeout = timeout;
#endif

            if (ttl != null)
            {
#if !NETCF
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
#endif
            }

            Energy.Core.Bug.Write("Energy.Core.Network.ConfigureSocket", string.Format("Socket configuration: {0}", string.Join(", ", GetSocketConfigurationStringArray(socket))));

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
#if !NETCF
                socket.ExclusiveAddressUse = (bool)exclusive;
#endif
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

            Energy.Core.Bug.Write(string.Format("TcpClient configuration: {0}", string.Join(", ", GetSocketConfigurationStringArray(socket))));

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

        #region SocketClient

        public class SocketClient : SocketConnection, Energy.Interface.ISocketClient
        {
            #region Property

            private bool _AlwaysReceive = false;
            /// <summary>
            /// Start receiving automatically after connection is established
            /// and keep receiving until connection is deactivated.
            /// </summary>
            public bool AlwaysReceive { get { return _AlwaysReceive; } set { _AlwaysReceive = value; } }

            #endregion

            #region Stamp

            public DateTime ConnectStamp { get; private set; }

            public DateTime SendStamp { get; private set; }

            public DateTime ReceiveStamp { get; private set; }

            #endregion

            #region Event

            public event Energy.Base.Network.ConnectDelegate OnConnect;

            public event Energy.Base.Network.CloseDelegate OnClose;

            public event Energy.Base.Network.ReceiveDelegate OnReceive;

            public event Energy.Base.Network.SendDelegate OnSend;

            public event Energy.Base.Network.ExceptionDelegate OnException;

            #endregion

            #region Synchronisation

            public ManualResetEvent ConnectDone = new ManualResetEvent(true);

            public ManualResetEvent ReceiveDone = new ManualResetEvent(true);

            public ManualResetEvent SendDone = new ManualResetEvent(true);

            #endregion

            #region Connect

            public bool Connect()
            {
                if (this.Active || this.Connected)
                {
                    Close();
                }

                Clear();

                if (ConnectDone.WaitOne(0, true))
                {
                    if (!ConnectBegin())
                    {
                        return false;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Connect operation already waiting for callback");
                }

                return true;

            }

            public bool ConnectBegin()
            {
                ConnectDone.Reset();

                try
                {
                    Socket connectSocket = this.CreateSocket();

                    this.Socket = connectSocket;
                    this.Active = true;
                    this.Connected = false;

                    IPEndPoint endPoint = this.GetEndPoint();

                    connectSocket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), this);

                    int timeout = this.Timeout;
                    bool timeoutEnable = timeout > 0;
                    if (timeoutEnable)
                    {
                        Energy.Core.Worker.Fire(() =>
                        {
                            if (!this.ConnectDone.WaitOne(timeout, true))
                            {
                                //this.Active = false;
                                this.Close();
                            }
                        });
                    }

                    return true;
                }
                catch (Exception exception)
                {
                    if (this.OnException != null)
                    {
                        this.OnException(this, exception);
                    }
                    return false;
                    throw;
                }

            }

            private void ConnectCallback(IAsyncResult ar)
            {
                SocketClient connection = ar.AsyncState as SocketClient;

                if (connection == null)
                {
                    throw new ArgumentNullException("Asynchronous State Object");
                }

                try
                {
                    Socket remoteSocket = connection.Socket;

                    if (remoteSocket == null)
                        return;

                    try
                    {
                        remoteSocket.EndConnect(ar);
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (ArgumentException)
                    {
                        return;
                    }
                    catch (Exception x)
                    {
                        if (!connection.Catch(x))
                            throw;
                    }

                    DateTime now = DateTime.Now;
                    connection.ActivityStamp = now;
                    connection.ConnectStamp = now;

                    connection.ConnectDone.Set();

                    if (!this.Active)
                        return;

                    connection.Connected = true;

                    if (connection.OnConnect != null)
                    {
                        connection.OnConnect(this);
                    }

                    if (connection.AlwaysReceive)
                    {
                        connection.Receive();
                    }
                }
                catch (SocketException socketException)
                {
                    //switch (socketException.SocketErrorCode)
                    //{
                    //    default:
                    //        connection.Active = false;
                    //        break;

                    //    case SocketError.ConnectionRefused:
                    //        connection.Active = false;
                    //        break;

                    //    case SocketError.TimedOut:
                    //        connection.Active = false;
                    //        break;
                    //}

                    switch (socketException.ErrorCode)
                    {
                        default:
                            connection.Active = false;
                            break;

                        case 10061: // ConnectionRefused
                            connection.Active = false;
                            break;

                        case 10060: // TimedOut
                            connection.Active = false;
                            break;
                    }

                    if (this.OnException != null)
                    {
                        this.OnException(this, (Exception)socketException);
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Write("Energy.Core.Network.SocketClient.ConnectCallback", exception);

                    if (this.OnException != null)
                    {
                        this.OnException(this, (Exception)exception);
                    }

                    if (this.OnException == null)
                    {
                        throw;
                    }
                }
            }

            #endregion

            #region Send

            private readonly object SendLock = new object();

            public virtual bool Send(byte[] data)
            {
                if (!this.Active)
                    return false;
                if (!this.Connected)
                    return false;

                lock (SendLock)
                {
                    SendBuffer.Push(data);
                    Energy.Core.Bug.Write("X " + SendBuffer.Count);
                }

                if (SendDone.WaitOne(0, true))
                {
                    if (!SendBegin())
                    {
                        return false;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Send operation already waiting for callback");
                }

                return true;
            }

            private bool SendBegin()
            {
                SendDone.Reset();

                if (!this.Active)
                    return false;
                if (!this.Connected)
                    return false;

                byte[] data = null;

                lock (SendLock)
                {
                    data = SendBuffer.First;
                }

                if (data == null)
                {
                    SendDone.Set();
                    return true;
                }

                Socket clientSocket = this.Socket;

                clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, this);

                if (this.OnSend != null)
                {
                    this.OnSend(this, data);
                }

                return true;
            }

            private void SendCallback(IAsyncResult ar)
            {
                bool more = false;
                byte[] data = null;
                try
                {
                    SocketConnection connection = ar.AsyncState as SocketConnection;
                    if (connection == null)
                    {
                        throw new ArgumentNullException("Asynchronous State Object");
                    }
                    Socket clientSocket = connection.Socket;

                    int count = 0;

                    try
                    {
                        count = clientSocket.EndSend(ar);
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (ArgumentException)
                    {
                        return;
                    }
                    catch (Exception x)
                    {
                        if (!connection.Catch(x))
                            throw;
                    }

                    lock (SendLock)
                    {
                        data = SendBuffer.Pull();
                    }

                    if (count != data.Length)
                    {
                        Energy.Core.Bug.Write("E8401", "Different size of buffer and send count");
                    }

                    DateTime now = DateTime.Now;
                    this.ActivityStamp = now;
                    this.SendStamp = now;

                    SendDone.Set();

                    if (!this.Active)
                        return;

                    lock (SendLock)
                    {
                        System.Diagnostics.Debug.WriteLine("Y " + SendBuffer.Count);
                        if (SendBuffer.Count > 0)
                            more = true;
                    }

                    if (more)
                    {
                        if (SendDone.WaitOne(0, true))
                        {
                            SendBegin();
                        }
                    }

                    //if (connection.OnSend != null)
                    //{
                    //    connection.OnSend(this, data);
                    //}
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Write(e);
                    bool ignore = false;
                    if (OnException != null)
                    {
                        ignore = OnException(this, e);
                    }
                    if (!ignore)
                    {
                        throw;
                    }
                }
            }

            public bool Send(string content)
            {
                byte[] data = ((System.Text.Encoding)this.Encoding).GetBytes(content);
                return Send(data);
            }

            #endregion

            #region Receive

            public bool Receive()
            {
                if (this.ReceiveDone.WaitOne(0, true))
                {
                    return ReceiveBegin();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Receive operation already waiting for callback");
                    return true;
                }
            }

            private bool ReceiveBegin()
            {
                ReceiveDone.Reset();

                if (!this.Active)
                    return false;
                if (!this.Connected)
                    return false;

                Socket clientSocket = this.Socket;

                // Begin receiving the data from the remote  
                clientSocket.BeginReceive(this.Buffer, 0, this.Buffer.Length
                    , (SocketFlags)0, new AsyncCallback(ReceiveCallback)
                    , this
                    );

                return true;
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                SocketClient connection = ar.AsyncState as SocketClient;

                if (connection == null)
                {
                    throw new ArgumentNullException("Asynchronous State Object");
                }

                bool flush = false;
                bool more = false;

                int length = 0;

                try
                {
                    Socket clientSocket = connection.Socket;

                    try
                    {
                        // Read data from the client socket.   
                        length = clientSocket.EndReceive(ar);
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (ArgumentException)
                    {
                        return;
                    }
                    catch (Exception x)
                    {
                        if (!connection.Catch(x))
                            throw;
                    }

                    DateTime now = DateTime.Now;
                    connection.ActivityStamp = now;
                    connection.ReceiveStamp = now;

                    if (length == 0)
                    {
                        // no data to receive
                        more = false;
                        flush = true;
                    }
                    else if (length > 0)
                    {
                        more = true;
                        connection.Stream.Write(connection.Buffer, 0, length);
                        if (length < connection.Capacity)
                        {
                            flush = true;
                        }
                        else
                        {
                            if (connection.Socket.Available > 0)
                            {
                                flush = false;
                            }
                        }
                    }

                    byte[] data = null;

                    if (flush)
                    {
                        if (connection.Stream.Length > 0)
                        {
                            data = connection.Stream.ToArray();
                            connection.Stream.SetLength(0);
                            connection.ReceiveBuffer.Add(data);
                        }
                    }

                    ReceiveDone.Set();

                    if (!this.Active)
                        return;

                    // fire event

                    if (data != null)
                    {
                        if (this.OnReceive != null)
                        {
                            this.OnReceive(this, data);
                        }
                    }

                    // start receiving

                    if (more || connection.AlwaysReceive)
                    {
                        connection.Receive();
                    }
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Write(e);
                    bool ignore = false;
                    if (OnException != null)
                    {
                        ignore = OnException(this, e);
                    }
                    if (!ignore)
                    {
                        throw;
                    }
                }
            }

            #endregion

            #region Close

            public override void Close()
            {
                base.Close();

                if (this.OnClose != null)
                {
                    this.OnClose(this);
                }
            }

            #endregion

            #region Clear

            public override void Clear()
            {
                base.Clear();

                this.ReceiveDone.Set();
                this.SendDone.Set();
                this.ConnectDone.Set();

                this.ConnectStamp = DateTime.MinValue;
                this.ReceiveStamp = DateTime.MinValue;
                this.SendStamp = DateTime.MinValue;
            }

            #endregion
        }

        #endregion

        #region SocketServer

        public class SocketServer : SocketConnection, Energy.Interface.ISocketServer
        {
            #region Property

            private int _Backlog = 100;
            [DefaultValue(100)]
            public int Backlog { get { return _Backlog; } set { _Backlog = value; } }

            private bool _AlwaysReceive = false;
            /// <summary>
            /// Start receiving automatically after connection is established
            /// and keep receiving until connection is deactivated.
            /// </summary>
            public bool AlwaysReceive { get { return _AlwaysReceive; } set { _AlwaysReceive = value; } }

            #endregion

            #region Stamp

            public DateTime ListenStamp { get; set; }

            public DateTime AcceptStamp { get; set; }

            #endregion

            #region Event

            public event Energy.Base.Network.ListenDelegate OnListen;

            public event Energy.Base.Network.AcceptDelegate OnAccept;

            public event Energy.Base.Network.CloseDelegate OnClose;

            public event Energy.Base.Network.ReceiveDelegate OnReceive;

            public event Energy.Base.Network.SendDelegate OnSend;

            public event Energy.Base.Network.ExceptionDelegate OnException;

            #endregion

            #region Synchronisation

            public ManualResetEvent ListenDone = new ManualResetEvent(true);

            public ManualResetEvent AcceptDone = new ManualResetEvent(true);

            #endregion

            #region Constructor

            public SocketServer()
                : base()
            { }

            #endregion

            #region Mirror

            public virtual void Mirror(SocketClient clone)
            {
                if (this.OnReceive != null)
                {
                    clone.OnReceive += new Energy.Base.Network.ReceiveDelegate((object o, byte[] b) =>
                    {
                        if (this.OnReceive != null)
                        {
                            this.OnReceive(clone, b);
                        }
                    });
                }
                if (this.OnSend != null)
                {
                    clone.OnSend += new Energy.Base.Network.SendDelegate((object o, byte[] b) =>
                    {
                        if (this.OnSend != null)
                        {
                            this.OnSend(clone, b);
                        }
                    });
                }
            }

            #endregion

            #region Listen

            public bool Listen()
            {
                try
                {
                    Clear();

                    AcceptDone.Reset();

                    Socket socket = this.CreateSocket();

                    IPEndPoint endPoint = this.GetEndPoint();

                    try
                    {
                        socket.Bind(endPoint);
                        socket.Listen(Backlog);
                        this.Socket = socket;
                        this.Active = true;
                    }
                    catch (System.Security.SecurityException)
                    {
                        throw;
                    }
                    catch (ObjectDisposedException)
                    {
                        throw;
                    }
                    catch (SocketException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    this.Socket = socket;

                    if (this.OnListen != null)
                    {
                        this.OnListen(this);
                    }

                    if (this.Active)
                    {
                        socket.BeginAccept(new AsyncCallback(AcceptCallback), this);
                    }
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Catch(e);
                    return false;
                }
                return true;
            }

            #endregion

            #region Accept

            private void AcceptCallback(IAsyncResult ar)
            {
                SocketServer serverConnection = ar.AsyncState as SocketServer;

                try
                {
                    Socket serverSocket = serverConnection.Socket;

                    Socket remoteSocket = null;

                    try
                    {
                        remoteSocket = serverSocket.EndAccept(ar);
                    }
                    catch (Exception exception)
                    {
                        if (serverConnection.OnException != null)
                        {
                            if (!serverConnection.OnException(serverConnection, exception))
                            {
                                return;
                            }
                        }
                        if (!serverConnection.Active)
                        {
                            return;
                        }
                        if (!this.Catch(exception))
                        {
                            return;
                        }
                    }
                    finally
                    {
                        AcceptDone.Set();
                    }

                    this.ActivityStamp = DateTime.Now;

                    if (remoteSocket != null)
                    {

                        SocketClient remoteConnection = new SocketClient();
                        IPEndPoint remoteEndPoint = remoteSocket.RemoteEndPoint as IPEndPoint;

                        remoteConnection.Host = remoteEndPoint.Address.ToString();
                        remoteConnection.Port = remoteEndPoint.Port;
                        remoteConnection.Family = remoteEndPoint.AddressFamily;
                        remoteConnection.Protocol = serverConnection.Protocol;

                        this.Mirror(remoteConnection);

                        remoteConnection.Socket = remoteSocket;
                        remoteConnection.Active = true;
                        remoteConnection.Connected = true;

                        if (this.OnAccept != null)
                        {
                            this.OnAccept(remoteConnection);
                        }

                        if (this.AlwaysReceive)
                        {
                            remoteConnection.Receive();
                        }
                    }

                    if (this.Active)
                    {
                        this.Socket.BeginAccept(new AsyncCallback(AcceptCallback), this);
                    }
                }
                catch
                {
                }
            }

            #endregion

            #region Close

            public override void Close()
            {
                base.Close();

                if (this.OnClose != null)
                {
                    this.OnClose(this);
                }
            }

            #endregion

            #region Clear

            public override void Clear()
            {
                base.Clear();

                this.AcceptDone.Set();
                this.ListenDone.Set();
            }

            #endregion
        }

        #endregion

        #region SocketConnection

        public class SocketConnection : Energy.Interface.ISocketConnection
        {
            #region Property

            private string _Host = "";
            [DefaultValue(null)]
            public string Host { get { return _Host; } set { _Host = value; } }

            private int _Port = 0;
            [DefaultValue(0)]
            public int Port { get { return _Port; } set { _Port = value; } }

            private AddressFamily _Family = AddressFamily.InterNetwork;
            [DefaultValue(default(AddressFamily))]
            public AddressFamily Family { get { return _Family; } set { _Family = value; } }

            private ProtocolType _Protocol = ProtocolType.Tcp;
            [DefaultValue(default(ProtocolType))]
            public ProtocolType Protocol { get { return _Protocol; } set { _Protocol = value; } }

            private int _Timeout = 3000;
            /// <summary>
            /// Timeout in milliseconds
            /// </summary>
            [DefaultValue(0)]
            public int Timeout { get { return _Timeout; } set { _Timeout = value; } }

            private int _Capacity = 8192;
            /// <summary>
            /// Buffer capacity
            /// </summary>
            public int Capacity { get { return _Capacity; } set { _Capacity = value; } }

            private int _Repeat = 0;
            /// <summary>
            /// Repeat operation if possible, 0 means no repeat
            /// </summary>
            public int Repeat { get { return _Repeat; } set { _Repeat = value; } }

            private Encoding _Encoding;
            /// <summary>
            /// Text encoding for strings. UTF-8 will be used if not set.
            /// </summary>
            public Encoding Encoding { get { return GetEncoding(); } set { _Encoding = value; } }

            /// <summary>
            /// Retry connections
            /// </summary>
            public int Retry { get; set; }

            #endregion

            #region State

            private bool _Active;
            /// <summary>
            /// Is connection active? Set to 0 if should be closed immediately
            /// </summary>
            public bool Active { get { return _Active; } set { _Active = value; } }

            private bool _Connected;
            /// <summary>
            /// Is connection active? Set to 0 if should be closed immediately
            /// </summary>
            public bool Connected { get { return _Connected; } set { _Connected = value; } }

            private Socket _Socket;
            /// <summary>
            /// Working socket
            /// </summary>
            public Socket Socket { get { return _Socket; } set { _Socket = value; } }

            #endregion

            #region Stamp

            public DateTime ActivityStamp { get; set; }

            #endregion

            //#region Stamp

            //public DateTime ActivityStamp { get; private set; }

            //public DateTime ConnectStamp { get; private set; }

            //public DateTime SendStamp { get; private set; }

            //public DateTime ReceiveStamp { get; private set; }

            //#endregion

            #region Private

            private byte[] _Buffer;

            public byte[] Buffer { get { return GetBuffer(); } set { _Buffer = value; } }

            private MemoryStream _Stream;

            public MemoryStream Stream { get { return GetStream(); } private set { _Stream = value; } }

            private Energy.Base.Collection.Circular<byte[]> _ReceiveBuffer;

            public Energy.Base.Collection.Circular<byte[]> ReceiveBuffer { get { return GetReceiveBuffer(); } private set { _ReceiveBuffer = value; } }

            private Energy.Base.Collection.Circular<byte[]> _SendBuffer;

            public Energy.Base.Collection.Circular<byte[]> SendBuffer { get { return GetSendBuffer(); } private set { _SendBuffer = value; } }

            #endregion

            //#region Event

            //public event Energy.Base.Network.CloseDelegate OnClose;

            //public event Energy.Base.Network.ReceiveDelegate OnReceive;

            //public event Energy.Base.Network.SendDelegate OnSend;

            //public event Energy.Base.Network.ExceptionDelegate OnException;

            //#endregion

            #region Constructor

            public SocketConnection()
            {
                this.Encoding = System.Text.Encoding.UTF8;
            }

            #endregion

            #region Get

            private Encoding GetEncoding()
            {
                if (_Encoding == null)
                {
                    _Encoding = Encoding.UTF8;
                }
                return _Encoding;
            }

            private byte[] GetBuffer()
            {
                if (_Buffer == null)
                {
                    _Buffer = new byte[Capacity];
                }
                return _Buffer;
            }

            private MemoryStream GetStream()
            {
                if (_Stream == null)
                {
                    _Stream = new MemoryStream(Capacity);
                }
                return _Stream;
            }

            private Energy.Base.Collection.Circular<byte[]> GetReceiveBuffer()
            {
                if (_ReceiveBuffer == null)
                {
                    _ReceiveBuffer = new Energy.Base.Collection.Circular<byte[]>();
                }
                return _ReceiveBuffer;
            }

            private Energy.Base.Collection.Circular<byte[]> GetSendBuffer()
            {
                if (_SendBuffer == null)
                {
                    _SendBuffer = new Energy.Base.Collection.Circular<byte[]>();
                }
                return _SendBuffer;
            }

            public Socket CreateSocket()
            {
                string host = this.GetHost();
                AddressFamily family = this.GetAddressFamily(this.Family, host);
                ProtocolType protocol = this.Protocol;
                SocketType socketType = Energy.Core.Network.GetSocketType(protocol, family);
                Socket socket = new Socket(family, socketType, protocol);
                return socket;
            }

            public AddressFamily GetAddressFamily(AddressFamily family, string host)
            {
                if (family == default(AddressFamily))
                {
                    family = Energy.Core.Network.GetAddressFamily(host);
                }
                return family;
            }

            public string GetHost()
            {
                string host;
                host = Energy.Core.Network.GetHostAddress(this.Host, this.Family);
                if (string.IsNullOrEmpty(host))
                {
                    host = Energy.Core.Network.GetHostAddress(Dns.GetHostName(), this.Family);
                }
                return host;
            }

            public IPEndPoint GetEndPoint()
            {
                string host = GetHost();
                AddressFamily family = this.GetAddressFamily(this.Family, host);

                int port = this.Port;
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), port);

                return endPoint;
            }

            #endregion

            #region Catch

            public bool Catch(Exception exception)
            {
                if (exception == null)
                    return false;

                if (exception is SocketException)
                {
                    //switch (((SocketException)exception).SocketErrorCode)
                    //{
                    //    case SocketError.ConnectionRefused:
                    //        return true;

                    //    case SocketError.ConnectionReset:
                    //        Close();
                    //        return true;

                    //    case SocketError.ConnectionAborted:
                    //        Close();
                    //        return true;
                    //}
                    SocketException socketException = (SocketException)exception;
                    switch (socketException.ErrorCode)
                    {
                        case 10061: // ConnectionRefused
                            return true;

                        case 10054: // ConnectionReset
                            Close();
                            return true;

                        case 10053: // ConnectionAborted
                            Close();
                            return true;
                    }
                }

                return false;
            }

            #endregion

            #region Close

            public virtual void Close()
            {
                this.Active = false;

                Energy.Core.Network.Shutdown(this.Socket);

                this.Connected = false;

                this.Socket = null;
            }

            #endregion

            #region Clear

            public virtual void Clear()
            {
                this.Active = false;
                this.Connected = false;

                this.Socket = null;

                this.ActivityStamp = DateTime.MinValue;
            }

            #endregion

            //#region Clone

            //public virtual object Clone()
            //{
            //    SocketConnection clone = new SocketConnection();

            //    clone.Host = this.Host;
            //    clone.Protocol = this.Protocol;
            //    clone.Port = this.Port;
            //    clone.Family = this.Family;
            //    clone.Timeout = this.Timeout;

            //    this.Mirror(clone);

            //    return clone;
            //}

            //#endregion

            //#region Wait

            ///// <summary>
            ///// Blocks current thread until receiving is done.
            ///// Specify number of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely.
            ///// </summary>
            ///// <param name="millisecondsTimeout">Nmber of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely</param>
            ///// <returns></returns>
            //public bool WaitUntilReceiveDone(int millisecondsTimeout)
            //{
            //    return this.ReceiveDone.WaitOne(millisecondsTimeout);
            //}

            ///// <summary>
            ///// Blocks current thread until receiving is done.
            ///// </summary>
            ///// <returns></returns>
            //public bool WaitUntilReceiveDone()
            //{
            //    return this.ReceiveDone.WaitOne();
            //}

            ///// <summary>
            ///// Blocks current thread until sending is done.
            ///// Specify number of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely.
            ///// </summary>
            ///// <param name="millisecondsTimeout">Nmber of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely</param>
            ///// <returns></returns>
            //public bool WaitUntilSendDone(int millisecondsTimeout)
            //{
            //    return this.SendDone.WaitOne(millisecondsTimeout);
            //}

            ///// <summary>
            ///// Blocks current thread until sending is done.
            ///// </summary>
            ///// <returns></returns>
            //public bool WaitUntilSendDone()
            //{
            //    return this.SendDone.WaitOne();
            //}

            //#endregion

            #region ToString

            public override string ToString()
            {
                string result = this.Host ?? "";
                if (this.Port > 0)
                    result += ":" + this.Port;
                return result;
            }

            #endregion
        }

        #endregion

        #region Ping

#if !NETCF

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

#endif

        #endregion
    }
}
