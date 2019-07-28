// <copyright file="IGenericService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace duncans.WorkerPattern
{
    /// <summary>
    /// The interface for the generic worker.
    /// </summary>
    /// <typeparam name="TDbModel"></typeparam>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>
    public interface IGenericService<TDbModel, TApiRowModel, TDbModelKey>
    {
        List<TApiRowModel> FetchAll();

        TApiRowModel Fetch(TDbModelKey key);

        TApiRowModel Fetch(TApiRowModel apiModel, Func<TDbModel, bool> fetchWithoutKey);

        TApiRowModel Create(TApiRowModel apiModel);

        TApiRowModel Update(TApiRowModel apiModel);

        void Delete(TDbModelKey key);

        int Commit();
    }
}
