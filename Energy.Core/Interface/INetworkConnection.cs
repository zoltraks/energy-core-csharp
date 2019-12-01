using System.ComponentModel;

namespace Energy.Interface
{
    public interface INetworkConnection
    {
        /// <summary>
        /// Host name
        /// </summary>
        [DefaultValue(null)]
        string Host { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        [DefaultValue(0)]
        int Port { get; set; }
    }
}