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

            string address = Energy.Core.Network.GetHostAddress(host);
            Energy.Core.Tilde.WriteLine($"~w~{address}");
            IPAddress ipAddress = IPAddress.Parse(address);
            //IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            Socket client = new Socket((AddressFamily)Energy.Core.Network.GetAddressFamily(address),
                SocketType.Stream, ProtocolType.Tcp);

            StateObject state = new StateObject();
            state.Socket = client;
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

                Energy.Core.Tilde.WriteLine("Socket connected to ~g~{0}",
                    socket.RemoteEndPoint.ToString());

                string text = "Hello\r\n";
                // Convert the string data to byte data using ASCII encoding.  
                byte[] data = Encoding.ASCII.GetBytes(text);

                // Begin sending the data to the remote device.  
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

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Energy.Core.Tilde.WriteLine("Sent ~w~{0}~0~ bytes to server", bytesSent);

                int size = state.Capacity;
                state.Buffer = new byte[size];
                state.Stream = new MemoryStream(size);
                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.Buffer, 0, size, 0,
                    new AsyncCallback(ReceiveCallback), state);
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
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                Socket client = state.Socket;
                // Read data from the remote device.
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
                    return;
                }

                text += "\r\n";

                data = Encoding.UTF8.GetBytes(text);

                // Begin sending the data to the remote device.  
                client.BeginSend(data, 0, data.Length, 0,
                    new AsyncCallback(SendCallback), state);
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
    }
}
