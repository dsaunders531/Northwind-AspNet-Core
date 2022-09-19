using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace tools.EF
{
    /// <summary>
    /// The base repository class. All repositories need  to inherit from this and implement IRepository T 
    /// </summary>
    public abstract class Repository<TModel, TKey> : IRepository<TModel, TKey>
    {
        public DbContext Context { get; set; } = null;

        public abstract IQueryable<TModel> FetchAll { get; }

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        public virtual void Save()
        {
            this.Context.SaveChanges();
        }

        public abstract void Create(TModel item);

        public abstract void Update(TModel item);

        public abstract void Delete(TModel item);

        public abstract TModel Fetch(TKey id);
    }
}
