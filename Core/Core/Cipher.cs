using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Energy.Core
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
        /// <throws>
        /// NotSupportedException
        /// </throws>
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

        /// <summary>
        /// MD5
        /// </summary>
        public class MD5 : Method
        {
            public override string Encrypt(string content)
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                StringBuilder t = new StringBuilder(b.Length * 2);
                for (int i = 0; i < b.Length; i++)
                {
                    t.Append(b[i].ToString("x2"));
                }
                return t.ToString();
            }
        }

        /// <summary>
        /// SHA1
        /// </summary>
        public class SHA1 : Method
        {
            public override string Encrypt(string content)
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] b = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
                    StringBuilder t = new StringBuilder(b.Length * 2);
                    for (int i = 0; i < b.Length; i++)
                    {
                        t.Append(b[i].ToString("x2"));
                    }
                    return t.ToString();
                }
            }
        }

        /// <summary>
        /// DES
        /// </summary>
        public class DES : Method
        {
            public string Secret { private get; set; }

            /// <summary>
            /// Encrypt a string using DES algorithm
            /// </summary>
            /// <param name="text">Decrypted string</param>
            /// <returns>Encrypted string</returns>
            public override string Encrypt(string text)
            {
                if (String.IsNullOrEmpty(text)) return "";

                ICryptoTransform algorithm = CreateAlgorithm();

                MemoryStream memory = new MemoryStream();
                CryptoStream crypto = new CryptoStream(memory, algorithm, CryptoStreamMode.Write);

                StreamWriter writer = new StreamWriter(crypto);
                writer.Write(text);
                writer.Flush();
                crypto.FlushFinalBlock();
                return System.Convert.ToBase64String(memory.GetBuffer(), 0, (int)memory.Length);
            }

            /// <summary>
            /// Decrypt a string using DES algorithm
            /// </summary>
            /// <param name="text">Encrypted string</param>
            /// <returns>Decrypted string</returns>
            public override string Decrypt(string text)
            {
                if (String.IsNullOrEmpty(text)) return "";

                ICryptoTransform algorithm = CreateAlgorithm();

                MemoryStream memory = new MemoryStream(System.Convert.FromBase64String(text));
                CryptoStream crypto = new CryptoStream(memory, algorithm, CryptoStreamMode.Read);

                StreamReader reader = new StreamReader(crypto);
                return reader.ReadToEnd();
            }

            private ICryptoTransform CreateAlgorithm()
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                byte[] key, vector;

                if (Secret == null)
                {
                    //// Use DES secret key and initialization vector
                    key = provider.Key;
                    vector = provider.IV;
                }
                else
                {
                    // Use secret text as key and vector
                    string secret = Secret;

                    while (secret.Length < 8) secret = secret + secret;
                    if (secret.Length > 8) secret = secret.Substring(0, 8);

                    key = ASCIIEncoding.ASCII.GetBytes(secret);
                    vector = key;
                }

                return provider.CreateEncryptor(key, vector);
            }
        }
    }
}
