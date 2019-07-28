// <copyright file="IPointInTimeService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Linq;

namespace mezzanine.EF
{
    /// <summary>
    /// Interface for the point in time repository service.
    /// </summary>
    /// <typeparam name="THistoricModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IPointInTimeService<THistoricModel, TKey>
    {
        THistoricModel Fetch(TKey modelId, DateTime dateTime);

        IQueryable<THistoricModel> FetchAll(DateTime dateTime);

        THistoricModel MakeHistory(IHistoricDbModel<TKey> model);

        IQueryable<THistoricModel> FetchRaw();

        void Commit();
    }
}
