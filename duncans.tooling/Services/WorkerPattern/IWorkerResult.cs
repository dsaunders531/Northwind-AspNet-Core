// <copyright file="IWorkerResult.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans.WorkerPattern
{
    /// <summary>
    /// Interface for the WorkerResult object.
    /// </summary>
    public interface IWorkerResult
    {
        Exception Exception { get; set; }

        string Message { get; set; }

        bool Success { get; }
    }
}