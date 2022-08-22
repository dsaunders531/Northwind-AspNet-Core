using System;
using System.Runtime.Serialization;

namespace tools.Exceptions
{
    /// <summary>
    /// An exception to throw when a record has been found.
    /// </summary>
    [Serializable]
    public class RecordFoundException : Exception
    {
        private readonly string value1;
        private readonly string value2;

        public RecordFoundException()
        {
        }

        public RecordFoundException(string message) : base(message)
        {
        }

        public RecordFoundException(string value1, string value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public RecordFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecordFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
