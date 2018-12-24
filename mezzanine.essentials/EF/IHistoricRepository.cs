using System;
using System.Linq;

namespace mezzanine.EF
{
    /// <summary>
    /// The historic repository interface.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="THistoricModel"></typeparam>
    public interface IHistoricRepository<TModel, TKey, THistoricModel> : IRepository<TModel, TKey>
    {
        IQueryable<THistoricModel> FetchHistory(TKey modelId);

        THistoricModel FetchHistoric(TKey modelId, DateTime onDate);        
    }
}
