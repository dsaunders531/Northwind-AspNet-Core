// <copyright file="HistoricRecordsService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace duncans.EF
{
    /// <summary>
    /// Base repository class where historic records are required.
    /// The model must derive from DbModel, the historic model must derive from DbHistoricModel.
    /// </summary>
    /// <typeparam name="TModel">The type of model we want to keep history records for.</typeparam>
    /// <typeparam name="TKey">The type of key used by the history model.</typeparam>
    /// <typeparam name="THistoricModel">The model used to store the history records.</typeparam>
    public abstract class HistoricRecordsService<TModel, TKey, THistoricModel> : IHistoryService<TModel, TKey, THistoricModel>, IDisposable
    {
        public HistoricRecordsService(IRepository<TModel, TKey> currentRecordRepository, IRepository<THistoricModel, long> historicRecordRepository)
        {
            if (! (typeof(TModel).BaseType == typeof(DbModel<TKey>)))
            {
                throw new ApplicationException("The model must derive from DbModel.");
            }

            if ( typeof(THistoricModel).BaseType.Name.Contains("HistoricDbModel") == false)
            {
                throw new ApplicationException("The model must derive from HistoricDbModel.");
            }

            this.HistoricRecordRepository = historicRecordRepository;
            this.CurrentRecordRepository = currentRecordRepository;
        }

        public IQueryable<TModel> FetchAll
        {
            get
            {
                return this.CurrentRecordRepository.FetchAll.Where(r => ((IDbModel<TKey>)r).Deleted == false);
            }
        }

        public IQueryable<TModel> FetchRaw
        {
            get
            {
                return this.CurrentRecordRepository.FetchAll;
            }
        }

        public int DegreeOfParallelism => this.CurrentRecordRepository.DegreeOfParallelism;

        private IRepository<TModel, TKey> CurrentRecordRepository { get; set; }

        private IRepository<THistoricModel, long> HistoricRecordRepository { get; set; }

        public int Commit()
        {
            int result = this.CurrentRecordRepository.Commit();
            this.HistoricRecordRepository.Commit();
            return result;
        }

        public async Task<int> CommitAsync()
        {
            int result = await this.CurrentRecordRepository.CommitAsync();
            await this.HistoricRecordRepository.CommitAsync();
            return result;
        }

        public void Create(TModel item)
        {
            TKey newRowId = default(TKey); // Create an assumed next row Id

            this.CurrentRecordRepository.Create(item);

            // I have to commit the data now so I get a new record Id otherwise there will be many problems creating the history record.
            this.CurrentRecordRepository.Commit();

            try
            {
                newRowId = ((IDbModel<TKey>)item).RowId;
                this.CreateHistory(item, newRowId, EFActionType.Create, DateTime.UtcNow, null);
            }
            catch (Exception ex)
            {
                // Its gone wrong creating the history record.
                // It has not been saved so for consistancy, the main record must be deleted.
                this.CurrentRecordRepository.Delete(item);
                this.CurrentRecordRepository.Commit();
                throw new ApplicationException("The history record could not be created. The records have not been saved. See inner exception for details.", ex);
            }
        }

        public void Delete(TModel item)
        {
            IDbModel<TKey> deletedItem = (IDbModel<TKey>)item;
            deletedItem.Deleted = true;
            this.CurrentRecordRepository.Update((TModel)deletedItem);
            this.CurrentRecordRepository.Commit();

            this.CreateHistory(item, ((IDbModel<TKey>)item).RowId, EFActionType.Delete, DateTime.UtcNow, null);
        }

        public TModel Fetch(TKey id)
        {
            IDbModel<TKey> result = (IDbModel<TKey>)this.CurrentRecordRepository.Fetch(id);

            if (result != null)
            {
                if (result.Deleted == true)
                {
                    return default(TModel);
                }
            }

            return (TModel)result;
        }

        public THistoricModel FetchHistoric(TKey modelId, DateTime onDate)
        {
            THistoricModel result = default(THistoricModel);

            if (onDate.Kind != DateTimeKind.Utc)
            {
                onDate = new DateTime(onDate.Ticks, DateTimeKind.Utc);
            }

            IQueryable<THistoricModel> allItems = this.FetchHistory(modelId);

            if (allItems != null)
            {
                if (allItems.Count() > 0)
                {
                    result = allItems.Where(h => (((IHistoricDbModel<TKey>)h).StartDate ?? DateTime.UtcNow) <= onDate
                                        && (((IHistoricDbModel<TKey>)h).EndDate ?? DateTime.UtcNow) >= onDate).FirstOrDefault();
                }
            }

            return result;
        }

        public IQueryable<THistoricModel> FetchHistory(TKey modelId)
        {
            return this.HistoricRecordRepository.FetchAll.Where(h => ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId));
        }

        public void Update(TModel item)
        {
            this.CurrentRecordRepository.Update(item);
            this.CurrentRecordRepository.Commit();

            this.CreateHistory(item, ((IDbModel<TKey>)item).RowId, EFActionType.Update, DateTime.UtcNow, null);
        }

        public virtual void Dispose()
        {
            // does nothing - needed for using statements
        }

        public virtual THistoricModel FetchHistoric(Func<THistoricModel, bool> recordSelector, DateTime onDate)
        {
            if (onDate.Kind != DateTimeKind.Utc)
            {
                onDate = new DateTime(onDate.Ticks, DateTimeKind.Utc);
            }

            return this.HistoricRecordRepository.FetchAll
                        .Where(recordSelector)
                        .Where(DateTrackSelector(onDate))
                        .OrderBy(DateTrackSorter()).FirstOrDefault();
        }

        public IQueryable<THistoricModel> FetchHistory(Func<THistoricModel, bool> recordSelector)
        {
            return this.HistoricRecordRepository.FetchAll.Where(recordSelector).AsQueryable();
        }

        public void CreateHistory(TModel model, TKey rowId, EFActionType action, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue)
            {
                if (startDate.Value.Kind != DateTimeKind.Utc)
                {
                    startDate = startDate.HasValue ? new DateTime?(new DateTime(startDate.Value.Ticks, DateTimeKind.Utc)) : null;
                }
            }

            if (endDate.HasValue)
            {
                if (endDate.Value.Kind != DateTimeKind.Utc)
                {
                    endDate = endDate.HasValue ? new DateTime?(new DateTime(endDate.Value.Ticks, DateTimeKind.Utc)) : null;
                }
            }

            // 1 - find current history record if any and end-date it (startDate - 1)
            IHistoricDbModel<TKey> currentRecord = (IHistoricDbModel<TKey>)this.FetchHistoric(((IDbModel<TKey>)model).RowId, startDate == null ? DateTime.UtcNow : startDate.Value);

            if (currentRecord != null)
            {
                currentRecord.EndDate = (startDate == null ? DateTime.UtcNow : startDate.Value).AddTicks(-1);

                this.HistoricRecordRepository.Update((THistoricModel)currentRecord);
            }
            else
            {
                action = EFActionType.Create; // when a current record cannot be found, the action must be create as we are creating the first historic record.
            }

            // 2 - create new history record
            THistoricModel historic = Activator.CreateInstance<THistoricModel>();

            using (Transposition transposition = new Transposition())
            {
                historic = transposition.Transpose(model, historic);
            }

            IHistoricDbModel<TKey> commonItems = (IHistoricDbModel<TKey>)historic;

            commonItems.RowId = default(long);
            commonItems.ModelId = rowId;
            commonItems.Action = action;
            commonItems.StartDate = startDate == null ? DateTime.UtcNow : startDate.Value;
            commonItems.EndDate = endDate == null ? DateTimeExtensions.DefaultHighDate() : endDate.Value;

            this.HistoricRecordRepository.Create((THistoricModel)commonItems);
            this.HistoricRecordRepository.Commit();
        }

        public void Ignore(TModel item)
        {
            throw new NotImplementedException();
        }

        protected Func<THistoricModel, bool> DateTrackSelector(DateTime onDate)
        {
            if (onDate.Kind != DateTimeKind.Utc)
            {
                onDate = new DateTime(onDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(o =>
                                                (((IHistoricDbModel<TKey>)o).StartDate == null ? DateTime.UtcNow : ((IHistoricDbModel<TKey>)o).StartDate.Value) <= onDate
                                                && (((IHistoricDbModel<TKey>)o).EndDate == null ? DateTime.UtcNow : ((IHistoricDbModel<TKey>)o).EndDate.Value) >= onDate);
        }

        protected Func<THistoricModel, DateTime?> DateTrackSorter()
        {
            return new Func<THistoricModel, DateTime?>(o => ((IHistoricDbModel<TKey>)o).StartDate);
        }
    }
}
