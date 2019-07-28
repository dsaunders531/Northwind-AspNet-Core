// <copyright file="IDeleteable.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.EF
{
    /// <summary>
    /// Interface for items which must support a Deleted column.
    /// </summary>
    public interface IDeleteable
    {
        bool Deleted { get; set; }
    }
}
