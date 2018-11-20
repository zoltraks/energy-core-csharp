using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ISocketConnection
    {
        string Host { get; set; }
        int Port { get; set; }
    }
}
