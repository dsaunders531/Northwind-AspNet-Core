// <copyright file="LoggerException.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace duncans
{
    /// <summary>
    /// Exception specifically for the logger.
    /// </summary>
    public class LoggerException : Exception
    {
        public LoggerException(string message, Exception innerException) : base(message, innerException) { }

        public LoggerException(string message) : base(message) { }

        public LoggerException() : base(@"Something went wrong while logging.") { }
    }
}
