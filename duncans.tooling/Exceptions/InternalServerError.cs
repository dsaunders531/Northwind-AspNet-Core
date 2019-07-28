// <copyright file="InternalServerError.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.RestClient
{
    public class InternalServerError
    {
        public string ExceptionType { get; set; }

        public string Message { get; set; }

        public string Trace { get; set; }
    }
}
