// <copyright file="EventArgs.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.WorkerPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace duncans.tooling
{
    /// <summary>
    /// Send additional data with an event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs, IWorkerResult
    {
        public EventArgs()
        {
            this.EventData = default(T);
        }

        public EventArgs(T eventData)
        {
            this.EventData = eventData;
        }

        public T EventData { get; set; }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public bool Success => this.Exception == null || this.Message.IsNullOrEmpty();
    }
}
