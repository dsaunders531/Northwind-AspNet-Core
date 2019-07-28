// <copyright file="DbClientException.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans
{
    [Serializable]
    public class DbClientException : Exception
    {
        public DbClientException() { }

        public DbClientException(string message) : base(message) { }

        public DbClientException(string message, Exception innerException) : base(message, innerException) { }
    }
}
