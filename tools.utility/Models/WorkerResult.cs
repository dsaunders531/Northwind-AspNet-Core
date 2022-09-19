using System;

namespace tools.Models
{
    /// <summary>
    /// A result object to store values at the end of a process.
    /// </summary>
    public class WorkerResult
    {
        public WorkerResult()
        {
            this.Success = true;
            this.Message = string.Empty;
            this.Exception = null;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
