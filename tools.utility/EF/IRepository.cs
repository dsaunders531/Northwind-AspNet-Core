using System.Linq;

namespace tools.EF
{
    /// <summary>
    /// The respository interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<TModel, TKey>
    {
        void Save();
        void Create(TModel item);
        void Update(TModel item);
        void Delete(TModel item);
        IQueryable<TModel> FetchAll { get; }
        TModel Fetch(TKey id);
    }
}
