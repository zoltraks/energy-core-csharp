using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Builder for byte arrays.
    /// </summary>
    [Energy.Attribute.Markdown.Text(@"

    Byte array builder class
    ------------------------

    Use ByteArrayBuilder to work with byte arrays.

    This class is not thread safe for performance reasons.
    You must use your own locking mechanism to achieve thread safety.

    Limitations
    -----------

    You cant't change UseMemoryStream property when ByteArrayBuilder already contains data.

    ")]
    public class ByteArrayBuilder: IDisposable
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

        public ByteArrayBuilder()
        {
        }

        public ByteArrayBuilder(byte[] array)
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
        }

        #endregion

        #region Private

        private void EnsureCapacity(int size)
        {
            if (_Capacity >= size)
                return;
            if (size < _InitialSize)
            {
                SetCapacity(_InitialSize);
            }
            else if (size <= _ChunkSize)
            {
                if (size % _InitialSize == 0)
                {
                    SetCapacity(size);
                }
                else
                {
                    int ceil = (int)Math.Ceiling((double)(size / _InitialSize));
                    SetCapacity(ceil * _InitialSize);
                }
            }
            else
            {
                int ceil = (int)Math.Ceiling((double)(size / _ChunkSize));
                SetCapacity(ceil * _ChunkSize);
            }
        }

        public ByteArrayBuilder SetCapacity(int size)
        {
            System.Diagnostics.Debug.WriteLine(string.Concat("SetCapacity(", size, ")"));
            if (size <= 0)
            {
                Clear();
                return this;
            }
            if (size <= _ChunkSize)
            {
                return this;
            }
            else
            {
                int listSize = (int)Math.Ceiling((double)(size / _ChunkSize));
                while (_ListBuffer.Count < listSize)
                {
                    _ListBuffer.Add(new byte[_ChunkSize]);
                }
                while (_ListBuffer.Count > listSize)
                {
                    _ListBuffer.RemoveAt(_ListBuffer.Count - 1);
                }
                return this;
            }
        }

        #endregion

        #region Append

        public ByteArrayBuilder Append(byte value)
        {
            EnsureCapacity(_Length + 1);
            this[_Length++] = value;
            return this;
        }

        public ByteArrayBuilder Append(byte[] value) { return this; }
        public ByteArrayBuilder Append(byte value, int repeatCount) { return this; }
        public ByteArrayBuilder Append(int value) { return this; }
        public ByteArrayBuilder Append(long value) { return this; }
        public ByteArrayBuilder Append(sbyte value) { return this; }
        public ByteArrayBuilder Append(ulong value) { return this; }
        public ByteArrayBuilder Append(uint value) { return this; }
        public ByteArrayBuilder Append(ushort value) { return this; }
        public ByteArrayBuilder Append(string value) { return this; }
        public ByteArrayBuilder Append(object value) { return this; }

        #endregion
    }
}
