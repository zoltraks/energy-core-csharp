using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace Energy.Core
{
    /// <summary>
    /// Cipher functions
    /// </summary>
    public class Cipher
    {
        /// <summary>
        /// MD5
        /// </summary>
        public class MD5 : Energy.Base.Cipher.Method
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
        public class SHA1 : Energy.Base.Cipher.Method
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
        public class DES : Energy.Base.Cipher.Method, IDisposable
        {
            private string _Secret;

            public string Secret
            {
                private get
                {
                    return _Secret;
                }
                set
                {
                    _Secret = value;
                    _Encryptor = null;
                    _Decryptor = null;
                }
            }

            private ICryptoTransform _Encryptor;

            private ICryptoTransform Encryptor
            {
                get
                {
                    if (_Encryptor == null)
                    {
                        _Encryptor = CreateProviderAlgorithm(true);
                    }
                    return _Encryptor;
                }
            }

            private ICryptoTransform _Decryptor;

            private ICryptoTransform Decryptor
            {
                get
                {
                    if (_Decryptor == null)
                    {
                        _Decryptor = CreateProviderAlgorithm(false);
                    }
                    return _Decryptor;
                }
            }

            public DES() { }

            public DES(string secret)
            {
                this.Secret = secret;
            }

            /// <summary>
            /// Encrypt a string using DES algorithm
            /// </summary>
            /// <param name="text">Decrypted string</param>
            /// <returns>Encrypted string</returns>
            public override string Encrypt(string text)
            {
                if (String.IsNullOrEmpty(text))
                    return "";

                ICryptoTransform algorithm = Encryptor;

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
                if (string.IsNullOrEmpty(text))
                    return "";

                ICryptoTransform algorithm = Decryptor;

                MemoryStream memory = new MemoryStream(System.Convert.FromBase64String(text));
                CryptoStream crypto = new CryptoStream(memory, algorithm, CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(crypto);
                try
                {
                    return reader.ReadToEnd();
                }
                catch (CryptographicException x)
                {
                    Debug.WriteLine(x.Message);
                    return null;
                }
            }

            private ICryptoTransform CreateProviderAlgorithm(bool encryptor)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                byte[] key, vector;

                if (string.IsNullOrEmpty(Secret))
                {
                    //// Use DES secret key and initialization vector
                    key = provider.Key;
                    vector = provider.IV;
                }
                else
                {
                    // Use secret text as key and vector
                    string secret = Secret;

                    while (secret.Length < 8)
                        secret = secret + secret;
                    if (secret.Length > 8)
                        secret = secret.Substring(0, 8);

                    key = ASCIIEncoding.UTF8.GetBytes(secret);
                    vector = key;
                }

                return encryptor ? provider.CreateEncryptor(key, vector) : provider.CreateDecryptor(key, vector);
            }

            public void Dispose()
            {
                _Secret = null;
                _Encryptor = null;
                _Decryptor = null;
            }
        }
    }
}
