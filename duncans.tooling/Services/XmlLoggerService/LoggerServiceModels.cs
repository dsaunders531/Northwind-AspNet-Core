// <copyright file="LoggerServiceModels.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans.Services.Logging
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
