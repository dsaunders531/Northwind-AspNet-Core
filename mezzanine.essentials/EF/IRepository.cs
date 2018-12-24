using System;
using System.Linq;

namespace mezzanine.EF
{
    /// <summary>
    /// The respository interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<TModel, TKey> : IDisposable
    {
        void Commit();
        void Create(TModel item);
        void Update(TModel item);
        void Delete(TModel item);
        IQueryable<TModel> FetchAll { get; }
        TModel Fetch(TKey id);
    }
}
