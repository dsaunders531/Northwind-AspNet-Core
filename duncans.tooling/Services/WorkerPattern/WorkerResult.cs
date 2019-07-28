// <copyright file="WorkerResult.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace duncans.WorkerPattern
{
    /// <summary>
    /// A result object to store values at the end of a process.
    /// </summary>
    public class WorkerResult : IWorkerResult
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

    /// <summary>
    /// Generic worker result type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WorkerResult<T> : WorkerResult
    {
        public WorkerResult() : base()
        {
            try
            {
                this.Value = Activator.CreateInstance<T>();
            }
            catch (Exception)
            {
                this.Value = default(T);
            }
        }

        public WorkerResult(T model) : base()
        {
            this.Value = model;
        }

        public T Value { get; set; }
    }
}
