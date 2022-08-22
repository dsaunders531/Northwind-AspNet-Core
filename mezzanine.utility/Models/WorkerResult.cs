using System;

namespace mezzanine.Models
{
    /// <summary>
    /// A result object to store values at the end of a process.
    /// </summary>
    public class WorkerResult
    {
        public WorkerResult()
        {
            Success = true;
            Message = string.Empty;
            Exception = null;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
