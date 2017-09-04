using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Builder for byte arrays.
    /// </summary>
    public class ByteArrayBuilder: IDisposable
    {
        public class MemoryStream
        {

        }

        private List<byte[]> _ListBuffer;

        public ByteArrayBuilder()
        {
        }

        public void Add(byte b)
        {

        }

        public void Dispose()
        {
            Clear();
        }

        private void Clear()
        {
            if (_ListBuffer != null)
            {
                _ListBuffer.Clear();
                _ListBuffer = null;
            }
        }
    }
}
