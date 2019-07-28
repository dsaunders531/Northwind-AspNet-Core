// <copyright file="RecordLockedException.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace duncans
{
    /// <summary>
    /// An exception to throw when a record has been locked.
    /// </summary>
    [Serializable]
    public class RecordLockedException : Exception
    {
        public RecordLockedException()
        {
        }

        public RecordLockedException(string message) : base(message)
        {
        }

        public RecordLockedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecordLockedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
