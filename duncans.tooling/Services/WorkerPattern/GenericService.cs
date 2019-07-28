// <copyright file="GenericService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.EF;
using duncans.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace duncans.WorkerPattern
{
    /// <summary>
    /// The generic worker base.
    /// </summary>
    /// <typeparam name="TDbModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TFetchSortFieldType"></typeparam>
    public abstract class GenericService<TDbModel, TDbModelKey, TApiRowModel, TFetchSortFieldType> : IGenericService<TDbModel, TApiRowModel, TDbModelKey>
    {
        public GenericService(IRepository<TDbModel, TDbModelKey> repository)
        {
            this.Repository = repository;
        }

        private IRepository<TDbModel, TDbModelKey> Repository { get; set; }

        /// <summary>
        /// Fetches all the items.
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public List<TApiRowModel> FetchAll(Func<TDbModel, TFetchSortFieldType> sortOrder)
        {
            List<TDbModel> table = this.Repository.FetchAll.OrderBy(sortOrder).ToList();
            List<TApiRowModel> result = new List<TApiRowModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (TDbModel row in table)
                {
                    result.Add(transposition.Transpose(row, Activator.CreateInstance<TApiRowModel>()));
                }
            }

            return result;
        }

        /// <summary>
        /// Override this function with an appropriate where clause in the inherited class.
        /// </summary>
        /// <returns></returns>
        public abstract List<TApiRowModel> FetchAll();

        /// <summary>
        /// Fetches 1 item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TApiRowModel Fetch(TDbModelKey key)
        {
            TDbModel row = this.Repository.Fetch(key);

            if (row == null)
            {
                throw new RecordNotFoundException("Could not find a item with id '{0}'", key.ToString());
            }
            else
            {
                TApiRowModel result = Activator.CreateInstance<TApiRowModel>();

                using (Transposition tranposition = new Transposition())
                {
                    result = tranposition.Transpose(row, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Fetches 1 item without using its key.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <param name="fetchSelector"></param>
        /// <returns></returns>
        public TApiRowModel Fetch(TApiRowModel apiRowModel, Func<TDbModel, bool> fetchSelector)
        {
            TDbModel row = this.Repository.FetchAll.Where(fetchSelector).FirstOrDefault();

            if (row == null)
            {
                throw new RecordNotFoundException("Could not find a matching item.");
            }
            else
            {
                TApiRowModel result = Activator.CreateInstance<TApiRowModel>();

                using (Transposition transposition = new Transposition())
                {
                    result = transposition.Transpose(row, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Adds a new record.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <param name="existingRecordSelector">Selector for existing record.</param>
        /// <returns>The key for the new record.</returns>
        /// <remarks>The existing record selector must select the record without using the key (it will be 0 in a new record).</remarks>
        public TApiRowModel Create(TApiRowModel apiRowModel, Func<TDbModel, bool> existingRecordSelector)
        {
            TApiRowModel result = default(TApiRowModel);

            if (apiRowModel == null)
            {
                throw new ArgumentNullException("The input parameter cannot be null");
            }
            else
            {
                // check the item does not already exist
                TDbModel dbModel = this.Repository.FetchAll.Where(existingRecordSelector).FirstOrDefault();

                if (dbModel != null)
                {
                    // the item already exists
                    throw new RecordFoundException("This record already exists!");
                }
                else
                {
                    dbModel = Activator.CreateInstance<TDbModel>();

                    // map the item and add it
                    using (Transposition transposition = new Transposition())
                    {
                        dbModel = transposition.Transpose(apiRowModel, dbModel);

                        this.Repository.Create(dbModel);
                        this.Repository.Commit();

                        result = transposition.Transpose(dbModel, apiRowModel);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Override this function with an appropriate record selector in the inherited class.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <returns></returns>
        public abstract TApiRowModel Create(TApiRowModel apiRowModel);

        /// <summary>
        /// Update a record in the database.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <param name="existingRecordSelector">Selector for existing records.</param>
        /// <returns></returns>
        public TApiRowModel Update(TApiRowModel apiRowModel, Func<TDbModel, bool> existingRecordSelector)
        {
            TApiRowModel result = default(TApiRowModel);

            if (apiRowModel == null)
            {
                throw new ArgumentNullException("The input parameter cannot be null");
            }
            else
            {
                // find the item
                TDbModel dbModel = this.Repository.FetchAll.Where(existingRecordSelector).FirstOrDefault();

                if (dbModel == null)
                {
                    // item was not found!
                    throw new RecordNotFoundException("Could not find a item to update.");
                }
                else
                {
                    // map the apiO to the db model.
                    using (Transposition transposition = new Transposition())
                    {
                        dbModel = transposition.Transpose(apiRowModel, dbModel);

                        // update the item
                        this.Repository.Update(dbModel);
                        this.Repository.Commit();

                        result = transposition.Transpose(dbModel, apiRowModel);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Override this function with an appropriate record selector in the inherited class.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <returns></returns>
        public abstract TApiRowModel Update(TApiRowModel apiRowModel);

        /// <summary>
        /// Remove a record from the database.
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TDbModelKey key)
        {
            TDbModel dbModel = this.Repository.Fetch(key);

            if (dbModel == null)
            {
                // record not found
                throw new RecordNotFoundException("Could not find a item with id '{0}' to delete.", key.ToString());
            }
            else
            {
                this.Repository.Delete(dbModel);
                this.Repository.Commit();
            }
        }

        public int Commit()
        {
            return this.Repository.Commit();
        }
    }
}
