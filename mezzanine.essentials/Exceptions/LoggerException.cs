using System;
using System.Collections.Generic;
using System.Text;

namespace mezzanine
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
