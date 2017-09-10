using System;
using System.Collections.Generic;
using System.Text;

namespace ByteArrayBenchmark
{
    /// <summary>
    /// Builder for byte arrays.
    /// </summary>
    [Energy.Attribute.Markdown.Text(@"

    Byte array builder class
    ------------------------

    Use ByteArrayBuilder to build with byte arrays.

    This class is not thread safe for performance reasons.
    You must use your own locking mechanism to achieve thread safety.

    Limitations
    -----------

    You cant't change UseMemoryStream property when ByteArrayBuilder already contains data.

    ")]
    public class ByteArrayBuilder2: IDisposable
    {
        #region Buffer

        private List<byte[]> _ListBuffer;
     
        private int _Length;

        private int _Capacity;

        private int _InitialSize = 16;

        private int _ChunkSize = 16384;

        #endregion

        #region Property

        /// <summary>
        /// Accessor
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index]
        {
            get
            {
                if (_Length <= index)
                    throw new ArgumentOutOfRangeException();
                int block = index / _ChunkSize;
                int offset = index % _ChunkSize;
                return _ListBuffer[block][offset];
            }
            set
            {
                if (_Length <= index)
                    throw new ArgumentOutOfRangeException();
                int block = index / _ChunkSize;
                int offset = index % _ChunkSize;
                _ListBuffer[block][offset] = value;
            }
        }

        /// <summary>
        /// Returns true if object contains no data.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Length == 0;
            }
        }

        public int InitialSize
        {
            get
            {
                return _InitialSize;
            }
        }

        public int ChunkSize
        {
            get
            {
                return _ChunkSize;
            }            
        }

        public int Capacity
        {
            get
            {
                return _Capacity;
            }
            set
            {
                if (value == _Capacity)
                    return;
                SetCapacity(value);
            }
        }

        #endregion

        #region

        public ByteArrayBuilder2()
        {
        }

        public ByteArrayBuilder2(byte[] array)
        {
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Clear();
        }

        #endregion

        #region Clear

        private void Clear()
        {
            if (_ListBuffer != null)
            {
                _ListBuffer.Clear();
                _ListBuffer = null;
            }
            _Capacity = 0;
            _Length = 0;
        }

        #endregion

        #region Private

        private void EnsureCapacity(int size)
        {
            if (_Capacity >= size)
                return;
            SetCapacity(size);
        }

        public ByteArrayBuilder2 SetCapacity(int size)
        {
            System.Diagnostics.Debug.WriteLine(string.Concat("SetCapacity(", size, ")"));
            if (size <= 0)
            {
                Clear();
                return this;
            }
            if (_ListBuffer == null)
            {
                _ListBuffer = new List<byte[]>();
            }
            if (size < _InitialSize)
                size = _InitialSize;
            if (size <= _ChunkSize)
            {
                if (size > _InitialSize)
                {
                    int n = _InitialSize;
                    while (n < size)
                        n = n << 1;
                    size = n;
                }
                if (size == _Capacity)
                    return this;
                if (_ListBuffer.Count == 0)
                {
                    _ListBuffer.Add(new byte[size]);
                    _Capacity = size;
                    return this;
                }
                else
                {
                    byte[] b = new byte[size];
                    if (_Length > 0)
                    {
                        int l = _Length;
                        if (l > size)
                            l = size;
                        Array.Copy(_ListBuffer[0], b, l);
                        _Length = l;
                    }
                    _ListBuffer[0] = b;
                    _Capacity = size;
                }
                while (_ListBuffer.Count > 1)
                {
                    _ListBuffer.RemoveAt(_ListBuffer.Count - 1);
                }
                return this;
            }
            else
            {
                int listSize = (int)(Math.Ceiling((double)size / _ChunkSize));
                if (listSize == _ListBuffer.Count)
                    return this;
                while (_ListBuffer.Count < listSize)
                {
                    _ListBuffer.Add(new byte[_ChunkSize]);
                }
                while (_ListBuffer.Count > listSize)
                {
                    _ListBuffer.RemoveAt(_ListBuffer.Count - 1);
                }
                _Capacity = _ListBuffer.Count * _ChunkSize;
                return this;
            }
        }

        #endregion

        #region Append

        public ByteArrayBuilder2 Append(byte value)
        {
            EnsureCapacity(_Length + 1);
            int offset = _Length % _ChunkSize;
            _ListBuffer[_ListBuffer.Count - 1][offset] = value;
            _Length++;
            return this;
        }
        
        #endregion
    }
}
