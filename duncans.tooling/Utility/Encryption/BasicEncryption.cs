// <copyright file="BasicEncryption.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace duncans.Utility
{
    /// <summary>
    /// Very simple encryption. Create your own keys!
    /// </summary>
    public sealed class BasicEncryption : IEncryptionProvider, IDisposable
    {
        // 16 parts each
        private byte[] key = new byte[] { 52, 12, 6, 89, 65, 45, 98, 201, 185, 8, 78, 110, 156, 86, 41, 55 };

        // initialisation vector!
        private byte[] iv = new byte[] { 74, 12, 103, 106, 92, 245, 68, 201, 241, 135, 68, 74, 1, 64, 24, 9 };

        private System.Security.Cryptography.Aes cryptor = null;

        private Encoding encoding = Encoding.UTF8;

        private int padding = 1024;

        private bool disposedValue = false; // To detect redundant calls

        public BasicEncryption()
        {
            this.CreateEncryptor(this.key, this.iv);
        }

        public BasicEncryption(byte[] key, byte[] iv)
        {
            this.CreateEncryptor(key, iv);
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] EncryptToBytes(string value)
        {
            byte[] output = null;
            ICryptoTransform et = cryptor.CreateEncryptor(cryptor.Key, cryptor.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, et, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs, this.encoding, this.padding))
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
                return Convert.ToBase64String(this.EncryptToBytes(value.HTMLEncode()));
            }
        }

        public string Encrypt(string value)
        {
            return this.EncryptToString(value);
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public string DecryptBytes(byte[] value)
        {
            ICryptoTransform et = cryptor.CreateDecryptor(cryptor.Key, cryptor.IV);
            string output = string.Empty;

            if (value != null)
            {
                using (MemoryStream ms = new MemoryStream(value))
                {
                    using (CryptoStream cs = new CryptoStream(ms, et, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs, this.encoding, true, this.padding))
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
                return this.DecryptBytes(Convert.FromBase64String(value)).HTMLDecode();
            }
            else
            {
                return value;
            }
        }

        public string Decrypt(string value)
        {
            return this.DecryptString(value);
        }

        #region IDisposable Support
        public void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cryptor.Dispose();
                    cryptor = null;
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

        private void CreateEncryptor(byte[] key, byte[] iv)
        {
            cryptor = Aes.Create();
            cryptor.Key = key;
            cryptor.IV = iv;
        }
    }
}
