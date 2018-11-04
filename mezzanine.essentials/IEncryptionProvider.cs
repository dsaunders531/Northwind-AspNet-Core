using System;

namespace mezzanine
{
    public interface IEncryptionProvider : IDisposable
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
