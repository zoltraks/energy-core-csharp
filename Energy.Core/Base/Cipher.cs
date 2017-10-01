using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace Energy.Base
{
    /// <summary>
    /// Cipher functions
    /// </summary>
    public class Cipher
    {
        /// <summary>
        /// Generic class for cipher algorithms with default behaviour of
        /// throwing NotSupportedException if method is not implemented
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when method is not supported</exception>>
        public abstract class Method
        {
            public virtual string Encrypt(string content)
            {
                throw new System.NotSupportedException();
            }

            public virtual string Decrypt(string content)
            {
                throw new System.NotSupportedException();
            }
        }
    }
}
