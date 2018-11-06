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
            if (host == "localhost")
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
        /// Check if socket is connected
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

        #region SocketConnection

        public class SocketConnection : Energy.Interface.ISocketConnection
        {
            #region StateObject

            public class StateObject
            {
                private volatile Socket _Socket;
                /// <summary>
                /// Working socket
                /// </summary>
                public Socket Socket { get { return _Socket; } set { _Socket = value; } }

                private volatile int _Capacity = 8192;

                /// <summary>
                /// Buffer capacity
                /// </summary>
                public int Capacity { get { return _Capacity; } set { _Capacity = value; } }

                private volatile byte[] _Buffer;

                public byte[] Buffer { get { return _Buffer; } set { _Buffer = value; } }

                private volatile MemoryStream _Stream;

                public MemoryStream Stream { get { return _Stream; } set { _Stream = value; } }

                private volatile bool _Active;

                /// <summary>
                /// Is connection active? Set to 0 if should be closed immediately
                /// </summary>
                public bool Active { get { return _Active; } set { _Active = value; } }

                private volatile int _Repeat = 0;

                /// <summary>
                /// Repeat operation if possible, 0 means no repeat
                /// </summary>
                public int Repeat { get { return _Repeat; } set { _Repeat = value; } }

                public ManualResetEvent AcceptDone { get; set; }

                public ManualResetEvent ConnectDone { get; set; }

                public ManualResetEvent ReceiveDone { get; set; }

                public ManualResetEvent SendDone { get; set; }

                public List<byte[]> ReceiveBuffer { get; private set; }

                public List<byte[]> SendBuffer { get; private set; }

                public int SendBufferPosition { get; set; }

                #region Constructor

                public StateObject()
                {
                    Buffer = new byte[Capacity];
                    Stream = new MemoryStream(Capacity);

                    ReceiveBuffer = new List<byte[]>();
                    SendBuffer = new List<byte[]>();

                    AcceptDone = new ManualResetEvent(false);
                }

                public StateObject(int repeat) : this()
                {
                    Repeat = repeat;
                }

                #endregion
            }

            #endregion

            #region Lock

            private readonly object _PropertyLock = new object();

            #endregion

            #region Property

            private bool _Active = false;
            [DefaultValue(false)]
            public bool Active { get { lock (_PropertyLock) return _Active; } set { lock (_PropertyLock) _Active = value; } }

            private string _Host = "";
            [DefaultValue(null)]
            public string Host { get { lock (_PropertyLock) return _Host; } set { lock (_PropertyLock) _Host = value; } }

            private int _Port = 0;
            [DefaultValue(0)]
            public int Port { get { lock (_PropertyLock) return _Port; } set { lock (_PropertyLock) _Port = value; } }

            private AddressFamily _Family = AddressFamily.InterNetwork;
            [DefaultValue(default(AddressFamily))]
            public AddressFamily Family { get { lock (_PropertyLock) return _Family; } set { lock (_PropertyLock) _Family = value; } }

            private ProtocolType _Protocol = ProtocolType.Tcp;
            [DefaultValue(default(ProtocolType))]
            public ProtocolType Protocol { get { lock (_PropertyLock) return _Protocol; } set { lock (_PropertyLock) _Protocol = value; } }

            private int _Backlog = 100;
            [DefaultValue(100)]
            public int Backlog { get { lock (_PropertyLock) return _Backlog; } set { lock (_PropertyLock) _Backlog = value; } }

            private int _Timeout = 3000;
            /// <summary>
            /// Timeout in milliseconds
            /// </summary>
            [DefaultValue(0)]
            public int Timeout { get { lock (_PropertyLock) return _Timeout; } set { lock (_PropertyLock) _Timeout = value; } }

            #endregion

            #region Event

            public event Energy.Base.Network.ConnectDelegate OnConnect;

            public event Energy.Base.Network.CloseDelegate OnClose;

            public event Energy.Base.Network.ListenDelegate OnListen;

            public event Energy.Base.Network.AcceptDelegate OnAccept;

            public event Energy.Base.Network.ReceiveDelegate OnReceive;

            public event Energy.Base.Network.SendDelegate OnSend;

            public event Energy.Base.Network.ErrorDelegate OnError;

            #endregion

            #region Synchronisation

            public ManualResetEvent ConnectResetEvent = new ManualResetEvent(false);

            #endregion

            #region Other

            private StateObject State { get; set; }

            #endregion

            #region Listen

            public bool Listen()
            {
                string host = GetHost();
                AddressFamily family = this.GetAddressFamily(this.Family, host);
                ProtocolType protocol = this.Protocol;
                SocketType socketType = Energy.Core.Network.GetSocketType(protocol, family);
                int port = this.Port;

                Socket socket = new Socket(family, socketType, protocol);

                try
                {
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(host), port);

                    socket.Bind(localEndPoint);
                    socket.Listen(Backlog);

                    StateObject stateObject = new StateObject();

                    stateObject.AcceptDone.Reset();

                    this.State = stateObject;

                    this.State = new StateObject();
                    this.State.Socket = socket;

                    socket.BeginAccept(new AsyncCallback(AcceptCallback), this);

                    if (OnListen != null)
                    {
                        Energy.Core.Worker.Fire(() => 
                        {
                            if (this.OnListen != null)
                                this.OnListen(this);
                        });
                    }
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Catch(e);
                    return false;
                }
                return true;
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

            #endregion

            #region Accept

            private void AcceptCallback(IAsyncResult ar)
            {
                SocketConnection socketConnection = ar.AsyncState as SocketConnection;
                StateObject stateObject = socketConnection.State;

                //socketConnection = (SocketConnection)socketConnection.Clone();

                try
                {
                    Socket serverSocket = stateObject.Socket;

                    // Get the socket that handles the client request.
                    Socket clientSocket = serverSocket.EndAccept(ar);

                    SocketConnection clientConnection = new SocketConnection();
                    IPEndPoint remoteEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;

                    clientConnection.Host = remoteEndPoint.Address.ToString();
                    clientConnection.Port = remoteEndPoint.Port;
                    clientConnection.Family = remoteEndPoint.AddressFamily;
                    clientConnection.Protocol = socketConnection.Protocol;

                    this.Mirror(clientConnection);

                    clientConnection.State = new StateObject();
                    clientConnection.State.Socket = clientSocket;

                    if (OnAccept != null)
                    {
                        Energy.Core.Worker.Fire(() =>
                        {
                            if (this.OnAccept != null)
                                this.OnAccept(clientConnection);
                        });
                    }

                    stateObject.SendDone.Set();

                    clientConnection.Receive();
                }
                catch
                {
                }
            }

            #endregion

            #region Connect

            public bool Connect()
            {
                string host = GetHost();
                AddressFamily family = this.GetAddressFamily(this.Family, host);
                ProtocolType protocol = this.Protocol;
                SocketType socketType = Energy.Core.Network.GetSocketType(protocol, family);

                int port = this.Port;
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port);

                Socket connectSocket = new Socket(family, socketType, protocol);

                StateObject stateObject = new StateObject();
                stateObject.Socket = connectSocket;
                stateObject.Active = true;
                stateObject.ConnectDone = new ManualResetEvent(false);

                this.State = stateObject;

                connectSocket.BeginConnect(remoteEndPoint, new AsyncCallback(ConnectCallback), stateObject);

                int timeout = this.Timeout;
                bool timeoutEnable = timeout > 0;
                if (timeoutEnable)
                {
                    Energy.Core.Worker.Fire(() =>
                    {
                        if (!stateObject.ConnectDone.WaitOne(timeout))
                        {
                            this.State.Active = false;
                        }
                    });
                }

                return true;
            }

            private void ConnectCallback(IAsyncResult ar)
            {
                StateObject stateObject = ar.AsyncState as StateObject;
                if (stateObject == null)
                {
                    throw new ArgumentNullException("Asynchronous State Object");
                }

                try
                {
                    // Retrieve the socket from the state object.  
                    Socket clientSocket = stateObject.Socket;

                    // Complete the connection.  
                    clientSocket.EndConnect(ar);

                    // Signal that the connection has been made.  
                    stateObject.ConnectDone.Set();

                    if (this.OnConnect != null)
                    {
                        Energy.Core.Worker.Fire(() => { this.OnConnect(this); });
                    }

                    if (stateObject.Active)
                    {
                        Receive(stateObject);
                    }
                    else
                    {
                        //Close(stateObject);
                    }
                }
                catch (SocketException exceptionSocket)
                {
                    switch (exceptionSocket.SocketErrorCode)
                    {
                        default:
                            stateObject.Active = false;
                            break;

                        case SocketError.TimedOut:
                            stateObject.Active = false;
                            break;
                    }

                    if (this.OnError != null)
                    {
                        Energy.Core.Worker.Fire(() => { this.OnError(this, (Exception)exceptionSocket); });
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Write(exception);

                    if (this.OnError != null)
                    {
                        Energy.Core.Worker.Fire(() => { this.OnError(this, exception); });
                    }

                    throw;
                }
            }

            public bool Send(byte[] data)
            {
                if (this.State == null)
                    return false;
                StateObject stateObject = this.State;
                if (!stateObject.Active)
                    return false;

                int count = data.Length;
                byte[] copy = new byte[count];
                Array.Copy(data, copy, count);
                stateObject.SendBuffer.Add(copy);

                if (!SendBegin())
                {
                    return false;
                }

                return true;
            }

            private bool SendBegin()
            {
                if (this.State == null)
                    return false;
                StateObject stateObject = this.State;
                if (!stateObject.Active)
                    return false;

                byte[] copy = stateObject.SendBuffer[stateObject.SendBufferPosition];

                stateObject.SendDone = new ManualResetEvent(false);

                Socket clientSocket = stateObject.Socket;

                clientSocket.BeginSend(copy, 0, copy.Length, (SocketFlags)0, SendCallback, stateObject);

                return true;

            }

            private void SendCallback(IAsyncResult ar)
            {
                StateObject stateObject = ar.AsyncState as StateObject;
                if (stateObject == null)
                {
                    throw new ArgumentNullException("Asynchronous State Object");
                }
                Socket clientSocket = stateObject.Socket;

                int count = clientSocket.EndSend(ar);

                stateObject.SendDone.Set();

                byte[] data = stateObject.SendBuffer[stateObject.SendBufferPosition];

                if (count != data.Length)
                {
                    Energy.Core.Bug.Write("E8401", "Different size of buffer and send count");
                }

                if (this.OnSend != null)
                {
                    Thread threadEvent = new Thread(() =>
                    {
                        try
                        {
                            this.OnSend(this, data);
                        }
                        catch (ThreadAbortException)
                        { }
                    })
                    { IsBackground = true };
                    threadEvent.Start();
                }

                if (++stateObject.SendBufferPosition < stateObject.SendBuffer.Count)
                {
                    SendBegin();
                }
            }

            private void Receive()
            {
                StateObject stateObject = this.State;
                if (stateObject == null)
                    return;
                Receive(stateObject);
            }

            private void Receive(StateObject stateObject)
            {
                if (false == stateObject.Active)
                    return;

                stateObject.ReceiveDone = new ManualResetEvent(false);

                Socket clientSocket = stateObject.Socket;

                // Begin receiving the data from the remote device.  
                clientSocket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length
                    , (SocketFlags)0, new AsyncCallback(ReceiveCallback)
                    , stateObject
                    );
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                StateObject stateObject = ar.AsyncState as StateObject;
                if (stateObject == null)
                {
                    throw new ArgumentNullException("Asynchronous State Object");
                }

                bool flush = false;
                bool more = false;

                try
                {
                    Socket clientSocket = stateObject.Socket;

                    // Read data from the client socket.   
                    int dataLength = clientSocket.EndReceive(ar);

                    // Signal that the connection has been made.  
                    stateObject.ReceiveDone.Set();

                    if (dataLength == 0)
                    {
                        // no data to receive
                        more = false;
                        flush = true;
                    }
                    else if (dataLength > 0)
                    {
                        more = true;
                        if (dataLength < stateObject.Capacity)
                            flush = true;
                        else
                            flush = false;
                        stateObject.Stream.Write(stateObject.Buffer, 0, dataLength);
                    }
                }
                catch (SocketException exceptionSocket)
                {
                    switch (exceptionSocket.SocketErrorCode)
                    {
                        case SocketError.ConnectionReset:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Write(e);
                    throw;
                }

                try
                {
                    if (flush)
                    {
                        if (stateObject.Stream.Length > 0)
                        {
                            byte[] receiveData = stateObject.Stream.ToArray();
                            stateObject.Stream.SetLength(0);
                            stateObject.ReceiveBuffer.Add(receiveData);

                            if (this.OnReceive != null)
                            {
                                Energy.Core.Worker.Fire(() => { this.OnReceive(this, receiveData); });
                            }
                        }
                    }

                    if (more)
                    {
                        Receive(stateObject);
                    }
                }
                catch (Exception e)
                {
                    Energy.Core.Bug.Write(e);
                    throw;
                }
            }

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

            public void Send(object p)
            {
                throw new NotImplementedException();
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
