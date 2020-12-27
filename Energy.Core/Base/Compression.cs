using System;
using System.IO;

namespace Energy.Base
{
    /// <summary>
    /// Compression
    /// </summary>
    public class Compression
    {
        /// <summary>
        /// Deflate
        /// </summary>
        public class Deflate
        {
            /// <summary>
            /// Compress using deflate algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Compress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (System.IO.Compression.DeflateStream compressionStream = new System.IO.Compression.DeflateStream(memoryStream
                        , System.IO.Compression.CompressionMode.Compress))
                    {
                        compressionStream.Write(data, 0, data.Length);
                        compressionStream.Close();
                        return memoryStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }

            /// <summary>
            /// Decompress using deflate algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Decompress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream inputStream = new MemoryStream(data))
                    using (MemoryStream outputStream = new MemoryStream())
                    using (System.IO.Compression.DeflateStream decompressionStream = new System.IO.Compression.DeflateStream(inputStream
                        , System.IO.Compression.CompressionMode.Decompress))
                    {
                        int count = 2048;
                        byte[] buffer = new byte[count];
                        for (; ; )
                        {
                            int length = decompressionStream.Read(buffer, 0, count);
                            if (length == 0)
                                break;
                            outputStream.Write(buffer, 0, length);
                        }
                        return outputStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }
        }

        /// <summary>
        /// GZip
        /// </summary>
        public class GZip
        {
            /// <summary>
            /// Compress using gzip algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Compress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (System.IO.Compression.GZipStream compressionStream = new System.IO.Compression.GZipStream(memoryStream
                        , System.IO.Compression.CompressionMode.Compress))
                    {
                        compressionStream.Write(data, 0, data.Length);
                        compressionStream.Close();
                        return memoryStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }

            /// <summary>
            /// Decompress using gzip algorithm.
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] Decompress(byte[] data)
            {
                byte[] array = null;
                try
                {
                    using (MemoryStream inputStream = new MemoryStream(data))
                    using (MemoryStream outputStream = new MemoryStream())
                    using (System.IO.Compression.GZipStream decompressionStream = new System.IO.Compression.GZipStream(inputStream
                        , System.IO.Compression.CompressionMode.Decompress))
                    {
                        int count = 2048;
                        byte[] buffer = new byte[count];
                        for (; ; )
                        {
                            int length = decompressionStream.Read(buffer, 0, count);
                            if (length == 0)
                                break;
                            outputStream.Write(buffer, 0, length);
                        }
                        return outputStream.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Energy.Core.Bug.Catch(exception);
                }
                return array;
            }
        }
    }
}
