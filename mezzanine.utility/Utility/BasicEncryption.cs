using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace mezzanine.Utility
{
    /// <summary>
    /// Very simple encryption. Create your own keys!
    /// </summary>
    public sealed class BasicEncryption : IEncryptionProvider, IDisposable
    {
        // 16 parts each
        private readonly byte[] _key = new byte[] { 52, 12, 6, 89, 49, 128, 98, 201, 185, 16, 78, 110, 65, 86, 41, 55 };
        // initialisation vector!
        private readonly byte[] _IV = new byte[] { 74, 63, 103, 106, 88, 142, 68, 201, 241, 135, 68, 74, 1, 64, 24, 9 };

        private System.Security.Cryptography.Aes _cryptor = null;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly int _padding = 1024;

        public BasicEncryption()
        {
            CreateEncryptor(_key, _IV);
        }

        public BasicEncryption(byte[] key, byte[] iv)
        {
            CreateEncryptor(key, iv);
        }

        private void CreateEncryptor(byte[] key, byte[] iv)
        {
            _cryptor = Aes.Create();
            _cryptor.Key = key;
            _cryptor.IV = iv;
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] EncryptToBytes(string value)
        {
            byte[] output = null;
            ICryptoTransform et = _cryptor.CreateEncryptor(_cryptor.Key, _cryptor.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, et, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs, _encoding, _padding))
                    {
                        sw.Write(value);
                        sw.Flush();
                    }
                    cs.Flush();
                    output = ms.ToArray();
                }

                ms.Flush();
                ms.Dispose();
            }

            et.Dispose();
            et = null;
            return output;
        }

        public string EncryptToString(string value)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                return Convert.ToBase64String(EncryptToBytes(value));
            }
        }

        public string Encrypt(string value)
        {
            return EncryptToString(value);
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public string DecryptBytes(byte[] value)
        {
            ICryptoTransform et = _cryptor.CreateDecryptor(_cryptor.Key, _cryptor.IV);
            string output = string.Empty;

            if (value != null)
            {
                using (MemoryStream ms = new MemoryStream(value))
                {
                    using (CryptoStream cs = new CryptoStream(ms, et, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs, _encoding, true, _padding))
                        {
                            output = sr.ReadToEnd();
                            sr.Dispose();
                        }
                        cs.Dispose();
                    }
                    ms.Dispose();
                }
            }

            return output;
        }

        public string DecryptString(string value)
        {
            if (value != null)
            {
                return DecryptBytes(Convert.FromBase64String(value));
            }
            else
            {
                return value;
            }
        }

        public string Decrypt(string value)
        {
            return DecryptString(value);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cryptor.Dispose();
                    _cryptor = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
