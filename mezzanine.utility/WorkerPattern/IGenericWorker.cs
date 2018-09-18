using System.Collections.Generic;

namespace mezzanine.WorkerPattern
{
    /// <summary>
    /// The interface for the generic worker
    /// </summary>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>
    public interface IGenericWorker<TApiRowModel, TDbModelKey>
    {
        List<TApiRowModel> FetchAll();
        TApiRowModel Fetch(TDbModelKey key);
        TApiRowModel Create(TApiRowModel apiModel);
        TApiRowModel Update(TApiRowModel apiModel);
        void Delete(TDbModelKey key);
        void Commit();
    }
}
