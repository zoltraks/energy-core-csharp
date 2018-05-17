using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Energy.Core
{
    public class Network
    {
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
            list.Add(string.Format("SendBufferSize: {0}", socket.SendBufferSize));
            list.Add(string.Format("ReceiveTimeout: {0}", socket.ReceiveTimeout));
            list.Add(string.Format("SendTimeout: {0}", socket.SendTimeout));
            list.Add(string.Format("ExclusiveAddressUse: {0}", socket.ExclusiveAddressUse));
            list.Add(string.Format("Ttl: {0}", socket.Ttl));
            list.Add(string.Format("NoDelay: {0}", socket.NoDelay));
            list.Add(string.Format("LingerState: {0}, {1}", socket.LingerState.Enabled, socket.LingerState.LingerTime));
            list.Add(string.Format("IsBound: {0}", socket.IsBound));
            return list.ToArray();
        }

        /// <summary>
        /// Make configuration of System.Net.Sockets.Socket object.
        /// </summary>
        /// <param name="socket">System.Net.Sockets.Socket object</param>
        /// <param name="buffer">Buffer size in bytes (typically 8192)</param>
        /// <param name="timeout">Timeout in milliseconds (10000 for 10 seconds)</param>
        /// <param name="exclusive">Don't allow another socket to bind to this port (should be true on server socket)</param>
        /// <param name="ttl">TTL value indicates the maximum number of routers the packet can traverse before the router discards the packet</param>
        /// <param name="linger">
        /// Specifies behaviour when closing socket, possible values are:
        /// negative number to disable (attempts to send pending data until the default IP protocol time-out expires),
        /// zero (discards any pending data, for connection-oriented socket Winsock resets the connection), and
        /// positive number of seconds (attempts to send pending data until the specified time-out expires, 
        /// if the attempt fails, then Winsock resets the connection).
        /// </param>
        public static void ConfigureSocket(System.Net.Sockets.Socket socket, int buffer, int timeout, bool? exclusive, int? ttl, int? linger)
        {
            if (exclusive != null)
            {
                // Don't allow another socket to bind to this port
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

            Energy.Core.Bug.Write("Socket configured: {0}", string.Join(", ", GetSocketConfigurationStringArray(socket)));
        }

        public static void ConfigureTcpSocket(Socket socket, int bufferSize)
        {
            // Don't allow another socket to bind to this port.
            socket.ExclusiveAddressUse = true;

            // The socket will linger for 10 seconds after
            // Socket.Close is called.
            socket.LingerState = new LingerOption(true, 10);

            // Disable the Nagle Algorithm for this tcp socket.
            socket.NoDelay = true;

            // Set the receive buffer size to 8k
            socket.ReceiveBufferSize = bufferSize;

            // Set the timeout for synchronous receive methods to
            // 1 second (1000 milliseconds.)
            socket.ReceiveTimeout = 1000;

            // Set the send buffer size to 8k.
            socket.SendBufferSize = bufferSize;

            // Set the timeout for synchronous send methods
            // to 1 second (1000 milliseconds.)
            socket.SendTimeout = 1000;

            // Set the Time To Live (TTL) to 42 router hops.
            socket.Ttl = 42;

            System.Diagnostics.Debug.WriteLine("Tcp Socket configured:");
            System.Diagnostics.Debug.WriteLine(string.Format("  ExclusiveAddressUse {0}", socket.ExclusiveAddressUse));
            System.Diagnostics.Debug.WriteLine(string.Format("  LingerState {0}, {1}", socket.LingerState.Enabled, socket.LingerState.LingerTime));
            System.Diagnostics.Debug.WriteLine(string.Format("  NoDelay {0}", socket.NoDelay));
            System.Diagnostics.Debug.WriteLine(string.Format("  ReceiveBufferSize {0}", socket.ReceiveBufferSize));
            System.Diagnostics.Debug.WriteLine(string.Format("  ReceiveTimeout {0}", socket.ReceiveTimeout));
            System.Diagnostics.Debug.WriteLine(string.Format("  SendBufferSize {0}", socket.SendBufferSize));
            System.Diagnostics.Debug.WriteLine(string.Format("  SendTimeout {0}", socket.SendTimeout));
            System.Diagnostics.Debug.WriteLine(string.Format("  Ttl {0}", socket.Ttl));
            System.Diagnostics.Debug.WriteLine(string.Format("  IsBound {0}", socket.IsBound));
        }

        public static void ConfigureTcpSocket(Socket socket)
        {
            ConfigureTcpSocket(socket, 8192);
        }

        public static void ConfigureTcpClient(TcpClient socket, int bufferSize)
        {
            // Don't allow another socket to bind to this port.
            //socket.ExclusiveAddressUse = true;

            // The socket will linger for 10 seconds after
            // Socket.Close is called.
            socket.LingerState = new LingerOption(true, 10);

            // Disable the Nagle Algorithm for this tcp socket.
            socket.NoDelay = true;

            // Set the receive buffer size to 8k
            socket.ReceiveBufferSize = bufferSize;

            // Set the timeout for synchronous receive methods to
            // 1 second (1000 milliseconds.)
            socket.ReceiveTimeout = 1000;

            // Set the send buffer size to 8k.
            socket.SendBufferSize = bufferSize;

            // Set the timeout for synchronous send methods
            // to 1 second (1000 milliseconds.)
            socket.SendTimeout = 1000;

            // Set the Time To Live (TTL) to 42 router hops.

            System.Diagnostics.Debug.WriteLine("Tcp Socket configured:");
            System.Diagnostics.Debug.WriteLine(string.Format("  ExclusiveAddressUse {0}", socket.ExclusiveAddressUse));
            System.Diagnostics.Debug.WriteLine(string.Format("  LingerState {0}, {1}", socket.LingerState.Enabled, socket.LingerState.LingerTime));
            System.Diagnostics.Debug.WriteLine(string.Format("  NoDelay {0}", socket.NoDelay));
            System.Diagnostics.Debug.WriteLine(string.Format("  ReceiveBufferSize {0}", socket.ReceiveBufferSize));
            System.Diagnostics.Debug.WriteLine(string.Format("  ReceiveTimeout {0}", socket.ReceiveTimeout));
            System.Diagnostics.Debug.WriteLine(string.Format("  SendBufferSize {0}", socket.SendBufferSize));
            System.Diagnostics.Debug.WriteLine(string.Format("  SendTimeout {0}", socket.SendTimeout));
        }

        public static void ConfigureTcpClient(TcpClient socket)
        {
            ConfigureTcpClient(socket, 8192);
        }

        #endregion

        #region SocketClient

        /// <summary>
        /// Client
        /// </summary>
        [Energy.Attribute.Code.Future]
        public class SocketClient
        {

        }

        #endregion

        #region SocketServer

        /// <summary>
        /// Server
        /// </summary>
        [Energy.Attribute.Code.Future]
        public class SocketServer
        {

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
            return Ping(address, 30000, out status);
        }

        #endregion
    }
}
