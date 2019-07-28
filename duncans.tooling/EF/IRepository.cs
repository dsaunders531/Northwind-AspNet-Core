// <copyright file="IRepository.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;

namespace duncans.EF
{
    /// <summary>
    /// The respository interface.
    /// </summary>
    /// <typeparam name="TModel">The model you want to use.</typeparam>
    /// <typeparam name="TKey">The type for the key column.</typeparam>
    public interface IRepository<TModel, TKey> : IParallelable
    {
        IQueryable<TModel> FetchAll { get; }

        IQueryable<TModel> FetchRaw { get; }

        int Commit();

        void Create(TModel item);

        void Update(TModel item);

        void Delete(TModel item);

        void Ignore(TModel item);

        TModel Fetch(TKey id);

        Task<int> CommitAsync();
    }
}
