using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// Base classes for networking.
    /// </summary>
    public class Network
    {
        #region Constant

        public const int DEFAULT_SOCKET_TIMEOUT = 10000;

        public const int DEFAULT_SOCKET_BUFFER_SIZE = 8192;

        public const int DEFAULT_PING_TIMEOUT = 30000;

        public const AddressFamily DEFAULT_ADDRESS_FAMILY = AddressFamily.Unspecified;

        /// <summary>
        /// Regex pattern for IPv4 address from https://ihateregex.io/expr/ipv4/
        /// </summary>
        public const string IP4_PATTERN = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";

        private const string IP4_PATTERN_FULL = "^" + IP4_PATTERN + "$";

        /// <summary>
        /// Regex pattern for IPv4 address from https://ihateregex.io/expr/ipv6/
        /// </summary>
        public const string IP6_PATTERN = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";

        private const string IP6_PATTERN_FULL = "^(\\[" + IP6_PATTERN + "\\]|" + IP6_PATTERN + ")$";

        #endregion

        #region Delegate

        public delegate void ConnectDelegate(object self);

        public delegate void CloseDelegate(object self);

        public delegate void ListenDelegate(object self);

        public delegate void AcceptDelegate(object self);

        public delegate void SendDelegate<T>(object self, T data);

        public delegate void SendDelegate(object self, byte[] data);

        public delegate void ReceiveDelegate<T>(object self, T data);

        public delegate void ReceiveDelegate(object self, byte[] data);

        public delegate void ErrorDelegate(object self);

        public delegate bool ExceptionDelegate(object self, Exception exception);

        #endregion

        #region Settings

        /// <summary>
        /// Settings
        /// </summary>
        public class Settings
        {
            public int SocketTimeout { get; set; }

            public int SocketBufferSize { get; set; }

            public AddressFamily AddressFamily { get; set; }

            public Settings()
            {
                SocketTimeout = DEFAULT_SOCKET_TIMEOUT;
                SocketBufferSize = DEFAULT_SOCKET_BUFFER_SIZE;
                AddressFamily = DEFAULT_ADDRESS_FAMILY;
            }
        }

        #endregion

        #region Connection

        /// <summary>
        /// Network connection information
        /// </summary>
        public class Connection
        {
            [DefaultValue(null)]
            public string Host { get; set; }

            [DefaultValue(null)]
            public string Protocol { get; set; }

            public ProtocolType Type { get; set; }

            [DefaultValue(0)]
            public int Port { get; set; }
        }

        #endregion

        #region Utility

        #region IsValidAddress

        /// <summary>
        /// Check if text is valid IPv4 or IPv6 network address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValidAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }
            if (Regex.Match(address, IP4_PATTERN_FULL).Success)
            {
                return true;
            }
            else if (Regex.Match(address, IP6_PATTERN_FULL).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
    }
}
