// <copyright file="IHistoryService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Linq;

namespace duncans.EF
{
    /// <summary>
    /// The historic repository interface.
    /// </summary>
    /// <typeparam name="TModel">The model which is being date tracked.</typeparam>
    /// <typeparam name="TKey">The type of key for the model.</typeparam>
    /// <typeparam name="THistoricModel">The historic model which stores the history records.</typeparam>
    public interface IHistoryService<TModel, TKey, THistoricModel> : IRepository<TModel, TKey>
    {
        IQueryable<THistoricModel> FetchHistory(TKey modelId);

        /// <summary>
        /// Fetch 1 item when you know the model id.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="onDate"></param>
        /// <returns></returns>
        THistoricModel FetchHistoric(TKey modelId, DateTime onDate);

        /// <summary>
        /// Fetch 1 item when you don't know the model id or you want to use different criteria.
        /// </summary>
        /// <param name="recordSelector"></param>
        /// <param name="onDate"></param>
        /// <returns></returns>
        THistoricModel FetchHistoric(Func<THistoricModel, bool> recordSelector, DateTime onDate);

        /// <summary>
        /// Fetch all the historic items when you don't know the model id or you want to use different criteria.
        /// </summary>
        /// <param name="recordSelector"></param>
        /// <returns></returns>
        IQueryable<THistoricModel> FetchHistory(Func<THistoricModel, bool> recordSelector);
    }
}
