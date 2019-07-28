// <copyright file="MemoryRepository.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.EF;
using duncans.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace duncans.Test
{
    /// <summary>
    /// A mock implementation of a repository for testing.
    /// </summary>
    /// <typeparam name="TModel">The type must implement the IModel interface.</typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class MemoryRepository<TModel, TKey> : IRepository<TModel, TKey>, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRepository{TModel, TKey}"/> class.
        /// The mock repository for test data.
        /// </summary>
        public MemoryRepository()
        {
            this.Committed = new List<TModel>();
            this.Uncomitted = new List<TModel>();
        }

        public IQueryable<TModel> FetchAll
        {
            get
            {
                return this.Committed.AsQueryable();
            }
        }

        public IQueryable<TModel> FetchRaw => this.Committed.AsQueryable();

        public int DegreeOfParallelism => 1;

        private List<TModel> Committed { get; set; }

        private List<TModel> Uncomitted { get; set; }

        private bool Busy { get; set; }

        public Func<IDbModel<TKey>, bool> RecordSelector(TKey rowId)
        {
            return new Func<IDbModel<TKey>, bool>(r => r.RowId.Equals(rowId));
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

        public async Task<int> CommitAsync()
        {
            return await new Task<int>(() => this.Commit());
        }

        public int Commit()
        {
            int result = this.Uncomitted.Count();

            this.WaitForNotBusy();

            try
            {
                this.Busy = true;

                int counter = result;

                // loop backwards through list.
                for (int i = result - 1; i >= 0; i--)
                {
                    TModel item = this.Uncomitted[i];

                    if (item != null)
                    {
                        if (((IDbModel<TKey>)item).RowId.ToString() == "0")
                        {
                            this.NewRecordIncrementor((IDbModel<TKey>)item);
                        }
                        else if (((IDbModel<TKey>)item).RowId.Equals(default(TKey)))
                        {
                            this.NewRecordIncrementor((IDbModel<TKey>)item);
                        }

                        if (this.Committed.Where(c => ((IDbModel<TKey>)c).RowId.Equals(((IDbModel<TKey>)item).RowId)).Count() > 0)
                        {
                            // There already is an item with same row id in committed.
                            this.NewRecordIncrementor((IDbModel<TKey>)item);
                        }

                        ((IDbModel<TKey>)item).ConcurrencyToken = DateTime.UtcNow.ToLongDateString().ToBytes();
                        ((IDbModel<TKey>)item).RowVersion = (((IDbModel<TKey>)item).RowVersion ?? 0) + 1;
                        this.Committed.Add(item);
                    }
                }

                // Put the items back into order
                this.Committed = this.Committed.OrderBy(s => ((IDbModel<TKey>)s).RowId).ToList();

                ////foreach (TModel item in this.Uncomitted)
                ////{
                ////    if (item != null)
                ////    {
                ////        if (((IDbModel<TKey>)item).RowId.ToString() == "0")
                ////        {
                ////            this.NewRecordIncrementor((IDbModel<TKey>)item);
                ////        }
                ////        else if (((IDbModel<TKey>)item).RowId.Equals(default(TKey)))
                ////        {
                ////            this.NewRecordIncrementor((IDbModel<TKey>)item);
                ////        }

                ////        ((IDbModel<TKey>)item).ConcurrencyToken = DateTime.UtcNow.ToLongDateString().ToBytes();
                ////        ((IDbModel<TKey>)item).RowVersion = (((IDbModel<TKey>)item).RowVersion ?? 0) + 1;
                ////        this.Committed.Add(item);
                ////    }
                ////}

                this.Uncomitted.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Busy = false;
            }

            return result;
        }

        public void Create(TModel item)
        {
            if (item != null)
            {
                this.WaitForNotBusy();
                this.Busy = true;

                if (this.Uncomitted.Contains(item) == true)
                {
                    this.Uncomitted.Remove(item);
                }

                this.Uncomitted.Add(item);

                this.Busy = false;
            }
        }

        public void Delete(TModel item)
        {
            if (item != null)
            {
                this.WaitForNotBusy();
                this.Busy = true;

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

                this.Busy = false;
            }
        }

        public TModel Fetch(TKey id)
        {
            IQueryable<IDbModel<TKey>> items = (IQueryable<IDbModel<TKey>>)this.FetchAll;
            return (TModel)items.Where(RecordSelector(id)).FirstOrDefault();
        }

        public void Update(TModel item)
        {
            if (item != null)
            {
                this.WaitForNotBusy();
                this.Busy = true;

                TModel found = this.FindInComitted(item);

                if (found != null)
                {
                    this.Committed.Remove(found);

                    using (Transposition transposition = new Transposition())
                    {
                        found = transposition.Transpose(item, found);
                    }

                    this.Busy = false;
                    this.Create(found);
                }
                else
                {
                    this.Busy = false;
                    this.Create(item);
                }
            }
        }

        public void Ignore(TModel item)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            this.Committed.Clear();
            this.Committed = null;
            this.Uncomitted.Clear();
            this.Uncomitted = null;
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

            return (TModel)items.Where(i => i.RowId.Equals(searchTerm.RowId)).FirstOrDefault();
        }

        private void WaitForNotBusy()
        {
            if (Busy)
            {
                int counter = 0;

                while (this.Busy)
                {
                    // timeout after 30 seconds (4 * 30)
                    if (counter > 120)
                    {
                        throw new TimeoutException("It took too long to wait for the memory repository to become available.");
                    }

                    System.Threading.Thread.Sleep(250);
                    counter++;
                }
            }
        }
    }
}
