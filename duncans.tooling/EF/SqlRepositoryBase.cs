// <copyright file="SqlRepositoryBase.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.DbClient;
using duncans.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace duncans.shared.SQLRepository
{
    public abstract class SqlRepositoryBase<TDbModel, TKey> : IRepository<TDbModel, TKey>, IDisposable
    {
        /// <summary>
        /// Gets the select statement to get the data.
        /// </summary>
        public abstract SqlCommand SelectAllSqlCommand { get; }

        public IQueryable<TDbModel> FetchAll
        {
            get
            {
                this.WaitForNotBusy();

                List<TDbModel> all = new List<TDbModel>();

                try
                {
                    this.BusyState = true;

                    all = this.DbClient.Fill<List<TDbModel>>(this.SelectAllSqlCommand);

                    if (all == null)
                    {
                        all = new List<TDbModel>();
                    }
                }
                catch (Exception e)
                {
                    throw new DbClientException("The query did not run. See the inner exception for more details.", e);
                }
                finally
                {
                    this.BusyState = false;
                }

                return all.AsQueryable();
            }
        }

        public IQueryable<TDbModel> FetchRaw => throw new NotImplementedException("Use FetchAll() instead.");

        public int DegreeOfParallelism => 1;

        public virtual IDbClient DbClient { get; set; }

        private bool BusyState { get; set; } = false;

        public abstract SqlCommand FetchOneSqlCommand(TKey id);

        public abstract SqlCommand InsertSqlCommand(TDbModel item);

        public abstract SqlCommand PostInsertFetchCommand(TDbModel item);

        public abstract SqlCommand UpdateSqlCommand(TDbModel item);

        public int Commit()
        {
            // Does nothing all items are commited at the point changes are made.
            return 0;
        }

        public Task<int> CommitAsync()
        {
            return new Task<int>(() => this.Commit());
        }

        public void Create(TDbModel item)
        {
            this.WaitForNotBusy();

            try
            {
                this.BusyState = true;

                ((IDbModel<TKey>)item).RowId = default(TKey);

                this.DbClient.RunUpsert(this.InsertSqlCommand(item));

                // Update the data with row version etc.
                item = this.DbClient.Fill<TDbModel>(this.PostInsertFetchCommand(item));
            }
            catch (Exception e)
            {
                throw new DbClientException("The model could not be created. See the inner exception for more details.", e);
            }
            finally
            {
                this.BusyState = false;
            }
        }

        public void Delete(TDbModel item)
        {
            ((IDbModel<TKey>)item).Deleted = true;
            this.Update(item);
        }

        public void Dispose()
        {
            this.DbClient.Dispose();
            this.DbClient = null;
        }

        public TDbModel Fetch(TKey id)
        {
            this.WaitForNotBusy();

            TDbModel result = this.DbClient.Fill<TDbModel>(this.FetchOneSqlCommand(id));

            return result;
        }

        public void Ignore(TDbModel item)
        {
            // Does nothing.
        }

        public void Update(TDbModel item)
        {
            this.WaitForNotBusy();

            try
            {
                this.BusyState = true;

                this.DbClient.RunUpsert(this.UpdateSqlCommand(item));

                item = this.FetchWithoutWait(((IDbModel<TKey>)item).RowId);
            }
            catch (Exception e)
            {
                throw new DbClientException("The model could not be updated. See the inner exception for more details.", e);
            }
            finally
            {
                this.BusyState = false;
            }
        }

        private void WaitForNotBusy()
        {
            int counter = 0;

            while (this.BusyState)
            {
                System.Threading.Thread.Sleep(500);
                counter++;

                if (counter > 40)
                {
                    // After 20 seconds...
                    throw new TimeoutException("Timeout waiting for busy state to change.");
                }
            }
        }

        private TDbModel FetchWithoutWait(TKey id)
        {
            TDbModel result = this.DbClient.Fill<TDbModel>(this.FetchOneSqlCommand(id));

            return result;
        }
    }
}
