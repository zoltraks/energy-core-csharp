﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Energy.Core
{
    public class Network
    {
        public static void ConfigureTcpSocket(Socket socket, int bufferSize = 8192)
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
            socket.ReceiveTimeout = 10000;

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
    }
}