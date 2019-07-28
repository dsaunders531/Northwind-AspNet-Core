// <copyright file="IEncryptionProvider.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans
{
    /// <summary>
    /// Interface for encryption.
    /// </summary>
    public interface IEncryptionProvider : IDisposable
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }
}
