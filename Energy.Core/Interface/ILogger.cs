using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface ILogger
    {
        Energy.Base.Log.Entry Add(string message, int level);

        Energy.Base.Log.Entry Write(Exception exception);
    }
}
