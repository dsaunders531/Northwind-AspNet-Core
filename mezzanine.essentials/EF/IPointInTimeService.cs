using System;
using System.Linq;

namespace mezzanine.EF
{
    public interface IPointInTimeService<THistoricDbModel, TKey>
    {
        THistoricDbModel Fetch(TKey modelId, DateTime dateTime);

        IQueryable<THistoricDbModel> FetchAll(DateTime dateTime);

        THistoricDbModel Update(THistoricDbModel model);
    }
}
