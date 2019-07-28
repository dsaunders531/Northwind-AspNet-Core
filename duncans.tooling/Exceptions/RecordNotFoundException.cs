// <copyright file="RecordNotFoundException.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace duncans
{
    /// <summary>
    /// An exception to throw when a record has not been found.
    /// </summary>
    [Serializable]
    public class RecordNotFoundException : Exception
    {
        private string value1;
        private string value2;

        public RecordNotFoundException()
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }

        public RecordNotFoundException(string value1, string value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecordNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}