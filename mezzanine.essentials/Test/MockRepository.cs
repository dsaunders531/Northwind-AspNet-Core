using mezzanine.EF;
using mezzanine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mezzanine.Test
{
    /// <summary>
    /// A mock implementation of a repository for testing.
    /// </summary>
    /// <typeparam name="TModel">The type must implement the IModel interface.</typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class MockRepository<TModel, TKey> : IRepository<TModel, TKey>, IDisposable
    {
        private List<TModel> Committed { get; set; }
        private List<TModel> Uncomitted { get; set; }

        /// <summary>
        /// Creates an instance of the MockRespository class
        /// </summary>
        /// <param name="searchTerm">The search term you want to use in the fetch method. Use input parameter 'id' in the query.</param>
        public MockRepository()
        {
            this.Committed = new List<TModel>();
            this.Uncomitted = new List<TModel>();
        }

        public Func<IDbModel<TKey>, bool> RecordSelector(TKey rowId)
        {
            return new Func<IDbModel<TKey>, bool>(r => r.RowId.Equals(rowId));
        }

        public IQueryable<TModel> FetchAll
        {
            get
            {
                return this.Committed.AsQueryable();
            }
        }

        public void NewRecordIncrementor(IDbModel<TKey> recordWithNoRowId)
        {
            TKey newRowId = (TKey)default(TKey); // the seeding value
            newRowId = (TKey)newRowId.Increment(newRowId.GetType());
            recordWithNoRowId.RowId = newRowId;

            IQueryable<IDbModel<TKey>> models = (IQueryable<IDbModel<TKey>>)this.FetchAll;

            if (models != null)
            {
                if (models.Count() > 0)
                {
                    TKey max = models.Max(r => r.RowId);
                    recordWithNoRowId.RowId = (TKey)max.Increment(max.GetType());
                }
            }
        }

        public void Commit()
        {
            foreach (TModel item in Uncomitted)
            {
                if (((IDbModel<TKey>)item).RowId.Equals(default(TKey)))
                {
                    this.NewRecordIncrementor((IDbModel<TKey>)item);
                }

                ((IDbModel<TKey>)item).ConcurrencyToken = DateTime.Now.ToLongDateString().ToBytes();
                ((IDbModel<TKey>)item).RowVersion = ((IDbModel<TKey>)item).RowVersion + 1;
                this.Committed.Add(item);
            }

            this.Uncomitted.Clear();
        }

        public void Create(TModel item)
        {
            this.Uncomitted.Add(item);
        }

        public void Delete(TModel item)
        {
            TModel found = this.FindInComitted(item);

            if (found != null)
            {
                this.Committed.Remove(found);
            }
            else
            {
                found = this.FindInUnComitted(item);

                if (found != null)
                {
                    this.Uncomitted.Remove(found);
                }
            }
        }

        public TModel Fetch(TKey id)
        {
            IQueryable<IDbModel<TKey>> items = (IQueryable<IDbModel<TKey>>)this.FetchAll;

            return (TModel)items.Where(RecordSelector(id)).First();
        }

        public void Update(TModel item)
        {
            TModel found = this.FindInComitted(item);

            if (found != null)
            {
                this.Committed.Remove(found);

                using (Transposition transposition = new Transposition())
                {
                    found = transposition.Transpose(item, found);
                }

                this.Create(found);
            }
            else
            {
                this.Create(item);
            }
        }    
        
        private TModel FindInComitted(TModel item)
        {
            IDbModel<TKey> searchTerm = (IDbModel<TKey>)item;

            return this.Fetch(searchTerm.RowId);
        }

        private TModel FindInUnComitted(TModel item)
        {
            IDbModel<TKey> searchTerm = (IDbModel<TKey>)item;

            IQueryable<IDbModel<TKey>> items = (IQueryable<IDbModel<TKey>>)this.Uncomitted.AsQueryable();

            return (TModel)items.Where(i => i.RowId.Equals(searchTerm.RowId)).First();
        }

        public virtual void Dispose()
        {
            this.Committed.Clear();
            this.Committed = null;
            this.Uncomitted.Clear();
            this.Uncomitted = null;
        }
    }
}
