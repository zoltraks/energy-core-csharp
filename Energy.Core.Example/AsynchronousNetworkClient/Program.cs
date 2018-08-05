using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsynchronousNetworkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //(new SimpleClient()).Start();

            string host;
            //host = "localhost";
            //host = "211.1.1.1";
            //host = "::1";
            host = "127.0.0.1";
            int port = 9000;

            string value;
            int integer;

            value = Energy.Core.Tilde.Ask("Host", host);
            if (!string.IsNullOrEmpty(value))
                host = value;

            value = Energy.Core.Tilde.Ask("Port", port.ToString());
            if (0 < (integer = Energy.Base.Cast.StringToInteger(value)))
                port = integer;

            string address = Energy.Core.Network.GetHostAddress(host);
            Energy.Core.Tilde.WriteLine($"Trying to connect to ~w~{address}~0~:~y~{port}~0~ and start conversation...");
            IPAddress ipAddress = IPAddress.Parse(address);
            //IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            StateObject state = new StateObject();
            state.ConnectionRepeat = -1;
            state.Host = host;
            state.Port = port;
            state.Capacity = 10;

            Connect(state);

            Energy.Core.Tilde.Pause();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                Energy.Core.Bug.Write(Energy.Core.Bug.CallingMethod()
                    + " " + Energy.Core.Bug.ThreadIdHex());

                Socket socket = state.Socket;

                // Complete the connection.  
                socket.EndConnect(ar);

                Energy.Core.Tilde.WriteLine("Socket connected to ~g~{0}", socket.RemoteEndPoint.ToString());

                string text = "Hello\r\n";
                byte[] data = Encoding.UTF8.GetBytes(text);

                // Begin sending the data to the remote
                socket.BeginSend(data, 0, data.Length, 0,
                    new AsyncCallback(SendCallback), state);
            }
            catch (System.Net.Sockets.SocketException socketException)
            {
                bool time = false;
                bool block = false;
                switch (socketException.ErrorCode)
                {
                    default:
                        break;
                    case 10054:
                        // Message: "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond"
                        // Message: "An existing connection was forcibly closed by the remote host"
                        // NativeErrorCode: 10054
                        // SocketErrorCode: ConnectionReset
                        break;
                    case 10060:
                        // NativeErrorCode: 10060
                        // SocketErrorCode: TimedOut
                        time = true;
                        break;
                    case 10061:
                        // Message: "No connection could be made because the target machine actively refused it"                    
                        // NativeErrorCode: 10061
                        // SocketErrorCode: ConnectionRefused
                        block = true;
                        break;
                }
                Energy.Core.Bug.Catch(socketException);
                if (block || time)
                {
                    if (state.ConnectionRepeat < 0 || state.ConnectionRepeat > 0)
                    {
                        if (state.ConnectionRepeat > 0)
                            state.ConnectionRepeat--;

                        Socket socket = state.Socket;
                        socket.Close();
                        socket = new Socket(Energy.Core.Network.GetAddressFamily(state.Host)
                            , SocketType.Stream, ProtocolType.Tcp);
                        state.Socket = socket;
                        socket.BeginConnect(state.Host, state.Port, new AsyncCallback(ConnectCallback), state);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Energy.Core.Tilde.Exception(e, true);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Energy.Core.Bug.Write(Energy.Core.Bug.CallingMethod()
                + " " + Energy.Core.Bug.ThreadIdHex());
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                Socket client = state.Socket;

                // Complete sending the data to the remote
                int bytesSent = client.EndSend(ar);
                Energy.Core.Tilde.WriteLine("Sent ~w~{0}~0~ bytes to server", bytesSent);

                int size = state.Capacity;
                state.Buffer = new byte[size];
                state.Stream = new MemoryStream(size);
                // Begin receiving the data from the remote 
                client.BeginReceive(state.Buffer, 0, size, SocketFlags.None
                    , new AsyncCallback(ReceiveCallback), state);
            }
            catch (SocketException socketException)
            {
                switch (socketException.SocketErrorCode)
                {
                    default:
                        Energy.Core.Tilde.Exception(socketException);
                        break;
                    case SocketError.ConnectionReset:
                        Socket socket = state.Socket;
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        Connect(state);
                        break;
                }
            }
        }

        private static void Connect(StateObject state)
        {
            Socket socket;
            // Create a TCP/IP socket
            socket = new Socket(Energy.Core.Network.GetAddressFamily(state.Host)
                , SocketType.Stream, ProtocolType.Tcp);
            state.Socket = socket;
            socket.BeginConnect(state.Host, state.Port, new AsyncCallback(ConnectCallback), state);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Energy.Core.Bug.Write(Energy.Core.Bug.CallingMethod()
             + " " + Energy.Core.Bug.ThreadIdHex());
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                Socket client = state.Socket;
                // Read data from the remote
                int bytesRead = client.EndReceive(ar);
                Energy.Core.Tilde.WriteLine("Read ~w~{0}~0~ bytes from server", bytesRead);

                if (bytesRead > 0)
                {
                    state.Stream.Write(state.Buffer, 0, bytesRead);
                    //  Get the rest of the data if any
                    if (client.Available > 0)
                    {
                        client.BeginReceive(state.Buffer, 0, state.Capacity, SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), state);
                        return;
                    }
                }

                byte[] data;
                data = state.Stream.ToArray();
                Energy.Core.Memory.Clear(state.Stream);

                data = Parse(state, data);

                if (data != null)
                {
                    if (data.Length == 0)
                    {
                        // Begin receiving the data from the remote 
                        client.BeginReceive(state.Buffer, 0, state.Capacity, SocketFlags.None
                            , new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        // Begin sending the data to the remote
                        client.BeginSend(data, 0, data.Length, SocketFlags.None
                            , new AsyncCallback(SendCallback), state);
                    }
                }
            }
            catch (SocketException socketException)
            {
                switch (socketException.SocketErrorCode)
                {
                    default:
                        break;
                    case SocketError.ConnectionReset:
                        Socket socket = state.Socket;
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        Connect(state);
                        break;
                }
            }
            catch (Exception e)
            {
                Energy.Core.Tilde.Exception(e, true);
            }
        }

        private static byte[] Parse(StateObject state, byte[] data)
        {
            string text = "";
            try
            {
                text = Encoding.UTF8.GetString(data);
                text = text.Trim();
                if (text.Length > 0)
                {
                    text = "Repeat \"" + text + "\"";
                }
            }
            catch (DecoderFallbackException exceptionDecoderFallback)
            {
                text = exceptionDecoderFallback.Message;
            }

            if (text.Length == 0)
            {
                Socket socket = state.Socket;
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Connect(state);
                return null;
            }

            text += "\r\n";

            data = Encoding.UTF8.GetBytes(text);

            return data;
        }
    }
}
