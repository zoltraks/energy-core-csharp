using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace Energy.Core
{
    public class Network
    {
        #region Utility

        public static string GetHostAddress(string host)
        {
            if (host == null)
                return null;

            if (host == "")
                return "";

            if (host == "localhost" || host == ".")
            {
                return IPAddress.Parse("127.0.0.1").ToString();
            }

            IPAddress ipAddress;
            if (!IPAddress.TryParse(host, out ipAddress))
            {
                try
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
                    ipAddress = ipHostInfo.AddressList[0];
                }
                catch (SocketException socketException)
                {
                    Energy.Core.Bug.Catch(socketException);
                    return null;
                }
            }
            return ipAddress.ToString();
        }

        public static AddressFamily GetAddressFamily(string address)
        {
            if (address == "localhost" || address == "127.0.0.1")
                return AddressFamily.InterNetwork;
            if (address == "::1" || address == "[::1]" || address == "::" || address == "[::]")
                return AddressFamily.InterNetworkV6;
            IPAddress ipAddress = null;
            if (IPAddress.TryParse(address, out ipAddress))
            {
                return ipAddress.AddressFamily;
            }
            else
            {
                try
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(address);
                    ipAddress = ipHostInfo.AddressList[0];
                    return ipAddress.AddressFamily;
                }
                catch (SocketException socketException)
                {
                    Energy.Core.Bug.Catch(socketException);
                    return AddressFamily.Unknown;
                }
            }
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
                return;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
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
            catch (Exception exceptionAny)
            {
                Energy.Core.Bug.Catch(exceptionAny);
            }
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
                return false;
            if (!socket.Connected)
                return false;
            bool result = !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            return result;
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

            Energy.Core.Bug.Write(string.Format("Socket configuration: {0}", string.Join(", ", GetSocketConfigurationStringArray(socket))));

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

        public class SocketClient : SocketConnection
        {
        }

        #endregion

        #region SocketServer

        public class SocketServer : SocketConnection
        {
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

            private bool _AlwaysReceive = false;
            /// <summary>
            /// Start receiving automatically after connection is established
            /// and keep receiving until connection is deactivated.
            /// </summary>
            public bool AlwaysReceive { get { return _AlwaysReceive; } set { _AlwaysReceive = value; } }

            private Encoding _Encoding;
            /// <summary>
            /// Text encoding for strings. UTF-8 will be used if not set.
            /// </summary>
            public Encoding Encoding { get { return GetEncoding(); } set { _Encoding = value; } }

            public int Retry { get; set; }

            private int _Backlog = 100;
            [DefaultValue(100)]
            public int Backlog { get { return _Backlog; } set { _Backlog = value; } }

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

            public DateTime ActivityStamp { get; private set; }

            public DateTime ConnectStamp { get; private set; }

            public DateTime SendStamp { get; private set; }

            public DateTime ReceiveStamp { get; private set; }

            #endregion

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

            #region Event

            public event Energy.Base.Network.ConnectDelegate OnConnect;

            public event Energy.Base.Network.CloseDelegate OnClose;

            public event Energy.Base.Network.ListenDelegate OnListen;

            public event Energy.Base.Network.AcceptDelegate OnAccept;

            public event Energy.Base.Network.ReceiveDelegate OnReceive;

            public event Energy.Base.Network.SendDelegate OnSend;

            public event Energy.Base.Network.ExceptionDelegate OnException;

            #endregion

            #region Constructor

            public SocketConnection()
            {
                this.Encoding = System.Text.Encoding.UTF8;
            }

            #endregion

            #region Synchronisation

            public ManualResetEvent ConnectResetEvent = new ManualResetEvent(true);

            public ManualResetEvent AcceptDone = new ManualResetEvent(true);

            public ManualResetEvent ConnectDone = new ManualResetEvent(true);

            public ManualResetEvent ReceiveDone = new ManualResetEvent(true);

            public ManualResetEvent SendDone = new ManualResetEvent(true);

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

            #endregion

            #region Listen

            public bool Listen()
            {
                Socket socket = CreateSocket();

                try
                {
                    IPEndPoint endPoint = this.GetEndPoint();

                    socket.Bind(endPoint);
                    socket.Listen(Backlog);

                    Clear();

                    AcceptDone.Reset();

                    this.Socket = socket;

                    socket.BeginAccept(new AsyncCallback(AcceptCallback), this);

                    if (this.OnListen != null)
                    {
                        this.OnListen(this);
                    }
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Catch(e);
                    return false;
                }
                return true;
            }

            private Socket CreateSocket()
            {
                string host = this.GetHost();
                AddressFamily family = this.GetAddressFamily(this.Family, host);
                ProtocolType protocol = this.Protocol;
                SocketType socketType = Energy.Core.Network.GetSocketType(protocol, family);
                Socket socket = new Socket(family, socketType, protocol);
                return socket;
            }

            private AddressFamily GetAddressFamily(AddressFamily family, string host)
            {
                if (family == default(AddressFamily))
                {
                    family = Energy.Core.Network.GetAddressFamily(host);
                }
                return family;
            }

            private string GetHost()
            {
                string host;
                host = Energy.Core.Network.GetHostAddress(this.Host);
                if (string.IsNullOrEmpty(host))
                {
                    host = Energy.Core.Network.GetHostAddress(Dns.GetHostName());
                }
                return host;
            }

            private IPEndPoint GetEndPoint()
            {
                string host = GetHost();
                AddressFamily family = this.GetAddressFamily(this.Family, host);

                int port = this.Port;
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), port);

                return endPoint;
            }

            #endregion

            #region Accept

            private void AcceptCallback(IAsyncResult ar)
            {
                SocketConnection socketConnection = ar.AsyncState as SocketConnection;

                //socketConnection = (SocketConnection)socketConnection.Clone();

                try
                {
                    Socket serverSocket = this.Socket;

                    // Get the socket that handles the client request.
                    Socket clientSocket = serverSocket.EndAccept(ar);

                    AcceptDone.Set();

                    SocketConnection clientConnection = new SocketConnection();
                    IPEndPoint remoteEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;

                    clientConnection.Host = remoteEndPoint.Address.ToString();
                    clientConnection.Port = remoteEndPoint.Port;
                    clientConnection.Family = remoteEndPoint.AddressFamily;
                    clientConnection.Protocol = socketConnection.Protocol;

                    this.Mirror(clientConnection);

                    clientConnection.Socket = clientSocket;

                    if (this.OnAccept != null)
                    {
                        this.OnAccept(this);
                    }

                    if (this.AlwaysReceive)
                    {
                        clientConnection.Receive();
                    }
                }
                catch
                {
                }
            }

            #endregion

            #region Connect

            public bool Connect()
            {
                if (Connected)
                {
                    Close();
                }

                Clear();

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
                            if (!this.ConnectDone.WaitOne(timeout))
                            {
                                this.Active = false;
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
                SocketConnection connection = ar.AsyncState as SocketConnection;

                if (connection == null)
                {
                    throw new ArgumentNullException("Asynchronous State Object");
                }

                try
                {
                    Socket remoteSocket = connection.Socket;

                    try
                    {
                        remoteSocket.EndConnect(ar);
                    }
                    catch (Exception x)
                    {
                        if (!connection.Catch(x))
                            throw;
                    }

                    DateTime now = DateTime.Now;
                    connection.ActivityStamp = now;
                    connection.ConnectStamp = now;

                    connection.Connected = true;

                    connection.ConnectDone.Set();

                    if (connection.OnConnect != null)
                    {
                        connection.OnConnect(this);
                    }

                    if (connection.AlwaysReceive)
                    {
                        connection.Receive();
                    }
                }
                catch (SocketException exceptionSocket)
                {
                    switch (exceptionSocket.SocketErrorCode)
                    {
                        default:
                            connection.Active = false;
                            break;

                        case SocketError.TimedOut:
                            connection.Active = false;
                            break;
                    }

                    if (this.OnException != null)
                    {
                        this.OnException(this, (Exception)exceptionSocket);
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Write(exception);

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

            public bool Send(byte[] data)
            {
                if (!this.Active)
                    return false;
                if (!this.Connected)
                    return false;

                lock (SendLock)
                {
                    Energy.Core.Bug.Write("X");
                    SendBuffer.Push(data);
                }

                if (SendDone.WaitOne(0))
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

                return true;
            }

            private void SendCallback(IAsyncResult ar)
            {
                bool signal = true;
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
                    connection.ActivityStamp = now;
                    connection.SendStamp = now;

                    lock (SendLock)
                    {
                        if (SendBuffer.Count > 0)
                            more = true;
                    }

                    if (more)
                    {
                        SendBegin();
                    }
                    else
                    {
                        SendDone.Set();
                    }

                    if (connection.OnSend != null)
                    {
                        connection.OnSend(this, data);
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

            public bool Send(string content)
            {
                byte[] data = ((System.Text.Encoding)this.Encoding).GetBytes(content);
                return Send(data);
            }

            #endregion

            #region Receive

            public bool Receive()
            {
                if (this.ReceiveDone.WaitOne(0))
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
                SocketConnection connection = ar.AsyncState as SocketConnection;

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

            #region Catch

            public bool Catch(Exception exception)
            {
                if (exception == null)
                    return false;

                if (exception is SocketException)
                {
                    switch (((SocketException)exception).SocketErrorCode)
                    {
                        case SocketError.ConnectionRefused:
                            break;

                        case SocketError.ConnectionReset:
                            Close();
                            break;
                    }
                }

                return false;
            }

            #endregion

            #region Close

            public void Close()
            {
                this.Active = false;
                Energy.Core.Network.Shutdown(this.Socket);
                this.Connected = false;

                if (this.OnClose != null)
                {
                    this.OnClose(this);
                }
            }

            #endregion

            #region Clear

            public void Clear()
            {
                this.Active = false;
                this.Connected = false;

                this.ReceiveDone.Set();
                this.SendDone.Set();
                this.ConnectDone.Set();
                this.AcceptDone.Set();

                this.ActivityStamp = DateTime.MinValue;
                this.ConnectStamp = DateTime.MinValue;
                this.ReceiveStamp = DateTime.MinValue;
                this.SendStamp = DateTime.MinValue;
            }

            #endregion

            #region Clone

            public object Clone()
            {
                SocketConnection clone = new SocketConnection();

                clone.Host = this.Host;
                clone.Protocol = this.Protocol;
                clone.Port = this.Port;
                clone.Family = this.Family;
                clone.Backlog = this.Backlog;
                clone.Timeout = this.Timeout;

                this.Mirror(clone);

                return clone;
            }

            #endregion

            #region Mirror

            private void Mirror(SocketConnection clone)
            {
                if (this.OnConnect != null)
                {
                    clone.OnConnect += new Energy.Base.Network.ConnectDelegate((object o) =>
                    {
                        if (this.OnConnect != null)
                        {
                            this.OnConnect(clone);
                        }
                    });
                }
                if (this.OnAccept != null)
                {
                    clone.OnAccept += new Energy.Base.Network.AcceptDelegate((object o) =>
                    {
                        if (this.OnAccept != null)
                        {
                            this.OnAccept(clone);
                        }
                    });
                }
                if (this.OnListen != null)
                {
                    clone.OnListen += new Energy.Base.Network.ListenDelegate((object o) =>
                    {
                        if (this.OnListen != null)
                        {
                            this.OnListen(clone);
                        }
                    });
                }
                if (this.OnClose != null)
                {
                    clone.OnClose += new Energy.Base.Network.CloseDelegate((object o) =>
                    {
                        if (this.OnClose != null)
                        {
                            this.OnClose(clone);
                        }
                    });
                }
                if (this.OnSend != null)
                {
                    clone.OnSend += new Energy.Base.Network.SendDelegate((object o, byte[] data) =>
                    {
                        if (this.OnSend != null)
                        {
                            this.OnSend(clone, data);
                        }
                    });
                }
                if (this.OnReceive != null)
                {
                    clone.OnReceive += new Energy.Base.Network.ReceiveDelegate((object o, byte[] data) =>
                    {
                        if (this.OnReceive != null)
                        {
                            this.OnReceive(clone, data);
                        }
                    });
                }
            }

            #endregion

            #region Wait

            /// <summary>
            /// Blocks current thread until receiving is done.
            /// Specify number of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely.
            /// </summary>
            /// <param name="millisecondsTimeout">Nmber of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely</param>
            /// <returns></returns>
            public bool WaitUntilReceiveDone(int millisecondsTimeout)
            {
                return this.ReceiveDone.WaitOne(millisecondsTimeout);
            }

            /// <summary>
            /// Blocks current thread until receiving is done.
            /// </summary>
            /// <returns></returns>
            public bool WaitUntilReceiveDone()
            {
                return this.ReceiveDone.WaitOne();
            }

            /// <summary>
            /// Blocks current thread until sending is done.
            /// Specify number of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely.
            /// </summary>
            /// <param name="millisecondsTimeout">Nmber of milliseconds to wait, or a System.Threading.Timeout.Infinite (-1) to wait indefinitely</param>
            /// <returns></returns>
            public bool WaitUntilSendDone(int millisecondsTimeout)
            {
                return this.SendDone.WaitOne(millisecondsTimeout);
            }

            /// <summary>
            /// Blocks current thread until sending is done.
            /// </summary>
            /// <returns></returns>
            public bool WaitUntilSendDone()
            {
                return this.SendDone.WaitOne();
            }

            #endregion

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
