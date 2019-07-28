// <copyright file="IApiModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.EF
{
    /// <summary>
    /// Interface for api models which are simplified versions of IDbModel.
    /// </summary>
    /// <typeparam name="TKey">The type of key. Must be the same as the model.</typeparam>
    public interface IApiModel<TKey>
    {
        TKey RowId { get; set; }
    }
}
