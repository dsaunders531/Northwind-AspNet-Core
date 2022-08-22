using System;

namespace tools
{
    public interface IEncryptionProvider : IDisposable
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
