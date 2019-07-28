// <copyright file="RestException.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans.RestClient
{
    public class RestException : Exception
    {
        public RestException(string message) : base(message) { }

        public RestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
