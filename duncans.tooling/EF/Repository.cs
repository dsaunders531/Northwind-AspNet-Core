// <copyright file="Repository.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace duncans.EF
{
    /// <summary>
    /// The base repository class. All repositories need  to inherit from this and implement IRepository T.
    /// Models must derive from DbModel.
    /// </summary>
    /// <typeparam name="TModel">The type of model you want to use (must derive from DbModel or HistoricDbModel).</typeparam>
    /// <typeparam name="TKey">The type for the key.</typeparam>
    public abstract class EFRepositoryBase<TModel, TKey> : IRepository<TModel, TKey>, IDisposable, IParallelable
    {
        public EFRepositoryBase(DbContext context)
        {
            if (!(typeof(TModel).BaseType == typeof(DbModel<TKey>) || typeof(TModel).BaseType.Name.Contains("HistoricDbModel") == true))
            {
                throw new ApplicationException("The model must derive from DbModel or HistoricDbModel");
            }

            this.Context = context;
        }

        public DbContext Context { get; set; } = null;

        public int DegreeOfParallelism
        {
            get
            {
                // return cpu count - 1 or 1
                return Math.Max(System.Environment.ProcessorCount - 1, 1);
            }
        }

        public abstract IQueryable<TModel> FetchAll { get; }

        /// <summary>
        /// Gets data using an alternative way of getting data (ie: without the delete property set.
        /// </summary>
        public abstract IQueryable<TModel> FetchRaw { get; }

        public Func<IDbModel<TKey>, bool> RecordSelector(TKey rowId)
        {
            return new Func<IDbModel<TKey>, bool>(r => r.RowId.Equals(rowId));
        }

        public virtual int Commit()
        {
            if (this.Context.ChangeTracker.HasChanges())
            {
                return this.Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public virtual async Task<int> CommitAsync()
        {
            if (this.Context.ChangeTracker.HasChanges())
            {
                return await this.Context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Check to see if an item is being tracked.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public EntityState EntityIsTracked(IDbModel<TKey> item)
        {
            EntityState result = EntityState.Detached; // not tracked (to cover not added yet etc).

            // Get a list of already tracked entities which match the type.
            IEnumerable<EntityEntry> trackedEntities = this.Context.ChangeTracker.Entries().Where(e => e.Entity.GetType() == typeof(TModel));

            // See if item is already in list.
            EntityEntry foundTracked = trackedEntities.Where(te => ((IDbModel<TKey>)te.Entity).RowId.Equals(item.RowId)).FirstOrDefault();

            if (foundTracked != default(EntityEntry))
            {
                result = foundTracked.State;
            }

            return result;
        }

        /// <summary>
        /// Get a tracked entity.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public TModel FetchTrackedEntity(IDbModel<TKey> item)
        {
            TModel result = default(TModel);

            if (this.EntityIsTracked(item) != EntityState.Detached)
            {
                EntityEntry foundEntry = this.Context.ChangeTracker.Entries().Where(e => e.Entity.GetType() == typeof(TModel)
                                                                    && ((IDbModel<TKey>)e.Entity).RowId.Equals(item.RowId)).FirstOrDefault();

                if (foundEntry != default(EntityEntry))
                {
                    result = (TModel)foundEntry.Entity;
                }
            }

            return result;
        }

        public abstract void Create(TModel item);

        public abstract void Update(TModel item);

        public abstract void Delete(TModel item);

        public abstract TModel Fetch(TKey id);

        public virtual void Dispose()
        {
            // Does nothing - only need this for a using statement
        }

        /// <summary>
        /// Ignore the item (it will not be saved if there are any changes).
        /// </summary>
        /// <param name="item"></param>
        public abstract void Ignore(TModel item);

        /// <summary>
        /// Ingore the items relations (linked data and lists) to prevent problems with updates.
        /// </summary>
        /// <param name="item"></param>
        protected abstract void IgnoreRelations(TModel item);
    }
}
