using System;
using System.Collections.Generic;

namespace Energy.Base
{
    /// <summary>
    /// Builder for byte arrays.
    /// </summary>
    [Energy.Attribute.Markdown.Text(@"

    Byte array builder class
    ------------------------

    Use ByteArrayBuilder to build byte arrays with MemoryStream.

    This class is not thread safe for performance reasons.
    You must use your own locking mechanism to achieve thread safety.

    ")]
    public class ByteArrayBuilder : IDisposable
    {
        #region Buffer

        private System.IO.MemoryStream _Stream;

        private System.Text.Encoding _Encoding = System.Text.Encoding.UTF8;

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
                byte[] buffer = _Stream.GetBuffer();
                return buffer[index];
            }
            set
            {
                byte[] buffer = _Stream.GetBuffer();
                buffer[index] = value;
            }
        }

        /// <summary>
        /// Returns true if object contains no data.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Stream.Length == 0;
            }
        }

        /// <summary>
        /// Current data length.
        /// </summary>
        public int Length
        {
            get
            {
                return (int)_Stream.Length;
            }
        }

        /// <summary>
        /// Buffer capacity.
        /// </summary>
        public int Capacity
        {
            get
            {
                return _Stream.Capacity;
            }
            set
            {
                if (value == _Stream.Capacity)
                    return;
                _Stream.Capacity = value;
            }
        }

        /// <summary>
        /// String encoding.
        /// </summary>
        public System.Text.Encoding Encoding
        {
            get
            {
                return _Encoding;
            }
            set
            {
                _Encoding = value;
            }
        }

        #endregion

        #region

        /// <summary>
        /// Constructor
        /// </summary>
        public ByteArrayBuilder()
        {
            _Stream = new System.IO.MemoryStream();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public ByteArrayBuilder(byte[] data)
        {
            _Stream = new System.IO.MemoryStream(data);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (_Stream != null)
            {
                _Stream.Close();
                _Stream.Dispose();
                _Stream = null;
            }
        }

        #endregion

        #region Clear

        private void Clear()
        {
            _Stream.Close();
            _Stream.Dispose();
            _Stream = new System.IO.MemoryStream();
        }

        #endregion

        #region Append

        /// <summary>
        /// Append byte value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(byte value)
        {
            Tail();
            _Stream.WriteByte(value);
            return this;
        }

        /// <summary>
        /// Append byte array to a stream.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(byte[] data)
        {
            Tail();
            if (data == null || data.Length == 0)
                return this;
            _Stream.Write(data, 0, data.Length);
            return this;
        }

        /// <summary>
        /// Append byte array initialized to value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(byte value, int count)
        {
            Tail();
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
                data[i] = value;
            _Stream.Write(data, 0, data.Length);
            return this;
        }

        /// <summary>
        /// Append signed byte value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(sbyte value)
        {
            Tail();
            _Stream.WriteByte((byte)value);
            return this;
        }

        /// <summary>
        /// Append char value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(char value)
        {
            Tail();
            _Stream.WriteByte((byte)value);
            return this;
        }

        /// <summary>
        /// Append integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(int value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append long integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(long value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(uint value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append unsigned long integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(ulong value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append short value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(short value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append unsigned short value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(ushort value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append unsigned short value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(bool value)
        {
            return Append((byte)(value ? 1 : 0));
        }

        /// <summary>
        /// Append double value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(double value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append double value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(float value)
        {
            return Append(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Append GUID value to a stream.
        /// </summary>
        /// <param name="guid"></param>
        public ByteArrayBuilder Append(Guid guid)
        {
            return Append(guid.ToByteArray());
        }

        /// <summary>
        /// Append decimal value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(decimal value)
        {
            int[] data = decimal.GetBits(value);
            Tail();
            WriteInt(data[0]);
            WriteInt(data[1]);
            WriteInt(data[2]);
            WriteInt(data[3]);
            return this;
        }

        /// <summary>
        /// Append string to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(string value, System.Text.Encoding encoding, bool raw)
        {
            Tail();
            if (raw && string.IsNullOrEmpty(value))
                return this;
            byte[] data = encoding.GetBytes(value);
            if (!raw)
                WriteInt(data.Length);
            WriteArray(data);
            return this;
        }

        /// <summary>
        /// Append string to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(string value, System.Text.Encoding encoding)
        {
            return Append(value, encoding, false);
        }

        /// <summary>
        /// Append string to a stream with default encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(string value)
        {
            return Append(value, _Encoding, false);
        }

        /// <summary>
        /// Append string to a stream with default encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(string value, bool raw)
        {
            return Append(value, _Encoding, raw);
        }

        /// <summary>
        /// Append DateTime to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ByteArrayBuilder Append(DateTime value)
        {
            return Append(value.Ticks);
        }

        #endregion

        #region Read

        /// <summary>
        /// Read byte value from a stream.
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return (byte)_Stream.ReadByte();
        }

        /// <summary>
        /// Read char value from a stream.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return (char)_Stream.ReadByte();
        }

        /// <summary>
        /// Read signed byte value from a stream.
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return (sbyte)_Stream.ReadByte();
        }

        /// <summary>
        /// Read byte array from a stream.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadByteArray(int length)
        {
            byte[] data = new byte[length];
            if (length > 0)
            {
                int read = _Stream.Read(data, 0, length);
                if (read != length)
                    throw new ArgumentOutOfRangeException();
            }
            return data;
        }

        /// <summary>
        /// Read byte array from a stream.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadByteArray()
        {
            return ReadByteArray((int)(_Stream.Length - _Stream.Position));
        }

        /// <summary>
        /// Read integer value from a stream.
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadByteArray(4), 0);
        }

        /// <summary>
        /// Read long integer value from a stream.
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadByteArray(8), 0);
        }

        /// <summary>
        /// Read short integer value from a stream.
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadByteArray(2), 0);
        }

        /// <summary>
        /// Read unsigned integer value from a stream.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(ReadByteArray(4), 0);
        }

        /// <summary>
        /// Read unsigned long integer value from a stream.
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(ReadByteArray(8), 0);
        }

        /// <summary>
        /// Read unsigned short integer value from a stream.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadByteArray(2), 0);
        }

        /// <summary>
        /// Read double value from a stream.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadByteArray(8), 0);
        }

        /// <summary>
        /// Read float value from a stream.
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadByteArray(4), 0);
        }

        /// <summary>
        /// Read decimal value from a stream.
        /// </summary>
        /// <returns></returns>
        public decimal ReadDecimal()
        {
            int[] bits = new int[] { ReadInt(), ReadInt(), ReadInt(), ReadInt() };
            return new decimal(bits);
        }

        /// <summary>
        /// Read boolean value from a stream.
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            return _Stream.ReadByte() != 0;
        }

        /// <summary>
        /// Read DateTime value from a stream.
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            return new DateTime(ReadLong());
        }

        /// <summary>
        /// Read Guid from a stram.
        /// </summary>
        /// <returns></returns>
        public Guid ReadGuid()
        {
            return new Guid(ReadArray(16));
        }

        /// <summary>
        /// Read string from a stream.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadString(int length, System.Text.Encoding encoding)
        {
            byte[] data = ReadByteArray(length);
            return encoding.GetString(data);
        }

        /// <summary>
        /// Read string from a stream.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadString(int length)
        {
            byte[] data = ReadByteArray(length);
            return _Encoding.GetString(data);
        }

        /// <summary>
        /// Read string from a stream.
        /// </summary>
        /// <returns></returns>
        public string ReadString(System.Text.Encoding encoding, bool raw)
        {
            byte[] data = raw ? ReadByteArray() : ReadByteArray(ReadInt());
            return encoding.GetString(data);
        }

        /// <summary>
        /// Read string from a stream.
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public string ReadString(bool raw)
        {
            byte[] data = raw ? ReadByteArray() : ReadByteArray(ReadInt());
            return _Encoding.GetString(data);
        }

        /// <summary>
        /// Read string from a stream.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadString(System.Text.Encoding encoding)
        {
            byte[] data = ReadByteArray(ReadInt());
            return encoding.GetString(data);
        }

        /// <summary>
        /// Read string from a stream.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            byte[] data = ReadByteArray(ReadInt());
            return _Encoding.GetString(data);
        }

        #endregion

        #region Write

        /// <summary>
        /// Write byte value to a stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            _Stream.WriteByte(value);
        }

        /// <summary>
        /// Write unsigned byte value to a stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteChar(char value)
        {
            _Stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Write signed byte value to a stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteSByte(sbyte value)
        {
            _Stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Write byte array to a stream.
        /// </summary>
        /// <param name="data"></param>
        public void WriteArray(byte[] data)
        {
            if (data == null || data.Length == 0)
                return;
            _Stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Write integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt(int value)
        {
            WriteArray(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write long integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(long value)
        {
            WriteArray(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write unsigned integer value to a stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt(uint value)
        {
            WriteArray(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write unsigned long
        /// </summary>
        /// <param name="value"></param>
        public void WriteULong(uint value)
        {
            WriteArray(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write short value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteShort(short value)
        {
            WriteArray(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write unsigned short value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteUShort(ushort value)
        {
            WriteArray(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write boolean value to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteBool(bool value)
        {
            _Stream.WriteByte((byte)(value ? 1 : 0));
        }

        /// <summary>
        /// Append DateTime to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteDateTime(DateTime value)
        {
            byte[] data = BitConverter.GetBytes(value.Ticks);
            _Stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Write GUID value to a stream.
        /// </summary>
        /// <param name="guid"></param>
        public void WriteGuid(Guid guid)
        {
            WriteArray(guid.ToByteArray());
        }

        /// <summary>
        /// Write string to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public void WriteString(string value, System.Text.Encoding encoding, bool raw)
        {
            if (raw && string.IsNullOrEmpty(value))
                return;
            byte[] data = encoding.GetBytes(value);
            if (!raw)
                WriteInt(data.Length);
            WriteArray(data);
        }

        /// <summary>
        /// Write string to a stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public void WriteString(string value, System.Text.Encoding encoding)
        {
            WriteString(value, encoding, false);
        }

        /// <summary>
        /// Append string to a stream with default encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteString(string value)
        {
            WriteString(value, _Encoding, false);
        }

        /// <summary>
        /// Write string to a stream with default encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public void WriteString(string value, bool raw)
        {
            WriteString(value, _Encoding, raw);
        }

        #endregion

        #region Read

        /// <summary>
        /// Get array of bytes from stream.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadArray(int length)
        {
            byte[] data = new byte[length];
            if (length > 0)
            {
                int read = _Stream.Read(data, 0, length);
                if (read != length)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            return data;
        }

        #endregion

        #region Seek

        /// <summary>
        /// Set an absolute position in the builder for reading.
        /// </summary>
        /// <param name="position"></param>
        public void Seek(int position)
        {
            if (position >= 0)
                _Stream.Seek((long)position, System.IO.SeekOrigin.Begin);
            else
                _Stream.Seek(-(long)position, System.IO.SeekOrigin.End);
        }

        #endregion

        #region Rewind

        /// <summary>
        /// Move to the beginning of stream.
        /// </summary>
        public void Rewind()
        {
            _Stream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        #endregion

        #region Tail

        /// <summary>
        /// Move to the end of stream.
        /// </summary>
        public void Tail()
        {
            if (_Stream.Position != _Stream.Length)
                _Stream.Seek(0, System.IO.SeekOrigin.End);
        }

        #endregion

        #region Skip

        /// <summary>
        /// Skip next bytes in stream.
        /// </summary>
        /// <param name="count"></param>
        public void Skip(int count)
        {
            _Stream.Seek(count, System.IO.SeekOrigin.Current);
        }

        #endregion

        #region ToArray

        /// <summary>
        /// Returns the builder as an array of bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            int length = (int)_Stream.Length;
            byte[] data = new byte[length];
            Array.Copy(_Stream.GetBuffer(), data, length);
            return data;
        }

        #endregion
    }
}
