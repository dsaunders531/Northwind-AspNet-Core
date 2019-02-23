using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mezzanine.EF
{
    /// <summary>
    /// Service for managing data which changes over time.
    /// The same modelId is used.
    /// THistoricModel needs to implement IHistoricDbModel
    /// </summary>
    /// <typeparam name="THistoricDbModel">The type for the historic model. It must implement IHistoricDbModel.</typeparam>
    /// <typeparam name="TKey">The type of key (ModelId). Note - this may be different to the RowId which is always a long for historicDbModels.</typeparam>
    public abstract class PointInTimeService<THistoricDbModel, TKey> : IPointInTimeService<THistoricDbModel, TKey>
    {
        private IRepository<THistoricDbModel, TKey> Repository { get; set; }

        public PointInTimeService(IRepository<THistoricDbModel, TKey> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Fetch one record at a point in time.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public THistoricDbModel Fetch(TKey modelId, DateTime dateTime)
        {
            THistoricDbModel model = this.Repository.FetchAll
                                            .Where(h => ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId))
                                            .Where(this.PointInTimeSelector(dateTime)).First();

            return model;
        }

        /// <summary>
        /// Fetch all records at a point in time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public IQueryable<THistoricDbModel> FetchAll(DateTime dateTime)
        {
            return this.Repository.FetchAll.Where(this.PointInTimeSelector(dateTime)).AsQueryable();
        }

        /// <summary>
        /// Update, Create or Delete a record at a point in time defined by the models start and end dates.
        /// </summary>
        /// <param name="model"></param>
        public THistoricDbModel Update(THistoricDbModel model)
        {
            // Count the quantity of records

            // See which scenario needs solving

            // save.

            
        }

        private Func<THistoricDbModel, bool> PointInTimeSelector(DateTime dateTime)
        {
            return new Func<THistoricDbModel, bool>(p =>
                                                    ((IHistoricDbModel<TKey>)p).StartDate <= dateTime
                                                    && ((IHistoricDbModel<TKey>)p).EndDate >= dateTime);
        }
    }
}
