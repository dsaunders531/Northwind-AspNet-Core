// <copyright file="IParallelable.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.EF
{
    public interface IParallelable
    {
        int DegreeOfParallelism { get; }
    }
}
