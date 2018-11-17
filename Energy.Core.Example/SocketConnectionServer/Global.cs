using System;
using System.Collections.Generic;
using System.Text;

namespace SocketConnectionServer
{
    public static class Global
    {
        public static Energy.Base.Counter ClientCounter = new Energy.Base.Counter();

        public static string HelloText = "Hello client {0}";
    }
}
