// <copyright file="IHistoricDbModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans.EF
{
    /// <summary>
    /// The IHistoricDbModel interface.
    /// </summary>
    /// <typeparam name="TKey">The type for the key.</typeparam>
    public interface IHistoricDbModel<TKey> : IDbModel<long>
    {
        TKey ModelId { get; set; }

        EFActionType Action { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
