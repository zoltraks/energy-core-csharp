using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface IFileHandle
    {
        int Read(int count, out byte[] buffer);
        int Write(byte[] buffer);
        long Seek(long offset, System.IO.SeekOrigin origin);
    }
}
