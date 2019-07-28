// <copyright file="IDbModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.EF
{
    /// <summary>
    /// The IDbModel interface.
    /// </summary>
    /// <typeparam name="TKey">The type of key being used.</typeparam>
    public interface IDbModel<TKey> : IApiModel<TKey>, IDeleteable
    {
        byte[] ConcurrencyToken { get; set; }

        long? RowVersion { get; set; }
    }
}
