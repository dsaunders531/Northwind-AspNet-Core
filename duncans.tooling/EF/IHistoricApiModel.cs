// <copyright file="IHistoricApiModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.EF
{
    /// <summary>
    /// Interface for api models which are based on historic records
    /// </summary>
    /// <typeparam name="TKey">The type of key used by the model we want to store history records.</typeparam>
    public interface IHistoricApiModel<TKey> : IApiModel<long>, IHistoricDbModel<TKey>
    {
    }
}
