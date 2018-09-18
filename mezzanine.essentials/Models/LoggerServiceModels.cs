using System;

namespace mezzanine.Models
{
    /// <summary>
    /// LogEntryObject. Used by the logger service.
    /// </summary>
    public sealed class LogEntryData
    {
        public string LogLevel { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string State { get; set; }
        public DateTime Timestamp { get; set; }
        public LogExceptionData Exception { get; set; }
    }

    /// <summary>
    /// The LogExceptionData Object. Used by the logger service.
    /// </summary>
    public sealed class LogExceptionData
    {
        public string ExceptionType { get; set; }
        public string HelpLink { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public LogExceptionData InnerException { get; set; }
    }
}
