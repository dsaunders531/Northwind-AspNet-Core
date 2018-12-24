using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace mezzanine.EF
{
    /// <summary>
    /// The base repository class. All repositories need  to inherit from this and implement IRepository T.
    /// Models must derive from DbModel
    /// </summary>
    public abstract class Repository<TModel, TKey> : IRepository<TModel, TKey>, IDisposable
    {
        public Repository(DbContext context)
        {
            if ( ! (typeof(TModel).BaseType == typeof(DbModel<TKey>) || typeof(TModel).BaseType == typeof(HistoricDbModel<TKey>)))
            {
                throw new ApplicationException("The model must derive from DbModel or HistoricDbModel");
            }

            this.Context = context;
        }

        public DbContext Context { get; set; } = null;

        public abstract IQueryable<TModel> FetchAll { get; }

        public Func<IDbModel<TKey>, bool> RecordSelector(TKey rowId)
        {
            return new Func<IDbModel<TKey>, bool>(r => r.RowId.Equals(rowId));
        }

        public virtual void Commit()
        {
            this.Context.SaveChanges();            
        }

        public abstract void Create(TModel item);

        public abstract void Update(TModel item);

        public abstract void Delete(TModel item);

        public abstract TModel Fetch(TKey id);

        public virtual void Dispose()
        {
            this.Context.Dispose();
            this.Context = null;
        }
    }
}
